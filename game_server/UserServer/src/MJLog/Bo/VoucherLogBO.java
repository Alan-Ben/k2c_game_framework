package MJLog.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;
import NPCommon.DB.Annotation.RefBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

@RefBo(isIdAuto= true)
public class VoucherLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_uid =0;
    @DataBaseField(type = "varchar(64)", fieldname = "uid", comment = "用户在平台注册的ID")
    private String uid;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "角色id")
    private long cid;

    public static final int FIELD_server_id =2;
    @DataBaseField(type = "int(11)", fieldname = "server_id", comment = "玩家登录时的服务器id")
    private int server_id;

    public static final int FIELD_platform =3;
    @DataBaseField(type = "int(11)", fieldname = "platform", comment = "账号归属的平台id")
    private int platform;

    public static final int FIELD_region =4;
    @DataBaseField(type = "varchar(50)", fieldname = "region", comment = "账号归属的区域id")
    private String region;

    public static final int FIELD_lv =5;
    @DataBaseField(type = "int(11)", fieldname = "lv", comment = "事件发生时的角色等级")
    private int lv;

    public static final int FIELD_vip_lv =6;
    @DataBaseField(type = "int(11)", fieldname = "vip_lv", comment = "事件发生时的角色VIP")
    private int vip_lv;

    public static final int FIELD_ar_time =7;
    @DataBaseField(type = "int(11)", fieldname = "ar_time", comment = "角色创建时间戳（10位）")
    private int ar_time;

    public static final int FIELD_nation =8;
    @DataBaseField(type = "varchar(100)", fieldname = "nation", comment = "记录订单创建时的实时国家代号（2位）（无数据时记为空，字符串）")
    private String nation;

    public static final int FIELD_version =9;
    @DataBaseField(type = "varchar(50)", fieldname = "version", comment = "记录订单创建时的客户端版本号（无数据时记为空，字符串）")
    private String version;

    public static final int FIELD_adid =10;
    @DataBaseField(type = "varchar(50)", fieldname = "adid", comment = "取事件发生时的设备id；如果取不到则取创角的设备id")
    private String adid;

    public static final int FIELD_event =11;
    @DataBaseField(type = "int(11)", fieldname = "event", comment = "触发该事件的原因，等同于游戏内的mainevent")
    private int event;

    public static final int FIELD_oldNumber =12;
    @DataBaseField(type = "bigint(20)", fieldname = "oldNumber", comment = "总代金券的旧值 总代金券数量变更前的值")
    private long oldNumber;

    public static final int FIELD_finalNumber =13;
    @DataBaseField(type = "bigint(20)", fieldname = "finalNumber", comment = "总代金券的新值 总代金券数量变更后的值")
    private long finalNumber;

    public static final int FIELD_exchange =14;
    @DataBaseField(type = "bigint(20)", fieldname = "exchange", comment = "总代金券的改变值")
    private long exchange;

    public static final int FIELD_goods_id =15;
    @DataBaseField(type = "varchar(32)", fieldname = "goods_id", comment = "游戏策划配置的充值货物id，消耗代金券时，记录玩家用代币购买了什么商品。其他情况则记为null")
    private String goods_id;

    public static final int FIELD_goods_count =16;
    @DataBaseField(type = "int(11)", fieldname = "goods_count", comment = "消耗代金券购买商品时，记录本次购买的商品数量。其他情况则记为0")
    private int goods_count;

    public static final int FIELD_action =17;
    @DataBaseField(type = "int(11)", fieldname = "action", comment = "1=获得,2=消耗")
    private int action;

    public static final int FIELD_timestamp =18;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "记录事件发生的时间戳（10位）")
    private int timestamp;

    public static final int FIELD_cn_money =19;
    @DataBaseField(type = "int(11)", fieldname = "cn_money", comment = "商品对应人民币")
    private int cn_money;

    public static final int FIELD_money =20;
    @DataBaseField(type = "decimal(20,6)", fieldname = "money", comment = "海外对应美元 国内对应人民币")
    private double money;

    public static final int FIELD_order_id =21;
    @DataBaseField(type = "varchar(500)", fieldname = "order_id", comment = "订单号")
    private String order_id;

    public static final int FIELD_sdk_pay_id =22;
    @DataBaseField(type = "varchar(500)", fieldname = "sdk_pay_id", comment = "sdk档位id")
    private String sdk_pay_id;

    public static final int FIELD_match_id =23;
    @DataBaseField(type = "varchar(40)", fieldname = "match_id", comment = "匹配号")
    private String match_id;

    public static final int FIELD_subevent =24;
    @DataBaseField(type = "int(11)", fieldname = "subevent", comment = "物品获得子事件")
    private int subevent;

    public static final int FIELD_mainExtraParam =25;
    @DataBaseField(type = "bigint(20)", fieldname = "mainExtraParam", comment = "主事件额外参数")
    private long mainExtraParam;

    public static final int FIELD_subExtraParam =26;
    @DataBaseField(type = "bigint(20)", fieldname = "subExtraParam", comment = "子事件额外参数")
    private long subExtraParam;

    public static final int FIELD_date_time =27;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "事件日期")
    private int date_time;

    public static final int FIELD_paid_oldNumber =28;
    @DataBaseField(type = "bigint(20)", fieldname = "paid_oldNumber", comment = "付费代金卷的旧值 代金券数量变更前的值")
    private long paid_oldNumber;

    public static final int FIELD_paid_finalNumber =29;
    @DataBaseField(type = "bigint(20)", fieldname = "paid_finalNumber", comment = "付费代金卷的新值 代金券数量变更后的值")
    private long paid_finalNumber;

    public static final int FIELD_paid_exchange =30;
    @DataBaseField(type = "bigint(20)", fieldname = "paid_exchange", comment = "付费代金卷的改变值")
    private long paid_exchange;

    public static final int FIELD_ext =31;
    @DataBaseField(type = "text", fieldname = "ext", comment = "扩展字段：json格式 ，根据对应事件自己定义字符串的数据库类型")
    private String ext;

    public static final int FIELD_adfrom =32;
    @DataBaseField(type = "varchar(64)", fieldname = "adfrom", comment = "一级渠道")
    private String adfrom;

    public static final int FIELD_adfrom2 =33;
    @DataBaseField(type = "varchar(64)", fieldname = "adfrom2", comment = "二级渠道")
    private String adfrom2;

    public VoucherLogBO() {
        id = 0;
        uid = "";
        cid = 0L;
        server_id = 0;
        platform = 0;
        region = "";
        lv = 0;
        vip_lv = 0;
        ar_time = 0;
        nation = "";
        version = "";
        adid = "";
        event = 0;
        oldNumber = 0L;
        finalNumber = 0L;
        exchange = 0L;
        goods_id = "";
        goods_count = 0;
        action = 0;
        timestamp = 0;
        cn_money = 0;
        money = 0.0d;
        order_id = "";
        sdk_pay_id = "";
        match_id = "";
        subevent = 0;
        mainExtraParam = 0L;
        subExtraParam = 0L;
        date_time = 0;
        paid_oldNumber = 0L;
        paid_finalNumber = 0L;
        paid_exchange = 0L;
        ext = "";
        adfrom = "";
        adfrom2 = "";
    }

    public VoucherLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        uid = rs.getString(2);
        cid = rs.getLong(3);
        server_id = rs.getInt(4);
        platform = rs.getInt(5);
        region = rs.getString(6);
        lv = rs.getInt(7);
        vip_lv = rs.getInt(8);
        ar_time = rs.getInt(9);
        nation = rs.getString(10);
        version = rs.getString(11);
        adid = rs.getString(12);
        event = rs.getInt(13);
        oldNumber = rs.getLong(14);
        finalNumber = rs.getLong(15);
        exchange = rs.getLong(16);
        goods_id = rs.getString(17);
        goods_count = rs.getInt(18);
        action = rs.getInt(19);
        timestamp = rs.getInt(20);
        cn_money = rs.getInt(21);
        money = rs.getDouble(22);
        order_id = rs.getString(23);
        sdk_pay_id = rs.getString(24);
        match_id = rs.getString(25);
        subevent = rs.getInt(26);
        mainExtraParam = rs.getLong(27);
        subExtraParam = rs.getLong(28);
        date_time = rs.getInt(29);
        paid_oldNumber = rs.getLong(30);
        paid_finalNumber = rs.getLong(31);
        paid_exchange = rs.getLong(32);
        ext = rs.getString(33);
        adfrom = rs.getString(34);
        adfrom2 = rs.getString(35);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new VoucherLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `uid`, `cid`, `server_id`, `platform`, `region`, `lv`, `vip_lv`, `ar_time`, `nation`, `version`, `adid`, `event`, `oldNumber`, `finalNumber`, `exchange`, `goods_id`, `goods_count`, `action`, `timestamp`, `cn_money`, `money`, `order_id`, `sdk_pay_id`, `match_id`, `subevent`, `mainExtraParam`, `subExtraParam`, `date_time`, `paid_oldNumber`, `paid_finalNumber`, `paid_exchange`, `ext`, `adfrom`, `adfrom2`";
    }

    @Override
    public String getTableName() {
        return "`voucher_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(server_id).append("', ");
        strBuf.append("'").append(platform).append("', ");
        strBuf.append("'").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(lv).append("', ");
        strBuf.append("'").append(vip_lv).append("', ");
        strBuf.append("'").append(ar_time).append("', ");
        strBuf.append("'").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(version == null ? null : version.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(event).append("', ");
        strBuf.append("'").append(oldNumber).append("', ");
        strBuf.append("'").append(finalNumber).append("', ");
        strBuf.append("'").append(exchange).append("', ");
        strBuf.append("'").append(goods_id == null ? null : goods_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(goods_count).append("', ");
        strBuf.append("'").append(action).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(cn_money).append("', ");
        strBuf.append("'").append(money).append("', ");
        strBuf.append("'").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(sdk_pay_id == null ? null : sdk_pay_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(match_id == null ? null : match_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(subevent).append("', ");
        strBuf.append("'").append(mainExtraParam).append("', ");
        strBuf.append("'").append(subExtraParam).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(paid_oldNumber).append("', ");
        strBuf.append("'").append(paid_finalNumber).append("', ");
        strBuf.append("'").append(paid_exchange).append("', ");
        strBuf.append("'").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 用户在平台注册的ID
    public String getUid() { return this.uid; }
    public void setUid(BM _bm, String uid) {
        if(uid.equals(this.uid)) 
            return;
        this.uid = uid; 
        markField(_bm, FIELD_uid); 
    }
    public void saveUid(BM _bm, String uid) {
        if(uid.equals(this.uid)) 
            return;
        this.uid = uid;
        saveField(_bm, "uid", uid);
    }

    // 角色id
    public long getCid() { return this.cid; }
    public void setCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid; 
        markField(_bm, FIELD_cid); 
    }
    public void saveCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid;
        saveField(_bm, "cid", cid);
    }

    // 玩家登录时的服务器id
    public int getServerId() { return this.server_id; }
    public void setServerId(BM _bm, int server_id) {
        if(server_id==this.server_id) 
            return;
        this.server_id = server_id; 
        markField(_bm, FIELD_server_id); 
    }
    public void saveServerId(BM _bm, int server_id) {
        if(server_id==this.server_id) 
            return;
        this.server_id = server_id;
        saveField(_bm, "server_id", server_id);
    }

    // 账号归属的平台id
    public int getPlatform() { return this.platform; }
    public void setPlatform(BM _bm, int platform) {
        if(platform==this.platform) 
            return;
        this.platform = platform; 
        markField(_bm, FIELD_platform); 
    }
    public void savePlatform(BM _bm, int platform) {
        if(platform==this.platform) 
            return;
        this.platform = platform;
        saveField(_bm, "platform", platform);
    }

    // 账号归属的区域id
    public String getRegion() { return this.region; }
    public void setRegion(BM _bm, String region) {
        if(region.equals(this.region)) 
            return;
        this.region = region; 
        markField(_bm, FIELD_region); 
    }
    public void saveRegion(BM _bm, String region) {
        if(region.equals(this.region)) 
            return;
        this.region = region;
        saveField(_bm, "region", region);
    }

    // 事件发生时的角色等级
    public int getLv() { return this.lv; }
    public void setLv(BM _bm, int lv) {
        if(lv==this.lv) 
            return;
        this.lv = lv; 
        markField(_bm, FIELD_lv); 
    }
    public void saveLv(BM _bm, int lv) {
        if(lv==this.lv) 
            return;
        this.lv = lv;
        saveField(_bm, "lv", lv);
    }

    // 事件发生时的角色VIP
    public int getVipLv() { return this.vip_lv; }
    public void setVipLv(BM _bm, int vip_lv) {
        if(vip_lv==this.vip_lv) 
            return;
        this.vip_lv = vip_lv; 
        markField(_bm, FIELD_vip_lv); 
    }
    public void saveVipLv(BM _bm, int vip_lv) {
        if(vip_lv==this.vip_lv) 
            return;
        this.vip_lv = vip_lv;
        saveField(_bm, "vip_lv", vip_lv);
    }

    // 角色创建时间戳（10位）
    public int getArTime() { return this.ar_time; }
    public void setArTime(BM _bm, int ar_time) {
        if(ar_time==this.ar_time) 
            return;
        this.ar_time = ar_time; 
        markField(_bm, FIELD_ar_time); 
    }
    public void saveArTime(BM _bm, int ar_time) {
        if(ar_time==this.ar_time) 
            return;
        this.ar_time = ar_time;
        saveField(_bm, "ar_time", ar_time);
    }

    // 记录订单创建时的实时国家代号（2位）（无数据时记为空，字符串）
    public String getNation() { return this.nation; }
    public void setNation(BM _bm, String nation) {
        if(nation.equals(this.nation)) 
            return;
        this.nation = nation; 
        markField(_bm, FIELD_nation); 
    }
    public void saveNation(BM _bm, String nation) {
        if(nation.equals(this.nation)) 
            return;
        this.nation = nation;
        saveField(_bm, "nation", nation);
    }

    // 记录订单创建时的客户端版本号（无数据时记为空，字符串）
    public String getVersion() { return this.version; }
    public void setVersion(BM _bm, String version) {
        if(version.equals(this.version)) 
            return;
        this.version = version; 
        markField(_bm, FIELD_version); 
    }
    public void saveVersion(BM _bm, String version) {
        if(version.equals(this.version)) 
            return;
        this.version = version;
        saveField(_bm, "version", version);
    }

    // 取事件发生时的设备id；如果取不到则取创角的设备id
    public String getAdid() { return this.adid; }
    public void setAdid(BM _bm, String adid) {
        if(adid.equals(this.adid)) 
            return;
        this.adid = adid; 
        markField(_bm, FIELD_adid); 
    }
    public void saveAdid(BM _bm, String adid) {
        if(adid.equals(this.adid)) 
            return;
        this.adid = adid;
        saveField(_bm, "adid", adid);
    }

    // 触发该事件的原因，等同于游戏内的mainevent
    public int getEvent() { return this.event; }
    public void setEvent(BM _bm, int event) {
        if(event==this.event) 
            return;
        this.event = event; 
        markField(_bm, FIELD_event); 
    }
    public void saveEvent(BM _bm, int event) {
        if(event==this.event) 
            return;
        this.event = event;
        saveField(_bm, "event", event);
    }

    // 总代金券的旧值 总代金券数量变更前的值
    public long getOldNumber() { return this.oldNumber; }
    public void setOldNumber(BM _bm, long oldNumber) {
        if(oldNumber==this.oldNumber) 
            return;
        this.oldNumber = oldNumber; 
        markField(_bm, FIELD_oldNumber); 
    }
    public void saveOldNumber(BM _bm, long oldNumber) {
        if(oldNumber==this.oldNumber) 
            return;
        this.oldNumber = oldNumber;
        saveField(_bm, "oldNumber", oldNumber);
    }

    // 总代金券的新值 总代金券数量变更后的值
    public long getFinalNumber() { return this.finalNumber; }
    public void setFinalNumber(BM _bm, long finalNumber) {
        if(finalNumber==this.finalNumber) 
            return;
        this.finalNumber = finalNumber; 
        markField(_bm, FIELD_finalNumber); 
    }
    public void saveFinalNumber(BM _bm, long finalNumber) {
        if(finalNumber==this.finalNumber) 
            return;
        this.finalNumber = finalNumber;
        saveField(_bm, "finalNumber", finalNumber);
    }

    // 总代金券的改变值
    public long getExchange() { return this.exchange; }
    public void setExchange(BM _bm, long exchange) {
        if(exchange==this.exchange) 
            return;
        this.exchange = exchange; 
        markField(_bm, FIELD_exchange); 
    }
    public void saveExchange(BM _bm, long exchange) {
        if(exchange==this.exchange) 
            return;
        this.exchange = exchange;
        saveField(_bm, "exchange", exchange);
    }

    // 游戏策划配置的充值货物id，消耗代金券时，记录玩家用代币购买了什么商品。其他情况则记为null
    public String getGoodsId() { return this.goods_id; }
    public void setGoodsId(BM _bm, String goods_id) {
        if(goods_id.equals(this.goods_id)) 
            return;
        this.goods_id = goods_id; 
        markField(_bm, FIELD_goods_id); 
    }
    public void saveGoodsId(BM _bm, String goods_id) {
        if(goods_id.equals(this.goods_id)) 
            return;
        this.goods_id = goods_id;
        saveField(_bm, "goods_id", goods_id);
    }

    // 消耗代金券购买商品时，记录本次购买的商品数量。其他情况则记为0
    public int getGoodsCount() { return this.goods_count; }
    public void setGoodsCount(BM _bm, int goods_count) {
        if(goods_count==this.goods_count) 
            return;
        this.goods_count = goods_count; 
        markField(_bm, FIELD_goods_count); 
    }
    public void saveGoodsCount(BM _bm, int goods_count) {
        if(goods_count==this.goods_count) 
            return;
        this.goods_count = goods_count;
        saveField(_bm, "goods_count", goods_count);
    }

    // 1=获得,2=消耗
    public int getAction() { return this.action; }
    public void setAction(BM _bm, int action) {
        if(action==this.action) 
            return;
        this.action = action; 
        markField(_bm, FIELD_action); 
    }
    public void saveAction(BM _bm, int action) {
        if(action==this.action) 
            return;
        this.action = action;
        saveField(_bm, "action", action);
    }

    // 记录事件发生的时间戳（10位）
    public int getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }

    // 商品对应人民币
    public int getCnMoney() { return this.cn_money; }
    public void setCnMoney(BM _bm, int cn_money) {
        if(cn_money==this.cn_money) 
            return;
        this.cn_money = cn_money; 
        markField(_bm, FIELD_cn_money); 
    }
    public void saveCnMoney(BM _bm, int cn_money) {
        if(cn_money==this.cn_money) 
            return;
        this.cn_money = cn_money;
        saveField(_bm, "cn_money", cn_money);
    }

    // 海外对应美元 国内对应人民币
    public double getMoney() { return this.money; }
    public void setMoney(BM _bm, double money) {
        if(money==this.money) 
            return;
        this.money = money; 
        markField(_bm, FIELD_money); 
    }
    public void saveMoney(BM _bm, double money) {
        if(money==this.money) 
            return;
        this.money = money;
        saveField(_bm, "money", money);
    }

    // 订单号
    public String getOrderId() { return this.order_id; }
    public void setOrderId(BM _bm, String order_id) {
        if(order_id.equals(this.order_id)) 
            return;
        this.order_id = order_id; 
        markField(_bm, FIELD_order_id); 
    }
    public void saveOrderId(BM _bm, String order_id) {
        if(order_id.equals(this.order_id)) 
            return;
        this.order_id = order_id;
        saveField(_bm, "order_id", order_id);
    }

    // sdk档位id
    public String getSdkPayId() { return this.sdk_pay_id; }
    public void setSdkPayId(BM _bm, String sdk_pay_id) {
        if(sdk_pay_id.equals(this.sdk_pay_id)) 
            return;
        this.sdk_pay_id = sdk_pay_id; 
        markField(_bm, FIELD_sdk_pay_id); 
    }
    public void saveSdkPayId(BM _bm, String sdk_pay_id) {
        if(sdk_pay_id.equals(this.sdk_pay_id)) 
            return;
        this.sdk_pay_id = sdk_pay_id;
        saveField(_bm, "sdk_pay_id", sdk_pay_id);
    }

    // 匹配号
    public String getMatchId() { return this.match_id; }
    public void setMatchId(BM _bm, String match_id) {
        if(match_id.equals(this.match_id)) 
            return;
        this.match_id = match_id; 
        markField(_bm, FIELD_match_id); 
    }
    public void saveMatchId(BM _bm, String match_id) {
        if(match_id.equals(this.match_id)) 
            return;
        this.match_id = match_id;
        saveField(_bm, "match_id", match_id);
    }

    // 物品获得子事件
    public int getSubevent() { return this.subevent; }
    public void setSubevent(BM _bm, int subevent) {
        if(subevent==this.subevent) 
            return;
        this.subevent = subevent; 
        markField(_bm, FIELD_subevent); 
    }
    public void saveSubevent(BM _bm, int subevent) {
        if(subevent==this.subevent) 
            return;
        this.subevent = subevent;
        saveField(_bm, "subevent", subevent);
    }

    // 主事件额外参数
    public long getMainExtraParam() { return this.mainExtraParam; }
    public void setMainExtraParam(BM _bm, long mainExtraParam) {
        if(mainExtraParam==this.mainExtraParam) 
            return;
        this.mainExtraParam = mainExtraParam; 
        markField(_bm, FIELD_mainExtraParam); 
    }
    public void saveMainExtraParam(BM _bm, long mainExtraParam) {
        if(mainExtraParam==this.mainExtraParam) 
            return;
        this.mainExtraParam = mainExtraParam;
        saveField(_bm, "mainExtraParam", mainExtraParam);
    }

    // 子事件额外参数
    public long getSubExtraParam() { return this.subExtraParam; }
    public void setSubExtraParam(BM _bm, long subExtraParam) {
        if(subExtraParam==this.subExtraParam) 
            return;
        this.subExtraParam = subExtraParam; 
        markField(_bm, FIELD_subExtraParam); 
    }
    public void saveSubExtraParam(BM _bm, long subExtraParam) {
        if(subExtraParam==this.subExtraParam) 
            return;
        this.subExtraParam = subExtraParam;
        saveField(_bm, "subExtraParam", subExtraParam);
    }

    // 事件日期
    public int getDateTime() { return this.date_time; }
    public void setDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time; 
        markField(_bm, FIELD_date_time); 
    }
    public void saveDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time;
        saveField(_bm, "date_time", date_time);
    }

    // 付费代金卷的旧值 代金券数量变更前的值
    public long getPaidOldNumber() { return this.paid_oldNumber; }
    public void setPaidOldNumber(BM _bm, long paid_oldNumber) {
        if(paid_oldNumber==this.paid_oldNumber) 
            return;
        this.paid_oldNumber = paid_oldNumber; 
        markField(_bm, FIELD_paid_oldNumber); 
    }
    public void savePaidOldNumber(BM _bm, long paid_oldNumber) {
        if(paid_oldNumber==this.paid_oldNumber) 
            return;
        this.paid_oldNumber = paid_oldNumber;
        saveField(_bm, "paid_oldNumber", paid_oldNumber);
    }

    // 付费代金卷的新值 代金券数量变更后的值
    public long getPaidFinalNumber() { return this.paid_finalNumber; }
    public void setPaidFinalNumber(BM _bm, long paid_finalNumber) {
        if(paid_finalNumber==this.paid_finalNumber) 
            return;
        this.paid_finalNumber = paid_finalNumber; 
        markField(_bm, FIELD_paid_finalNumber); 
    }
    public void savePaidFinalNumber(BM _bm, long paid_finalNumber) {
        if(paid_finalNumber==this.paid_finalNumber) 
            return;
        this.paid_finalNumber = paid_finalNumber;
        saveField(_bm, "paid_finalNumber", paid_finalNumber);
    }

    // 付费代金卷的改变值
    public long getPaidExchange() { return this.paid_exchange; }
    public void setPaidExchange(BM _bm, long paid_exchange) {
        if(paid_exchange==this.paid_exchange) 
            return;
        this.paid_exchange = paid_exchange; 
        markField(_bm, FIELD_paid_exchange); 
    }
    public void savePaidExchange(BM _bm, long paid_exchange) {
        if(paid_exchange==this.paid_exchange) 
            return;
        this.paid_exchange = paid_exchange;
        saveField(_bm, "paid_exchange", paid_exchange);
    }

    // 扩展字段：json格式 ，根据对应事件自己定义字符串的数据库类型
    public String getExt() { return this.ext; }
    public void setExt(BM _bm, String ext) {
        if(ext.equals(this.ext)) 
            return;
        this.ext = ext; 
        markField(_bm, FIELD_ext); 
    }
    public void saveExt(BM _bm, String ext) {
        if(ext.equals(this.ext)) 
            return;
        this.ext = ext;
        saveField(_bm, "ext", ext);
    }

    // 一级渠道
    public String getAdfrom() { return this.adfrom; }
    public void setAdfrom(BM _bm, String adfrom) {
        if(adfrom.equals(this.adfrom)) 
            return;
        this.adfrom = adfrom; 
        markField(_bm, FIELD_adfrom); 
    }
    public void saveAdfrom(BM _bm, String adfrom) {
        if(adfrom.equals(this.adfrom)) 
            return;
        this.adfrom = adfrom;
        saveField(_bm, "adfrom", adfrom);
    }

    // 二级渠道
    public String getAdfrom2() { return this.adfrom2; }
    public void setAdfrom2(BM _bm, String adfrom2) {
        if(adfrom2.equals(this.adfrom2)) 
            return;
        this.adfrom2 = adfrom2; 
        markField(_bm, FIELD_adfrom2); 
    }
    public void saveAdfrom2(BM _bm, String adfrom2) {
        if(adfrom2.equals(this.adfrom2)) 
            return;
        this.adfrom2 = adfrom2;
        saveField(_bm, "adfrom2", adfrom2);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `server_id` = '").append(server_id).append("',");
        sBuilder.append(" `platform` = '").append(platform).append("',");
        sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `lv` = '").append(lv).append("',");
        sBuilder.append(" `vip_lv` = '").append(vip_lv).append("',");
        sBuilder.append(" `ar_time` = '").append(ar_time).append("',");
        sBuilder.append(" `nation` = '").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `version` = '").append(version == null ? null : version.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adid` = '").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `event` = '").append(event).append("',");
        sBuilder.append(" `oldNumber` = '").append(oldNumber).append("',");
        sBuilder.append(" `finalNumber` = '").append(finalNumber).append("',");
        sBuilder.append(" `exchange` = '").append(exchange).append("',");
        sBuilder.append(" `goods_id` = '").append(goods_id == null ? null : goods_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `goods_count` = '").append(goods_count).append("',");
        sBuilder.append(" `action` = '").append(action).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `cn_money` = '").append(cn_money).append("',");
        sBuilder.append(" `money` = '").append(money).append("',");
        sBuilder.append(" `order_id` = '").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `sdk_pay_id` = '").append(sdk_pay_id == null ? null : sdk_pay_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `match_id` = '").append(match_id == null ? null : match_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `subevent` = '").append(subevent).append("',");
        sBuilder.append(" `mainExtraParam` = '").append(mainExtraParam).append("',");
        sBuilder.append(" `subExtraParam` = '").append(subExtraParam).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `paid_oldNumber` = '").append(paid_oldNumber).append("',");
        sBuilder.append(" `paid_finalNumber` = '").append(paid_finalNumber).append("',");
        sBuilder.append(" `paid_exchange` = '").append(paid_exchange).append("',");
        sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adfrom` = '").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adfrom2` = '").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_server_id)) sBuilder.append(" `server_id` = '").append(server_id).append("',");
        if(isFieldMarked(FIELD_platform)) sBuilder.append(" `platform` = '").append(platform).append("',");
        if(isFieldMarked(FIELD_region)) sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_lv)) sBuilder.append(" `lv` = '").append(lv).append("',");
        if(isFieldMarked(FIELD_vip_lv)) sBuilder.append(" `vip_lv` = '").append(vip_lv).append("',");
        if(isFieldMarked(FIELD_ar_time)) sBuilder.append(" `ar_time` = '").append(ar_time).append("',");
        if(isFieldMarked(FIELD_nation)) sBuilder.append(" `nation` = '").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_version)) sBuilder.append(" `version` = '").append(version == null ? null : version.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adid)) sBuilder.append(" `adid` = '").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_event)) sBuilder.append(" `event` = '").append(event).append("',");
        if(isFieldMarked(FIELD_oldNumber)) sBuilder.append(" `oldNumber` = '").append(oldNumber).append("',");
        if(isFieldMarked(FIELD_finalNumber)) sBuilder.append(" `finalNumber` = '").append(finalNumber).append("',");
        if(isFieldMarked(FIELD_exchange)) sBuilder.append(" `exchange` = '").append(exchange).append("',");
        if(isFieldMarked(FIELD_goods_id)) sBuilder.append(" `goods_id` = '").append(goods_id == null ? null : goods_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_goods_count)) sBuilder.append(" `goods_count` = '").append(goods_count).append("',");
        if(isFieldMarked(FIELD_action)) sBuilder.append(" `action` = '").append(action).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_cn_money)) sBuilder.append(" `cn_money` = '").append(cn_money).append("',");
        if(isFieldMarked(FIELD_money)) sBuilder.append(" `money` = '").append(money).append("',");
        if(isFieldMarked(FIELD_order_id)) sBuilder.append(" `order_id` = '").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_sdk_pay_id)) sBuilder.append(" `sdk_pay_id` = '").append(sdk_pay_id == null ? null : sdk_pay_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_match_id)) sBuilder.append(" `match_id` = '").append(match_id == null ? null : match_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_subevent)) sBuilder.append(" `subevent` = '").append(subevent).append("',");
        if(isFieldMarked(FIELD_mainExtraParam)) sBuilder.append(" `mainExtraParam` = '").append(mainExtraParam).append("',");
        if(isFieldMarked(FIELD_subExtraParam)) sBuilder.append(" `subExtraParam` = '").append(subExtraParam).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_paid_oldNumber)) sBuilder.append(" `paid_oldNumber` = '").append(paid_oldNumber).append("',");
        if(isFieldMarked(FIELD_paid_finalNumber)) sBuilder.append(" `paid_finalNumber` = '").append(paid_finalNumber).append("',");
        if(isFieldMarked(FIELD_paid_exchange)) sBuilder.append(" `paid_exchange` = '").append(paid_exchange).append("',");
        if(isFieldMarked(FIELD_ext)) sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adfrom)) sBuilder.append(" `adfrom` = '").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adfrom2)) sBuilder.append(" `adfrom2` = '").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `voucher_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '用户在平台注册的ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '玩家登录时的服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '账号归属的平台id',"
                + "`region` varchar(50) NOT NULL DEFAULT '' COMMENT '账号归属的区域id',"
                + "`lv` int(11) NOT NULL DEFAULT '0' COMMENT '事件发生时的角色等级',"
                + "`vip_lv` int(11) NOT NULL DEFAULT '0' COMMENT '事件发生时的角色VIP',"
                + "`ar_time` int(11) NOT NULL DEFAULT '0' COMMENT '角色创建时间戳（10位）',"
                + "`nation` varchar(100) NOT NULL DEFAULT '' COMMENT '记录订单创建时的实时国家代号（2位）（无数据时记为空，字符串）',"
                + "`version` varchar(50) NOT NULL DEFAULT '' COMMENT '记录订单创建时的客户端版本号（无数据时记为空，字符串）',"
                + "`adid` varchar(50) NOT NULL DEFAULT '' COMMENT '取事件发生时的设备id；如果取不到则取创角的设备id',"
                + "`event` int(11) NOT NULL DEFAULT '0' COMMENT '触发该事件的原因，等同于游戏内的mainevent',"
                + "`oldNumber` bigint(20) NOT NULL DEFAULT '0' COMMENT '总代金券的旧值 总代金券数量变更前的值',"
                + "`finalNumber` bigint(20) NOT NULL DEFAULT '0' COMMENT '总代金券的新值 总代金券数量变更后的值',"
                + "`exchange` bigint(20) NOT NULL DEFAULT '0' COMMENT '总代金券的改变值',"
                + "`goods_id` varchar(32) NOT NULL DEFAULT '' COMMENT '游戏策划配置的充值货物id，消耗代金券时，记录玩家用代币购买了什么商品。其他情况则记为null',"
                + "`goods_count` int(11) NOT NULL DEFAULT '0' COMMENT '消耗代金券购买商品时，记录本次购买的商品数量。其他情况则记为0',"
                + "`action` int(11) NOT NULL DEFAULT '0' COMMENT '1=获得,2=消耗',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '记录事件发生的时间戳（10位）',"
                + "`cn_money` int(11) NOT NULL DEFAULT '0' COMMENT '商品对应人民币',"
                + "`money` decimal(20,6) NOT NULL DEFAULT '0.0' COMMENT '海外对应美元 国内对应人民币',"
                + "`order_id` varchar(500) NOT NULL DEFAULT '' COMMENT '订单号',"
                + "`sdk_pay_id` varchar(500) NOT NULL DEFAULT '' COMMENT 'sdk档位id',"
                + "`match_id` varchar(40) NOT NULL DEFAULT '' COMMENT '匹配号',"
                + "`subevent` int(11) NOT NULL DEFAULT '0' COMMENT '物品获得子事件',"
                + "`mainExtraParam` bigint(20) NOT NULL DEFAULT '0' COMMENT '主事件额外参数',"
                + "`subExtraParam` bigint(20) NOT NULL DEFAULT '0' COMMENT '子事件额外参数',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '事件日期',"
                + "`paid_oldNumber` bigint(20) NOT NULL DEFAULT '0' COMMENT '付费代金卷的旧值 代金券数量变更前的值',"
                + "`paid_finalNumber` bigint(20) NOT NULL DEFAULT '0' COMMENT '付费代金卷的新值 代金券数量变更后的值',"
                + "`paid_exchange` bigint(20) NOT NULL DEFAULT '0' COMMENT '付费代金卷的改变值',"
                + "`ext` text NULL COMMENT '扩展字段：json格式 ，根据对应事件自己定义字符串的数据库类型',"
                + "`adfrom` varchar(64) NOT NULL DEFAULT '' COMMENT '一级渠道',"
                + "`adfrom2` varchar(64) NOT NULL DEFAULT '' COMMENT '二级渠道',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-代金券日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.us_log;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);//uid
        _size+=8;//cid
        _size+=4;//server_id
        _size+=4;//platform
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(region);//region
        _size+=4;//lv
        _size+=4;//vip_lv
        _size+=4;//ar_time
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(nation);//nation
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(version);//version
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adid);//adid
        _size+=4;//event
        _size+=8;//oldNumber
        _size+=8;//finalNumber
        _size+=8;//exchange
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(goods_id);//goods_id
        _size+=4;//goods_count
        _size+=4;//action
        _size+=4;//timestamp
        _size+=4;//cn_money
        _size+=8;//money
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(order_id);//order_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdk_pay_id);//sdk_pay_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(match_id);//match_id
        _size+=4;//subevent
        _size+=8;//mainExtraParam
        _size+=8;//subExtraParam
        _size+=4;//date_time
        _size+=8;//paid_oldNumber
        _size+=8;//paid_finalNumber
        _size+=8;//paid_exchange
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ext);//ext
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adfrom);//adfrom
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adfrom2);//adfrom2
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, uid);
        buff.putLong(cid);
        buff.putInt(server_id);
        buff.putInt(platform);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, region);
        buff.putInt(lv);
        buff.putInt(vip_lv);
        buff.putInt(ar_time);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, nation);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, version);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adid);
        buff.putInt(event);
        buff.putLong(oldNumber);
        buff.putLong(finalNumber);
        buff.putLong(exchange);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, goods_id);
        buff.putInt(goods_count);
        buff.putInt(action);
        buff.putInt(timestamp);
        buff.putInt(cn_money);
        buff.putDouble(money);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, order_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sdk_pay_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, match_id);
        buff.putInt(subevent);
        buff.putLong(mainExtraParam);
        buff.putLong(subExtraParam);
        buff.putInt(date_time);
        buff.putLong(paid_oldNumber);
        buff.putLong(paid_finalNumber);
        buff.putLong(paid_exchange);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, ext);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adfrom);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adfrom2);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        uid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        cid=buff.getLong();
        server_id=buff.getInt();
        platform=buff.getInt();
        region=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        lv=buff.getInt();
        vip_lv=buff.getInt();
        ar_time=buff.getInt();
        nation=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        version=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        event=buff.getInt();
        oldNumber=buff.getLong();
        finalNumber=buff.getLong();
        exchange=buff.getLong();
        goods_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        goods_count=buff.getInt();
        action=buff.getInt();
        timestamp=buff.getInt();
        cn_money=buff.getInt();
        money=buff.getDouble();
        order_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        sdk_pay_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        match_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        subevent=buff.getInt();
        mainExtraParam=buff.getLong();
        subExtraParam=buff.getLong();
        date_time=buff.getInt();
        paid_oldNumber=buff.getLong();
        paid_finalNumber=buff.getLong();
        paid_exchange=buff.getLong();
        ext=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adfrom=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adfrom2=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
