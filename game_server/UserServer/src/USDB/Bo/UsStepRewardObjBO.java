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
public class UsStepRewardObjBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_step_reward_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "step_reward_instance_id", comment = "阶段奖励实例ID")
    private long step_reward_instance_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家cid")
    private long cid;

    public static final int FIELD_score =2;
    @DataBaseField(type = "bigint(20)", fieldname = "score", comment = "积分")
    private long score;

    public UsStepRewardObjBO() {
        id = 0;
        step_reward_instance_id = 0L;
        cid = 0L;
        score = 0L;
    }

    public UsStepRewardObjBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        step_reward_instance_id = rs.getLong(2);
        cid = rs.getLong(3);
        score = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsStepRewardObjBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `step_reward_instance_id`, `cid`, `score`";
    }

    @Override
    public String getTableName() {
        return "`us_step_reward_obj`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(step_reward_instance_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
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

    // 阶段奖励实例ID
    public long getStepRewardInstanceId() { return this.step_reward_instance_id; }
    public void setStepRewardInstanceId(BM _bm, long step_reward_instance_id) {
        if(step_reward_instance_id==this.step_reward_instance_id) 
            return;
        this.step_reward_instance_id = step_reward_instance_id; 
        markField(_bm, FIELD_step_reward_instance_id); 
    }
    public void saveStepRewardInstanceId(BM _bm, long step_reward_instance_id) {
        if(step_reward_instance_id==this.step_reward_instance_id) 
            return;
        this.step_reward_instance_id = step_reward_instance_id;
        saveField(_bm, "step_reward_instance_id", step_reward_instance_id);
    }

    // 玩家cid
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

    // 积分
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
        sBuilder.append(" `step_reward_instance_id` = '").append(step_reward_instance_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `score` = '").append(score).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_step_reward_instance_id)) sBuilder.append(" `step_reward_instance_id` = '").append(step_reward_instance_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_score)) sBuilder.append(" `score` = '").append(score).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_step_reward_obj` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`step_reward_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '阶段奖励实例ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家cid',"
                + "`score` bigint(20) NOT NULL DEFAULT '0' COMMENT '积分',"
                + "KEY `step_reward_instance_id` (`step_reward_instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='阶段奖励子数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//step_reward_instance_id
        _size+=8;//cid
        _size+=8;//score
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(step_reward_instance_id);
        buff.putLong(cid);
        buff.putLong(score);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        step_reward_instance_id=buff.getLong();
        cid=buff.getLong();
        score=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
