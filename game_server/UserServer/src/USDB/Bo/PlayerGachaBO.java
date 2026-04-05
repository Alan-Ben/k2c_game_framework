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
public class PlayerGachaBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家账号ID")
    private long cid;

    public static final int FIELD_pool_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "pool_id", comment = "奖池id")
    private long pool_id;

    public static final int FIELD_weight_info =2;
    @DataBaseField(type = "varchar(1024)", fieldname = "weight_info", comment = "权重信息")
    private String weight_info;

    public static final int FIELD_guarantee_info =3;
    @DataBaseField(type = "varchar(1024)", fieldname = "guarantee_info", comment = "保底信息")
    private String guarantee_info;

    public static final int FIELD_step =4;
    @DataBaseField(type = "int(11)", fieldname = "step", comment = "阶段")
    private int step;

    public static final int FIELD_cur_step_roll_num =5;
    @DataBaseField(type = "int(11)", fieldname = "cur_step_roll_num", comment = "当前阶段抽取次数，用于升阶")
    private int cur_step_roll_num;

    public static final int FIELD_cumulative_reward_point =6;
    @DataBaseField(type = "int(11)", fieldname = "cumulative_reward_point", comment = "累计奖励点数")
    private int cumulative_reward_point;

    public PlayerGachaBO() {
        id = 0;
        cid = 0L;
        pool_id = 0L;
        weight_info = "";
        guarantee_info = "";
        step = 0;
        cur_step_roll_num = 0;
        cumulative_reward_point = 0;
    }

    public PlayerGachaBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        pool_id = rs.getLong(3);
        weight_info = rs.getString(4);
        guarantee_info = rs.getString(5);
        step = rs.getInt(6);
        cur_step_roll_num = rs.getInt(7);
        cumulative_reward_point = rs.getInt(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerGachaBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `pool_id`, `weight_info`, `guarantee_info`, `step`, `cur_step_roll_num`, `cumulative_reward_point`";
    }

    @Override
    public String getTableName() {
        return "`player_gacha`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(pool_id).append("', ");
        strBuf.append("'").append(weight_info == null ? null : weight_info.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(guarantee_info == null ? null : guarantee_info.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(step).append("', ");
        strBuf.append("'").append(cur_step_roll_num).append("', ");
        strBuf.append("'").append(cumulative_reward_point).append("', ");
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

    // 玩家账号ID
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

    // 奖池id
    public long getPoolId() { return this.pool_id; }
    public void setPoolId(BM _bm, long pool_id) {
        if(pool_id==this.pool_id) 
            return;
        this.pool_id = pool_id; 
        markField(_bm, FIELD_pool_id); 
    }
    public void savePoolId(BM _bm, long pool_id) {
        if(pool_id==this.pool_id) 
            return;
        this.pool_id = pool_id;
        saveField(_bm, "pool_id", pool_id);
    }

    // 权重信息
    public String getWeightInfo() { return this.weight_info; }
    public void setWeightInfo(BM _bm, String weight_info) {
        if(weight_info.equals(this.weight_info)) 
            return;
        this.weight_info = weight_info; 
        markField(_bm, FIELD_weight_info); 
    }
    public void saveWeightInfo(BM _bm, String weight_info) {
        if(weight_info.equals(this.weight_info)) 
            return;
        this.weight_info = weight_info;
        saveField(_bm, "weight_info", weight_info);
    }

    // 保底信息
    public String getGuaranteeInfo() { return this.guarantee_info; }
    public void setGuaranteeInfo(BM _bm, String guarantee_info) {
        if(guarantee_info.equals(this.guarantee_info)) 
            return;
        this.guarantee_info = guarantee_info; 
        markField(_bm, FIELD_guarantee_info); 
    }
    public void saveGuaranteeInfo(BM _bm, String guarantee_info) {
        if(guarantee_info.equals(this.guarantee_info)) 
            return;
        this.guarantee_info = guarantee_info;
        saveField(_bm, "guarantee_info", guarantee_info);
    }

    // 阶段
    public int getStep() { return this.step; }
    public void setStep(BM _bm, int step) {
        if(step==this.step) 
            return;
        this.step = step; 
        markField(_bm, FIELD_step); 
    }
    public void saveStep(BM _bm, int step) {
        if(step==this.step) 
            return;
        this.step = step;
        saveField(_bm, "step", step);
    }

    // 当前阶段抽取次数，用于升阶
    public int getCurStepRollNum() { return this.cur_step_roll_num; }
    public void setCurStepRollNum(BM _bm, int cur_step_roll_num) {
        if(cur_step_roll_num==this.cur_step_roll_num) 
            return;
        this.cur_step_roll_num = cur_step_roll_num; 
        markField(_bm, FIELD_cur_step_roll_num); 
    }
    public void saveCurStepRollNum(BM _bm, int cur_step_roll_num) {
        if(cur_step_roll_num==this.cur_step_roll_num) 
            return;
        this.cur_step_roll_num = cur_step_roll_num;
        saveField(_bm, "cur_step_roll_num", cur_step_roll_num);
    }

    // 累计奖励点数
    public int getCumulativeRewardPoint() { return this.cumulative_reward_point; }
    public void setCumulativeRewardPoint(BM _bm, int cumulative_reward_point) {
        if(cumulative_reward_point==this.cumulative_reward_point) 
            return;
        this.cumulative_reward_point = cumulative_reward_point; 
        markField(_bm, FIELD_cumulative_reward_point); 
    }
    public void saveCumulativeRewardPoint(BM _bm, int cumulative_reward_point) {
        if(cumulative_reward_point==this.cumulative_reward_point) 
            return;
        this.cumulative_reward_point = cumulative_reward_point;
        saveField(_bm, "cumulative_reward_point", cumulative_reward_point);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `pool_id` = '").append(pool_id).append("',");
        sBuilder.append(" `weight_info` = '").append(weight_info == null ? null : weight_info.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `guarantee_info` = '").append(guarantee_info == null ? null : guarantee_info.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `step` = '").append(step).append("',");
        sBuilder.append(" `cur_step_roll_num` = '").append(cur_step_roll_num).append("',");
        sBuilder.append(" `cumulative_reward_point` = '").append(cumulative_reward_point).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_pool_id)) sBuilder.append(" `pool_id` = '").append(pool_id).append("',");
        if(isFieldMarked(FIELD_weight_info)) sBuilder.append(" `weight_info` = '").append(weight_info == null ? null : weight_info.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_guarantee_info)) sBuilder.append(" `guarantee_info` = '").append(guarantee_info == null ? null : guarantee_info.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_step)) sBuilder.append(" `step` = '").append(step).append("',");
        if(isFieldMarked(FIELD_cur_step_roll_num)) sBuilder.append(" `cur_step_roll_num` = '").append(cur_step_roll_num).append("',");
        if(isFieldMarked(FIELD_cumulative_reward_point)) sBuilder.append(" `cumulative_reward_point` = '").append(cumulative_reward_point).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_gacha` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家账号ID',"
                + "`pool_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '奖池id',"
                + "`weight_info` varchar(1024) NOT NULL DEFAULT '' COMMENT '权重信息',"
                + "`guarantee_info` varchar(1024) NOT NULL DEFAULT '' COMMENT '保底信息',"
                + "`step` int(11) NOT NULL DEFAULT '0' COMMENT '阶段',"
                + "`cur_step_roll_num` int(11) NOT NULL DEFAULT '0' COMMENT '当前阶段抽取次数，用于升阶',"
                + "`cumulative_reward_point` int(11) NOT NULL DEFAULT '0' COMMENT '累计奖励点数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家抽卡权重信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//pool_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(weight_info);//weight_info
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guarantee_info);//guarantee_info
        _size+=4;//step
        _size+=4;//cur_step_roll_num
        _size+=4;//cumulative_reward_point
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(pool_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, weight_info);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, guarantee_info);
        buff.putInt(step);
        buff.putInt(cur_step_roll_num);
        buff.putInt(cumulative_reward_point);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        pool_id=buff.getLong();
        weight_info=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        guarantee_info=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        step=buff.getInt();
        cur_step_roll_num=buff.getInt();
        cumulative_reward_point=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
