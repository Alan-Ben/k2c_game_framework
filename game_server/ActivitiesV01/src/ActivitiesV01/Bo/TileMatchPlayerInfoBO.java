package ActivitiesV01.Bo;
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
public class TileMatchPlayerInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_activity_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例id")
    private long activity_instance_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家ID")
    private long cid;

    public static final int FIELD_total_score =2;
    @DataBaseField(type = "bigint(20)", fieldname = "total_score", comment = "总分")
    private long total_score;

    public static final int FIELD_step_reward_step =3;
    @DataBaseField(type = "int(11)", fieldname = "step_reward_step", comment = "阶段奖励阶段")
    private int step_reward_step;

    public static final int FIELD_step_reward_score =4;
    @DataBaseField(type = "bigint(20)", fieldname = "step_reward_score", comment = "阶段奖励分数")
    private long step_reward_score;

    public static final int FIELD_can_draw_step_reward_list =5;
    @DataBaseField(type = "varchar(1024)", fieldname = "can_draw_step_reward_list", comment = "可领取的阶段奖励列表")
    private String can_draw_step_reward_list;

    public TileMatchPlayerInfoBO() {
        id = 0;
        activity_instance_id = 0L;
        cid = 0L;
        total_score = 0L;
        step_reward_step = 0;
        step_reward_score = 0L;
        can_draw_step_reward_list = "";
    }

    public TileMatchPlayerInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        activity_instance_id = rs.getLong(2);
        cid = rs.getLong(3);
        total_score = rs.getLong(4);
        step_reward_step = rs.getInt(5);
        step_reward_score = rs.getLong(6);
        can_draw_step_reward_list = rs.getString(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new TileMatchPlayerInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `activity_instance_id`, `cid`, `total_score`, `step_reward_step`, `step_reward_score`, `can_draw_step_reward_list`";
    }

    @Override
    public String getTableName() {
        return "`tile_match_player_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(total_score).append("', ");
        strBuf.append("'").append(step_reward_step).append("', ");
        strBuf.append("'").append(step_reward_score).append("', ");
        strBuf.append("'").append(can_draw_step_reward_list == null ? null : can_draw_step_reward_list.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 活动实例id
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

    // 玩家ID
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

    // 总分
    public long getTotalScore() { return this.total_score; }
    public void setTotalScore(BM _bm, long total_score) {
        if(total_score==this.total_score) 
            return;
        this.total_score = total_score; 
        markField(_bm, FIELD_total_score); 
    }
    public void saveTotalScore(BM _bm, long total_score) {
        if(total_score==this.total_score) 
            return;
        this.total_score = total_score;
        saveField(_bm, "total_score", total_score);
    }

    // 阶段奖励阶段
    public int getStepRewardStep() { return this.step_reward_step; }
    public void setStepRewardStep(BM _bm, int step_reward_step) {
        if(step_reward_step==this.step_reward_step) 
            return;
        this.step_reward_step = step_reward_step; 
        markField(_bm, FIELD_step_reward_step); 
    }
    public void saveStepRewardStep(BM _bm, int step_reward_step) {
        if(step_reward_step==this.step_reward_step) 
            return;
        this.step_reward_step = step_reward_step;
        saveField(_bm, "step_reward_step", step_reward_step);
    }

    // 阶段奖励分数
    public long getStepRewardScore() { return this.step_reward_score; }
    public void setStepRewardScore(BM _bm, long step_reward_score) {
        if(step_reward_score==this.step_reward_score) 
            return;
        this.step_reward_score = step_reward_score; 
        markField(_bm, FIELD_step_reward_score); 
    }
    public void saveStepRewardScore(BM _bm, long step_reward_score) {
        if(step_reward_score==this.step_reward_score) 
            return;
        this.step_reward_score = step_reward_score;
        saveField(_bm, "step_reward_score", step_reward_score);
    }

    // 可领取的阶段奖励列表
    public String getCanDrawStepRewardList() { return this.can_draw_step_reward_list; }
    public void setCanDrawStepRewardList(BM _bm, String can_draw_step_reward_list) {
        if(can_draw_step_reward_list.equals(this.can_draw_step_reward_list)) 
            return;
        this.can_draw_step_reward_list = can_draw_step_reward_list; 
        markField(_bm, FIELD_can_draw_step_reward_list); 
    }
    public void saveCanDrawStepRewardList(BM _bm, String can_draw_step_reward_list) {
        if(can_draw_step_reward_list.equals(this.can_draw_step_reward_list)) 
            return;
        this.can_draw_step_reward_list = can_draw_step_reward_list;
        saveField(_bm, "can_draw_step_reward_list", can_draw_step_reward_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `total_score` = '").append(total_score).append("',");
        sBuilder.append(" `step_reward_step` = '").append(step_reward_step).append("',");
        sBuilder.append(" `step_reward_score` = '").append(step_reward_score).append("',");
        sBuilder.append(" `can_draw_step_reward_list` = '").append(can_draw_step_reward_list == null ? null : can_draw_step_reward_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_total_score)) sBuilder.append(" `total_score` = '").append(total_score).append("',");
        if(isFieldMarked(FIELD_step_reward_step)) sBuilder.append(" `step_reward_step` = '").append(step_reward_step).append("',");
        if(isFieldMarked(FIELD_step_reward_score)) sBuilder.append(" `step_reward_score` = '").append(step_reward_score).append("',");
        if(isFieldMarked(FIELD_can_draw_step_reward_list)) sBuilder.append(" `can_draw_step_reward_list` = '").append(can_draw_step_reward_list == null ? null : can_draw_step_reward_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `tile_match_player_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家ID',"
                + "`total_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '总分',"
                + "`step_reward_step` int(11) NOT NULL DEFAULT '0' COMMENT '阶段奖励阶段',"
                + "`step_reward_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '阶段奖励分数',"
                + "`can_draw_step_reward_list` varchar(1024) NOT NULL DEFAULT '' COMMENT '可领取的阶段奖励列表',"
                + "KEY `activity_instance_id` (`activity_instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='三消游戏玩家数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=8;//total_score
        _size+=4;//step_reward_step
        _size+=8;//step_reward_score
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(can_draw_step_reward_list);//can_draw_step_reward_list
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(activity_instance_id);
        buff.putLong(cid);
        buff.putLong(total_score);
        buff.putInt(step_reward_step);
        buff.putLong(step_reward_score);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, can_draw_step_reward_list);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        activity_instance_id=buff.getLong();
        cid=buff.getLong();
        total_score=buff.getLong();
        step_reward_step=buff.getInt();
        step_reward_score=buff.getLong();
        can_draw_step_reward_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
