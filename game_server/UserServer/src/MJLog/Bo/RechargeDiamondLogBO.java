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
public class RechargeDiamondLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_uid =0;
    @DataBaseField(type = "varchar(64)", fieldname = "uid", comment = "平台用户id")
    private String uid;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "角色id")
    private long cid;

    public static final int FIELD_server_id =2;
    @DataBaseField(type = "int(11)", fieldname = "server_id", comment = "服务器id")
    private int server_id;

    public static final int FIELD_platform =3;
    @DataBaseField(type = "int(11)", fieldname = "platform", comment = "平台id")
    private int platform;

    public static final int FIELD_region =4;
    @DataBaseField(type = "varchar(50)", fieldname = "region", comment = "区域id")
    private String region;

    public static final int FIELD_lv =5;
    @DataBaseField(type = "int(11)", fieldname = "lv", comment = "角色等级")
    private int lv;

    public static final int FIELD_vip_lv =6;
    @DataBaseField(type = "int(11)", fieldname = "vip_lv", comment = "VIP等级")
    private int vip_lv;

    public static final int FIELD_ar_time =7;
    @DataBaseField(type = "int(11)", fieldname = "ar_time", comment = "角色创建时间")
    private int ar_time;

    public static final int FIELD_nation =8;
    @DataBaseField(type = "varchar(50)", fieldname = "nation", comment = "国家")
    private String nation;

    public static final int FIELD_version =9;
    @DataBaseField(type = "varchar(50)", fieldname = "version", comment = "版本号")
    private String version;

    public static final int FIELD_adid =10;
    @DataBaseField(type = "varchar(50)", fieldname = "adid", comment = "设备id")
    private String adid;

    public static final int FIELD_goods_id =11;
    @DataBaseField(type = "varchar(32)", fieldname = "goods_id", comment = "商品id")
    private String goods_id;

    public static final int FIELD_event =12;
    @DataBaseField(type = "int(11)", fieldname = "event", comment = "事件ID")
    private int event;

    public static final int FIELD_oldNumber =13;
    @DataBaseField(type = "bigint(20)", fieldname = "oldNumber", comment = "旧值")
    private long oldNumber;

    public static final int FIELD_finalNumber =14;
    @DataBaseField(type = "bigint(20)", fieldname = "finalNumber", comment = "新值")
    private long finalNumber;

    public static final int FIELD_exchange =15;
    @DataBaseField(type = "bigint(20)", fieldname = "exchange", comment = "变化值")
    private long exchange;

    public static final int FIELD_sdk_type =16;
    @DataBaseField(type = "smallint(10)", fieldname = "sdk_type", comment = "订单类型")
    private short sdk_type;

    public static final int FIELD_action =17;
    @DataBaseField(type = "int(11)", fieldname = "action", comment = "操作类型")
    private int action;

    public static final int FIELD_timestamp =18;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_ext =19;
    @DataBaseField(type = "varchar(500)", fieldname = "ext", comment = "拓展参数")
    private String ext;

    public RechargeDiamondLogBO() {
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
        goods_id = "";
        event = 0;
        oldNumber = 0L;
        finalNumber = 0L;
        exchange = 0L;
        sdk_type = 0;
        action = 0;
        timestamp = 0;
        ext = "";
    }

    public RechargeDiamondLogBO(ResultSet rs) throws Exception {
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
        goods_id = rs.getString(13);
        event = rs.getInt(14);
        oldNumber = rs.getLong(15);
        finalNumber = rs.getLong(16);
        exchange = rs.getLong(17);
        sdk_type = rs.getShort(18);
        action = rs.getInt(19);
        timestamp = rs.getInt(20);
        ext = rs.getString(21);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new RechargeDiamondLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `uid`, `cid`, `server_id`, `platform`, `region`, `lv`, `vip_lv`, `ar_time`, `nation`, `version`, `adid`, `goods_id`, `event`, `oldNumber`, `finalNumber`, `exchange`, `sdk_type`, `action`, `timestamp`, `ext`";
    }

    @Override
    public String getTableName() {
        return "`recharge_diamond_log`";
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
        strBuf.append("'").append(goods_id == null ? null : goods_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(event).append("', ");
        strBuf.append("'").append(oldNumber).append("', ");
        strBuf.append("'").append(finalNumber).append("', ");
        strBuf.append("'").append(exchange).append("', ");
        strBuf.append("'").append(sdk_type).append("', ");
        strBuf.append("'").append(action).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 平台用户id
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

    // 平台id
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

    // 区域id
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

    // 角色等级
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

    // VIP等级
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

    // 角色创建时间
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

    // 国家
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

    // 版本号
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

    // 商品id
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

    // 事件ID
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

    // 旧值
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

    // 新值
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

    // 变化值
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

    // 操作类型
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

    // 时间戳
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

    // 拓展参数
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
        sBuilder.append(" `goods_id` = '").append(goods_id == null ? null : goods_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `event` = '").append(event).append("',");
        sBuilder.append(" `oldNumber` = '").append(oldNumber).append("',");
        sBuilder.append(" `finalNumber` = '").append(finalNumber).append("',");
        sBuilder.append(" `exchange` = '").append(exchange).append("',");
        sBuilder.append(" `sdk_type` = '").append(sdk_type).append("',");
        sBuilder.append(" `action` = '").append(action).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
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
        if(isFieldMarked(FIELD_goods_id)) sBuilder.append(" `goods_id` = '").append(goods_id == null ? null : goods_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_event)) sBuilder.append(" `event` = '").append(event).append("',");
        if(isFieldMarked(FIELD_oldNumber)) sBuilder.append(" `oldNumber` = '").append(oldNumber).append("',");
        if(isFieldMarked(FIELD_finalNumber)) sBuilder.append(" `finalNumber` = '").append(finalNumber).append("',");
        if(isFieldMarked(FIELD_exchange)) sBuilder.append(" `exchange` = '").append(exchange).append("',");
        if(isFieldMarked(FIELD_sdk_type)) sBuilder.append(" `sdk_type` = '").append(sdk_type).append("',");
        if(isFieldMarked(FIELD_action)) sBuilder.append(" `action` = '").append(action).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_ext)) sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `recharge_diamond_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '平台用户id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '平台id',"
                + "`region` varchar(50) NOT NULL DEFAULT '' COMMENT '区域id',"
                + "`lv` int(11) NOT NULL DEFAULT '0' COMMENT '角色等级',"
                + "`vip_lv` int(11) NOT NULL DEFAULT '0' COMMENT 'VIP等级',"
                + "`ar_time` int(11) NOT NULL DEFAULT '0' COMMENT '角色创建时间',"
                + "`nation` varchar(50) NOT NULL DEFAULT '' COMMENT '国家',"
                + "`version` varchar(50) NOT NULL DEFAULT '' COMMENT '版本号',"
                + "`adid` varchar(50) NOT NULL DEFAULT '' COMMENT '设备id',"
                + "`goods_id` varchar(32) NOT NULL DEFAULT '' COMMENT '商品id',"
                + "`event` int(11) NOT NULL DEFAULT '0' COMMENT '事件ID',"
                + "`oldNumber` bigint(20) NOT NULL DEFAULT '0' COMMENT '旧值',"
                + "`finalNumber` bigint(20) NOT NULL DEFAULT '0' COMMENT '新值',"
                + "`exchange` bigint(20) NOT NULL DEFAULT '0' COMMENT '变化值',"
                + "`sdk_type` smallint(10) NOT NULL DEFAULT '0' COMMENT '订单类型',"
                + "`action` int(11) NOT NULL DEFAULT '0' COMMENT '操作类型',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`ext` varchar(500) NOT NULL DEFAULT '' COMMENT '拓展参数',"
                + "KEY `cid` (`cid`),"
                + "KEY `event` (`event`),"
                + "KEY `timestamp` (`timestamp`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-钻石充值表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(goods_id);//goods_id
        _size+=4;//event
        _size+=8;//oldNumber
        _size+=8;//finalNumber
        _size+=8;//exchange
        _size+=4;//sdk_type
        _size+=4;//action
        _size+=4;//timestamp
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ext);//ext
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
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, goods_id);
        buff.putInt(event);
        buff.putLong(oldNumber);
        buff.putLong(finalNumber);
        buff.putLong(exchange);
        buff.putShort(sdk_type);
        buff.putInt(action);
        buff.putInt(timestamp);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, ext);        
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
        goods_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        event=buff.getInt();
        oldNumber=buff.getLong();
        finalNumber=buff.getLong();
        exchange=buff.getLong();
        sdk_type=buff.getShort();
        action=buff.getInt();
        timestamp=buff.getInt();
        ext=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
