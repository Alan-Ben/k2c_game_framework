package MJLog;

import CommonEnum.ECurrency;
import MJLog.Bo.*;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.UserServerConf;
import com.google.gson.JsonObject;

/**
 * 
 * 梦加运营日志需求
 * https://alidocs.dingtalk.com/i/nodes/R1zknDm0WR3jEXdNs1L29O62VBQEx5rG
 * 
 */
public class MJLog
{
    /**
     * 返回SoleId，项目唯一角色id
     * 由平台id和玩家cid拼接
     * @param _userdata
     * @return
     */
    private static String getSoleId(NPUSUserData _userdata)
    {
        return UserServerConf.getInstance().getPlatformId() + "-" + _userdata.getCid();
    }

    /**
     * 物品日志
     * @param _userdata
     * @param _itemType
     * @param _itemId
     * @param _action
     * @param _oldNum
     * @param _exchange
     * @param _finalNum
     * @param _event
     */
    public static void logItemChg(NPUSUserData _userdata, ENPItemType _itemType, long _itemId, int _action
            , long _oldNum, long _exchange, long _finalNum, int _event)
    {
        //当前时间戳
        int nowTimeSec = CommonFunc.getNowTimeSec();

        BM bmObj = _userdata.getUSServer().getBM();

        BaseItemLogBO logBo = new BaseItemLogBO();
        logBo.setSoleId(bmObj, getSoleId(_userdata));
        logBo.setUid(bmObj, _userdata.getUid());
        logBo.setCid(bmObj, _userdata.getCid());
        logBo.setItemType(bmObj, _itemType.ordinal());
        logBo.setItemId(bmObj, (int) _itemId);
        logBo.setItem(bmObj, String.format("%s-%d", _itemType.toString().toLowerCase(), _itemId));
        logBo.setAction(bmObj, _action);
        logBo.setOldNumber(bmObj, _oldNum);
        logBo.setExchange(bmObj, _exchange);
        logBo.setFinalNumber(bmObj, _finalNum);
        logBo.setEvent(bmObj, _event);
        logBo.setTimestamp(bmObj, nowTimeSec);
        logBo.setServerId(bmObj, _userdata.getUSServer().getServerTypeId());
        logBo.setPlatform(bmObj, UserServerConf.getInstance().getPlatformId());
        logBo.setRegion(bmObj, String.valueOf(UserServerConf.getInstance().getPlatAreaId()));
        logBo.setLevel(bmObj, (int) _userdata.getParam(ENPPlayerParam.LEVEL));

        /**
         * vip_lv：记录时的vip等级
			{"vip_lv":0}
         */
        JsonObject jo = new JsonObject();
        jo.addProperty("vip_lv", _userdata.getParam(ENPPlayerParam.VIP_LVL));
        logBo.setExt(bmObj, jo.toString());
        logBo.setDateTime(bmObj, CommonFunc.getNowTagYYYYMMDD());
        
        logBo.insert(bmObj);
    }

