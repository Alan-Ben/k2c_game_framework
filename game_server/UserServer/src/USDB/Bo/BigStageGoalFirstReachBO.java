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
public class BigStageGoalFirstReachBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_big_stage_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "big_stage_id", comment = "大阶段ID")
    private long big_stage_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_reach_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "reach_time_ms", comment = "达成时间毫秒")
    private long reach_time_ms;

    public BigStageGoalFirstReachBO() {
        id = 0;
        big_stage_id = 0L;
        cid = 0L;
        reach_time_ms = 0L;
    }

    public BigStageGoalFirstReachBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        big_stage_id = rs.getLong(2);
        cid = rs.getLong(3);
        reach_time_ms = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new BigStageGoalFirstReachBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `big_stage_id`, `cid`, `reach_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`big_stage_goal_first_reach`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(big_stage_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(reach_time_ms).append("', ");
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

    // 大阶段ID
    public long getBigStageId() { return this.big_stage_id; }
    public void setBigStageId(BM _bm, long big_stage_id) {
        if(big_stage_id==this.big_stage_id) 
            return;
        this.big_stage_id = big_stage_id; 
        markField(_bm, FIELD_big_stage_id); 
    }
    public void saveBigStageId(BM _bm, long big_stage_id) {
        if(big_stage_id==this.big_stage_id) 
            return;
        this.big_stage_id = big_stage_id;
        saveField(_bm, "big_stage_id", big_stage_id);
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

    // 达成时间毫秒
    public long getReachTimeMs() { return this.reach_time_ms; }
    public void setReachTimeMs(BM _bm, long reach_time_ms) {
        if(reach_time_ms==this.reach_time_ms) 
            return;
        this.reach_time_ms = reach_time_ms; 
        markField(_bm, FIELD_reach_time_ms); 
    }
    public void saveReachTimeMs(BM _bm, long reach_time_ms) {
        if(reach_time_ms==this.reach_time_ms) 
            return;
        this.reach_time_ms = reach_time_ms;
        saveField(_bm, "reach_time_ms", reach_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `big_stage_id` = '").append(big_stage_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `reach_time_ms` = '").append(reach_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_big_stage_id)) sBuilder.append(" `big_stage_id` = '").append(big_stage_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_reach_time_ms)) sBuilder.append(" `reach_time_ms` = '").append(reach_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `big_stage_goal_first_reach` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`big_stage_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '大阶段ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`reach_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '达成时间毫秒',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家大阶段目标首次达成记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//big_stage_id
        _size+=8;//cid
        _size+=8;//reach_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(big_stage_id);
        buff.putLong(cid);
        buff.putLong(reach_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        big_stage_id=buff.getLong();
        cid=buff.getLong();
        reach_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
