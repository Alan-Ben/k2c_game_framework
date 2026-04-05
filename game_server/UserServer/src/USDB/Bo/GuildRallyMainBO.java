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

@RefBo(isIdAuto = true)
public class GuildRallyMainBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟ID")
    private long guild_id;

    public static final int FIELD_rally_type =1;
    @DataBaseField(type = "int(11)", fieldname = "rally_type", comment = "集结类型")
    private int rally_type;

    public static final int FIELD_leader_cid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "leader_cid", comment = "发起者CID")
    private long leader_cid;

    public static final int FIELD_leader_team_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "leader_team_id", comment = "发起者队伍ID")
    private long leader_team_id;

    public static final int FIELD_create_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "create_time_ms", comment = "创建时间(毫秒)")
    private long create_time_ms;

    public static final int FIELD_expire_time_ms =5;
    @DataBaseField(type = "bigint(20)", fieldname = "expire_time_ms", comment = "过期时间(毫秒)")
    private long expire_time_ms;

    public static final int FIELD_min_power_limit =6;
    @DataBaseField(type = "bigint(20)", fieldname = "min_power_limit", comment = "最低战力限制")
    private long min_power_limit;

    public static final int FIELD_max_member_limit =7;
    @DataBaseField(type = "int(11)", fieldname = "max_member_limit", comment = "最大参与人数")
    private int max_member_limit;

    public static final int FIELD_team_snapshot =8;
    @DataBaseField(type = "blob", fieldname = "team_snapshot", comment = "发起者队伍快照")
    private byte[] team_snapshot;

    public static final int FIELD_ext_data =9;
    @DataBaseField(type = "blob", fieldname = "ext_data", comment = "扩展数据")
    private byte[] ext_data;

    public GuildRallyMainBO() {
        id = 0;
        guild_id = 0L;
        rally_type = 0;
        leader_cid = 0L;
        leader_team_id = 0L;
        create_time_ms = 0L;
        expire_time_ms = 0L;
        min_power_limit = 0L;
        max_member_limit = 0;
        team_snapshot = null;
        ext_data = null;
    }

    public GuildRallyMainBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        rally_type = rs.getInt(3);
        leader_cid = rs.getLong(4);
        leader_team_id = rs.getLong(5);
        create_time_ms = rs.getLong(6);
        expire_time_ms = rs.getLong(7);
        min_power_limit = rs.getLong(8);
        max_member_limit = rs.getInt(9);
        team_snapshot = rs.getBytes(10);
        ext_data = rs.getBytes(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildRallyMainBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `rally_type`, `leader_cid`, `leader_team_id`, `create_time_ms`, `expire_time_ms`, `min_power_limit`, `max_member_limit`, `team_snapshot`, `ext_data`";
    }

    @Override
    public String getTableName() {
        return "`guild_rally_main`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(rally_type).append("', ");
        strBuf.append("'").append(leader_cid).append("', ");
        strBuf.append("'").append(leader_team_id).append("', ");
        strBuf.append("'").append(create_time_ms).append("', ");
        strBuf.append("'").append(expire_time_ms).append("', ");
        strBuf.append("'").append(min_power_limit).append("', ");
        strBuf.append("'").append(max_member_limit).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(team_snapshot); 
        ret.add(ext_data);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_team_snapshot)) ret.add(team_snapshot); 
        if(isFieldMarked(FIELD_ext_data)) ret.add(ext_data);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 联盟ID
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

    // 集结类型
    public int getRallyType() { return this.rally_type; }
    public void setRallyType(BM _bm, int rally_type) {
        if(rally_type==this.rally_type) 
            return;
        this.rally_type = rally_type; 
        markField(_bm, FIELD_rally_type); 
    }
    public void saveRallyType(BM _bm, int rally_type) {
        if(rally_type==this.rally_type) 
            return;
        this.rally_type = rally_type;
        saveField(_bm, "rally_type", rally_type);
    }

    // 发起者CID
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

    // 发起者队伍ID
    public long getLeaderTeamId() { return this.leader_team_id; }
    public void setLeaderTeamId(BM _bm, long leader_team_id) {
        if(leader_team_id==this.leader_team_id) 
            return;
        this.leader_team_id = leader_team_id; 
        markField(_bm, FIELD_leader_team_id); 
    }
    public void saveLeaderTeamId(BM _bm, long leader_team_id) {
        if(leader_team_id==this.leader_team_id) 
            return;
        this.leader_team_id = leader_team_id;
        saveField(_bm, "leader_team_id", leader_team_id);
    }

    // 创建时间(毫秒)
    public long getCreateTimeMs() { return this.create_time_ms; }
    public void setCreateTimeMs(BM _bm, long create_time_ms) {
        if(create_time_ms==this.create_time_ms) 
            return;
        this.create_time_ms = create_time_ms; 
        markField(_bm, FIELD_create_time_ms); 
    }
    public void saveCreateTimeMs(BM _bm, long create_time_ms) {
        if(create_time_ms==this.create_time_ms) 
            return;
        this.create_time_ms = create_time_ms;
        saveField(_bm, "create_time_ms", create_time_ms);
    }

    // 过期时间(毫秒)
    public long getExpireTimeMs() { return this.expire_time_ms; }
    public void setExpireTimeMs(BM _bm, long expire_time_ms) {
        if(expire_time_ms==this.expire_time_ms) 
            return;
        this.expire_time_ms = expire_time_ms; 
        markField(_bm, FIELD_expire_time_ms); 
    }
    public void saveExpireTimeMs(BM _bm, long expire_time_ms) {
        if(expire_time_ms==this.expire_time_ms) 
            return;
        this.expire_time_ms = expire_time_ms;
        saveField(_bm, "expire_time_ms", expire_time_ms);
    }

    // 最低战力限制
    public long getMinPowerLimit() { return this.min_power_limit; }
    public void setMinPowerLimit(BM _bm, long min_power_limit) {
        if(min_power_limit==this.min_power_limit) 
            return;
        this.min_power_limit = min_power_limit; 
        markField(_bm, FIELD_min_power_limit); 
    }
    public void saveMinPowerLimit(BM _bm, long min_power_limit) {
        if(min_power_limit==this.min_power_limit) 
            return;
        this.min_power_limit = min_power_limit;
        saveField(_bm, "min_power_limit", min_power_limit);
    }

    // 最大参与人数
    public int getMaxMemberLimit() { return this.max_member_limit; }
    public void setMaxMemberLimit(BM _bm, int max_member_limit) {
        if(max_member_limit==this.max_member_limit) 
            return;
        this.max_member_limit = max_member_limit; 
        markField(_bm, FIELD_max_member_limit); 
    }
    public void saveMaxMemberLimit(BM _bm, int max_member_limit) {
        if(max_member_limit==this.max_member_limit) 
            return;
        this.max_member_limit = max_member_limit;
        saveField(_bm, "max_member_limit", max_member_limit);
    }

    // 发起者队伍快照
    public byte[] getTeamSnapshot() { return this.team_snapshot; }
    public void setTeamSnapshot(BM _bm, byte[] team_snapshot) {
        if(team_snapshot==this.team_snapshot) 
            return;
        this.team_snapshot = team_snapshot; 
        markField(_bm, FIELD_team_snapshot); 
    }
    public void saveTeamSnapshot(BM _bm, byte[] team_snapshot) {
        if(team_snapshot==this.team_snapshot) 
            return;
        this.team_snapshot = team_snapshot;
        saveFieldBytes(_bm, "team_snapshot", team_snapshot);
    }

    // 扩展数据
    public byte[] getExtData() { return this.ext_data; }
    public void setExtData(BM _bm, byte[] ext_data) {
        if(ext_data==this.ext_data) 
            return;
        this.ext_data = ext_data; 
        markField(_bm, FIELD_ext_data); 
    }
    public void saveExtData(BM _bm, byte[] ext_data) {
        if(ext_data==this.ext_data) 
            return;
        this.ext_data = ext_data;
        saveFieldBytes(_bm, "ext_data", ext_data);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `rally_type` = '").append(rally_type).append("',");
        sBuilder.append(" `leader_cid` = '").append(leader_cid).append("',");
        sBuilder.append(" `leader_team_id` = '").append(leader_team_id).append("',");
        sBuilder.append(" `create_time_ms` = '").append(create_time_ms).append("',");
        sBuilder.append(" `expire_time_ms` = '").append(expire_time_ms).append("',");
        sBuilder.append(" `min_power_limit` = '").append(min_power_limit).append("',");
        sBuilder.append(" `max_member_limit` = '").append(max_member_limit).append("',");
        sBuilder.append(" `team_snapshot` = ?,");
        sBuilder.append(" `ext_data` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_rally_type)) sBuilder.append(" `rally_type` = '").append(rally_type).append("',");
        if(isFieldMarked(FIELD_leader_cid)) sBuilder.append(" `leader_cid` = '").append(leader_cid).append("',");
        if(isFieldMarked(FIELD_leader_team_id)) sBuilder.append(" `leader_team_id` = '").append(leader_team_id).append("',");
        if(isFieldMarked(FIELD_create_time_ms)) sBuilder.append(" `create_time_ms` = '").append(create_time_ms).append("',");
        if(isFieldMarked(FIELD_expire_time_ms)) sBuilder.append(" `expire_time_ms` = '").append(expire_time_ms).append("',");
        if(isFieldMarked(FIELD_min_power_limit)) sBuilder.append(" `min_power_limit` = '").append(min_power_limit).append("',");
        if(isFieldMarked(FIELD_max_member_limit)) sBuilder.append(" `max_member_limit` = '").append(max_member_limit).append("',");
        if(isFieldMarked(FIELD_team_snapshot)) sBuilder.append(" `team_snapshot` = ?,");
        if(isFieldMarked(FIELD_ext_data)) sBuilder.append(" `ext_data` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_rally_main` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`rally_type` int(11) NOT NULL DEFAULT '0' COMMENT '集结类型',"
                + "`leader_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '发起者CID',"
                + "`leader_team_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '发起者队伍ID',"
                + "`create_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '创建时间(毫秒)',"
                + "`expire_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '过期时间(毫秒)',"
                + "`min_power_limit` bigint(20) NOT NULL DEFAULT '0' COMMENT '最低战力限制',"
                + "`max_member_limit` int(11) NOT NULL DEFAULT '0' COMMENT '最大参与人数',"
                + "`team_snapshot` blob NULL COMMENT '发起者队伍快照',"
                + "`ext_data` blob NULL COMMENT '扩展数据',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟集结主数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//rally_type
        _size+=8;//leader_cid
        _size+=8;//leader_team_id
        _size+=8;//create_time_ms
        _size+=8;//expire_time_ms
        _size+=8;//min_power_limit
        _size+=4;//max_member_limit
        _size+=2;_size+=team_snapshot.length;//team_snapshot
        _size+=2;_size+=ext_data.length;//ext_data
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putInt(rally_type);
        buff.putLong(leader_cid);
        buff.putLong(leader_team_id);
        buff.putLong(create_time_ms);
        buff.putLong(expire_time_ms);
        buff.putLong(min_power_limit);
        buff.putInt(max_member_limit);
        buff.putShort((short)(team_snapshot == null ? 0 : team_snapshot.length));if(null != team_snapshot){buff.put(team_snapshot);}
        buff.putShort((short)(ext_data == null ? 0 : ext_data.length));if(null != ext_data){buff.put(ext_data);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        rally_type=buff.getInt();
        leader_cid=buff.getLong();
        leader_team_id=buff.getLong();
        create_time_ms=buff.getLong();
        expire_time_ms=buff.getLong();
        min_power_limit=buff.getLong();
        max_member_limit=buff.getInt();
        int team_snapshot_count = buff.getShort();if(team_snapshot_count>0){team_snapshot = new byte[team_snapshot_count];buff.get(team_snapshot);}
        int ext_data_count = buff.getShort();if(ext_data_count>0){ext_data = new byte[ext_data_count];buff.get(ext_data);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
