package SSDB.Bo;
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
public class ActivityScheduleBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_php_schedule_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "php_schedule_id", comment = "后台排期ID")
    private long php_schedule_id;

    public static final int FIELD_submit_count =1;
    @DataBaseField(type = "int(11)", fieldname = "submit_count", comment = "提交次数")
    private int submit_count;

    public static final int FIELD_is_active =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_active", comment = "是否生效")
    private boolean is_active;

    public static final int FIELD_pre_push_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "pre_push_time_ms", comment = "预下发时间")
    private long pre_push_time_ms;

    public static final int FIELD_activity_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_id", comment = "活动id")
    private long activity_id;

    public static final int FIELD_start_time_ms =5;
    @DataBaseField(type = "bigint(20)", fieldname = "start_time_ms", comment = "开始时间")
    private long start_time_ms;

    public static final int FIELD_end_time_ms =6;
    @DataBaseField(type = "bigint(20)", fieldname = "end_time_ms", comment = "领奖时间")
    private long end_time_ms;

    public static final int FIELD_close_time_ms =7;
    @DataBaseField(type = "bigint(20)", fieldname = "close_time_ms", comment = "结束时间")
    private long close_time_ms;

    public static final int FIELD_group_data =8;
    @DataBaseField(type = "blob", fieldname = "group_data", comment = "分组数据 生效后按此数据构造分组")
    private byte[] group_data;

    public static final int FIELD_is_inited =9;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_inited", comment = "是否完成初始化")
    private boolean is_inited;

    public ActivityScheduleBO() {
        id = 0;
        php_schedule_id = 0L;
        submit_count = 0;
        is_active = false;
        pre_push_time_ms = 0L;
        activity_id = 0L;
        start_time_ms = 0L;
        end_time_ms = 0L;
        close_time_ms = 0L;
        group_data = null;
        is_inited = false;
    }

    public ActivityScheduleBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        php_schedule_id = rs.getLong(2);
        submit_count = rs.getInt(3);
        is_active = rs.getBoolean(4);
        pre_push_time_ms = rs.getLong(5);
        activity_id = rs.getLong(6);
        start_time_ms = rs.getLong(7);
        end_time_ms = rs.getLong(8);
        close_time_ms = rs.getLong(9);
        group_data = rs.getBytes(10);
        is_inited = rs.getBoolean(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ActivityScheduleBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `php_schedule_id`, `submit_count`, `is_active`, `pre_push_time_ms`, `activity_id`, `start_time_ms`, `end_time_ms`, `close_time_ms`, `group_data`, `is_inited`";
    }

    @Override
    public String getTableName() {
        return "`activity_schedule`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(php_schedule_id).append("', ");
        strBuf.append("'").append(submit_count).append("', ");
        strBuf.append("'").append(is_active ? 1 : 0).append("', ");
        strBuf.append("'").append(pre_push_time_ms).append("', ");
        strBuf.append("'").append(activity_id).append("', ");
        strBuf.append("'").append(start_time_ms).append("', ");
        strBuf.append("'").append(end_time_ms).append("', ");
        strBuf.append("'").append(close_time_ms).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(is_inited ? 1 : 0).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(group_data);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_group_data)) ret.add(group_data);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 后台排期ID
    public long getPhpScheduleId() { return this.php_schedule_id; }
    public void setPhpScheduleId(BM _bm, long php_schedule_id) {
        if(php_schedule_id==this.php_schedule_id) 
            return;
        this.php_schedule_id = php_schedule_id; 
        markField(_bm, FIELD_php_schedule_id); 
    }
    public void savePhpScheduleId(BM _bm, long php_schedule_id) {
        if(php_schedule_id==this.php_schedule_id) 
            return;
        this.php_schedule_id = php_schedule_id;
        saveField(_bm, "php_schedule_id", php_schedule_id);
    }

    // 提交次数
    public int getSubmitCount() { return this.submit_count; }
    public void setSubmitCount(BM _bm, int submit_count) {
        if(submit_count==this.submit_count) 
            return;
        this.submit_count = submit_count; 
        markField(_bm, FIELD_submit_count); 
    }
    public void saveSubmitCount(BM _bm, int submit_count) {
        if(submit_count==this.submit_count) 
            return;
        this.submit_count = submit_count;
        saveField(_bm, "submit_count", submit_count);
    }

    // 是否生效
    public boolean getIsActive() { return this.is_active; }
    public void setIsActive(BM _bm, boolean is_active) {
        if(is_active==this.is_active) 
            return;
        this.is_active = is_active; 
        markField(_bm, FIELD_is_active); 
    }
    public void saveIsActive(BM _bm, boolean is_active) {
        if(is_active==this.is_active) 
            return;
        this.is_active = is_active;
        saveField(_bm, "is_active", is_active ? 1 : 0);
    }

    // 预下发时间
    public long getPrePushTimeMs() { return this.pre_push_time_ms; }
    public void setPrePushTimeMs(BM _bm, long pre_push_time_ms) {
        if(pre_push_time_ms==this.pre_push_time_ms) 
            return;
        this.pre_push_time_ms = pre_push_time_ms; 
        markField(_bm, FIELD_pre_push_time_ms); 
    }
    public void savePrePushTimeMs(BM _bm, long pre_push_time_ms) {
        if(pre_push_time_ms==this.pre_push_time_ms) 
            return;
        this.pre_push_time_ms = pre_push_time_ms;
        saveField(_bm, "pre_push_time_ms", pre_push_time_ms);
    }

    // 活动id
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

    // 开始时间
    public long getStartTimeMs() { return this.start_time_ms; }
    public void setStartTimeMs(BM _bm, long start_time_ms) {
        if(start_time_ms==this.start_time_ms) 
            return;
        this.start_time_ms = start_time_ms; 
        markField(_bm, FIELD_start_time_ms); 
    }
    public void saveStartTimeMs(BM _bm, long start_time_ms) {
        if(start_time_ms==this.start_time_ms) 
            return;
        this.start_time_ms = start_time_ms;
        saveField(_bm, "start_time_ms", start_time_ms);
    }

    // 领奖时间
    public long getEndTimeMs() { return this.end_time_ms; }
    public void setEndTimeMs(BM _bm, long end_time_ms) {
        if(end_time_ms==this.end_time_ms) 
            return;
        this.end_time_ms = end_time_ms; 
        markField(_bm, FIELD_end_time_ms); 
    }
    public void saveEndTimeMs(BM _bm, long end_time_ms) {
        if(end_time_ms==this.end_time_ms) 
            return;
        this.end_time_ms = end_time_ms;
        saveField(_bm, "end_time_ms", end_time_ms);
    }

    // 结束时间
    public long getCloseTimeMs() { return this.close_time_ms; }
    public void setCloseTimeMs(BM _bm, long close_time_ms) {
        if(close_time_ms==this.close_time_ms) 
            return;
        this.close_time_ms = close_time_ms; 
        markField(_bm, FIELD_close_time_ms); 
    }
    public void saveCloseTimeMs(BM _bm, long close_time_ms) {
        if(close_time_ms==this.close_time_ms) 
            return;
        this.close_time_ms = close_time_ms;
        saveField(_bm, "close_time_ms", close_time_ms);
    }

    // 分组数据 生效后按此数据构造分组
    public byte[] getGroupData() { return this.group_data; }
    public void setGroupData(BM _bm, byte[] group_data) {
        if(group_data==this.group_data) 
            return;
        this.group_data = group_data; 
        markField(_bm, FIELD_group_data); 
    }
    public void saveGroupData(BM _bm, byte[] group_data) {
        if(group_data==this.group_data) 
            return;
        this.group_data = group_data;
        saveFieldBytes(_bm, "group_data", group_data);
    }

    // 是否完成初始化
    public boolean getIsInited() { return this.is_inited; }
    public void setIsInited(BM _bm, boolean is_inited) {
        if(is_inited==this.is_inited) 
            return;
        this.is_inited = is_inited; 
        markField(_bm, FIELD_is_inited); 
    }
    public void saveIsInited(BM _bm, boolean is_inited) {
        if(is_inited==this.is_inited) 
            return;
        this.is_inited = is_inited;
        saveField(_bm, "is_inited", is_inited ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `php_schedule_id` = '").append(php_schedule_id).append("',");
        sBuilder.append(" `submit_count` = '").append(submit_count).append("',");
        sBuilder.append(" `is_active` = '").append(is_active ? 1 : 0).append("',");
        sBuilder.append(" `pre_push_time_ms` = '").append(pre_push_time_ms).append("',");
        sBuilder.append(" `activity_id` = '").append(activity_id).append("',");
        sBuilder.append(" `start_time_ms` = '").append(start_time_ms).append("',");
        sBuilder.append(" `end_time_ms` = '").append(end_time_ms).append("',");
        sBuilder.append(" `close_time_ms` = '").append(close_time_ms).append("',");
        sBuilder.append(" `group_data` = ?,");
        sBuilder.append(" `is_inited` = '").append(is_inited ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_php_schedule_id)) sBuilder.append(" `php_schedule_id` = '").append(php_schedule_id).append("',");
        if(isFieldMarked(FIELD_submit_count)) sBuilder.append(" `submit_count` = '").append(submit_count).append("',");
        if(isFieldMarked(FIELD_is_active)) sBuilder.append(" `is_active` = '").append(is_active ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_pre_push_time_ms)) sBuilder.append(" `pre_push_time_ms` = '").append(pre_push_time_ms).append("',");
        if(isFieldMarked(FIELD_activity_id)) sBuilder.append(" `activity_id` = '").append(activity_id).append("',");
        if(isFieldMarked(FIELD_start_time_ms)) sBuilder.append(" `start_time_ms` = '").append(start_time_ms).append("',");
        if(isFieldMarked(FIELD_end_time_ms)) sBuilder.append(" `end_time_ms` = '").append(end_time_ms).append("',");
        if(isFieldMarked(FIELD_close_time_ms)) sBuilder.append(" `close_time_ms` = '").append(close_time_ms).append("',");
        if(isFieldMarked(FIELD_group_data)) sBuilder.append(" `group_data` = ?,");
        if(isFieldMarked(FIELD_is_inited)) sBuilder.append(" `is_inited` = '").append(is_inited ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `activity_schedule` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`php_schedule_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '后台排期ID',"
                + "`submit_count` int(11) NOT NULL DEFAULT '0' COMMENT '提交次数',"
                + "`is_active` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否生效',"
                + "`pre_push_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '预下发时间',"
                + "`activity_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动id',"
                + "`start_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '开始时间',"
                + "`end_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '领奖时间',"
                + "`close_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '结束时间',"
                + "`group_data` blob NULL COMMENT '分组数据 生效后按此数据构造分组',"
                + "`is_inited` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否完成初始化',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动排期数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.ss_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//php_schedule_id
        _size+=4;//submit_count
        _size+=1;//is_active
        _size+=8;//pre_push_time_ms
        _size+=8;//activity_id
        _size+=8;//start_time_ms
        _size+=8;//end_time_ms
        _size+=8;//close_time_ms
        _size+=2;_size+=group_data.length;//group_data
        _size+=1;//is_inited
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(php_schedule_id);
        buff.putInt(submit_count);
        buff.put((byte)(is_active?1:0));
        buff.putLong(pre_push_time_ms);
        buff.putLong(activity_id);
        buff.putLong(start_time_ms);
        buff.putLong(end_time_ms);
        buff.putLong(close_time_ms);
        buff.putShort((short)(group_data == null ? 0 : group_data.length));if(null != group_data){buff.put(group_data);}
        buff.put((byte)(is_inited?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        php_schedule_id=buff.getLong();
        submit_count=buff.getInt();
        is_active=(buff.get()==1);
        pre_push_time_ms=buff.getLong();
        activity_id=buff.getLong();
        start_time_ms=buff.getLong();
        end_time_ms=buff.getLong();
        close_time_ms=buff.getLong();
        int group_data_count = buff.getShort();if(group_data_count>0){group_data = new byte[group_data_count];buff.get(group_data);}
        is_inited=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
