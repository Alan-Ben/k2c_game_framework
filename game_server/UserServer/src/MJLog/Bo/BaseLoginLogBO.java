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
public class BaseLoginLogBO extends BaseBO {
    
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

    public static final int FIELD_operation =3;
    @DataBaseField(type = "int(11)", fieldname = "operation", comment = "2登录,3登出")
    private int operation;

    public static final int FIELD_login_ip =4;
    @DataBaseField(type = "varchar(50)", fieldname = "login_ip", comment = "登入时的ip")
    private String login_ip;

    public static final int FIELD_timestamp =5;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "登入时的unix时间戳（10位）")
    private int timestamp;

    public static final int FIELD_adid =6;
    @DataBaseField(type = "varchar(50)", fieldname = "adid", comment = "登录时的手机设备id")
    private String adid;

    public static final int FIELD_version =7;
    @DataBaseField(type = "varchar(64)", fieldname = "version", comment = "玩家登录时的客户端版本号")
    private String version;

    public static final int FIELD_server_id =8;
    @DataBaseField(type = "int(11)", fieldname = "server_id", comment = "玩家登录时的服务器id")
    private int server_id;

    public static final int FIELD_platform =9;
    @DataBaseField(type = "int(11)", fieldname = "platform", comment = "账号归属的平台id")
    private int platform;

    public static final int FIELD_region =10;
    @DataBaseField(type = "varchar(50)", fieldname = "region", comment = "账号归属的区域id")
    private String region;

    public static final int FIELD_ext =11;
    @DataBaseField(type = "text", fieldname = "ext", comment = "扩展字段：json格式")
    private String ext;

    public static final int FIELD_date_time =12;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_main_quest_progress =13;
    @DataBaseField(type = "bigint(20)", fieldname = "main_quest_progress", comment = "主线任务进度")
    private long main_quest_progress;

    public static final int FIELD_chapter_progress =14;
    @DataBaseField(type = "bigint(20)", fieldname = "chapter_progress", comment = "关卡进度：关卡id*1000+格子索引")
    private long chapter_progress;

    public static final int FIELD_level =15;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public static final int FIELD_crystal =16;
    @DataBaseField(type = "bigint(20)", fieldname = "crystal", comment = "钻石")
    private long crystal;

    public static final int FIELD_create_date =17;
    @DataBaseField(type = "int(11)", fieldname = "create_date", comment = "玩家创角日期")
    private int create_date;

    public static final int FIELD_total_power =18;
    @DataBaseField(type = "bigint(20)", fieldname = "total_power", comment = "大臣总实力")
    private long total_power;

    public static final int FIELD_earnings =19;
    @DataBaseField(type = "bigint(20)", fieldname = "earnings", comment = "玩家总赚速")
    private long earnings;

    public BaseLoginLogBO() {
        id = 0;
        sole_id = "";
        uid = "";
        cid = 0L;
        operation = 0;
        login_ip = "";
        timestamp = 0;
        adid = "";
        version = "";
        server_id = 0;
        platform = 0;
        region = "";
        ext = "";
        date_time = 0;
        main_quest_progress = 0L;
        chapter_progress = 0L;
        level = 0;
        crystal = 0L;
        create_date = 0;
        total_power = 0L;
        earnings = 0L;
    }

    public BaseLoginLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        sole_id = rs.getString(2);
        uid = rs.getString(3);
        cid = rs.getLong(4);
        operation = rs.getInt(5);
        login_ip = rs.getString(6);
        timestamp = rs.getInt(7);
        adid = rs.getString(8);
        version = rs.getString(9);
        server_id = rs.getInt(10);
        platform = rs.getInt(11);
        region = rs.getString(12);
        ext = rs.getString(13);
        date_time = rs.getInt(14);
        main_quest_progress = rs.getLong(15);
        chapter_progress = rs.getLong(16);
        level = rs.getInt(17);
        crystal = rs.getLong(18);
        create_date = rs.getInt(19);
        total_power = rs.getLong(20);
        earnings = rs.getLong(21);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new BaseLoginLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `sole_id`, `uid`, `cid`, `operation`, `login_ip`, `timestamp`, `adid`, `version`, `server_id`, `platform`, `region`, `ext`, `date_time`, `main_quest_progress`, `chapter_progress`, `level`, `crystal`, `create_date`, `total_power`, `earnings`";
    }

    @Override
    public String getTableName() {
        return "`base_login_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(sole_id == null ? null : sole_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(operation).append("', ");
        strBuf.append("'").append(login_ip == null ? null : login_ip.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(version == null ? null : version.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(server_id).append("', ");
        strBuf.append("'").append(platform).append("', ");
        strBuf.append("'").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(main_quest_progress).append("', ");
        strBuf.append("'").append(chapter_progress).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(crystal).append("', ");
        strBuf.append("'").append(create_date).append("', ");
        strBuf.append("'").append(total_power).append("', ");
        strBuf.append("'").append(earnings).append("', ");
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

    // 2登录,3登出
    public int getOperation() { return this.operation; }
    public void setOperation(BM _bm, int operation) {
        if(operation==this.operation) 
            return;
        this.operation = operation; 
        markField(_bm, FIELD_operation); 
    }
    public void saveOperation(BM _bm, int operation) {
        if(operation==this.operation) 
            return;
        this.operation = operation;
        saveField(_bm, "operation", operation);
    }

    // 登入时的ip
    public String getLoginIp() { return this.login_ip; }
    public void setLoginIp(BM _bm, String login_ip) {
        if(login_ip.equals(this.login_ip)) 
            return;
        this.login_ip = login_ip; 
        markField(_bm, FIELD_login_ip); 
    }
    public void saveLoginIp(BM _bm, String login_ip) {
        if(login_ip.equals(this.login_ip)) 
            return;
        this.login_ip = login_ip;
        saveField(_bm, "login_ip", login_ip);
    }

    // 登入时的unix时间戳（10位）
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

    // 登录时的手机设备id
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

    // 主线任务进度
    public long getMainQuestProgress() { return this.main_quest_progress; }
    public void setMainQuestProgress(BM _bm, long main_quest_progress) {
        if(main_quest_progress==this.main_quest_progress) 
            return;
        this.main_quest_progress = main_quest_progress; 
        markField(_bm, FIELD_main_quest_progress); 
    }
    public void saveMainQuestProgress(BM _bm, long main_quest_progress) {
        if(main_quest_progress==this.main_quest_progress) 
            return;
        this.main_quest_progress = main_quest_progress;
        saveField(_bm, "main_quest_progress", main_quest_progress);
    }

    // 关卡进度：关卡id*1000+格子索引
    public long getChapterProgress() { return this.chapter_progress; }
    public void setChapterProgress(BM _bm, long chapter_progress) {
        if(chapter_progress==this.chapter_progress) 
            return;
        this.chapter_progress = chapter_progress; 
        markField(_bm, FIELD_chapter_progress); 
    }
    public void saveChapterProgress(BM _bm, long chapter_progress) {
        if(chapter_progress==this.chapter_progress) 
            return;
        this.chapter_progress = chapter_progress;
        saveField(_bm, "chapter_progress", chapter_progress);
    }

    // 等级
    public int getLevel() { return this.level; }
    public void setLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level; 
        markField(_bm, FIELD_level); 
    }
    public void saveLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level;
        saveField(_bm, "level", level);
    }

    // 钻石
    public long getCrystal() { return this.crystal; }
    public void setCrystal(BM _bm, long crystal) {
        if(crystal==this.crystal) 
            return;
        this.crystal = crystal; 
        markField(_bm, FIELD_crystal); 
    }
    public void saveCrystal(BM _bm, long crystal) {
        if(crystal==this.crystal) 
            return;
        this.crystal = crystal;
        saveField(_bm, "crystal", crystal);
    }

    // 玩家创角日期
    public int getCreateDate() { return this.create_date; }
    public void setCreateDate(BM _bm, int create_date) {
        if(create_date==this.create_date) 
            return;
        this.create_date = create_date; 
        markField(_bm, FIELD_create_date); 
    }
    public void saveCreateDate(BM _bm, int create_date) {
        if(create_date==this.create_date) 
            return;
        this.create_date = create_date;
        saveField(_bm, "create_date", create_date);
    }

    // 大臣总实力
    public long getTotalPower() { return this.total_power; }
    public void setTotalPower(BM _bm, long total_power) {
        if(total_power==this.total_power) 
            return;
        this.total_power = total_power; 
        markField(_bm, FIELD_total_power); 
    }
    public void saveTotalPower(BM _bm, long total_power) {
        if(total_power==this.total_power) 
            return;
        this.total_power = total_power;
        saveField(_bm, "total_power", total_power);
    }

    // 玩家总赚速
    public long getEarnings() { return this.earnings; }
    public void setEarnings(BM _bm, long earnings) {
        if(earnings==this.earnings) 
            return;
        this.earnings = earnings; 
        markField(_bm, FIELD_earnings); 
    }
    public void saveEarnings(BM _bm, long earnings) {
        if(earnings==this.earnings) 
            return;
        this.earnings = earnings;
        saveField(_bm, "earnings", earnings);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `sole_id` = '").append(sole_id == null ? null : sole_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `operation` = '").append(operation).append("',");
        sBuilder.append(" `login_ip` = '").append(login_ip == null ? null : login_ip.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `adid` = '").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `version` = '").append(version == null ? null : version.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `server_id` = '").append(server_id).append("',");
        sBuilder.append(" `platform` = '").append(platform).append("',");
        sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `main_quest_progress` = '").append(main_quest_progress).append("',");
        sBuilder.append(" `chapter_progress` = '").append(chapter_progress).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `crystal` = '").append(crystal).append("',");
        sBuilder.append(" `create_date` = '").append(create_date).append("',");
        sBuilder.append(" `total_power` = '").append(total_power).append("',");
        sBuilder.append(" `earnings` = '").append(earnings).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_sole_id)) sBuilder.append(" `sole_id` = '").append(sole_id == null ? null : sole_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_operation)) sBuilder.append(" `operation` = '").append(operation).append("',");
        if(isFieldMarked(FIELD_login_ip)) sBuilder.append(" `login_ip` = '").append(login_ip == null ? null : login_ip.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_adid)) sBuilder.append(" `adid` = '").append(adid == null ? null : adid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_version)) sBuilder.append(" `version` = '").append(version == null ? null : version.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_server_id)) sBuilder.append(" `server_id` = '").append(server_id).append("',");
        if(isFieldMarked(FIELD_platform)) sBuilder.append(" `platform` = '").append(platform).append("',");
        if(isFieldMarked(FIELD_region)) sBuilder.append(" `region` = '").append(region == null ? null : region.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_ext)) sBuilder.append(" `ext` = '").append(ext == null ? null : ext.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_main_quest_progress)) sBuilder.append(" `main_quest_progress` = '").append(main_quest_progress).append("',");
        if(isFieldMarked(FIELD_chapter_progress)) sBuilder.append(" `chapter_progress` = '").append(chapter_progress).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_crystal)) sBuilder.append(" `crystal` = '").append(crystal).append("',");
        if(isFieldMarked(FIELD_create_date)) sBuilder.append(" `create_date` = '").append(create_date).append("',");
        if(isFieldMarked(FIELD_total_power)) sBuilder.append(" `total_power` = '").append(total_power).append("',");
        if(isFieldMarked(FIELD_earnings)) sBuilder.append(" `earnings` = '").append(earnings).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `base_login_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`sole_id` varchar(32) NOT NULL DEFAULT '' COMMENT '项目唯一角色id',"
                + "`uid` varchar(64) NOT NULL DEFAULT '' COMMENT '用户id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`operation` int(11) NOT NULL DEFAULT '0' COMMENT '2登录,3登出',"
                + "`login_ip` varchar(50) NOT NULL DEFAULT '' COMMENT '登入时的ip',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '登入时的unix时间戳（10位）',"
                + "`adid` varchar(50) NOT NULL DEFAULT '' COMMENT '登录时的手机设备id',"
                + "`version` varchar(64) NOT NULL DEFAULT '' COMMENT '玩家登录时的客户端版本号',"
                + "`server_id` int(11) NOT NULL DEFAULT '0' COMMENT '玩家登录时的服务器id',"
                + "`platform` int(11) NOT NULL DEFAULT '0' COMMENT '账号归属的平台id',"
                + "`region` varchar(50) NOT NULL DEFAULT '' COMMENT '账号归属的区域id',"
                + "`ext` text NULL COMMENT '扩展字段：json格式',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`main_quest_progress` bigint(20) NOT NULL DEFAULT '0' COMMENT '主线任务进度',"
                + "`chapter_progress` bigint(20) NOT NULL DEFAULT '0' COMMENT '关卡进度：关卡id*1000+格子索引',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`crystal` bigint(20) NOT NULL DEFAULT '0' COMMENT '钻石',"
                + "`create_date` int(11) NOT NULL DEFAULT '0' COMMENT '玩家创角日期',"
                + "`total_power` bigint(20) NOT NULL DEFAULT '0' COMMENT '大臣总实力',"
                + "`earnings` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家总赚速',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='梦加日志-登录表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//operation
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(login_ip);//login_ip
        _size+=4;//timestamp
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(adid);//adid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(version);//version
        _size+=4;//server_id
        _size+=4;//platform
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(region);//region
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ext);//ext
        _size+=4;//date_time
        _size+=8;//main_quest_progress
        _size+=8;//chapter_progress
        _size+=4;//level
        _size+=8;//crystal
        _size+=4;//create_date
        _size+=8;//total_power
        _size+=8;//earnings
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
        buff.putInt(operation);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, login_ip);
        buff.putInt(timestamp);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, adid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, version);
        buff.putInt(server_id);
        buff.putInt(platform);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, region);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, ext);
        buff.putInt(date_time);
        buff.putLong(main_quest_progress);
        buff.putLong(chapter_progress);
        buff.putInt(level);
        buff.putLong(crystal);
        buff.putInt(create_date);
        buff.putLong(total_power);
        buff.putLong(earnings);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        sole_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        uid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        cid=buff.getLong();
        operation=buff.getInt();
        login_ip=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        timestamp=buff.getInt();
        adid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        version=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        server_id=buff.getInt();
        platform=buff.getInt();
        region=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        ext=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        date_time=buff.getInt();
        main_quest_progress=buff.getLong();
        chapter_progress=buff.getLong();
        level=buff.getInt();
        crystal=buff.getLong();
        create_date=buff.getInt();
        total_power=buff.getLong();
        earnings=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
