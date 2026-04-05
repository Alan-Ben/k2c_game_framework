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
public class ActivityPlanBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_plan_ref_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "plan_ref_id", comment = "计划配表ID")
    private long plan_ref_id;

    public static final int FIELD_disable =1;
    @DataBaseField(type = "tinyint(1)", fieldname = "disable", comment = "是否禁用")
    private boolean disable;

    public static final int FIELD_activity_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_id", comment = "活动ID")
    private long activity_id;

    public static final int FIELD_trigger_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "trigger_ms", comment = "触发时间")
    private long trigger_ms;

    public static final int FIELD_start_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "start_ms", comment = "活动开始时间")
    private long start_ms;

    public static final int FIELD_end_ms =5;
    @DataBaseField(type = "bigint(20)", fieldname = "end_ms", comment = "活动结束时间")
    private long end_ms;

    public static final int FIELD_close_ms =6;
    @DataBaseField(type = "bigint(20)", fieldname = "close_ms", comment = "活动关闭时间")
    private long close_ms;

    public ActivityPlanBO() {
        id = 0;
        plan_ref_id = 0L;
        disable = false;
        activity_id = 0L;
        trigger_ms = 0L;
        start_ms = 0L;
        end_ms = 0L;
        close_ms = 0L;
    }

    public ActivityPlanBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        plan_ref_id = rs.getLong(2);
        disable = rs.getBoolean(3);
        activity_id = rs.getLong(4);
        trigger_ms = rs.getLong(5);
        start_ms = rs.getLong(6);
        end_ms = rs.getLong(7);
        close_ms = rs.getLong(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ActivityPlanBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `plan_ref_id`, `disable`, `activity_id`, `trigger_ms`, `start_ms`, `end_ms`, `close_ms`";
    }

    @Override
    public String getTableName() {
        return "`activity_plan`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(plan_ref_id).append("', ");
        strBuf.append("'").append(disable ? 1 : 0).append("', ");
        strBuf.append("'").append(activity_id).append("', ");
        strBuf.append("'").append(trigger_ms).append("', ");
        strBuf.append("'").append(start_ms).append("', ");
        strBuf.append("'").append(end_ms).append("', ");
        strBuf.append("'").append(close_ms).append("', ");
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

    // 计划配表ID
    public long getPlanRefId() { return this.plan_ref_id; }
    public void setPlanRefId(BM _bm, long plan_ref_id) {
        if(plan_ref_id==this.plan_ref_id) 
            return;
        this.plan_ref_id = plan_ref_id; 
        markField(_bm, FIELD_plan_ref_id); 
    }
    public void savePlanRefId(BM _bm, long plan_ref_id) {
        if(plan_ref_id==this.plan_ref_id) 
            return;
        this.plan_ref_id = plan_ref_id;
        saveField(_bm, "plan_ref_id", plan_ref_id);
    }

    // 是否禁用
    public boolean getDisable() { return this.disable; }
    public void setDisable(BM _bm, boolean disable) {
        if(disable==this.disable) 
            return;
        this.disable = disable; 
        markField(_bm, FIELD_disable); 
    }
    public void saveDisable(BM _bm, boolean disable) {
        if(disable==this.disable) 
            return;
        this.disable = disable;
        saveField(_bm, "disable", disable ? 1 : 0);
    }

    // 活动ID
    public long getActivityId() { return this.activity_id; }
    public void setActivityId(BM _bm, long activity_id) {
        if(activity_id==this.activity_id) 
            return;
        this.activity_id = activity_id; 
        markField(_bm, FIELD_activity_id); 
    }
    public void saveActivityId(BM _bm, long activity_id) {
        if(activity_id==this.activity_id) 
            return;
        this.activity_id = activity_id;
        saveField(_bm, "activity_id", activity_id);
    }

    // 触发时间
    public long getTriggerMs() { return this.trigger_ms; }
    public void setTriggerMs(BM _bm, long trigger_ms) {
        if(trigger_ms==this.trigger_ms) 
            return;
        this.trigger_ms = trigger_ms; 
        markField(_bm, FIELD_trigger_ms); 
    }
    public void saveTriggerMs(BM _bm, long trigger_ms) {
        if(trigger_ms==this.trigger_ms) 
            return;
        this.trigger_ms = trigger_ms;
        saveField(_bm, "trigger_ms", trigger_ms);
    }

    // 活动开始时间
    public long getStartMs() { return this.start_ms; }
    public void setStartMs(BM _bm, long start_ms) {
        if(start_ms==this.start_ms) 
            return;
        this.start_ms = start_ms; 
        markField(_bm, FIELD_start_ms); 
    }
    public void saveStartMs(BM _bm, long start_ms) {
        if(start_ms==this.start_ms) 
            return;
        this.start_ms = start_ms;
        saveField(_bm, "start_ms", start_ms);
    }

    // 活动结束时间
    public long getEndMs() { return this.end_ms; }
    public void setEndMs(BM _bm, long end_ms) {
        if(end_ms==this.end_ms) 
            return;
        this.end_ms = end_ms; 
        markField(_bm, FIELD_end_ms); 
    }
    public void saveEndMs(BM _bm, long end_ms) {
        if(end_ms==this.end_ms) 
            return;
        this.end_ms = end_ms;
        saveField(_bm, "end_ms", end_ms);
    }

    // 活动关闭时间
    public long getCloseMs() { return this.close_ms; }
    public void setCloseMs(BM _bm, long close_ms) {
        if(close_ms==this.close_ms) 
            return;
        this.close_ms = close_ms; 
        markField(_bm, FIELD_close_ms); 
    }
    public void saveCloseMs(BM _bm, long close_ms) {
        if(close_ms==this.close_ms) 
            return;
        this.close_ms = close_ms;
        saveField(_bm, "close_ms", close_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `plan_ref_id` = '").append(plan_ref_id).append("',");
        sBuilder.append(" `disable` = '").append(disable ? 1 : 0).append("',");
        sBuilder.append(" `activity_id` = '").append(activity_id).append("',");
        sBuilder.append(" `trigger_ms` = '").append(trigger_ms).append("',");
        sBuilder.append(" `start_ms` = '").append(start_ms).append("',");
        sBuilder.append(" `end_ms` = '").append(end_ms).append("',");
        sBuilder.append(" `close_ms` = '").append(close_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_plan_ref_id)) sBuilder.append(" `plan_ref_id` = '").append(plan_ref_id).append("',");
        if(isFieldMarked(FIELD_disable)) sBuilder.append(" `disable` = '").append(disable ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_activity_id)) sBuilder.append(" `activity_id` = '").append(activity_id).append("',");
        if(isFieldMarked(FIELD_trigger_ms)) sBuilder.append(" `trigger_ms` = '").append(trigger_ms).append("',");
        if(isFieldMarked(FIELD_start_ms)) sBuilder.append(" `start_ms` = '").append(start_ms).append("',");
        if(isFieldMarked(FIELD_end_ms)) sBuilder.append(" `end_ms` = '").append(end_ms).append("',");
        if(isFieldMarked(FIELD_close_ms)) sBuilder.append(" `close_ms` = '").append(close_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `activity_plan` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`plan_ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '计划配表ID',"
                + "`disable` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否禁用',"
                + "`activity_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动ID',"
                + "`trigger_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '触发时间',"
                + "`start_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动开始时间',"
                + "`end_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动结束时间',"
                + "`close_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动关闭时间',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动排期数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//plan_ref_id
        _size+=1;//disable
        _size+=8;//activity_id
        _size+=8;//trigger_ms
        _size+=8;//start_ms
        _size+=8;//end_ms
        _size+=8;//close_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(plan_ref_id);
        buff.put((byte)(disable?1:0));
        buff.putLong(activity_id);
        buff.putLong(trigger_ms);
        buff.putLong(start_ms);
        buff.putLong(end_ms);
        buff.putLong(close_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        plan_ref_id=buff.getLong();
        disable=(buff.get()==1);
        activity_id=buff.getLong();
        trigger_ms=buff.getLong();
        start_ms=buff.getLong();
        end_ms=buff.getLong();
        close_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
