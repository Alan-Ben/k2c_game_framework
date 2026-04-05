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
public class GuildCooperatePointInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "公会ID")
    private long guild_id;

    public static final int FIELD_area_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "area_id", comment = "区域ID")
    private long area_id;

    public static final int FIELD_reward_point_index =2;
    @DataBaseField(type = "int(11)", fieldname = "reward_point_index", comment = "奖励据点索引")
    private int reward_point_index;

    public static final int FIELD_is_unlock =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_unlock", comment = "是否解锁")
    private boolean is_unlock;

    public static final int FIELD_leader_cid =4;
    @DataBaseField(type = "bigint(20)", fieldname = "leader_cid", comment = "盟主CID")
    private long leader_cid;

    public static final int FIELD_damage_list =5;
    @DataBaseField(type = "varchar(1024)", fieldname = "damage_list", comment = "已造成伤害列表（分号分隔：1100;1200;5000）")
    private String damage_list;

    public static final int FIELD_had_draw_guild_reward =6;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_draw_guild_reward", comment = "公会奖励是否已领取")
    private boolean had_draw_guild_reward;

    public GuildCooperatePointInfoBO() {
        id = 0;
        guild_id = 0L;
        area_id = 0L;
        reward_point_index = 0;
        is_unlock = false;
        leader_cid = 0L;
        damage_list = "";
        had_draw_guild_reward = false;
    }

    public GuildCooperatePointInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        area_id = rs.getLong(3);
        reward_point_index = rs.getInt(4);
        is_unlock = rs.getBoolean(5);
        leader_cid = rs.getLong(6);
        damage_list = rs.getString(7);
        had_draw_guild_reward = rs.getBoolean(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildCooperatePointInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `area_id`, `reward_point_index`, `is_unlock`, `leader_cid`, `damage_list`, `had_draw_guild_reward`";
    }

    @Override
    public String getTableName() {
        return "`guild_cooperate_point_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(area_id).append("', ");
        strBuf.append("'").append(reward_point_index).append("', ");
        strBuf.append("'").append(is_unlock ? 1 : 0).append("', ");
        strBuf.append("'").append(leader_cid).append("', ");
        strBuf.append("'").append(damage_list == null ? null : damage_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(had_draw_guild_reward ? 1 : 0).append("', ");
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

    // 公会ID
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
    }

    // 区域ID
    public long getAreaId() { return this.area_id; }
    public void setAreaId(BM _bm, long area_id) {
        if(area_id==this.area_id) 
            return;
        this.area_id = area_id; 
        markField(_bm, FIELD_area_id); 
    }
    public void saveAreaId(BM _bm, long area_id) {
        if(area_id==this.area_id) 
            return;
        this.area_id = area_id;
        saveField(_bm, "area_id", area_id);
    }

    // 奖励据点索引
    public int getRewardPointIndex() { return this.reward_point_index; }
    public void setRewardPointIndex(BM _bm, int reward_point_index) {
        if(reward_point_index==this.reward_point_index) 
            return;
        this.reward_point_index = reward_point_index; 
        markField(_bm, FIELD_reward_point_index); 
    }
    public void saveRewardPointIndex(BM _bm, int reward_point_index) {
        if(reward_point_index==this.reward_point_index) 
            return;
        this.reward_point_index = reward_point_index;
        saveField(_bm, "reward_point_index", reward_point_index);
    }

    // 是否解锁
    public boolean getIsUnlock() { return this.is_unlock; }
    public void setIsUnlock(BM _bm, boolean is_unlock) {
        if(is_unlock==this.is_unlock) 
            return;
        this.is_unlock = is_unlock; 
        markField(_bm, FIELD_is_unlock); 
    }
    public void saveIsUnlock(BM _bm, boolean is_unlock) {
        if(is_unlock==this.is_unlock) 
            return;
        this.is_unlock = is_unlock;
        saveField(_bm, "is_unlock", is_unlock ? 1 : 0);
    }

    // 盟主CID
    public long getLeaderCid() { return this.leader_cid; }
    public void setLeaderCid(BM _bm, long leader_cid) {
        if(leader_cid==this.leader_cid) 
            return;
        this.leader_cid = leader_cid; 
        markField(_bm, FIELD_leader_cid); 
    }
    public void saveLeaderCid(BM _bm, long leader_cid) {
        if(leader_cid==this.leader_cid) 
            return;
        this.leader_cid = leader_cid;
        saveField(_bm, "leader_cid", leader_cid);
    }

    // 已造成伤害列表（分号分隔：1100;1200;5000）
    public String getDamageList() { return this.damage_list; }
    public void setDamageList(BM _bm, String damage_list) {
        if(damage_list.equals(this.damage_list)) 
            return;
        this.damage_list = damage_list; 
        markField(_bm, FIELD_damage_list); 
    }
    public void saveDamageList(BM _bm, String damage_list) {
        if(damage_list.equals(this.damage_list)) 
            return;
        this.damage_list = damage_list;
        saveField(_bm, "damage_list", damage_list);
    }

    // 公会奖励是否已领取
    public boolean getHadDrawGuildReward() { return this.had_draw_guild_reward; }
    public void setHadDrawGuildReward(BM _bm, boolean had_draw_guild_reward) {
        if(had_draw_guild_reward==this.had_draw_guild_reward) 
            return;
        this.had_draw_guild_reward = had_draw_guild_reward; 
        markField(_bm, FIELD_had_draw_guild_reward); 
    }
    public void saveHadDrawGuildReward(BM _bm, boolean had_draw_guild_reward) {
        if(had_draw_guild_reward==this.had_draw_guild_reward) 
            return;
        this.had_draw_guild_reward = had_draw_guild_reward;
        saveField(_bm, "had_draw_guild_reward", had_draw_guild_reward ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `area_id` = '").append(area_id).append("',");
        sBuilder.append(" `reward_point_index` = '").append(reward_point_index).append("',");
        sBuilder.append(" `is_unlock` = '").append(is_unlock ? 1 : 0).append("',");
        sBuilder.append(" `leader_cid` = '").append(leader_cid).append("',");
        sBuilder.append(" `damage_list` = '").append(damage_list == null ? null : damage_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `had_draw_guild_reward` = '").append(had_draw_guild_reward ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_area_id)) sBuilder.append(" `area_id` = '").append(area_id).append("',");
        if(isFieldMarked(FIELD_reward_point_index)) sBuilder.append(" `reward_point_index` = '").append(reward_point_index).append("',");
        if(isFieldMarked(FIELD_is_unlock)) sBuilder.append(" `is_unlock` = '").append(is_unlock ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_leader_cid)) sBuilder.append(" `leader_cid` = '").append(leader_cid).append("',");
        if(isFieldMarked(FIELD_damage_list)) sBuilder.append(" `damage_list` = '").append(damage_list == null ? null : damage_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_had_draw_guild_reward)) sBuilder.append(" `had_draw_guild_reward` = '").append(had_draw_guild_reward ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_cooperate_point_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '公会ID',"
                + "`area_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '区域ID',"
                + "`reward_point_index` int(11) NOT NULL DEFAULT '0' COMMENT '奖励据点索引',"
                + "`is_unlock` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否解锁',"
                + "`leader_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '盟主CID',"
                + "`damage_list` varchar(1024) NOT NULL DEFAULT '' COMMENT '已造成伤害列表（分号分隔：1100;1200;5000）',"
                + "`had_draw_guild_reward` tinyint(1) NOT NULL DEFAULT '0' COMMENT '公会奖励是否已领取',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='公会协作据点信息表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guild_id
        _size+=8;//area_id
        _size+=4;//reward_point_index
        _size+=1;//is_unlock
        _size+=8;//leader_cid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(damage_list);//damage_list
        _size+=1;//had_draw_guild_reward
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(area_id);
        buff.putInt(reward_point_index);
        buff.put((byte)(is_unlock?1:0));
        buff.putLong(leader_cid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, damage_list);
        buff.put((byte)(had_draw_guild_reward?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        area_id=buff.getLong();
        reward_point_index=buff.getInt();
        is_unlock=(buff.get()==1);
        leader_cid=buff.getLong();
        damage_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        had_draw_guild_reward=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
