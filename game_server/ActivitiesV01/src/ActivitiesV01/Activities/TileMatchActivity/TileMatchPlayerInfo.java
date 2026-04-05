package ActivitiesV01.Activities.TileMatchActivity;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ActivitiesV01.Activities.TileMatchActivity.Game.TileMatchPlayerGameInfo;
import ActivitiesV01.Bo.TileMatchPlayerGameInfoBO;
import ActivitiesV01.Bo.TileMatchPlayerInfoBO;
import ActivitiesV01.Err.TileMatchErr;
import ActivitiesV01.Events.Event_P_TILE_MATCH_SCORE_CHG;
import ActivitiesV01.Refs.TileMatch.*;
import Common.MailObj.Mail_Data;
import Hotfix.V01.Common.TileMatchObj.TileMatch_CanDrawStepReward;
import Hotfix.V01.Common.TileMatchObj.TileMatch_Info;
import Hotfix.V01.Common.TileMatchObj.TileMatch_StepRewardInfo;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_GameEvent;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType;
import Hotfix.V01.GS2GC.p201_TileMatchOp.GS2GC_201_053_OnTileMatchStepRewardChg;
import Hotfix.V01.GS2GC.p201_TileMatchOp.GS2GC_201_054_OnTileMatchCanDrawStepRewardChg;
import Hotfix.V01.GS2GC.p201_TileMatchOp.GS2GC_201_055_OnTileMatchTotalScoreChg;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.Pair.WCGPairIntList;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class TileMatchPlayerInfo
{
    private TileMatchPlayerMgr _m_mgr;
    private TileMatchPlayerInfoBO _m_bo;
    private List<TileMatchPlayerGameInfo> _m_gameList;
    private WCGPairIntList _m_stepRewardList;
    private MutexAtom _m_mutex;
    private int _m_serial = 0;

    public TileMatchPlayerInfo(TileMatchPlayerMgr _mgr, TileMatchPlayerInfoBO _bo)
    {
        _m_mgr = _mgr;
        _m_bo = _bo;
        _m_gameList = new ArrayList<>();
        _m_stepRewardList = new WCGPairIntList();
        _m_mutex = new MutexAtom();

        _m_stepRewardList.parseFromString(_bo.getCanDrawStepRewardList());
    }

    public TileMatchPlayerMgr getMgr()
    {
        return _m_mgr;
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public long getCid()
    {
        return _m_bo.getCid();
    }


    public NPUserServer getUsServer()
    {
        return getMgr().getActivity().getUSServer();
    }

    /**
     * 初始化玩家信息
     * @param _bo
     */
    public void initGameFromDB(TileMatchPlayerGameInfoBO _bo)
    {
        RefTileMatchMode refTileMatchMode = RefTileMatchMode.getMgr().get(_bo.getModeType());
        if (refTileMatchMode == null)
        {
            USLog.error(getUsServer(), "TileMatchPlayerInfo.initGameFromDB, RefTileMatchMode not found for modeType:{}", _bo.getModeType());
            return;
        }

        _m_gameList.add(new TileMatchPlayerGameInfo(this, refTileMatchMode, _bo));
    }

    /**
     * 查找玩家游戏信息
     * @param _modeType
     * @return
     */
    public TileMatchPlayerGameInfo lookupGame(ETileMatch_ModeType _modeType)
    {
        _lock();
        try
        {
            for (TileMatchPlayerGameInfo gameInfo : _m_gameList)
            {
                if (gameInfo.getModeType() == _modeType.ordinal())
                    return gameInfo;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保玩家游戏信息存在，如果不存在则创建一个新的
     * @param _modeType
     * @return
     */
    public ResultOne<TileMatchPlayerGameInfo> ensureGame(NPUSUserData _userdata, ETileMatch_ModeType _modeType, NPPlayerContext _context)
    {
        TileMatchPlayerGameInfo gameInfo = lookupGame(_modeType);
        if (gameInfo != null)
            return ResultOne.succ(gameInfo);

        // 查询模式配置
        RefTileMatchMode refTileMatchMode = RefTileMatchMode.getMgr().get(_modeType.ordinal());
        if (refTileMatchMode == null)
            return ResultOne.failed(CommErr.REF_NOT_FOUND);

        // 检查玩家是否满足解锁条件
        boolean canUnlock = NPPlayerConditionDealerMgr.IsEnable(refTileMatchMode.unlock_condition, _userdata, null);
        if (!canUnlock)
            return ResultOne.failed(TileMatchErr.TILE_MATCH_MODE_UNLOCK_FAIL);

        // 检查分数是否达到要求
        if (_m_bo.getTotalScore() < refTileMatchMode.unlock_need_score)
            return ResultOne.failed(TileMatchErr.TILE_MATCH_MODE_UNLOCK_FAIL);

        BM bmObj = getUsServer().getBM();

        TileMatchPlayerGameInfoBO bo = new TileMatchPlayerGameInfoBO();
        bo.setCid(bmObj, getCid());
        bo.setModeType(bmObj, _modeType.ordinal());
        bo.setActivityInstanceId(bmObj, getMgr().getActivity().getInstanceId());

        //随机任务配置
        Long taskId = refTileMatchMode.task_rand_list.random();
        if (taskId != null)
        {
            RefTileMatchTask refTileMatchTask = RefTileMatchTask.getMgr().get(taskId);
            if (refTileMatchTask != null)
            {
                bo.setTaskId(bmObj, taskId);
                //随机排序
                List<Integer> blockList = new ArrayList<>(RefTileMatchBlock.getMgr().getNormalBlockIdList());
                Collections.shuffle(blockList);
                //获取指定数量的方块
                WCGPairIntList wcgPairIntList = new WCGPairIntList();
                for (Integer blockId : blockList.subList(0, Math.min(refTileMatchTask.chess_pieces_num.size(), blockList.size())))
                {
                    wcgPairIntList.addPair(blockId, 0);
                }
                bo.setTaskBlockList(bmObj, wcgPairIntList.toString());
            }
        }
        bo.insert(bmObj);

        TileMatchPlayerGameInfo newGame = new TileMatchPlayerGameInfo(this, refTileMatchMode, bo);
        _m_gameList.add(newGame);

        NPPlayerContext context = NPPlayerContext.createNew(ETileMatch_GameEvent.TILE_MATCH_GAME_INIT.value());
        context.setGuid(_context.getGuid());

        // 初始化游戏信息
        newGame.resetMap(_userdata, context);

        return ResultOne.succ(newGame);
    }

    /**
     * 加分
     * @param _userdata
     * @param _score
     * @param _context
     */
    public void addScore(NPUSUserData _userdata, long _score, NPPlayerContext _context)
    {
        _lock();
        try
        {
            int curStep = _m_bo.getStepRewardStep();
            long curScore = _m_bo.getStepRewardScore() + _score;
            List<Integer> stepRewardList = new ArrayList<>();

            RefTileMatchStepReward refCurStep = RefTileMatchStepReward.getMgr().get(curStep);
            while (curScore >= refCurStep.goal)
            {
                //校验数据
                if (refCurStep.goal <= 0)
                {
                    USLog.error(getUsServer(),
                            "TileMatchPlayerInfo.addScore, refCurStep.goal is zero or negative, step: {}, goal: {}", curStep, refCurStep.goal);
                    break;
                }

                curScore -= refCurStep.goal;
                stepRewardList.add(curStep);

                _m_serial++;

                //如果有下一阶段，则进阶
                RefTileMatchStepReward refNextStep = RefTileMatchStepReward.getMgr().get(curStep + 1);
                if (refNextStep != null)
                {
                    curStep++;
                    refCurStep = refNextStep;
                }
            }

            //记录奖励
            for (Integer step : stepRewardList)
            {
                WCGPairInt pair = _m_stepRewardList.ensure(step);
                pair.setSecond(pair.second() + 1);
            }

            BM bmObj = getUsServer().getBM();
            _m_bo.setStepRewardStep(bmObj, curStep);
            _m_bo.setStepRewardScore(bmObj, curScore);
            _m_bo.setCanDrawStepRewardList(bmObj, _m_stepRewardList.toString());
            _m_bo.setTotalScore(bmObj, _m_bo.getTotalScore() + _score);
            _m_bo.saveAllMarked(bmObj);

            //如果奖励有变化，发送通知
            if (!stepRewardList.isEmpty())
            {
                pushCanDrawStepRewardChg(_userdata);
            }

            _userdata.sendMsgToGC(new GS2GC_201_053_OnTileMatchStepRewardChg(makeStepRewardInfo()));
            _userdata.sendMsgToGC(new GS2GC_201_055_OnTileMatchTotalScoreChg(_m_bo.getTotalScore()));
        } finally
        {
            _unlock();
        }

        //触发分数变更事件
        Event_P_TILE_MATCH_SCORE_CHG event = new Event_P_TILE_MATCH_SCORE_CHG(_context, _m_bo.getTotalScore());
        _userdata.onLogicEvent(event);
    }

    /**
     * 推送阶段奖励变化
     * @param _userdata
     */
    public void pushCanDrawStepRewardChg(NPUSUserData _userdata)
    {
        GS2GC_201_054_OnTileMatchCanDrawStepRewardChg proto = new GS2GC_201_054_OnTileMatchCanDrawStepRewardChg();
        for (WCGPairInt pair : _m_stepRewardList.getList())
        {
            TileMatch_CanDrawStepReward stepReward = new TileMatch_CanDrawStepReward();
            stepReward.setStep(pair.first());
            stepReward.setNum(pair.second());
            proto.addCanDrawStepRewardList(stepReward);
        }
        _userdata.sendMsgToGC(proto);
    }

    /**
     * 领取阶段奖励
     * @param _userData
     * @param _context
     * @return
     */
    public Result drawStepReward(NPUSUserData _userData, NPPlayerContext _context)
    {
        NPItemCostCollector_nosafe itemCollector;

        _lock();
        try
        {
            //如果没有可领取的阶段奖励
            if (_m_stepRewardList.getList().isEmpty())
                return TileMatchErr.TILE_MATCH_STEP_REWARD_EMPTY;

            itemCollector = new NPItemCostCollector_nosafe();

            //统计抽取奖励
            for (WCGPairInt pair : _m_stepRewardList.getList())
            {
                RefTileMatchStepReward refStepReward = RefTileMatchStepReward.getMgr().get(pair.first());
                if (refStepReward == null)
                {
                    USLog.error(getUsServer(),
                            "TileMatchPlayerInfo.drawStepReward, RefTileMatchStepReward not found for step: {}", pair.first());
                    continue;
                }

                //抽取次数 = 奖励倍数 * 每次抽取数量
                int drawTimes = pair.second() * refStepReward.draw_item_num;

                for (int i = 0; i < drawTimes; i++)
                {
                    NPCommonCostItem rewardItem = RefTileMatchJackpotGroup.getMgr().drawItem(refStepReward.jackpot_group_id);
                    if (rewardItem == null)
                    {
                        USLog.error(getUsServer(),
                                "TileMatchPlayerInfo.drawStepReward, drawItem failed for groupId: {}", refStepReward.jackpot_group_id);
                        continue;
                    }

                    itemCollector.addItem(rewardItem);
                }
            }

            //清空奖励
            _m_stepRewardList.clear();
            _m_bo.saveCanDrawStepRewardList(getUsServer().getBM(), _m_stepRewardList.toString());
            pushCanDrawStepRewardChg(_userData);
        } finally
        {
            _unlock();
        }

        _userData.gainItemList(itemCollector.getItemList(), _context);

        return Result.SUCC;
    }

    /**
     * 构造阶段奖励信息
     * @return
     */
    public TileMatch_StepRewardInfo makeStepRewardInfo()
    {
        _lock();
        try
        {
            TileMatch_StepRewardInfo stepRewardInfo = new TileMatch_StepRewardInfo();
            stepRewardInfo.setStep(_m_bo.getStepRewardStep());
            stepRewardInfo.setCurScore(_m_bo.getStepRewardScore());
            stepRewardInfo.setSerialId(_m_serial);
            return stepRewardInfo;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造信息
     */
    public TileMatch_Info makeInfo()
    {
        _lock();
        try
        {
            TileMatch_Info info = new TileMatch_Info();
            info.setStepRewardInfo(makeStepRewardInfo());
            for (WCGPairInt pair : _m_stepRewardList.getList())
            {
                info.addCanDrawStepRewardList(new TileMatch_CanDrawStepReward(pair.first(), pair.second()));
            }
            info.setTotalScore(_m_bo.getTotalScore());
            return info;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 发放玩家未领取的阶段奖励
     */
    public void dispatchUnclaimedStepRewards()
    {
        _lock();
        try{
            //如果没有未领取的奖励，跳过
            if (_m_stepRewardList.getList().isEmpty())
                return;

            //计算奖励道具列表
            List<NPCommonCostItem> rewardItemList = new ArrayList<>();
            for (WCGPairInt pair : _m_stepRewardList.getList())
            {
                RefTileMatchStepReward refStepReward = RefTileMatchStepReward.getMgr().get(pair.first());
                if (refStepReward == null)
                {
                    USLog.error(getUsServer(),
                            "TileMatchActivity._dispatchUnclaimedStepRewards, RefTileMatchStepReward not found for step: {}", pair.first());
                    continue;
                }

                //抽取次数 = 奖励倍数 * 每次抽取数量
                int drawTimes = pair.second() * refStepReward.draw_item_num;

                int failCount = 0;
                for (int i = 0; i < drawTimes; i++)
                {
                    NPCommonCostItem rewardItem = RefTileMatchJackpotGroup.getMgr().drawItem(refStepReward.jackpot_group_id);
                    if (rewardItem == null)
                    {
                        failCount++;
                        continue;
                    }

                    rewardItemList.add(rewardItem);
                }

                if (failCount > 0)
                {
                    USLog.error(getUsServer(),
                            "TileMatchActivity._dispatchUnclaimedStepRewards, drawItem failed, cid:{} groupId:{} failTimes:{}",
                            getCid(), refStepReward.jackpot_group_id, failCount);
                }
            }

            //如果没有有效奖励，跳过
            if (rewardItemList.isEmpty())
                return;

            //构造邮件数据
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefTileMatchOther.Ref().tilematch_step_reward_mail_id);
            mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(rewardItemList));
            MailSystem.addMail(getUsServer(),
                    getCid(), mailData, NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_CLOSE));
        }finally
        {
            _unlock();
        }
    }
}