    /**
     * 玩家登录日志
     * @param _userdata
     * @param _operation
     */
    public static void logLogin(NPUSUserData _userdata, int _operation)
    {
        //当前时间戳
        int nowTimeSec = CommonFunc.getNowTimeSec();

        BM bmObj = _userdata.getUSServer().getBM();

        BaseLoginLogBO logBo = new BaseLoginLogBO();
        logBo.setSoleId(bmObj, getSoleId(_userdata));
        logBo.setUid(bmObj, _userdata.getUid());
        logBo.setCid(bmObj, _userdata.getCid());
        logBo.setOperation(bmObj, _operation);
        logBo.setLoginIp(bmObj, _userdata.getSdkInfo().clientIp);
        logBo.setTimestamp(bmObj, nowTimeSec);
        logBo.setVersion(bmObj, _userdata.getSdkInfo().version);
        logBo.setAdid(bmObj, _userdata.getSdkInfo().adid);
        logBo.setServerId(bmObj, _userdata.getUSServer().getServerTypeId());
        logBo.setPlatform(bmObj, UserServerConf.getInstance().getPlatformId());
        logBo.setRegion(bmObj, String.valueOf(UserServerConf.getInstance().getPlatAreaId()));
        //扩展字段：json格式
        /**
         * vip_lv：记录时的vip等级
			lv：记录时的游戏等级
			crystal：记录时的钻石
			nation：国家
			adfrom2：玩家登陆时候的实时adfrom2的值
			{"vip_lv":0,"lv":1,"crystal":110,"nation":"KR","adfrom2":"com.andorid.id1"}
         */
        JsonObject jo = new JsonObject();
        jo.addProperty("vip_lv", _userdata.getParam(ENPPlayerParam.VIP_LVL));
        jo.addProperty("lv", _userdata.getParam(ENPPlayerParam.LEVEL));
        jo.addProperty("crystal", _userdata.getCurrencyComponent().getItemCount(ECurrency.GEM.ordinal()));
        jo.addProperty("nation", _userdata.getSdkInfo().nation);
        jo.addProperty("adfrom2", _userdata.getSdkInfo().adfrom2);
        logBo.setExt(bmObj, jo.toString());
        logBo.setDateTime(bmObj, CommonFunc.getNowTagYYYYMMDD());
        logBo.setMainQuestProgress(bmObj, _userdata.getQuestComponent().getMainQuestId());
        logBo.setChapterProgress(bmObj, _userdata.getChapterComponent().getChapterInfo().getChapterProgress());
        logBo.setLevel(bmObj, (int) _userdata.getParam(ENPPlayerParam.LEVEL));
        logBo.setCrystal(bmObj,_userdata.getCurrencyComponent().getItemCount(ECurrency.GEM.ordinal()));
        logBo.setCreateDate(bmObj, _userdata.getPlayerComponent().getBo().getCreateRoleDate());
        logBo.setTotalPower(bmObj, _userdata.getHeroComponent().getTotalPower());
        logBo.setEarnings(bmObj, _userdata.getPlayerComponent().getEarnings());
        logBo.insert(bmObj);
    }

    /**
     * 在线玩家数量日志
     * @param _onlineNum
     * @param _onlineAndroidNum
     * @param _onlineIosNum
     */
    public static void logOnlineNum(NPUserServer _server, int _onlineNum, int _onlineAndroidNum, int _onlineIosNum)
    {
        BM bmObj = _server.getBM();

        BaseOnlineNumLogBO logBo = new BaseOnlineNumLogBO();
        logBo.setOnlineNum(bmObj, _onlineNum);
        logBo.setOnlineAndroid(bmObj, _onlineAndroidNum);
        logBo.setOnlineIos(bmObj, _onlineIosNum);
        logBo.setTimestamp(bmObj, CommonFunc.getNowTimeSec());
        logBo.setServerId(bmObj, _server.getServerTypeId());
        logBo.setPlatform(bmObj, UserServerConf.getInstance().getPlatformId());
        logBo.setRegion(bmObj, String.valueOf(UserServerConf.getInstance().getPlatAreaId()));
        logBo.setExt(bmObj, "");
        logBo.setDateTime(bmObj, CommonFunc.getNowTagYYYYMMDD());
        logBo.insert(bmObj);
    }

