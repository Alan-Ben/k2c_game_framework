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
public class ActivityStepRewardBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instance_id", comment = "活动实例ID")
    private long instance_id;

    public static final int FIELD_step_reward_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "step_reward_id", comment = "阶段奖励配置ID")
    private long step_reward_id;

    public static final int FIELD_step_reward_instance_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "step_reward_instance_id", comment = "阶段奖励实例ID")
    private long step_reward_instance_id;

    public ActivityStepRewardBO() {
        id = 0;
        instance_id = 0L;
        step_reward_id = 0L;
        step_reward_instance_id = 0L;
    }

    public ActivityStepRewardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instance_id = rs.getLong(2);
        step_reward_id = rs.getLong(3);
        step_reward_instance_id = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ActivityStepRewardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instance_id`, `step_reward_id`, `step_reward_instance_id`";
    }

    @Override
    public String getTableName() {
        return "`activity_step_reward`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instance_id).append("', ");
        strBuf.append("'").append(step_reward_id).append("', ");
        strBuf.append("'").append(step_reward_instance_id).append("', ");
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

    // 阶段奖励配置ID
    public long getStepRewardId() { return this.step_reward_id; }
    public void setStepRewardId(BM _bm, long step_reward_id) {
        if(step_reward_id==this.step_reward_id) 
            return;
        this.step_reward_id = step_reward_id; 
        markField(_bm, FIELD_step_reward_id); 
    }
    public void saveStepRewardId(BM _bm, long step_reward_id) {
        if(step_reward_id==this.step_reward_id) 
            return;
        this.step_reward_id = step_reward_id;
        saveField(_bm, "step_reward_id", step_reward_id);
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        sBuilder.append(" `step_reward_id` = '").append(step_reward_id).append("',");
        sBuilder.append(" `step_reward_instance_id` = '").append(step_reward_instance_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instance_id)) sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        if(isFieldMarked(FIELD_step_reward_id)) sBuilder.append(" `step_reward_id` = '").append(step_reward_id).append("',");
        if(isFieldMarked(FIELD_step_reward_instance_id)) sBuilder.append(" `step_reward_instance_id` = '").append(step_reward_instance_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `activity_step_reward` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`step_reward_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '阶段奖励配置ID',"
                + "`step_reward_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '阶段奖励实例ID',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动阶段奖励数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//instance_id
        _size+=8;//step_reward_id
        _size+=8;//step_reward_instance_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instance_id);
        buff.putLong(step_reward_id);
        buff.putLong(step_reward_instance_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instance_id=buff.getLong();
        step_reward_id=buff.getLong();
        step_reward_instance_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
