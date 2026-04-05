package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogActivityFundTaskCountBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_fund_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "fund_id", comment = "基金ID")
    private long fund_id;

    public static final int FIELD_activity_instance_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例ID")
    private long activity_instance_id;

    public static final int FIELD_task_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "task_id", comment = "任务ID")
    private long task_id;

    public static final int FIELD_count_before =4;
    @DataBaseField(type = "bigint(20)", fieldname = "count_before", comment = "变动前计数")
    private long count_before;

    public static final int FIELD_count_after =5;
    @DataBaseField(type = "bigint(20)", fieldname = "count_after", comment = "变动后计数")
    private long count_after;

    public static final int FIELD_change_value =6;
    @DataBaseField(type = "bigint(20)", fieldname = "change_value", comment = "变动值")
    private long change_value;

    public static final int FIELD_finished_times_before =7;
    @DataBaseField(type = "int(11)", fieldname = "finished_times_before", comment = "变动前完成次数")
    private int finished_times_before;

    public static final int FIELD_finished_times_after =8;
    @DataBaseField(type = "int(11)", fieldname = "finished_times_after", comment = "变动后完成次数")
    private int finished_times_after;

    public static final int FIELD_event_id =9;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =10;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =11;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =12;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public LogActivityFundTaskCountBO() {
        id = 0;
        cid = 0L;
        fund_id = 0L;
        activity_instance_id = 0L;
        task_id = 0L;
        count_before = 0L;
        count_after = 0L;
        change_value = 0L;
        finished_times_before = 0;
        finished_times_after = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
    }

    public LogActivityFundTaskCountBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        fund_id = rs.getLong(3);
        activity_instance_id = rs.getLong(4);
        task_id = rs.getLong(5);
        count_before = rs.getLong(6);
        count_after = rs.getLong(7);
        change_value = rs.getLong(8);
        finished_times_before = rs.getInt(9);
        finished_times_after = rs.getInt(10);
        event_id = rs.getInt(11);
        guid = rs.getLong(12);
        date_time = rs.getInt(13);
        timestamp = rs.getInt(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogActivityFundTaskCountBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `fund_id`, `activity_instance_id`, `task_id`, `count_before`, `count_after`, `change_value`, `finished_times_before`, `finished_times_after`, `event_id`, `guid`, `date_time`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`log_activity_fund_task_count`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(fund_id).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(task_id).append("', ");
        strBuf.append("'").append(count_before).append("', ");
        strBuf.append("'").append(count_after).append("', ");
        strBuf.append("'").append(change_value).append("', ");
        strBuf.append("'").append(finished_times_before).append("', ");
        strBuf.append("'").append(finished_times_after).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
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

    // 基金ID
    public long getFundId() { return this.fund_id; }
    public void setFundId(BM _bm, long fund_id) {
        if(fund_id==this.fund_id) 
            return;
        this.fund_id = fund_id; 
        markField(_bm, FIELD_fund_id); 
    }
    public void saveFundId(BM _bm, long fund_id) {
        if(fund_id==this.fund_id) 
            return;
        this.fund_id = fund_id;
        saveField(_bm, "fund_id", fund_id);
    }

    // 活动实例ID
    public long getActivityInstanceId() { return this.activity_instance_id; }
    public void setActivityInstanceId(BM _bm, long activity_instance_id) {
        if(activity_instance_id==this.activity_instance_id) 
            return;
        this.activity_instance_id = activity_instance_id; 
        markField(_bm, FIELD_activity_instance_id); 
    }
    public void saveActivityInstanceId(BM _bm, long activity_instance_id) {
        if(activity_instance_id==this.activity_instance_id) 
            return;
        this.activity_instance_id = activity_instance_id;
        saveField(_bm, "activity_instance_id", activity_instance_id);
    }

    // 任务ID
    public long getTaskId() { return this.task_id; }
    public void setTaskId(BM _bm, long task_id) {
        if(task_id==this.task_id) 
            return;
        this.task_id = task_id; 
        markField(_bm, FIELD_task_id); 
    }
    public void saveTaskId(BM _bm, long task_id) {
        if(task_id==this.task_id) 
            return;
        this.task_id = task_id;
        saveField(_bm, "task_id", task_id);
    }

    // 变动前计数
    public long getCountBefore() { return this.count_before; }
    public void setCountBefore(BM _bm, long count_before) {
        if(count_before==this.count_before) 
            return;
        this.count_before = count_before; 
        markField(_bm, FIELD_count_before); 
    }
    public void saveCountBefore(BM _bm, long count_before) {
        if(count_before==this.count_before) 
            return;
        this.count_before = count_before;
        saveField(_bm, "count_before", count_before);
    }

    // 变动后计数
    public long getCountAfter() { return this.count_after; }
    public void setCountAfter(BM _bm, long count_after) {
        if(count_after==this.count_after) 
            return;
        this.count_after = count_after; 
        markField(_bm, FIELD_count_after); 
    }
    public void saveCountAfter(BM _bm, long count_after) {
        if(count_after==this.count_after) 
            return;
        this.count_after = count_after;
        saveField(_bm, "count_after", count_after);
    }

    // 变动值
    public long getChangeValue() { return this.change_value; }
    public void setChangeValue(BM _bm, long change_value) {
        if(change_value==this.change_value) 
            return;
        this.change_value = change_value; 
        markField(_bm, FIELD_change_value); 
    }
    public void saveChangeValue(BM _bm, long change_value) {
        if(change_value==this.change_value) 
            return;
        this.change_value = change_value;
        saveField(_bm, "change_value", change_value);
    }

    // 变动前完成次数
    public int getFinishedTimesBefore() { return this.finished_times_before; }
    public void setFinishedTimesBefore(BM _bm, int finished_times_before) {
        if(finished_times_before==this.finished_times_before) 
            return;
        this.finished_times_before = finished_times_before; 
        markField(_bm, FIELD_finished_times_before); 
    }
    public void saveFinishedTimesBefore(BM _bm, int finished_times_before) {
        if(finished_times_before==this.finished_times_before) 
            return;
        this.finished_times_before = finished_times_before;
        saveField(_bm, "finished_times_before", finished_times_before);
    }

    // 变动后完成次数
    public int getFinishedTimesAfter() { return this.finished_times_after; }
    public void setFinishedTimesAfter(BM _bm, int finished_times_after) {
        if(finished_times_after==this.finished_times_after) 
            return;
        this.finished_times_after = finished_times_after; 
        markField(_bm, FIELD_finished_times_after); 
    }
    public void saveFinishedTimesAfter(BM _bm, int finished_times_after) {
        if(finished_times_after==this.finished_times_after) 
            return;
        this.finished_times_after = finished_times_after;
        saveField(_bm, "finished_times_after", finished_times_after);
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



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `fund_id` = '").append(fund_id).append("',");
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `task_id` = '").append(task_id).append("',");
        sBuilder.append(" `count_before` = '").append(count_before).append("',");
        sBuilder.append(" `count_after` = '").append(count_after).append("',");
        sBuilder.append(" `change_value` = '").append(change_value).append("',");
        sBuilder.append(" `finished_times_before` = '").append(finished_times_before).append("',");
        sBuilder.append(" `finished_times_after` = '").append(finished_times_after).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_fund_id)) sBuilder.append(" `fund_id` = '").append(fund_id).append("',");
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_task_id)) sBuilder.append(" `task_id` = '").append(task_id).append("',");
        if(isFieldMarked(FIELD_count_before)) sBuilder.append(" `count_before` = '").append(count_before).append("',");
        if(isFieldMarked(FIELD_count_after)) sBuilder.append(" `count_after` = '").append(count_after).append("',");
        if(isFieldMarked(FIELD_change_value)) sBuilder.append(" `change_value` = '").append(change_value).append("',");
        if(isFieldMarked(FIELD_finished_times_before)) sBuilder.append(" `finished_times_before` = '").append(finished_times_before).append("',");
        if(isFieldMarked(FIELD_finished_times_after)) sBuilder.append(" `finished_times_after` = '").append(finished_times_after).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_activity_fund_task_count` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`fund_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '基金ID',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`task_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务ID',"
                + "`count_before` bigint(20) NOT NULL DEFAULT '0' COMMENT '变动前计数',"
                + "`count_after` bigint(20) NOT NULL DEFAULT '0' COMMENT '变动后计数',"
                + "`change_value` bigint(20) NOT NULL DEFAULT '0' COMMENT '变动值',"
                + "`finished_times_before` int(11) NOT NULL DEFAULT '0' COMMENT '变动前完成次数',"
                + "`finished_times_after` int(11) NOT NULL DEFAULT '0' COMMENT '变动后完成次数',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动基金任务计数变更日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//fund_id
        _size+=8;//activity_instance_id
        _size+=8;//task_id
        _size+=8;//count_before
        _size+=8;//count_after
        _size+=8;//change_value
        _size+=4;//finished_times_before
        _size+=4;//finished_times_after
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(fund_id);
        buff.putLong(activity_instance_id);
        buff.putLong(task_id);
        buff.putLong(count_before);
        buff.putLong(count_after);
        buff.putLong(change_value);
        buff.putInt(finished_times_before);
        buff.putInt(finished_times_after);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        fund_id=buff.getLong();
        activity_instance_id=buff.getLong();
        task_id=buff.getLong();
        count_before=buff.getLong();
        count_after=buff.getLong();
        change_value=buff.getLong();
        finished_times_before=buff.getInt();
        finished_times_after=buff.getInt();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
