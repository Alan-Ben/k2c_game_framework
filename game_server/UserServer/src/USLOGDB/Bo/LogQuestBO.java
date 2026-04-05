package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogQuestBO extends BaseLogBo {

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

    public static final int FIELD_quest_id =6;
    @DataBaseField(type = "bigint(20)", fieldname = "quest_id", comment = "任务ID")
    private long quest_id;

    public static final int FIELD_quest_dbid =7;
    @DataBaseField(type = "bigint(20)", fieldname = "quest_dbid", comment = "任务数据ID")
    private long quest_dbid;

    public static final int FIELD_type =8;
    @DataBaseField(type = "int(11)", fieldname = "type", comment = "任务类型")
    private int type;

    public static final int FIELD_step =9;
    @DataBaseField(type = "bigint(20)", fieldname = "step", comment = "当前阶段")
    private long step;

    public static final int FIELD_expired_ts =10;
    @DataBaseField(type = "int(11)", fieldname = "expired_ts", comment = "超时时间")
    private int expired_ts;

    public static final int FIELD_status =11;
    @DataBaseField(type = "int(11)", fieldname = "status", comment = "任务状态")
    private int status;

    public LogQuestBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        cid = 0L;
        logType = 0;
        quest_id = 0L;
        quest_dbid = 0L;
        type = 0;
        step = 0L;
        expired_ts = 0;
        status = 0;
    }

    public LogQuestBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        cid = rs.getLong(6);
        logType = rs.getInt(7);
        quest_id = rs.getLong(8);
        quest_dbid = rs.getLong(9);
        type = rs.getInt(10);
        step = rs.getLong(11);
        expired_ts = rs.getInt(12);
        status = rs.getInt(13);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogQuestBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `cid`, `logType`, `quest_id`, `quest_dbid`, `type`, `step`, `expired_ts`, `status`";
    }

    @Override
    public String getTableName() {
        return "`log_quest`";
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
        strBuf.append("'").append(quest_id).append("', ");
        strBuf.append("'").append(quest_dbid).append("', ");
        strBuf.append("'").append(type).append("', ");
        strBuf.append("'").append(step).append("', ");
        strBuf.append("'").append(expired_ts).append("', ");
        strBuf.append("'").append(status).append("', ");
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

    // 任务ID
    public long getQuestId() { return this.quest_id; }
    public void setQuestId(BM _bm, long quest_id) {
        if(quest_id==this.quest_id) 
            return;
        this.quest_id = quest_id; 
        markField(_bm, FIELD_quest_id); 
    }
    public void saveQuestId(BM _bm, long quest_id) {
        if(quest_id==this.quest_id) 
            return;
        this.quest_id = quest_id;
        saveField(_bm, "quest_id", quest_id);
    }

    // 任务数据ID
    public long getQuestDbid() { return this.quest_dbid; }
    public void setQuestDbid(BM _bm, long quest_dbid) {
        if(quest_dbid==this.quest_dbid) 
            return;
        this.quest_dbid = quest_dbid; 
        markField(_bm, FIELD_quest_dbid); 
    }
    public void saveQuestDbid(BM _bm, long quest_dbid) {
        if(quest_dbid==this.quest_dbid) 
            return;
        this.quest_dbid = quest_dbid;
        saveField(_bm, "quest_dbid", quest_dbid);
    }

    // 任务类型
    public int getType() { return this.type; }
    public void setType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type; 
        markField(_bm, FIELD_type); 
    }
    public void saveType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type;
        saveField(_bm, "type", type);
    }

    // 当前阶段
    public long getStep() { return this.step; }
    public void setStep(BM _bm, long step) {
        if(step==this.step) 
            return;
        this.step = step; 
        markField(_bm, FIELD_step); 
    }
    public void saveStep(BM _bm, long step) {
        if(step==this.step) 
            return;
        this.step = step;
        saveField(_bm, "step", step);
    }

    // 超时时间
    public int getExpiredTs() { return this.expired_ts; }
    public void setExpiredTs(BM _bm, int expired_ts) {
        if(expired_ts==this.expired_ts) 
            return;
        this.expired_ts = expired_ts; 
        markField(_bm, FIELD_expired_ts); 
    }
    public void saveExpiredTs(BM _bm, int expired_ts) {
        if(expired_ts==this.expired_ts) 
            return;
        this.expired_ts = expired_ts;
        saveField(_bm, "expired_ts", expired_ts);
    }

    // 任务状态
    public int getStatus() { return this.status; }
    public void setStatus(BM _bm, int status) {
        if(status==this.status) 
            return;
        this.status = status; 
        markField(_bm, FIELD_status); 
    }
    public void saveStatus(BM _bm, int status) {
        if(status==this.status) 
            return;
        this.status = status;
        saveField(_bm, "status", status);
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
        sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        sBuilder.append(" `quest_dbid` = '").append(quest_dbid).append("',");
        sBuilder.append(" `type` = '").append(type).append("',");
        sBuilder.append(" `step` = '").append(step).append("',");
        sBuilder.append(" `expired_ts` = '").append(expired_ts).append("',");
        sBuilder.append(" `status` = '").append(status).append("',");
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
        if(isFieldMarked(FIELD_quest_id)) sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        if(isFieldMarked(FIELD_quest_dbid)) sBuilder.append(" `quest_dbid` = '").append(quest_dbid).append("',");
        if(isFieldMarked(FIELD_type)) sBuilder.append(" `type` = '").append(type).append("',");
        if(isFieldMarked(FIELD_step)) sBuilder.append(" `step` = '").append(step).append("',");
        if(isFieldMarked(FIELD_expired_ts)) sBuilder.append(" `expired_ts` = '").append(expired_ts).append("',");
        if(isFieldMarked(FIELD_status)) sBuilder.append(" `status` = '").append(status).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_quest` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`logType` int(11) NOT NULL DEFAULT '0' COMMENT '日志类型，增删改',"
                + "`quest_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务ID',"
                + "`quest_dbid` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务数据ID',"
                + "`type` int(11) NOT NULL DEFAULT '0' COMMENT '任务类型',"
                + "`step` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前阶段',"
                + "`expired_ts` int(11) NOT NULL DEFAULT '0' COMMENT '超时时间',"
                + "`status` int(11) NOT NULL DEFAULT '0' COMMENT '任务状态',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家任务明细日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//quest_id
        _size+=8;//quest_dbid
        _size+=4;//type
        _size+=8;//step
        _size+=4;//expired_ts
        _size+=4;//status
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
        buff.putLong(quest_id);
        buff.putLong(quest_dbid);
        buff.putInt(type);
        buff.putLong(step);
        buff.putInt(expired_ts);
        buff.putInt(status);        
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
        quest_id=buff.getLong();
        quest_dbid=buff.getLong();
        type=buff.getInt();
        step=buff.getLong();
        expired_ts=buff.getInt();
        status=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
