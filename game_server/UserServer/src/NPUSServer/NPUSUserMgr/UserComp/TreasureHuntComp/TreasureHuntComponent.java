package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp;

import ALBasicServer.ALProcess.ALProcess;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.TreasureHuntEnum.ETreasureHuntCaptureType;
import Common.TreasureHuntEnum.ETreasureHuntGainType;
import Common.TreasureHuntObj.*;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.ErrMain.TreasureHuntErr;
import NPCommon.Game.QualityValueData;
import NPCommon.Game.WeightQualityValueList;
import NPCommon.Game.WeightValueList;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.ENPPlayerRecordParam;
import NPEnum.EQuality;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntArea;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntAreaDistance;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntStationLevel;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntTreasure;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_TREASURE_HUNT_CAPTURE;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerLazyCDComp.PlayerLazyCD;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Composite.TreasureHuntCompositeInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Composite.TreasureHuntCompositeMgr;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Guarantee.TreasureHuntGuaranteeAreaInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Guarantee.TreasureHuntGuaranteeMgr;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Ore.TreasureHuntOreInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Ore.TreasureHuntOreMgr;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Treasure.TreasureHuntTreasureInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Treasure.TreasureHuntTreasureMgr;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureAnalyseCollector.ETreasureHuntDataAnalyseType;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerTreasureHuntBO;
import com.google.gson.Gson;
import com.google.gson.JsonObject;

import java.util.ArrayList;
import java.util.List;

public class TreasureHuntComponent extends _ANPUserComponent implements _IHandlerHolder
{
    private TreasureHuntOreMgr _m_oreMgr;
    private TreasureHuntTreasureMgr _m_treasureMgr;
    private TreasureHuntCompositeMgr _m_compositeMgr;
    private TreasureHuntGuaranteeMgr _m_guaranteeMgr;

    private long _m_dbId;
    private int _m_stationLevel;
    private long _m_exp;

    private RefTreasureHuntStationLevel _m_levelRef;

    private NPPlayerPropertyContainer _m_propertyContainer;

