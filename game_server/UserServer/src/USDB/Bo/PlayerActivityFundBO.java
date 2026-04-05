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
public class PlayerActivityFundBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_fund_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "fund_id", comment = "基金ID")
    private long fund_id;

    public static final int FIELD_activity_instance_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例ID（常驻基金为0）")
    private long activity_instance_id;

    public static final int FIELD_formula_score =3;
    @DataBaseField(type = "bigint(20)", fieldname = "formula_score", comment = "公式分数")
    private long formula_score;

    public static final int FIELD_task_score =4;
    @DataBaseField(type = "bigint(20)", fieldname = "task_score", comment = "任务分数")
    private long task_score;

    public static final int FIELD_drawn_free_steps =5;
    @DataBaseField(type = "int(11)", fieldname = "drawn_free_steps", comment = "已领取免费档最大阶段")
    private int drawn_free_steps;

    public static final int FIELD_drawn_paid_steps =6;
    @DataBaseField(type = "int(11)", fieldname = "drawn_paid_steps", comment = "已领取付费档最大阶段")
    private int drawn_paid_steps;

    public static final int FIELD_activity_start_time_ms =7;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_start_time_ms", comment = "活动开始时间（毫秒，永久基金为0）")
    private long activity_start_time_ms;

    public static final int FIELD_last_refresh_round =8;
    @DataBaseField(type = "bigint(20)", fieldname = "last_refresh_round", comment = "上次刷新轮次号")
    private long last_refresh_round;

    public PlayerActivityFundBO() {
        id = 0;
        cid = 0L;
        fund_id = 0L;
        activity_instance_id = 0L;
        formula_score = 0L;
        task_score = 0L;
        drawn_free_steps = 0;
        drawn_paid_steps = 0;
        activity_start_time_ms = 0L;
        last_refresh_round = 0L;
    }

    public PlayerActivityFundBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        fund_id = rs.getLong(3);
        activity_instance_id = rs.getLong(4);
        formula_score = rs.getLong(5);
        task_score = rs.getLong(6);
        drawn_free_steps = rs.getInt(7);
        drawn_paid_steps = rs.getInt(8);
        activity_start_time_ms = rs.getLong(9);
        last_refresh_round = rs.getLong(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerActivityFundBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `fund_id`, `activity_instance_id`, `formula_score`, `task_score`, `drawn_free_steps`, `drawn_paid_steps`, `activity_start_time_ms`, `last_refresh_round`";
    }

    @Override
    public String getTableName() {
        return "`player_activity_fund`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(fund_id).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(formula_score).append("', ");
        strBuf.append("'").append(task_score).append("', ");
        strBuf.append("'").append(drawn_free_steps).append("', ");
        strBuf.append("'").append(drawn_paid_steps).append("', ");
        strBuf.append("'").append(activity_start_time_ms).append("', ");
        strBuf.append("'").append(last_refresh_round).append("', ");
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

    // 基金ID
    public long getFundId() { return this.fund_id; }
    public void setFundId(BM _bm, long fund_id) {
        if(fund_id==this.fund_id) 
            return;
        this.fund_id = fund_id; 
        markField(_bm, FIELD_fund_id); 
    }
    public void saveFundId(BM _bm, long fund_id) {
        if(fund_id==this.fund_id) 
            return;
        this.fund_id = fund_id;
        saveField(_bm, "fund_id", fund_id);
    }

    // 活动实例ID（常驻基金为0）
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

    // 公式分数
    public long getFormulaScore() { return this.formula_score; }
    public void setFormulaScore(BM _bm, long formula_score) {
        if(formula_score==this.formula_score) 
            return;
        this.formula_score = formula_score; 
        markField(_bm, FIELD_formula_score); 
    }
    public void saveFormulaScore(BM _bm, long formula_score) {
        if(formula_score==this.formula_score) 
            return;
        this.formula_score = formula_score;
        saveField(_bm, "formula_score", formula_score);
    }

    // 任务分数
    public long getTaskScore() { return this.task_score; }
    public void setTaskScore(BM _bm, long task_score) {
        if(task_score==this.task_score) 
            return;
        this.task_score = task_score; 
        markField(_bm, FIELD_task_score); 
    }
    public void saveTaskScore(BM _bm, long task_score) {
        if(task_score==this.task_score) 
            return;
        this.task_score = task_score;
        saveField(_bm, "task_score", task_score);
    }

    // 已领取免费档最大阶段
    public int getDrawnFreeSteps() { return this.drawn_free_steps; }
    public void setDrawnFreeSteps(BM _bm, int drawn_free_steps) {
        if(drawn_free_steps==this.drawn_free_steps) 
            return;
        this.drawn_free_steps = drawn_free_steps; 
        markField(_bm, FIELD_drawn_free_steps); 
    }
    public void saveDrawnFreeSteps(BM _bm, int drawn_free_steps) {
        if(drawn_free_steps==this.drawn_free_steps) 
            return;
        this.drawn_free_steps = drawn_free_steps;
        saveField(_bm, "drawn_free_steps", drawn_free_steps);
    }

    // 已领取付费档最大阶段
    public int getDrawnPaidSteps() { return this.drawn_paid_steps; }
    public void setDrawnPaidSteps(BM _bm, int drawn_paid_steps) {
        if(drawn_paid_steps==this.drawn_paid_steps) 
            return;
        this.drawn_paid_steps = drawn_paid_steps; 
        markField(_bm, FIELD_drawn_paid_steps); 
    }
    public void saveDrawnPaidSteps(BM _bm, int drawn_paid_steps) {
        if(drawn_paid_steps==this.drawn_paid_steps) 
            return;
        this.drawn_paid_steps = drawn_paid_steps;
        saveField(_bm, "drawn_paid_steps", drawn_paid_steps);
    }

    // 活动开始时间（毫秒，永久基金为0）
    public long getActivityStartTimeMs() { return this.activity_start_time_ms; }
    public void setActivityStartTimeMs(BM _bm, long activity_start_time_ms) {
        if(activity_start_time_ms==this.activity_start_time_ms) 
            return;
        this.activity_start_time_ms = activity_start_time_ms; 
        markField(_bm, FIELD_activity_start_time_ms); 
    }
    public void saveActivityStartTimeMs(BM _bm, long activity_start_time_ms) {
        if(activity_start_time_ms==this.activity_start_time_ms) 
            return;
        this.activity_start_time_ms = activity_start_time_ms;
        saveField(_bm, "activity_start_time_ms", activity_start_time_ms);
    }

    // 上次刷新轮次号
    public long getLastRefreshRound() { return this.last_refresh_round; }
    public void setLastRefreshRound(BM _bm, long last_refresh_round) {
        if(last_refresh_round==this.last_refresh_round) 
            return;
        this.last_refresh_round = last_refresh_round; 
        markField(_bm, FIELD_last_refresh_round); 
    }
    public void saveLastRefreshRound(BM _bm, long last_refresh_round) {
        if(last_refresh_round==this.last_refresh_round) 
            return;
        this.last_refresh_round = last_refresh_round;
        saveField(_bm, "last_refresh_round", last_refresh_round);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `fund_id` = '").append(fund_id).append("',");
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `formula_score` = '").append(formula_score).append("',");
        sBuilder.append(" `task_score` = '").append(task_score).append("',");
        sBuilder.append(" `drawn_free_steps` = '").append(drawn_free_steps).append("',");
        sBuilder.append(" `drawn_paid_steps` = '").append(drawn_paid_steps).append("',");
        sBuilder.append(" `activity_start_time_ms` = '").append(activity_start_time_ms).append("',");
        sBuilder.append(" `last_refresh_round` = '").append(last_refresh_round).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_fund_id)) sBuilder.append(" `fund_id` = '").append(fund_id).append("',");
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_formula_score)) sBuilder.append(" `formula_score` = '").append(formula_score).append("',");
        if(isFieldMarked(FIELD_task_score)) sBuilder.append(" `task_score` = '").append(task_score).append("',");
        if(isFieldMarked(FIELD_drawn_free_steps)) sBuilder.append(" `drawn_free_steps` = '").append(drawn_free_steps).append("',");
        if(isFieldMarked(FIELD_drawn_paid_steps)) sBuilder.append(" `drawn_paid_steps` = '").append(drawn_paid_steps).append("',");
        if(isFieldMarked(FIELD_activity_start_time_ms)) sBuilder.append(" `activity_start_time_ms` = '").append(activity_start_time_ms).append("',");
        if(isFieldMarked(FIELD_last_refresh_round)) sBuilder.append(" `last_refresh_round` = '").append(last_refresh_round).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_activity_fund` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`fund_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '基金ID',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID（常驻基金为0）',"
                + "`formula_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '公式分数',"
                + "`task_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务分数',"
                + "`drawn_free_steps` int(11) NOT NULL DEFAULT '0' COMMENT '已领取免费档最大阶段',"
                + "`drawn_paid_steps` int(11) NOT NULL DEFAULT '0' COMMENT '已领取付费档最大阶段',"
                + "`activity_start_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动开始时间（毫秒，永久基金为0）',"
                + "`last_refresh_round` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次刷新轮次号',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家活动基金数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//fund_id
        _size+=8;//activity_instance_id
        _size+=8;//formula_score
        _size+=8;//task_score
        _size+=4;//drawn_free_steps
        _size+=4;//drawn_paid_steps
        _size+=8;//activity_start_time_ms
        _size+=8;//last_refresh_round
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(fund_id);
        buff.putLong(activity_instance_id);
        buff.putLong(formula_score);
        buff.putLong(task_score);
        buff.putInt(drawn_free_steps);
        buff.putInt(drawn_paid_steps);
        buff.putLong(activity_start_time_ms);
        buff.putLong(last_refresh_round);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        fund_id=buff.getLong();
        activity_instance_id=buff.getLong();
        formula_score=buff.getLong();
        task_score=buff.getLong();
        drawn_free_steps=buff.getInt();
        drawn_paid_steps=buff.getInt();
        activity_start_time_ms=buff.getLong();
        last_refresh_round=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
