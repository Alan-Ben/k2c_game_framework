package USDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;
import NPCommon.DB.Annotation.RefBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

@RefBo(isIdAuto= true)
public class ActivityRankRewardInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_activity_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例ID")
    private long activity_instance_id;

    public static final int FIELD_rank_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "rank_id", comment = "排行榜配置ID")
    private long rank_id;

    public static final int FIELD_cid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "cid")
    private long cid;

    public static final int FIELD_groupId =3;
    @DataBaseField(type = "bigint(20)", fieldname = "groupId", comment = "团体id")
    private long groupId;

    public static final int FIELD_rank =4;
    @DataBaseField(type = "int(11)", fieldname = "rank", comment = "排名")
    private int rank;

    public static final int FIELD_had_draw =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_draw", comment = "是否已领取")
    private boolean had_draw;

    public static final int FIELD_score =6;
    @DataBaseField(type = "bigint(20)", fieldname = "score", comment = "分数")
    private long score;

    public ActivityRankRewardInfoBO() {
        id = 0;
        activity_instance_id = 0L;
        rank_id = 0L;
        cid = 0L;
        groupId = 0L;
        rank = 0;
        had_draw = false;
        score = 0L;
    }

    public ActivityRankRewardInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        activity_instance_id = rs.getLong(2);
        rank_id = rs.getLong(3);
        cid = rs.getLong(4);
        groupId = rs.getLong(5);
        rank = rs.getInt(6);
        had_draw = rs.getBoolean(7);
        score = rs.getLong(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ActivityRankRewardInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `activity_instance_id`, `rank_id`, `cid`, `groupId`, `rank`, `had_draw`, `score`";
    }

    @Override
    public String getTableName() {
        return "`activity_rank_reward_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(rank_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(groupId).append("', ");
        strBuf.append("'").append(rank).append("', ");
        strBuf.append("'").append(had_draw ? 1 : 0).append("', ");
        strBuf.append("'").append(score).append("', ");
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

    // 活动实例ID
    public long getActivityInstanceId() { return this.activity_instance_id; }
    public void setActivityInstanceId(BM _bm, long activity_instance_id) {
        if(activity_instance_id==this.activity_instance_id) 
            return;
        this.activity_instance_id = activity_instance_id; 
        markField(_bm, FIELD_activity_instance_id); 
    }
    public void saveActivityInstanceId(BM _bm, long activity_instance_id) {
        if(activity_instance_id==this.activity_instance_id) 
            return;
        this.activity_instance_id = activity_instance_id;
        saveField(_bm, "activity_instance_id", activity_instance_id);
    }

    // 排行榜配置ID
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

    // cid
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

    // 团体id
    public long getGroupId() { return this.groupId; }
    public void setGroupId(BM _bm, long groupId) {
        if(groupId==this.groupId) 
            return;
        this.groupId = groupId; 
        markField(_bm, FIELD_groupId); 
    }
    public void saveGroupId(BM _bm, long groupId) {
        if(groupId==this.groupId) 
            return;
        this.groupId = groupId;
        saveField(_bm, "groupId", groupId);
    }

    // 排名
    public int getRank() { return this.rank; }
    public void setRank(BM _bm, int rank) {
        if(rank==this.rank) 
            return;
        this.rank = rank; 
        markField(_bm, FIELD_rank); 
    }
    public void saveRank(BM _bm, int rank) {
        if(rank==this.rank) 
            return;
        this.rank = rank;
        saveField(_bm, "rank", rank);
    }

    // 是否已领取
    public boolean getHadDraw() { return this.had_draw; }
    public void setHadDraw(BM _bm, boolean had_draw) {
        if(had_draw==this.had_draw) 
            return;
        this.had_draw = had_draw; 
        markField(_bm, FIELD_had_draw); 
    }
    public void saveHadDraw(BM _bm, boolean had_draw) {
        if(had_draw==this.had_draw) 
            return;
        this.had_draw = had_draw;
        saveField(_bm, "had_draw", had_draw ? 1 : 0);
    }

    // 分数
    public long getScore() { return this.score; }
    public void setScore(BM _bm, long score) {
        if(score==this.score) 
            return;
        this.score = score; 
        markField(_bm, FIELD_score); 
    }
    public void saveScore(BM _bm, long score) {
        if(score==this.score) 
            return;
        this.score = score;
        saveField(_bm, "score", score);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `rank_id` = '").append(rank_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `groupId` = '").append(groupId).append("',");
        sBuilder.append(" `rank` = '").append(rank).append("',");
        sBuilder.append(" `had_draw` = '").append(had_draw ? 1 : 0).append("',");
        sBuilder.append(" `score` = '").append(score).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_rank_id)) sBuilder.append(" `rank_id` = '").append(rank_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_groupId)) sBuilder.append(" `groupId` = '").append(groupId).append("',");
        if(isFieldMarked(FIELD_rank)) sBuilder.append(" `rank` = '").append(rank).append("',");
        if(isFieldMarked(FIELD_had_draw)) sBuilder.append(" `had_draw` = '").append(had_draw ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_score)) sBuilder.append(" `score` = '").append(score).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `activity_rank_reward_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`rank_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜配置ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT 'cid',"
                + "`groupId` bigint(20) NOT NULL DEFAULT '0' COMMENT '团体id',"
                + "`rank` int(11) NOT NULL DEFAULT '0' COMMENT '排名',"
                + "`had_draw` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已领取',"
                + "`score` bigint(20) NOT NULL DEFAULT '0' COMMENT '分数',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动排行榜奖励领取记录数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//activity_instance_id
        _size+=8;//rank_id
        _size+=8;//cid
        _size+=8;//groupId
        _size+=4;//rank
        _size+=1;//had_draw
        _size+=8;//score
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(activity_instance_id);
        buff.putLong(rank_id);
        buff.putLong(cid);
        buff.putLong(groupId);
        buff.putInt(rank);
        buff.put((byte)(had_draw?1:0));
        buff.putLong(score);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        activity_instance_id=buff.getLong();
        rank_id=buff.getLong();
        cid=buff.getLong();
        groupId=buff.getLong();
        rank=buff.getInt();
        had_draw=(buff.get()==1);
        score=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
