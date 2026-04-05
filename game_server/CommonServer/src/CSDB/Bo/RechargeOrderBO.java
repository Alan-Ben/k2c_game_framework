package CSDB.Bo;
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
public class RechargeOrderBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_carrier =0;
    @DataBaseField(type = "varchar(256)", fieldname = "carrier", comment = "运营商")
    private String carrier;

    public static final int FIELD_platform =1;
    @DataBaseField(type = "varchar(256)", fieldname = "platform", comment = "平台ios，andriod，越狱")
    private String platform;

    public static final int FIELD_adfrom =2;
    @DataBaseField(type = "varchar(256)", fieldname = "adfrom", comment = "pp，360，qq 主来源1")
    private String adfrom;

    public static final int FIELD_adfrom2 =3;
    @DataBaseField(type = "varchar(256)", fieldname = "adfrom2", comment = "来源2")
    private String adfrom2;

    public static final int FIELD_gameid =4;
    @DataBaseField(type = "varchar(256)", fieldname = "gameid", comment = "游戏id")
    private String gameid;

    public static final int FIELD_server_id =5;
    @DataBaseField(type = "varchar(256)", fieldname = "server_id", comment = "服务器id")
    private String server_id;

    public static final int FIELD_appid =6;
    @DataBaseField(type = "varchar(256)", fieldname = "appid", comment = "")
    private String appid;

    public static final int FIELD_pid =7;
    @DataBaseField(type = "varchar(256)", fieldname = "pid", comment = "php平台id")
    private String pid;

    public static final int FIELD_uid =8;
    @DataBaseField(type = "bigint(20)", fieldname = "uid", comment = "玩家id")
    private long uid;

    public static final int FIELD_cporderid =9;
    @DataBaseField(type = "varchar(128)", fieldname = "cporderid", comment = "cp定单号,php正式的，唯一")
    private String cporderid;

    public static final int FIELD_adfrom_orderid =10;
    @DataBaseField(type = "varchar(256)", fieldname = "adfrom_orderid", comment = "渠道支付定单号")
    private String adfrom_orderid;

    public static final int FIELD_item =11;
    @DataBaseField(type = "varchar(256)", fieldname = "item", comment = "物品列表")
    private String item;

    public static final int FIELD_actiid =12;
    @DataBaseField(type = "varchar(256)", fieldname = "actiid", comment = " 活动列表")
    private String actiid;

    public static final int FIELD_crystal =13;
    @DataBaseField(type = "int(11)", fieldname = "crystal", comment = "钻石数量")
    private int crystal;

    public static final int FIELD_amount =14;
    @DataBaseField(type = "bigint(20)", fieldname = "amount", comment = "实际货币金额")
    private long amount;

    public static final int FIELD_usamount =15;
    @DataBaseField(type = "bigint(20)", fieldname = "usamount", comment = "实际美元金额")
    private long usamount;

    public static final int FIELD_goodname =16;
    @DataBaseField(type = "varchar(256)", fieldname = "goodname", comment = " 商品名称")
    private String goodname;

    public static final int FIELD_cpgoodid =17;
    @DataBaseField(type = "varchar(256)", fieldname = "cpgoodid", comment = " cp商品id")
    private String cpgoodid;

    public static final int FIELD_appgoodid =18;
    @DataBaseField(type = "varchar(256)", fieldname = "appgoodid", comment = " app商品id")
    private String appgoodid;

    public static final int FIELD_activityExtra =19;
    @DataBaseField(type = "int(11)", fieldname = "activityExtra", comment = "赠送发钻数量")
    private int activityExtra;

    public static final int FIELD_status =20;
    @DataBaseField(type = "varchar(20)", fieldname = "status", comment = " 状态")
    private String status;

    public static final int FIELD_orderTime =21;
    @DataBaseField(type = "int(11)", fieldname = "orderTime", comment = "到游戏服务器上时间")
    private int orderTime;

    public static final int FIELD_deliverTime =22;
    @DataBaseField(type = "int(11)", fieldname = "deliverTime", comment = "完成发送时间")
    private int deliverTime;

    public static final int FIELD_limitNum =23;
    @DataBaseField(type = "int(11)", fieldname = "limitNum", comment = "购买限制次数")
    private int limitNum;

    public static final int FIELD_resetLimitSec =24;
    @DataBaseField(type = "int(11)", fieldname = "resetLimitSec", comment = "重置限制时间，秒")
    private int resetLimitSec;

    public static final int FIELD_limitedItems =25;
    @DataBaseField(type = "varchar(256)", fieldname = "limitedItems", comment = "超出限制后给的物品列表")
    private String limitedItems;

    public static final int FIELD_gameextinfo =26;
    @DataBaseField(type = "varchar(256)", fieldname = "gameextinfo", comment = "游戏额外信息")
    private String gameextinfo;

    public static final int FIELD_err_code =27;
    @DataBaseField(type = "int(11)", fieldname = "err_code", comment = "处理错误码")
    private int err_code;

    public RechargeOrderBO() {
        id = 0;
        carrier = "";
        platform = "";
        adfrom = "";
        adfrom2 = "";
        gameid = "";
        server_id = "";
        appid = "";
        pid = "";
        uid = 0L;
        cporderid = "";
        adfrom_orderid = "";
        item = "";
        actiid = "";
        crystal = 0;
        amount = 0L;
        usamount = 0L;
        goodname = "";
        cpgoodid = "";
        appgoodid = "";
        activityExtra = 0;
        status = "";
        orderTime = 0;
        deliverTime = 0;
        limitNum = 0;
        resetLimitSec = 0;
        limitedItems = "";
        gameextinfo = "";
        err_code = 0;
    }

    public RechargeOrderBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        carrier = rs.getString(2);
        platform = rs.getString(3);
        adfrom = rs.getString(4);
        adfrom2 = rs.getString(5);
        gameid = rs.getString(6);
        server_id = rs.getString(7);
        appid = rs.getString(8);
        pid = rs.getString(9);
        uid = rs.getLong(10);
        cporderid = rs.getString(11);
        adfrom_orderid = rs.getString(12);
        item = rs.getString(13);
        actiid = rs.getString(14);
        crystal = rs.getInt(15);
        amount = rs.getLong(16);
        usamount = rs.getLong(17);
        goodname = rs.getString(18);
        cpgoodid = rs.getString(19);
        appgoodid = rs.getString(20);
        activityExtra = rs.getInt(21);
        status = rs.getString(22);
        orderTime = rs.getInt(23);
        deliverTime = rs.getInt(24);
        limitNum = rs.getInt(25);
        resetLimitSec = rs.getInt(26);
        limitedItems = rs.getString(27);
        gameextinfo = rs.getString(28);
        err_code = rs.getInt(29);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new RechargeOrderBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `carrier`, `platform`, `adfrom`, `adfrom2`, `gameid`, `server_id`, `appid`, `pid`, `uid`, `cporderid`, `adfrom_orderid`, `item`, `actiid`, `crystal`, `amount`, `usamount`, `goodname`, `cpgoodid`, `appgoodid`, `activityExtra`, `status`, `orderTime`, `deliverTime`, `limitNum`, `resetLimitSec`, `limitedItems`, `gameextinfo`, `err_code`";
    }

    @Override
    public String getTableName() {
        return "`recharge_order`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(carrier == null ? null : carrier.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(platform == null ? null : platform.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(gameid == null ? null : gameid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(server_id == null ? null : server_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(appid == null ? null : appid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(pid == null ? null : pid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(uid).append("', ");
        strBuf.append("'").append(cporderid == null ? null : cporderid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adfrom_orderid == null ? null : adfrom_orderid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(item == null ? null : item.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(actiid == null ? null : actiid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(crystal).append("', ");
        strBuf.append("'").append(amount).append("', ");
        strBuf.append("'").append(usamount).append("', ");
        strBuf.append("'").append(goodname == null ? null : goodname.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(cpgoodid == null ? null : cpgoodid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(appgoodid == null ? null : appgoodid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(activityExtra).append("', ");
        strBuf.append("'").append(status == null ? null : status.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(orderTime).append("', ");
        strBuf.append("'").append(deliverTime).append("', ");
        strBuf.append("'").append(limitNum).append("', ");
        strBuf.append("'").append(resetLimitSec).append("', ");
        strBuf.append("'").append(limitedItems == null ? null : limitedItems.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(gameextinfo == null ? null : gameextinfo.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(err_code).append("', ");
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

    // 运营商
    public String getCarrier() { return this.carrier; }
    public void setCarrier(BM _bm, String carrier) {
        if(carrier.equals(this.carrier)) 
            return;
        this.carrier = carrier; 
        markField(_bm, FIELD_carrier); 
    }
    public void saveCarrier(BM _bm, String carrier) {
        if(carrier.equals(this.carrier)) 
            return;
        this.carrier = carrier;
        saveField(_bm, "carrier", carrier);
    }

    // 平台ios，andriod，越狱
    public String getPlatform() { return this.platform; }
    public void setPlatform(BM _bm, String platform) {
        if(platform.equals(this.platform)) 
            return;
        this.platform = platform; 
        markField(_bm, FIELD_platform); 
    }
    public void savePlatform(BM _bm, String platform) {
        if(platform.equals(this.platform)) 
            return;
        this.platform = platform;
        saveField(_bm, "platform", platform);
    }

    // pp，360，qq 主来源1
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

    // 来源2
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

    // 游戏id
    public String getGameid() { return this.gameid; }
    public void setGameid(BM _bm, String gameid) {
        if(gameid.equals(this.gameid)) 
            return;
        this.gameid = gameid; 
        markField(_bm, FIELD_gameid); 
    }
    public void saveGameid(BM _bm, String gameid) {
        if(gameid.equals(this.gameid)) 
            return;
        this.gameid = gameid;
        saveField(_bm, "gameid", gameid);
    }

    // 服务器id
    public String getServerId() { return this.server_id; }
    public void setServerId(BM _bm, String server_id) {
        if(server_id.equals(this.server_id)) 
            return;
        this.server_id = server_id; 
        markField(_bm, FIELD_server_id); 
    }
    public void saveServerId(BM _bm, String server_id) {
        if(server_id.equals(this.server_id)) 
            return;
        this.server_id = server_id;
        saveField(_bm, "server_id", server_id);
    }

    // 
    public String getAppid() { return this.appid; }
    public void setAppid(BM _bm, String appid) {
        if(appid.equals(this.appid)) 
            return;
        this.appid = appid; 
        markField(_bm, FIELD_appid); 
    }
    public void saveAppid(BM _bm, String appid) {
        if(appid.equals(this.appid)) 
            return;
        this.appid = appid;
        saveField(_bm, "appid", appid);
    }

    // php平台id
    public String getPid() { return this.pid; }
    public void setPid(BM _bm, String pid) {
        if(pid.equals(this.pid)) 
            return;
        this.pid = pid; 
        markField(_bm, FIELD_pid); 
    }
    public void savePid(BM _bm, String pid) {
        if(pid.equals(this.pid)) 
            return;
        this.pid = pid;
        saveField(_bm, "pid", pid);
    }

    // 玩家id
    public long getUid() { return this.uid; }
    public void setUid(BM _bm, long uid) {
        if(uid==this.uid) 
            return;
        this.uid = uid; 
        markField(_bm, FIELD_uid); 
    }
    public void saveUid(BM _bm, long uid) {
        if(uid==this.uid) 
            return;
        this.uid = uid;
        saveField(_bm, "uid", uid);
    }

    // cp定单号,php正式的，唯一
    public String getCporderid() { return this.cporderid; }
    public void setCporderid(BM _bm, String cporderid) {
        if(cporderid.equals(this.cporderid)) 
            return;
        this.cporderid = cporderid; 
        markField(_bm, FIELD_cporderid); 
    }
    public void saveCporderid(BM _bm, String cporderid) {
        if(cporderid.equals(this.cporderid)) 
            return;
        this.cporderid = cporderid;
        saveField(_bm, "cporderid", cporderid);
    }

    // 渠道支付定单号
    public String getAdfromOrderid() { return this.adfrom_orderid; }
    public void setAdfromOrderid(BM _bm, String adfrom_orderid) {
        if(adfrom_orderid.equals(this.adfrom_orderid)) 
            return;
        this.adfrom_orderid = adfrom_orderid; 
        markField(_bm, FIELD_adfrom_orderid); 
    }
    public void saveAdfromOrderid(BM _bm, String adfrom_orderid) {
        if(adfrom_orderid.equals(this.adfrom_orderid)) 
            return;
        this.adfrom_orderid = adfrom_orderid;
        saveField(_bm, "adfrom_orderid", adfrom_orderid);
    }

    // 物品列表
    public String getItem() { return this.item; }
    public void setItem(BM _bm, String item) {
        if(item.equals(this.item)) 
            return;
        this.item = item; 
        markField(_bm, FIELD_item); 
    }
    public void saveItem(BM _bm, String item) {
        if(item.equals(this.item)) 
            return;
        this.item = item;
        saveField(_bm, "item", item);
    }

    //  活动列表
    public String getActiid() { return this.actiid; }
    public void setActiid(BM _bm, String actiid) {
        if(actiid.equals(this.actiid)) 
            return;
        this.actiid = actiid; 
        markField(_bm, FIELD_actiid); 
    }
    public void saveActiid(BM _bm, String actiid) {
        if(actiid.equals(this.actiid)) 
            return;
        this.actiid = actiid;
        saveField(_bm, "actiid", actiid);
    }

    // 钻石数量
    public int getCrystal() { return this.crystal; }
    public void setCrystal(BM _bm, int crystal) {
        if(crystal==this.crystal) 
            return;
        this.crystal = crystal; 
        markField(_bm, FIELD_crystal); 
    }
    public void saveCrystal(BM _bm, int crystal) {
        if(crystal==this.crystal) 
            return;
        this.crystal = crystal;
        saveField(_bm, "crystal", crystal);
    }

    // 实际货币金额
    public long getAmount() { return this.amount; }
    public void setAmount(BM _bm, long amount) {
        if(amount==this.amount) 
            return;
        this.amount = amount; 
        markField(_bm, FIELD_amount); 
    }
    public void saveAmount(BM _bm, long amount) {
        if(amount==this.amount) 
            return;
        this.amount = amount;
        saveField(_bm, "amount", amount);
    }

    // 实际美元金额
    public long getUsamount() { return this.usamount; }
    public void setUsamount(BM _bm, long usamount) {
        if(usamount==this.usamount) 
            return;
        this.usamount = usamount; 
        markField(_bm, FIELD_usamount); 
    }
    public void saveUsamount(BM _bm, long usamount) {
        if(usamount==this.usamount) 
            return;
        this.usamount = usamount;
        saveField(_bm, "usamount", usamount);
    }

    //  商品名称
    public String getGoodname() { return this.goodname; }
    public void setGoodname(BM _bm, String goodname) {
        if(goodname.equals(this.goodname)) 
            return;
        this.goodname = goodname; 
        markField(_bm, FIELD_goodname); 
    }
    public void saveGoodname(BM _bm, String goodname) {
        if(goodname.equals(this.goodname)) 
            return;
        this.goodname = goodname;
        saveField(_bm, "goodname", goodname);
    }

    //  cp商品id
    public String getCpgoodid() { return this.cpgoodid; }
    public void setCpgoodid(BM _bm, String cpgoodid) {
        if(cpgoodid.equals(this.cpgoodid)) 
            return;
        this.cpgoodid = cpgoodid; 
        markField(_bm, FIELD_cpgoodid); 
    }
    public void saveCpgoodid(BM _bm, String cpgoodid) {
        if(cpgoodid.equals(this.cpgoodid)) 
            return;
        this.cpgoodid = cpgoodid;
        saveField(_bm, "cpgoodid", cpgoodid);
    }

    //  app商品id
    public String getAppgoodid() { return this.appgoodid; }
    public void setAppgoodid(BM _bm, String appgoodid) {
        if(appgoodid.equals(this.appgoodid)) 
            return;
        this.appgoodid = appgoodid; 
        markField(_bm, FIELD_appgoodid); 
    }
    public void saveAppgoodid(BM _bm, String appgoodid) {
        if(appgoodid.equals(this.appgoodid)) 
            return;
        this.appgoodid = appgoodid;
        saveField(_bm, "appgoodid", appgoodid);
    }

    // 赠送发钻数量
    public int getActivityExtra() { return this.activityExtra; }
    public void setActivityExtra(BM _bm, int activityExtra) {
        if(activityExtra==this.activityExtra) 
            return;
        this.activityExtra = activityExtra; 
        markField(_bm, FIELD_activityExtra); 
    }
    public void saveActivityExtra(BM _bm, int activityExtra) {
        if(activityExtra==this.activityExtra) 
            return;
        this.activityExtra = activityExtra;
        saveField(_bm, "activityExtra", activityExtra);
    }

    //  状态
    public String getStatus() { return this.status; }
    public void setStatus(BM _bm, String status) {
        if(status.equals(this.status)) 
            return;
        this.status = status; 
        markField(_bm, FIELD_status); 
    }
    public void saveStatus(BM _bm, String status) {
        if(status.equals(this.status)) 
            return;
        this.status = status;
        saveField(_bm, "status", status);
    }

    // 到游戏服务器上时间
    public int getOrderTime() { return this.orderTime; }
    public void setOrderTime(BM _bm, int orderTime) {
        if(orderTime==this.orderTime) 
            return;
        this.orderTime = orderTime; 
        markField(_bm, FIELD_orderTime); 
    }
    public void saveOrderTime(BM _bm, int orderTime) {
        if(orderTime==this.orderTime) 
            return;
        this.orderTime = orderTime;
        saveField(_bm, "orderTime", orderTime);
    }

    // 完成发送时间
    public int getDeliverTime() { return this.deliverTime; }
    public void setDeliverTime(BM _bm, int deliverTime) {
        if(deliverTime==this.deliverTime) 
            return;
        this.deliverTime = deliverTime; 
        markField(_bm, FIELD_deliverTime); 
    }
    public void saveDeliverTime(BM _bm, int deliverTime) {
        if(deliverTime==this.deliverTime) 
            return;
        this.deliverTime = deliverTime;
        saveField(_bm, "deliverTime", deliverTime);
    }

    // 购买限制次数
    public int getLimitNum() { return this.limitNum; }
    public void setLimitNum(BM _bm, int limitNum) {
        if(limitNum==this.limitNum) 
            return;
        this.limitNum = limitNum; 
        markField(_bm, FIELD_limitNum); 
    }
    public void saveLimitNum(BM _bm, int limitNum) {
        if(limitNum==this.limitNum) 
            return;
        this.limitNum = limitNum;
        saveField(_bm, "limitNum", limitNum);
    }

    // 重置限制时间，秒
    public int getResetLimitSec() { return this.resetLimitSec; }
    public void setResetLimitSec(BM _bm, int resetLimitSec) {
        if(resetLimitSec==this.resetLimitSec) 
            return;
        this.resetLimitSec = resetLimitSec; 
        markField(_bm, FIELD_resetLimitSec); 
    }
    public void saveResetLimitSec(BM _bm, int resetLimitSec) {
        if(resetLimitSec==this.resetLimitSec) 
            return;
        this.resetLimitSec = resetLimitSec;
        saveField(_bm, "resetLimitSec", resetLimitSec);
    }

    // 超出限制后给的物品列表
    public String getLimitedItems() { return this.limitedItems; }
    public void setLimitedItems(BM _bm, String limitedItems) {
        if(limitedItems.equals(this.limitedItems)) 
            return;
        this.limitedItems = limitedItems; 
        markField(_bm, FIELD_limitedItems); 
    }
    public void saveLimitedItems(BM _bm, String limitedItems) {
        if(limitedItems.equals(this.limitedItems)) 
            return;
        this.limitedItems = limitedItems;
        saveField(_bm, "limitedItems", limitedItems);
    }

    // 游戏额外信息
    public String getGameextinfo() { return this.gameextinfo; }
    public void setGameextinfo(BM _bm, String gameextinfo) {
        if(gameextinfo.equals(this.gameextinfo)) 
            return;
        this.gameextinfo = gameextinfo; 
        markField(_bm, FIELD_gameextinfo); 
    }
    public void saveGameextinfo(BM _bm, String gameextinfo) {
        if(gameextinfo.equals(this.gameextinfo)) 
            return;
        this.gameextinfo = gameextinfo;
        saveField(_bm, "gameextinfo", gameextinfo);
    }

    // 处理错误码
    public int getErrCode() { return this.err_code; }
    public void setErrCode(BM _bm, int err_code) {
        if(err_code==this.err_code) 
            return;
        this.err_code = err_code; 
        markField(_bm, FIELD_err_code); 
    }
    public void saveErrCode(BM _bm, int err_code) {
        if(err_code==this.err_code) 
            return;
        this.err_code = err_code;
        saveField(_bm, "err_code", err_code);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `carrier` = '").append(carrier == null ? null : carrier.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `platform` = '").append(platform == null ? null : platform.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adfrom` = '").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adfrom2` = '").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `gameid` = '").append(gameid == null ? null : gameid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `server_id` = '").append(server_id == null ? null : server_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `appid` = '").append(appid == null ? null : appid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `pid` = '").append(pid == null ? null : pid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `uid` = '").append(uid).append("',");
        sBuilder.append(" `cporderid` = '").append(cporderid == null ? null : cporderid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adfrom_orderid` = '").append(adfrom_orderid == null ? null : adfrom_orderid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `item` = '").append(item == null ? null : item.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `actiid` = '").append(actiid == null ? null : actiid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `crystal` = '").append(crystal).append("',");
        sBuilder.append(" `amount` = '").append(amount).append("',");
        sBuilder.append(" `usamount` = '").append(usamount).append("',");
        sBuilder.append(" `goodname` = '").append(goodname == null ? null : goodname.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `cpgoodid` = '").append(cpgoodid == null ? null : cpgoodid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `appgoodid` = '").append(appgoodid == null ? null : appgoodid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `activityExtra` = '").append(activityExtra).append("',");
        sBuilder.append(" `status` = '").append(status == null ? null : status.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `orderTime` = '").append(orderTime).append("',");
        sBuilder.append(" `deliverTime` = '").append(deliverTime).append("',");
        sBuilder.append(" `limitNum` = '").append(limitNum).append("',");
        sBuilder.append(" `resetLimitSec` = '").append(resetLimitSec).append("',");
        sBuilder.append(" `limitedItems` = '").append(limitedItems == null ? null : limitedItems.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `gameextinfo` = '").append(gameextinfo == null ? null : gameextinfo.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `err_code` = '").append(err_code).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_carrier)) sBuilder.append(" `carrier` = '").append(carrier == null ? null : carrier.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_platform)) sBuilder.append(" `platform` = '").append(platform == null ? null : platform.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adfrom)) sBuilder.append(" `adfrom` = '").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adfrom2)) sBuilder.append(" `adfrom2` = '").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_gameid)) sBuilder.append(" `gameid` = '").append(gameid == null ? null : gameid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_server_id)) sBuilder.append(" `server_id` = '").append(server_id == null ? null : server_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_appid)) sBuilder.append(" `appid` = '").append(appid == null ? null : appid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_pid)) sBuilder.append(" `pid` = '").append(pid == null ? null : pid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid).append("',");
        if(isFieldMarked(FIELD_cporderid)) sBuilder.append(" `cporderid` = '").append(cporderid == null ? null : cporderid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adfrom_orderid)) sBuilder.append(" `adfrom_orderid` = '").append(adfrom_orderid == null ? null : adfrom_orderid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_item)) sBuilder.append(" `item` = '").append(item == null ? null : item.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_actiid)) sBuilder.append(" `actiid` = '").append(actiid == null ? null : actiid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_crystal)) sBuilder.append(" `crystal` = '").append(crystal).append("',");
        if(isFieldMarked(FIELD_amount)) sBuilder.append(" `amount` = '").append(amount).append("',");
        if(isFieldMarked(FIELD_usamount)) sBuilder.append(" `usamount` = '").append(usamount).append("',");
        if(isFieldMarked(FIELD_goodname)) sBuilder.append(" `goodname` = '").append(goodname == null ? null : goodname.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_cpgoodid)) sBuilder.append(" `cpgoodid` = '").append(cpgoodid == null ? null : cpgoodid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_appgoodid)) sBuilder.append(" `appgoodid` = '").append(appgoodid == null ? null : appgoodid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_activityExtra)) sBuilder.append(" `activityExtra` = '").append(activityExtra).append("',");
        if(isFieldMarked(FIELD_status)) sBuilder.append(" `status` = '").append(status == null ? null : status.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_orderTime)) sBuilder.append(" `orderTime` = '").append(orderTime).append("',");
        if(isFieldMarked(FIELD_deliverTime)) sBuilder.append(" `deliverTime` = '").append(deliverTime).append("',");
        if(isFieldMarked(FIELD_limitNum)) sBuilder.append(" `limitNum` = '").append(limitNum).append("',");
        if(isFieldMarked(FIELD_resetLimitSec)) sBuilder.append(" `resetLimitSec` = '").append(resetLimitSec).append("',");
        if(isFieldMarked(FIELD_limitedItems)) sBuilder.append(" `limitedItems` = '").append(limitedItems == null ? null : limitedItems.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_gameextinfo)) sBuilder.append(" `gameextinfo` = '").append(gameextinfo == null ? null : gameextinfo.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_err_code)) sBuilder.append(" `err_code` = '").append(err_code).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `recharge_order` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`carrier` varchar(256) NOT NULL DEFAULT '' COMMENT '运营商',"
                + "`platform` varchar(256) NOT NULL DEFAULT '' COMMENT '平台ios，andriod，越狱',"
                + "`adfrom` varchar(256) NOT NULL DEFAULT '' COMMENT 'pp，360，qq 主来源1',"
                + "`adfrom2` varchar(256) NOT NULL DEFAULT '' COMMENT '来源2',"
                + "`gameid` varchar(256) NOT NULL DEFAULT '' COMMENT '游戏id',"
                + "`server_id` varchar(256) NOT NULL DEFAULT '' COMMENT '服务器id',"
                + "`appid` varchar(256) NOT NULL DEFAULT '' COMMENT '',"
                + "`pid` varchar(256) NOT NULL DEFAULT '' COMMENT 'php平台id',"
                + "`uid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家id',"
                + "`cporderid` varchar(128) NOT NULL DEFAULT '' COMMENT 'cp定单号,php正式的，唯一',"
                + "`adfrom_orderid` varchar(256) NOT NULL DEFAULT '' COMMENT '渠道支付定单号',"
                + "`item` varchar(256) NOT NULL DEFAULT '' COMMENT '物品列表',"
                + "`actiid` varchar(256) NOT NULL DEFAULT '' COMMENT ' 活动列表',"
                + "`crystal` int(11) NOT NULL DEFAULT '0' COMMENT '钻石数量',"
                + "`amount` bigint(20) NOT NULL DEFAULT '0' COMMENT '实际货币金额',"
                + "`usamount` bigint(20) NOT NULL DEFAULT '0' COMMENT '实际美元金额',"
                + "`goodname` varchar(256) NOT NULL DEFAULT '' COMMENT ' 商品名称',"
                + "`cpgoodid` varchar(256) NOT NULL DEFAULT '' COMMENT ' cp商品id',"
                + "`appgoodid` varchar(256) NOT NULL DEFAULT '' COMMENT ' app商品id',"
                + "`activityExtra` int(11) NOT NULL DEFAULT '0' COMMENT '赠送发钻数量',"
                + "`status` varchar(20) NOT NULL DEFAULT '' COMMENT ' 状态',"
                + "`orderTime` int(11) NOT NULL DEFAULT '0' COMMENT '到游戏服务器上时间',"
                + "`deliverTime` int(11) NOT NULL DEFAULT '0' COMMENT '完成发送时间',"
                + "`limitNum` int(11) NOT NULL DEFAULT '0' COMMENT '购买限制次数',"
                + "`resetLimitSec` int(11) NOT NULL DEFAULT '0' COMMENT '重置限制时间，秒',"
                + "`limitedItems` varchar(256) NOT NULL DEFAULT '' COMMENT '超出限制后给的物品列表',"
                + "`gameextinfo` varchar(256) NOT NULL DEFAULT '' COMMENT '游戏额外信息',"
                + "`err_code` int(11) NOT NULL DEFAULT '0' COMMENT '处理错误码',"
                + "KEY `status` (`status`),"
                + "UNIQUE INDEX `cporderid` (`cporderid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='充值订单表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.comm_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(carrier);//carrier
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(platform);//platform
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adfrom);//adfrom
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adfrom2);//adfrom2
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(gameid);//gameid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(server_id);//server_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(appid);//appid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(pid);//pid
        _size+=8;//uid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cporderid);//cporderid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adfrom_orderid);//adfrom_orderid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(item);//item
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(actiid);//actiid
        _size+=4;//crystal
        _size+=8;//amount
        _size+=8;//usamount
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(goodname);//goodname
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cpgoodid);//cpgoodid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(appgoodid);//appgoodid
        _size+=4;//activityExtra
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(status);//status
        _size+=4;//orderTime
        _size+=4;//deliverTime
        _size+=4;//limitNum
        _size+=4;//resetLimitSec
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(limitedItems);//limitedItems
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(gameextinfo);//gameextinfo
        _size+=4;//err_code
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, carrier);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, platform);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adfrom);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adfrom2);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, gameid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, server_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, appid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, pid);
        buff.putLong(uid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, cporderid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adfrom_orderid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, item);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, actiid);
        buff.putInt(crystal);
        buff.putLong(amount);
        buff.putLong(usamount);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, goodname);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, cpgoodid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, appgoodid);
        buff.putInt(activityExtra);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, status);
        buff.putInt(orderTime);
        buff.putInt(deliverTime);
        buff.putInt(limitNum);
        buff.putInt(resetLimitSec);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, limitedItems);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, gameextinfo);
        buff.putInt(err_code);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        carrier=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        platform=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adfrom=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adfrom2=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        gameid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        server_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        appid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        pid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        uid=buff.getLong();
        cporderid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adfrom_orderid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        item=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        actiid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        crystal=buff.getInt();
        amount=buff.getLong();
        usamount=buff.getLong();
        goodname=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        cpgoodid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        appgoodid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        activityExtra=buff.getInt();
        status=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        orderTime=buff.getInt();
        deliverTime=buff.getInt();
        limitNum=buff.getInt();
        resetLimitSec=buff.getInt();
        limitedItems=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        gameextinfo=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        err_code=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
