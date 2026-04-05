package CGSDB.Bo;
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
public class NpArenaBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instanceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "实例id")
    private long instanceId;

    public static final int FIELD_server_type =1;
    @DataBaseField(type = "int(11)", fieldname = "server_type", comment = "服务器类型")
    private int server_type;

    public static final int FIELD_server_type_id =2;
    @DataBaseField(type = "int(11)", fieldname = "server_type_id", comment = "服务器id")
    private int server_type_id;

    public static final int FIELD_life_state =3;
    @DataBaseField(type = "int(11)", fieldname = "life_state", comment = "生命周期状态")
    private int life_state;

    public static final int FIELD_ref_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "ref_id", comment = "比武擂台配置表id")
    private long ref_id;

    public static final int FIELD_begin_time_ms =5;
    @DataBaseField(type = "bigint(20)", fieldname = "begin_time_ms", comment = "本期开启时间")
    private long begin_time_ms;

    public static final int FIELD_end_time_ms =6;
    @DataBaseField(type = "bigint(20)", fieldname = "end_time_ms", comment = "本期结束时间")
    private long end_time_ms;

    public static final int FIELD_clear_time_ms =7;
    @DataBaseField(type = "bigint(20)", fieldname = "clear_time_ms", comment = "本期清空时间")
    private long clear_time_ms;

    public static final int FIELD_last_stack_reward_settle_time_ms =8;
    @DataBaseField(type = "bigint(20)", fieldname = "last_stack_reward_settle_time_ms", comment = "最后累计奖励结算时间")
    private long last_stack_reward_settle_time_ms;

    public static final int FIELD_over_msg_info_id =9;
    @DataBaseField(type = "bigint(20)", fieldname = "over_msg_info_id", comment = "最后一条战斗公告消息id")
    private long over_msg_info_id;

    public NpArenaBO() {
        id = 0;
        instanceId = 0L;
        server_type = 0;
        server_type_id = 0;
        life_state = 0;
        ref_id = 0L;
        begin_time_ms = 0L;
        end_time_ms = 0L;
        clear_time_ms = 0L;
        last_stack_reward_settle_time_ms = 0L;
        over_msg_info_id = 0L;
    }

    public NpArenaBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instanceId = rs.getLong(2);
        server_type = rs.getInt(3);
        server_type_id = rs.getInt(4);
        life_state = rs.getInt(5);
        ref_id = rs.getLong(6);
        begin_time_ms = rs.getLong(7);
        end_time_ms = rs.getLong(8);
        clear_time_ms = rs.getLong(9);
        last_stack_reward_settle_time_ms = rs.getLong(10);
        over_msg_info_id = rs.getLong(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpArenaBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instanceId`, `server_type`, `server_type_id`, `life_state`, `ref_id`, `begin_time_ms`, `end_time_ms`, `clear_time_ms`, `last_stack_reward_settle_time_ms`, `over_msg_info_id`";
    }

    @Override
    public String getTableName() {
        return "`np_arena`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(server_type).append("', ");
        strBuf.append("'").append(server_type_id).append("', ");
        strBuf.append("'").append(life_state).append("', ");
        strBuf.append("'").append(ref_id).append("', ");
        strBuf.append("'").append(begin_time_ms).append("', ");
        strBuf.append("'").append(end_time_ms).append("', ");
        strBuf.append("'").append(clear_time_ms).append("', ");
        strBuf.append("'").append(last_stack_reward_settle_time_ms).append("', ");
        strBuf.append("'").append(over_msg_info_id).append("', ");
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

    // 实例id
    public long getInstanceId() { return this.instanceId; }
    public void setInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId; 
        markField(_bm, FIELD_instanceId); 
    }
    public void saveInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId;
        saveField(_bm, "instanceId", instanceId);
    }

    // 服务器类型
    public int getServerType() { return this.server_type; }
    public void setServerType(BM _bm, int server_type) {
        if(server_type==this.server_type) 
            return;
        this.server_type = server_type; 
        markField(_bm, FIELD_server_type); 
    }
    public void saveServerType(BM _bm, int server_type) {
        if(server_type==this.server_type) 
            return;
        this.server_type = server_type;
        saveField(_bm, "server_type", server_type);
    }

    // 服务器id
    public int getServerTypeId() { return this.server_type_id; }
    public void setServerTypeId(BM _bm, int server_type_id) {
        if(server_type_id==this.server_type_id) 
            return;
        this.server_type_id = server_type_id; 
        markField(_bm, FIELD_server_type_id); 
    }
    public void saveServerTypeId(BM _bm, int server_type_id) {
        if(server_type_id==this.server_type_id) 
            return;
        this.server_type_id = server_type_id;
        saveField(_bm, "server_type_id", server_type_id);
    }

    // 生命周期状态
    public int getLifeState() { return this.life_state; }
    public void setLifeState(BM _bm, int life_state) {
        if(life_state==this.life_state) 
            return;
        this.life_state = life_state; 
        markField(_bm, FIELD_life_state); 
    }
    public void saveLifeState(BM _bm, int life_state) {
        if(life_state==this.life_state) 
            return;
        this.life_state = life_state;
        saveField(_bm, "life_state", life_state);
    }

    // 比武擂台配置表id
    public long getRefId() { return this.ref_id; }
    public void setRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id; 
        markField(_bm, FIELD_ref_id); 
    }
    public void saveRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id;
        saveField(_bm, "ref_id", ref_id);
    }

    // 本期开启时间
    public long getBeginTimeMs() { return this.begin_time_ms; }
    public void setBeginTimeMs(BM _bm, long begin_time_ms) {
        if(begin_time_ms==this.begin_time_ms) 
            return;
        this.begin_time_ms = begin_time_ms; 
        markField(_bm, FIELD_begin_time_ms); 
    }
    public void saveBeginTimeMs(BM _bm, long begin_time_ms) {
        if(begin_time_ms==this.begin_time_ms) 
            return;
        this.begin_time_ms = begin_time_ms;
        saveField(_bm, "begin_time_ms", begin_time_ms);
    }

    // 本期结束时间
    public long getEndTimeMs() { return this.end_time_ms; }
    public void setEndTimeMs(BM _bm, long end_time_ms) {
        if(end_time_ms==this.end_time_ms) 
            return;
        this.end_time_ms = end_time_ms; 
        markField(_bm, FIELD_end_time_ms); 
    }
    public void saveEndTimeMs(BM _bm, long end_time_ms) {
        if(end_time_ms==this.end_time_ms) 
            return;
        this.end_time_ms = end_time_ms;
        saveField(_bm, "end_time_ms", end_time_ms);
    }

    // 本期清空时间
    public long getClearTimeMs() { return this.clear_time_ms; }
    public void setClearTimeMs(BM _bm, long clear_time_ms) {
        if(clear_time_ms==this.clear_time_ms) 
            return;
        this.clear_time_ms = clear_time_ms; 
        markField(_bm, FIELD_clear_time_ms); 
    }
    public void saveClearTimeMs(BM _bm, long clear_time_ms) {
        if(clear_time_ms==this.clear_time_ms) 
            return;
        this.clear_time_ms = clear_time_ms;
        saveField(_bm, "clear_time_ms", clear_time_ms);
    }

    // 最后累计奖励结算时间
    public long getLastStackRewardSettleTimeMs() { return this.last_stack_reward_settle_time_ms; }
    public void setLastStackRewardSettleTimeMs(BM _bm, long last_stack_reward_settle_time_ms) {
        if(last_stack_reward_settle_time_ms==this.last_stack_reward_settle_time_ms) 
            return;
        this.last_stack_reward_settle_time_ms = last_stack_reward_settle_time_ms; 
        markField(_bm, FIELD_last_stack_reward_settle_time_ms); 
    }
    public void saveLastStackRewardSettleTimeMs(BM _bm, long last_stack_reward_settle_time_ms) {
        if(last_stack_reward_settle_time_ms==this.last_stack_reward_settle_time_ms) 
            return;
        this.last_stack_reward_settle_time_ms = last_stack_reward_settle_time_ms;
        saveField(_bm, "last_stack_reward_settle_time_ms", last_stack_reward_settle_time_ms);
    }

    // 最后一条战斗公告消息id
    public long getOverMsgInfoId() { return this.over_msg_info_id; }
    public void setOverMsgInfoId(BM _bm, long over_msg_info_id) {
        if(over_msg_info_id==this.over_msg_info_id) 
            return;
        this.over_msg_info_id = over_msg_info_id; 
        markField(_bm, FIELD_over_msg_info_id); 
    }
    public void saveOverMsgInfoId(BM _bm, long over_msg_info_id) {
        if(over_msg_info_id==this.over_msg_info_id) 
            return;
        this.over_msg_info_id = over_msg_info_id;
        saveField(_bm, "over_msg_info_id", over_msg_info_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `server_type` = '").append(server_type).append("',");
        sBuilder.append(" `server_type_id` = '").append(server_type_id).append("',");
        sBuilder.append(" `life_state` = '").append(life_state).append("',");
        sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        sBuilder.append(" `begin_time_ms` = '").append(begin_time_ms).append("',");
        sBuilder.append(" `end_time_ms` = '").append(end_time_ms).append("',");
        sBuilder.append(" `clear_time_ms` = '").append(clear_time_ms).append("',");
        sBuilder.append(" `last_stack_reward_settle_time_ms` = '").append(last_stack_reward_settle_time_ms).append("',");
        sBuilder.append(" `over_msg_info_id` = '").append(over_msg_info_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_server_type)) sBuilder.append(" `server_type` = '").append(server_type).append("',");
        if(isFieldMarked(FIELD_server_type_id)) sBuilder.append(" `server_type_id` = '").append(server_type_id).append("',");
        if(isFieldMarked(FIELD_life_state)) sBuilder.append(" `life_state` = '").append(life_state).append("',");
        if(isFieldMarked(FIELD_ref_id)) sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        if(isFieldMarked(FIELD_begin_time_ms)) sBuilder.append(" `begin_time_ms` = '").append(begin_time_ms).append("',");
        if(isFieldMarked(FIELD_end_time_ms)) sBuilder.append(" `end_time_ms` = '").append(end_time_ms).append("',");
        if(isFieldMarked(FIELD_clear_time_ms)) sBuilder.append(" `clear_time_ms` = '").append(clear_time_ms).append("',");
        if(isFieldMarked(FIELD_last_stack_reward_settle_time_ms)) sBuilder.append(" `last_stack_reward_settle_time_ms` = '").append(last_stack_reward_settle_time_ms).append("',");
        if(isFieldMarked(FIELD_over_msg_info_id)) sBuilder.append(" `over_msg_info_id` = '").append(over_msg_info_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_arena` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '实例id',"
                + "`server_type` int(11) NOT NULL DEFAULT '0' COMMENT '服务器类型',"
                + "`server_type_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器id',"
                + "`life_state` int(11) NOT NULL DEFAULT '0' COMMENT '生命周期状态',"
                + "`ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '比武擂台配置表id',"
                + "`begin_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '本期开启时间',"
                + "`end_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '本期结束时间',"
                + "`clear_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '本期清空时间',"
                + "`last_stack_reward_settle_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后累计奖励结算时间',"
                + "`over_msg_info_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后一条战斗公告消息id',"
                + "UNIQUE INDEX `instanceId` (`instanceId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='比武擂台 聚会数据表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.crossgame_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//instanceId
        _size+=4;//server_type
        _size+=4;//server_type_id
        _size+=4;//life_state
        _size+=8;//ref_id
        _size+=8;//begin_time_ms
        _size+=8;//end_time_ms
        _size+=8;//clear_time_ms
        _size+=8;//last_stack_reward_settle_time_ms
        _size+=8;//over_msg_info_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instanceId);
        buff.putInt(server_type);
        buff.putInt(server_type_id);
        buff.putInt(life_state);
        buff.putLong(ref_id);
        buff.putLong(begin_time_ms);
        buff.putLong(end_time_ms);
        buff.putLong(clear_time_ms);
        buff.putLong(last_stack_reward_settle_time_ms);
        buff.putLong(over_msg_info_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instanceId=buff.getLong();
        server_type=buff.getInt();
        server_type_id=buff.getInt();
        life_state=buff.getInt();
        ref_id=buff.getLong();
        begin_time_ms=buff.getLong();
        end_time_ms=buff.getLong();
        clear_time_ms=buff.getLong();
        last_stack_reward_settle_time_ms=buff.getLong();
        over_msg_info_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