    /**
     * 玩家注册信息日志
     * @param _userdata
     */
    public static void logUserCreate(NPUSUserData _userdata)
    {
        BM bmObj = _userdata.getUSServer().getBM();

        int nowTimeSec = CommonFunc.getNowTimeSec();

        BaseUserInfoBO logBo = new BaseUserInfoBO();
        logBo.setSoleId(bmObj, getSoleId(_userdata));
        logBo.setUid(bmObj, _userdata.getUid());
        logBo.setCid(bmObj, _userdata.getCid());
        logBo.setName(bmObj, _userdata.getPlayerComponent().getName());
        logBo.setArTime(bmObj, _userdata.getPlayerComponent().getBo().getCreateTime());
        logBo.setCreateTime(bmObj, nowTimeSec);
        logBo.setArIp(bmObj, _userdata.getSdkInfo().clientIp);
        logBo.setAfid(bmObj, _userdata.getSdkInfo().afid);
        logBo.setAdid(bmObj, _userdata.getSdkInfo().adid);
        logBo.setVersion(bmObj, _userdata.getSdkInfo().version);
        logBo.setNation(bmObj, _userdata.getSdkInfo().nation);
        logBo.setAdfrom(bmObj, _userdata.getSdkInfo().adfrom);
        logBo.setAdfrom2(bmObj, _userdata.getSdkInfo().adfrom2);
        logBo.setServerId(bmObj, _userdata.getUSServer().getServerTypeId());
        logBo.setPlatform(bmObj, UserServerConf.getInstance().getPlatformId());
        logBo.setRegion(bmObj, String.valueOf(UserServerConf.getInstance().getPlatAreaId()));
        logBo.setExt(bmObj, "");
        logBo.setDateTime(bmObj, CommonFunc.getNowTagYYYYMMDD());
        logBo.insert(bmObj);
    }

    /**
     * 充值日志
     * @param _userdata    玩家数据
     * @param _orderId     订单号
     * @param _tpOrderId   第三方渠道返回的订单号
     * @param _sdkOrderId  SDK订单号
     * @param _money       配表取美元定价
     * @param _goodsId     礼包ID
     * @param _coinNum     货币数量 默认为1
     * @param _status      状态 默认为1，充值=1，充值回退=2
     * @param _payTime     充值中心 支付时间戳（秒）
     * @param _arriveTime  项目组实际收到时间戳（秒）
     * @param _timeStamp   订单创建时间（秒）
     * @param _sdkType     订单类型 1:正常订单, 2:补单, 3:虚拟充值订单,4:测试订单,6：怀疑代充订单（ios）,99:使用代币购买
     * @param _payId       支付方式 SDK推送的"支付ID(pay_id)"；使用代币购买商品时，记为：“gametoken”
     * @param _payMoney    本币金额（真实付费金额或代币券数量）
     * @param _payCurrency 货币类型（如USD、voucher等）
     * @param _orderType   订单来源类型 来源于内购还是网页充值：对应充值回调中的（order_type）
     */
    public static void logRecharge(NPUSUserData _userdata, String _orderId, String _tpOrderId, String _sdkOrderId,
                                   float _money, String _goodsId, int _coinNum, int _status, int _payTime,
                                   int _arriveTime, int _timeStamp, short _sdkType, String _payId,
                                   float _payMoney, String _payCurrency, short _orderType)
    {

        BM bmObj = _userdata.getUSServer().getBM();

        BaseRechargeLogBO logBo = new BaseRechargeLogBO();
        logBo.setSoleId(bmObj, getSoleId(_userdata));
        logBo.setUid(bmObj, _userdata.getUid());
        logBo.setCid(bmObj, _userdata.getCid());
        logBo.setOrderId(bmObj, _orderId);
        logBo.setTpOrderId(bmObj, _tpOrderId);
        logBo.setSdkorderId(bmObj, _sdkOrderId);
        logBo.setMoney(bmObj, _money);
        logBo.setCnMoney(bmObj, 0); //默认为0
        logBo.setGoodsId(bmObj, _goodsId);
        logBo.setCoinNum(bmObj, _coinNum);
        logBo.setStatus(bmObj, _status);
        logBo.setPayTime(bmObj, _payTime);
        logBo.setArriveTime(bmObj, _arriveTime);
        logBo.setTimestamp(bmObj, _timeStamp);
        logBo.setAdid(bmObj, _userdata.getSdkInfo().adid);
        logBo.setServerId(bmObj, _userdata.getUSServer().getServerTypeId());
        logBo.setPlatform(bmObj, UserServerConf.getInstance().getPlatformId());
        logBo.setRegion(bmObj, String.valueOf(UserServerConf.getInstance().getPlatAreaId()));
        logBo.setSdkType(bmObj, _sdkType);
        logBo.setPayId(bmObj, _payId);
        logBo.setDateTime(bmObj, CommonFunc.getNowTagYYYYMMDD());

        /**
         * ext扩展字段：json格式
         * vip_lv：记录时的vip等级
         * lv：记录时的游戏等级
         * version：记录时的版本号
         * crystal：记录时的钻石数量
         * pay_money：本币金额（真实付费金额或代币券数量）
         * pay_currency：货币类型（如USD、voucher等）
         * nation：国家代码
         * order_type：订单来源类型
         * rt_adfrom2：充值时的实时adfrom2
         * {\"vip_lv\":12,\"lv\":28,\"version\":\"2.3.7.0\",\"crystal\":26917,\"pay_money\":0.99,\"pay_currency\":\"USD\",
         * \"nation\":\"CN\",\"order_type\":\"1\",\"rt_adfrom2\":\"aos_com.mechanist.com.mjggpt\"}
         */
        JsonObject jo = new JsonObject();
        jo.addProperty("vip_lv", _userdata.getParam(ENPPlayerParam.VIP_LVL));
        jo.addProperty("lv", _userdata.getParam(ENPPlayerParam.LEVEL));
        jo.addProperty("version", _userdata.getSdkInfo().version);
        jo.addProperty("crystal", _userdata.getCurrencyComponent().getItemCount(ECurrency.GEM.ordinal()));
        jo.addProperty("pay_money", _payMoney);
        jo.addProperty("pay_currency", _payCurrency);
        jo.addProperty("nation", _userdata.getSdkInfo().nation);
        jo.addProperty("order_type", _orderType);
        jo.addProperty("rt_adfrom2", _userdata.getSdkInfo().adfrom2);
        logBo.setExt(bmObj, jo.toString());

        logBo.insert(bmObj);
    }

