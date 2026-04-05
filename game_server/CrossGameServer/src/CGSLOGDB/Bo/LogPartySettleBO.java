package CGSLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogPartySettleBO extends BaseLogBo {

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
    @DataBaseField(type = "bigint(20)", fieldname = "instance_id", comment = "宴会实例ID")
    private long instance_id;

    public static final int FIELD_cid =5;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_is_owner =6;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_owner", comment = "宴会拥有者")
    private boolean is_owner;

    public static final int FIELD_settled_secs =7;
    @DataBaseField(type = "int(11)", fieldname = "settled_secs", comment = "宴会结算时长")
    private int settled_secs;

    public static final int FIELD_seated_secs =8;
    @DataBaseField(type = "int(11)", fieldname = "seated_secs", comment = "宴会座位结算时长")
    private int seated_secs;

    public static final int FIELD_profit_secs =9;
    @DataBaseField(type = "int(11)", fieldname = "profit_secs", comment = "宴会收益时长")
    private int profit_secs;

    public LogPartySettleBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        instance_id = 0L;
        cid = 0L;
        is_owner = false;
        settled_secs = 0;
        seated_secs = 0;
        profit_secs = 0;
    }

    public LogPartySettleBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        instance_id = rs.getLong(6);
        cid = rs.getLong(7);
        is_owner = rs.getBoolean(8);
        settled_secs = rs.getInt(9);
        seated_secs = rs.getInt(10);
        profit_secs = rs.getInt(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogPartySettleBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `instance_id`, `cid`, `is_owner`, `settled_secs`, `seated_secs`, `profit_secs`";
    }

    @Override
    public String getTableName() {
        return "`log_party_settle`";
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
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(is_owner ? 1 : 0).append("', ");
        strBuf.append("'").append(settled_secs).append("', ");
        strBuf.append("'").append(seated_secs).append("', ");
        strBuf.append("'").append(profit_secs).append("', ");
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

    // 宴会实例ID
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

    // 宴会拥有者
    public boolean getIsOwner() { return this.is_owner; }
    public void setIsOwner(BM _bm, boolean is_owner) {
        if(is_owner==this.is_owner) 
            return;
        this.is_owner = is_owner; 
        markField(_bm, FIELD_is_owner); 
    }
    public void saveIsOwner(BM _bm, boolean is_owner) {
        if(is_owner==this.is_owner) 
            return;
        this.is_owner = is_owner;
        saveField(_bm, "is_owner", is_owner ? 1 : 0);
    }

    // 宴会结算时长
    public int getSettledSecs() { return this.settled_secs; }
    public void setSettledSecs(BM _bm, int settled_secs) {
        if(settled_secs==this.settled_secs) 
            return;
        this.settled_secs = settled_secs; 
        markField(_bm, FIELD_settled_secs); 
    }
    public void saveSettledSecs(BM _bm, int settled_secs) {
        if(settled_secs==this.settled_secs) 
            return;
        this.settled_secs = settled_secs;
        saveField(_bm, "settled_secs", settled_secs);
    }

    // 宴会座位结算时长
    public int getSeatedSecs() { return this.seated_secs; }
    public void setSeatedSecs(BM _bm, int seated_secs) {
        if(seated_secs==this.seated_secs) 
            return;
        this.seated_secs = seated_secs; 
        markField(_bm, FIELD_seated_secs); 
    }
    public void saveSeatedSecs(BM _bm, int seated_secs) {
        if(seated_secs==this.seated_secs) 
            return;
        this.seated_secs = seated_secs;
        saveField(_bm, "seated_secs", seated_secs);
    }

    // 宴会收益时长
    public int getProfitSecs() { return this.profit_secs; }
    public void setProfitSecs(BM _bm, int profit_secs) {
        if(profit_secs==this.profit_secs) 
            return;
        this.profit_secs = profit_secs; 
        markField(_bm, FIELD_profit_secs); 
    }
    public void saveProfitSecs(BM _bm, int profit_secs) {
        if(profit_secs==this.profit_secs) 
            return;
        this.profit_secs = profit_secs;
        saveField(_bm, "profit_secs", profit_secs);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `is_owner` = '").append(is_owner ? 1 : 0).append("',");
        sBuilder.append(" `settled_secs` = '").append(settled_secs).append("',");
        sBuilder.append(" `seated_secs` = '").append(seated_secs).append("',");
        sBuilder.append(" `profit_secs` = '").append(profit_secs).append("',");
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
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_is_owner)) sBuilder.append(" `is_owner` = '").append(is_owner ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_settled_secs)) sBuilder.append(" `settled_secs` = '").append(settled_secs).append("',");
        if(isFieldMarked(FIELD_seated_secs)) sBuilder.append(" `seated_secs` = '").append(seated_secs).append("',");
        if(isFieldMarked(FIELD_profit_secs)) sBuilder.append(" `profit_secs` = '").append(profit_secs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_party_settle` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '宴会实例ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`is_owner` tinyint(1) NOT NULL DEFAULT '0' COMMENT '宴会拥有者',"
                + "`settled_secs` int(11) NOT NULL DEFAULT '0' COMMENT '宴会结算时长',"
                + "`seated_secs` int(11) NOT NULL DEFAULT '0' COMMENT '宴会座位结算时长',"
                + "`profit_secs` int(11) NOT NULL DEFAULT '0' COMMENT '宴会收益时长',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='聚会-聚会结算' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=1;//is_owner
        _size+=4;//settled_secs
        _size+=4;//seated_secs
        _size+=4;//profit_secs
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
        buff.putLong(cid);
        buff.put((byte)(is_owner?1:0));
        buff.putInt(settled_secs);
        buff.putInt(seated_secs);
        buff.putInt(profit_secs);        
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
        cid=buff.getLong();
        is_owner=(buff.get()==1);
        settled_secs=buff.getInt();
        seated_secs=buff.getInt();
        profit_secs=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
