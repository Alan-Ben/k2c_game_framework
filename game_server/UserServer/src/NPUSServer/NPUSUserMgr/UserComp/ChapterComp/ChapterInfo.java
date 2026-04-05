package NPUSServer.NPUSUserMgr.UserComp.ChapterComp;

import Common.ChapterEnum.EChapterEventType;
import Common.ChapterEnum.EChapterInspireType;
import Common.ChapterObj.Chapter_EventInfo;
import Common.ChapterObj.Chapter_InspireInfo;
import Common.ChapterObj.Chapter_PosInfo;
import Common.ChapterObj.Chapter_SingleInspireInfo;
import CommonEnum.ECurrency;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.ChapterErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerVariableVarType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.Refs.Chapter.Event._ARefChapterEvent;
import NPGameRes.Refs.Chapter.RefChapter;
import NPGameRes.Refs.Chapter.RefChapterCost;
import NPGameRes.Refs.Chapter.RefChapterEvent;
import NPGameRes.Refs.Common.RefTimePrice;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_CHAPTER_FORWARD;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event.ChapterEvent_Choice;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event.ChapterEvent_Dispatch;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event.ChapterEvent_Reward;
import NPUSServer.NPUSUserMgr.UserComp.ChapterComp.Event._AChapterEvent;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_016_ChapterOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerChapterBO;
import USDB.Bo.PlayerChapterEventBO;
import USLOGDB.Bo.LogChapterFightBossBO;
import USLOGDB.Bo.LogChapterForwardBO;

import java.util.ArrayList;
import java.util.List;

public class ChapterInfo
{
    private ChapterComponent _m_comp;
    private PlayerChapterBO _m_bo;
    private RefChapter _m_ref;

    private _AChapterEvent _m_eventInfo;

    public ChapterInfo(ChapterComponent _comp, PlayerChapterBO _bo)
    {
        _m_comp = _comp;
        _m_bo = _bo;

        _m_ref = RefChapter.getMgr().get(_bo.getChapterId());
        if (_m_ref == null)
            USLog.error(_comp.getUSServer(), "ChapterInfo init fail ref is null, chapterId:{}", _bo.getChapterId());
    }

    public long getChapterId()
    {
        return _m_bo.getChapterId();
    }

    public int getPoint()
    {
        return _m_bo.getPoint();
    }

    public RefChapter getRef()
    {
        return _m_ref;
    }

    /**
     * 初始化事件
     * @param _bo
     */
    public void initEvent(PlayerChapterEventBO _bo)
    {
        if (_bo == null)
            return;

        _m_eventInfo = _createChapterEventObj(_bo);
    }

    /**
     * 创建章节事件对象
     * @param _bo 章节事件数据对象
     * @return 章节事件对象
     */
    private _AChapterEvent _createChapterEventObj(PlayerChapterEventBO _bo)
    {
        //查找事件配置
        RefChapterEvent refChapterEvent = RefChapterEvent.getMgr().get(_bo.getEventId());
        if (refChapterEvent == null)
        {
            USLog.error(_m_comp.getUSServer(), "ChapterInfo._createChapterEventObj: can not find ref, eventId:{}", _bo.getEventId());
            return null;
        }

        _ARefChapterEvent detailRef = refChapterEvent.detailRef;
        if (detailRef == null)
        {
            USLog.error(_m_comp.getUSServer(), "ChapterInfo._createChapterEventObj: can not find detail ref, eventId:{}", _bo.getEventId());
            return null;
        }

        //根据事件类型创建事件对象
        EChapterEventType eventType = detailRef.getEventType();
        switch (eventType)
        {
            case REWARD:
                return new ChapterEvent_Reward(_m_comp, _bo, refChapterEvent);
            case CHOICE:
                return new ChapterEvent_Choice(_m_comp, _bo, refChapterEvent);
            case DISPATCH:
                return new ChapterEvent_Dispatch(_m_comp, _bo, refChapterEvent);
            default:
                return null;
        }
    }

    /**
     * 获取事件信息
     * @return
     */
    public _AChapterEvent getEventInfo()
    {
        return _m_eventInfo;
    }

