package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogGuildPositionChgBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_event_id =0;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =2;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =3;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_guild_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟id")
    private long guild_id;

    public static final int FIELD_cid =5;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "被变更职位的玩家cid")
    private long cid;

    public static final int FIELD_old_position =6;
    @DataBaseField(type = "int(11)", fieldname = "old_position", comment = "原职位")
    private int old_position;

    public static final int FIELD_new_position =7;
    @DataBaseField(type = "int(11)", fieldname = "new_position", comment = "新职位")
    private int new_position;

    public static final int FIELD_operator_cid =8;
    @DataBaseField(type = "bigint(20)", fieldname = "operator_cid", comment = "操作者cid，系统操作为0")
    private long operator_cid;

    public LogGuildPositionChgBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        guild_id = 0L;
        cid = 0L;
        old_position = 0;
        new_position = 0;
        operator_cid = 0L;
    }

    public LogGuildPositionChgBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        guild_id = rs.getLong(6);
        cid = rs.getLong(7);
        old_position = rs.getInt(8);
        new_position = rs.getInt(9);
        operator_cid = rs.getLong(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogGuildPositionChgBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `guild_id`, `cid`, `old_position`, `new_position`, `operator_cid`";
    }

    @Override
    public String getTableName() {
        return "`log_guild_position_chg`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(old_position).append("', ");
        strBuf.append("'").append(new_position).append("', ");
        strBuf.append("'").append(operator_cid).append("', ");
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

    // 联盟id
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
    }

    // 被变更职位的玩家cid
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

    // 原职位
    public int getOldPosition() { return this.old_position; }
    public void setOldPosition(BM _bm, int old_position) {
        if(old_position==this.old_position) 
            return;
        this.old_position = old_position; 
        markField(_bm, FIELD_old_position); 
    }
    public void saveOldPosition(BM _bm, int old_position) {
        if(old_position==this.old_position) 
            return;
        this.old_position = old_position;
        saveField(_bm, "old_position", old_position);
    }

    // 新职位
    public int getNewPosition() { return this.new_position; }
    public void setNewPosition(BM _bm, int new_position) {
        if(new_position==this.new_position) 
            return;
        this.new_position = new_position; 
        markField(_bm, FIELD_new_position); 
    }
    public void saveNewPosition(BM _bm, int new_position) {
        if(new_position==this.new_position) 
            return;
        this.new_position = new_position;
        saveField(_bm, "new_position", new_position);
    }

    // 操作者cid，系统操作为0
    public long getOperatorCid() { return this.operator_cid; }
    public void setOperatorCid(BM _bm, long operator_cid) {
        if(operator_cid==this.operator_cid) 
            return;
        this.operator_cid = operator_cid; 
        markField(_bm, FIELD_operator_cid); 
    }
    public void saveOperatorCid(BM _bm, long operator_cid) {
        if(operator_cid==this.operator_cid) 
            return;
        this.operator_cid = operator_cid;
        saveField(_bm, "operator_cid", operator_cid);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `old_position` = '").append(old_position).append("',");
        sBuilder.append(" `new_position` = '").append(new_position).append("',");
        sBuilder.append(" `operator_cid` = '").append(operator_cid).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_old_position)) sBuilder.append(" `old_position` = '").append(old_position).append("',");
        if(isFieldMarked(FIELD_new_position)) sBuilder.append(" `new_position` = '").append(new_position).append("',");
        if(isFieldMarked(FIELD_operator_cid)) sBuilder.append(" `operator_cid` = '").append(operator_cid).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_guild_position_chg` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '被变更职位的玩家cid',"
                + "`old_position` int(11) NOT NULL DEFAULT '0' COMMENT '原职位',"
                + "`new_position` int(11) NOT NULL DEFAULT '0' COMMENT '新职位',"
                + "`operator_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '操作者cid，系统操作为0',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟职位变更日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=8;//guild_id
        _size+=8;//cid
        _size+=4;//old_position
        _size+=4;//new_position
        _size+=8;//operator_cid
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putLong(guild_id);
        buff.putLong(cid);
        buff.putInt(old_position);
        buff.putInt(new_position);
        buff.putLong(operator_cid);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        guild_id=buff.getLong();
        cid=buff.getLong();
        old_position=buff.getInt();
        new_position=buff.getInt();
        operator_cid=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
