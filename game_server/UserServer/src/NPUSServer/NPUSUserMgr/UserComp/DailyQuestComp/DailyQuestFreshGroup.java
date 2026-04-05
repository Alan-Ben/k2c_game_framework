package NPUSServer.NPUSUserMgr.UserComp.DailyQuestComp;

import Common.MailObj.Mail_Data;
import Common.PlayerEnum.EPlayerEventRecordType;
import Common.QuestEnum.EDailyQuestType;
import Common.QuestObj.DailyQuest_Group;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.QuestErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Game.WeightLongValueList;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPEnum.ENCounterDealType;
import NPEnum.ENPFunctionType;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.GameObjs.Reward.RewardMgr;
import NPGameRes.GameObjs.Reward.RewardObj;
import NPGameRes.Refs.Quest.DailyQuest.RefDailyQuest;
import NPGameRes.Refs.Quest.DailyQuest.RefDailyQuestActiveReward;
import NPGameRes.Refs.Quest.DailyQuest.RefDailyQuestGroup;
import NPGameRes.Refs.Quest.DailyQuest.RefDailyQuestRefresh;
import NPGameRes.Refs.RefFuncUnlock;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.SynTask.NPSynPlayerEvnetRecordTask;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerDailyQuestActiveRewardBO;
import USDB.Bo.PlayerDailyQuestFreshBO;
import USLOGDB.Bo.LogDailyQuestRefreshBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * @description: 日常任务刷新组
 * @author: ricci
 * @date: 2022-10-12 14:31:36
 */
public class DailyQuestFreshGroup
{
    /**
     * 日常任务刷新组件
     */
    private final DailyQuestComponent _m_comp;
    /**
     * 刷新配置
     */
    private RefDailyQuestRefresh _m_refresh;

    /**
     * 刷新数据
     */
    private PlayerDailyQuestFreshBO _m_freshBo;

    /**
     * 任务列表
     */
    private ArrayList<DailyQuestInfo> _m_questInfoList;

    /**
     * 随机任务列表
     */
    private ArrayList<DailyQuestInfo> _m_randomQuestInfoList;

    /**
     * 已领取的活跃度任务奖励id记录
     */
    private ArrayList<Long> _m_hasTakenActiveRewardList;

    public DailyQuestFreshGroup(DailyQuestComponent _component, RefDailyQuestRefresh _refreshRef,
                                PlayerDailyQuestFreshBO _freshBo)
    {
        _m_freshBo = _freshBo;
        _m_comp = _component;
        _m_refresh = _refreshRef;
        _m_questInfoList = new ArrayList<>();
        _m_randomQuestInfoList = new ArrayList<>();
        _m_hasTakenActiveRewardList = new ArrayList<>();
    }

    public EDailyQuestType getType()
    {
        return _m_refresh.daily_quest_type;
    }

    public long getNextFreshTimeMs()
    {
        return getBo().getNextFreshTimeMs();
    }

    public PlayerDailyQuestFreshBO getBo()
    {
        return _m_freshBo;
    }

    public long getCid()
    {
        return _m_comp.getCid();
    }

    public long getSerial()
    {
        return getBo().getFreshSerial();
    }

    private synchronized long __incFreshSerial()
    {
        long serial = getSerial();
        return serial + 1;
    }

    public ArrayList<Long> getHasTakenActiveRewardIdList()
    {
        return new ArrayList<>(_m_hasTakenActiveRewardList);
    }

    /**
     * 从数据库初始化任务数据
     * @param questInfo NpPlayerDailyQuestBO
     */
    public void initQuestFromDB(DailyQuestInfo questInfo)
    {
        //根据是否是随机出来的任务，放入不同列表
        if (questInfo.getIsRandom())
        {
            _m_randomQuestInfoList.add(questInfo);
        } else
        {
            _m_questInfoList.add(questInfo);
        }
    }

    /**
     * 从数据库初始化活跃度奖励
     * @param _bo PlayerDailyQuestActiveRewardBO
     */
    public void initActiveRewardFromDB(PlayerDailyQuestActiveRewardBO _bo)
    {
        //与当前刷新序列号不一致
        if (_bo.getFreshSerial() != _m_freshBo.getFreshSerial())
        {
            USLog.error(_m_comp.getUSServer(), "initActiveReward freshSerial not equal cid:{},rewardId:{}"
                    , getCid(), _bo.getActiveRewardId());
            return;
        }
        _m_hasTakenActiveRewardList.add(_bo.getActiveRewardId());
    }