    /**
     * 战力比例 万分比
     * 战力比例 =（玩家战力-关卡战力）/关卡战力
     */
    public ResultOne<Long> getPowerRate(RefChapter _ref, int _point)
    {
        //前进消耗金币比例范围
        WCGPairInt forwardGoldCostRatioRange = RefGeneral.Ref().forward_gold_cost_ratio_range;
        //计算目标位置所需战力
        ResultOne<Long> needPowerResult = _ref.calNeedPower(_point);
        if (!needPowerResult.isSucc())
            return needPowerResult;

        long needPower = needPowerResult.getData();
        long totalPower = _m_comp.getUserData().getHeroComponent().getTotalPower();

        //计算减免比例
        long reduceRate = (long) Math.ceil((double) (totalPower - needPower) / needPower * 10000);
        //限制减免比例范围
        return ResultOne.succ(forwardGoldCostRatioRange.limit(reduceRate));
    }

    /**
     * 计算金币消耗
     *
     * 计算流程：
     * 每前进1步的金币消耗 = 关卡金币消耗基础值*(1+递增比例)*（1 - 金币消耗减免比例），结果向上取整
     * 金币消耗减免比例=（玩家战力-关卡战力）/关卡战力 * 金币消耗放大倍数
     * 其中, 金币消耗放大倍数根据[（玩家战力-关卡战力）/关卡战力]的计算结果所在范围进行取值
     *
     * @param _ref 章节配置
     * @param _point 目标点位
     * @return 金币消耗
     */
    public ResultOne<Long> calGoldCost(RefChapter _ref, int _point)
    {
        //计算基础金币消耗
        ResultOne<Double> goldCostResult = _ref.calNeedCost(_point);
        if (!goldCostResult.isSucc())
            return ResultOne.failed(goldCostResult.getResult());

        double baseGoldCost = goldCostResult.getData();

        //计算战力比例 万分比
        ResultOne<Long> powerRateResult = getPowerRate(_ref, _point);
        if (!powerRateResult.isSucc())
            return powerRateResult;

        long powerRate = powerRateResult.getData();

        //金币消耗放大倍数 万分比
        int costMultipleRate = RefChapterCost.getMgr().getCostMultipleRate((int) powerRate);

        //计算最终金币消耗
        long finalCost = (long) Math.ceil(baseGoldCost * (1 - costMultipleRate / 10000d * powerRate / 10000d));

        return ResultOne.succ(finalCost);
    }

    /**
     * 获取关卡进度
     * @return
     */
    public long getChapterProgress()
    {
        return getChapterId() * 1000 + getPoint();
    }

    public static class ForwardResult
    {
        public int coefficient;
        public long rewardHeroExp;
        public long rewardPlayerExp;
        public long costGoldNum;

        public ForwardResult(int coefficient, long rewardExp, long rewardPlayerExp, long costGoldNum)
        {
            this.coefficient = coefficient;
            this.rewardHeroExp = rewardExp;
            this.rewardPlayerExp = rewardPlayerExp;
            this.costGoldNum = costGoldNum;
        }
    }

    /**
     * 前进
     */
    public ResultOne<ForwardResult> forward(long _chapterId, int _targetPoint, NPPlayerContext _context)
    {
        //获取关卡配置
        RefChapter ref = getRef();
        if (ref == null)
            return ResultOne.failed(CommErr.REF_NOT_FOUND);

        //判断是否有事件
        if (_m_eventInfo != null)
            return ResultOne.failed(ChapterErr.CHAPTER_EVENT_NOT_DONE);

        //判断关卡是否解锁
        if (!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().unlock_chapter_simple_unlock_id, _m_comp.getUserData(), null))
            return ResultOne.failed(ChapterErr.CHAPTER_NOT_UNLOCK);

        //获取当前位置
        int curPoint = _m_bo.getPoint();
        //获取目标位置
        int targetPoint = curPoint + 1;

        //校验目标位置是否符合
        if (_chapterId != _m_bo.getChapterId() || targetPoint != _targetPoint)
            return ResultOne.failed(ChapterErr.CHAPTER_POINT_NOT_FIT);

        //如果下一个格子是boss格, boss格需要走特殊逻辑
        if (targetPoint == ref.point_count)
            return ResultOne.failed(ChapterErr.CHAPTER_BOSS_NOT_DEFEAT);

        //计算金币消耗
        ResultOne<Long> goldCostResult = calGoldCost(ref, targetPoint);
        if (!goldCostResult.isSucc())
            return ResultOne.failed(goldCostResult.getResult());
        long goldCost = goldCostResult.getData();

