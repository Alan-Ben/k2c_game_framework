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
public class CrossGroupDiscardBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_group_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "group_id", comment = "分组id")
    private long group_id;

    public static final int FIELD_team_max_idx =1;
    @DataBaseField(type = "int(11)", fieldname = "team_max_idx", comment = "队伍最大idx")
    private int team_max_idx;

    public static final int FIELD_discard_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "discard_ms", comment = "销毁时间")
    private long discard_ms;

    public CrossGroupDiscardBO() {
        id = 0;
        group_id = 0L;
        team_max_idx = 0;
        discard_ms = 0L;
    }

    public CrossGroupDiscardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        group_id = rs.getLong(2);
        team_max_idx = rs.getInt(3);
        discard_ms = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new CrossGroupDiscardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `group_id`, `team_max_idx`, `discard_ms`";
    }

    @Override
    public String getTableName() {
        return "`cross_group_discard`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(group_id).append("', ");
        strBuf.append("'").append(team_max_idx).append("', ");
        strBuf.append("'").append(discard_ms).append("', ");
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

    // 销毁时间
    public long getDiscardMs() { return this.discard_ms; }
    public void setDiscardMs(BM _bm, long discard_ms) {
        if(discard_ms==this.discard_ms) 
            return;
        this.discard_ms = discard_ms; 
        markField(_bm, FIELD_discard_ms); 
    }
    public void saveDiscardMs(BM _bm, long discard_ms) {
        if(discard_ms==this.discard_ms) 
            return;
        this.discard_ms = discard_ms;
        saveField(_bm, "discard_ms", discard_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `group_id` = '").append(group_id).append("',");
        sBuilder.append(" `team_max_idx` = '").append(team_max_idx).append("',");
        sBuilder.append(" `discard_ms` = '").append(discard_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_group_id)) sBuilder.append(" `group_id` = '").append(group_id).append("',");
        if(isFieldMarked(FIELD_team_max_idx)) sBuilder.append(" `team_max_idx` = '").append(team_max_idx).append("',");
        if(isFieldMarked(FIELD_discard_ms)) sBuilder.append(" `discard_ms` = '").append(discard_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `cross_group_discard` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '分组id',"
                + "`team_max_idx` int(11) NOT NULL DEFAULT '0' COMMENT '队伍最大idx',"
                + "`discard_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '销毁时间',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='跨服（已销毁）分组数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//team_max_idx
        _size+=8;//discard_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(group_id);
        buff.putInt(team_max_idx);
        buff.putLong(discard_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        group_id=buff.getLong();
        team_max_idx=buff.getInt();
        discard_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
