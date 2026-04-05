package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogRankScoreChgBO extends BaseLogBo {

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

    public static final int FIELD_rank_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "rank_id", comment = "排行榜配表id")
    private long rank_id;

    public static final int FIELD_obj_id =5;
    @DataBaseField(type = "bigint(20)", fieldname = "obj_id", comment = "数据对象id")
    private long obj_id;

    public static final int FIELD_ori_score =6;
    @DataBaseField(type = "bigint(20)", fieldname = "ori_score", comment = "原分数")
    private long ori_score;

    public static final int FIELD_ori_rank =7;
    @DataBaseField(type = "bigint(20)", fieldname = "ori_rank", comment = "原排名")
    private long ori_rank;

    public static final int FIELD_new_score =8;
    @DataBaseField(type = "bigint(20)", fieldname = "new_score", comment = "新分数")
    private long new_score;

    public static final int FIELD_new_rank =9;
    @DataBaseField(type = "bigint(20)", fieldname = "new_rank", comment = "新排名")
    private long new_rank;

    public LogRankScoreChgBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        rank_id = 0L;
        obj_id = 0L;
        ori_score = 0L;
        ori_rank = 0L;
        new_score = 0L;
        new_rank = 0L;
    }

    public LogRankScoreChgBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        rank_id = rs.getLong(6);
        obj_id = rs.getLong(7);
        ori_score = rs.getLong(8);
        ori_rank = rs.getLong(9);
        new_score = rs.getLong(10);
        new_rank = rs.getLong(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogRankScoreChgBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `rank_id`, `obj_id`, `ori_score`, `ori_rank`, `new_score`, `new_rank`";
    }

    @Override
    public String getTableName() {
        return "`log_rank_score_chg`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(rank_id).append("', ");
        strBuf.append("'").append(obj_id).append("', ");
        strBuf.append("'").append(ori_score).append("', ");
        strBuf.append("'").append(ori_rank).append("', ");
        strBuf.append("'").append(new_score).append("', ");
        strBuf.append("'").append(new_rank).append("', ");
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

    // 排行榜配表id
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

    // 数据对象id
    public long getObjId() { return this.obj_id; }
    public void setObjId(BM _bm, long obj_id) {
        if(obj_id==this.obj_id) 
            return;
        this.obj_id = obj_id; 
        markField(_bm, FIELD_obj_id); 
    }
    public void saveObjId(BM _bm, long obj_id) {
        if(obj_id==this.obj_id) 
            return;
        this.obj_id = obj_id;
        saveField(_bm, "obj_id", obj_id);
    }

    // 原分数
    public long getOriScore() { return this.ori_score; }
    public void setOriScore(BM _bm, long ori_score) {
        if(ori_score==this.ori_score) 
            return;
        this.ori_score = ori_score; 
        markField(_bm, FIELD_ori_score); 
    }
    public void saveOriScore(BM _bm, long ori_score) {
        if(ori_score==this.ori_score) 
            return;
        this.ori_score = ori_score;
        saveField(_bm, "ori_score", ori_score);
    }

    // 原排名
    public long getOriRank() { return this.ori_rank; }
    public void setOriRank(BM _bm, long ori_rank) {
        if(ori_rank==this.ori_rank) 
            return;
        this.ori_rank = ori_rank; 
        markField(_bm, FIELD_ori_rank); 
    }
    public void saveOriRank(BM _bm, long ori_rank) {
        if(ori_rank==this.ori_rank) 
            return;
        this.ori_rank = ori_rank;
        saveField(_bm, "ori_rank", ori_rank);
    }

    // 新分数
    public long getNewScore() { return this.new_score; }
    public void setNewScore(BM _bm, long new_score) {
        if(new_score==this.new_score) 
            return;
        this.new_score = new_score; 
        markField(_bm, FIELD_new_score); 
    }
    public void saveNewScore(BM _bm, long new_score) {
        if(new_score==this.new_score) 
            return;
        this.new_score = new_score;
        saveField(_bm, "new_score", new_score);
    }

    // 新排名
    public long getNewRank() { return this.new_rank; }
    public void setNewRank(BM _bm, long new_rank) {
        if(new_rank==this.new_rank) 
            return;
        this.new_rank = new_rank; 
        markField(_bm, FIELD_new_rank); 
    }
    public void saveNewRank(BM _bm, long new_rank) {
        if(new_rank==this.new_rank) 
            return;
        this.new_rank = new_rank;
        saveField(_bm, "new_rank", new_rank);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `rank_id` = '").append(rank_id).append("',");
        sBuilder.append(" `obj_id` = '").append(obj_id).append("',");
        sBuilder.append(" `ori_score` = '").append(ori_score).append("',");
        sBuilder.append(" `ori_rank` = '").append(ori_rank).append("',");
        sBuilder.append(" `new_score` = '").append(new_score).append("',");
        sBuilder.append(" `new_rank` = '").append(new_rank).append("',");
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
        if(isFieldMarked(FIELD_rank_id)) sBuilder.append(" `rank_id` = '").append(rank_id).append("',");
        if(isFieldMarked(FIELD_obj_id)) sBuilder.append(" `obj_id` = '").append(obj_id).append("',");
        if(isFieldMarked(FIELD_ori_score)) sBuilder.append(" `ori_score` = '").append(ori_score).append("',");
        if(isFieldMarked(FIELD_ori_rank)) sBuilder.append(" `ori_rank` = '").append(ori_rank).append("',");
        if(isFieldMarked(FIELD_new_score)) sBuilder.append(" `new_score` = '").append(new_score).append("',");
        if(isFieldMarked(FIELD_new_rank)) sBuilder.append(" `new_rank` = '").append(new_rank).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_rank_score_chg` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`rank_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜配表id',"
                + "`obj_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '数据对象id',"
                + "`ori_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '原分数',"
                + "`ori_rank` bigint(20) NOT NULL DEFAULT '0' COMMENT '原排名',"
                + "`new_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '新分数',"
                + "`new_rank` bigint(20) NOT NULL DEFAULT '0' COMMENT '新排名',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='排行榜分数变更日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//rank_id
        _size+=8;//obj_id
        _size+=8;//ori_score
        _size+=8;//ori_rank
        _size+=8;//new_score
        _size+=8;//new_rank
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
        buff.putLong(rank_id);
        buff.putLong(obj_id);
        buff.putLong(ori_score);
        buff.putLong(ori_rank);
        buff.putLong(new_score);
        buff.putLong(new_rank);        
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
        rank_id=buff.getLong();
        obj_id=buff.getLong();
        ori_score=buff.getLong();
        ori_rank=buff.getLong();
        new_score=buff.getLong();
        new_rank=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
