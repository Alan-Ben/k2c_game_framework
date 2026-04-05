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
public class CrossGroupBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_group_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "group_id", comment = "分组id")
    private long group_id;

    public static final int FIELD_joiner_list =1;
    @DataBaseField(type = "blob", fieldname = "joiner_list", comment = "参与对象ID集合")
    private byte[] joiner_list;

    public static final int FIELD_team_max_idx =2;
    @DataBaseField(type = "int(11)", fieldname = "team_max_idx", comment = "队伍最大idx")
    private int team_max_idx;

    public CrossGroupBO() {
        id = 0;
        group_id = 0L;
        joiner_list = null;
        team_max_idx = 0;
    }

    public CrossGroupBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        group_id = rs.getLong(2);
        joiner_list = rs.getBytes(3);
        team_max_idx = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new CrossGroupBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `group_id`, `joiner_list`, `team_max_idx`";
    }

    @Override
    public String getTableName() {
        return "`cross_group`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(group_id).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(team_max_idx).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(joiner_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_joiner_list)) ret.add(joiner_list);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 分组id
    public long getGroupId() { return this.group_id; }
    public void setGroupId(BM _bm, long group_id) {
        if(group_id==this.group_id) 
            return;
        this.group_id = group_id; 
        markField(_bm, FIELD_group_id); 
    }
    public void saveGroupId(BM _bm, long group_id) {
        if(group_id==this.group_id) 
            return;
        this.group_id = group_id;
        saveField(_bm, "group_id", group_id);
    }

    // 参与对象ID集合
    public byte[] getJoinerList() { return this.joiner_list; }
    public void setJoinerList(BM _bm, byte[] joiner_list) {
        if(joiner_list==this.joiner_list) 
            return;
        this.joiner_list = joiner_list; 
        markField(_bm, FIELD_joiner_list); 
    }
    public void saveJoinerList(BM _bm, byte[] joiner_list) {
        if(joiner_list==this.joiner_list) 
            return;
        this.joiner_list = joiner_list;
        saveFieldBytes(_bm, "joiner_list", joiner_list);
    }

    // 队伍最大idx
    public int getTeamMaxIdx() { return this.team_max_idx; }
    public void setTeamMaxIdx(BM _bm, int team_max_idx) {
        if(team_max_idx==this.team_max_idx) 
            return;
        this.team_max_idx = team_max_idx; 
        markField(_bm, FIELD_team_max_idx); 
    }
    public void saveTeamMaxIdx(BM _bm, int team_max_idx) {
        if(team_max_idx==this.team_max_idx) 
            return;
        this.team_max_idx = team_max_idx;
        saveField(_bm, "team_max_idx", team_max_idx);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `group_id` = '").append(group_id).append("',");
        sBuilder.append(" `joiner_list` = ?,");
        sBuilder.append(" `team_max_idx` = '").append(team_max_idx).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_group_id)) sBuilder.append(" `group_id` = '").append(group_id).append("',");
        if(isFieldMarked(FIELD_joiner_list)) sBuilder.append(" `joiner_list` = ?,");
        if(isFieldMarked(FIELD_team_max_idx)) sBuilder.append(" `team_max_idx` = '").append(team_max_idx).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `cross_group` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '分组id',"
                + "`joiner_list` blob NULL COMMENT '参与对象ID集合',"
                + "`team_max_idx` int(11) NOT NULL DEFAULT '0' COMMENT '队伍最大idx',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='跨服分组数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//group_id
        _size+=2;_size+=joiner_list.length;//joiner_list
        _size+=4;//team_max_idx
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(group_id);
        buff.putShort((short)(joiner_list == null ? 0 : joiner_list.length));if(null != joiner_list){buff.put(joiner_list);}
        buff.putInt(team_max_idx);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        group_id=buff.getLong();
        int joiner_list_count = buff.getShort();if(joiner_list_count>0){joiner_list = new byte[joiner_list_count];buff.get(joiner_list);}
        team_max_idx=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
