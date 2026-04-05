package ActivitiesV02.Bo;
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
public class NumMergePlayerInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_activity_instance_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例ID")
    private long activity_instance_id;

    public static final int FIELD_total_score =2;
    @DataBaseField(type = "bigint(20)", fieldname = "total_score", comment = "累计总积分")
    private long total_score;

    public static final int FIELD_round_max_score =3;
    @DataBaseField(type = "bigint(20)", fieldname = "round_max_score", comment = "单轮最高分")
    private long round_max_score;

    public static final int FIELD_total_cost_stamina =4;
    @DataBaseField(type = "int(11)", fieldname = "total_cost_stamina", comment = "累计消耗体力")
    private int total_cost_stamina;

    public static final int FIELD_current_cost_stamina =5;
    @DataBaseField(type = "int(11)", fieldname = "current_cost_stamina", comment = "当前消耗体力")
    private int current_cost_stamina;

    public static final int FIELD_current_step =6;
    @DataBaseField(type = "int(11)", fieldname = "current_step", comment = "当前步数")
    private int current_step;

    public static final int FIELD_current_score =7;
    @DataBaseField(type = "bigint(20)", fieldname = "current_score", comment = "当前得分")
    private long current_score;

    public static final int FIELD_blocks =8;
    @DataBaseField(type = "blob", fieldname = "blocks", comment = "方块数据")
    private byte[] blocks;

    public static final int FIELD_generated_buff_count =9;
    @DataBaseField(type = "int(11)", fieldname = "generated_buff_count", comment = "已生成buff数量")
    private int generated_buff_count;

    public static final int FIELD_box_level =10;
    @DataBaseField(type = "int(11)", fieldname = "box_level", comment = "已领取的宝箱等级")
    private int box_level;

    public static final int FIELD_box_score =11;
    @DataBaseField(type = "bigint(20)", fieldname = "box_score", comment = "当前宝箱积分")
    private long box_score;

    public NumMergePlayerInfoBO() {
        id = 0;
        cid = 0L;
        activity_instance_id = 0L;
        total_score = 0L;
        round_max_score = 0L;
        total_cost_stamina = 0;
        current_cost_stamina = 0;
        current_step = 0;
        current_score = 0L;
        blocks = null;
        generated_buff_count = 0;
        box_level = 0;
        box_score = 0L;
    }

    public NumMergePlayerInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        activity_instance_id = rs.getLong(3);
        total_score = rs.getLong(4);
        round_max_score = rs.getLong(5);
        total_cost_stamina = rs.getInt(6);
        current_cost_stamina = rs.getInt(7);
        current_step = rs.getInt(8);
        current_score = rs.getLong(9);
        blocks = rs.getBytes(10);
        generated_buff_count = rs.getInt(11);
        box_level = rs.getInt(12);
        box_score = rs.getLong(13);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NumMergePlayerInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `activity_instance_id`, `total_score`, `round_max_score`, `total_cost_stamina`, `current_cost_stamina`, `current_step`, `current_score`, `blocks`, `generated_buff_count`, `box_level`, `box_score`";
    }

    @Override
    public String getTableName() {
        return "`num_merge_player_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(total_score).append("', ");
        strBuf.append("'").append(round_max_score).append("', ");
        strBuf.append("'").append(total_cost_stamina).append("', ");
        strBuf.append("'").append(current_cost_stamina).append("', ");
        strBuf.append("'").append(current_step).append("', ");
        strBuf.append("'").append(current_score).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(generated_buff_count).append("', ");
        strBuf.append("'").append(box_level).append("', ");
        strBuf.append("'").append(box_score).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(blocks);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_blocks)) ret.add(blocks);         return ret;
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

    // 累计总积分
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

    // 单轮最高分
    public long getRoundMaxScore() { return this.round_max_score; }
    public void setRoundMaxScore(BM _bm, long round_max_score) {
        if(round_max_score==this.round_max_score) 
            return;
        this.round_max_score = round_max_score; 
        markField(_bm, FIELD_round_max_score); 
    }
    public void saveRoundMaxScore(BM _bm, long round_max_score) {
        if(round_max_score==this.round_max_score) 
            return;
        this.round_max_score = round_max_score;
        saveField(_bm, "round_max_score", round_max_score);
    }

    // 累计消耗体力
    public int getTotalCostStamina() { return this.total_cost_stamina; }
    public void setTotalCostStamina(BM _bm, int total_cost_stamina) {
        if(total_cost_stamina==this.total_cost_stamina) 
            return;
        this.total_cost_stamina = total_cost_stamina; 
        markField(_bm, FIELD_total_cost_stamina); 
    }
    public void saveTotalCostStamina(BM _bm, int total_cost_stamina) {
        if(total_cost_stamina==this.total_cost_stamina) 
            return;
        this.total_cost_stamina = total_cost_stamina;
        saveField(_bm, "total_cost_stamina", total_cost_stamina);
    }

    // 当前消耗体力
    public int getCurrentCostStamina() { return this.current_cost_stamina; }
    public void setCurrentCostStamina(BM _bm, int current_cost_stamina) {
        if(current_cost_stamina==this.current_cost_stamina) 
            return;
        this.current_cost_stamina = current_cost_stamina; 
        markField(_bm, FIELD_current_cost_stamina); 
    }
    public void saveCurrentCostStamina(BM _bm, int current_cost_stamina) {
        if(current_cost_stamina==this.current_cost_stamina) 
            return;
        this.current_cost_stamina = current_cost_stamina;
        saveField(_bm, "current_cost_stamina", current_cost_stamina);
    }

    // 当前步数
    public int getCurrentStep() { return this.current_step; }
    public void setCurrentStep(BM _bm, int current_step) {
        if(current_step==this.current_step) 
            return;
        this.current_step = current_step; 
        markField(_bm, FIELD_current_step); 
    }
    public void saveCurrentStep(BM _bm, int current_step) {
        if(current_step==this.current_step) 
            return;
        this.current_step = current_step;
        saveField(_bm, "current_step", current_step);
    }

    // 当前得分
    public long getCurrentScore() { return this.current_score; }
    public void setCurrentScore(BM _bm, long current_score) {
        if(current_score==this.current_score) 
            return;
        this.current_score = current_score; 
        markField(_bm, FIELD_current_score); 
    }
    public void saveCurrentScore(BM _bm, long current_score) {
        if(current_score==this.current_score) 
            return;
        this.current_score = current_score;
        saveField(_bm, "current_score", current_score);
    }

    // 方块数据
    public byte[] getBlocks() { return this.blocks; }
    public void setBlocks(BM _bm, byte[] blocks) {
        if(blocks==this.blocks) 
            return;
        this.blocks = blocks; 
        markField(_bm, FIELD_blocks); 
    }
    public void saveBlocks(BM _bm, byte[] blocks) {
        if(blocks==this.blocks) 
            return;
        this.blocks = blocks;
        saveFieldBytes(_bm, "blocks", blocks);
    }

    // 已生成buff数量
    public int getGeneratedBuffCount() { return this.generated_buff_count; }
    public void setGeneratedBuffCount(BM _bm, int generated_buff_count) {
        if(generated_buff_count==this.generated_buff_count) 
            return;
        this.generated_buff_count = generated_buff_count; 
        markField(_bm, FIELD_generated_buff_count); 
    }
    public void saveGeneratedBuffCount(BM _bm, int generated_buff_count) {
        if(generated_buff_count==this.generated_buff_count) 
            return;
        this.generated_buff_count = generated_buff_count;
        saveField(_bm, "generated_buff_count", generated_buff_count);
    }

    // 已领取的宝箱等级
    public int getBoxLevel() { return this.box_level; }
    public void setBoxLevel(BM _bm, int box_level) {
        if(box_level==this.box_level) 
            return;
        this.box_level = box_level; 
        markField(_bm, FIELD_box_level); 
    }
    public void saveBoxLevel(BM _bm, int box_level) {
        if(box_level==this.box_level) 
            return;
        this.box_level = box_level;
        saveField(_bm, "box_level", box_level);
    }

    // 当前宝箱积分
    public long getBoxScore() { return this.box_score; }
    public void setBoxScore(BM _bm, long box_score) {
        if(box_score==this.box_score) 
            return;
        this.box_score = box_score; 
        markField(_bm, FIELD_box_score); 
    }
    public void saveBoxScore(BM _bm, long box_score) {
        if(box_score==this.box_score) 
            return;
        this.box_score = box_score;
        saveField(_bm, "box_score", box_score);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `total_score` = '").append(total_score).append("',");
        sBuilder.append(" `round_max_score` = '").append(round_max_score).append("',");
        sBuilder.append(" `total_cost_stamina` = '").append(total_cost_stamina).append("',");
        sBuilder.append(" `current_cost_stamina` = '").append(current_cost_stamina).append("',");
        sBuilder.append(" `current_step` = '").append(current_step).append("',");
        sBuilder.append(" `current_score` = '").append(current_score).append("',");
        sBuilder.append(" `blocks` = ?,");
        sBuilder.append(" `generated_buff_count` = '").append(generated_buff_count).append("',");
        sBuilder.append(" `box_level` = '").append(box_level).append("',");
        sBuilder.append(" `box_score` = '").append(box_score).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_total_score)) sBuilder.append(" `total_score` = '").append(total_score).append("',");
        if(isFieldMarked(FIELD_round_max_score)) sBuilder.append(" `round_max_score` = '").append(round_max_score).append("',");
        if(isFieldMarked(FIELD_total_cost_stamina)) sBuilder.append(" `total_cost_stamina` = '").append(total_cost_stamina).append("',");
        if(isFieldMarked(FIELD_current_cost_stamina)) sBuilder.append(" `current_cost_stamina` = '").append(current_cost_stamina).append("',");
        if(isFieldMarked(FIELD_current_step)) sBuilder.append(" `current_step` = '").append(current_step).append("',");
        if(isFieldMarked(FIELD_current_score)) sBuilder.append(" `current_score` = '").append(current_score).append("',");
        if(isFieldMarked(FIELD_blocks)) sBuilder.append(" `blocks` = ?,");
        if(isFieldMarked(FIELD_generated_buff_count)) sBuilder.append(" `generated_buff_count` = '").append(generated_buff_count).append("',");
        if(isFieldMarked(FIELD_box_level)) sBuilder.append(" `box_level` = '").append(box_level).append("',");
        if(isFieldMarked(FIELD_box_score)) sBuilder.append(" `box_score` = '").append(box_score).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `num_merge_player_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`total_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '累计总积分',"
                + "`round_max_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '单轮最高分',"
                + "`total_cost_stamina` int(11) NOT NULL DEFAULT '0' COMMENT '累计消耗体力',"
                + "`current_cost_stamina` int(11) NOT NULL DEFAULT '0' COMMENT '当前消耗体力',"
                + "`current_step` int(11) NOT NULL DEFAULT '0' COMMENT '当前步数',"
                + "`current_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前得分',"
                + "`blocks` blob NULL COMMENT '方块数据',"
                + "`generated_buff_count` int(11) NOT NULL DEFAULT '0' COMMENT '已生成buff数量',"
                + "`box_level` int(11) NOT NULL DEFAULT '0' COMMENT '已领取的宝箱等级',"
                + "`box_score` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前宝箱积分',"
                + "KEY `activity_instance_id` (`activity_instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='数字合并玩家信息表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//activity_instance_id
        _size+=8;//total_score
        _size+=8;//round_max_score
        _size+=4;//total_cost_stamina
        _size+=4;//current_cost_stamina
        _size+=4;//current_step
        _size+=8;//current_score
        _size+=2;_size+=blocks.length;//blocks
        _size+=4;//generated_buff_count
        _size+=4;//box_level
        _size+=8;//box_score
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(activity_instance_id);
        buff.putLong(total_score);
        buff.putLong(round_max_score);
        buff.putInt(total_cost_stamina);
        buff.putInt(current_cost_stamina);
        buff.putInt(current_step);
        buff.putLong(current_score);
        buff.putShort((short)(blocks == null ? 0 : blocks.length));if(null != blocks){buff.put(blocks);}
        buff.putInt(generated_buff_count);
        buff.putInt(box_level);
        buff.putLong(box_score);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        activity_instance_id=buff.getLong();
        total_score=buff.getLong();
        round_max_score=buff.getLong();
        total_cost_stamina=buff.getInt();
        current_cost_stamina=buff.getInt();
        current_step=buff.getInt();
        current_score=buff.getLong();
        int blocks_count = buff.getShort();if(blocks_count>0){blocks = new byte[blocks_count];buff.get(blocks);}
        generated_buff_count=buff.getInt();
        box_level=buff.getInt();
        box_score=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