    /**
     * 钻石充值日志
     * @param _userdata    玩家数据
     * @param _goodsId     商品id
     * @param _event       事件ID
     * @param _oldNumber   旧值
     * @param _finalNumber 新值
     * @param _exchange    变化值
     * @param _sdkType     订单类型
     * @param _action      操作类型（1=获得,2=消耗）
     */
    public static void logRechargeDiamond(NPUSUserData _userdata, long _goodsId, int _event,
                                          long _oldNumber, long _finalNumber, long _exchange,
                                          short _sdkType, int _action)
    {
        BM bmObj = _userdata.getUSServer().getBM();
        int nowTimeSec = CommonFunc.getNowTimeSec();

        RechargeDiamondLogBO logBo = new RechargeDiamondLogBO();
        logBo.setUid(bmObj, _userdata.getUid());
        logBo.setCid(bmObj, _userdata.getCid());
        logBo.setServerId(bmObj, _userdata.getUSServer().getServerTypeId());
        logBo.setPlatform(bmObj, UserServerConf.getInstance().getPlatformId());
        logBo.setRegion(bmObj, String.valueOf(UserServerConf.getInstance().getPlatAreaId()));
        logBo.setLv(bmObj, (int) _userdata.getParam(ENPPlayerParam.LEVEL));
        logBo.setVipLv(bmObj, (int) _userdata.getParam(ENPPlayerParam.VIP_LVL));
        logBo.setArTime(bmObj, _userdata.getPlayerComponent().getBo().getCreateTime());
        logBo.setNation(bmObj, _userdata.getSdkInfo().nation);
        logBo.setVersion(bmObj, _userdata.getSdkInfo().version);
        logBo.setAdid(bmObj, _userdata.getSdkInfo().adid);
        logBo.setGoodsId(bmObj, Long.toString(_goodsId));
        logBo.setEvent(bmObj, _event);
        logBo.setOldNumber(bmObj, _oldNumber);
        logBo.setFinalNumber(bmObj, _finalNumber);
        logBo.setExchange(bmObj, _exchange);
        logBo.setSdkType(bmObj, _sdkType);
        logBo.setAction(bmObj, _action);
        logBo.setTimestamp(bmObj, nowTimeSec);
        logBo.setExt(bmObj, "");

        logBo.insert(bmObj);
    }

