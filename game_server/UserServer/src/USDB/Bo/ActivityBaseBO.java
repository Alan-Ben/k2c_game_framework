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
public class ActivityBaseBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_activity_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_id", comment = "活动配置ID")
    private long activity_id;

    public static final int FIELD_cross_instance_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cross_instance_id", comment = "跨服分组实例ID")
    private long cross_instance_id;

    public static final int FIELD_game_logic_instance_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "game_logic_instance_id", comment = "游戏逻辑主体实例ID")
    private long game_logic_instance_id;

    public static final int FIELD_start_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "start_ms", comment = "开启时间戳（毫秒）")
    private long start_ms;

    public static final int FIELD_end_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "end_ms", comment = "结束时间戳（毫秒）")
    private long end_ms;

    public static final int FIELD_settle_ms =5;
    @DataBaseField(type = "bigint(20)", fieldname = "settle_ms", comment = "结算时间戳（毫秒）")
    private long settle_ms;

    public static final int FIELD_close_ms =6;
    @DataBaseField(type = "bigint(20)", fieldname = "close_ms", comment = "关闭时间戳（毫秒）")
    private long close_ms;

    public static final int FIELD_cur_state =7;
    @DataBaseField(type = "int(11)", fieldname = "cur_state", comment = "当前状态")
    private int cur_state;

    public static final int FIELD_settled_guild_leaders_set =8;
    @DataBaseField(type = "blob", fieldname = "settled_guild_leaders_set", comment = "结算时当前US联盟盟主CID集合")
    private byte[] settled_guild_leaders_set;

    public static final int FIELD_settled_activity_team_leaders_set =9;
    @DataBaseField(type = "blob", fieldname = "settled_activity_team_leaders_set", comment = "结算时当前US队伍CID集合")
    private byte[] settled_activity_team_leaders_set;

    public ActivityBaseBO() {
        id = 0;
        activity_id = 0L;
        cross_instance_id = 0L;
        game_logic_instance_id = 0L;
        start_ms = 0L;
        end_ms = 0L;
        settle_ms = 0L;
        close_ms = 0L;
        cur_state = 0;
        settled_guild_leaders_set = null;
        settled_activity_team_leaders_set = null;
    }

    public ActivityBaseBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        activity_id = rs.getLong(2);
        cross_instance_id = rs.getLong(3);
        game_logic_instance_id = rs.getLong(4);
        start_ms = rs.getLong(5);
        end_ms = rs.getLong(6);
        settle_ms = rs.getLong(7);
        close_ms = rs.getLong(8);
        cur_state = rs.getInt(9);
        settled_guild_leaders_set = rs.getBytes(10);
        settled_activity_team_leaders_set = rs.getBytes(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ActivityBaseBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `activity_id`, `cross_instance_id`, `game_logic_instance_id`, `start_ms`, `end_ms`, `settle_ms`, `close_ms`, `cur_state`, `settled_guild_leaders_set`, `settled_activity_team_leaders_set`";
    }

    @Override
    public String getTableName() {
        return "`activity_base`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(activity_id).append("', ");
        strBuf.append("'").append(cross_instance_id).append("', ");
        strBuf.append("'").append(game_logic_instance_id).append("', ");
        strBuf.append("'").append(start_ms).append("', ");
        strBuf.append("'").append(end_ms).append("', ");
        strBuf.append("'").append(settle_ms).append("', ");
        strBuf.append("'").append(close_ms).append("', ");
        strBuf.append("'").append(cur_state).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(settled_guild_leaders_set); 
        ret.add(settled_activity_team_leaders_set);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_settled_guild_leaders_set)) ret.add(settled_guild_leaders_set); 
        if(isFieldMarked(FIELD_settled_activity_team_leaders_set)) ret.add(settled_activity_team_leaders_set);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 活动配置ID
    public long getActivityId() { return this.activity_id; }
    public void setActivityId(BM _bm, long activity_id) {
        if(activity_id==this.activity_id) 
            return;
        this.activity_id = activity_id; 
        markField(_bm, FIELD_activity_id); 
    }
    public void saveActivityId(BM _bm, long activity_id) {
        if(activity_id==this.activity_id) 
            return;
        this.activity_id = activity_id;
        saveField(_bm, "activity_id", activity_id);
    }

    // 跨服分组实例ID
    public long getCrossInstanceId() { return this.cross_instance_id; }
    public void setCrossInstanceId(BM _bm, long cross_instance_id) {
        if(cross_instance_id==this.cross_instance_id) 
            return;
        this.cross_instance_id = cross_instance_id; 
        markField(_bm, FIELD_cross_instance_id); 
    }
    public void saveCrossInstanceId(BM _bm, long cross_instance_id) {
        if(cross_instance_id==this.cross_instance_id) 
            return;
        this.cross_instance_id = cross_instance_id;
        saveField(_bm, "cross_instance_id", cross_instance_id);
    }

    // 游戏逻辑主体实例ID
    public long getGameLogicInstanceId() { return this.game_logic_instance_id; }
    public void setGameLogicInstanceId(BM _bm, long game_logic_instance_id) {
        if(game_logic_instance_id==this.game_logic_instance_id) 
            return;
        this.game_logic_instance_id = game_logic_instance_id; 
        markField(_bm, FIELD_game_logic_instance_id); 
    }
    public void saveGameLogicInstanceId(BM _bm, long game_logic_instance_id) {
        if(game_logic_instance_id==this.game_logic_instance_id) 
            return;
        this.game_logic_instance_id = game_logic_instance_id;
        saveField(_bm, "game_logic_instance_id", game_logic_instance_id);
    }

    // 开启时间戳（毫秒）
    public long getStartMs() { return this.start_ms; }
    public void setStartMs(BM _bm, long start_ms) {
        if(start_ms==this.start_ms) 
            return;
        this.start_ms = start_ms; 
        markField(_bm, FIELD_start_ms); 
    }
    public void saveStartMs(BM _bm, long start_ms) {
        if(start_ms==this.start_ms) 
            return;
        this.start_ms = start_ms;
        saveField(_bm, "start_ms", start_ms);
    }

    // 结束时间戳（毫秒）
    public long getEndMs() { return this.end_ms; }
    public void setEndMs(BM _bm, long end_ms) {
        if(end_ms==this.end_ms) 
            return;
        this.end_ms = end_ms; 
        markField(_bm, FIELD_end_ms); 
    }
    public void saveEndMs(BM _bm, long end_ms) {
        if(end_ms==this.end_ms) 
            return;
        this.end_ms = end_ms;
        saveField(_bm, "end_ms", end_ms);
    }

    // 结算时间戳（毫秒）
    public long getSettleMs() { return this.settle_ms; }
    public void setSettleMs(BM _bm, long settle_ms) {
        if(settle_ms==this.settle_ms) 
            return;
        this.settle_ms = settle_ms; 
        markField(_bm, FIELD_settle_ms); 
    }
    public void saveSettleMs(BM _bm, long settle_ms) {
        if(settle_ms==this.settle_ms) 
            return;
        this.settle_ms = settle_ms;
        saveField(_bm, "settle_ms", settle_ms);
    }

    // 关闭时间戳（毫秒）
    public long getCloseMs() { return this.close_ms; }
    public void setCloseMs(BM _bm, long close_ms) {
        if(close_ms==this.close_ms) 
            return;
        this.close_ms = close_ms; 
        markField(_bm, FIELD_close_ms); 
    }
    public void saveCloseMs(BM _bm, long close_ms) {
        if(close_ms==this.close_ms) 
            return;
        this.close_ms = close_ms;
        saveField(_bm, "close_ms", close_ms);
    }

    // 当前状态
    public int getCurState() { return this.cur_state; }
    public void setCurState(BM _bm, int cur_state) {
        if(cur_state==this.cur_state) 
            return;
        this.cur_state = cur_state; 
        markField(_bm, FIELD_cur_state); 
    }
    public void saveCurState(BM _bm, int cur_state) {
        if(cur_state==this.cur_state) 
            return;
        this.cur_state = cur_state;
        saveField(_bm, "cur_state", cur_state);
    }

    // 结算时当前US联盟盟主CID集合
    public byte[] getSettledGuildLeadersSet() { return this.settled_guild_leaders_set; }
    public void setSettledGuildLeadersSet(BM _bm, byte[] settled_guild_leaders_set) {
        if(settled_guild_leaders_set==this.settled_guild_leaders_set) 
            return;
        this.settled_guild_leaders_set = settled_guild_leaders_set; 
        markField(_bm, FIELD_settled_guild_leaders_set); 
    }
    public void saveSettledGuildLeadersSet(BM _bm, byte[] settled_guild_leaders_set) {
        if(settled_guild_leaders_set==this.settled_guild_leaders_set) 
            return;
        this.settled_guild_leaders_set = settled_guild_leaders_set;
        saveFieldBytes(_bm, "settled_guild_leaders_set", settled_guild_leaders_set);
    }

    // 结算时当前US队伍CID集合
    public byte[] getSettledActivityTeamLeadersSet() { return this.settled_activity_team_leaders_set; }
    public void setSettledActivityTeamLeadersSet(BM _bm, byte[] settled_activity_team_leaders_set) {
        if(settled_activity_team_leaders_set==this.settled_activity_team_leaders_set) 
            return;
        this.settled_activity_team_leaders_set = settled_activity_team_leaders_set; 
        markField(_bm, FIELD_settled_activity_team_leaders_set); 
    }
    public void saveSettledActivityTeamLeadersSet(BM _bm, byte[] settled_activity_team_leaders_set) {
        if(settled_activity_team_leaders_set==this.settled_activity_team_leaders_set) 
            return;
        this.settled_activity_team_leaders_set = settled_activity_team_leaders_set;
        saveFieldBytes(_bm, "settled_activity_team_leaders_set", settled_activity_team_leaders_set);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `activity_id` = '").append(activity_id).append("',");
        sBuilder.append(" `cross_instance_id` = '").append(cross_instance_id).append("',");
        sBuilder.append(" `game_logic_instance_id` = '").append(game_logic_instance_id).append("',");
        sBuilder.append(" `start_ms` = '").append(start_ms).append("',");
        sBuilder.append(" `end_ms` = '").append(end_ms).append("',");
        sBuilder.append(" `settle_ms` = '").append(settle_ms).append("',");
        sBuilder.append(" `close_ms` = '").append(close_ms).append("',");
        sBuilder.append(" `cur_state` = '").append(cur_state).append("',");
        sBuilder.append(" `settled_guild_leaders_set` = ?,");
        sBuilder.append(" `settled_activity_team_leaders_set` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_activity_id)) sBuilder.append(" `activity_id` = '").append(activity_id).append("',");
        if(isFieldMarked(FIELD_cross_instance_id)) sBuilder.append(" `cross_instance_id` = '").append(cross_instance_id).append("',");
        if(isFieldMarked(FIELD_game_logic_instance_id)) sBuilder.append(" `game_logic_instance_id` = '").append(game_logic_instance_id).append("',");
        if(isFieldMarked(FIELD_start_ms)) sBuilder.append(" `start_ms` = '").append(start_ms).append("',");
        if(isFieldMarked(FIELD_end_ms)) sBuilder.append(" `end_ms` = '").append(end_ms).append("',");
        if(isFieldMarked(FIELD_settle_ms)) sBuilder.append(" `settle_ms` = '").append(settle_ms).append("',");
        if(isFieldMarked(FIELD_close_ms)) sBuilder.append(" `close_ms` = '").append(close_ms).append("',");
        if(isFieldMarked(FIELD_cur_state)) sBuilder.append(" `cur_state` = '").append(cur_state).append("',");
        if(isFieldMarked(FIELD_settled_guild_leaders_set)) sBuilder.append(" `settled_guild_leaders_set` = ?,");
        if(isFieldMarked(FIELD_settled_activity_team_leaders_set)) sBuilder.append(" `settled_activity_team_leaders_set` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `activity_base` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`activity_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动配置ID',"
                + "`cross_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服分组实例ID',"
                + "`game_logic_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '游戏逻辑主体实例ID',"
                + "`start_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '开启时间戳（毫秒）',"
                + "`end_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '结束时间戳（毫秒）',"
                + "`settle_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '结算时间戳（毫秒）',"
                + "`close_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '关闭时间戳（毫秒）',"
                + "`cur_state` int(11) NOT NULL DEFAULT '0' COMMENT '当前状态',"
                + "`settled_guild_leaders_set` blob NULL COMMENT '结算时当前US联盟盟主CID集合',"
                + "`settled_activity_team_leaders_set` blob NULL COMMENT '结算时当前US队伍CID集合',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动基础数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//activity_id
        _size+=8;//cross_instance_id
        _size+=8;//game_logic_instance_id
        _size+=8;//start_ms
        _size+=8;//end_ms
        _size+=8;//settle_ms
        _size+=8;//close_ms
        _size+=4;//cur_state
        _size+=2;_size+=settled_guild_leaders_set.length;//settled_guild_leaders_set
        _size+=2;_size+=settled_activity_team_leaders_set.length;//settled_activity_team_leaders_set
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(activity_id);
        buff.putLong(cross_instance_id);
        buff.putLong(game_logic_instance_id);
        buff.putLong(start_ms);
        buff.putLong(end_ms);
        buff.putLong(settle_ms);
        buff.putLong(close_ms);
        buff.putInt(cur_state);
        buff.putShort((short)(settled_guild_leaders_set == null ? 0 : settled_guild_leaders_set.length));if(null != settled_guild_leaders_set){buff.put(settled_guild_leaders_set);}
        buff.putShort((short)(settled_activity_team_leaders_set == null ? 0 : settled_activity_team_leaders_set.length));if(null != settled_activity_team_leaders_set){buff.put(settled_activity_team_leaders_set);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        activity_id=buff.getLong();
        cross_instance_id=buff.getLong();
        game_logic_instance_id=buff.getLong();
        start_ms=buff.getLong();
        end_ms=buff.getLong();
        settle_ms=buff.getLong();
        close_ms=buff.getLong();
        cur_state=buff.getInt();
        int settled_guild_leaders_set_count = buff.getShort();if(settled_guild_leaders_set_count>0){settled_guild_leaders_set = new byte[settled_guild_leaders_set_count];buff.get(settled_guild_leaders_set);}
        int settled_activity_team_leaders_set_count = buff.getShort();if(settled_activity_team_leaders_set_count>0){settled_activity_team_leaders_set = new byte[settled_activity_team_leaders_set_count];buff.get(settled_activity_team_leaders_set);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
