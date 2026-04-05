package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogQuestTargetBO extends BaseLogBo {

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

    public static final int FIELD_quest_id =5;
    @DataBaseField(type = "bigint(20)", fieldname = "quest_id", comment = "任务ID")
    private long quest_id;

    public static final int FIELD_quest_dbid =6;
    @DataBaseField(type = "bigint(20)", fieldname = "quest_dbid", comment = "任务数据ID")
    private long quest_dbid;

    public static final int FIELD_step =7;
    @DataBaseField(type = "bigint(20)", fieldname = "step", comment = "任务步骤")
    private long step;

    public static final int FIELD_target =8;
    @DataBaseField(type = "bigint(20)", fieldname = "target", comment = "目标ID")
    private long target;

    public static final int FIELD_ori_count =9;
    @DataBaseField(type = "bigint(20)", fieldname = "ori_count", comment = "原目标计数")
    private long ori_count;

    public static final int FIELD_cur_count =10;
    @DataBaseField(type = "bigint(20)", fieldname = "cur_count", comment = "现目标计数")
    private long cur_count;

    public LogQuestTargetBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        cid = 0L;
        quest_id = 0L;
        quest_dbid = 0L;
        step = 0L;
        target = 0L;
        ori_count = 0L;
        cur_count = 0L;
    }

    public LogQuestTargetBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        cid = rs.getLong(6);
        quest_id = rs.getLong(7);
        quest_dbid = rs.getLong(8);
        step = rs.getLong(9);
        target = rs.getLong(10);
        ori_count = rs.getLong(11);
        cur_count = rs.getLong(12);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogQuestTargetBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `cid`, `quest_id`, `quest_dbid`, `step`, `target`, `ori_count`, `cur_count`";
    }

    @Override
    public String getTableName() {
        return "`log_quest_target`";
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
        strBuf.append("'").append(quest_id).append("', ");
        strBuf.append("'").append(quest_dbid).append("', ");
        strBuf.append("'").append(step).append("', ");
        strBuf.append("'").append(target).append("', ");
        strBuf.append("'").append(ori_count).append("', ");
        strBuf.append("'").append(cur_count).append("', ");
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

    // 任务步骤
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

    // 目标ID
    public long getTarget() { return this.target; }
    public void setTarget(BM _bm, long target) {
        if(target==this.target) 
            return;
        this.target = target; 
        markField(_bm, FIELD_target); 
    }
    public void saveTarget(BM _bm, long target) {
        if(target==this.target) 
            return;
        this.target = target;
        saveField(_bm, "target", target);
    }

    // 原目标计数
    public long getOriCount() { return this.ori_count; }
    public void setOriCount(BM _bm, long ori_count) {
        if(ori_count==this.ori_count) 
            return;
        this.ori_count = ori_count; 
        markField(_bm, FIELD_ori_count); 
    }
    public void saveOriCount(BM _bm, long ori_count) {
        if(ori_count==this.ori_count) 
            return;
        this.ori_count = ori_count;
        saveField(_bm, "ori_count", ori_count);
    }

    // 现目标计数
    public long getCurCount() { return this.cur_count; }
    public void setCurCount(BM _bm, long cur_count) {
        if(cur_count==this.cur_count) 
            return;
        this.cur_count = cur_count; 
        markField(_bm, FIELD_cur_count); 
    }
    public void saveCurCount(BM _bm, long cur_count) {
        if(cur_count==this.cur_count) 
            return;
        this.cur_count = cur_count;
        saveField(_bm, "cur_count", cur_count);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        sBuilder.append(" `quest_dbid` = '").append(quest_dbid).append("',");
        sBuilder.append(" `step` = '").append(step).append("',");
        sBuilder.append(" `target` = '").append(target).append("',");
        sBuilder.append(" `ori_count` = '").append(ori_count).append("',");
        sBuilder.append(" `cur_count` = '").append(cur_count).append("',");
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
        if(isFieldMarked(FIELD_quest_id)) sBuilder.append(" `quest_id` = '").append(quest_id).append("',");
        if(isFieldMarked(FIELD_quest_dbid)) sBuilder.append(" `quest_dbid` = '").append(quest_dbid).append("',");
        if(isFieldMarked(FIELD_step)) sBuilder.append(" `step` = '").append(step).append("',");
        if(isFieldMarked(FIELD_target)) sBuilder.append(" `target` = '").append(target).append("',");
        if(isFieldMarked(FIELD_ori_count)) sBuilder.append(" `ori_count` = '").append(ori_count).append("',");
        if(isFieldMarked(FIELD_cur_count)) sBuilder.append(" `cur_count` = '").append(cur_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_quest_target` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`quest_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务ID',"
                + "`quest_dbid` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务数据ID',"
                + "`step` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务步骤',"
                + "`target` bigint(20) NOT NULL DEFAULT '0' COMMENT '目标ID',"
                + "`ori_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '原目标计数',"
                + "`cur_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '现目标计数',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家任务目标明细日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//quest_id
        _size+=8;//quest_dbid
        _size+=8;//step
        _size+=8;//target
        _size+=8;//ori_count
        _size+=8;//cur_count
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
        buff.putLong(quest_id);
        buff.putLong(quest_dbid);
        buff.putLong(step);
        buff.putLong(target);
        buff.putLong(ori_count);
        buff.putLong(cur_count);        
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
        quest_id=buff.getLong();
        quest_dbid=buff.getLong();
        step=buff.getLong();
        target=buff.getLong();
        ori_count=buff.getLong();
        cur_count=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
