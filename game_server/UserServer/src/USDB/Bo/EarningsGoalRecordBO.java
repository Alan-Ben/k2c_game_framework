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
public class EarningsGoalRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_activity_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例id")
    private long activity_instance_id;

    public static final int FIELD_type =1;
    @DataBaseField(type = "int(11)", fieldname = "type", comment = "类型")
    private int type;

    public static final int FIELD_ref_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "ref_id", comment = "配置id")
    private long ref_id;

    public static final int FIELD_first_reach_cid =3;
    @DataBaseField(type = "bigint(20)", fieldname = "first_reach_cid", comment = "首达cid")
    private long first_reach_cid;

    public static final int FIELD_timestamp =4;
    @DataBaseField(type = "bigint(20)", fieldname = "timestamp", comment = "时间戳")
    private long timestamp;

    public EarningsGoalRecordBO() {
        id = 0;
        activity_instance_id = 0L;
        type = 0;
        ref_id = 0L;
        first_reach_cid = 0L;
        timestamp = 0L;
    }

    public EarningsGoalRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        activity_instance_id = rs.getLong(2);
        type = rs.getInt(3);
        ref_id = rs.getLong(4);
        first_reach_cid = rs.getLong(5);
        timestamp = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new EarningsGoalRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `activity_instance_id`, `type`, `ref_id`, `first_reach_cid`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`earnings_goal_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(type).append("', ");
        strBuf.append("'").append(ref_id).append("', ");
        strBuf.append("'").append(first_reach_cid).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
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

    // 类型
    public int getType() { return this.type; }
    public void setType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type; 
        markField(_bm, FIELD_type); 
    }
    public void saveType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type;
        saveField(_bm, "type", type);
    }

    // 配置id
    public long getRefId() { return this.ref_id; }
    public void setRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id; 
        markField(_bm, FIELD_ref_id); 
    }
    public void saveRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id;
        saveField(_bm, "ref_id", ref_id);
    }

    // 首达cid
    public long getFirstReachCid() { return this.first_reach_cid; }
    public void setFirstReachCid(BM _bm, long first_reach_cid) {
        if(first_reach_cid==this.first_reach_cid) 
            return;
        this.first_reach_cid = first_reach_cid; 
        markField(_bm, FIELD_first_reach_cid); 
    }
    public void saveFirstReachCid(BM _bm, long first_reach_cid) {
        if(first_reach_cid==this.first_reach_cid) 
            return;
        this.first_reach_cid = first_reach_cid;
        saveField(_bm, "first_reach_cid", first_reach_cid);
    }

    // 时间戳
    public long getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, long timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, long timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `type` = '").append(type).append("',");
        sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        sBuilder.append(" `first_reach_cid` = '").append(first_reach_cid).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_type)) sBuilder.append(" `type` = '").append(type).append("',");
        if(isFieldMarked(FIELD_ref_id)) sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        if(isFieldMarked(FIELD_first_reach_cid)) sBuilder.append(" `first_reach_cid` = '").append(first_reach_cid).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `earnings_goal_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例id',"
                + "`type` int(11) NOT NULL DEFAULT '0' COMMENT '类型',"
                + "`ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置id',"
                + "`first_reach_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '首达cid',"
                + "`timestamp` bigint(20) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "KEY `activity_instance_id` (`activity_instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='赚速目标奖励数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//type
        _size+=8;//ref_id
        _size+=8;//first_reach_cid
        _size+=8;//timestamp
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(activity_instance_id);
        buff.putInt(type);
        buff.putLong(ref_id);
        buff.putLong(first_reach_cid);
        buff.putLong(timestamp);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        activity_instance_id=buff.getLong();
        type=buff.getInt();
        ref_id=buff.getLong();
        first_reach_cid=buff.getLong();
        timestamp=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
