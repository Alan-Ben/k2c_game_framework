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
public class PlayerStageGoalBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_step =1;
    @DataBaseField(type = "bigint(20)", fieldname = "step", comment = "阶段ID")
    private long step;

    public static final int FIELD_is_done =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_done", comment = "是否已完成")
    private boolean is_done;

    public static final int FIELD_had_draw_big_step_list =3;
    @DataBaseField(type = "blob", fieldname = "had_draw_big_step_list", comment = "已领取大阶段ID列表")
    private byte[] had_draw_big_step_list;

    public static final int FIELD_had_draw_big_step_first_reach_list =4;
    @DataBaseField(type = "blob", fieldname = "had_draw_big_step_first_reach_list", comment = "已领取大阶段首达ID列表")
    private byte[] had_draw_big_step_first_reach_list;

    public PlayerStageGoalBO() {
        id = 0;
        cid = 0L;
        step = 0L;
        is_done = false;
        had_draw_big_step_list = null;
        had_draw_big_step_first_reach_list = null;
    }

    public PlayerStageGoalBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        step = rs.getLong(3);
        is_done = rs.getBoolean(4);
        had_draw_big_step_list = rs.getBytes(5);
        had_draw_big_step_first_reach_list = rs.getBytes(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerStageGoalBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `step`, `is_done`, `had_draw_big_step_list`, `had_draw_big_step_first_reach_list`";
    }

    @Override
    public String getTableName() {
        return "`player_stage_goal`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(step).append("', ");
        strBuf.append("'").append(is_done ? 1 : 0).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(had_draw_big_step_list); 
        ret.add(had_draw_big_step_first_reach_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_had_draw_big_step_list)) ret.add(had_draw_big_step_list); 
        if(isFieldMarked(FIELD_had_draw_big_step_first_reach_list)) ret.add(had_draw_big_step_first_reach_list);         return ret;
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

    // 是否已完成
    public boolean getIsDone() { return this.is_done; }
    public void setIsDone(BM _bm, boolean is_done) {
        if(is_done==this.is_done) 
            return;
        this.is_done = is_done; 
        markField(_bm, FIELD_is_done); 
    }
    public void saveIsDone(BM _bm, boolean is_done) {
        if(is_done==this.is_done) 
            return;
        this.is_done = is_done;
        saveField(_bm, "is_done", is_done ? 1 : 0);
    }

    // 已领取大阶段ID列表
    public byte[] getHadDrawBigStepList() { return this.had_draw_big_step_list; }
    public void setHadDrawBigStepList(BM _bm, byte[] had_draw_big_step_list) {
        if(had_draw_big_step_list==this.had_draw_big_step_list) 
            return;
        this.had_draw_big_step_list = had_draw_big_step_list; 
        markField(_bm, FIELD_had_draw_big_step_list); 
    }
    public void saveHadDrawBigStepList(BM _bm, byte[] had_draw_big_step_list) {
        if(had_draw_big_step_list==this.had_draw_big_step_list) 
            return;
        this.had_draw_big_step_list = had_draw_big_step_list;
        saveFieldBytes(_bm, "had_draw_big_step_list", had_draw_big_step_list);
    }

    // 已领取大阶段首达ID列表
    public byte[] getHadDrawBigStepFirstReachList() { return this.had_draw_big_step_first_reach_list; }
    public void setHadDrawBigStepFirstReachList(BM _bm, byte[] had_draw_big_step_first_reach_list) {
        if(had_draw_big_step_first_reach_list==this.had_draw_big_step_first_reach_list) 
            return;
        this.had_draw_big_step_first_reach_list = had_draw_big_step_first_reach_list; 
        markField(_bm, FIELD_had_draw_big_step_first_reach_list); 
    }
    public void saveHadDrawBigStepFirstReachList(BM _bm, byte[] had_draw_big_step_first_reach_list) {
        if(had_draw_big_step_first_reach_list==this.had_draw_big_step_first_reach_list) 
            return;
        this.had_draw_big_step_first_reach_list = had_draw_big_step_first_reach_list;
        saveFieldBytes(_bm, "had_draw_big_step_first_reach_list", had_draw_big_step_first_reach_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `step` = '").append(step).append("',");
        sBuilder.append(" `is_done` = '").append(is_done ? 1 : 0).append("',");
        sBuilder.append(" `had_draw_big_step_list` = ?,");
        sBuilder.append(" `had_draw_big_step_first_reach_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_step)) sBuilder.append(" `step` = '").append(step).append("',");
        if(isFieldMarked(FIELD_is_done)) sBuilder.append(" `is_done` = '").append(is_done ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_had_draw_big_step_list)) sBuilder.append(" `had_draw_big_step_list` = ?,");
        if(isFieldMarked(FIELD_had_draw_big_step_first_reach_list)) sBuilder.append(" `had_draw_big_step_first_reach_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_stage_goal` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`step` bigint(20) NOT NULL DEFAULT '0' COMMENT '阶段ID',"
                + "`is_done` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已完成',"
                + "`had_draw_big_step_list` blob NULL COMMENT '已领取大阶段ID列表',"
                + "`had_draw_big_step_first_reach_list` blob NULL COMMENT '已领取大阶段首达ID列表',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家阶段目标数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=1;//is_done
        _size+=2;_size+=had_draw_big_step_list.length;//had_draw_big_step_list
        _size+=2;_size+=had_draw_big_step_first_reach_list.length;//had_draw_big_step_first_reach_list
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
        buff.put((byte)(is_done?1:0));
        buff.putShort((short)(had_draw_big_step_list == null ? 0 : had_draw_big_step_list.length));if(null != had_draw_big_step_list){buff.put(had_draw_big_step_list);}
        buff.putShort((short)(had_draw_big_step_first_reach_list == null ? 0 : had_draw_big_step_first_reach_list.length));if(null != had_draw_big_step_first_reach_list){buff.put(had_draw_big_step_first_reach_list);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        step=buff.getLong();
        is_done=(buff.get()==1);
        int had_draw_big_step_list_count = buff.getShort();if(had_draw_big_step_list_count>0){had_draw_big_step_list = new byte[had_draw_big_step_list_count];buff.get(had_draw_big_step_list);}
        int had_draw_big_step_first_reach_list_count = buff.getShort();if(had_draw_big_step_first_reach_list_count>0){had_draw_big_step_first_reach_list = new byte[had_draw_big_step_first_reach_list_count];buff.get(had_draw_big_step_first_reach_list);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
