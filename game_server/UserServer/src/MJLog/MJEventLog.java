package MJLog;

import MJLog.EventBo.*;
import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.MJEventLogBo;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.UserServerConf;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

/**
 * 梦加事件日志类
 */
public class MJEventLog
{
    private static final Log log = LogFactory.getLog(MJEventLog.class);

    /**
     * 构造基础信息
     * @param _userdata 玩家信息
     * @param _bo       日志对象
     */
    private static void _makeBaseInfo(NPUSUserData _userdata, MJEventLogBo _bo)
    {
        BM bmObj = _userdata.getUSServer().getBM();
        _bo.setCid(bmObj, _userdata.getCid());
        _bo.setUid(bmObj, _userdata.getUid());
        _bo.setVipLv(bmObj, (int) _userdata.getParam(ENPPlayerParam.VIP_LVL));
        _bo.setServerId(bmObj, _userdata.getUSServer().getServerTypeId());
        _bo.setPlatform(bmObj, UserServerConf.getInstance().getPlatformId());
        _bo.setRegion(bmObj, UserServerConf.getInstance().getPlatAreaId());
        _bo.setTimestamp(bmObj, CommonFunc.getNowTimeSec());
        _bo.setCreateTime(bmObj, _userdata.getPlayerComponent().getBo().getCreateTime());
    }

    /**
     * 任务完成日志
     * @param _userdata
     * @param _taskIdList
     */
    public static void logTask(NPUSUserData _userdata, int _taskType, String _taskIdList)
    {
        MjTaskLogBO logBo = new MjTaskLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setTaskType(_userdata.getUSServer().getBM(), _taskType);
        logBo.setTaskIdList(_userdata.getUSServer().getBM(), _taskIdList);
        logBo.insert(_userdata.getUSServer().getBM());
    }

    /**
     * 伙伴资质&经营技能变更日志
     * @param _userdata   玩家数据
     * @param _heroId     伙伴id
     * @param _heroLevel  伙伴等级
     * @param _trainType  培养类型：1=资质；2=经营技能；3=等级
     * @param _event      事件id（context.getContextId()）
     * @param _beforeAttr 变更前的属性值
     * @param _finalAttr  变更后的属性值
     */
    public static void logFellowAptitudeSkill(NPUSUserData _userdata, long _heroId, int _heroLevel,
                                              int _trainType, int _event,
                                              int _beforeAttr, int _finalAttr)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjFellowAptitudeSkillLogBO logBo = new MjFellowAptitudeSkillLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setFid(bmObj, _heroId);
        logBo.setFLv(bmObj, _heroLevel);
        logBo.setTrainType(bmObj, _trainType);
        logBo.setEvent(bmObj, _event);
        logBo.setBeforeAttr(bmObj, _beforeAttr);
        logBo.setFinalAttr(bmObj, _finalAttr);

