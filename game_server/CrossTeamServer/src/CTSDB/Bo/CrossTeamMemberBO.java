package CTSDB.Bo;
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
public class CrossTeamMemberBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_team_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "team_id", comment = "队伍实例ID")
    private long team_id;

    public static final int FIELD_member_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "member_cid", comment = "队伍成员CID")
    private long member_cid;

    public static final int FIELD_member_pos =2;
    @DataBaseField(type = "int(11)", fieldname = "member_pos", comment = "队伍成员职位")
    private int member_pos;

    public static final int FIELD_join_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "join_ms", comment = "加入队伍时间（毫秒）")
    private long join_ms;

    public CrossTeamMemberBO() {
        id = 0;
        team_id = 0L;
        member_cid = 0L;
        member_pos = 0;
        join_ms = 0L;
    }

    public CrossTeamMemberBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        team_id = rs.getLong(2);
        member_cid = rs.getLong(3);
        member_pos = rs.getInt(4);
        join_ms = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new CrossTeamMemberBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `team_id`, `member_cid`, `member_pos`, `join_ms`";
    }

    @Override
    public String getTableName() {
        return "`cross_team_member`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(team_id).append("', ");
        strBuf.append("'").append(member_cid).append("', ");
        strBuf.append("'").append(member_pos).append("', ");
        strBuf.append("'").append(join_ms).append("', ");
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

    // 队伍实例ID
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

    // 队伍成员CID
    public long getMemberCid() { return this.member_cid; }
    public void setMemberCid(BM _bm, long member_cid) {
        if(member_cid==this.member_cid) 
            return;
        this.member_cid = member_cid; 
        markField(_bm, FIELD_member_cid); 
    }
    public void saveMemberCid(BM _bm, long member_cid) {
        if(member_cid==this.member_cid) 
            return;
        this.member_cid = member_cid;
        saveField(_bm, "member_cid", member_cid);
    }

    // 队伍成员职位
    public int getMemberPos() { return this.member_pos; }
    public void setMemberPos(BM _bm, int member_pos) {
        if(member_pos==this.member_pos) 
            return;
        this.member_pos = member_pos; 
        markField(_bm, FIELD_member_pos); 
    }
    public void saveMemberPos(BM _bm, int member_pos) {
        if(member_pos==this.member_pos) 
            return;
        this.member_pos = member_pos;
        saveField(_bm, "member_pos", member_pos);
    }

    // 加入队伍时间（毫秒）
    public long getJoinMs() { return this.join_ms; }
    public void setJoinMs(BM _bm, long join_ms) {
        if(join_ms==this.join_ms) 
            return;
        this.join_ms = join_ms; 
        markField(_bm, FIELD_join_ms); 
    }
    public void saveJoinMs(BM _bm, long join_ms) {
        if(join_ms==this.join_ms) 
            return;
        this.join_ms = join_ms;
        saveField(_bm, "join_ms", join_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `team_id` = '").append(team_id).append("',");
        sBuilder.append(" `member_cid` = '").append(member_cid).append("',");
        sBuilder.append(" `member_pos` = '").append(member_pos).append("',");
        sBuilder.append(" `join_ms` = '").append(join_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_team_id)) sBuilder.append(" `team_id` = '").append(team_id).append("',");
        if(isFieldMarked(FIELD_member_cid)) sBuilder.append(" `member_cid` = '").append(member_cid).append("',");
        if(isFieldMarked(FIELD_member_pos)) sBuilder.append(" `member_pos` = '").append(member_pos).append("',");
        if(isFieldMarked(FIELD_join_ms)) sBuilder.append(" `join_ms` = '").append(join_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `cross_team_member` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`team_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '队伍实例ID',"
                + "`member_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '队伍成员CID',"
                + "`member_pos` int(11) NOT NULL DEFAULT '0' COMMENT '队伍成员职位',"
                + "`join_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '加入队伍时间（毫秒）',"
                + "KEY `team_id` (`team_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='跨服队伍成员数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.crossteam_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//team_id
        _size+=8;//member_cid
        _size+=4;//member_pos
        _size+=8;//join_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(team_id);
        buff.putLong(member_cid);
        buff.putInt(member_pos);
        buff.putLong(join_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        team_id=buff.getLong();
        member_cid=buff.getLong();
        member_pos=buff.getInt();
        join_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
