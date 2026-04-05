package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogMarketBO extends BaseLogBo {

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

    public static final int FIELD_cid =4;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_logType =5;
    @DataBaseField(type = "int(11)", fieldname = "logType", comment = "日志类型，增删改")
    private int logType;

    public static final int FIELD_market_id =6;
    @DataBaseField(type = "bigint(20)", fieldname = "market_id", comment = "市场id")
    private long market_id;

    public static final int FIELD_market_lvl =7;
    @DataBaseField(type = "bigint(20)", fieldname = "market_lvl", comment = "市场等级")
    private long market_lvl;

    public static final int FIELD_op_count =8;
    @DataBaseField(type = "bigint(20)", fieldname = "op_count", comment = "经营次数")
    private long op_count;

    public static final int FIELD_op_ms =9;
    @DataBaseField(type = "bigint(20)", fieldname = "op_ms", comment = "经营时间")
    private long op_ms;

    public LogMarketBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        cid = 0L;
        logType = 0;
        market_id = 0L;
        market_lvl = 0L;
        op_count = 0L;
        op_ms = 0L;
    }

    public LogMarketBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        cid = rs.getLong(6);
        logType = rs.getInt(7);
        market_id = rs.getLong(8);
        market_lvl = rs.getLong(9);
        op_count = rs.getLong(10);
        op_ms = rs.getLong(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogMarketBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `cid`, `logType`, `market_id`, `market_lvl`, `op_count`, `op_ms`";
    }

    @Override
    public String getTableName() {
        return "`log_market`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(logType).append("', ");
        strBuf.append("'").append(market_id).append("', ");
        strBuf.append("'").append(market_lvl).append("', ");
        strBuf.append("'").append(op_count).append("', ");
        strBuf.append("'").append(op_ms).append("', ");
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

    // 市场id
    public long getMarketId() { return this.market_id; }
    public void setMarketId(BM _bm, long market_id) {
        if(market_id==this.market_id) 
            return;
        this.market_id = market_id; 
        markField(_bm, FIELD_market_id); 
    }
    public void saveMarketId(BM _bm, long market_id) {
        if(market_id==this.market_id) 
            return;
        this.market_id = market_id;
        saveField(_bm, "market_id", market_id);
    }

    // 市场等级
    public long getMarketLvl() { return this.market_lvl; }
    public void setMarketLvl(BM _bm, long market_lvl) {
        if(market_lvl==this.market_lvl) 
            return;
        this.market_lvl = market_lvl; 
        markField(_bm, FIELD_market_lvl); 
    }
    public void saveMarketLvl(BM _bm, long market_lvl) {
        if(market_lvl==this.market_lvl) 
            return;
        this.market_lvl = market_lvl;
        saveField(_bm, "market_lvl", market_lvl);
    }

    // 经营次数
    public long getOpCount() { return this.op_count; }
    public void setOpCount(BM _bm, long op_count) {
        if(op_count==this.op_count) 
            return;
        this.op_count = op_count; 
        markField(_bm, FIELD_op_count); 
    }
    public void saveOpCount(BM _bm, long op_count) {
        if(op_count==this.op_count) 
            return;
        this.op_count = op_count;
        saveField(_bm, "op_count", op_count);
    }

    // 经营时间
    public long getOpMs() { return this.op_ms; }
    public void setOpMs(BM _bm, long op_ms) {
        if(op_ms==this.op_ms) 
            return;
        this.op_ms = op_ms; 
        markField(_bm, FIELD_op_ms); 
    }
    public void saveOpMs(BM _bm, long op_ms) {
        if(op_ms==this.op_ms) 
            return;
        this.op_ms = op_ms;
        saveField(_bm, "op_ms", op_ms);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `logType` = '").append(logType).append("',");
        sBuilder.append(" `market_id` = '").append(market_id).append("',");
        sBuilder.append(" `market_lvl` = '").append(market_lvl).append("',");
        sBuilder.append(" `op_count` = '").append(op_count).append("',");
        sBuilder.append(" `op_ms` = '").append(op_ms).append("',");
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
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_logType)) sBuilder.append(" `logType` = '").append(logType).append("',");
        if(isFieldMarked(FIELD_market_id)) sBuilder.append(" `market_id` = '").append(market_id).append("',");
        if(isFieldMarked(FIELD_market_lvl)) sBuilder.append(" `market_lvl` = '").append(market_lvl).append("',");
        if(isFieldMarked(FIELD_op_count)) sBuilder.append(" `op_count` = '").append(op_count).append("',");
        if(isFieldMarked(FIELD_op_ms)) sBuilder.append(" `op_ms` = '").append(op_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_market` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`logType` int(11) NOT NULL DEFAULT '0' COMMENT '日志类型，增删改',"
                + "`market_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '市场id',"
                + "`market_lvl` bigint(20) NOT NULL DEFAULT '0' COMMENT '市场等级',"
                + "`op_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '经营次数',"
                + "`op_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '经营时间',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='头像数据日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=4;//logType
        _size+=8;//market_id
        _size+=8;//market_lvl
        _size+=8;//op_count
        _size+=8;//op_ms
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
        buff.putLong(cid);
        buff.putInt(logType);
        buff.putLong(market_id);
        buff.putLong(market_lvl);
        buff.putLong(op_count);
        buff.putLong(op_ms);        
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
        cid=buff.getLong();
        logType=buff.getInt();
        market_id=buff.getLong();
        market_lvl=buff.getLong();
        op_count=buff.getLong();
        op_ms=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
