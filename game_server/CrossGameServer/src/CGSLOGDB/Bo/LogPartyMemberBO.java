package CGSLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogPartyMemberBO extends BaseLogBo {

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
    @DataBaseField(type = "tinyint(1)", fieldname = "is_owner", comment = "是否开宴 true-开宴")
    private boolean is_owner;

    public static final int FIELD_is_add =7;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_add", comment = "是否赴宴 true-赴宴")
    private boolean is_add;

    public static final int FIELD_seat_idx =8;
    @DataBaseField(type = "int(11)", fieldname = "seat_idx", comment = "座位下标")
    private int seat_idx;

    public static final int FIELD_join_ts =9;
    @DataBaseField(type = "bigint(20)", fieldname = "join_ts", comment = "入座时间")
    private long join_ts;

    public static final int FIELD_protect_end_ts =10;
    @DataBaseField(type = "bigint(20)", fieldname = "protect_end_ts", comment = "保护截至时间")
    private long protect_end_ts;

    public static final int FIELD_profit_end_ts =11;
    @DataBaseField(type = "bigint(20)", fieldname = "profit_end_ts", comment = "收益截至时间")
    private long profit_end_ts;

    public LogPartyMemberBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        instance_id = 0L;
        cid = 0L;
        is_owner = false;
        is_add = false;
        seat_idx = 0;
        join_ts = 0L;
        protect_end_ts = 0L;
        profit_end_ts = 0L;
    }

    public LogPartyMemberBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        instance_id = rs.getLong(6);
        cid = rs.getLong(7);
        is_owner = rs.getBoolean(8);
        is_add = rs.getBoolean(9);
        seat_idx = rs.getInt(10);
        join_ts = rs.getLong(11);
        protect_end_ts = rs.getLong(12);
        profit_end_ts = rs.getLong(13);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogPartyMemberBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `instance_id`, `cid`, `is_owner`, `is_add`, `seat_idx`, `join_ts`, `protect_end_ts`, `profit_end_ts`";
    }

    @Override
    public String getTableName() {
        return "`log_party_member`";
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
        strBuf.append("'").append(is_add ? 1 : 0).append("', ");
        strBuf.append("'").append(seat_idx).append("', ");
        strBuf.append("'").append(join_ts).append("', ");
        strBuf.append("'").append(protect_end_ts).append("', ");
        strBuf.append("'").append(profit_end_ts).append("', ");
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

    // 是否开宴 true-开宴
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

    // 是否赴宴 true-赴宴
    public boolean getIsAdd() { return this.is_add; }
    public void setIsAdd(BM _bm, boolean is_add) {
        if(is_add==this.is_add) 
            return;
        this.is_add = is_add; 
        markField(_bm, FIELD_is_add); 
    }
    public void saveIsAdd(BM _bm, boolean is_add) {
        if(is_add==this.is_add) 
            return;
        this.is_add = is_add;
        saveField(_bm, "is_add", is_add ? 1 : 0);
    }

    // 座位下标
    public int getSeatIdx() { return this.seat_idx; }
    public void setSeatIdx(BM _bm, int seat_idx) {
        if(seat_idx==this.seat_idx) 
            return;
        this.seat_idx = seat_idx; 
        markField(_bm, FIELD_seat_idx); 
    }
    public void saveSeatIdx(BM _bm, int seat_idx) {
        if(seat_idx==this.seat_idx) 
            return;
        this.seat_idx = seat_idx;
        saveField(_bm, "seat_idx", seat_idx);
    }

    // 入座时间
    public long getJoinTs() { return this.join_ts; }
    public void setJoinTs(BM _bm, long join_ts) {
        if(join_ts==this.join_ts) 
            return;
        this.join_ts = join_ts; 
        markField(_bm, FIELD_join_ts); 
    }
    public void saveJoinTs(BM _bm, long join_ts) {
        if(join_ts==this.join_ts) 
            return;
        this.join_ts = join_ts;
        saveField(_bm, "join_ts", join_ts);
    }

    // 保护截至时间
    public long getProtectEndTs() { return this.protect_end_ts; }
    public void setProtectEndTs(BM _bm, long protect_end_ts) {
        if(protect_end_ts==this.protect_end_ts) 
            return;
        this.protect_end_ts = protect_end_ts; 
        markField(_bm, FIELD_protect_end_ts); 
    }
    public void saveProtectEndTs(BM _bm, long protect_end_ts) {
        if(protect_end_ts==this.protect_end_ts) 
            return;
        this.protect_end_ts = protect_end_ts;
        saveField(_bm, "protect_end_ts", protect_end_ts);
    }

    // 收益截至时间
    public long getProfitEndTs() { return this.profit_end_ts; }
    public void setProfitEndTs(BM _bm, long profit_end_ts) {
        if(profit_end_ts==this.profit_end_ts) 
            return;
        this.profit_end_ts = profit_end_ts; 
        markField(_bm, FIELD_profit_end_ts); 
    }
    public void saveProfitEndTs(BM _bm, long profit_end_ts) {
        if(profit_end_ts==this.profit_end_ts) 
            return;
        this.profit_end_ts = profit_end_ts;
        saveField(_bm, "profit_end_ts", profit_end_ts);
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
        sBuilder.append(" `is_add` = '").append(is_add ? 1 : 0).append("',");
        sBuilder.append(" `seat_idx` = '").append(seat_idx).append("',");
        sBuilder.append(" `join_ts` = '").append(join_ts).append("',");
        sBuilder.append(" `protect_end_ts` = '").append(protect_end_ts).append("',");
        sBuilder.append(" `profit_end_ts` = '").append(profit_end_ts).append("',");
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
        if(isFieldMarked(FIELD_is_add)) sBuilder.append(" `is_add` = '").append(is_add ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_seat_idx)) sBuilder.append(" `seat_idx` = '").append(seat_idx).append("',");
        if(isFieldMarked(FIELD_join_ts)) sBuilder.append(" `join_ts` = '").append(join_ts).append("',");
        if(isFieldMarked(FIELD_protect_end_ts)) sBuilder.append(" `protect_end_ts` = '").append(protect_end_ts).append("',");
        if(isFieldMarked(FIELD_profit_end_ts)) sBuilder.append(" `profit_end_ts` = '").append(profit_end_ts).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_party_member` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '宴会实例ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`is_owner` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否开宴 true-开宴',"
                + "`is_add` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否赴宴 true-赴宴',"
                + "`seat_idx` int(11) NOT NULL DEFAULT '0' COMMENT '座位下标',"
                + "`join_ts` bigint(20) NOT NULL DEFAULT '0' COMMENT '入座时间',"
                + "`protect_end_ts` bigint(20) NOT NULL DEFAULT '0' COMMENT '保护截至时间',"
                + "`profit_end_ts` bigint(20) NOT NULL DEFAULT '0' COMMENT '收益截至时间',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='宴会-宴会玩家日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=1;//is_add
        _size+=4;//seat_idx
        _size+=8;//join_ts
        _size+=8;//protect_end_ts
        _size+=8;//profit_end_ts
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
        buff.put((byte)(is_add?1:0));
        buff.putInt(seat_idx);
        buff.putLong(join_ts);
        buff.putLong(protect_end_ts);
        buff.putLong(profit_end_ts);        
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
        is_add=(buff.get()==1);
        seat_idx=buff.getInt();
        join_ts=buff.getLong();
        protect_end_ts=buff.getLong();
        profit_end_ts=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
