package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogPlayerLevelUpBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_event_id =1;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =3;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =4;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_ori_level =5;
    @DataBaseField(type = "bigint(20)", fieldname = "ori_level", comment = "原等级")
    private long ori_level;

    public static final int FIELD_exp =6;
    @DataBaseField(type = "bigint(20)", fieldname = "exp", comment = "经验")
    private long exp;

    public static final int FIELD_earnings =7;
    @DataBaseField(type = "bigint(20)", fieldname = "earnings", comment = "赚速")
    private long earnings;

    public static final int FIELD_new_level =8;
    @DataBaseField(type = "bigint(20)", fieldname = "new_level", comment = "目标等级")
    private long new_level;

    public static final int FIELD_nation =9;
    @DataBaseField(type = "varchar(50)", fieldname = "nation", comment = "国家")
    private String nation;

    public static final int FIELD_device_os =10;
    @DataBaseField(type = "varchar(50)", fieldname = "device_os", comment = "客户端操作系统")
    private String device_os;

    public static final int FIELD_create_role_time =11;
    @DataBaseField(type = "int(11)", fieldname = "create_role_time", comment = "创角时间戳(10位)")
    private int create_role_time;

    public static final int FIELD_create_role_date =12;
    @DataBaseField(type = "int(11)", fieldname = "create_role_date", comment = "创角日期")
    private int create_role_date;

    public LogPlayerLevelUpBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        ori_level = 0L;
        exp = 0L;
        earnings = 0L;
        new_level = 0L;
        nation = "";
        device_os = "";
        create_role_time = 0;
        create_role_date = 0;
    }

    public LogPlayerLevelUpBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        ori_level = rs.getLong(7);
        exp = rs.getLong(8);
        earnings = rs.getLong(9);
        new_level = rs.getLong(10);
        nation = rs.getString(11);
        device_os = rs.getString(12);
        create_role_time = rs.getInt(13);
        create_role_date = rs.getInt(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogPlayerLevelUpBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `ori_level`, `exp`, `earnings`, `new_level`, `nation`, `device_os`, `create_role_time`, `create_role_date`";
    }

    @Override
    public String getTableName() {
        return "`log_player_level_up`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(ori_level).append("', ");
        strBuf.append("'").append(exp).append("', ");
        strBuf.append("'").append(earnings).append("', ");
        strBuf.append("'").append(new_level).append("', ");
        strBuf.append("'").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(device_os == null ? null : device_os.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(create_role_time).append("', ");
        strBuf.append("'").append(create_role_date).append("', ");
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

    // 玩家CID
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

    // 事件类型
    public int getEventId() { return this.event_id; }
    public void setEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id; 
        markField(_bm, FIELD_event_id); 
    }
    public void saveEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id;
        saveField(_bm, "event_id", event_id);
    }

    // 事件唯一id
    public long getGuid() { return this.guid; }
    public void setGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid; 
        markField(_bm, FIELD_guid); 
    }
    public void saveGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid;
        saveField(_bm, "guid", guid);
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

    // 原等级
    public long getOriLevel() { return this.ori_level; }
    public void setOriLevel(BM _bm, long ori_level) {
        if(ori_level==this.ori_level) 
            return;
        this.ori_level = ori_level; 
        markField(_bm, FIELD_ori_level); 
    }
    public void saveOriLevel(BM _bm, long ori_level) {
        if(ori_level==this.ori_level) 
            return;
        this.ori_level = ori_level;
        saveField(_bm, "ori_level", ori_level);
    }

    // 经验
    public long getExp() { return this.exp; }
    public void setExp(BM _bm, long exp) {
        if(exp==this.exp) 
            return;
        this.exp = exp; 
        markField(_bm, FIELD_exp); 
    }
    public void saveExp(BM _bm, long exp) {
        if(exp==this.exp) 
            return;
        this.exp = exp;
        saveField(_bm, "exp", exp);
    }

    // 赚速
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

    // 目标等级
    public long getNewLevel() { return this.new_level; }
    public void setNewLevel(BM _bm, long new_level) {
        if(new_level==this.new_level) 
            return;
        this.new_level = new_level; 
        markField(_bm, FIELD_new_level); 
    }
    public void saveNewLevel(BM _bm, long new_level) {
        if(new_level==this.new_level) 
            return;
        this.new_level = new_level;
        saveField(_bm, "new_level", new_level);
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

    // 客户端操作系统
    public String getDeviceOs() { return this.device_os; }
    public void setDeviceOs(BM _bm, String device_os) {
        if(device_os.equals(this.device_os)) 
            return;
        this.device_os = device_os; 
        markField(_bm, FIELD_device_os); 
    }
    public void saveDeviceOs(BM _bm, String device_os) {
        if(device_os.equals(this.device_os)) 
            return;
        this.device_os = device_os;
        saveField(_bm, "device_os", device_os);
    }

    // 创角时间戳(10位)
    public int getCreateRoleTime() { return this.create_role_time; }
    public void setCreateRoleTime(BM _bm, int create_role_time) {
        if(create_role_time==this.create_role_time) 
            return;
        this.create_role_time = create_role_time; 
        markField(_bm, FIELD_create_role_time); 
    }
    public void saveCreateRoleTime(BM _bm, int create_role_time) {
        if(create_role_time==this.create_role_time) 
            return;
        this.create_role_time = create_role_time;
        saveField(_bm, "create_role_time", create_role_time);
    }

    // 创角日期
    public int getCreateRoleDate() { return this.create_role_date; }
    public void setCreateRoleDate(BM _bm, int create_role_date) {
        if(create_role_date==this.create_role_date) 
            return;
        this.create_role_date = create_role_date; 
        markField(_bm, FIELD_create_role_date); 
    }
    public void saveCreateRoleDate(BM _bm, int create_role_date) {
        if(create_role_date==this.create_role_date) 
            return;
        this.create_role_date = create_role_date;
        saveField(_bm, "create_role_date", create_role_date);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `ori_level` = '").append(ori_level).append("',");
        sBuilder.append(" `exp` = '").append(exp).append("',");
        sBuilder.append(" `earnings` = '").append(earnings).append("',");
        sBuilder.append(" `new_level` = '").append(new_level).append("',");
        sBuilder.append(" `nation` = '").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `device_os` = '").append(device_os == null ? null : device_os.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `create_role_time` = '").append(create_role_time).append("',");
        sBuilder.append(" `create_role_date` = '").append(create_role_date).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_ori_level)) sBuilder.append(" `ori_level` = '").append(ori_level).append("',");
        if(isFieldMarked(FIELD_exp)) sBuilder.append(" `exp` = '").append(exp).append("',");
        if(isFieldMarked(FIELD_earnings)) sBuilder.append(" `earnings` = '").append(earnings).append("',");
        if(isFieldMarked(FIELD_new_level)) sBuilder.append(" `new_level` = '").append(new_level).append("',");
        if(isFieldMarked(FIELD_nation)) sBuilder.append(" `nation` = '").append(nation == null ? null : nation.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_device_os)) sBuilder.append(" `device_os` = '").append(device_os == null ? null : device_os.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_create_role_time)) sBuilder.append(" `create_role_time` = '").append(create_role_time).append("',");
        if(isFieldMarked(FIELD_create_role_date)) sBuilder.append(" `create_role_date` = '").append(create_role_date).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_player_level_up` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`ori_level` bigint(20) NOT NULL DEFAULT '0' COMMENT '原等级',"
                + "`exp` bigint(20) NOT NULL DEFAULT '0' COMMENT '经验',"
                + "`earnings` bigint(20) NOT NULL DEFAULT '0' COMMENT '赚速',"
                + "`new_level` bigint(20) NOT NULL DEFAULT '0' COMMENT '目标等级',"
                + "`nation` varchar(50) NOT NULL DEFAULT '' COMMENT '国家',"
                + "`device_os` varchar(50) NOT NULL DEFAULT '' COMMENT '客户端操作系统',"
                + "`create_role_time` int(11) NOT NULL DEFAULT '0' COMMENT '创角时间戳(10位)',"
                + "`create_role_date` int(11) NOT NULL DEFAULT '0' COMMENT '创角日期',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家升级日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=8;//ori_level
        _size+=8;//exp
        _size+=8;//earnings
        _size+=8;//new_level
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(nation);//nation
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(device_os);//device_os
        _size+=4;//create_role_time
        _size+=4;//create_role_date
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putLong(ori_level);
        buff.putLong(exp);
        buff.putLong(earnings);
        buff.putLong(new_level);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, nation);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, device_os);
        buff.putInt(create_role_time);
        buff.putInt(create_role_date);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        ori_level=buff.getLong();
        exp=buff.getLong();
        earnings=buff.getLong();
        new_level=buff.getLong();
        nation=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        device_os=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        create_role_time=buff.getInt();
        create_role_date=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
