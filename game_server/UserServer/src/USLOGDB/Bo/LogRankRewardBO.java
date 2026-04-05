package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogRankRewardBO extends BaseLogBo {

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

    public static final int FIELD_version =5;
    @DataBaseField(type = "int(11)", fieldname = "version", comment = "0-旧流程 1-新流程1")
    private int version;

    public static final int FIELD_is_mail =6;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_mail", comment = "是否邮件奖励")
    private boolean is_mail;

    public static final int FIELD_rank_instance =7;
    @DataBaseField(type = "bigint(20)", fieldname = "rank_instance", comment = "排行榜实例ID")
    private long rank_instance;

    public static final int FIELD_rank_id =8;
    @DataBaseField(type = "bigint(20)", fieldname = "rank_id", comment = "排行榜ID")
    private long rank_id;

    public static final int FIELD_rank_type =9;
    @DataBaseField(type = "int(11)", fieldname = "rank_type", comment = "排行榜类型")
    private int rank_type;

    public static final int FIELD_rank =10;
    @DataBaseField(type = "bigint(20)", fieldname = "rank", comment = "排行")
    private long rank;

    public static final int FIELD_is_leader =11;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_leader", comment = "是否盟主")
    private boolean is_leader;

    public LogRankRewardBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        cid = 0L;
        version = 0;
        is_mail = false;
        rank_instance = 0L;
        rank_id = 0L;
        rank_type = 0;
        rank = 0L;
        is_leader = false;
    }

    public LogRankRewardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        cid = rs.getLong(6);
        version = rs.getInt(7);
        is_mail = rs.getBoolean(8);
        rank_instance = rs.getLong(9);
        rank_id = rs.getLong(10);
        rank_type = rs.getInt(11);
        rank = rs.getLong(12);
        is_leader = rs.getBoolean(13);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogRankRewardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `cid`, `version`, `is_mail`, `rank_instance`, `rank_id`, `rank_type`, `rank`, `is_leader`";
    }

    @Override
    public String getTableName() {
        return "`log_rank_reward`";
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
        strBuf.append("'").append(version).append("', ");
        strBuf.append("'").append(is_mail ? 1 : 0).append("', ");
        strBuf.append("'").append(rank_instance).append("', ");
        strBuf.append("'").append(rank_id).append("', ");
        strBuf.append("'").append(rank_type).append("', ");
        strBuf.append("'").append(rank).append("', ");
        strBuf.append("'").append(is_leader ? 1 : 0).append("', ");
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

    // 0-旧流程 1-新流程1
    public int getVersion() { return this.version; }
    public void setVersion(BM _bm, int version) {
        if(version==this.version) 
            return;
        this.version = version; 
        markField(_bm, FIELD_version); 
    }
    public void saveVersion(BM _bm, int version) {
        if(version==this.version) 
            return;
        this.version = version;
        saveField(_bm, "version", version);
    }

    // 是否邮件奖励
    public boolean getIsMail() { return this.is_mail; }
    public void setIsMail(BM _bm, boolean is_mail) {
        if(is_mail==this.is_mail) 
            return;
        this.is_mail = is_mail; 
        markField(_bm, FIELD_is_mail); 
    }
    public void saveIsMail(BM _bm, boolean is_mail) {
        if(is_mail==this.is_mail) 
            return;
        this.is_mail = is_mail;
        saveField(_bm, "is_mail", is_mail ? 1 : 0);
    }

    // 排行榜实例ID
    public long getRankInstance() { return this.rank_instance; }
    public void setRankInstance(BM _bm, long rank_instance) {
        if(rank_instance==this.rank_instance) 
            return;
        this.rank_instance = rank_instance; 
        markField(_bm, FIELD_rank_instance); 
    }
    public void saveRankInstance(BM _bm, long rank_instance) {
        if(rank_instance==this.rank_instance) 
            return;
        this.rank_instance = rank_instance;
        saveField(_bm, "rank_instance", rank_instance);
    }

    // 排行榜ID
    public long getRankId() { return this.rank_id; }
    public void setRankId(BM _bm, long rank_id) {
        if(rank_id==this.rank_id) 
            return;
        this.rank_id = rank_id; 
        markField(_bm, FIELD_rank_id); 
    }
    public void saveRankId(BM _bm, long rank_id) {
        if(rank_id==this.rank_id) 
            return;
        this.rank_id = rank_id;
        saveField(_bm, "rank_id", rank_id);
    }

    // 排行榜类型
    public int getRankType() { return this.rank_type; }
    public void setRankType(BM _bm, int rank_type) {
        if(rank_type==this.rank_type) 
            return;
        this.rank_type = rank_type; 
        markField(_bm, FIELD_rank_type); 
    }
    public void saveRankType(BM _bm, int rank_type) {
        if(rank_type==this.rank_type) 
            return;
        this.rank_type = rank_type;
        saveField(_bm, "rank_type", rank_type);
    }

    // 排行
    public long getRank() { return this.rank; }
    public void setRank(BM _bm, long rank) {
        if(rank==this.rank) 
            return;
        this.rank = rank; 
        markField(_bm, FIELD_rank); 
    }
    public void saveRank(BM _bm, long rank) {
        if(rank==this.rank) 
            return;
        this.rank = rank;
        saveField(_bm, "rank", rank);
    }

    // 是否盟主
    public boolean getIsLeader() { return this.is_leader; }
    public void setIsLeader(BM _bm, boolean is_leader) {
        if(is_leader==this.is_leader) 
            return;
        this.is_leader = is_leader; 
        markField(_bm, FIELD_is_leader); 
    }
    public void saveIsLeader(BM _bm, boolean is_leader) {
        if(is_leader==this.is_leader) 
            return;
        this.is_leader = is_leader;
        saveField(_bm, "is_leader", is_leader ? 1 : 0);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `version` = '").append(version).append("',");
        sBuilder.append(" `is_mail` = '").append(is_mail ? 1 : 0).append("',");
        sBuilder.append(" `rank_instance` = '").append(rank_instance).append("',");
        sBuilder.append(" `rank_id` = '").append(rank_id).append("',");
        sBuilder.append(" `rank_type` = '").append(rank_type).append("',");
        sBuilder.append(" `rank` = '").append(rank).append("',");
        sBuilder.append(" `is_leader` = '").append(is_leader ? 1 : 0).append("',");
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
        if(isFieldMarked(FIELD_version)) sBuilder.append(" `version` = '").append(version).append("',");
        if(isFieldMarked(FIELD_is_mail)) sBuilder.append(" `is_mail` = '").append(is_mail ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_rank_instance)) sBuilder.append(" `rank_instance` = '").append(rank_instance).append("',");
        if(isFieldMarked(FIELD_rank_id)) sBuilder.append(" `rank_id` = '").append(rank_id).append("',");
        if(isFieldMarked(FIELD_rank_type)) sBuilder.append(" `rank_type` = '").append(rank_type).append("',");
        if(isFieldMarked(FIELD_rank)) sBuilder.append(" `rank` = '").append(rank).append("',");
        if(isFieldMarked(FIELD_is_leader)) sBuilder.append(" `is_leader` = '").append(is_leader ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_rank_reward` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`version` int(11) NOT NULL DEFAULT '0' COMMENT '0-旧流程 1-新流程1',"
                + "`is_mail` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否邮件奖励',"
                + "`rank_instance` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜实例ID',"
                + "`rank_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜ID',"
                + "`rank_type` int(11) NOT NULL DEFAULT '0' COMMENT '排行榜类型',"
                + "`rank` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行',"
                + "`is_leader` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否盟主',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='排行榜结算奖励日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//version
        _size+=1;//is_mail
        _size+=8;//rank_instance
        _size+=8;//rank_id
        _size+=4;//rank_type
        _size+=8;//rank
        _size+=1;//is_leader
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
        buff.putInt(version);
        buff.put((byte)(is_mail?1:0));
        buff.putLong(rank_instance);
        buff.putLong(rank_id);
        buff.putInt(rank_type);
        buff.putLong(rank);
        buff.put((byte)(is_leader?1:0));        
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
        version=buff.getInt();
        is_mail=(buff.get()==1);
        rank_instance=buff.getLong();
        rank_id=buff.getLong();
        rank_type=buff.getInt();
        rank=buff.getLong();
        is_leader=(buff.get()==1); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
