package CGSLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogArenaPlayerFightBO extends BaseLogBo {

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

    public static final int FIELD_instance_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "instance_id", comment = "实例ID")
    private long instance_id;

    public static final int FIELD_ref_id =5;
    @DataBaseField(type = "bigint(20)", fieldname = "ref_id", comment = "配置ID")
    private long ref_id;

    public static final int FIELD_cid =6;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_ori_rank =7;
    @DataBaseField(type = "int(11)", fieldname = "ori_rank", comment = "源名次")
    private int ori_rank;

    public static final int FIELD_section_data =8;
    @DataBaseField(type = "varchar(512)", fieldname = "section_data", comment = "攻击方截面数据")
    private String section_data;

    public static final int FIELD_target_cid =9;
    @DataBaseField(type = "bigint(20)", fieldname = "target_cid", comment = "目标玩家CID")
    private long target_cid;

    public static final int FIELD_target_rank =10;
    @DataBaseField(type = "int(11)", fieldname = "target_rank", comment = "目标名次")
    private int target_rank;

    public static final int FIELD_target_section_data =11;
    @DataBaseField(type = "varchar(512)", fieldname = "target_section_data", comment = "防守方截面数据")
    private String target_section_data;

    public static final int FIELD_result =12;
    @DataBaseField(type = "int(11)", fieldname = "result", comment = "挑战结果")
    private int result;

    public LogArenaPlayerFightBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        instance_id = 0L;
        ref_id = 0L;
        cid = 0L;
        ori_rank = 0;
        section_data = "";
        target_cid = 0L;
        target_rank = 0;
        target_section_data = "";
        result = 0;
    }

    public LogArenaPlayerFightBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        instance_id = rs.getLong(6);
        ref_id = rs.getLong(7);
        cid = rs.getLong(8);
        ori_rank = rs.getInt(9);
        section_data = rs.getString(10);
        target_cid = rs.getLong(11);
        target_rank = rs.getInt(12);
        target_section_data = rs.getString(13);
        result = rs.getInt(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogArenaPlayerFightBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `instance_id`, `ref_id`, `cid`, `ori_rank`, `section_data`, `target_cid`, `target_rank`, `target_section_data`, `result`";
    }

    @Override
    public String getTableName() {
        return "`log_arena_player_fight`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(instance_id).append("', ");
        strBuf.append("'").append(ref_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(ori_rank).append("', ");
        strBuf.append("'").append(section_data == null ? null : section_data.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(target_cid).append("', ");
        strBuf.append("'").append(target_rank).append("', ");
        strBuf.append("'").append(target_section_data == null ? null : target_section_data.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(result).append("', ");
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

    // 实例ID
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

    // 配置ID
    public long getRefId() { return this.ref_id; }
    public void setRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id; 
        markField(_bm, FIELD_ref_id); 
    }
    public void saveRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id;
        saveField(_bm, "ref_id", ref_id);
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

    // 源名次
    public int getOriRank() { return this.ori_rank; }
    public void setOriRank(BM _bm, int ori_rank) {
        if(ori_rank==this.ori_rank) 
            return;
        this.ori_rank = ori_rank; 
        markField(_bm, FIELD_ori_rank); 
    }
    public void saveOriRank(BM _bm, int ori_rank) {
        if(ori_rank==this.ori_rank) 
            return;
        this.ori_rank = ori_rank;
        saveField(_bm, "ori_rank", ori_rank);
    }

    // 攻击方截面数据
    public String getSectionData() { return this.section_data; }
    public void setSectionData(BM _bm, String section_data) {
        if(section_data.equals(this.section_data)) 
            return;
        this.section_data = section_data; 
        markField(_bm, FIELD_section_data); 
    }
    public void saveSectionData(BM _bm, String section_data) {
        if(section_data.equals(this.section_data)) 
            return;
        this.section_data = section_data;
        saveField(_bm, "section_data", section_data);
    }

    // 目标玩家CID
    public long getTargetCid() { return this.target_cid; }
    public void setTargetCid(BM _bm, long target_cid) {
        if(target_cid==this.target_cid) 
            return;
        this.target_cid = target_cid; 
        markField(_bm, FIELD_target_cid); 
    }
    public void saveTargetCid(BM _bm, long target_cid) {
        if(target_cid==this.target_cid) 
            return;
        this.target_cid = target_cid;
        saveField(_bm, "target_cid", target_cid);
    }

    // 目标名次
    public int getTargetRank() { return this.target_rank; }
    public void setTargetRank(BM _bm, int target_rank) {
        if(target_rank==this.target_rank) 
            return;
        this.target_rank = target_rank; 
        markField(_bm, FIELD_target_rank); 
    }
    public void saveTargetRank(BM _bm, int target_rank) {
        if(target_rank==this.target_rank) 
            return;
        this.target_rank = target_rank;
        saveField(_bm, "target_rank", target_rank);
    }

    // 防守方截面数据
    public String getTargetSectionData() { return this.target_section_data; }
    public void setTargetSectionData(BM _bm, String target_section_data) {
        if(target_section_data.equals(this.target_section_data)) 
            return;
        this.target_section_data = target_section_data; 
        markField(_bm, FIELD_target_section_data); 
    }
    public void saveTargetSectionData(BM _bm, String target_section_data) {
        if(target_section_data.equals(this.target_section_data)) 
            return;
        this.target_section_data = target_section_data;
        saveField(_bm, "target_section_data", target_section_data);
    }

    // 挑战结果
    public int getResult() { return this.result; }
    public void setResult(BM _bm, int result) {
        if(result==this.result) 
            return;
        this.result = result; 
        markField(_bm, FIELD_result); 
    }
    public void saveResult(BM _bm, int result) {
        if(result==this.result) 
            return;
        this.result = result;
        saveField(_bm, "result", result);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `ori_rank` = '").append(ori_rank).append("',");
        sBuilder.append(" `section_data` = '").append(section_data == null ? null : section_data.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `target_cid` = '").append(target_cid).append("',");
        sBuilder.append(" `target_rank` = '").append(target_rank).append("',");
        sBuilder.append(" `target_section_data` = '").append(target_section_data == null ? null : target_section_data.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `result` = '").append(result).append("',");
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
        if(isFieldMarked(FIELD_instance_id)) sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        if(isFieldMarked(FIELD_ref_id)) sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_ori_rank)) sBuilder.append(" `ori_rank` = '").append(ori_rank).append("',");
        if(isFieldMarked(FIELD_section_data)) sBuilder.append(" `section_data` = '").append(section_data == null ? null : section_data.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_target_cid)) sBuilder.append(" `target_cid` = '").append(target_cid).append("',");
        if(isFieldMarked(FIELD_target_rank)) sBuilder.append(" `target_rank` = '").append(target_rank).append("',");
        if(isFieldMarked(FIELD_target_section_data)) sBuilder.append(" `target_section_data` = '").append(target_section_data == null ? null : target_section_data.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_result)) sBuilder.append(" `result` = '").append(result).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_arena_player_fight` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '实例ID',"
                + "`ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`ori_rank` int(11) NOT NULL DEFAULT '0' COMMENT '源名次',"
                + "`section_data` varchar(512) NOT NULL DEFAULT '' COMMENT '攻击方截面数据',"
                + "`target_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '目标玩家CID',"
                + "`target_rank` int(11) NOT NULL DEFAULT '0' COMMENT '目标名次',"
                + "`target_section_data` varchar(512) NOT NULL DEFAULT '' COMMENT '防守方截面数据',"
                + "`result` int(11) NOT NULL DEFAULT '0' COMMENT '挑战结果',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='擂台-挑战日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.crossgame_log;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=8;//instance_id
        _size+=8;//ref_id
        _size+=8;//cid
        _size+=4;//ori_rank
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(section_data);//section_data
        _size+=8;//target_cid
        _size+=4;//target_rank
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(target_section_data);//target_section_data
        _size+=4;//result
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
        buff.putLong(instance_id);
        buff.putLong(ref_id);
        buff.putLong(cid);
        buff.putInt(ori_rank);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, section_data);
        buff.putLong(target_cid);
        buff.putInt(target_rank);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, target_section_data);
        buff.putInt(result);        
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
        instance_id=buff.getLong();
        ref_id=buff.getLong();
        cid=buff.getLong();
        ori_rank=buff.getInt();
        section_data=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        target_cid=buff.getLong();
        target_rank=buff.getInt();
        target_section_data=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        result=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
