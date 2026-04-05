package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogConsortAddBO extends BaseLogBo {

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

    public static final int FIELD_consortId =5;
    @DataBaseField(type = "bigint(20)", fieldname = "consortId", comment = "妃子ID")
    private long consortId;

    public static final int FIELD_sourceType =6;
    @DataBaseField(type = "int(11)", fieldname = "sourceType", comment = "获取途径")
    private int sourceType;

    public static final int FIELD_allIntimacy =7;
    @DataBaseField(type = "bigint(20)", fieldname = "allIntimacy", comment = "总亲密度")
    private long allIntimacy;

    public static final int FIELD_allCharm =8;
    @DataBaseField(type = "bigint(20)", fieldname = "allCharm", comment = "总加护力")
    private long allCharm;

    public static final int FIELD_allCharmPointPer =9;
    @DataBaseField(type = "bigint(20)", fieldname = "allCharmPointPer", comment = "总加护点加成")
    private long allCharmPointPer;

    public LogConsortAddBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        consortId = 0L;
        sourceType = 0;
        allIntimacy = 0L;
        allCharm = 0L;
        allCharmPointPer = 0L;
    }

    public LogConsortAddBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        consortId = rs.getLong(7);
        sourceType = rs.getInt(8);
        allIntimacy = rs.getLong(9);
        allCharm = rs.getLong(10);
        allCharmPointPer = rs.getLong(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogConsortAddBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `consortId`, `sourceType`, `allIntimacy`, `allCharm`, `allCharmPointPer`";
    }

    @Override
    public String getTableName() {
        return "`log_consort_add`";
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
        strBuf.append("'").append(consortId).append("', ");
        strBuf.append("'").append(sourceType).append("', ");
        strBuf.append("'").append(allIntimacy).append("', ");
        strBuf.append("'").append(allCharm).append("', ");
        strBuf.append("'").append(allCharmPointPer).append("', ");
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

    // 妃子ID
    public long getConsortId() { return this.consortId; }
    public void setConsortId(BM _bm, long consortId) {
        if(consortId==this.consortId) 
            return;
        this.consortId = consortId; 
        markField(_bm, FIELD_consortId); 
    }
    public void saveConsortId(BM _bm, long consortId) {
        if(consortId==this.consortId) 
            return;
        this.consortId = consortId;
        saveField(_bm, "consortId", consortId);
    }

    // 获取途径
    public int getSourceType() { return this.sourceType; }
    public void setSourceType(BM _bm, int sourceType) {
        if(sourceType==this.sourceType) 
            return;
        this.sourceType = sourceType; 
        markField(_bm, FIELD_sourceType); 
    }
    public void saveSourceType(BM _bm, int sourceType) {
        if(sourceType==this.sourceType) 
            return;
        this.sourceType = sourceType;
        saveField(_bm, "sourceType", sourceType);
    }

    // 总亲密度
    public long getAllIntimacy() { return this.allIntimacy; }
    public void setAllIntimacy(BM _bm, long allIntimacy) {
        if(allIntimacy==this.allIntimacy) 
            return;
        this.allIntimacy = allIntimacy; 
        markField(_bm, FIELD_allIntimacy); 
    }
    public void saveAllIntimacy(BM _bm, long allIntimacy) {
        if(allIntimacy==this.allIntimacy) 
            return;
        this.allIntimacy = allIntimacy;
        saveField(_bm, "allIntimacy", allIntimacy);
    }

    // 总加护力
    public long getAllCharm() { return this.allCharm; }
    public void setAllCharm(BM _bm, long allCharm) {
        if(allCharm==this.allCharm) 
            return;
        this.allCharm = allCharm; 
        markField(_bm, FIELD_allCharm); 
    }
    public void saveAllCharm(BM _bm, long allCharm) {
        if(allCharm==this.allCharm) 
            return;
        this.allCharm = allCharm;
        saveField(_bm, "allCharm", allCharm);
    }

    // 总加护点加成
    public long getAllCharmPointPer() { return this.allCharmPointPer; }
    public void setAllCharmPointPer(BM _bm, long allCharmPointPer) {
        if(allCharmPointPer==this.allCharmPointPer) 
            return;
        this.allCharmPointPer = allCharmPointPer; 
        markField(_bm, FIELD_allCharmPointPer); 
    }
    public void saveAllCharmPointPer(BM _bm, long allCharmPointPer) {
        if(allCharmPointPer==this.allCharmPointPer) 
            return;
        this.allCharmPointPer = allCharmPointPer;
        saveField(_bm, "allCharmPointPer", allCharmPointPer);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `consortId` = '").append(consortId).append("',");
        sBuilder.append(" `sourceType` = '").append(sourceType).append("',");
        sBuilder.append(" `allIntimacy` = '").append(allIntimacy).append("',");
        sBuilder.append(" `allCharm` = '").append(allCharm).append("',");
        sBuilder.append(" `allCharmPointPer` = '").append(allCharmPointPer).append("',");
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
        if(isFieldMarked(FIELD_consortId)) sBuilder.append(" `consortId` = '").append(consortId).append("',");
        if(isFieldMarked(FIELD_sourceType)) sBuilder.append(" `sourceType` = '").append(sourceType).append("',");
        if(isFieldMarked(FIELD_allIntimacy)) sBuilder.append(" `allIntimacy` = '").append(allIntimacy).append("',");
        if(isFieldMarked(FIELD_allCharm)) sBuilder.append(" `allCharm` = '").append(allCharm).append("',");
        if(isFieldMarked(FIELD_allCharmPointPer)) sBuilder.append(" `allCharmPointPer` = '").append(allCharmPointPer).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_consort_add` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`consortId` bigint(20) NOT NULL DEFAULT '0' COMMENT '妃子ID',"
                + "`sourceType` int(11) NOT NULL DEFAULT '0' COMMENT '获取途径',"
                + "`allIntimacy` bigint(20) NOT NULL DEFAULT '0' COMMENT '总亲密度',"
                + "`allCharm` bigint(20) NOT NULL DEFAULT '0' COMMENT '总加护力',"
                + "`allCharmPointPer` bigint(20) NOT NULL DEFAULT '0' COMMENT '总加护点加成',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='妃子数据日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//consortId
        _size+=4;//sourceType
        _size+=8;//allIntimacy
        _size+=8;//allCharm
        _size+=8;//allCharmPointPer
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
        buff.putLong(consortId);
        buff.putInt(sourceType);
        buff.putLong(allIntimacy);
        buff.putLong(allCharm);
        buff.putLong(allCharmPointPer);        
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
        consortId=buff.getLong();
        sourceType=buff.getInt();
        allIntimacy=buff.getLong();
        allCharm=buff.getLong();
        allCharmPointPer=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
