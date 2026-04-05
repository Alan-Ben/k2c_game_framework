package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogAchievePointBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_event_id =1;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =3;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =4;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_achieve_type =5;
    @DataBaseField(type = "int(11)", fieldname = "achieve_type", comment = "成就类型")
    private int achieve_type;

    public static final int FIELD_ori_count =6;
    @DataBaseField(type = "bigint(20)", fieldname = "ori_count", comment = "原先成就点数")
    private long ori_count;

    public static final int FIELD_cur_value =7;
    @DataBaseField(type = "bigint(20)", fieldname = "cur_value", comment = "当前成就点数")
    private long cur_value;

    public LogAchievePointBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        achieve_type = 0;
        ori_count = 0L;
        cur_value = 0L;
    }

    public LogAchievePointBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        achieve_type = rs.getInt(7);
        ori_count = rs.getLong(8);
        cur_value = rs.getLong(9);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogAchievePointBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `achieve_type`, `ori_count`, `cur_value`";
    }

    @Override
    public String getTableName() {
        return "`log_achieve_point`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(achieve_type).append("', ");
        strBuf.append("'").append(ori_count).append("', ");
        strBuf.append("'").append(cur_value).append("', ");
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

    // 成就类型
    public int getAchieveType() { return this.achieve_type; }
    public void setAchieveType(BM _bm, int achieve_type) {
        if(achieve_type==this.achieve_type) 
            return;
        this.achieve_type = achieve_type; 
        markField(_bm, FIELD_achieve_type); 
    }
    public void saveAchieveType(BM _bm, int achieve_type) {
        if(achieve_type==this.achieve_type) 
            return;
        this.achieve_type = achieve_type;
        saveField(_bm, "achieve_type", achieve_type);
    }

    // 原先成就点数
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

    // 当前成就点数
    public long getCurValue() { return this.cur_value; }
    public void setCurValue(BM _bm, long cur_value) {
        if(cur_value==this.cur_value) 
            return;
        this.cur_value = cur_value; 
        markField(_bm, FIELD_cur_value); 
    }
    public void saveCurValue(BM _bm, long cur_value) {
        if(cur_value==this.cur_value) 
            return;
        this.cur_value = cur_value;
        saveField(_bm, "cur_value", cur_value);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `achieve_type` = '").append(achieve_type).append("',");
        sBuilder.append(" `ori_count` = '").append(ori_count).append("',");
        sBuilder.append(" `cur_value` = '").append(cur_value).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_achieve_type)) sBuilder.append(" `achieve_type` = '").append(achieve_type).append("',");
        if(isFieldMarked(FIELD_ori_count)) sBuilder.append(" `ori_count` = '").append(ori_count).append("',");
        if(isFieldMarked(FIELD_cur_value)) sBuilder.append(" `cur_value` = '").append(cur_value).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_achieve_point` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`achieve_type` int(11) NOT NULL DEFAULT '0' COMMENT '成就类型',"
                + "`ori_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '原先成就点数',"
                + "`cur_value` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前成就点数',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家成就点数据日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=4;//achieve_type
        _size+=8;//ori_count
        _size+=8;//cur_value
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putInt(achieve_type);
        buff.putLong(ori_count);
        buff.putLong(cur_value);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        achieve_type=buff.getInt();
        ori_count=buff.getLong();
        cur_value=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