        logBo.insert(bmObj);
    }

    /**
     * 情人亲密度&吸引力&加护力变更日志
     * @param _userdata   玩家数据
     * @param _consortId  情人id
     * @param _trainType  培养类型：1=亲密度；2=吸引力；3=加护力；4=羁绊升级；5=星辉升级
     * @param _event      事件id（context.getContextId()）
     * @param _beforeAttr 变更前的属性值
     * @param _finalAttr  变更后的属性值
     */
    public static void logConsortAttribute(NPUSUserData _userdata, long _consortId,
                                           int _trainType, int _event,
                                           int _beforeAttr, int _finalAttr)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjConsortAttributeLogBO logBo = new MjConsortAttributeLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setLid(bmObj, _consortId);
        logBo.setTrainType(bmObj, _trainType);
        logBo.setEvent(bmObj, _event);
        logBo.setBeforeAttr(bmObj, _beforeAttr);
        logBo.setFinalAttr(bmObj, _finalAttr);

        logBo.insert(bmObj);
    }

    /**
     * 情人洗练日志
     * @param _userdata   玩家数据
     * @param _consortId  情人id
     * @param _intimacy   情人亲密度
     * @param _refineType 洗练类型：1=普通领悟；2=高级领悟
     * @param _attrId     本次洗练的属性id
     * @param _isSucceed  本次洗练是否成功：1=成功；0=未成功
     * @param _beforeAttr 洗练前属性值（加成万分比）
     * @param _finalAttr  洗练后属性值（加成万分比）
     * @param _event      事件id（context.getContextId()）
     */
    public static void logLoverRefine(NPUSUserData _userdata, long _consortId, long _intimacy,
                                      int _refineType, long _attrId, int _isSucceed,
                                      int _beforeAttr, int _finalAttr, int _event)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjLoverRefineLogBO logBo = new MjLoverRefineLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setLid(bmObj, _consortId);
        logBo.setIntimacy(bmObj, _intimacy);
        logBo.setRefineType(bmObj, _refineType);
        logBo.setAttrId(bmObj, _attrId);
        logBo.setIsSucceed(bmObj, _isSucceed);
        logBo.setBeforeAttr(bmObj, _beforeAttr);
        logBo.setFinalAttr(bmObj, _finalAttr);
        logBo.setEvent(bmObj, _event);

        logBo.insert(bmObj);
    }
    
    /**
     * 子嗣数据日志
     * @param _userdata 玩家数据
     * @param _uniqeId 子嗣的唯一ID，创建子嗣的时候赋予的唯一ID
     * @param _childId 子嗣ID，与数值策划的配表一致
     * @param _attribute 子嗣当前的属性值(收益/秒），刷新
     * @param _loversId 子嗣对应的情人ID
     * @param _childEp 子嗣当前的培养进度(经验值），刷新
     * @param _stateId 1 = 获取、2 = 取名、3 = 升级、4 = 成年、5 = 已婚、6 = 流放
     * @param _sexId 子嗣性别，男、女
     * @param _quality 子嗣的品质，如A、B+、B-等
     * @param _time 获得该子嗣的时间戳
     */
    public static void logChild(NPUSUserData _userdata, long _uniqeId, long _childId,
                                      long _attribute, long _loversId, long _childEp,
                                      int _stateId, int _sexId, long _quality, int _time)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjChildLogBO logBo = new MjChildLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setUniqeId(bmObj, _uniqeId);
        logBo.setChildId(bmObj, _childId);
        logBo.setAttribute(bmObj, _attribute);
        logBo.setLoversid(bmObj, _loversId);
        logBo.setChildEp(bmObj, _childEp);
        logBo.setStateid(bmObj, _stateId);
        logBo.setSexId(bmObj, _sexId);
        logBo.setQuality(bmObj, _quality);
        logBo.setTime(bmObj, _time);

        logBo.insert(bmObj);
    }
    
    /**
     * 建筑升级&员工雇佣日志
     * @param _userdata      玩家数据
     * @param _buildingId    建筑id
     * @param _buildingLevel 建筑等级（操作时的等级，即升级前）
     * @param _trainType     培养类型：1=雇佣员工；2=提升建筑等级
     * @param _beforeAttr    变更前的属性值（员工数/建筑等级）
     * @param _finalAttr     变更后的属性值（员工数/建筑等级）
     */
    public static void logBuildUpdate(NPUSUserData _userdata, long _buildingId, int _buildingLevel,
                                      int _trainType,
                                      int _beforeAttr, int _finalAttr)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjBuildUpdateLogBO logBo = new MjBuildUpdateLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setBid(bmObj, _buildingId);
        logBo.setBLevel(bmObj, _buildingLevel);
        logBo.setTrainType(bmObj, _trainType);
        logBo.setBeforeAttr(bmObj, _beforeAttr);
        logBo.setFinalAttr(bmObj, _finalAttr);

        logBo.insert(bmObj);
    }

    /**
     * 藏品洗练日志
     * @param _userdata      玩家数据
     * @param _equipId       藏品id（配表id）
     * @param _equipDbId     藏品唯一id（数据库id）
     * @param _equipLevel    藏品等级
     * @param _equipAptitude 藏品资质点
     * @param _heroId        伙伴id，无伙伴则记0
     * @param _opType        操作类型：1=洗练
     * @param _isSucceed     本次洗练是否成功：1=成功；0=未成功
     * @param _beforeAttr    变更前的属性值（加成万分比）
     * @param _finalAttr     变更后的属性值（加成万分比）
     */
    public static void logArtifactRefine(NPUSUserData _userdata, long _equipId, long _equipDbId,
                                         int _equipLevel, int _equipAptitude, long _heroId,
                                         int _opType, int _isSucceed,
                                         int _beforeAttr, int _finalAttr)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjArtifactRefineLogBO logBo = new MjArtifactRefineLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setAid(bmObj, _equipId);
        logBo.setCollectionId(bmObj, _equipDbId);
        logBo.setALevel(bmObj, _equipLevel);
        logBo.setAAptitude(bmObj, _equipAptitude);
        logBo.setFid(bmObj, _heroId);
        logBo.setOpType(bmObj, _opType);
        logBo.setIsSucceed(bmObj, _isSucceed);
        logBo.setInitialAttr(bmObj, _beforeAttr);
        logBo.setFinalAttr(bmObj, _finalAttr);

        logBo.insert(bmObj);
    }

    /**
     * 藏品升级日志
     * @param _userdata        玩家数据
     * @param _equipId         藏品id（配表id）
     * @param _equipDbId       藏品唯一id（数据库id）
     * @param _heroId          伙伴id，无伙伴则记0
     * @param _initialLv       升级前的等级
     * @param _initialAptitude 升级前的资质点
     * @param _finalLv         升级后的等级
     * @param _finalAptitude   升级后的资质点
     */
    public static void logArtifactUpdate(NPUSUserData _userdata, long _equipId, long _equipDbId,
                                         long _heroId, int _initialLv, int _initialAptitude,
                                         int _finalLv, int _finalAptitude)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjArtifactUpdateLogBO logBo = new MjArtifactUpdateLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setAid(bmObj, _equipId);
        logBo.setCollectionId(bmObj, _equipDbId);
        logBo.setFid(bmObj, _heroId);
        logBo.setInitialLv(bmObj, _initialLv);
        logBo.setInitialAptitude(bmObj, _initialAptitude);
        logBo.setFinalLv(bmObj, _finalLv);
        logBo.setFinalAptitude(bmObj, _finalAptitude);

        logBo.insert(bmObj);
    }

    /**
     * 记录幸运转盘抽卡日志
     * @param _userdata 玩家数据
     * @param _amount   抽卡数量（1=单抽，10=十连抽）
     * @param _product  抽卡产出物，json格式：{"抽取次数1":物品id,"抽取次数2":物品id,...}
     */
    public static void logWish(NPUSUserData _userdata, int _amount, String _product)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjWishLogBO logBo = new MjWishLogBO();

        // 使用 _makeBaseInfo 填充基础信息
        _makeBaseInfo(_userdata, logBo);

        // 设置业务字段
        logBo.setAmount(bmObj, _amount);
        logBo.setProduct(bmObj, _product);

        logBo.insert(bmObj);
    }

    /**
     * 旅店货舱/清单/礼物解锁日志
     * @param _userdata 玩家数据
     * @param _deviceId 货舱/清单/礼物id
     */
    public static void logInnUnlock(NPUSUserData _userdata, long _deviceId)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjInnUnlockLogBO logBo = new MjInnUnlockLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setDeviceId(bmObj, _deviceId);

        logBo.insert(bmObj);
    }

    /**
     * 旅店货舱/清单/礼物/勋章升级日志
     * @param _userdata 玩家数据
     * @param _deviceId 货舱/清单/礼物/勋章id（勋章为1）
     * @param _beforeLv 升级前等级
     * @param _afterLv  升级后等级
     */
    public static void logInnUpdate(NPUSUserData _userdata, long _deviceId, int _beforeLv, int _afterLv)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjInnUpdateLogBO logBo = new MjInnUpdateLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setDeviceId(bmObj, _deviceId);
        logBo.setBeforeLv(bmObj, _beforeLv);
        logBo.setAfterLv(bmObj, _afterLv);

        logBo.insert(bmObj);
    }

    /**
     * 爬塔采掘进度日志
     * @param _userdata        玩家数据
     * @param _triggerIdBefore 采掘进度_挑战前
     * @param _isWin           是否成功（1=成功；0=失败）
     * @param _triggerIdFinal  采掘进度_挑战后
     */
    public static void logClimbTower(NPUSUserData _userdata, int _triggerIdBefore,
                                     int _isWin, int _triggerIdFinal)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjClimbTowerLogBO logBo = new MjClimbTowerLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setTriggeridBefore(bmObj, _triggerIdBefore);
        logBo.setIsWin(bmObj, _isWin);
        logBo.setTriggeridFinal(bmObj, _triggerIdFinal);

        logBo.insert(bmObj);
    }

    /**
     * 宴会参与日志
     * @param _userdata  玩家数据
     * @param _partyType 舞会类型
     * @param _action    参与类别：1=参与；2=举办
     */
    public static void logParty(NPUSUserData _userdata, long _partyType, int _action)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjPartyLogBO logBo = new MjPartyLogBO();

        _makeBaseInfo(_userdata, logBo);
        // 设置业务字段
        logBo.setPartyType(bmObj, _partyType);
        logBo.setAction(bmObj, _action);

        logBo.insert(bmObj);
    }

    /**
     * 矿石处理及升级日志
     * @param _userdata              玩家数据
     * @param _oreId                 矿石id（配表id）
     * @param _oreQa                 矿石品质
     * @param _researchPoint         养成点数
     * @param _advancedResearchPoint 高级养成点数
     */
    public static void logOre(NPUSUserData _userdata, long _oreId, int _oreQa,
                              long _researchPoint, int _advancedResearchPoint)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjOreLogBO logBo = new MjOreLogBO();

        _makeBaseInfo(_userdata, logBo);
        logBo.setOreId(bmObj, _oreId);
        logBo.setOreQa(bmObj, _oreQa);
        String researchPoint = String.format("{normal:%d,advanced:%d}", _researchPoint, _advancedResearchPoint);
        logBo.setResearchPoint(bmObj, researchPoint);

        logBo.insert(bmObj);
    }

    /**
     * 碰撞测试挑战日志
     * @param _userdata     玩家数据
     * @param _fid          伙伴id
     * @param _rivalCid     对手cid
     * @param _rivalHp      对手总血量
     * @param _killNum      击败对手伙伴数量
     * @param _initialHp    起始血量
     * @param _finalHp      剩余血量
     * @param _buff         增益明细，格式：{buff_id:使用次数,...}
     * @param _chgScore     变更积分
     */
    public static void logArenaBattle(NPUSUserData _userdata, long _fid, long _rivalCid,
                                      long _rivalHp, int _killNum, long _initialHp, long _finalHp,
                                      String _buff, long _chgScore)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjArenaBattleLogBO logBo = new MjArenaBattleLogBO();

        _makeBaseInfo(_userdata, logBo);
        logBo.setFid(bmObj, _fid);
        logBo.setRivalCid(bmObj, _rivalCid);
        logBo.setRivalHp(bmObj, _rivalHp);
        logBo.setKillNum(bmObj, _killNum);
        logBo.setInitialHp(bmObj, _initialHp);
        logBo.setFinalHp(bmObj, _finalHp);
        logBo.setBuffList(bmObj, _buff);
        logBo.setChgScore(bmObj, _chgScore);

        logBo.insert(bmObj);
    }

    /**
     * 游历次数记录日志
     * @param _userdata    玩家数据
     * @param _travelCount 游历次数
     * @param _eventId     事件ID
     */
    public static void logTravel(NPUSUserData _userdata, int _travelCount, int _eventId)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjTravelLogBO logBo = new MjTravelLogBO();

        _makeBaseInfo(_userdata, logBo);
        logBo.setTravelCount(bmObj, _travelCount);
        logBo.setEventId(bmObj, _eventId);

        logBo.insert(bmObj);
    }

    /**
     * 太空运输订单结算日志
     * @param _userdata     玩家数据
     * @param _num          订单数量
     * @param _ratingNum    完成该订单获得的人气值
     * @param _blueprintNum 完成该订单获得的图纸
     * @param _gratitudeNum 完成该订单获得的心意值
     */
    public static void logInnOrderSettle(NPUSUserData _userdata, int _num,
                                         long _ratingNum, long _blueprintNum, long _gratitudeNum)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjInnOrderLogBO logBo = new MjInnOrderLogBO();

        _makeBaseInfo(_userdata, logBo);
        logBo.setNum(bmObj, _num);
        logBo.setRatingNum(bmObj, _ratingNum);
        logBo.setBlueprintNum(bmObj, _blueprintNum);
        logBo.setGratitudeNum(bmObj, _gratitudeNum);

        logBo.insert(bmObj);
    }

    /**
     * 寻宝图鉴记录日志
     * @param _userdata    玩家数据
     * @param _type        物品类型：1=矿石；2=奇物；3=组合
     * @param _id          物品id
     * @param _isAdvanced  是否高级：1=是；2=否
     * @param _actionType  操作类型：1=解锁；2=升级
     * @param _beforeLevel 升级前等级，未解锁为0；eg: {normal:1,advanced:0}
     * @param _newLevel    升级后等级；eg: {normal:2,advanced:0}
     * @param _event       事件id（context.getContextId()）
     */
    public static void logFishItemRecord(NPUSUserData _userdata, int _type, long _id,
                                         int _isAdvanced, int _actionType,
                                         String _beforeLevel, String _newLevel, int _event)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjFishItemRecordLogBO logBo = new MjFishItemRecordLogBO();

        _makeBaseInfo(_userdata, logBo);
        logBo.setType(bmObj, _type);
        logBo.setItemId(bmObj, _id);
        logBo.setIsAdvanced(bmObj, _isAdvanced);
        logBo.setActionType(bmObj, _actionType);
        logBo.setBeforeLevel(bmObj, _beforeLevel);
        logBo.setNewLevel(bmObj, _newLevel);
        logBo.setEvent(bmObj, _event);

        logBo.insert(bmObj);
    }

    /**
     * 藏品重塑日志
     * @param _userdata    玩家数据
     * @param _recycleNum  本次重塑的藏品数量
     * @param _recycleDict 重塑明细JSON字符串
     */
    public static void logArtifactRecycle(NPUSUserData _userdata, int _recycleNum, String _recycleDict)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjArtifactRecycleLogBO logBo = new MjArtifactRecycleLogBO();
        _makeBaseInfo(_userdata, logBo);
        logBo.setRecycleNum(bmObj, _recycleNum);
        logBo.setRecycleDict(bmObj, _recycleDict);

        logBo.insert(bmObj);
    }

    /**
     * 寻宝记录日志
     * @param _userdata     玩家数据
     * @param _isAdvanced   是否高级寻宝(1=是,2=否)
     * @param _areaId       本次飞行所在的飞行点
     * @param _flyDistance  飞行距离
     * @param _result       寻宝结果JSON字符串
     * @param _totalNum     累计获得该矿石数量
     */
    public static void logFishRecord(NPUSUserData _userdata, int _isAdvanced,
                                     long _areaId, long _flyDistance,
                                     String _result, long _totalNum)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        MjFishRecordLogBO logBo = new MjFishRecordLogBO();

        // 使用 _makeBaseInfo 填充基础信息
        _makeBaseInfo(_userdata, logBo);

        // 设置寻宝详情
        logBo.setIsAdvanced(bmObj, _isAdvanced);
        logBo.setAreaId(bmObj, _areaId);
        logBo.setFlyDistance(bmObj, _flyDistance);
        logBo.setResultList(bmObj, _result);
        logBo.setTotalNum(bmObj, _totalNum);

        logBo.insert(bmObj);
    }

}
