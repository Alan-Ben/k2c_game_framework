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
public class RechargeRebateActivityInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_activity_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例ID")
    private long activity_instance_id;

    public static final int FIELD_last_check_date =1;
    @DataBaseField(type = "int(11)", fieldname = "last_check_date", comment = "最后检查日期（YYYYMMDD格式）")
    private int last_check_date;

    public RechargeRebateActivityInfoBO() {
        id = 0;
        activity_instance_id = 0L;
        last_check_date = 0;
    }

    public RechargeRebateActivityInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        activity_instance_id = rs.getLong(2);
        last_check_date = rs.getInt(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new RechargeRebateActivityInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `activity_instance_id`, `last_check_date`";
    }

    @Override
    public String getTableName() {
        return "`recharge_rebate_activity_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(last_check_date).append("', ");
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

    // 活动实例ID
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

    // 最后检查日期（YYYYMMDD格式）
    public int getLastCheckDate() { return this.last_check_date; }
    public void setLastCheckDate(BM _bm, int last_check_date) {
        if(last_check_date==this.last_check_date) 
            return;
        this.last_check_date = last_check_date; 
        markField(_bm, FIELD_last_check_date); 
    }
    public void saveLastCheckDate(BM _bm, int last_check_date) {
        if(last_check_date==this.last_check_date) 
            return;
        this.last_check_date = last_check_date;
        saveField(_bm, "last_check_date", last_check_date);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `last_check_date` = '").append(last_check_date).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_last_check_date)) sBuilder.append(" `last_check_date` = '").append(last_check_date).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `recharge_rebate_activity_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`last_check_date` int(11) NOT NULL DEFAULT '0' COMMENT '最后检查日期（YYYYMMDD格式）',"
                + "KEY `activity_instance_id` (`activity_instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='充值返利数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//last_check_date
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(activity_instance_id);
        buff.putInt(last_check_date);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        activity_instance_id=buff.getLong();
        last_check_date=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
