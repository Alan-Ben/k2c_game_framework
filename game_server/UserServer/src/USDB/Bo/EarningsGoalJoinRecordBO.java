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
public class EarningsGoalJoinRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_activity_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例id")
    private long activity_instance_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家id")
    private long cid;

    public EarningsGoalJoinRecordBO() {
        id = 0;
        activity_instance_id = 0L;
        cid = 0L;
    }

    public EarningsGoalJoinRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        activity_instance_id = rs.getLong(2);
        cid = rs.getLong(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new EarningsGoalJoinRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `activity_instance_id`, `cid`";
    }

    @Override
    public String getTableName() {
        return "`earnings_goal_join_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
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

    // 活动实例id
    public long getActivityInstanceId() { return this.activity_instance_id; }
    public void setActivityInstanceId(BM _bm, long activity_instance_id) {
        if(activity_instance_id==this.activity_instance_id) 
            return;
        this.activity_instance_id = activity_instance_id; 
        markField(_bm, FIELD_activity_instance_id); 
    }
    public void saveActivityInstanceId(BM _bm, long activity_instance_id) {
        if(activity_instance_id==this.activity_instance_id) 
            return;
        this.activity_instance_id = activity_instance_id;
        saveField(_bm, "activity_instance_id", activity_instance_id);
    }

    // 玩家id
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `earnings_goal_join_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家id',"
                + "KEY `activity_instance_id` (`activity_instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='赚速目标参加数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//activity_instance_id
        _size+=8;//cid
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(activity_instance_id);
        buff.putLong(cid);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        activity_instance_id=buff.getLong();
        cid=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
