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
public class GuildRallyMemberBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟ID")
    private long guild_id;

    public static final int FIELD_rally_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "rally_id", comment = "集结ID")
    private long rally_id;

    public static final int FIELD_cid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "成员CID")
    private long cid;

    public static final int FIELD_team_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "team_id", comment = "成员队伍ID")
    private long team_id;

    public static final int FIELD_join_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "join_time_ms", comment = "加入时间(毫秒)")
    private long join_time_ms;

    public static final int FIELD_is_ready =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_ready", comment = "是否准备完成")
    private boolean is_ready;

    public static final int FIELD_team_snapshot =6;
    @DataBaseField(type = "blob", fieldname = "team_snapshot", comment = "成员队伍快照")
    private byte[] team_snapshot;

    public GuildRallyMemberBO() {
        id = 0;
        guild_id = 0L;
        rally_id = 0L;
        cid = 0L;
        team_id = 0L;
        join_time_ms = 0L;
        is_ready = false;
        team_snapshot = null;
    }

    public GuildRallyMemberBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        rally_id = rs.getLong(3);
        cid = rs.getLong(4);
        team_id = rs.getLong(5);
        join_time_ms = rs.getLong(6);
        is_ready = rs.getBoolean(7);
        team_snapshot = rs.getBytes(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildRallyMemberBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `rally_id`, `cid`, `team_id`, `join_time_ms`, `is_ready`, `team_snapshot`";
    }

    @Override
    public String getTableName() {
        return "`guild_rally_member`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(rally_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(team_id).append("', ");
        strBuf.append("'").append(join_time_ms).append("', ");
        strBuf.append("'").append(is_ready ? 1 : 0).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(team_snapshot);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_team_snapshot)) ret.add(team_snapshot);         return ret;
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

    // 集结ID
    public long getRallyId() { return this.rally_id; }
    public void setRallyId(BM _bm, long rally_id) {
        if(rally_id==this.rally_id) 
            return;
        this.rally_id = rally_id; 
        markField(_bm, FIELD_rally_id); 
    }
    public void saveRallyId(BM _bm, long rally_id) {
        if(rally_id==this.rally_id) 
            return;
        this.rally_id = rally_id;
        saveField(_bm, "rally_id", rally_id);
    }

    // 成员CID
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

    // 成员队伍ID
    public long getTeamId() { return this.team_id; }
    public void setTeamId(BM _bm, long team_id) {
        if(team_id==this.team_id) 
            return;
        this.team_id = team_id; 
        markField(_bm, FIELD_team_id); 
    }
    public void saveTeamId(BM _bm, long team_id) {
        if(team_id==this.team_id) 
            return;
        this.team_id = team_id;
        saveField(_bm, "team_id", team_id);
    }

    // 加入时间(毫秒)
    public long getJoinTimeMs() { return this.join_time_ms; }
    public void setJoinTimeMs(BM _bm, long join_time_ms) {
        if(join_time_ms==this.join_time_ms) 
            return;
        this.join_time_ms = join_time_ms; 
        markField(_bm, FIELD_join_time_ms); 
    }
    public void saveJoinTimeMs(BM _bm, long join_time_ms) {
        if(join_time_ms==this.join_time_ms) 
            return;
        this.join_time_ms = join_time_ms;
        saveField(_bm, "join_time_ms", join_time_ms);
    }

    // 是否准备完成
    public boolean getIsReady() { return this.is_ready; }
    public void setIsReady(BM _bm, boolean is_ready) {
        if(is_ready==this.is_ready) 
            return;
        this.is_ready = is_ready; 
        markField(_bm, FIELD_is_ready); 
    }
    public void saveIsReady(BM _bm, boolean is_ready) {
        if(is_ready==this.is_ready) 
            return;
        this.is_ready = is_ready;
        saveField(_bm, "is_ready", is_ready ? 1 : 0);
    }

    // 成员队伍快照
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `rally_id` = '").append(rally_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `team_id` = '").append(team_id).append("',");
        sBuilder.append(" `join_time_ms` = '").append(join_time_ms).append("',");
        sBuilder.append(" `is_ready` = '").append(is_ready ? 1 : 0).append("',");
        sBuilder.append(" `team_snapshot` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_rally_id)) sBuilder.append(" `rally_id` = '").append(rally_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_team_id)) sBuilder.append(" `team_id` = '").append(team_id).append("',");
        if(isFieldMarked(FIELD_join_time_ms)) sBuilder.append(" `join_time_ms` = '").append(join_time_ms).append("',");
        if(isFieldMarked(FIELD_is_ready)) sBuilder.append(" `is_ready` = '").append(is_ready ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_team_snapshot)) sBuilder.append(" `team_snapshot` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_rally_member` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`rally_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '集结ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '成员CID',"
                + "`team_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '成员队伍ID',"
                + "`join_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '加入时间(毫秒)',"
                + "`is_ready` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否准备完成',"
                + "`team_snapshot` blob NULL COMMENT '成员队伍快照',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟集结成员数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//rally_id
        _size+=8;//cid
        _size+=8;//team_id
        _size+=8;//join_time_ms
        _size+=1;//is_ready
        _size+=2;_size+=team_snapshot.length;//team_snapshot
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(rally_id);
        buff.putLong(cid);
        buff.putLong(team_id);
        buff.putLong(join_time_ms);
        buff.put((byte)(is_ready?1:0));
        buff.putShort((short)(team_snapshot == null ? 0 : team_snapshot.length));if(null != team_snapshot){buff.put(team_snapshot);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        rally_id=buff.getLong();
        cid=buff.getLong();
        team_id=buff.getLong();
        join_time_ms=buff.getLong();
        is_ready=(buff.get()==1);
        int team_snapshot_count = buff.getShort();if(team_snapshot_count>0){team_snapshot = new byte[team_snapshot_count];buff.get(team_snapshot);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