    /**
     * 代金券日志
     * <p>
     * 写入时机：代金券发生变更的时候记录，区分免费代金券、付费代金券
     * @param _userdata        玩家数据
     * @param _event           触发该事件的原因（mainevent）
     * @param _oldNumber       总代金券的旧值
     * @param _finalNumber     总代金券的新值
     * @param _exchange        总代金券的改变值
     * @param _goodsId         游戏策划配置的充值货物id（消耗代金券时记录，其他情况传null）
     * @param _goodsCount      消耗代金券购买商品时记录本次购买的商品数量（其他情况传0）
     * @param _action          1=获得,2=消耗
     * @param _cnMoney         商品对应人民币
     * @param _money           海外对应美元 国内对应人民币
     * @param _orderId         订单号
     * @param _sdkPayId        sdk档位id
     * @param _paidOldNumber   付费代金券的旧值
     * @param _paidFinalNumber 付费代金券的新值
     * @param _paidExchange    付费代金券的改变值
     */
    public static void logVoucher(NPUSUserData _userdata, int _event,
                                  long _oldNumber, long _finalNumber, long _exchange,
                                  String _goodsId, int _goodsCount, int _action,
                                  int _cnMoney, double _money, String _orderId, String _sdkPayId,
                                  long _paidOldNumber, long _paidFinalNumber, long _paidExchange)
    {
        BM bmObj = _userdata.getUSServer().getBM();
        int nowTimeSec = CommonFunc.getNowTimeSec();

        VoucherLogBO logBo = new VoucherLogBO();

        // 基础玩家信息
        logBo.setUid(bmObj, _userdata.getUid());
        logBo.setCid(bmObj, _userdata.getCid());
        logBo.setServerId(bmObj, _userdata.getUSServer().getServerTypeId());
        logBo.setPlatform(bmObj, UserServerConf.getInstance().getPlatformId());
        logBo.setRegion(bmObj, String.valueOf(UserServerConf.getInstance().getPlatAreaId()));
        logBo.setLv(bmObj, (int) _userdata.getParam(ENPPlayerParam.LEVEL));
        logBo.setVipLv(bmObj, (int) _userdata.getParam(ENPPlayerParam.VIP_LVL));
        logBo.setArTime(bmObj, _userdata.getPlayerComponent().getBo().getCreateTime());
        logBo.setNation(bmObj, _userdata.getSdkInfo().nation);
        logBo.setVersion(bmObj, _userdata.getSdkInfo().version);
        logBo.setAdid(bmObj, _userdata.getSdkInfo().adid);
        logBo.setAdfrom(bmObj, _userdata.getSdkInfo().adfrom);
        logBo.setAdfrom2(bmObj, _userdata.getSdkInfo().adfrom2);

        // 事件和代金券变更信息
        logBo.setEvent(bmObj, _event);
        logBo.setOldNumber(bmObj, _oldNumber);
        logBo.setFinalNumber(bmObj, _finalNumber);
        logBo.setExchange(bmObj, _exchange);

        // 商品信息（消耗代金券购买时才有值）
        logBo.setGoodsId(bmObj, _goodsId == null ? "" : _goodsId);
        logBo.setGoodsCount(bmObj, _goodsCount);

        // 操作类型和金额
        logBo.setAction(bmObj, _action);
        logBo.setCnMoney(bmObj, _cnMoney);
        logBo.setMoney(bmObj, _money);

        // 订单和匹配信息
        logBo.setOrderId(bmObj, _orderId == null ? "" : _orderId);
        logBo.setSdkPayId(bmObj, _sdkPayId == null ? "" : _sdkPayId);

        // 付费代金券信息
        logBo.setPaidOldNumber(bmObj, _paidOldNumber);
        logBo.setPaidFinalNumber(bmObj, _paidFinalNumber);
        logBo.setPaidExchange(bmObj, _paidExchange);

        // 扩展字段和时间戳
        logBo.setTimestamp(bmObj, nowTimeSec);
        logBo.setDateTime(bmObj, CommonFunc.getNowTagYYYYMMDD());

        logBo.insert(bmObj);
    }

}
