package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogActivityStepRewardDrawBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_instance_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "instance_id", comment = "活动实例ID")
    private long instance_id;

    public static final int FIELD_step_reward_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "step_reward_id", comment = "阶段奖励ID")
    private long step_reward_id;

    public static final int FIELD_step_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "step_id", comment = "阶段ID")
    private long step_id;

    public static final int FIELD_is_a_key =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_a_key", comment = "是否一键领取")
    private boolean is_a_key;

    public static final int FIELD_event_id =5;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =6;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =7;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =8;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public LogActivityStepRewardDrawBO() {
        id = 0;
        cid = 0L;
        instance_id = 0L;
        step_reward_id = 0L;
        step_id = 0L;
        is_a_key = false;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
    }

    public LogActivityStepRewardDrawBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        instance_id = rs.getLong(3);
        step_reward_id = rs.getLong(4);
        step_id = rs.getLong(5);
        is_a_key = rs.getBoolean(6);
        event_id = rs.getInt(7);
        guid = rs.getLong(8);
        date_time = rs.getInt(9);
        timestamp = rs.getInt(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogActivityStepRewardDrawBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `instance_id`, `step_reward_id`, `step_id`, `is_a_key`, `event_id`, `guid`, `date_time`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`log_activity_step_reward_draw`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(instance_id).append("', ");
        strBuf.append("'").append(step_reward_id).append("', ");
        strBuf.append("'").append(step_id).append("', ");
        strBuf.append("'").append(is_a_key ? 1 : 0).append("', ");
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

    // 活动实例ID
    public long getInstanceId() { return this.instance_id; }
    public void setInstanceId(BM _bm, long instance_id) {
        if(instance_id==this.instance_id) 
            return;
        this.instance_id = instance_id; 
        markField(_bm, FIELD_instance_id); 
    }
    public void saveInstanceId(BM _bm, long instance_id) {
        if(instance_id==this.instance_id) 
            return;
        this.instance_id = instance_id;
        saveField(_bm, "instance_id", instance_id);
    }

    // 阶段奖励ID
    public long getStepRewardId() { return this.step_reward_id; }
    public void setStepRewardId(BM _bm, long step_reward_id) {
        if(step_reward_id==this.step_reward_id) 
            return;
        this.step_reward_id = step_reward_id; 
        markField(_bm, FIELD_step_reward_id); 
    }
    public void saveStepRewardId(BM _bm, long step_reward_id) {
        if(step_reward_id==this.step_reward_id) 
            return;
        this.step_reward_id = step_reward_id;
        saveField(_bm, "step_reward_id", step_reward_id);
    }

    // 阶段ID
    public long getStepId() { return this.step_id; }
    public void setStepId(BM _bm, long step_id) {
        if(step_id==this.step_id) 
            return;
        this.step_id = step_id; 
        markField(_bm, FIELD_step_id); 
    }
    public void saveStepId(BM _bm, long step_id) {
        if(step_id==this.step_id) 
            return;
        this.step_id = step_id;
        saveField(_bm, "step_id", step_id);
    }

    // 是否一键领取
    public boolean getIsAKey() { return this.is_a_key; }
    public void setIsAKey(BM _bm, boolean is_a_key) {
        if(is_a_key==this.is_a_key) 
            return;
        this.is_a_key = is_a_key; 
        markField(_bm, FIELD_is_a_key); 
    }
    public void saveIsAKey(BM _bm, boolean is_a_key) {
        if(is_a_key==this.is_a_key) 
            return;
        this.is_a_key = is_a_key;
        saveField(_bm, "is_a_key", is_a_key ? 1 : 0);
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
        sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        sBuilder.append(" `step_reward_id` = '").append(step_reward_id).append("',");
        sBuilder.append(" `step_id` = '").append(step_id).append("',");
        sBuilder.append(" `is_a_key` = '").append(is_a_key ? 1 : 0).append("',");
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
        if(isFieldMarked(FIELD_instance_id)) sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        if(isFieldMarked(FIELD_step_reward_id)) sBuilder.append(" `step_reward_id` = '").append(step_reward_id).append("',");
        if(isFieldMarked(FIELD_step_id)) sBuilder.append(" `step_id` = '").append(step_id).append("',");
        if(isFieldMarked(FIELD_is_a_key)) sBuilder.append(" `is_a_key` = '").append(is_a_key ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_activity_step_reward_draw` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`step_reward_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '阶段奖励ID',"
                + "`step_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '阶段ID',"
                + "`is_a_key` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否一键领取',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动阶段奖励领取日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//instance_id
        _size+=8;//step_reward_id
        _size+=8;//step_id
        _size+=1;//is_a_key
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
        buff.putLong(instance_id);
        buff.putLong(step_reward_id);
        buff.putLong(step_id);
        buff.put((byte)(is_a_key?1:0));
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
        instance_id=buff.getLong();
        step_reward_id=buff.getLong();
        step_id=buff.getLong();
        is_a_key=(buff.get()==1);
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
