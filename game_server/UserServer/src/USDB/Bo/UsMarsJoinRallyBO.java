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
public class UsMarsJoinRallyBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guildId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guildId", comment = "联盟ID")
    private long guildId;

    public static final int FIELD_rallyId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "rallyId", comment = "集结ID")
    private long rallyId;

    public static final int FIELD_cid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_teamId =3;
    @DataBaseField(type = "bigint(20)", fieldname = "teamId", comment = "队伍ID")
    private long teamId;

    public static final int FIELD_triggerTimeMs =4;
    @DataBaseField(type = "bigint(20)", fieldname = "triggerTimeMs", comment = "触发时间毫秒")
    private long triggerTimeMs;

    public UsMarsJoinRallyBO() {
        id = 0;
        guildId = 0L;
        rallyId = 0L;
        cid = 0L;
        teamId = 0L;
        triggerTimeMs = 0L;
    }

    public UsMarsJoinRallyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guildId = rs.getLong(2);
        rallyId = rs.getLong(3);
        cid = rs.getLong(4);
        teamId = rs.getLong(5);
        triggerTimeMs = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsMarsJoinRallyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guildId`, `rallyId`, `cid`, `teamId`, `triggerTimeMs`";
    }

    @Override
    public String getTableName() {
        return "`us_mars_join_rally`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guildId).append("', ");
        strBuf.append("'").append(rallyId).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(teamId).append("', ");
        strBuf.append("'").append(triggerTimeMs).append("', ");
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

    // 联盟ID
    public long getGuildId() { return this.guildId; }
    public void setGuildId(BM _bm, long guildId) {
        if(guildId==this.guildId) 
            return;
        this.guildId = guildId; 
        markField(_bm, FIELD_guildId); 
    }
    public void saveGuildId(BM _bm, long guildId) {
        if(guildId==this.guildId) 
            return;
        this.guildId = guildId;
        saveField(_bm, "guildId", guildId);
    }

    // 集结ID
    public long getRallyId() { return this.rallyId; }
    public void setRallyId(BM _bm, long rallyId) {
        if(rallyId==this.rallyId) 
            return;
        this.rallyId = rallyId; 
        markField(_bm, FIELD_rallyId); 
    }
    public void saveRallyId(BM _bm, long rallyId) {
        if(rallyId==this.rallyId) 
            return;
        this.rallyId = rallyId;
        saveField(_bm, "rallyId", rallyId);
    }

    // 玩家CID
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

    // 队伍ID
    public long getTeamId() { return this.teamId; }
    public void setTeamId(BM _bm, long teamId) {
        if(teamId==this.teamId) 
            return;
        this.teamId = teamId; 
        markField(_bm, FIELD_teamId); 
    }
    public void saveTeamId(BM _bm, long teamId) {
        if(teamId==this.teamId) 
            return;
        this.teamId = teamId;
        saveField(_bm, "teamId", teamId);
    }

    // 触发时间毫秒
    public long getTriggerTimeMs() { return this.triggerTimeMs; }
    public void setTriggerTimeMs(BM _bm, long triggerTimeMs) {
        if(triggerTimeMs==this.triggerTimeMs) 
            return;
        this.triggerTimeMs = triggerTimeMs; 
        markField(_bm, FIELD_triggerTimeMs); 
    }
    public void saveTriggerTimeMs(BM _bm, long triggerTimeMs) {
        if(triggerTimeMs==this.triggerTimeMs) 
            return;
        this.triggerTimeMs = triggerTimeMs;
        saveField(_bm, "triggerTimeMs", triggerTimeMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guildId` = '").append(guildId).append("',");
        sBuilder.append(" `rallyId` = '").append(rallyId).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `teamId` = '").append(teamId).append("',");
        sBuilder.append(" `triggerTimeMs` = '").append(triggerTimeMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guildId)) sBuilder.append(" `guildId` = '").append(guildId).append("',");
        if(isFieldMarked(FIELD_rallyId)) sBuilder.append(" `rallyId` = '").append(rallyId).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_teamId)) sBuilder.append(" `teamId` = '").append(teamId).append("',");
        if(isFieldMarked(FIELD_triggerTimeMs)) sBuilder.append(" `triggerTimeMs` = '").append(triggerTimeMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_mars_join_rally` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guildId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`rallyId` bigint(20) NOT NULL DEFAULT '0' COMMENT '集结ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`teamId` bigint(20) NOT NULL DEFAULT '0' COMMENT '队伍ID',"
                + "`triggerTimeMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '触发时间毫秒',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='本服加入集结行军行为数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guildId
        _size+=8;//rallyId
        _size+=8;//cid
        _size+=8;//teamId
        _size+=8;//triggerTimeMs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guildId);
        buff.putLong(rallyId);
        buff.putLong(cid);
        buff.putLong(teamId);
        buff.putLong(triggerTimeMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guildId=buff.getLong();
        rallyId=buff.getLong();
        cid=buff.getLong();
        teamId=buff.getLong();
        triggerTimeMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
