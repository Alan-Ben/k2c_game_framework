package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogMarsEventAddBO extends BaseLogBo {

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

    public static final int FIELD_instanceId =5;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "实例ID")
    private long instanceId;

    public static final int FIELD_exploreLvl =6;
    @DataBaseField(type = "int(11)", fieldname = "exploreLvl", comment = "探索等级")
    private int exploreLvl;

    public static final int FIELD_eventType =7;
    @DataBaseField(type = "int(11)", fieldname = "eventType", comment = "事件类型")
    private int eventType;

    public static final int FIELD_eventRefId =8;
    @DataBaseField(type = "bigint(20)", fieldname = "eventRefId", comment = "事件ID")
    private long eventRefId;

    public static final int FIELD_quality =9;
    @DataBaseField(type = "int(11)", fieldname = "quality", comment = "事件品质")
    private int quality;

    public static final int FIELD_pos =10;
    @DataBaseField(type = "bigint(20)", fieldname = "pos", comment = "事件位置")
    private long pos;

    public LogMarsEventAddBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        instanceId = 0L;
        exploreLvl = 0;
        eventType = 0;
        eventRefId = 0L;
        quality = 0;
        pos = 0L;
    }

    public LogMarsEventAddBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        instanceId = rs.getLong(7);
        exploreLvl = rs.getInt(8);
        eventType = rs.getInt(9);
        eventRefId = rs.getLong(10);
        quality = rs.getInt(11);
        pos = rs.getLong(12);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogMarsEventAddBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `instanceId`, `exploreLvl`, `eventType`, `eventRefId`, `quality`, `pos`";
    }

    @Override
    public String getTableName() {
        return "`log_mars_event_add`";
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
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(exploreLvl).append("', ");
        strBuf.append("'").append(eventType).append("', ");
        strBuf.append("'").append(eventRefId).append("', ");
        strBuf.append("'").append(quality).append("', ");
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
    public long getInstanceId() { return this.instanceId; }
    public void setInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId; 
        markField(_bm, FIELD_instanceId); 
    }
    public void saveInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId;
        saveField(_bm, "instanceId", instanceId);
    }

    // 探索等级
    public int getExploreLvl() { return this.exploreLvl; }
    public void setExploreLvl(BM _bm, int exploreLvl) {
        if(exploreLvl==this.exploreLvl) 
            return;
        this.exploreLvl = exploreLvl; 
        markField(_bm, FIELD_exploreLvl); 
    }
    public void saveExploreLvl(BM _bm, int exploreLvl) {
        if(exploreLvl==this.exploreLvl) 
            return;
        this.exploreLvl = exploreLvl;
        saveField(_bm, "exploreLvl", exploreLvl);
    }

    // 事件类型
    public int getEventType() { return this.eventType; }
    public void setEventType(BM _bm, int eventType) {
        if(eventType==this.eventType) 
            return;
        this.eventType = eventType; 
        markField(_bm, FIELD_eventType); 
    }
    public void saveEventType(BM _bm, int eventType) {
        if(eventType==this.eventType) 
            return;
        this.eventType = eventType;
        saveField(_bm, "eventType", eventType);
    }

    // 事件ID
    public long getEventRefId() { return this.eventRefId; }
    public void setEventRefId(BM _bm, long eventRefId) {
        if(eventRefId==this.eventRefId) 
            return;
        this.eventRefId = eventRefId; 
        markField(_bm, FIELD_eventRefId); 
    }
    public void saveEventRefId(BM _bm, long eventRefId) {
        if(eventRefId==this.eventRefId) 
            return;
        this.eventRefId = eventRefId;
        saveField(_bm, "eventRefId", eventRefId);
    }

    // 事件品质
    public int getQuality() { return this.quality; }
    public void setQuality(BM _bm, int quality) {
        if(quality==this.quality) 
            return;
        this.quality = quality; 
        markField(_bm, FIELD_quality); 
    }
    public void saveQuality(BM _bm, int quality) {
        if(quality==this.quality) 
            return;
        this.quality = quality;
        saveField(_bm, "quality", quality);
    }

    // 事件位置
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
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `exploreLvl` = '").append(exploreLvl).append("',");
        sBuilder.append(" `eventType` = '").append(eventType).append("',");
        sBuilder.append(" `eventRefId` = '").append(eventRefId).append("',");
        sBuilder.append(" `quality` = '").append(quality).append("',");
        sBuilder.append(" `pos` = '").append(pos).append("',");
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
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_exploreLvl)) sBuilder.append(" `exploreLvl` = '").append(exploreLvl).append("',");
        if(isFieldMarked(FIELD_eventType)) sBuilder.append(" `eventType` = '").append(eventType).append("',");
        if(isFieldMarked(FIELD_eventRefId)) sBuilder.append(" `eventRefId` = '").append(eventRefId).append("',");
        if(isFieldMarked(FIELD_quality)) sBuilder.append(" `quality` = '").append(quality).append("',");
        if(isFieldMarked(FIELD_pos)) sBuilder.append(" `pos` = '").append(pos).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_mars_event_add` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '实例ID',"
                + "`exploreLvl` int(11) NOT NULL DEFAULT '0' COMMENT '探索等级',"
                + "`eventType` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`eventRefId` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件ID',"
                + "`quality` int(11) NOT NULL DEFAULT '0' COMMENT '事件品质',"
                + "`pos` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件位置',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家创建火星事件日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//instanceId
        _size+=4;//exploreLvl
        _size+=4;//eventType
        _size+=8;//eventRefId
        _size+=4;//quality
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
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putLong(instanceId);
        buff.putInt(exploreLvl);
        buff.putInt(eventType);
        buff.putLong(eventRefId);
        buff.putInt(quality);
        buff.putLong(pos);        
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
        instanceId=buff.getLong();
        exploreLvl=buff.getInt();
        eventType=buff.getInt();
        eventRefId=buff.getLong();
        quality=buff.getInt();
        pos=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
