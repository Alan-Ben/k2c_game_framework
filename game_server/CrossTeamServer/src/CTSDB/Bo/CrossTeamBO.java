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
public class CrossTeamBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_team_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "team_id", comment = "队伍实例ID")
    private long team_id;

    public static final int FIELD_member_limit =1;
    @DataBaseField(type = "int(11)", fieldname = "member_limit", comment = "队伍成员上限")
    private int member_limit;

    public static final int FIELD_join_type =2;
    @DataBaseField(type = "int(11)", fieldname = "join_type", comment = "加入方式（ENPCrossTeamJoinType枚举序号）")
    private int join_type;

    public static final int FIELD_join_cond_type =3;
    @DataBaseField(type = "int(11)", fieldname = "join_cond_type", comment = "加入条件类型（ENPCrossTeamApplyCond枚举序号）")
    private int join_cond_type;

    public static final int FIELD_join_cond_value =4;
    @DataBaseField(type = "bigint(20)", fieldname = "join_cond_value", comment = "加入条件数值")
    private long join_cond_value;

    public static final int FIELD_team_name =5;
    @DataBaseField(type = "varchar(500)", fieldname = "team_name", comment = "队伍名称")
    private String team_name;

    public static final int FIELD_team_dec =6;
    @DataBaseField(type = "varchar(1024)", fieldname = "team_dec", comment = "队伍宣言")
    private String team_dec;

    public CrossTeamBO() {
        id = 0;
        team_id = 0L;
        member_limit = 0;
        join_type = 0;
        join_cond_type = 0;
        join_cond_value = 0L;
        team_name = "";
        team_dec = "";
    }

    public CrossTeamBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        team_id = rs.getLong(2);
        member_limit = rs.getInt(3);
        join_type = rs.getInt(4);
        join_cond_type = rs.getInt(5);
        join_cond_value = rs.getLong(6);
        team_name = rs.getString(7);
        team_dec = rs.getString(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new CrossTeamBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `team_id`, `member_limit`, `join_type`, `join_cond_type`, `join_cond_value`, `team_name`, `team_dec`";
    }

    @Override
    public String getTableName() {
        return "`cross_team`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(team_id).append("', ");
        strBuf.append("'").append(member_limit).append("', ");
        strBuf.append("'").append(join_type).append("', ");
        strBuf.append("'").append(join_cond_type).append("', ");
        strBuf.append("'").append(join_cond_value).append("', ");
        strBuf.append("'").append(team_name == null ? null : team_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(team_dec == null ? null : team_dec.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 队伍成员上限
    public int getMemberLimit() { return this.member_limit; }
    public void setMemberLimit(BM _bm, int member_limit) {
        if(member_limit==this.member_limit) 
            return;
        this.member_limit = member_limit; 
        markField(_bm, FIELD_member_limit); 
    }
    public void saveMemberLimit(BM _bm, int member_limit) {
        if(member_limit==this.member_limit) 
            return;
        this.member_limit = member_limit;
        saveField(_bm, "member_limit", member_limit);
    }

    // 加入方式（ENPCrossTeamJoinType枚举序号）
    public int getJoinType() { return this.join_type; }
    public void setJoinType(BM _bm, int join_type) {
        if(join_type==this.join_type) 
            return;
        this.join_type = join_type; 
        markField(_bm, FIELD_join_type); 
    }
    public void saveJoinType(BM _bm, int join_type) {
        if(join_type==this.join_type) 
            return;
        this.join_type = join_type;
        saveField(_bm, "join_type", join_type);
    }

    // 加入条件类型（ENPCrossTeamApplyCond枚举序号）
    public int getJoinCondType() { return this.join_cond_type; }
    public void setJoinCondType(BM _bm, int join_cond_type) {
        if(join_cond_type==this.join_cond_type) 
            return;
        this.join_cond_type = join_cond_type; 
        markField(_bm, FIELD_join_cond_type); 
    }
    public void saveJoinCondType(BM _bm, int join_cond_type) {
        if(join_cond_type==this.join_cond_type) 
            return;
        this.join_cond_type = join_cond_type;
        saveField(_bm, "join_cond_type", join_cond_type);
    }

    // 加入条件数值
    public long getJoinCondValue() { return this.join_cond_value; }
    public void setJoinCondValue(BM _bm, long join_cond_value) {
        if(join_cond_value==this.join_cond_value) 
            return;
        this.join_cond_value = join_cond_value; 
        markField(_bm, FIELD_join_cond_value); 
    }
    public void saveJoinCondValue(BM _bm, long join_cond_value) {
        if(join_cond_value==this.join_cond_value) 
            return;
        this.join_cond_value = join_cond_value;
        saveField(_bm, "join_cond_value", join_cond_value);
    }

    // 队伍名称
    public String getTeamName() { return this.team_name; }
    public void setTeamName(BM _bm, String team_name) {
        if(team_name.equals(this.team_name)) 
            return;
        this.team_name = team_name; 
        markField(_bm, FIELD_team_name); 
    }
    public void saveTeamName(BM _bm, String team_name) {
        if(team_name.equals(this.team_name)) 
            return;
        this.team_name = team_name;
        saveField(_bm, "team_name", team_name);
    }

    // 队伍宣言
    public String getTeamDec() { return this.team_dec; }
    public void setTeamDec(BM _bm, String team_dec) {
        if(team_dec.equals(this.team_dec)) 
            return;
        this.team_dec = team_dec; 
        markField(_bm, FIELD_team_dec); 
    }
    public void saveTeamDec(BM _bm, String team_dec) {
        if(team_dec.equals(this.team_dec)) 
            return;
        this.team_dec = team_dec;
        saveField(_bm, "team_dec", team_dec);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `team_id` = '").append(team_id).append("',");
        sBuilder.append(" `member_limit` = '").append(member_limit).append("',");
        sBuilder.append(" `join_type` = '").append(join_type).append("',");
        sBuilder.append(" `join_cond_type` = '").append(join_cond_type).append("',");
        sBuilder.append(" `join_cond_value` = '").append(join_cond_value).append("',");
        sBuilder.append(" `team_name` = '").append(team_name == null ? null : team_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `team_dec` = '").append(team_dec == null ? null : team_dec.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_team_id)) sBuilder.append(" `team_id` = '").append(team_id).append("',");
        if(isFieldMarked(FIELD_member_limit)) sBuilder.append(" `member_limit` = '").append(member_limit).append("',");
        if(isFieldMarked(FIELD_join_type)) sBuilder.append(" `join_type` = '").append(join_type).append("',");
        if(isFieldMarked(FIELD_join_cond_type)) sBuilder.append(" `join_cond_type` = '").append(join_cond_type).append("',");
        if(isFieldMarked(FIELD_join_cond_value)) sBuilder.append(" `join_cond_value` = '").append(join_cond_value).append("',");
        if(isFieldMarked(FIELD_team_name)) sBuilder.append(" `team_name` = '").append(team_name == null ? null : team_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_team_dec)) sBuilder.append(" `team_dec` = '").append(team_dec == null ? null : team_dec.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `cross_team` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`team_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '队伍实例ID',"
                + "`member_limit` int(11) NOT NULL DEFAULT '0' COMMENT '队伍成员上限',"
                + "`join_type` int(11) NOT NULL DEFAULT '0' COMMENT '加入方式（ENPCrossTeamJoinType枚举序号）',"
                + "`join_cond_type` int(11) NOT NULL DEFAULT '0' COMMENT '加入条件类型（ENPCrossTeamApplyCond枚举序号）',"
                + "`join_cond_value` bigint(20) NOT NULL DEFAULT '0' COMMENT '加入条件数值',"
                + "`team_name` varchar(500) NOT NULL DEFAULT '' COMMENT '队伍名称',"
                + "`team_dec` varchar(1024) NOT NULL DEFAULT '' COMMENT '队伍宣言',"
                + "KEY `team_id` (`team_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='跨服队伍数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//member_limit
        _size+=4;//join_type
        _size+=4;//join_cond_type
        _size+=8;//join_cond_value
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(team_name);//team_name
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(team_dec);//team_dec
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(team_id);
        buff.putInt(member_limit);
        buff.putInt(join_type);
        buff.putInt(join_cond_type);
        buff.putLong(join_cond_value);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, team_name);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, team_dec);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        team_id=buff.getLong();
        member_limit=buff.getInt();
        join_type=buff.getInt();
        join_cond_type=buff.getInt();
        join_cond_value=buff.getLong();
        team_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        team_dec=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
