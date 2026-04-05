package CRSDB.Bo;
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
public class NpCrossRankSubObjBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instanceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "跨服活动实例ID")
    private long instanceId;

    public static final int FIELD_rankId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "rankId", comment = "排行榜配置ID")
    private long rankId;

    public static final int FIELD_objId =2;
    @DataBaseField(type = "bigint(20)", fieldname = "objId", comment = "排行榜对象ID")
    private long objId;

    public static final int FIELD_subObjId =3;
    @DataBaseField(type = "bigint(20)", fieldname = "subObjId", comment = "排行榜对象的子对象ID")
    private long subObjId;

    public static final int FIELD_score_source_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "score_source_id", comment = "排行榜分数来源id")
    private long score_source_id;

    public static final int FIELD_score =5;
    @DataBaseField(type = "bigint(20)", fieldname = "score", comment = "排行榜分数")
    private long score;

    public static final int FIELD_updatedMs =6;
    @DataBaseField(type = "bigint(20)", fieldname = "updatedMs", comment = "排行榜变更时间戳")
    private long updatedMs;

    public NpCrossRankSubObjBO() {
        id = 0;
        instanceId = 0L;
        rankId = 0L;
        objId = 0L;
        subObjId = 0L;
        score_source_id = 0L;
        score = 0L;
        updatedMs = 0L;
    }

    public NpCrossRankSubObjBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instanceId = rs.getLong(2);
        rankId = rs.getLong(3);
        objId = rs.getLong(4);
        subObjId = rs.getLong(5);
        score_source_id = rs.getLong(6);
        score = rs.getLong(7);
        updatedMs = rs.getLong(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpCrossRankSubObjBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instanceId`, `rankId`, `objId`, `subObjId`, `score_source_id`, `score`, `updatedMs`";
    }

    @Override
    public String getTableName() {
        return "`np_cross_rank_sub_obj`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(rankId).append("', ");
        strBuf.append("'").append(objId).append("', ");
        strBuf.append("'").append(subObjId).append("', ");
        strBuf.append("'").append(score_source_id).append("', ");
        strBuf.append("'").append(score).append("', ");
        strBuf.append("'").append(updatedMs).append("', ");
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

    // 跨服活动实例ID
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

    // 排行榜配置ID
    public long getRankId() { return this.rankId; }
    public void setRankId(BM _bm, long rankId) {
        if(rankId==this.rankId) 
            return;
        this.rankId = rankId; 
        markField(_bm, FIELD_rankId); 
    }
    public void saveRankId(BM _bm, long rankId) {
        if(rankId==this.rankId) 
            return;
        this.rankId = rankId;
        saveField(_bm, "rankId", rankId);
    }

    // 排行榜对象ID
    public long getObjId() { return this.objId; }
    public void setObjId(BM _bm, long objId) {
        if(objId==this.objId) 
            return;
        this.objId = objId; 
        markField(_bm, FIELD_objId); 
    }
    public void saveObjId(BM _bm, long objId) {
        if(objId==this.objId) 
            return;
        this.objId = objId;
        saveField(_bm, "objId", objId);
    }

    // 排行榜对象的子对象ID
    public long getSubObjId() { return this.subObjId; }
    public void setSubObjId(BM _bm, long subObjId) {
        if(subObjId==this.subObjId) 
            return;
        this.subObjId = subObjId; 
        markField(_bm, FIELD_subObjId); 
    }
    public void saveSubObjId(BM _bm, long subObjId) {
        if(subObjId==this.subObjId) 
            return;
        this.subObjId = subObjId;
        saveField(_bm, "subObjId", subObjId);
    }

    // 排行榜分数来源id
    public long getScoreSourceId() { return this.score_source_id; }
    public void setScoreSourceId(BM _bm, long score_source_id) {
        if(score_source_id==this.score_source_id) 
            return;
        this.score_source_id = score_source_id; 
        markField(_bm, FIELD_score_source_id); 
    }
    public void saveScoreSourceId(BM _bm, long score_source_id) {
        if(score_source_id==this.score_source_id) 
            return;
        this.score_source_id = score_source_id;
        saveField(_bm, "score_source_id", score_source_id);
    }

    // 排行榜分数
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

    // 排行榜变更时间戳
    public long getUpdatedMs() { return this.updatedMs; }
    public void setUpdatedMs(BM _bm, long updatedMs) {
        if(updatedMs==this.updatedMs) 
            return;
        this.updatedMs = updatedMs; 
        markField(_bm, FIELD_updatedMs); 
    }
    public void saveUpdatedMs(BM _bm, long updatedMs) {
        if(updatedMs==this.updatedMs) 
            return;
        this.updatedMs = updatedMs;
        saveField(_bm, "updatedMs", updatedMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `rankId` = '").append(rankId).append("',");
        sBuilder.append(" `objId` = '").append(objId).append("',");
        sBuilder.append(" `subObjId` = '").append(subObjId).append("',");
        sBuilder.append(" `score_source_id` = '").append(score_source_id).append("',");
        sBuilder.append(" `score` = '").append(score).append("',");
        sBuilder.append(" `updatedMs` = '").append(updatedMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_rankId)) sBuilder.append(" `rankId` = '").append(rankId).append("',");
        if(isFieldMarked(FIELD_objId)) sBuilder.append(" `objId` = '").append(objId).append("',");
        if(isFieldMarked(FIELD_subObjId)) sBuilder.append(" `subObjId` = '").append(subObjId).append("',");
        if(isFieldMarked(FIELD_score_source_id)) sBuilder.append(" `score_source_id` = '").append(score_source_id).append("',");
        if(isFieldMarked(FIELD_score)) sBuilder.append(" `score` = '").append(score).append("',");
        if(isFieldMarked(FIELD_updatedMs)) sBuilder.append(" `updatedMs` = '").append(updatedMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_cross_rank_sub_obj` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服活动实例ID',"
                + "`rankId` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜配置ID',"
                + "`objId` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜对象ID',"
                + "`subObjId` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜对象的子对象ID',"
                + "`score_source_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜分数来源id',"
                + "`score` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜分数',"
                + "`updatedMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜变更时间戳',"
                + "KEY `instanceId` (`instanceId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='存放本服的排行榜子数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.crossrank_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//instanceId
        _size+=8;//rankId
        _size+=8;//objId
        _size+=8;//subObjId
        _size+=8;//score_source_id
        _size+=8;//score
        _size+=8;//updatedMs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instanceId);
        buff.putLong(rankId);
        buff.putLong(objId);
        buff.putLong(subObjId);
        buff.putLong(score_source_id);
        buff.putLong(score);
        buff.putLong(updatedMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instanceId=buff.getLong();
        rankId=buff.getLong();
        objId=buff.getLong();
        subObjId=buff.getLong();
        score_source_id=buff.getLong();
        score=buff.getLong();
        updatedMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