    /**
     * 尝试刷新任务
     * @param _nowTimeMS   当前时间
     * @param _freshSerial 刷新序列号
     */
    public void tryFresh(long _nowTimeMS, long _freshSerial)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DAILY_QUEST_REFRESH);

        long nextFreshTimeMs = getNextFreshTimeMs();
        //不到更新时间
        if (nextFreshTimeMs > _nowTimeMS)
        {
            return;
        }
        //刷新序列号不匹配。不允许刷新
        if (_freshSerial != getBo().getFreshSerial())
        {
            USLog.error(_m_comp.getUSServer(), "dailyQuest tryFresh serial not equal serial:{},curSerial:{}"
                    , _freshSerial, getBo().getFreshSerial());
            return;
        }

        //发放未领取奖励
        __drawNotDrawItem(context);

        //删除活跃度领取记录
        __removeActiveRewardRecord(getSerial());

        //删除刷新时需要移除的道具
        __removeFreshItem(context);

        //删除任务数据
        __removeQuest();

        //更新刷新时间和刷新序列号
        long freshSerial = __incFreshSerial();
        long nextFreshTimeTagMS = _m_refresh.refresh_clock.getNextFreshTimeTagMS(CommonFunc.getNowTimeMS());

        BM bmObj = _m_comp.getUSServer().getBM();

        _m_freshBo.setFreshSerial(bmObj, freshSerial);
        _m_freshBo.setNextFreshTimeMs(bmObj, nextFreshTimeTagMS);
        _m_freshBo.saveAllMarked(bmObj);

        //刷新任务
        __refreshQuest();

        //日常任务刷新日志
        LogDailyQuestRefreshBO bo = new LogDailyQuestRefreshBO();
        bo.setCid(bmObj, _m_comp.getCid());
        List<Long> normalQuestIdList = new ArrayList<>();
        List<Long> randomQuestIdList = new ArrayList<>();
        for (DailyQuestInfo questInfo : _m_questInfoList)
        {
            normalQuestIdList.add(questInfo.getRefId());
        }
        for (DailyQuestInfo questInfo : _m_randomQuestInfoList)
        {
            randomQuestIdList.add(questInfo.getRefId());
        }

        bo.setNormalQuestList(bmObj, CommonFunc.list2String(normalQuestIdList, ','));
        bo.setRandomQuestList(bmObj, CommonFunc.list2String(randomQuestIdList, ','));
        CommLogDB.log(bmObj, bo, null);
    }

    /**
     * 领取未发放奖励
     * @param _context
     */
    private void __drawNotDrawItem(NPPlayerContext _context)
    {
        //需要判断玩家是否已经解锁日常任务功能
        RefFuncUnlock refFuncUnlock = RefFuncUnlock.getMgr().get(ENPFunctionType.DAILY_QUEST.ordinal());
        if (refFuncUnlock != null
                && !NPPlayerConditionDealerMgr.IsEnable(refFuncUnlock.simple_unlock_id, _m_comp.getUserData(), null))
            return;
        
        //构造邮件数据
        Mail_Data mailData = new Mail_Data();
        mailData.setMailRefId(_m_refresh.mail_id);

        NPItemCollector collector = new NPItemCollector(_context.getContextId());
        //活跃值收集器
        NPItemCollector activationCollector = new NPItemCollector(_context.getContextId());

        int finishCount = 0;
        //放入任务未领取附件
        for (DailyQuestInfo questInfo : _m_questInfoList)
        {
            if (questInfo.recordTakeReward(true, _context).isSucc())
            {
                RefDailyQuest ref = questInfo.getRef();
                if (ref == null)
                    continue;

                collector.addItemList(ref.reward_item_list);
                activationCollector.addItem(ref.activation_item);
                finishCount++;
            }
        }
        //放入随机任务未领取附件
        for (DailyQuestInfo questInfo : _m_randomQuestInfoList)
        {
            if (questInfo.recordTakeReward(true, _context).isSucc())
            {
                RefDailyQuest ref = questInfo.getRef();
                if (ref == null)
                {
                    continue;
                }
                collector.addItemList(ref.reward_item_list);
                activationCollector.addItem(ref.activation_item);
                finishCount++;
            }
        }

        NPUSUserData userData = _m_comp.getUserData();
        userData.gainItemList(activationCollector.getAllItemList(), _context);

        //放入活跃度奖励附件
        for (RefDailyQuestActiveReward refDailyQuestActiveReward : _m_refresh._m_activeRewardList)
        {
            if (refDailyQuestActiveReward == null)
                continue;

            //检查活跃值是否足够
            if (!userData.hasItem(refDailyQuestActiveReward.draw_active_reward_need))
                continue;

            //领取活跃奖励，记录已领取状态
            if (!recordTakeActiveReward(refDailyQuestActiveReward.id))
                continue;

            //统计奖励
            RewardObj rewardObj = RewardMgr.getInstance().lookupReward(refDailyQuestActiveReward.reward_id);
            if (rewardObj == null)
            {
                USLog.error(_m_comp.getUSServer(), "DailyGroup __drawNotDrawItem rewardObj is null cid:{}, activeRefId:{} ,rewardId:{}"
                        , getCid(), refDailyQuestActiveReward.id, refDailyQuestActiveReward.reward_id);
                continue;
            }
            collector.addItemList(rewardObj.getItemList());
        }

        //检查是否有奖励
        if (collector.isEmpty())
        {
            return;
        }

        collector.fillProtoList(mailData.getItemList().getItemList());

        mailData.setIsMustRead(false);

        //玩家发送邮件处理
        MailSystem.addMail(_m_comp.getUSServer(), getCid(), mailData, _context);

        //记录玩家完成日常任务次数
        _m_comp.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.FINISH_DAILY_QUEST, finishCount, _context);
    }

    /**
     * 刷新任务
     */
    private void __refreshQuest()
    {
        //填充每日任务
        for (long questId : _m_refresh.daily_quest_id_list)
        {
            RefDailyQuest refDailyQuest = RefDailyQuest.getMgr().get(questId);
            if (refDailyQuest == null)
            {
                continue;
            }
            DailyQuestInfo questInfo = _m_comp._createNewQuest(refDailyQuest, this, false);
            if (questInfo == null)
            {
                //重复任务
                USLog.error(_m_comp.getUSServer(), "DailyGroup tryFresh multiple questInfo cid:{},questId:{}"
                        , getCid(), questId);
                continue;
            }
            _m_questInfoList.add(questInfo);
        }

        //使用随机规则生成随机任务
        for (Long randomRefId : _m_refresh.random_group_id_list)
        {
            RefDailyQuestGroup ref = RefDailyQuestGroup.getMgr().get(randomRefId);
            if (ref == null)
            {
                continue;
            }
            WeightLongValueList randomPoolClone = ref.refresh_daily_quest_wei_list.clone();
            for (int num = ref.num; num > 0; num--)
            {

                Long questRefId
                        = randomPoolClone.randomAndRemove();
                if (questRefId == null)
                {
                    //无法选取更多任务
                    continue;
                }
                RefDailyQuest refDailyQuest = RefDailyQuest.getMgr().get(questRefId);
                if (refDailyQuest == null)
                {
                    continue;
                }
                DailyQuestInfo questInfo = _m_comp._createNewQuest(refDailyQuest, this, true);
                if (questInfo == null)
                {
                    //重复任务
                    USLog.error(_m_comp.getUSServer(), "DailyGroup tryFresh multiple randomQuestInfo cid:{},questId:{}"
                            , getCid(), questRefId);
                    continue;
                }
                _m_randomQuestInfoList.add(questInfo);
            }
        }
    }

    /**
     * 删除任务数据
     */
    private void __removeQuest()
    {
        //销毁当前任务
        for (DailyQuestInfo questInfo : _m_questInfoList)
        {
            if (questInfo == null)
            {
                continue;
            }
            _m_comp._removeQuest(questInfo);
        }
        //清空任务列表
        _m_questInfoList.clear();

        for (DailyQuestInfo questInfo : _m_randomQuestInfoList)
        {
            if (questInfo == null)
            {
                continue;
            }
            //维护所有任务的数据集，同时触发任务的销毁
            _m_comp._removeQuest(questInfo);
        }
        //清空随机任务列表
        _m_randomQuestInfoList.clear();
    }

    /**
     * 删除活跃度阶段奖励领取记录
     */
    private void __removeActiveRewardRecord(long _serial)
    {
        //清空内存数据
        _m_hasTakenActiveRewardList.clear();

        //删除数据库数据
        HashMap<String, Object> condMap = new HashMap<>();
        condMap.put("cid", getCid());
        condMap.put("fresh_serial", _serial);
        condMap.put("daily_quest_type", getType().ordinal());

        _m_comp.getUSServer().getBM().getBM(PlayerDailyQuestActiveRewardBO.class).delAll(condMap);
    }


    /**
     * 刷新任务时需要移除的道具
     * @param _context 上下文
     */
    private void __removeFreshItem(NPPlayerContext _context)
    {
        for (NPCommonItem npCommonItem : _m_refresh.fresh_item_list)
        {
            _m_comp.getUserData().spendAll(npCommonItem, _context);
        }
    }


    /**
     * 领取活跃度奖励
     * @param _activeRewardId 活跃度奖励id
     * @return boolean
     */
    public boolean recordTakeActiveReward(long _activeRewardId)
    {
        //已领取
        if (_m_hasTakenActiveRewardList.contains(_activeRewardId))
        {
            return false;
        }

        BM bmObj = _m_comp.getUSServer().getBM();

        //记录数据库数据
        PlayerDailyQuestActiveRewardBO bo = new PlayerDailyQuestActiveRewardBO();
        bo.setCid(bmObj, getCid());
        bo.setActiveRewardId(bmObj, _activeRewardId);
        bo.setDailyQuestType(bmObj, getType().ordinal());
        bo.setFreshSerial(bmObj, getSerial());
        bo.insert(bmObj);

        _m_hasTakenActiveRewardList.add(_activeRewardId);

        //记录领取奖励次数
        NPSynPlayerEvnetRecordTask.syncRecord(_m_comp.getUserData(), ENCounterDealType.ADD,
                EPlayerEventRecordType.DRAW_DAILY_QUEST_ACTIVE_REWARD.ordinal(), _activeRewardId, 1);

        return true;
    }

    /**
     * 构造刷新组协议数据
     * @return DailyQuest_Group
     */
    public DailyQuest_Group makeProto()
    {
        DailyQuest_Group group = new DailyQuest_Group();
        group.setNextFreshTimeMs(getNextFreshTimeMs());
        group.setRefreshSerial(getSerial());
        group.setType(getType());
        for (DailyQuestInfo questInfo : _m_questInfoList)
        {
            group.addQuestInfoList(questInfo.makeProto());
        }
        for (DailyQuestInfo questInfo : _m_randomQuestInfoList)
        {
            group.addRandomQuestInfoList(questInfo.makeProto());
        }
        group.getHasTakenActiveRewardRefIdList().addAll(getHasTakenActiveRewardIdList());
        return group;
    }

    /**
     * 设置刷新时间
     * @param _freshTime 刷新时间
     */
    public void setFreshTime(long _freshTime)
    {
        getBo().saveNextFreshTimeMs(_m_comp.getUSServer().getBM(), _freshTime);
    }

    /**
     * 领取任务奖励
     * @param _refreshSerial 刷新id
     * @param _context       上下文
     * @param _itemList
     * @return 奖励信息
     */
    public List<Long> aKeyDrawQuestReward(long _refreshSerial, NPPlayerContext _context, List<NPCommon_ItemInfo> _itemList)
    {
        //领取成功的奖励id列表
        List<Long> retList = new ArrayList<>();

        //检查刷新序列号
        if (getSerial() != _refreshSerial)
        {
            USLog.error(_m_comp.getUSServer(), "DailyQuestFreshGroup aKeyDrawQuestReward questSerial not equal cid:{} type:{}"
                    , getCid(), getType());
            return retList;
        }

        //遍历任务列表
        for (DailyQuestInfo questInfo : _m_questInfoList)
        {
            //尝试获取每日任务奖励
            if (!questInfo.recordTakeReward(false, _context).isSucc())
                continue;

            //发放道具
            _m_comp.getUserData().gainItem(questInfo.getRef().activation_item, _context);
            _m_comp.getUserData().gainItemList(questInfo.getRef().reward_item_list, _context);

            retList.add(questInfo.getRefId());
            _context.getCollector().fillProtoList(_itemList);
            _context.resetCollector();
        }

        return retList;
    }

    /**
     * 领取活跃度奖励
     * @param _refreshSerial 刷新id
     * @param _context       上下文
     * @return 成功领取的奖励id列表
     */
    public Result drawActiveReward(long _activeRewardId, long _refreshSerial, NPPlayerContext _context)
    {
        //检查刷新序列号
        if (getSerial() != _refreshSerial)
        {
            USLog.error(_m_comp.getUSServer(), "DailyQuestFreshGroup drawActiveReward questSerial not equal cid:{} type:{}"
                    , getCid(), getType());
            return CommErr.SYS_ERR;
        }

        RefDailyQuestActiveReward refActiveReward = RefDailyQuestActiveReward.getMgr().get(_activeRewardId);
        if (refActiveReward == null)
            return CommErr.REF_NOT_FOUND;

        //检查活跃值是否足够
        if (!_m_comp.getUserData().hasItem(refActiveReward.draw_active_reward_need))
            return QuestErr.DAILY_QUEST_ACTIVE_POINT_NOT_ENOUGH;

        //领取活跃奖励，记录已领取状态
        if (!recordTakeActiveReward(_activeRewardId))
            return QuestErr.DAILY_QUEST_ACTIVE_REWARD_HAS_DRAW;

        //发放奖励
        RewardObj rewardObj = RewardMgr.getInstance().lookupReward(refActiveReward.reward_id);
        if (rewardObj == null)
        {
            USLog.error(_m_comp.getUSServer(), "DailyGroup __drawNotDrawItem rewardObj is null cid:{}, activeRefId:{} ,rewardId:{}"
                    , _m_comp.getUserData().getCid(), refActiveReward.id, refActiveReward.reward_id);
            return CommErr.REF_NOT_FOUND;
        }

        _m_comp.getUserData().gainItemList(rewardObj.getItemList(), _context);

        _m_comp.getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_061_OnPlayerDailyQuestActiveRewardChg(getType(),
                getSerial(), getHasTakenActiveRewardIdList()));

        return Result.SUCC;
    }

    /**
     * 领取活跃度奖励
     * @param _refreshSerial    刷新id
     * @param _context          上下文
     * @return 成功领取的奖励id列表
     */
    public List<Long> aKeyDrawActiveReward(long _refreshSerial, NPPlayerContext _context, ArrayList<NPCommon_ItemInfo> _itemList)
    {
        //领取成功的奖励id列表
        List<Long> sucDrawRewardList = new ArrayList<>();

        //检查刷新序列号
        if (getSerial() != _refreshSerial)
        {
            USLog.error(_m_comp.getUSServer(), "DailyQuestFreshGroup aKeyDrawActiveReward questSerial not equal cid:{} type:{}"
                    , getCid(), getType());
            return sucDrawRewardList;
        }

        //根据任务类型获取对应的奖励列表
        List<RefDailyQuestActiveReward> rewardRefList = RefDailyQuestActiveReward.getMgr().getRewardListByType(getType());
        //遍历活跃度奖励列表
        for (RefDailyQuestActiveReward refReward : rewardRefList)
        {
            //检查活跃值是否足够
            if (!_m_comp.getUserData().hasItem(refReward.draw_active_reward_need))
                continue;

            //领取活跃奖励，记录已领取状态
            if (!recordTakeActiveReward(refReward.Id()))
                continue;

            //发放奖励
            RewardObj rewardObj = RewardMgr.getInstance().lookupReward(refReward.reward_id);
            if (rewardObj == null)
            {
                USLog.error(_m_comp.getUSServer(), "DailyGroup __drawNotDrawItem rewardObj is null cid:{}, activeRefId:{} ,rewardId:{}"
                        , _m_comp.getUserData().getCid(), refReward.id, refReward.reward_id);
                continue;
            }

            _m_comp.getUserData().gainItemList(rewardObj.getItemList(), _context);
            sucDrawRewardList.add(refReward.Id());

            _context.getCollector().fillProtoList(_itemList);
            _context.resetCollector();
        }

        if (!sucDrawRewardList.isEmpty())
        {
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_061_OnPlayerDailyQuestActiveRewardChg(getType(),
                    getSerial(), getHasTakenActiveRewardIdList()));
        }

        return sucDrawRewardList;
    }
}
