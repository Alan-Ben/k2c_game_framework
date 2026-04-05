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
public class TileMatchPlayerGameInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_activity_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例id")
    private long activity_instance_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家ID")
    private long cid;

    public static final int FIELD_mode_type =2;
    @DataBaseField(type = "int(11)", fieldname = "mode_type", comment = "游戏类型 ETileMatch_ModeType")
    private int mode_type;

    public static final int FIELD_block_list =3;
    @DataBaseField(type = "blob", fieldname = "block_list", comment = "三消方格列表")
    private byte[] block_list;

    public static final int FIELD_task_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "task_id", comment = "任务ID")
    private long task_id;

    public static final int FIELD_had_go_step =5;
    @DataBaseField(type = "int(11)", fieldname = "had_go_step", comment = "已经走的步数")
    private int had_go_step;

    public static final int FIELD_task_block_list =6;
    @DataBaseField(type = "varchar(1024)", fieldname = "task_block_list", comment = "任务方块计数列表")
    private String task_block_list;

    public TileMatchPlayerGameInfoBO() {
        id = 0;
        activity_instance_id = 0L;
        cid = 0L;
        mode_type = 0;
        block_list = null;
        task_id = 0L;
        had_go_step = 0;
        task_block_list = "";
    }

    public TileMatchPlayerGameInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        activity_instance_id = rs.getLong(2);
        cid = rs.getLong(3);
        mode_type = rs.getInt(4);
        block_list = rs.getBytes(5);
        task_id = rs.getLong(6);
        had_go_step = rs.getInt(7);
        task_block_list = rs.getString(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new TileMatchPlayerGameInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `activity_instance_id`, `cid`, `mode_type`, `block_list`, `task_id`, `had_go_step`, `task_block_list`";
    }

    @Override
    public String getTableName() {
        return "`tile_match_player_game_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(mode_type).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(task_id).append("', ");
        strBuf.append("'").append(had_go_step).append("', ");
        strBuf.append("'").append(task_block_list == null ? null : task_block_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(block_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_block_list)) ret.add(block_list);         return ret;
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

    // 游戏类型 ETileMatch_ModeType
    public int getModeType() { return this.mode_type; }
    public void setModeType(BM _bm, int mode_type) {
        if(mode_type==this.mode_type) 
            return;
        this.mode_type = mode_type; 
        markField(_bm, FIELD_mode_type); 
    }
    public void saveModeType(BM _bm, int mode_type) {
        if(mode_type==this.mode_type) 
            return;
        this.mode_type = mode_type;
        saveField(_bm, "mode_type", mode_type);
    }

    // 三消方格列表
    public byte[] getBlockList() { return this.block_list; }
    public void setBlockList(BM _bm, byte[] block_list) {
        if(block_list==this.block_list) 
            return;
        this.block_list = block_list; 
        markField(_bm, FIELD_block_list); 
    }
    public void saveBlockList(BM _bm, byte[] block_list) {
        if(block_list==this.block_list) 
            return;
        this.block_list = block_list;
        saveFieldBytes(_bm, "block_list", block_list);
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

    // 已经走的步数
    public int getHadGoStep() { return this.had_go_step; }
    public void setHadGoStep(BM _bm, int had_go_step) {
        if(had_go_step==this.had_go_step) 
            return;
        this.had_go_step = had_go_step; 
        markField(_bm, FIELD_had_go_step); 
    }
    public void saveHadGoStep(BM _bm, int had_go_step) {
        if(had_go_step==this.had_go_step) 
            return;
        this.had_go_step = had_go_step;
        saveField(_bm, "had_go_step", had_go_step);
    }

    // 任务方块计数列表
    public String getTaskBlockList() { return this.task_block_list; }
    public void setTaskBlockList(BM _bm, String task_block_list) {
        if(task_block_list.equals(this.task_block_list)) 
            return;
        this.task_block_list = task_block_list; 
        markField(_bm, FIELD_task_block_list); 
    }
    public void saveTaskBlockList(BM _bm, String task_block_list) {
        if(task_block_list.equals(this.task_block_list)) 
            return;
        this.task_block_list = task_block_list;
        saveField(_bm, "task_block_list", task_block_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `mode_type` = '").append(mode_type).append("',");
        sBuilder.append(" `block_list` = ?,");
        sBuilder.append(" `task_id` = '").append(task_id).append("',");
        sBuilder.append(" `had_go_step` = '").append(had_go_step).append("',");
        sBuilder.append(" `task_block_list` = '").append(task_block_list == null ? null : task_block_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_mode_type)) sBuilder.append(" `mode_type` = '").append(mode_type).append("',");
        if(isFieldMarked(FIELD_block_list)) sBuilder.append(" `block_list` = ?,");
        if(isFieldMarked(FIELD_task_id)) sBuilder.append(" `task_id` = '").append(task_id).append("',");
        if(isFieldMarked(FIELD_had_go_step)) sBuilder.append(" `had_go_step` = '").append(had_go_step).append("',");
        if(isFieldMarked(FIELD_task_block_list)) sBuilder.append(" `task_block_list` = '").append(task_block_list == null ? null : task_block_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `tile_match_player_game_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家ID',"
                + "`mode_type` int(11) NOT NULL DEFAULT '0' COMMENT '游戏类型 ETileMatch_ModeType',"
                + "`block_list` blob NULL COMMENT '三消方格列表',"
                + "`task_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '任务ID',"
                + "`had_go_step` int(11) NOT NULL DEFAULT '0' COMMENT '已经走的步数',"
                + "`task_block_list` varchar(1024) NOT NULL DEFAULT '' COMMENT '任务方块计数列表',"
                + "KEY `activity_instance_id` (`activity_instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='三消游戏玩家游戏数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//mode_type
        _size+=2;_size+=block_list.length;//block_list
        _size+=8;//task_id
        _size+=4;//had_go_step
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(task_block_list);//task_block_list
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
        buff.putInt(mode_type);
        buff.putShort((short)(block_list == null ? 0 : block_list.length));if(null != block_list){buff.put(block_list);}
        buff.putLong(task_id);
        buff.putInt(had_go_step);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, task_block_list);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        activity_instance_id=buff.getLong();
        cid=buff.getLong();
        mode_type=buff.getInt();
        int block_list_count = buff.getShort();if(block_list_count>0){block_list = new byte[block_list_count];buff.get(block_list);}
        task_id=buff.getLong();
        had_go_step=buff.getInt();
        task_block_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
