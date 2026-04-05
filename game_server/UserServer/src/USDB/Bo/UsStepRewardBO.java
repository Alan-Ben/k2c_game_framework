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
public class UsStepRewardBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_step_reward_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "step_reward_id", comment = "阶段奖励配置ID")
    private long step_reward_id;

    public UsStepRewardBO() {
        id = 0;
        step_reward_id = 0L;
    }

    public UsStepRewardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        step_reward_id = rs.getLong(2);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsStepRewardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `step_reward_id`";
    }

    @Override
    public String getTableName() {
        return "`us_step_reward`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(step_reward_id).append("', ");
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `step_reward_id` = '").append(step_reward_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_step_reward_id)) sBuilder.append(" `step_reward_id` = '").append(step_reward_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_step_reward` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`step_reward_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '阶段奖励配置ID',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='阶段奖励数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//step_reward_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(step_reward_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        step_reward_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
