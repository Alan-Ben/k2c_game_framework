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
public class RechargeRebatePlayerInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_activity_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例ID")
    private long activity_instance_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_group_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "group_id", comment = "返利组ID")
    private long group_id;

    public static final int FIELD_count =3;
    @DataBaseField(type = "bigint(20)", fieldname = "count", comment = "当前计数（VIP点数或充值天数）")
    private long count;

    public static final int FIELD_last_recharge_date =4;
    @DataBaseField(type = "int(11)", fieldname = "last_recharge_date", comment = "最后充值日期（YYYYMMDD格式）")
    private int last_recharge_date;

    public static final int FIELD_had_draw_step_list =5;
    @DataBaseField(type = "text", fieldname = "had_draw_step_list", comment = "已领取档位ID列表（逗号分隔）")
    private String had_draw_step_list;

    public RechargeRebatePlayerInfoBO() {
        id = 0;
        activity_instance_id = 0L;
        cid = 0L;
        group_id = 0L;
        count = 0L;
        last_recharge_date = 0;
        had_draw_step_list = "";
    }

    public RechargeRebatePlayerInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        activity_instance_id = rs.getLong(2);
        cid = rs.getLong(3);
        group_id = rs.getLong(4);
        count = rs.getLong(5);
        last_recharge_date = rs.getInt(6);
        had_draw_step_list = rs.getString(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new RechargeRebatePlayerInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `activity_instance_id`, `cid`, `group_id`, `count`, `last_recharge_date`, `had_draw_step_list`";
    }

    @Override
    public String getTableName() {
        return "`recharge_rebate_player_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(group_id).append("', ");
        strBuf.append("'").append(count).append("', ");
        strBuf.append("'").append(last_recharge_date).append("', ");
        strBuf.append("'").append(had_draw_step_list == null ? null : had_draw_step_list.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 返利组ID
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

    // 当前计数（VIP点数或充值天数）
    public long getCount() { return this.count; }
    public void setCount(BM _bm, long count) {
        if(count==this.count) 
            return;
        this.count = count; 
        markField(_bm, FIELD_count); 
    }
    public void saveCount(BM _bm, long count) {
        if(count==this.count) 
            return;
        this.count = count;
        saveField(_bm, "count", count);
    }

    // 最后充值日期（YYYYMMDD格式）
    public int getLastRechargeDate() { return this.last_recharge_date; }
    public void setLastRechargeDate(BM _bm, int last_recharge_date) {
        if(last_recharge_date==this.last_recharge_date) 
            return;
        this.last_recharge_date = last_recharge_date; 
        markField(_bm, FIELD_last_recharge_date); 
    }
    public void saveLastRechargeDate(BM _bm, int last_recharge_date) {
        if(last_recharge_date==this.last_recharge_date) 
            return;
        this.last_recharge_date = last_recharge_date;
        saveField(_bm, "last_recharge_date", last_recharge_date);
    }

    // 已领取档位ID列表（逗号分隔）
    public String getHadDrawStepList() { return this.had_draw_step_list; }
    public void setHadDrawStepList(BM _bm, String had_draw_step_list) {
        if(had_draw_step_list.equals(this.had_draw_step_list)) 
            return;
        this.had_draw_step_list = had_draw_step_list; 
        markField(_bm, FIELD_had_draw_step_list); 
    }
    public void saveHadDrawStepList(BM _bm, String had_draw_step_list) {
        if(had_draw_step_list.equals(this.had_draw_step_list)) 
            return;
        this.had_draw_step_list = had_draw_step_list;
        saveField(_bm, "had_draw_step_list", had_draw_step_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `group_id` = '").append(group_id).append("',");
        sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.append(" `last_recharge_date` = '").append(last_recharge_date).append("',");
        sBuilder.append(" `had_draw_step_list` = '").append(had_draw_step_list == null ? null : had_draw_step_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_group_id)) sBuilder.append(" `group_id` = '").append(group_id).append("',");
        if(isFieldMarked(FIELD_count)) sBuilder.append(" `count` = '").append(count).append("',");
        if(isFieldMarked(FIELD_last_recharge_date)) sBuilder.append(" `last_recharge_date` = '").append(last_recharge_date).append("',");
        if(isFieldMarked(FIELD_had_draw_step_list)) sBuilder.append(" `had_draw_step_list` = '").append(had_draw_step_list == null ? null : had_draw_step_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `recharge_rebate_player_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '返利组ID',"
                + "`count` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前计数（VIP点数或充值天数）',"
                + "`last_recharge_date` int(11) NOT NULL DEFAULT '0' COMMENT '最后充值日期（YYYYMMDD格式）',"
                + "`had_draw_step_list` text NULL COMMENT '已领取档位ID列表（逗号分隔）',"
                + "KEY `activity_instance_id` (`activity_instance_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家充值返利数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//group_id
        _size+=8;//count
        _size+=4;//last_recharge_date
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(had_draw_step_list);//had_draw_step_list
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
        buff.putLong(group_id);
        buff.putLong(count);
        buff.putInt(last_recharge_date);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, had_draw_step_list);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        activity_instance_id=buff.getLong();
        cid=buff.getLong();
        group_id=buff.getLong();
        count=buff.getLong();
        last_recharge_date=buff.getInt();
        had_draw_step_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
