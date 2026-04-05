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
public class BaseRechargeLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_sole_id =0;
    @DataBaseField(type = "varchar(32)", fieldname = "sole_id", comment = "全服唯一角色id")
    private String sole_id;

    public static final int FIELD_uid =1;
    @DataBaseField(type = "varchar(64)", fieldname = "uid", comment = "用户id")
    private String uid;

    public static final int FIELD_cid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "角色id")
    private long cid;

    public static final int FIELD_order_id =3;
    @DataBaseField(type = "varchar(128)", fieldname = "order_id", comment = "订单号")
    private String order_id;

    public static final int FIELD_tp_order_id =4;
    @DataBaseField(type = "varchar(128)", fieldname = "tp_order_id", comment = "第三方订单号")
    private String tp_order_id;

    public static final int FIELD_sdkorderId =5;
    @DataBaseField(type = "varchar(128)", fieldname = "sdkorderId", comment = "SDK订单号")
    private String sdkorderId;

    public static final int FIELD_money =6;
    @DataBaseField(type = "decimal(20,2)", fieldname = "money", comment = "充值金额")
    private double money;

    public static final int FIELD_cn_money =7;
    @DataBaseField(type = "decimal(20,2)", fieldname = "cn_money", comment = "人民币金额")
    private double cn_money;

    public static final int FIELD_goods_id =8;
    @DataBaseField(type = "varchar(32)", fieldname = "goods_id", comment = "充值货物id")
    private String goods_id;

    public static final int FIELD_coin_num =9;
    @DataBaseField(type = "int(11)", fieldname = "coin_num", comment = "货币数量")
    private int coin_num;

    public static final int FIELD_status =10;
    @DataBaseField(type = "int(11)", fieldname = "status", comment = "状态:充值=1")
    private int status;

    public static final int FIELD_pay_time =11;
    @DataBaseField(type = "int(11)", fieldname = "pay_time", comment = "充值时间戳（10位）")
    private int pay_time;

    public static final int FIELD_arrive_time =12;
    @DataBaseField(type = "int(11)", fieldname = "arrive_time", comment = "到账时间戳（10位）")
    private int arrive_time;

    public static final int FIELD_timestamp =13;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "创建时间戳（10位）")
    private int timestamp;

    public static final int FIELD_adid =14;
    @DataBaseField(type = "varchar(50)", fieldname = "adid", comment = "设备id")
    private String adid;

    public static final int FIELD_server_id =15;
    @DataBaseField(type = "int(11)", fieldname = "server_id", comment = "服务器id")
    private int server_id;

    public static final int FIELD_platform =16;
    @DataBaseField(type = "int(11)", fieldname = "platform", comment = "账号归属的平台id")
    private int platform;

    public static final int FIELD_region =17;
    @DataBaseField(type = "varchar(50)", fieldname = "region", comment = "账号归属的区域id")
    private String region;

    public static final int FIELD_sdk_type =18;
    @DataBaseField(type = "smallint(10)", fieldname = "sdk_type", comment = "订单类型")
    private short sdk_type;

    public static final int FIELD_pay_id =19;
    @DataBaseField(type = "varchar(64)", fieldname = "pay_id", comment = "支付方式")
    private String pay_id;

    public static final int FIELD_ext =20;
    @DataBaseField(type = "text", fieldname = "ext", comment = "扩展字段：json格式")
    private String ext;

    public static final int FIELD_date_time =21;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public BaseRechargeLogBO() {
        id = 0;
        sole_id = "";
        uid = "";
        cid = 0L;
        order_id = "";
        tp_order_id = "";
        sdkorderId = "";
        money = 0.0d;
        cn_money = 0.0d;
        goods_id = "";
        coin_num = 0;
        status = 0;
        pay_time = 0;
        arrive_time = 0;
        timestamp = 0;
        adid = "";
        server_id = 0;
        platform = 0;
        region = "";
        sdk_type = 0;
        pay_id = "";
        ext = "";
        date_time = 0;
    }

    public BaseRechargeLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        sole_id = rs.getString(2);
        uid = rs.getString(3);
        cid = rs.getLong(4);
        order_id = rs.getString(5);
        tp_order_id = rs.getString(6);
        sdkorderId = rs.getString(7);
        money = rs.getDouble(8);
        cn_money = rs.getDouble(9);
        goods_id = rs.getString(10);
        coin_num = rs.getInt(11);
        status = rs.getInt(12);
        pay_time = rs.getInt(13);
        arrive_time = rs.getInt(14);
        timestamp = rs.getInt(15);
        adid = rs.getString(16);
        server_id = rs.getInt(17);
        platform = rs.getInt(18);
        region = rs.getString(19);
        sdk_type = rs.getShort(20);
        pay_id = rs.getString(21);
        ext = rs.getString(22);
        date_time = rs.getInt(23);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new BaseRechargeLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `sole_id`, `uid`, `cid`, `order_id`, `tp_order_id`, `sdkorderId`, `money`, `cn_money`, `goods_id`, `coin_num`, `status`, `pay_time`, `arrive_time`, `timestamp`, `adid`, `server_id`, `platform`, `region`, `sdk_type`, `pay_id`, `ext`, `date_time`";
    }

    @Override
    public String getTableName() {
        return "`base_recharge_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(sole_id == null ? null : sole_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(tp_order_id == null ? null : tp_order_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(sdkorderId == null ? null : sdkorderId.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(money).append("', ");
        strBuf.append("'").append(cn_money).append("', ");
        strBuf.append("'").append(goods_id == null ? null : goods_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(coin_num).append("', ");
        strBuf.append("'").append(status).append("', ");
        strBuf.append("'").append(pay_time).append("', ");
        strBuf.append("'").append(arrive_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(server_id).append("', ");
        strBuf.append("'").append(platform).append("', ");
        strBuf.append("'").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(sdk_type).append("', ");
        strBuf.append("'").append(pay_id == null ? null : pay_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(date_time).append("', ");
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

    // 全服唯一角色id
    public String getSoleId() { return this.sole_id; }
    public void setSoleId(BM _bm, String sole_id) {
        if(sole_id.equals(this.sole_id)) 
            return;
        this.sole_id = sole_id; 
        markField(_bm, FIELD_sole_id); 
    }
    public void saveSoleId(BM _bm, String sole_id) {
        if(sole_id.equals(this.sole_id)) 
            return;
        this.sole_id = sole_id;
        saveField(_bm, "sole_id", sole_id);
    }

    // 用户id
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

    // 第三方订单号
    public String getTpOrderId() { return this.tp_order_id; }
    public void setTpOrderId(BM _bm, String tp_order_id) {
        if(tp_order_id.equals(this.tp_order_id)) 
            return;
        this.tp_order_id = tp_order_id; 
        markField(_bm, FIELD_tp_order_id); 
    }
    public void saveTpOrderId(BM _bm, String tp_order_id) {
        if(tp_order_id.equals(this.tp_order_id)) 
            return;
        this.tp_order_id = tp_order_id;
        saveField(_bm, "tp_order_id", tp_order_id);
    }

    // SDK订单号
    public String getSdkorderId() { return this.sdkorderId; }
    public void setSdkorderId(BM _bm, String sdkorderId) {
        if(sdkorderId.equals(this.sdkorderId)) 
            return;
        this.sdkorderId = sdkorderId; 
        markField(_bm, FIELD_sdkorderId); 
    }
    public void saveSdkorderId(BM _bm, String sdkorderId) {
        if(sdkorderId.equals(this.sdkorderId)) 
            return;
        this.sdkorderId = sdkorderId;
        saveField(_bm, "sdkorderId", sdkorderId);
    }

    // 充值金额
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

    // 人民币金额
    public double getCnMoney() { return this.cn_money; }
    public void setCnMoney(BM _bm, double cn_money) {
        if(cn_money==this.cn_money) 
            return;
        this.cn_money = cn_money; 
        markField(_bm, FIELD_cn_money); 
    }
    public void saveCnMoney(BM _bm, double cn_money) {
        if(cn_money==this.cn_money) 
            return;
        this.cn_money = cn_money;
        saveField(_bm, "cn_money", cn_money);
    }

    // 充值货物id
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

    // 货币数量
    public int getCoinNum() { return this.coin_num; }
    public void setCoinNum(BM _bm, int coin_num) {
        if(coin_num==this.coin_num) 
            return;
        this.coin_num = coin_num; 
        markField(_bm, FIELD_coin_num); 
    }
    public void saveCoinNum(BM _bm, int coin_num) {
        if(coin_num==this.coin_num) 
            return;
        this.coin_num = coin_num;
        saveField(_bm, "coin_num", coin_num);
    }

    // 状态:充值=1
    public int getStatus() { return this.status; }
    public void setStatus(BM _bm, int status) {
        if(status==this.status) 
            return;
        this.status = status; 
        markField(_bm, FIELD_status); 
    }
    public void saveStatus(BM _bm, int status) {
        if(status==this.status) 
            return;
        this.status = status;
        saveField(_bm, "status", status);
    }

    // 充值时间戳（10位）
    public int getPayTime() { return this.pay_time; }
    public void setPayTime(BM _bm, int pay_time) {
        if(pay_time==this.pay_time) 
            return;
        this.pay_time = pay_time; 
        markField(_bm, FIELD_pay_time); 
    }
    public void savePayTime(BM _bm, int pay_time) {
        if(pay_time==this.pay_time) 
            return;
        this.pay_time = pay_time;
        saveField(_bm, "pay_time", pay_time);
    }

    // 到账时间戳（10位）
    public int getArriveTime() { return this.arrive_time; }
    public void setArriveTime(BM _bm, int arrive_time) {
        if(arrive_time==this.arrive_time) 
            return;
        this.arrive_time = arrive_time; 
        markField(_bm, FIELD_arrive_time); 
    }
    public void saveArriveTime(BM _bm, int arrive_time) {
        if(arrive_time==this.arrive_time) 
            return;
        this.arrive_time = arrive_time;
        saveField(_bm, "arrive_time", arrive_time);
    }

    // 创建时间戳（10位）
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

    // 设备id
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

    // 服务器id
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

    // 订单类型
    public short getSdkType() { return this.sdk_type; }
    public void setSdkType(BM _bm, short sdk_type) {
        if(sdk_type==this.sdk_type) 
            return;
        this.sdk_type = sdk_type; 
        markField(_bm, FIELD_sdk_type); 
    }
    public void saveSdkType(BM _bm, short sdk_type) {
        if(sdk_type==this.sdk_type) 
            return;
        this.sdk_type = sdk_type;
        saveField(_bm, "sdk_type", sdk_type);
    }

    // 支付方式
    public String getPayId() { return this.pay_id; }
    public void setPayId(BM _bm, String pay_id) {
        if(pay_id.equals(this.pay_id)) 
            return;
        this.pay_id = pay_id; 
        markField(_bm, FIELD_pay_id); 
    }
    public void savePayId(BM _bm, String pay_id) {
        if(pay_id.equals(this.pay_id)) 
            return;
        this.pay_id = pay_id;
        saveField(_bm, "pay_id", pay_id);
    }

    // 扩展字段：json格式
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

    // 日期
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `sole_id` = '").append(sole_id == null ? null : sole_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `order_id` = '").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `tp_order_id` = '").append(tp_order_id == null ? null : tp_order_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `sdkorderId` = '").append(sdkorderId == null ? null : sdkorderId.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `money` = '").append(money).append("',");
        sBuilder.append(" `cn_money` = '").append(cn_money).append("',");
        sBuilder.append(" `goods_id` = '").append(goods_id == null ? null : goods_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `coin_num` = '").append(coin_num).append("',");
        sBuilder.append(" `status` = '").append(status).append("',");
        sBuilder.append(" `pay_time` = '").append(pay_time).append("',");
        sBuilder.append(" `arrive_time` = '").append(arrive_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `adid` = '").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `server_id` = '").append(server_id).append("',");
        sBuilder.append(" `platform` = '").append(platform).append("',");
        sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `sdk_type` = '").append(sdk_type).append("',");
        sBuilder.append(" `pay_id` = '").append(pay_id == null ? null : pay_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_sole_id)) sBuilder.append(" `sole_id` = '").append(sole_id == null ? null : sole_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_order_id)) sBuilder.append(" `order_id` = '").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_tp_order_id)) sBuilder.append(" `tp_order_id` = '").append(tp_order_id == null ? null : tp_order_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_sdkorderId)) sBuilder.append(" `sdkorderId` = '").append(sdkorderId == null ? null : sdkorderId.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_money)) sBuilder.append(" `money` = '").append(money).append("',");
        if(isFieldMarked(FIELD_cn_money)) sBuilder.append(" `cn_money` = '").append(cn_money).append("',");
        if(isFieldMarked(FIELD_goods_id)) sBuilder.append(" `goods_id` = '").append(goods_id == null ? null : goods_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_coin_num)) sBuilder.append(" `coin_num` = '").append(coin_num).append("',");
        if(isFieldMarked(FIELD_status)) sBuilder.append(" `status` = '").append(status).append("',");
        if(isFieldMarked(FIELD_pay_time)) sBuilder.append(" `pay_time` = '").append(pay_time).append("',");
        if(isFieldMarked(FIELD_arrive_time)) sBuilder.append(" `arrive_time` = '").append(arrive_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_adid)) sBuilder.append(" `adid` = '").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_server_id)) sBuilder.append(" `server_id` = '").append(server_id).append("',");
        if(isFieldMarked(FIELD_platform)) sBuilder.append(" `platform` = '").append(platform).append("',");
        if(isFieldMarked(FIELD_region)) sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_sdk_type)) sBuilder.append(" `sdk_type` = '").append(sdk_type).append("',");
        if(isFieldMarked(FIELD_pay_id)) sBuilder.append(" `pay_id` = '").append(pay_id == null ? null : pay_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_ext)) sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `base_recharge_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`sole_id` varchar(32) NOT NULL DEFAULT '' COMMENT '全服唯一角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '用户id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`order_id` varchar(128) NOT NULL DEFAULT '' COMMENT '订单号',"
                + "`tp_order_id` varchar(128) NOT NULL DEFAULT '' COMMENT '第三方订单号',"
                + "`sdkorderId` varchar(128) NOT NULL DEFAULT '' COMMENT 'SDK订单号',"
                + "`money` decimal(20,2) NOT NULL DEFAULT '0.0' COMMENT '充值金额',"
                + "`cn_money` decimal(20,2) NOT NULL DEFAULT '0.0' COMMENT '人民币金额',"
                + "`goods_id` varchar(32) NOT NULL DEFAULT '' COMMENT '充值货物id',"
                + "`coin_num` int(11) NOT NULL DEFAULT '0' COMMENT '货币数量',"
                + "`status` int(11) NOT NULL DEFAULT '0' COMMENT '状态:充值=1',"
                + "`pay_time` int(11) NOT NULL DEFAULT '0' COMMENT '充值时间戳（10位）',"
                + "`arrive_time` int(11) NOT NULL DEFAULT '0' COMMENT '到账时间戳（10位）',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '创建时间戳（10位）',"
                + "`adid` varchar(50) NOT NULL DEFAULT '' COMMENT '设备id',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '账号归属的平台id',"
                + "`region` varchar(50) NOT NULL DEFAULT '' COMMENT '账号归属的区域id',"
                + "`sdk_type` smallint(10) NOT NULL DEFAULT '0' COMMENT '订单类型',"
                + "`pay_id` varchar(64) NOT NULL DEFAULT '' COMMENT '支付方式',"
                + "`ext` text NULL COMMENT '扩展字段：json格式',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-充值表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sole_id);//sole_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);//uid
        _size+=8;//cid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(order_id);//order_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(tp_order_id);//tp_order_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkorderId);//sdkorderId
        _size+=8;//money
        _size+=8;//cn_money
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(goods_id);//goods_id
        _size+=4;//coin_num
        _size+=4;//status
        _size+=4;//pay_time
        _size+=4;//arrive_time
        _size+=4;//timestamp
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adid);//adid
        _size+=4;//server_id
        _size+=4;//platform
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(region);//region
        _size+=4;//sdk_type
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(pay_id);//pay_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ext);//ext
        _size+=4;//date_time
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sole_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, uid);
        buff.putLong(cid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, order_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, tp_order_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sdkorderId);
        buff.putDouble(money);
        buff.putDouble(cn_money);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, goods_id);
        buff.putInt(coin_num);
        buff.putInt(status);
        buff.putInt(pay_time);
        buff.putInt(arrive_time);
        buff.putInt(timestamp);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adid);
        buff.putInt(server_id);
        buff.putInt(platform);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, region);
        buff.putShort(sdk_type);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, pay_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, ext);
        buff.putInt(date_time);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        sole_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        uid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        cid=buff.getLong();
        order_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        tp_order_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        sdkorderId=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        money=buff.getDouble();
        cn_money=buff.getDouble();
        goods_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        coin_num=buff.getInt();
        status=buff.getInt();
        pay_time=buff.getInt();
        arrive_time=buff.getInt();
        timestamp=buff.getInt();
        adid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        server_id=buff.getInt();
        platform=buff.getInt();
        region=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        sdk_type=buff.getShort();
        pay_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        ext=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        date_time=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
