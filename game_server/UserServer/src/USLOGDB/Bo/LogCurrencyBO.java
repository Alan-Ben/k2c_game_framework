package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogCurrencyBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_logType =1;
    @DataBaseField(type = "int(11)", fieldname = "logType", comment = "日志类型，增删改")
    private int logType;

    public static final int FIELD_event_id =2;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =3;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =4;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =5;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_sub_id =6;
    @DataBaseField(type = "bigint(20)", fieldname = "sub_id", comment = "细分id")
    private long sub_id;

    public static final int FIELD_origin_value =7;
    @DataBaseField(type = "bigint(20)", fieldname = "origin_value", comment = "初始值")
    private long origin_value;

    public static final int FIELD_chg_value =8;
    @DataBaseField(type = "bigint(20)", fieldname = "chg_value", comment = "变化值")
    private long chg_value;

    public static final int FIELD_final_value =9;
    @DataBaseField(type = "bigint(20)", fieldname = "final_value", comment = "最终值")
    private long final_value;

    public static final int FIELD_player_level =10;
    @DataBaseField(type = "int(11)", fieldname = "player_level", comment = "玩家等级")
    private int player_level;

    public LogCurrencyBO() {
        id = 0;
        cid = 0L;
        logType = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        sub_id = 0L;
        origin_value = 0L;
        chg_value = 0L;
        final_value = 0L;
        player_level = 0;
    }

    public LogCurrencyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        logType = rs.getInt(3);
        event_id = rs.getInt(4);
        guid = rs.getLong(5);
        date_time = rs.getInt(6);
        timestamp = rs.getInt(7);
        sub_id = rs.getLong(8);
        origin_value = rs.getLong(9);
        chg_value = rs.getLong(10);
        final_value = rs.getLong(11);
        player_level = rs.getInt(12);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogCurrencyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `logType`, `event_id`, `guid`, `date_time`, `timestamp`, `sub_id`, `origin_value`, `chg_value`, `final_value`, `player_level`";
    }

    @Override
    public String getTableName() {
        return "`log_currency`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(logType).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(sub_id).append("', ");
        strBuf.append("'").append(origin_value).append("', ");
        strBuf.append("'").append(chg_value).append("', ");
        strBuf.append("'").append(final_value).append("', ");
        strBuf.append("'").append(player_level).append("', ");
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

    // 日志类型，增删改
    public int getLogType() { return this.logType; }
    public void setLogType(BM _bm, int logType) {
        if(logType==this.logType) 
            return;
        this.logType = logType; 
        markField(_bm, FIELD_logType); 
    }
    public void saveLogType(BM _bm, int logType) {
        if(logType==this.logType) 
            return;
        this.logType = logType;
        saveField(_bm, "logType", logType);
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

    // 细分id
    public long getSubId() { return this.sub_id; }
    public void setSubId(BM _bm, long sub_id) {
        if(sub_id==this.sub_id) 
            return;
        this.sub_id = sub_id; 
        markField(_bm, FIELD_sub_id); 
    }
    public void saveSubId(BM _bm, long sub_id) {
        if(sub_id==this.sub_id) 
            return;
        this.sub_id = sub_id;
        saveField(_bm, "sub_id", sub_id);
    }

    // 初始值
    public long getOriginValue() { return this.origin_value; }
    public void setOriginValue(BM _bm, long origin_value) {
        if(origin_value==this.origin_value) 
            return;
        this.origin_value = origin_value; 
        markField(_bm, FIELD_origin_value); 
    }
    public void saveOriginValue(BM _bm, long origin_value) {
        if(origin_value==this.origin_value) 
            return;
        this.origin_value = origin_value;
        saveField(_bm, "origin_value", origin_value);
    }

    // 变化值
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

    // 最终值
    public long getFinalValue() { return this.final_value; }
    public void setFinalValue(BM _bm, long final_value) {
        if(final_value==this.final_value) 
            return;
        this.final_value = final_value; 
        markField(_bm, FIELD_final_value); 
    }
    public void saveFinalValue(BM _bm, long final_value) {
        if(final_value==this.final_value) 
            return;
        this.final_value = final_value;
        saveField(_bm, "final_value", final_value);
    }

    // 玩家等级
    public int getPlayerLevel() { return this.player_level; }
    public void setPlayerLevel(BM _bm, int player_level) {
        if(player_level==this.player_level) 
            return;
        this.player_level = player_level; 
        markField(_bm, FIELD_player_level); 
    }
    public void savePlayerLevel(BM _bm, int player_level) {
        if(player_level==this.player_level) 
            return;
        this.player_level = player_level;
        saveField(_bm, "player_level", player_level);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `logType` = '").append(logType).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `sub_id` = '").append(sub_id).append("',");
        sBuilder.append(" `origin_value` = '").append(origin_value).append("',");
        sBuilder.append(" `chg_value` = '").append(chg_value).append("',");
        sBuilder.append(" `final_value` = '").append(final_value).append("',");
        sBuilder.append(" `player_level` = '").append(player_level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_logType)) sBuilder.append(" `logType` = '").append(logType).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_sub_id)) sBuilder.append(" `sub_id` = '").append(sub_id).append("',");
        if(isFieldMarked(FIELD_origin_value)) sBuilder.append(" `origin_value` = '").append(origin_value).append("',");
        if(isFieldMarked(FIELD_chg_value)) sBuilder.append(" `chg_value` = '").append(chg_value).append("',");
        if(isFieldMarked(FIELD_final_value)) sBuilder.append(" `final_value` = '").append(final_value).append("',");
        if(isFieldMarked(FIELD_player_level)) sBuilder.append(" `player_level` = '").append(player_level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_currency` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`logType` int(11) NOT NULL DEFAULT '0' COMMENT '日志类型，增删改',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`sub_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '细分id',"
                + "`origin_value` bigint(20) NOT NULL DEFAULT '0' COMMENT '初始值',"
                + "`chg_value` bigint(20) NOT NULL DEFAULT '0' COMMENT '变化值',"
                + "`final_value` bigint(20) NOT NULL DEFAULT '0' COMMENT '最终值',"
                + "`player_level` int(11) NOT NULL DEFAULT '0' COMMENT '玩家等级',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='货币数据日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//logType
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=8;//sub_id
        _size+=8;//origin_value
        _size+=8;//chg_value
        _size+=8;//final_value
        _size+=4;//player_level
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(logType);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putLong(sub_id);
        buff.putLong(origin_value);
        buff.putLong(chg_value);
        buff.putLong(final_value);
        buff.putInt(player_level);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        logType=buff.getInt();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        sub_id=buff.getLong();
        origin_value=buff.getLong();
        chg_value=buff.getLong();
        final_value=buff.getLong();
        player_level=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
