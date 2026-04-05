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
public class BaseUserInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_sole_id =0;
    @DataBaseField(type = "varchar(32)", fieldname = "sole_id", comment = "项目唯一角色id")
    private String sole_id;

    public static final int FIELD_uid =1;
    @DataBaseField(type = "varchar(64)", fieldname = "uid", comment = "用户id")
    private String uid;

    public static final int FIELD_cid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "角色id")
    private long cid;

    public static final int FIELD_name =3;
    @DataBaseField(type = "varchar(50)", fieldname = "name", comment = "角色名")
    private String name;

    public static final int FIELD_create_time =4;
    @DataBaseField(type = "int(11)", fieldname = "create_time", comment = "创建-时间戳（10位）")
    private int create_time;

    public static final int FIELD_ar_time =5;
    @DataBaseField(type = "int(11)", fieldname = "ar_time", comment = "帐号注册-时间戳（10位）")
    private int ar_time;

    public static final int FIELD_ar_ip =6;
    @DataBaseField(type = "varchar(50)", fieldname = "ar_ip", comment = "账号注册ip")
    private String ar_ip;

    public static final int FIELD_afid =7;
    @DataBaseField(type = "varchar(300)", fieldname = "afid", comment = "广告afid")
    private String afid;

    public static final int FIELD_adid =8;
    @DataBaseField(type = "varchar(50)", fieldname = "adid", comment = "设备id")
    private String adid;

    public static final int FIELD_version =9;
    @DataBaseField(type = "varchar(64)", fieldname = "version", comment = "玩家登录时的客户端版本号")
    private String version;

    public static final int FIELD_nation =10;
    @DataBaseField(type = "varchar(100)", fieldname = "nation", comment = "国家(使用国际通用缩写英文比如：中国CN、加拿大CD)")
    private String nation;

    public static final int FIELD_adfrom =11;
    @DataBaseField(type = "varchar(50)", fieldname = "adfrom", comment = "一级渠道：无渠道时，苹果默认ios，安卓默认android,小程序：applet")
    private String adfrom;

    public static final int FIELD_adfrom2 =12;
    @DataBaseField(type = "varchar(50)", fieldname = "adfrom2", comment = "二级渠道名称：无渠道时，默认使用default")
    private String adfrom2;

    public static final int FIELD_server_id =13;
    @DataBaseField(type = "int(11)", fieldname = "server_id", comment = "创角所在的服务器id")
    private int server_id;

    public static final int FIELD_platform =14;
    @DataBaseField(type = "int(11)", fieldname = "platform", comment = "创角所在的平台id")
    private int platform;

    public static final int FIELD_region =15;
    @DataBaseField(type = "varchar(50)", fieldname = "region", comment = "创角所在的区域id")
    private String region;

    public static final int FIELD_ext =16;
    @DataBaseField(type = "text", fieldname = "ext", comment = "扩展字段：json格式")
    private String ext;

    public static final int FIELD_date_time =17;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public BaseUserInfoBO() {
        id = 0;
        sole_id = "";
        uid = "";
        cid = 0L;
        name = "";
        create_time = 0;
        ar_time = 0;
        ar_ip = "";
        afid = "";
        adid = "";
        version = "";
        nation = "";
        adfrom = "";
        adfrom2 = "";
        server_id = 0;
        platform = 0;
        region = "";
        ext = "";
        date_time = 0;
    }

    public BaseUserInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        sole_id = rs.getString(2);
        uid = rs.getString(3);
        cid = rs.getLong(4);
        name = rs.getString(5);
        create_time = rs.getInt(6);
        ar_time = rs.getInt(7);
        ar_ip = rs.getString(8);
        afid = rs.getString(9);
        adid = rs.getString(10);
        version = rs.getString(11);
        nation = rs.getString(12);
        adfrom = rs.getString(13);
        adfrom2 = rs.getString(14);
        server_id = rs.getInt(15);
        platform = rs.getInt(16);
        region = rs.getString(17);
        ext = rs.getString(18);
        date_time = rs.getInt(19);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new BaseUserInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `sole_id`, `uid`, `cid`, `name`, `create_time`, `ar_time`, `ar_ip`, `afid`, `adid`, `version`, `nation`, `adfrom`, `adfrom2`, `server_id`, `platform`, `region`, `ext`, `date_time`";
    }

    @Override
    public String getTableName() {
        return "`base_user_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(sole_id == null ? null : sole_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(create_time).append("', ");
        strBuf.append("'").append(ar_time).append("', ");
        strBuf.append("'").append(ar_ip == null ? null : ar_ip.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(afid == null ? null : afid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(version == null ? null : version.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(server_id).append("', ");
        strBuf.append("'").append(platform).append("', ");
        strBuf.append("'").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 项目唯一角色id
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

    // 角色名
    public String getName() { return this.name; }
    public void setName(BM _bm, String name) {
        if(name.equals(this.name)) 
            return;
        this.name = name; 
        markField(_bm, FIELD_name); 
    }
    public void saveName(BM _bm, String name) {
        if(name.equals(this.name)) 
            return;
        this.name = name;
        saveField(_bm, "name", name);
    }

    // 创建-时间戳（10位）
    public int getCreateTime() { return this.create_time; }
    public void setCreateTime(BM _bm, int create_time) {
        if(create_time==this.create_time) 
            return;
        this.create_time = create_time; 
        markField(_bm, FIELD_create_time); 
    }
    public void saveCreateTime(BM _bm, int create_time) {
        if(create_time==this.create_time) 
            return;
        this.create_time = create_time;
        saveField(_bm, "create_time", create_time);
    }

    // 帐号注册-时间戳（10位）
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

    // 账号注册ip
    public String getArIp() { return this.ar_ip; }
    public void setArIp(BM _bm, String ar_ip) {
        if(ar_ip.equals(this.ar_ip)) 
            return;
        this.ar_ip = ar_ip; 
        markField(_bm, FIELD_ar_ip); 
    }
    public void saveArIp(BM _bm, String ar_ip) {
        if(ar_ip.equals(this.ar_ip)) 
            return;
        this.ar_ip = ar_ip;
        saveField(_bm, "ar_ip", ar_ip);
    }

    // 广告afid
    public String getAfid() { return this.afid; }
    public void setAfid(BM _bm, String afid) {
        if(afid.equals(this.afid)) 
            return;
        this.afid = afid; 
        markField(_bm, FIELD_afid); 
    }
    public void saveAfid(BM _bm, String afid) {
        if(afid.equals(this.afid)) 
            return;
        this.afid = afid;
        saveField(_bm, "afid", afid);
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

    // 玩家登录时的客户端版本号
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

    // 国家(使用国际通用缩写英文比如：中国CN、加拿大CD)
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

    // 一级渠道：无渠道时，苹果默认ios，安卓默认android,小程序：applet
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

    // 二级渠道名称：无渠道时，默认使用default
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

    // 创角所在的服务器id
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

    // 创角所在的平台id
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

    // 创角所在的区域id
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
        sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `create_time` = '").append(create_time).append("',");
        sBuilder.append(" `ar_time` = '").append(ar_time).append("',");
        sBuilder.append(" `ar_ip` = '").append(ar_ip == null ? null : ar_ip.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `afid` = '").append(afid == null ? null : afid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adid` = '").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `version` = '").append(version == null ? null : version.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `nation` = '").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adfrom` = '").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `adfrom2` = '").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `server_id` = '").append(server_id).append("',");
        sBuilder.append(" `platform` = '").append(platform).append("',");
        sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
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
        if(isFieldMarked(FIELD_name)) sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_create_time)) sBuilder.append(" `create_time` = '").append(create_time).append("',");
        if(isFieldMarked(FIELD_ar_time)) sBuilder.append(" `ar_time` = '").append(ar_time).append("',");
        if(isFieldMarked(FIELD_ar_ip)) sBuilder.append(" `ar_ip` = '").append(ar_ip == null ? null : ar_ip.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_afid)) sBuilder.append(" `afid` = '").append(afid == null ? null : afid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adid)) sBuilder.append(" `adid` = '").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_version)) sBuilder.append(" `version` = '").append(version == null ? null : version.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_nation)) sBuilder.append(" `nation` = '").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adfrom)) sBuilder.append(" `adfrom` = '").append(adfrom == null ? null : adfrom.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_adfrom2)) sBuilder.append(" `adfrom2` = '").append(adfrom2 == null ? null : adfrom2.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_server_id)) sBuilder.append(" `server_id` = '").append(server_id).append("',");
        if(isFieldMarked(FIELD_platform)) sBuilder.append(" `platform` = '").append(platform).append("',");
        if(isFieldMarked(FIELD_region)) sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_ext)) sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `base_user_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`sole_id` varchar(32) NOT NULL DEFAULT '' COMMENT '项目唯一角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '用户id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`name` varchar(50) NOT NULL DEFAULT '' COMMENT '角色名',"
                + "`create_time` int(11) NOT NULL DEFAULT '0' COMMENT '创建-时间戳（10位）',"
                + "`ar_time` int(11) NOT NULL DEFAULT '0' COMMENT '帐号注册-时间戳（10位）',"
                + "`ar_ip` varchar(50) NOT NULL DEFAULT '' COMMENT '账号注册ip',"
                + "`afid` varchar(300) NOT NULL DEFAULT '' COMMENT '广告afid',"
                + "`adid` varchar(50) NOT NULL DEFAULT '' COMMENT '设备id',"
                + "`version` varchar(64) NOT NULL DEFAULT '' COMMENT '玩家登录时的客户端版本号',"
                + "`nation` varchar(100) NOT NULL DEFAULT '' COMMENT '国家(使用国际通用缩写英文比如：中国CN、加拿大CD)',"
                + "`adfrom` varchar(50) NOT NULL DEFAULT '' COMMENT '一级渠道：无渠道时，苹果默认ios，安卓默认android,小程序：applet',"
                + "`adfrom2` varchar(50) NOT NULL DEFAULT '' COMMENT '二级渠道名称：无渠道时，默认使用default',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '创角所在的服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '创角所在的平台id',"
                + "`region` varchar(50) NOT NULL DEFAULT '' COMMENT '创角所在的区域id',"
                + "`ext` text NULL COMMENT '扩展字段：json格式',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-用户信息表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);//name
        _size+=4;//create_time
        _size+=4;//ar_time
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ar_ip);//ar_ip
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(afid);//afid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adid);//adid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(version);//version
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(nation);//nation
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adfrom);//adfrom
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adfrom2);//adfrom2
        _size+=4;//server_id
        _size+=4;//platform
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(region);//region
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
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, name);
        buff.putInt(create_time);
        buff.putInt(ar_time);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, ar_ip);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, afid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, version);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, nation);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adfrom);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adfrom2);
        buff.putInt(server_id);
        buff.putInt(platform);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, region);
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
        name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        create_time=buff.getInt();
        ar_time=buff.getInt();
        ar_ip=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        afid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        version=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        nation=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adfrom=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        adfrom2=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        server_id=buff.getInt();
        platform=buff.getInt();
        region=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        ext=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        date_time=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
