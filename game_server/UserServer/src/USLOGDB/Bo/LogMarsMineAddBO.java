package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogMarsMineAddBO extends BaseLogBo {

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

    public static final int FIELD_mineInstanceId =6;
    @DataBaseField(type = "bigint(20)", fieldname = "mineInstanceId", comment = "火星矿实例ID")
    private long mineInstanceId;

    public static final int FIELD_mineRefId =7;
    @DataBaseField(type = "bigint(20)", fieldname = "mineRefId", comment = "火星矿配置ID")
    private long mineRefId;

    public static final int FIELD_startShowMs =8;
    @DataBaseField(type = "bigint(20)", fieldname = "startShowMs", comment = "火星矿开始展示时间")
    private long startShowMs;

    public static final int FIELD_endShowMs =9;
    @DataBaseField(type = "bigint(20)", fieldname = "endShowMs", comment = "火星矿结束展示时间")
    private long endShowMs;

    public static final int FIELD_pos =10;
    @DataBaseField(type = "bigint(20)", fieldname = "pos", comment = "火星矿位置ID")
    private long pos;

    public LogMarsMineAddBO() {
        id = 0;
        cid = 0L;
        logType = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        mineInstanceId = 0L;
        mineRefId = 0L;
        startShowMs = 0L;
        endShowMs = 0L;
        pos = 0L;
    }

    public LogMarsMineAddBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        logType = rs.getInt(3);
        event_id = rs.getInt(4);
        guid = rs.getLong(5);
        date_time = rs.getInt(6);
        timestamp = rs.getInt(7);
        mineInstanceId = rs.getLong(8);
        mineRefId = rs.getLong(9);
        startShowMs = rs.getLong(10);
        endShowMs = rs.getLong(11);
        pos = rs.getLong(12);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogMarsMineAddBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `logType`, `event_id`, `guid`, `date_time`, `timestamp`, `mineInstanceId`, `mineRefId`, `startShowMs`, `endShowMs`, `pos`";
    }

    @Override
    public String getTableName() {
        return "`log_mars_mine_add`";
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
        strBuf.append("'").append(mineInstanceId).append("', ");
        strBuf.append("'").append(mineRefId).append("', ");
        strBuf.append("'").append(startShowMs).append("', ");
        strBuf.append("'").append(endShowMs).append("', ");
        strBuf.append("'").append(pos).append("', ");
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

    // 火星矿实例ID
    public long getMineInstanceId() { return this.mineInstanceId; }
    public void setMineInstanceId(BM _bm, long mineInstanceId) {
        if(mineInstanceId==this.mineInstanceId) 
            return;
        this.mineInstanceId = mineInstanceId; 
        markField(_bm, FIELD_mineInstanceId); 
    }
    public void saveMineInstanceId(BM _bm, long mineInstanceId) {
        if(mineInstanceId==this.mineInstanceId) 
            return;
        this.mineInstanceId = mineInstanceId;
        saveField(_bm, "mineInstanceId", mineInstanceId);
    }

    // 火星矿配置ID
    public long getMineRefId() { return this.mineRefId; }
    public void setMineRefId(BM _bm, long mineRefId) {
        if(mineRefId==this.mineRefId) 
            return;
        this.mineRefId = mineRefId; 
        markField(_bm, FIELD_mineRefId); 
    }
    public void saveMineRefId(BM _bm, long mineRefId) {
        if(mineRefId==this.mineRefId) 
            return;
        this.mineRefId = mineRefId;
        saveField(_bm, "mineRefId", mineRefId);
    }

    // 火星矿开始展示时间
    public long getStartShowMs() { return this.startShowMs; }
    public void setStartShowMs(BM _bm, long startShowMs) {
        if(startShowMs==this.startShowMs) 
            return;
        this.startShowMs = startShowMs; 
        markField(_bm, FIELD_startShowMs); 
    }
    public void saveStartShowMs(BM _bm, long startShowMs) {
        if(startShowMs==this.startShowMs) 
            return;
        this.startShowMs = startShowMs;
        saveField(_bm, "startShowMs", startShowMs);
    }

    // 火星矿结束展示时间
    public long getEndShowMs() { return this.endShowMs; }
    public void setEndShowMs(BM _bm, long endShowMs) {
        if(endShowMs==this.endShowMs) 
            return;
        this.endShowMs = endShowMs; 
        markField(_bm, FIELD_endShowMs); 
    }
    public void saveEndShowMs(BM _bm, long endShowMs) {
        if(endShowMs==this.endShowMs) 
            return;
        this.endShowMs = endShowMs;
        saveField(_bm, "endShowMs", endShowMs);
    }

    // 火星矿位置ID
    public long getPos() { return this.pos; }
    public void setPos(BM _bm, long pos) {
        if(pos==this.pos) 
            return;
        this.pos = pos; 
        markField(_bm, FIELD_pos); 
    }
    public void savePos(BM _bm, long pos) {
        if(pos==this.pos) 
            return;
        this.pos = pos;
        saveField(_bm, "pos", pos);
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
        sBuilder.append(" `mineInstanceId` = '").append(mineInstanceId).append("',");
        sBuilder.append(" `mineRefId` = '").append(mineRefId).append("',");
        sBuilder.append(" `startShowMs` = '").append(startShowMs).append("',");
        sBuilder.append(" `endShowMs` = '").append(endShowMs).append("',");
        sBuilder.append(" `pos` = '").append(pos).append("',");
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
        if(isFieldMarked(FIELD_mineInstanceId)) sBuilder.append(" `mineInstanceId` = '").append(mineInstanceId).append("',");
        if(isFieldMarked(FIELD_mineRefId)) sBuilder.append(" `mineRefId` = '").append(mineRefId).append("',");
        if(isFieldMarked(FIELD_startShowMs)) sBuilder.append(" `startShowMs` = '").append(startShowMs).append("',");
        if(isFieldMarked(FIELD_endShowMs)) sBuilder.append(" `endShowMs` = '").append(endShowMs).append("',");
        if(isFieldMarked(FIELD_pos)) sBuilder.append(" `pos` = '").append(pos).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_mars_mine_add` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`logType` int(11) NOT NULL DEFAULT '0' COMMENT '日志类型，增删改',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`mineInstanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '火星矿实例ID',"
                + "`mineRefId` bigint(20) NOT NULL DEFAULT '0' COMMENT '火星矿配置ID',"
                + "`startShowMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '火星矿开始展示时间',"
                + "`endShowMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '火星矿结束展示时间',"
                + "`pos` bigint(20) NOT NULL DEFAULT '0' COMMENT '火星矿位置ID',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家火星矿增加日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//mineInstanceId
        _size+=8;//mineRefId
        _size+=8;//startShowMs
        _size+=8;//endShowMs
        _size+=8;//pos
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
        buff.putLong(mineInstanceId);
        buff.putLong(mineRefId);
        buff.putLong(startShowMs);
        buff.putLong(endShowMs);
        buff.putLong(pos);        
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
        mineInstanceId=buff.getLong();
        mineRefId=buff.getLong();
        startShowMs=buff.getLong();
        endShowMs=buff.getLong();
        pos=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
