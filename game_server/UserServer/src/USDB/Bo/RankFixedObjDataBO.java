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
public class RankFixedObjDataBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_rank_fixed_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "rank_fixed_id", comment = "常驻排行榜id")
    private long rank_fixed_id;

    public static final int FIELD_key =1;
    @DataBaseField(type = "bigint(20)", fieldname = "key", comment = "玩家CID")
    private long key;

    public static final int FIELD_like_score =2;
    @DataBaseField(type = "bigint(20)", fieldname = "like_score", comment = "点赞积分")
    private long like_score;

    public static final int FIELD_cross_like_score =3;
    @DataBaseField(type = "bigint(20)", fieldname = "cross_like_score", comment = "跨服点赞积分")
    private long cross_like_score;

    public RankFixedObjDataBO() {
        id = 0;
        rank_fixed_id = 0L;
        key = 0L;
        like_score = 0L;
        cross_like_score = 0L;
    }

    public RankFixedObjDataBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        rank_fixed_id = rs.getLong(2);
        key = rs.getLong(3);
        like_score = rs.getLong(4);
        cross_like_score = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new RankFixedObjDataBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `rank_fixed_id`, `key`, `like_score`, `cross_like_score`";
    }

    @Override
    public String getTableName() {
        return "`rank_fixed_obj_data`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(rank_fixed_id).append("', ");
        strBuf.append("'").append(key).append("', ");
        strBuf.append("'").append(like_score).append("', ");
        strBuf.append("'").append(cross_like_score).append("', ");
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

    // 常驻排行榜id
    public long getRankFixedId() { return this.rank_fixed_id; }
    public void setRankFixedId(BM _bm, long rank_fixed_id) {
        if(rank_fixed_id==this.rank_fixed_id) 
            return;
        this.rank_fixed_id = rank_fixed_id; 
        markField(_bm, FIELD_rank_fixed_id); 
    }
    public void saveRankFixedId(BM _bm, long rank_fixed_id) {
        if(rank_fixed_id==this.rank_fixed_id) 
            return;
        this.rank_fixed_id = rank_fixed_id;
        saveField(_bm, "rank_fixed_id", rank_fixed_id);
    }

    // 玩家CID
    public long getKey() { return this.key; }
    public void setKey(BM _bm, long key) {
        if(key==this.key) 
            return;
        this.key = key; 
        markField(_bm, FIELD_key); 
    }
    public void saveKey(BM _bm, long key) {
        if(key==this.key) 
            return;
        this.key = key;
        saveField(_bm, "key", key);
    }

    // 点赞积分
    public long getLikeScore() { return this.like_score; }
    public void setLikeScore(BM _bm, long like_score) {
        if(like_score==this.like_score) 
            return;
        this.like_score = like_score; 
        markField(_bm, FIELD_like_score); 
    }
    public void saveLikeScore(BM _bm, long like_score) {
        if(like_score==this.like_score) 
            return;
        this.like_score = like_score;
        saveField(_bm, "like_score", like_score);
    }

    // 跨服点赞积分
    public long getCrossLikeScore() { return this.cross_like_score; }
    public void setCrossLikeScore(BM _bm, long cross_like_score) {
        if(cross_like_score==this.cross_like_score) 
            return;
        this.cross_like_score = cross_like_score; 
        markField(_bm, FIELD_cross_like_score); 
    }
    public void saveCrossLikeScore(BM _bm, long cross_like_score) {
        if(cross_like_score==this.cross_like_score) 
            return;
        this.cross_like_score = cross_like_score;
        saveField(_bm, "cross_like_score", cross_like_score);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `rank_fixed_id` = '").append(rank_fixed_id).append("',");
        sBuilder.append(" `key` = '").append(key).append("',");
        sBuilder.append(" `like_score` = '").append(like_score).append("',");
        sBuilder.append(" `cross_like_score` = '").append(cross_like_score).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_rank_fixed_id)) sBuilder.append(" `rank_fixed_id` = '").append(rank_fixed_id).append("',");
        if(isFieldMarked(FIELD_key)) sBuilder.append(" `key` = '").append(key).append("',");
        if(isFieldMarked(FIELD_like_score)) sBuilder.append(" `like_score` = '").append(like_score).append("',");
        if(isFieldMarked(FIELD_cross_like_score)) sBuilder.append(" `cross_like_score` = '").append(cross_like_score).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `rank_fixed_obj_data` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`rank_fixed_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '常驻排行榜id',"
                + "`key` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`like_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '点赞积分',"
                + "`cross_like_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服点赞积分',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='排行榜对象数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//rank_fixed_id
        _size+=8;//key
        _size+=8;//like_score
        _size+=8;//cross_like_score
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(rank_fixed_id);
        buff.putLong(key);
        buff.putLong(like_score);
        buff.putLong(cross_like_score);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        rank_fixed_id=buff.getLong();
        key=buff.getLong();
        like_score=buff.getLong();
        cross_like_score=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
