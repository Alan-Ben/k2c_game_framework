package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogAdultMarriedAdultBO extends BaseLogBo {

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

    public static final int FIELD_adultId =5;
    @DataBaseField(type = "bigint(20)", fieldname = "adultId", comment = "子嗣实例ID")
    private long adultId;

    public static final int FIELD_marriedCid =6;
    @DataBaseField(type = "bigint(20)", fieldname = "marriedCid", comment = "关联玩家CID")
    private long marriedCid;

    public static final int FIELD_marriedAdultId =7;
    @DataBaseField(type = "bigint(20)", fieldname = "marriedAdultId", comment = "关联子嗣实例ID")
    private long marriedAdultId;

    public static final int FIELD_marriedBonus =8;
    @DataBaseField(type = "bigint(20)", fieldname = "marriedBonus", comment = "关联子嗣收益")
    private long marriedBonus;

    public LogAdultMarriedAdultBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        adultId = 0L;
        marriedCid = 0L;
        marriedAdultId = 0L;
        marriedBonus = 0L;
    }

    public LogAdultMarriedAdultBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        adultId = rs.getLong(7);
        marriedCid = rs.getLong(8);
        marriedAdultId = rs.getLong(9);
        marriedBonus = rs.getLong(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogAdultMarriedAdultBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `adultId`, `marriedCid`, `marriedAdultId`, `marriedBonus`";
    }

    @Override
    public String getTableName() {
        return "`log_adult_married_adult`";
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
        strBuf.append("'").append(adultId).append("', ");
        strBuf.append("'").append(marriedCid).append("', ");
        strBuf.append("'").append(marriedAdultId).append("', ");
        strBuf.append("'").append(marriedBonus).append("', ");
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

    // 子嗣实例ID
    public long getAdultId() { return this.adultId; }
    public void setAdultId(BM _bm, long adultId) {
        if(adultId==this.adultId) 
            return;
        this.adultId = adultId; 
        markField(_bm, FIELD_adultId); 
    }
    public void saveAdultId(BM _bm, long adultId) {
        if(adultId==this.adultId) 
            return;
        this.adultId = adultId;
        saveField(_bm, "adultId", adultId);
    }

    // 关联玩家CID
    public long getMarriedCid() { return this.marriedCid; }
    public void setMarriedCid(BM _bm, long marriedCid) {
        if(marriedCid==this.marriedCid) 
            return;
        this.marriedCid = marriedCid; 
        markField(_bm, FIELD_marriedCid); 
    }
    public void saveMarriedCid(BM _bm, long marriedCid) {
        if(marriedCid==this.marriedCid) 
            return;
        this.marriedCid = marriedCid;
        saveField(_bm, "marriedCid", marriedCid);
    }

    // 关联子嗣实例ID
    public long getMarriedAdultId() { return this.marriedAdultId; }
    public void setMarriedAdultId(BM _bm, long marriedAdultId) {
        if(marriedAdultId==this.marriedAdultId) 
            return;
        this.marriedAdultId = marriedAdultId; 
        markField(_bm, FIELD_marriedAdultId); 
    }
    public void saveMarriedAdultId(BM _bm, long marriedAdultId) {
        if(marriedAdultId==this.marriedAdultId) 
            return;
        this.marriedAdultId = marriedAdultId;
        saveField(_bm, "marriedAdultId", marriedAdultId);
    }

    // 关联子嗣收益
    public long getMarriedBonus() { return this.marriedBonus; }
    public void setMarriedBonus(BM _bm, long marriedBonus) {
        if(marriedBonus==this.marriedBonus) 
            return;
        this.marriedBonus = marriedBonus; 
        markField(_bm, FIELD_marriedBonus); 
    }
    public void saveMarriedBonus(BM _bm, long marriedBonus) {
        if(marriedBonus==this.marriedBonus) 
            return;
        this.marriedBonus = marriedBonus;
        saveField(_bm, "marriedBonus", marriedBonus);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `adultId` = '").append(adultId).append("',");
        sBuilder.append(" `marriedCid` = '").append(marriedCid).append("',");
        sBuilder.append(" `marriedAdultId` = '").append(marriedAdultId).append("',");
        sBuilder.append(" `marriedBonus` = '").append(marriedBonus).append("',");
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
        if(isFieldMarked(FIELD_adultId)) sBuilder.append(" `adultId` = '").append(adultId).append("',");
        if(isFieldMarked(FIELD_marriedCid)) sBuilder.append(" `marriedCid` = '").append(marriedCid).append("',");
        if(isFieldMarked(FIELD_marriedAdultId)) sBuilder.append(" `marriedAdultId` = '").append(marriedAdultId).append("',");
        if(isFieldMarked(FIELD_marriedBonus)) sBuilder.append(" `marriedBonus` = '").append(marriedBonus).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_adult_married_adult` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`adultId` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣实例ID',"
                + "`marriedCid` bigint(20) NOT NULL DEFAULT '0' COMMENT '关联玩家CID',"
                + "`marriedAdultId` bigint(20) NOT NULL DEFAULT '0' COMMENT '关联子嗣实例ID',"
                + "`marriedBonus` bigint(20) NOT NULL DEFAULT '0' COMMENT '关联子嗣收益',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家成年子嗣联姻关联子嗣日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//adultId
        _size+=8;//marriedCid
        _size+=8;//marriedAdultId
        _size+=8;//marriedBonus
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
        buff.putLong(adultId);
        buff.putLong(marriedCid);
        buff.putLong(marriedAdultId);
        buff.putLong(marriedBonus);        
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
        adultId=buff.getLong();
        marriedCid=buff.getLong();
        marriedAdultId=buff.getLong();
        marriedBonus=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
