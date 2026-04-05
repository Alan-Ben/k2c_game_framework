package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogAdultStatusChgBO extends BaseLogBo {

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

    public static final int FIELD_preStatus =6;
    @DataBaseField(type = "int(11)", fieldname = "preStatus", comment = "原状态")
    private int preStatus;

    public static final int FIELD_newStatus =7;
    @DataBaseField(type = "int(11)", fieldname = "newStatus", comment = "新状态")
    private int newStatus;

    public static final int FIELD_expiredTs =8;
    @DataBaseField(type = "int(11)", fieldname = "expiredTs", comment = "截至时间戳（秒）")
    private int expiredTs;

    public LogAdultStatusChgBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        adultId = 0L;
        preStatus = 0;
        newStatus = 0;
        expiredTs = 0;
    }

    public LogAdultStatusChgBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        adultId = rs.getLong(7);
        preStatus = rs.getInt(8);
        newStatus = rs.getInt(9);
        expiredTs = rs.getInt(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogAdultStatusChgBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `adultId`, `preStatus`, `newStatus`, `expiredTs`";
    }

    @Override
    public String getTableName() {
        return "`log_adult_status_chg`";
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
        strBuf.append("'").append(preStatus).append("', ");
        strBuf.append("'").append(newStatus).append("', ");
        strBuf.append("'").append(expiredTs).append("', ");
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

    // 原状态
    public int getPreStatus() { return this.preStatus; }
    public void setPreStatus(BM _bm, int preStatus) {
        if(preStatus==this.preStatus) 
            return;
        this.preStatus = preStatus; 
        markField(_bm, FIELD_preStatus); 
    }
    public void savePreStatus(BM _bm, int preStatus) {
        if(preStatus==this.preStatus) 
            return;
        this.preStatus = preStatus;
        saveField(_bm, "preStatus", preStatus);
    }

    // 新状态
    public int getNewStatus() { return this.newStatus; }
    public void setNewStatus(BM _bm, int newStatus) {
        if(newStatus==this.newStatus) 
            return;
        this.newStatus = newStatus; 
        markField(_bm, FIELD_newStatus); 
    }
    public void saveNewStatus(BM _bm, int newStatus) {
        if(newStatus==this.newStatus) 
            return;
        this.newStatus = newStatus;
        saveField(_bm, "newStatus", newStatus);
    }

    // 截至时间戳（秒）
    public int getExpiredTs() { return this.expiredTs; }
    public void setExpiredTs(BM _bm, int expiredTs) {
        if(expiredTs==this.expiredTs) 
            return;
        this.expiredTs = expiredTs; 
        markField(_bm, FIELD_expiredTs); 
    }
    public void saveExpiredTs(BM _bm, int expiredTs) {
        if(expiredTs==this.expiredTs) 
            return;
        this.expiredTs = expiredTs;
        saveField(_bm, "expiredTs", expiredTs);
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
        sBuilder.append(" `preStatus` = '").append(preStatus).append("',");
        sBuilder.append(" `newStatus` = '").append(newStatus).append("',");
        sBuilder.append(" `expiredTs` = '").append(expiredTs).append("',");
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
        if(isFieldMarked(FIELD_preStatus)) sBuilder.append(" `preStatus` = '").append(preStatus).append("',");
        if(isFieldMarked(FIELD_newStatus)) sBuilder.append(" `newStatus` = '").append(newStatus).append("',");
        if(isFieldMarked(FIELD_expiredTs)) sBuilder.append(" `expiredTs` = '").append(expiredTs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_adult_status_chg` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`adultId` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣实例ID',"
                + "`preStatus` int(11) NOT NULL DEFAULT '0' COMMENT '原状态',"
                + "`newStatus` int(11) NOT NULL DEFAULT '0' COMMENT '新状态',"
                + "`expiredTs` int(11) NOT NULL DEFAULT '0' COMMENT '截至时间戳（秒）',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家成年子嗣状态变更日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//preStatus
        _size+=4;//newStatus
        _size+=4;//expiredTs
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
        buff.putInt(preStatus);
        buff.putInt(newStatus);
        buff.putInt(expiredTs);        
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
        preStatus=buff.getInt();
        newStatus=buff.getInt();
        expiredTs=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
