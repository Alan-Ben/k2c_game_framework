package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogGuildWealthChgBO extends BaseLogBo {

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

    public static final int FIELD_event =5;
    @DataBaseField(type = "int(11)", fieldname = "event", comment = "事件id")
    private int event;

    public static final int FIELD_old_number =6;
    @DataBaseField(type = "bigint(20)", fieldname = "old_number", comment = "变更前的值")
    private long old_number;

    public static final int FIELD_chg_value =7;
    @DataBaseField(type = "bigint(20)", fieldname = "chg_value", comment = "变更值")
    private long chg_value;

    public static final int FIELD_final_number =8;
    @DataBaseField(type = "bigint(20)", fieldname = "final_number", comment = "变更前的值")
    private long final_number;

    public LogGuildWealthChgBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        guild_id = 0L;
        event = 0;
        old_number = 0L;
        chg_value = 0L;
        final_number = 0L;
    }

    public LogGuildWealthChgBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        guild_id = rs.getLong(6);
        event = rs.getInt(7);
        old_number = rs.getLong(8);
        chg_value = rs.getLong(9);
        final_number = rs.getLong(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogGuildWealthChgBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `guild_id`, `event`, `old_number`, `chg_value`, `final_number`";
    }

    @Override
    public String getTableName() {
        return "`log_guild_wealth_chg`";
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
        strBuf.append("'").append(event).append("', ");
        strBuf.append("'").append(old_number).append("', ");
        strBuf.append("'").append(chg_value).append("', ");
        strBuf.append("'").append(final_number).append("', ");
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

    // 事件id
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

    // 变更前的值
    public long getOldNumber() { return this.old_number; }
    public void setOldNumber(BM _bm, long old_number) {
        if(old_number==this.old_number) 
            return;
        this.old_number = old_number; 
        markField(_bm, FIELD_old_number); 
    }
    public void saveOldNumber(BM _bm, long old_number) {
        if(old_number==this.old_number) 
            return;
        this.old_number = old_number;
        saveField(_bm, "old_number", old_number);
    }

    // 变更值
    public long getChgValue() { return this.chg_value; }
    public void setChgValue(BM _bm, long chg_value) {
        if(chg_value==this.chg_value) 
            return;
        this.chg_value = chg_value; 
        markField(_bm, FIELD_chg_value); 
    }
    public void saveChgValue(BM _bm, long chg_value) {
        if(chg_value==this.chg_value) 
            return;
        this.chg_value = chg_value;
        saveField(_bm, "chg_value", chg_value);
    }

    // 变更前的值
    public long getFinalNumber() { return this.final_number; }
    public void setFinalNumber(BM _bm, long final_number) {
        if(final_number==this.final_number) 
            return;
        this.final_number = final_number; 
        markField(_bm, FIELD_final_number); 
    }
    public void saveFinalNumber(BM _bm, long final_number) {
        if(final_number==this.final_number) 
            return;
        this.final_number = final_number;
        saveField(_bm, "final_number", final_number);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `event` = '").append(event).append("',");
        sBuilder.append(" `old_number` = '").append(old_number).append("',");
        sBuilder.append(" `chg_value` = '").append(chg_value).append("',");
        sBuilder.append(" `final_number` = '").append(final_number).append("',");
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
        if(isFieldMarked(FIELD_event)) sBuilder.append(" `event` = '").append(event).append("',");
        if(isFieldMarked(FIELD_old_number)) sBuilder.append(" `old_number` = '").append(old_number).append("',");
        if(isFieldMarked(FIELD_chg_value)) sBuilder.append(" `chg_value` = '").append(chg_value).append("',");
        if(isFieldMarked(FIELD_final_number)) sBuilder.append(" `final_number` = '").append(final_number).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_guild_wealth_chg` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟id',"
                + "`event` int(11) NOT NULL DEFAULT '0' COMMENT '事件id',"
                + "`old_number` bigint(20) NOT NULL DEFAULT '0' COMMENT '变更前的值',"
                + "`chg_value` bigint(20) NOT NULL DEFAULT '0' COMMENT '变更值',"
                + "`final_number` bigint(20) NOT NULL DEFAULT '0' COMMENT '变更前的值',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟财富变更日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//event
        _size+=8;//old_number
        _size+=8;//chg_value
        _size+=8;//final_number
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
        buff.putInt(event);
        buff.putLong(old_number);
        buff.putLong(chg_value);
        buff.putLong(final_number);        
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
        event=buff.getInt();
        old_number=buff.getLong();
        chg_value=buff.getLong();
        final_number=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