        //计算是否暴击
        int coefficient = 10000;
        boolean isCrit = false;
        if (CommonFunc.randomInt(10000) < ref.crit_probability)
        {
            isCrit = true;
            coefficient = RefGeneral.Ref().critical_hit_coefficient;
        }

        //获得奖励
        ResultOne<Long> rewardExpResult = ref.calRewardExp(targetPoint);
        if (!rewardExpResult.isSucc())
            return ResultOne.failed(rewardExpResult.getResult());
        //计算暴击后的奖励
        long rewardExp = rewardExpResult.getData() * coefficient / 10000;
        //检查金币是否足够
        if (!_m_comp.getUserData().spendItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), goldCost, _context))
            return ResultOne.failed(CommErr.ITEM_NOT_ENOUGH);

        _m_comp.getUserData().gainItem(ENPItemType.CURRENCY, ECurrency.HERO_EXP.ordinal(), rewardExp, _context);
        _m_comp.getUserData().gainItem(ENPItemType.CURRENCY, ECurrency.P_EXP.ordinal(), ref.reward_player_exp, _context);

        //更新当前位置
        _m_bo.savePoint(_m_comp.getUSServer().getBM(), targetPoint);

        //推送变更
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_051_OnChapterPosChg(getPosInfo()));

        _m_comp.getUserData().onLogicEvent(new Event_P_CHAPTER_FORWARD(_context));

        // 写入日志
        LogChapterForwardBO logBo = new LogChapterForwardBO();
        logBo.setCid(_m_comp.getUSServer().getBM(), _m_comp.getUserData().getCid());
        logBo.setLevel(_m_comp.getUSServer().getBM(), (int) _m_comp.getUserData().getParam(ENPPlayerParam.LEVEL));
        logBo.setChapterId(_m_comp.getUSServer().getBM(), _chapterId);
        logBo.setPoint(_m_comp.getUSServer().getBM(), targetPoint);
        logBo.setPower(_m_comp.getUSServer().getBM(), _m_comp.getUserData().getHeroComponent().getTotalPower());
        logBo.setEarnings(_m_comp.getUSServer().getBM(), _m_comp.getUserData().getPlayerComponent().getEarnings());
        logBo.setIsCriticalHit(_m_comp.getUSServer().getBM(), isCrit);
        logBo.setGoldCost(_m_comp.getUSServer().getBM(), goldCost);
        CommLogDB.log(_m_comp.getUSServer().getBM(), logBo, _context);

        return ResultOne.succ(new ForwardResult(coefficient, rewardExp, ref.reward_player_exp, goldCost));
    }

    /**
     * 触发事件
     */
    public RefChapterEvent tryTriggerEvent(NPPlayerContext context)
    {
        //获取关卡配置
        RefChapter ref = getRef();
        if (ref == null)
        {
            USLog.error(_m_comp.getUSServer(), "ChapterInfo.tryTriggerEvent: can not find ref, chapterId:{}", _m_bo.getChapterId());
            return null;
        }

        //查询当前位置是否有事件
        for (WCGPairLong eventPair : ref.point_event.getList())
        {
            if (eventPair.first() == _m_bo.getPoint())
            {
                //获取事件配置
                RefChapterEvent refEvent = RefChapterEvent.getMgr().get(eventPair.second());
                if (refEvent == null)
                {
                    USLog.error(_m_comp.getUSServer(), "ChapterInfo.tryTriggerEvent: can not find ref, eventId:{}", eventPair.second());
                    return null;
                }

                return refEvent;
            }
        }

        return null;
    }

    /**
     * 创建事件
     * @param _refEvent
     */
    public void createEvent(RefChapterEvent _refEvent)
    {
        PlayerChapterEventBO bo = new PlayerChapterEventBO();
        bo.setCid(_m_comp.getUSServer().getBM(), _m_bo.getCid());
        bo.setEventId(_m_comp.getUSServer().getBM(), _refEvent.Id());
        bo.insert(_m_comp.getUSServer().getBM());

        //创建事件对象
        _m_eventInfo = _createChapterEventObj(bo);
        if (_m_eventInfo != null)
        {
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_053_OnChapterEventChg(_m_eventInfo.makeInfo()));
        }
    }

    /**
     * 鼓舞
     * @param _type
     * @param _context
     * @return
     */
    public Result inspire(EChapterInspireType _type, NPPlayerContext _context)
    {
        switch (_type)
        {
            case GOLD:
                return inspireGold(_context);
            case ITEM:
                return inspireItem(_context);
            case CRYSTAL:
                return inspireCrystal(_context);
        }
        return CommErr.PARAM_ERROR;
    }

    /**
     * 道具鼓舞
     */
    public Result inspireItem(NPPlayerContext _context)
    {
        //获取关卡配置
        RefChapter ref = getRef();
        if (ref == null)
            return CommErr.REF_NOT_FOUND;

        //获取目标位置
        int targetPoint = _m_bo.getPoint() + 1;

        //检查是否下一格是boss格
        if (targetPoint != ref.point_count)
            return ChapterErr.CHAPTER_NOT_ATTACK_BOSS;

        //获取道具鼓舞次数
        int times = _m_bo.getItemInspireTimes();
        //检查道具是否足够
        if (!_m_comp.getUserData().spendItem(RefGeneral.Ref().item_inspire_cost, _context))
            return CommErr.ITEM_NOT_ENOUGH;

        //增加鼓舞次数
        _m_bo.saveItemInspireTimes(_m_comp.getUSServer().getBM(), times + 1);

        //推送变更
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_052_OnChapterInspireChg(getInspireInfo()));

        return Result.SUCC;
    }

    /**
     * 钻石鼓舞
     */
    public Result inspireCrystal(NPPlayerContext _context)
    {
        //获取关卡配置
        RefChapter ref = getRef();
        if (ref == null)
            return CommErr.REF_NOT_FOUND;

        //获取目标位置
        int targetPoint = _m_bo.getPoint() + 1;

        //检查是否下一格是boss格
        if (targetPoint != ref.point_count)
            return ChapterErr.CHAPTER_NOT_ATTACK_BOSS;

        //获取钻石鼓舞次数
        int times = _m_bo.getCrystalInspireTimes();
        //检查钻石是否足够
        if (!_m_comp.getUserData().spendItem(RefGeneral.Ref().crystal_inspire_fixed_cost, _context))
            return CommErr.ITEM_NOT_ENOUGH;

        //增加鼓舞次数
        _m_bo.saveCrystalInspireTimes(_m_comp.getUSServer().getBM(), times + 1);

        //推送变更
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_052_OnChapterInspireChg(getInspireInfo()));

        return Result.SUCC;
    }

    /**
     * 金币鼓舞
     */
    public Result inspireGold(NPPlayerContext _context)
    {
        //获取关卡配置
        RefChapter ref = getRef();
        if (ref == null)
            return CommErr.REF_NOT_FOUND;

        //获取目标位置
        int targetPoint = _m_bo.getPoint() + 1;

        //检查是否下一格是boss格
        if (targetPoint != ref.point_count)
            return ChapterErr.CHAPTER_NOT_ATTACK_BOSS;

        //获取金币鼓舞次数
        int times = _m_bo.getGoldInspireTimes();
        //获取金币鼓舞消耗
        long cost = calGoldInspireCost(ref, times + 1);
        //检查金币是否足够
        if (!_m_comp.getUserData().spendItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), cost, _context))
            return CommErr.ITEM_NOT_ENOUGH;

        //增加鼓舞次数
        _m_bo.saveGoldInspireTimes(_m_comp.getUSServer().getBM(), times + 1);

        //推送变更
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_052_OnChapterInspireChg(getInspireInfo()));

        return Result.SUCC;
    }

    /**
     * 计算金币鼓舞消耗
     * @param _times
     * @return
     */
    private long calGoldInspireCost(RefChapter _ref, int _times)
    {
        long costNum = 0;
        costNum += _ref.gold_inspire_base_value;
        RefTimePrice refTimePrice = RefTimePrice.getMgr().getPrice(RefGeneral.Ref().gold_inspire_calculate_ratio_time_price_id, _times);
        if (refTimePrice != null)
        {
            NPVarInfo varInfo = new NPVarInfo();
            varInfo.addObj(ENPPlayerVariableVarType.BUY_TIMES.ordinal(), _m_bo.getGoldInspireTimes() + 1);
            costNum *= NPPlayerVariableDeal.getInstance().CalculateVariableResult(_m_comp.getUserData(), refTimePrice.cost_item_formula, varInfo);
        }
        return costNum;
    }

    /**
     * 挑战boss
     */
    public ResultOne<List<NPCommon_ItemInfo>> challengeBoss(long _chapterId, NPPlayerContext _context)
    {
        //获取关卡配置
        RefChapter ref = getRef();
        if (ref == null)
            return ResultOne.failed(CommErr.REF_NOT_FOUND);

        //判断是否有事件
        if (_m_eventInfo != null)
            return ResultOne.failed(ChapterErr.CHAPTER_EVENT_NOT_DONE);

        //判断关卡是否解锁
        if (!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().unlock_chapter_simple_unlock_id, _m_comp.getUserData(), null))
            return ResultOne.failed(ChapterErr.CHAPTER_NOT_UNLOCK);

        //获取当前位置
        int curPoint = _m_bo.getPoint();
        //获取目标位置
        int targetPoint = curPoint + 1;

        //校验目标位置是否符合
        if (_chapterId != _m_bo.getChapterId())
            return ResultOne.failed(ChapterErr.CHAPTER_POINT_NOT_FIT);

        //检查是否已经到达最后一个格子
        if (curPoint >= ref.point_count)
            return ResultOne.failed(ChapterErr.CHAPTER_BOSS_HAD_DEFEAT);

        //检查下一个格子是否是boss格
        if (targetPoint != ref.point_count)
            return ResultOne.failed(ChapterErr.CHAPTER_NOT_ATTACK_BOSS);

        //boss战力
        long bossPower = ref.boss_power;
        //大臣总战力
        long basePower = _m_comp.getUserData().getHeroComponent().getTotalPower();
        //需要算上鼓舞的加成
        int totalIncreasePer = 0;
        totalIncreasePer += RefGeneral.Ref().gold_inspire_increase_power_ratio_per * _m_bo.getGoldInspireTimes();
        totalIncreasePer += RefGeneral.Ref().item_inspire_increase_power_ratio_per * _m_bo.getItemInspireTimes();
        totalIncreasePer += RefGeneral.Ref().crystal_inspire_increase_power_ratio_per * _m_bo.getCrystalInspireTimes();
        //总战力
        long totalPower = basePower * (10000 + totalIncreasePer) / 10000;
        //检查战力是否足够
        if (totalPower < bossPower)
            return ResultOne.failed(ChapterErr.CHAPTER_ATTACK_BOSS_POWER_NOT_ENOUGH);

        //需要过滤妃子单独获取
        List<NPCommonCostItem> itemList = new ArrayList<>();
        List<NPCommonCostItem> consortList = new ArrayList<>();
        for (NPCommonCostItem item : ref.reward_list)
        {
            if (item.getItemType() == ENPItemType.CONSORT)
            {
                consortList.add(item);
            } else
            {
                itemList.add(item);
            }
        }
        //获得奖励
        _m_comp.getUserData().gainItemList(itemList, _context);

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHAPTER_GAIN_CONSORT);
        for (NPCommonCostItem consortItem : consortList)
        {
            _m_comp.getUserData().gainItem(ENPItemType.CONSORT, consortItem.getItemId(), context);
        }

        //获取下一章节
        long nextChapterId = _m_bo.getChapterId() + 1;
        _m_ref = RefChapter.getMgr().get(nextChapterId);
        //更新当前位置
        BM bmObj = _m_comp.getUSServer().getBM();
        _m_bo.setPoint(bmObj, 0);
        _m_bo.setChapterId(bmObj, nextChapterId);
        _m_bo.setGoldInspireTimes(bmObj, 0);
        _m_bo.setCrystalInspireTimes(bmObj, 0);
        _m_bo.setItemInspireTimes(bmObj, 0);
        _m_bo.saveAllMarked(bmObj);

        //构造奖励信息
        List<NPCommon_ItemInfo> rewardList = new ArrayList<>();
        _context.getCollector().fillProtoList(rewardList);

        //推送变更
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_051_OnChapterPosChg(getPosInfo()));
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_052_OnChapterInspireChg(getInspireInfo()));

        _m_comp.getUserData().onLogicEvent(new Event_P_CHAPTER_FORWARD(_context));

        LogChapterFightBossBO logBo = new LogChapterFightBossBO();
        logBo.setCid(_m_comp.getUSServer().getBM(), _m_comp.getUserData().getCid());
        logBo.setLevel(_m_comp.getUSServer().getBM(), (int)_m_comp.getUserData().getParam(ENPPlayerParam.LEVEL));
        logBo.setChapterId(_m_comp.getUSServer().getBM(), _chapterId);
        logBo.setPower(_m_comp.getUSServer().getBM(), basePower);
        logBo.setEarnings(_m_comp.getUSServer().getBM(), _m_comp.getUserData().getPlayerComponent().getEarnings());
        logBo.setGoldInspireTimes(_m_comp.getUSServer().getBM(), _m_bo.getGoldInspireTimes());
        logBo.setItemInspireTimes(_m_comp.getUSServer().getBM(), _m_bo.getItemInspireTimes());
        logBo.setCrystalInspireTimes(_m_comp.getUSServer().getBM(), _m_bo.getCrystalInspireTimes());
        logBo.setFinalPower(_m_comp.getUSServer().getBM(), totalPower);
        CommLogDB.log(_m_comp.getUSServer().getBM(), logBo, _context);

        return ResultOne.succ(rewardList);
    }

    /**
     * 获取位置信息
     * @return
     */
    public Chapter_PosInfo getPosInfo()
    {
        return new Chapter_PosInfo(_m_bo.getChapterId(), _m_bo.getPoint());
    }

    /**
     * 构造鼓舞信息
     * @return
     */
    public Chapter_InspireInfo getInspireInfo()
    {
        Chapter_InspireInfo inspireInfo = new Chapter_InspireInfo();
        inspireInfo.addInspireList(new Chapter_SingleInspireInfo(EChapterInspireType.GOLD, _m_bo.getGoldInspireTimes()));
        inspireInfo.addInspireList(new Chapter_SingleInspireInfo(EChapterInspireType.ITEM, _m_bo.getItemInspireTimes()));
        inspireInfo.addInspireList(new Chapter_SingleInspireInfo(EChapterInspireType.CRYSTAL, _m_bo.getCrystalInspireTimes()));
        return inspireInfo;
    }

    /**
     * gm修改位置
     * @param _chapterId
     * @param _point
     * @return
     */
    public Result gmChgPos(long _chapterId, int _point)
    {
        RefChapter refChapter = RefChapter.getMgr().get(_chapterId);
        if (refChapter == null)
            return CommErr.REF_NOT_FOUND;

        //判断点位是否合法
        if (_point < 0 || _point > refChapter.point_count)
            return CommErr.PARAM_ERROR;

        _m_ref = refChapter;
        //更新当前位置
        BM bmObj = _m_comp.getUSServer().getBM();
        _m_bo.setPoint(bmObj, _point);
        _m_bo.setChapterId(bmObj, _chapterId);
        _m_bo.setGoldInspireTimes(bmObj, 0);
        _m_bo.setCrystalInspireTimes(bmObj, 0);
        _m_bo.setItemInspireTimes(bmObj, 0);
        _m_bo.saveAllMarked(bmObj);

        //推送变更
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_054_OnChapterGmPosChg(getPosInfo()));
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_052_OnChapterInspireChg(getInspireInfo()));

        return Result.SUCC;
    }

    /**
     * 销毁事件
     * @param _event
     * @param _context
     */
    public void disposeEvent(_AChapterEvent _event, NPPlayerContext _context)
    {
        if (_event == null)
            return;

        _m_eventInfo = null;
        _event.dispose();

        _m_comp.getUserData().sendMsgToGC(US2GCWriter_016_ChapterOp.make_053_OnChapterEventChg(new Chapter_EventInfo()));
    }

    /**
     * gm到达boss点
     * @return
     */
    public Result gmToBossPoint()
    {
        //获取关卡配置
        RefChapter ref = getRef();
        if (ref == null)
            return CommErr.REF_NOT_FOUND;

        //计算boss点位
        int bossPoint = ref.point_count - 1;

        //gm修改位置
        return gmChgPos(_m_bo.getChapterId(), bossPoint);
    }

    @Override
    public String toString()
    {
        return "ChapterInfo{" + "chapterId=" + _m_bo.getChapterId() +
                "point=" + _m_bo.getPoint() +
                '}';
    }
}
