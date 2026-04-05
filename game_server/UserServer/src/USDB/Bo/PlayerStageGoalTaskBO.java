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
public class PlayerStageGoalTaskBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_step =1;
    @DataBaseField(type = "bigint(20)", fieldname = "step", comment = "阶段ID")
    private long step;

    public static final int FIELD_task_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "task_id", comment = "任务ID")
    private long task_id;

    public static final int FIELD_counter =3;
    @DataBaseField(type = "bigint(20)", fieldname = "counter", comment = "当前任务计数")
    private long counter;

    public static final int FIELD_reward_drawed =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "reward_drawed", comment = "任务奖励是否已领取")
    private boolean reward_drawed;

    public PlayerStageGoalTaskBO() {
        id = 0;
        cid = 0L;
        step = 0L;
        task_id = 0L;
        counter = 0L;
        reward_drawed = false;
    }

    public PlayerStageGoalTaskBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        step = rs.getLong(3);
        task_id = rs.getLong(4);
        counter = rs.getLong(5);
        reward_drawed = rs.getBoolean(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerStageGoalTaskBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `step`, `task_id`, `counter`, `reward_drawed`";
    }

    @Override
    public String getTableName() {
        return "`player_stage_goal_task`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(step).append("', ");
        strBuf.append("'").append(task_id).append("', ");
        strBuf.append("'").append(counter).append("', ");
        strBuf.append("'").append(reward_drawed ? 1 : 0).append("', ");
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

    // 阶段ID
    public long getStep() { return this.step; }
    public void setStep(BM _bm, long step) {
        if(step==this.step) 
            return;
        this.step = step; 
        markField(_bm, FIELD_step); 
    }
    public void saveStep(BM _bm, long step) {
        if(step==this.step) 
            return;
        this.step = step;
        saveField(_bm, "step", step);
    }

    // 任务ID
    public long getTaskId() { return this.task_id; }
    public void setTaskId(BM _bm, long task_id) {
        if(task_id==this.task_id) 
            return;
        this.task_id = task_id; 
        markField(_bm, FIELD_task_id); 
    }
    public void saveTaskId(BM _bm, long task_id) {
        if(task_id==this.task_id) 
            return;
        this.task_id = task_id;
        saveField(_bm, "task_id", task_id);
    }

    // 当前任务计数
    public long getCounter() { return this.counter; }
    public void setCounter(BM _bm, long counter) {
        if(counter==this.counter) 
            return;
        this.counter = counter; 
        markField(_bm, FIELD_counter); 
    }
    public void saveCounter(BM _bm, long counter) {
        if(counter==this.counter) 
            return;
        this.counter = counter;
        saveField(_bm, "counter", counter);
    }

    // 任务奖励是否已领取
    public boolean getRewardDrawed() { return this.reward_drawed; }
    public void setRewardDrawed(BM _bm, boolean reward_drawed) {
        if(reward_drawed==this.reward_drawed) 
            return;
        this.reward_drawed = reward_drawed; 
        markField(_bm, FIELD_reward_drawed); 
    }
    public void saveRewardDrawed(BM _bm, boolean reward_drawed) {
        if(reward_drawed==this.reward_drawed) 
            return;
        this.reward_drawed = reward_drawed;
        saveField(_bm, "reward_drawed", reward_drawed ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `step` = '").append(step).append("',");
        sBuilder.append(" `task_id` = '").append(task_id).append("',");
        sBuilder.append(" `counter` = '").append(counter).append("',");
        sBuilder.append(" `reward_drawed` = '").append(reward_drawed ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_step)) sBuilder.append(" `step` = '").append(step).append("',");
        if(isFieldMarked(FIELD_task_id)) sBuilder.append(" `task_id` = '").append(task_id).append("',");
        if(isFieldMarked(FIELD_counter)) sBuilder.append(" `counter` = '").append(counter).append("',");
        if(isFieldMarked(FIELD_reward_drawed)) sBuilder.append(" `reward_drawed` = '").append(reward_drawed ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_stage_goal_task` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`step` bigint(20) NOT NULL DEFAULT '0' COMMENT '阶段ID',"
                + "`task_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务ID',"
                + "`counter` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前任务计数',"
                + "`reward_drawed` tinyint(1) NOT NULL DEFAULT '0' COMMENT '任务奖励是否已领取',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家阶段目标任务数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=8;//step
        _size+=8;//task_id
        _size+=8;//counter
        _size+=1;//reward_drawed
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(step);
        buff.putLong(task_id);
        buff.putLong(counter);
        buff.put((byte)(reward_drawed?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        step=buff.getLong();
        task_id=buff.getLong();
        counter=buff.getLong();
        reward_drawed=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
