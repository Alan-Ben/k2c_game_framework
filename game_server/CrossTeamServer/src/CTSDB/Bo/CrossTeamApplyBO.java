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
public class CrossTeamApplyBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_team_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "team_id", comment = "队伍实例ID")
    private long team_id;

    public static final int FIELD_apply_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "apply_cid", comment = "申请玩家CID")
    private long apply_cid;

    public static final int FIELD_apply_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "apply_ms", comment = "申请时间（毫秒）")
    private long apply_ms;

    public CrossTeamApplyBO() {
        id = 0;
        team_id = 0L;
        apply_cid = 0L;
        apply_ms = 0L;
    }

    public CrossTeamApplyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        team_id = rs.getLong(2);
        apply_cid = rs.getLong(3);
        apply_ms = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new CrossTeamApplyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `team_id`, `apply_cid`, `apply_ms`";
    }

    @Override
    public String getTableName() {
        return "`cross_team_apply`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(team_id).append("', ");
        strBuf.append("'").append(apply_cid).append("', ");
        strBuf.append("'").append(apply_ms).append("', ");
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

    // 申请玩家CID
    public long getApplyCid() { return this.apply_cid; }
    public void setApplyCid(BM _bm, long apply_cid) {
        if(apply_cid==this.apply_cid) 
            return;
        this.apply_cid = apply_cid; 
        markField(_bm, FIELD_apply_cid); 
    }
    public void saveApplyCid(BM _bm, long apply_cid) {
        if(apply_cid==this.apply_cid) 
            return;
        this.apply_cid = apply_cid;
        saveField(_bm, "apply_cid", apply_cid);
    }

    // 申请时间（毫秒）
    public long getApplyMs() { return this.apply_ms; }
    public void setApplyMs(BM _bm, long apply_ms) {
        if(apply_ms==this.apply_ms) 
            return;
        this.apply_ms = apply_ms; 
        markField(_bm, FIELD_apply_ms); 
    }
    public void saveApplyMs(BM _bm, long apply_ms) {
        if(apply_ms==this.apply_ms) 
            return;
        this.apply_ms = apply_ms;
        saveField(_bm, "apply_ms", apply_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `team_id` = '").append(team_id).append("',");
        sBuilder.append(" `apply_cid` = '").append(apply_cid).append("',");
        sBuilder.append(" `apply_ms` = '").append(apply_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_team_id)) sBuilder.append(" `team_id` = '").append(team_id).append("',");
        if(isFieldMarked(FIELD_apply_cid)) sBuilder.append(" `apply_cid` = '").append(apply_cid).append("',");
        if(isFieldMarked(FIELD_apply_ms)) sBuilder.append(" `apply_ms` = '").append(apply_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `cross_team_apply` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`team_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '队伍实例ID',"
                + "`apply_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '申请玩家CID',"
                + "`apply_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '申请时间（毫秒）',"
                + "KEY `team_id` (`team_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='跨服队伍申请数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//apply_cid
        _size+=8;//apply_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(team_id);
        buff.putLong(apply_cid);
        buff.putLong(apply_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        team_id=buff.getLong();
        apply_cid=buff.getLong();
        apply_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
