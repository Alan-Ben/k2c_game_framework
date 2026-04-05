package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogActivityFundDrawBO extends BaseLogBo {

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

    public static final int FIELD_had_draw_free_step_before =3;
    @DataBaseField(type = "int(11)", fieldname = "had_draw_free_step_before", comment = "领取前免费档已领取阶段")
    private int had_draw_free_step_before;

    public static final int FIELD_had_draw_pay_step_before =4;
    @DataBaseField(type = "int(11)", fieldname = "had_draw_pay_step_before", comment = "领取前付费档已领取阶段")
    private int had_draw_pay_step_before;

    public static final int FIELD_had_draw_free_step_after =5;
    @DataBaseField(type = "int(11)", fieldname = "had_draw_free_step_after", comment = "领取后免费档已领取阶段")
    private int had_draw_free_step_after;

    public static final int FIELD_had_draw_pay_step_after =6;
    @DataBaseField(type = "int(11)", fieldname = "had_draw_pay_step_after", comment = "领取后付费档已领取阶段")
    private int had_draw_pay_step_after;

    public static final int FIELD_event_id =7;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =8;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =9;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =10;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public LogActivityFundDrawBO() {
        id = 0;
        cid = 0L;
        fund_id = 0L;
        activity_instance_id = 0L;
        had_draw_free_step_before = 0;
        had_draw_pay_step_before = 0;
        had_draw_free_step_after = 0;
        had_draw_pay_step_after = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
    }

    public LogActivityFundDrawBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        fund_id = rs.getLong(3);
        activity_instance_id = rs.getLong(4);
        had_draw_free_step_before = rs.getInt(5);
        had_draw_pay_step_before = rs.getInt(6);
        had_draw_free_step_after = rs.getInt(7);
        had_draw_pay_step_after = rs.getInt(8);
        event_id = rs.getInt(9);
        guid = rs.getLong(10);
        date_time = rs.getInt(11);
        timestamp = rs.getInt(12);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogActivityFundDrawBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `fund_id`, `activity_instance_id`, `had_draw_free_step_before`, `had_draw_pay_step_before`, `had_draw_free_step_after`, `had_draw_pay_step_after`, `event_id`, `guid`, `date_time`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`log_activity_fund_draw`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(fund_id).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(had_draw_free_step_before).append("', ");
        strBuf.append("'").append(had_draw_pay_step_before).append("', ");
        strBuf.append("'").append(had_draw_free_step_after).append("', ");
        strBuf.append("'").append(had_draw_pay_step_after).append("', ");
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

    // 领取前免费档已领取阶段
    public int getHadDrawFreeStepBefore() { return this.had_draw_free_step_before; }
    public void setHadDrawFreeStepBefore(BM _bm, int had_draw_free_step_before) {
        if(had_draw_free_step_before==this.had_draw_free_step_before) 
            return;
        this.had_draw_free_step_before = had_draw_free_step_before; 
        markField(_bm, FIELD_had_draw_free_step_before); 
    }
    public void saveHadDrawFreeStepBefore(BM _bm, int had_draw_free_step_before) {
        if(had_draw_free_step_before==this.had_draw_free_step_before) 
            return;
        this.had_draw_free_step_before = had_draw_free_step_before;
        saveField(_bm, "had_draw_free_step_before", had_draw_free_step_before);
    }

    // 领取前付费档已领取阶段
    public int getHadDrawPayStepBefore() { return this.had_draw_pay_step_before; }
    public void setHadDrawPayStepBefore(BM _bm, int had_draw_pay_step_before) {
        if(had_draw_pay_step_before==this.had_draw_pay_step_before) 
            return;
        this.had_draw_pay_step_before = had_draw_pay_step_before; 
        markField(_bm, FIELD_had_draw_pay_step_before); 
    }
    public void saveHadDrawPayStepBefore(BM _bm, int had_draw_pay_step_before) {
        if(had_draw_pay_step_before==this.had_draw_pay_step_before) 
            return;
        this.had_draw_pay_step_before = had_draw_pay_step_before;
        saveField(_bm, "had_draw_pay_step_before", had_draw_pay_step_before);
    }

    // 领取后免费档已领取阶段
    public int getHadDrawFreeStepAfter() { return this.had_draw_free_step_after; }
    public void setHadDrawFreeStepAfter(BM _bm, int had_draw_free_step_after) {
        if(had_draw_free_step_after==this.had_draw_free_step_after) 
            return;
        this.had_draw_free_step_after = had_draw_free_step_after; 
        markField(_bm, FIELD_had_draw_free_step_after); 
    }
    public void saveHadDrawFreeStepAfter(BM _bm, int had_draw_free_step_after) {
        if(had_draw_free_step_after==this.had_draw_free_step_after) 
            return;
        this.had_draw_free_step_after = had_draw_free_step_after;
        saveField(_bm, "had_draw_free_step_after", had_draw_free_step_after);
    }

    // 领取后付费档已领取阶段
    public int getHadDrawPayStepAfter() { return this.had_draw_pay_step_after; }
    public void setHadDrawPayStepAfter(BM _bm, int had_draw_pay_step_after) {
        if(had_draw_pay_step_after==this.had_draw_pay_step_after) 
            return;
        this.had_draw_pay_step_after = had_draw_pay_step_after; 
        markField(_bm, FIELD_had_draw_pay_step_after); 
    }
    public void saveHadDrawPayStepAfter(BM _bm, int had_draw_pay_step_after) {
        if(had_draw_pay_step_after==this.had_draw_pay_step_after) 
            return;
        this.had_draw_pay_step_after = had_draw_pay_step_after;
        saveField(_bm, "had_draw_pay_step_after", had_draw_pay_step_after);
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
        sBuilder.append(" `had_draw_free_step_before` = '").append(had_draw_free_step_before).append("',");
        sBuilder.append(" `had_draw_pay_step_before` = '").append(had_draw_pay_step_before).append("',");
        sBuilder.append(" `had_draw_free_step_after` = '").append(had_draw_free_step_after).append("',");
        sBuilder.append(" `had_draw_pay_step_after` = '").append(had_draw_pay_step_after).append("',");
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
        if(isFieldMarked(FIELD_had_draw_free_step_before)) sBuilder.append(" `had_draw_free_step_before` = '").append(had_draw_free_step_before).append("',");
        if(isFieldMarked(FIELD_had_draw_pay_step_before)) sBuilder.append(" `had_draw_pay_step_before` = '").append(had_draw_pay_step_before).append("',");
        if(isFieldMarked(FIELD_had_draw_free_step_after)) sBuilder.append(" `had_draw_free_step_after` = '").append(had_draw_free_step_after).append("',");
        if(isFieldMarked(FIELD_had_draw_pay_step_after)) sBuilder.append(" `had_draw_pay_step_after` = '").append(had_draw_pay_step_after).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_activity_fund_draw` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`fund_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '基金ID',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`had_draw_free_step_before` int(11) NOT NULL DEFAULT '0' COMMENT '领取前免费档已领取阶段',"
                + "`had_draw_pay_step_before` int(11) NOT NULL DEFAULT '0' COMMENT '领取前付费档已领取阶段',"
                + "`had_draw_free_step_after` int(11) NOT NULL DEFAULT '0' COMMENT '领取后免费档已领取阶段',"
                + "`had_draw_pay_step_after` int(11) NOT NULL DEFAULT '0' COMMENT '领取后付费档已领取阶段',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动基金奖励领取日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//had_draw_free_step_before
        _size+=4;//had_draw_pay_step_before
        _size+=4;//had_draw_free_step_after
        _size+=4;//had_draw_pay_step_after
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
        buff.putInt(had_draw_free_step_before);
        buff.putInt(had_draw_pay_step_before);
        buff.putInt(had_draw_free_step_after);
        buff.putInt(had_draw_pay_step_after);
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
        had_draw_free_step_before=buff.getInt();
        had_draw_pay_step_before=buff.getInt();
        had_draw_free_step_after=buff.getInt();
        had_draw_pay_step_after=buff.getInt();
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