    public TreasureHuntComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.TREASURE_HUNT);

        _m_oreMgr = new TreasureHuntOreMgr(this);
        _m_treasureMgr = new TreasureHuntTreasureMgr(this);
        _m_compositeMgr = new TreasureHuntCompositeMgr(this);
        _m_guaranteeMgr = new TreasureHuntGuaranteeMgr(this);

        _m_propertyContainer = new NPPlayerPropertyContainer();
    }

    public TreasureHuntOreMgr getOreMgr()
    {
        return _m_oreMgr;
    }

    public TreasureHuntTreasureMgr getTreasureMgr()
    {
        return _m_treasureMgr;
    }

    public TreasureHuntCompositeMgr getCompositeMgr()
    {
        return _m_compositeMgr;
    }

    public TreasureHuntGuaranteeMgr getGuaranteeMgr()
    {
        return _m_guaranteeMgr;
    }

    public NPPlayerPropertyContainer getPropertyContainer()
    {
        return _m_propertyContainer;
    }

    public int getStationLevel()
    {
        return _m_stationLevel;
    }

    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("treasure_hunt_init_process");
        //初始化加载，加载过程如果出现异常，则直接加载失败
        //步骤1: 主数据加载
        process.addResDelegateProcess(action -> _initMainFromDB(action::dealAction),
                "init_mian", null, false);
        //步骤2: 矿石数据加载
        process.addResDelegateProcess(action -> _m_oreMgr._initOreFromDB(action::dealAction),
                "init_ore", null, false);
        //步骤3: 奇物数据加载
        process.addResDelegateProcess(action -> _m_treasureMgr._initTreasureFromDB(action::dealAction),
                "init_treasure", null, false);
        //步骤3: 奇物产出数据加载
        process.addResDelegateProcess(action -> _m_treasureMgr._initTreasureOutputFromDB(action::dealAction),
                "init_treasure_output", null, false);
        //步骤4: 组合数据加载
        process.addResDelegateProcess(action -> _m_compositeMgr._initCompositeFromDB(action::dealAction),
                "init_composite", null, false);
        //步骤5: 保底数据加载
        process.addResDelegateProcess(action -> _m_guaranteeMgr._initGuaranteeFromDB(action::dealAction),
                "init_guarantee", null, false);
        //开启执行
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    /**
     * 从数据库中初始化主数据
     * @param _handler
     */
    private void _initMainFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerTreasureHuntBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerTreasureHuntBO>()
        {
            @Override
            public void dealSuc(PlayerTreasureHuntBO _bo)
            {
                _m_dbId = _bo.getId();
                _m_stationLevel = _bo.getStationLevel();
                _m_exp = _bo.getExp();
                _m_levelRef = RefTreasureHuntStationLevel.getMgr().get(_m_stationLevel);

                _handler.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                if (getHasErr())
                {
                    _handler.onRunOver(false);
                    return;
                }

                _m_stationLevel = 1;
                _m_levelRef = RefTreasureHuntStationLevel.getMgr().get(_m_stationLevel);

                _handler.onRunOver(true);
            }
        });

    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        //初始化矿石组合
        for (TreasureHuntOreInfo oreInfo : _m_oreMgr.getOreList())
        {
            _m_compositeMgr.onGainOre(oreInfo.getRef(), oreInfo.hadReachAdvanced());
        }
    }

    @Override
    public void dispose()
    {
    }

    /**
     * 获得经验
     * @param _addExp
     * @param _context
     */
    public void addExp(long _addExp, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_addExp <= 0)
                return;

            _m_exp += _addExp;

            //检查经验是否足够升级
            if (_m_levelRef != null)
            {
                long newExp = _m_exp;
                int newLevel = _m_stationLevel;
                RefTreasureHuntStationLevel newLevelRef = _m_levelRef;

                while (newExp >= newLevelRef.level_up_need_exp)
                {
                    RefTreasureHuntStationLevel temp = RefTreasureHuntStationLevel.getMgr().get(newLevel + 1);
                    if (temp == null)
                        break;

                    newExp -= newLevelRef.level_up_need_exp;
                    newLevel++;
                    newLevelRef = temp;
                }

                _m_exp = newExp;
                _m_stationLevel = newLevel;
                _m_levelRef = newLevelRef;
            }

            //保存数据
            if (!_createBo())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("station_level", _m_stationLevel);
                updateValue.addValueObj("exp", _m_exp);
                getUSServer().getBM().getBM(PlayerTreasureHuntBO.class).update("id", _m_dbId, updateValue);
            }

            getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_057_OnTreasureStationChg(makeStationInfo()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取普通捕捉道具
     * @param _context
     */
    public Result drawNormalCaptureItem(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            PlayerLazyCD lazyCd = getUserData().getLazyCDComponent().lookupCD(RefGeneral.Ref().treasure_hunt_lazy_cd_id);
            if (lazyCd == null)
                return TreasureHuntErr.TREASURE_HUNT_NO_CAPTURE_ITEM_CAN_DRAW;

            int canDrawCount = lazyCd.getCount();
            if (canDrawCount == 0)
                return TreasureHuntErr.TREASURE_HUNT_NO_CAPTURE_ITEM_CAN_DRAW;

            NPCommonItem normalCaptureItem = RefGeneral.Ref().treasure_hunt_premium_energy_common_item;

            //检查捕捉道具数量是否超过限制
            long oriCount = getUserData().getItemCount(normalCaptureItem.getItemType(),
                    normalCaptureItem.getItemId());
            if (oriCount >= RefGeneral.Ref().treasure_hunt_premium_energy_num_limit)
                return TreasureHuntErr.TREASURE_HUNT_CAPTURE_ITEM_NUM_REACH_LIMIT;

            //减少体力
            lazyCd.descCount(canDrawCount, _context);

            //增加捕捉道具
            getUserData().gainItem(normalCaptureItem, canDrawCount, _context);

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 捕捉
     * @param _type      捕捉类型
     * @param _isAdvance 是否使用高级捕捉道具
     * @param _areaId
     * @param _context
     * @return
     */
    public ResultOne<TreasureHuntCaptureResult> capture(ETreasureHuntCaptureType _type, boolean _isAdvance, long _distance,
                                                        long _areaId, NPPlayerContext _context, TreasureAnalyseCollector _collector)
    {
        getUserData().lockUser();
        try
        {
            RefTreasureHuntStationLevel refLevel = _m_levelRef;
            if (refLevel == null)
                return ResultOne.failed(CommErr.REF_NOT_FOUND);

            RefTreasureHuntArea refArea = RefTreasureHuntArea.getMgr().get(_areaId);
            if (refArea == null)
                return ResultOne.failed(CommErr.REF_NOT_FOUND);

            if (!NPPlayerConditionDealerMgr.IsEnable(refArea.unlock_condition, getUserData(), null))
                return ResultOne.failed(CommErr.SYSTEM_UNLOCK);

            //获取cd道具数量
            long cdItemCount;
            if (_isAdvance)
            {
                cdItemCount = getUserData().getItemCount(RefGeneral.Ref().treasure_hunt_advanced_energy_common_item);
            } else
            {
                cdItemCount = getUserData().getItemCount(RefGeneral.Ref().treasure_hunt_premium_energy_common_item);
            }

            int captureTime = 1;
            long distance = _distance;
            if (_type == ETreasureHuntCaptureType.NORMAL)
            {
                if (cdItemCount < 1)
                    return ResultOne.failed(CommErr.ITEM_NOT_ENOUGH);

            } else if (_type == ETreasureHuntCaptureType.SINGLE_AKEY)
            {
                if (cdItemCount < 1)
                    return ResultOne.failed(CommErr.ITEM_NOT_ENOUGH);

                if (!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().treasure_hunt_instant_pickup_simple_unlock_id, getUserData(), null))
                    return ResultOne.failed(CommErr.SYSTEM_UNLOCK);

                distance = refLevel.max_fly_distance;

            } else if (_type == ETreasureHuntCaptureType.MULTIPLE_AKEY)
            {
                if (cdItemCount < 1)
                    return ResultOne.failed(CommErr.ITEM_NOT_ENOUGH);

                //如果是一键捕捉，则需要检查是否解锁了一键捕捉
                if (!NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().treasure_hunt_akey_pickup_simple_unlock_id, getUserData(), null))
                    return ResultOne.failed(CommErr.SYSTEM_UNLOCK);

                captureTime = (int) Math.min(cdItemCount, RefGeneral.Ref().treasure_hunt_akey_consume_energy_limit);
                distance = refLevel.max_fly_distance;

            } else if (_type == ETreasureHuntCaptureType.DATA_ANALYSE)
            {
                if (_context.getContextId() != ENPGameEvent.GM_CMD.ordinal())
                    return ResultOne.failed(CommErr.SYSTEM_UNLOCK);

                if (_collector == null)
                    return ResultOne.failed(CommErr.PARAM_ERROR);

                captureTime = _collector.getWantNum();
                refLevel = _collector.getWantLevelRef();
                distance = refLevel.max_fly_distance;
            }

            //查询对应的距离配置
            RefTreasureHuntAreaDistance refDistance = RefTreasureHuntAreaDistance.getMgr().getRefByDistance(distance);
            if (refDistance == null)
                return ResultOne.failed(CommErr.REF_NOT_FOUND);

            //计算矿石品质权重
            QualityValueData oreQualityWeightData = (_isAdvance ? refLevel.advancedEnergyWeightList : refLevel.ore_quality_weight_list).copy();
            //需要乘上距离调整系数
            oreQualityWeightData.multiply(refDistance.ore_gain_weight_adjust_per);
            //构造权重列表
            WeightQualityValueList oreQualityWeightList = oreQualityWeightData.buildWeightList();

            int realCaptureTime = 0;

            TreasureHuntCaptureResult captureResult = new TreasureHuntCaptureResult();
            for (int i = 0; i < captureTime; i++)
            {
                // 判断待处理矿石是否超过限制
                if (_m_oreMgr.getPendingOreNum() >= RefGeneral.Ref().treasure_hunt_pending_ore_num_limit && _type != ETreasureHuntCaptureType.DATA_ANALYSE)
                    break;

                //消耗道具
                if (_type != ETreasureHuntCaptureType.DATA_ANALYSE)
                {
                    boolean isSucc = getUserData().spendItem(
                            _isAdvance ? RefGeneral.Ref().treasure_hunt_advanced_energy_common_item : RefGeneral.Ref().treasure_hunt_premium_energy_common_item,
                            1, _context);
                    if (!isSucc)
                        return ResultOne.failed(CommErr.ITEM_NOT_ENOUGH);
                }

                TreasureHunt_CaptureResult result = null;
                try
                {
                    result = captureCore(refLevel, refArea, refDistance, _context, oreQualityWeightList, _collector, _isAdvance);
                } catch (Exception e)
                {
                    USLog.error(getUSServer(), "TreasureHuntComponent.capture: Exception in capture core, cid:{} type:{} isAdvance:{} areaId:{} distance:{}",
                            getUserData().getCid(), _type, _isAdvance, _areaId, _distance, e);
                }

                realCaptureTime++;
                captureResult.captureResultList.add(result);
            }

            if (_collector != null)
                _collector.addNum(realCaptureTime);

            if (realCaptureTime == 0)
                return ResultOne.failed(TreasureHuntErr.TREASURE_HUNT_PENDING_ORE_LIMIT);

            //计算经验
            int gainExp = (int) (realCaptureTime * Math.ceil(refLevel.each_pickup_exp
                    * (10000 + getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.TREASURE_HUNT_CAPTURE_GAIN_EXP_ADD_PER)) / 10000d));
            captureResult.exp = gainExp;
            //增加经验
            addExp(gainExp, _context);

            getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.TREASURE_HUNT_CAPTURE_TIMES, realCaptureTime, _context);

            // 触发事件
            getUserData().onLogicEvent(new Event_P_TREASURE_HUNT_CAPTURE(_context, realCaptureTime));

            return ResultOne.succ(captureResult);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 核心捕捉逻辑
     * @param _refLevel
     * @param _refArea
     * @param _refDistance
     * @param _context
     * @param _oreQualityWeightList
     * @param _collector
     * @param _isAdvance
     * @return
     */
    public TreasureHunt_CaptureResult captureCore(RefTreasureHuntStationLevel _refLevel, RefTreasureHuntArea _refArea, RefTreasureHuntAreaDistance _refDistance,
                                                  NPPlayerContext _context, WeightQualityValueList _oreQualityWeightList, TreasureAnalyseCollector _collector, boolean _isAdvance)
    {
        // 用于收集所有捕捉结果的JSON数组
        com.google.gson.JsonArray resultJsonArray = new com.google.gson.JsonArray();

        // 区域保底信息
        TreasureHuntGuaranteeAreaInfo guaranteeInfo = _m_guaranteeMgr.getAreaGuaranteeInfo(_refArea.Id());

        TreasureHunt_CaptureResult captureResult = new TreasureHunt_CaptureResult();
        // 计算可以抽取次数
        int captureRewardNum = 1 + _refDistance.capture_reward_num_add;
        for (int i = 0; i < captureRewardNum; i++)
        {
            // 检查是否有必定获得的奇物
            RefTreasureHuntTreasure refMustGainTreasure = _m_treasureMgr.checkHadMustGainTreasure(_refArea);
            if (refMustGainTreasure != null)
            {
                TreasureHunt_CaptureResult_Treasure treasureResult = _m_treasureMgr.gainTreasure(refMustGainTreasure, _context);
                if (treasureResult != null)
                {
                    captureResult.getCaptureRewardList().add(new TreasureHunt_CaptureReward(ETreasureHuntGainType.TREASURE, treasureResult.makePackage().array()));

                    // 构建必得奇物的JSON
                    JsonObject json = new JsonObject();
                    json.addProperty("type", 2);
                    json.addProperty("treasureId", refMustGainTreasure.Id());
                    resultJsonArray.add(json);
                }

                guaranteeInfo.resetTreasureGuaranteeCount();
                continue;
            }

            // 如果没有必定获得的奇物，则随机捕捉类型
            ETreasureHuntGainType type;
            //检查奇物保底触发条件
            int treasureWeight = _refArea.getTreasureWeight();
            if (treasureWeight > 0 && guaranteeInfo.canTriggerTreasureGuarantee())
            {
                // 如果有奇物保底触发条件且当前未获得奇物，则增加奇物权重
                type = ETreasureHuntGainType.TREASURE;
            } else
            {
                // 构造捕捉权重
                WeightValueList<ETreasureHuntGainType> weightList = new WeightValueList<>();
                weightList.add(ETreasureHuntGainType.ORE, _refLevel.ore_gain_weight);
                weightList.add(ETreasureHuntGainType.TREASURE, treasureWeight);
                weightList.add(ETreasureHuntGainType.REWARD, _refLevel.reward_gain_weight);
                // 随机捕捉类型
                type = weightList.random();
            }

            if (type == ETreasureHuntGainType.ORE)
            {
                //检查矿石是否触发矿石品质保底
                WeightQualityValueList oreGuarantee = guaranteeInfo.checkActiveOreGuarantee();

                WeightQualityValueList currentWeightList = (oreGuarantee != null) ? oreGuarantee : _oreQualityWeightList;

                //随机矿石品质
                EQuality oreQuality = currentWeightList.random();
                if (oreQuality != null)
                {
                    //随机矿石
                    TreasureHunt_CaptureResult_Ore resultOre = _m_oreMgr.randomGainOre(_refArea, oreQuality, _context);
                    if (resultOre != null)
                    {
                        captureResult.getCaptureRewardList().add(new TreasureHunt_CaptureReward(type, resultOre.makePackage().array()));

                        // 构建矿石的JSON
                        JsonObject json = new JsonObject();
                        json.addProperty("type", 1);
                        json.addProperty("oreId", resultOre.getOreId());
                        resultJsonArray.add(json);

                        if (_collector != null)
                            _collector.addAnalyseData(ETreasureHuntDataAnalyseType.ORE, oreQuality, resultOre.getOreId());
                    }

                    //记录保底次数
                    if (oreQuality.ordinal() >= RefGeneral.Ref().treasure_hunt_ore_guarantee_quality.ordinal())
                        guaranteeInfo.incrementConsecutiveHighQualityCount();
                    else
                        guaranteeInfo.incrementConsecutiveNoHighQualityCount();
                } else
                {
                    USLog.error(getUSServer(), "TreasureHuntComponent.captureCore: Failed to get ore quality from weight list, areaId: {}, distance: {}, oreQualityWeightList: {}",
                            _refArea.Id(), _refDistance.distance, currentWeightList);
                }

            } else if (type == ETreasureHuntGainType.TREASURE)
            {
                RefTreasureHuntTreasure refTreasure = _refArea.randomTreasure();
                if (refTreasure == null) {
                    USLog.error(getUSServer(), "TreasureHuntComponent.captureCore - treasure config missing: areaId={}, cid={}",
                            _refArea.Id(), getUserData().getCid());
                    continue; // 跳过本次抽取或使用默认奖励
                }

                //如果已经获得了奇物，则直接获得转换道具
                TreasureHuntTreasureInfo treasureInfo = _m_treasureMgr.lookupTreasure(refTreasure.Id());
                if (treasureInfo != null)
                {
                    NPPlayerContext context = NPPlayerContext.createNew(_context);
                    getUserData().gainItemList(RefGeneral.Ref().treasure_hunt_treasure_replace_common_item, context);

                    TreasureHunt_CaptureResult_Reward rewardResult = new TreasureHunt_CaptureResult_Reward();
                    context.getCollector().fillProtoList(rewardResult.getItemList());

                    captureResult.getCaptureRewardList().add(new TreasureHunt_CaptureReward(ETreasureHuntGainType.REWARD, rewardResult.makePackage().array()));

                    // 构建重复奇物的JSON
                    JsonObject json = new JsonObject();
                    json.addProperty("type", 3);
                    json.addProperty("treasureId", refTreasure.Id());
                    resultJsonArray.add(json);

                    if (_collector != null)
                        _collector.addAnalyseData(ETreasureHuntDataAnalyseType.TREASURE_REPEAT, refTreasure.quality, refTreasure.Id());
                } else
                {
                    //随机奇物
                    TreasureHunt_CaptureResult_Treasure treasureResult = _m_treasureMgr.gainTreasure(refTreasure, _context);
                    if (treasureResult != null)
                    {
                        captureResult.getCaptureRewardList().add(new TreasureHunt_CaptureReward(type, treasureResult.makePackage().array()));

                        // 构建新奇物的JSON
                        JsonObject json = new JsonObject();
                        json.addProperty("type", 2);
                        json.addProperty("treasureId", refTreasure.Id());
                        resultJsonArray.add(json);

                        if (_collector != null)
                            _collector.addAnalyseData(ETreasureHuntDataAnalyseType.TREASURE, refTreasure.quality, refTreasure.Id());
                    }
                }

                guaranteeInfo.resetTreasureGuaranteeCount();

            } else if (type == ETreasureHuntGainType.REWARD)
            {
                NPPlayerContext context = NPPlayerContext.createNew(_context);
                getUserData().gainReward(_refLevel.reward_id, context);

                TreasureHunt_CaptureResult_Reward rewardResult = new TreasureHunt_CaptureResult_Reward();
                context.getCollector().fillProtoList(rewardResult.getItemList());

                captureResult.getCaptureRewardList().add(new TreasureHunt_CaptureReward(type, rewardResult.makePackage().array()));

                // 构建道具奖励的JSON
                JsonObject json = new JsonObject();
                json.addProperty("type", 4);
                json.addProperty("rewardId", _refLevel.reward_id);
                resultJsonArray.add(json);

                //如果还有奇物没获得，且此次没抽到奇物，则增加保底计数
                guaranteeInfo.incrementTreasureGuaranteeCount();

                if (_collector != null)
                    _collector.addAnalyseData(ETreasureHuntDataAnalyseType.REWARD, EQuality.NONE, _refLevel.reward_id);
            }
        }

        // 将JSON数组转换为字符串
        try
        {
            String resultJson = new Gson().toJson(resultJsonArray);

            // 记录寻宝日志
            MJLog.MJEventLog.logFishRecord(
                    getUserData(),
                    _isAdvance ? 1 : 0,
                    _refArea.Id(),
                    _refDistance.distance,
                    resultJson,
                    captureResult.getCaptureRewardList().size()  // totalNum 暂时传0，可能需要根据业务逻辑计算
            );
        } catch (Exception e)
        {
            USLog.error(getUSServer(), "", e);
        }

        return captureResult;
    }


    /**
     * 创建数据库对象
     */
    private boolean _createBo()
    {
        if (_m_dbId != 0)
            return false; //如果已经有ID了，则不需要创建

        PlayerTreasureHuntBO bo = new PlayerTreasureHuntBO();
        bo.setCid(getUSServer().getBM(), getUserData().getCid());
        bo.setStationLevel(getUSServer().getBM(), _m_stationLevel);
        bo.setExp(getUSServer().getBM(), _m_exp);
        bo.insert(getUSServer().getBM());

        _m_dbId = bo.getId();
        return true;
    }

    /**
     * 设置太空舱等级（GM命令用）
     * @param _level   目标等级
     * @param _context 上下文
     */
    public void setStationLevel(int _level, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_level <= 0)
                return;

            RefTreasureHuntStationLevel newLevelRef = RefTreasureHuntStationLevel.getMgr().get(_level);
            if (newLevelRef == null)
                return;

            _m_stationLevel = _level;
            _m_levelRef = newLevelRef;
            _m_exp = 0;

            if (!_createBo())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("station_level", _m_stationLevel);
                updateValue.addValueObj("exp", _m_exp);
                getUSServer().getBM().getBM(PlayerTreasureHuntBO.class).update("id", _m_dbId, updateValue);
            }

            getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_057_OnTreasureStationChg(makeStationInfo()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造太空舱信息
     * @return
     */
    public TreasureHunt_StationInfo makeStationInfo()
    {
        TreasureHunt_StationInfo info = new TreasureHunt_StationInfo();
        info.setStationLevel(_m_stationLevel);
        info.setExp(_m_exp);
        return info;
    }

    /**
     * 构造信息
     * @return
     */
    public TreasureHunt_Info makeProto()
    {
        TreasureHunt_Info info = new TreasureHunt_Info();
        info.setStationInfo(makeStationInfo());
        _m_oreMgr.fillProto(info.getOreList());
        _m_treasureMgr.fillProto(info.getTreasureList());
        _m_compositeMgr.fillProto(info.getCompositeList());
        _m_treasureMgr.fillOutputProto(info.getTreasureOutputList());
        return info;
    }

    /**
     * 获取玩家当前的技能列表信息
     * 
     * 执行流程：
     * 1. 通过各管理器收集技能信息到统一列表中
     * 2. 按技能ID排序并格式化输出
     * 
     * @return 技能信息字符串，格式为"技能ID 等级"每行一个技能
     */
    public String getSkillInfo()
    {
        StringBuilder sb = new StringBuilder();
        
        try {
            getUserData().lockUser();
            
            // 收集所有技能信息到列表中
            List<SkillInfo> skillList = new ArrayList<>();
            
            // 1. 矿石技能统计
            _m_oreMgr.collectSkillInfo(skillList);
            
            // 2. 奇物技能统计  
            _m_treasureMgr.collectSkillInfo(skillList);
            
            // 3. 组合物品技能统计
            _m_compositeMgr.collectSkillInfo(skillList);
            
            // 按技能ID排序并格式化输出
            skillList.stream()
                    .sorted((a, b) -> Long.compare(a.skillId, b.skillId))
                    .forEach(skill -> sb.append(skill.skillId).append("  ").append(skill.level).append("\n"));
            
        } finally {
            getUserData().unlockUser();
        }
        
        return sb.toString().trim();
    }
    
    /**
     * 技能信息内部类
     */
    public static class SkillInfo
    {
        public long skillId;
        public int level;

        public SkillInfo(long skillId, int level)
        {
            this.skillId = skillId;
            this.level = level;
        }
    }

    /**
     * 设置矿石技能等级（GM命令）
     * @param _oreId 矿石ID
     * @param _isAdvanced 是否高级技能
     * @param _level 目标等级
     * @param _context 上下文
     */
    public void setOreSkillLevel(long _oreId, boolean _isAdvanced, int _level, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            TreasureHuntOreInfo oreInfo = _m_oreMgr.lookupOre(_oreId);
            if (oreInfo == null)
                return;

            oreInfo.setSkillLevel(_isAdvanced, _level, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 重置矿石高级技能到未解锁状态（GM命令）
     * @param _oreId 矿石ID
     * @param _context 上下文
     */
    public void resetOreAdvancedSkill(long _oreId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            TreasureHuntOreInfo oreInfo = _m_oreMgr.lookupOre(_oreId);
            if (oreInfo == null)
                return;

            oreInfo.resetAdvancedSkill(_context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 设置奇物技能等级（GM命令）
     * @param _treasureId 奇物ID
     * @param _level 目标等级
     * @param _context 上下文
     */
    public void setTreasureSkillLevel(long _treasureId, int _level, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            TreasureHuntTreasureInfo treasureInfo = _m_treasureMgr.lookupTreasure(_treasureId);
            if (treasureInfo == null)
                return;

            treasureInfo.setSkillLevel(_level, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 重置组合高级技能到未激活状态（GM命令）
     * @param _compositeId 组合ID
     * @param _context 上下文
     */
    public void resetCompositeAdvancedSkill(long _compositeId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            TreasureHuntCompositeInfo compositeInfo = _m_compositeMgr.lookupComposite(_compositeId);
            if (compositeInfo == null || compositeInfo.getBo() == null)
                return;

            compositeInfo.setSkillLevel(true, 0, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 设置矿石技能点数（GM命令）
     * @param _oreId 矿石ID
     * @param _isAdvanced 是否高级技能点数
     * @param _point 技能点数
     * @param _context 上下文
     */
    public void setOreSkillPoint(long _oreId, boolean _isAdvanced, int _point, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            TreasureHuntOreInfo oreInfo = _m_oreMgr.lookupOre(_oreId);
            if (oreInfo == null)
                return;

            oreInfo.setSkillPoint(_isAdvanced, _point, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }
}

