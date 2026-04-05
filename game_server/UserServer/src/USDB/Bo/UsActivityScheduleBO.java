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
public class UsActivityScheduleBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_schedule_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "schedule_id", comment = "排期ID")
    private long schedule_id;

    public static final int FIELD_us_group_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "us_group_id", comment = "US分组ID")
    private long us_group_id;

    public static final int FIELD_cross_instance_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "cross_instance_id", comment = "跨服实例ID")
    private long cross_instance_id;

    public static final int FIELD_game_logic_instance_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "game_logic_instance_id", comment = "游戏逻辑主体实例ID")
    private long game_logic_instance_id;

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

    public static final int FIELD_is_registered =8;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_registered", comment = "已注册")
    private boolean is_registered;

    public static final int FIELD_is_done =9;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_done", comment = "活动已完成")
    private boolean is_done;

    public static final int FIELD_us_id_list =10;
    @DataBaseField(type = "blob", fieldname = "us_id_list", comment = "usId列表")
    private byte[] us_id_list;

    public static final int FIELD_res_file_name =11;
    @DataBaseField(type = "varchar(1024)", fieldname = "res_file_name", comment = "资源文件名")
    private String res_file_name;

    public static final int FIELD_res_file_md5 =12;
    @DataBaseField(type = "varchar(1024)", fieldname = "res_file_md5", comment = "资源文件md5")
    private String res_file_md5;

    public static final int FIELD_res_file_dir =13;
    @DataBaseField(type = "varchar(1024)", fieldname = "res_file_dir", comment = "资源文件目录")
    private String res_file_dir;

    public static final int FIELD_activity_instance_id =14;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例id")
    private long activity_instance_id;

    public static final int FIELD_submit_count =15;
    @DataBaseField(type = "int(11)", fieldname = "submit_count", comment = "提交次数")
    private int submit_count;

    public UsActivityScheduleBO() {
        id = 0;
        schedule_id = 0L;
        us_group_id = 0L;
        cross_instance_id = 0L;
        game_logic_instance_id = 0L;
        activity_id = 0L;
        start_time_ms = 0L;
        end_time_ms = 0L;
        close_time_ms = 0L;
        is_registered = false;
        is_done = false;
        us_id_list = null;
        res_file_name = "";
        res_file_md5 = "";
        res_file_dir = "";
        activity_instance_id = 0L;
        submit_count = 0;
    }

    public UsActivityScheduleBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        schedule_id = rs.getLong(2);
        us_group_id = rs.getLong(3);
        cross_instance_id = rs.getLong(4);
        game_logic_instance_id = rs.getLong(5);
        activity_id = rs.getLong(6);
        start_time_ms = rs.getLong(7);
        end_time_ms = rs.getLong(8);
        close_time_ms = rs.getLong(9);
        is_registered = rs.getBoolean(10);
        is_done = rs.getBoolean(11);
        us_id_list = rs.getBytes(12);
        res_file_name = rs.getString(13);
        res_file_md5 = rs.getString(14);
        res_file_dir = rs.getString(15);
        activity_instance_id = rs.getLong(16);
        submit_count = rs.getInt(17);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsActivityScheduleBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `schedule_id`, `us_group_id`, `cross_instance_id`, `game_logic_instance_id`, `activity_id`, `start_time_ms`, `end_time_ms`, `close_time_ms`, `is_registered`, `is_done`, `us_id_list`, `res_file_name`, `res_file_md5`, `res_file_dir`, `activity_instance_id`, `submit_count`";
    }

    @Override
    public String getTableName() {
        return "`us_activity_schedule`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(schedule_id).append("', ");
        strBuf.append("'").append(us_group_id).append("', ");
        strBuf.append("'").append(cross_instance_id).append("', ");
        strBuf.append("'").append(game_logic_instance_id).append("', ");
        strBuf.append("'").append(activity_id).append("', ");
        strBuf.append("'").append(start_time_ms).append("', ");
        strBuf.append("'").append(end_time_ms).append("', ");
        strBuf.append("'").append(close_time_ms).append("', ");
        strBuf.append("'").append(is_registered ? 1 : 0).append("', ");
        strBuf.append("'").append(is_done ? 1 : 0).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(res_file_name == null ? null : res_file_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(res_file_md5 == null ? null : res_file_md5.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(res_file_dir == null ? null : res_file_dir.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(submit_count).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(us_id_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_us_id_list)) ret.add(us_id_list);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 排期ID
    public long getScheduleId() { return this.schedule_id; }
    public void setScheduleId(BM _bm, long schedule_id) {
        if(schedule_id==this.schedule_id) 
            return;
        this.schedule_id = schedule_id; 
        markField(_bm, FIELD_schedule_id); 
    }
    public void saveScheduleId(BM _bm, long schedule_id) {
        if(schedule_id==this.schedule_id) 
            return;
        this.schedule_id = schedule_id;
        saveField(_bm, "schedule_id", schedule_id);
    }

    // US分组ID
    public long getUsGroupId() { return this.us_group_id; }
    public void setUsGroupId(BM _bm, long us_group_id) {
        if(us_group_id==this.us_group_id) 
            return;
        this.us_group_id = us_group_id; 
        markField(_bm, FIELD_us_group_id); 
    }
    public void saveUsGroupId(BM _bm, long us_group_id) {
        if(us_group_id==this.us_group_id) 
            return;
        this.us_group_id = us_group_id;
        saveField(_bm, "us_group_id", us_group_id);
    }

    // 跨服实例ID
    public long getCrossInstanceId() { return this.cross_instance_id; }
    public void setCrossInstanceId(BM _bm, long cross_instance_id) {
        if(cross_instance_id==this.cross_instance_id) 
            return;
        this.cross_instance_id = cross_instance_id; 
        markField(_bm, FIELD_cross_instance_id); 
    }
    public void saveCrossInstanceId(BM _bm, long cross_instance_id) {
        if(cross_instance_id==this.cross_instance_id) 
            return;
        this.cross_instance_id = cross_instance_id;
        saveField(_bm, "cross_instance_id", cross_instance_id);
    }

    // 游戏逻辑主体实例ID
    public long getGameLogicInstanceId() { return this.game_logic_instance_id; }
    public void setGameLogicInstanceId(BM _bm, long game_logic_instance_id) {
        if(game_logic_instance_id==this.game_logic_instance_id) 
            return;
        this.game_logic_instance_id = game_logic_instance_id; 
        markField(_bm, FIELD_game_logic_instance_id); 
    }
    public void saveGameLogicInstanceId(BM _bm, long game_logic_instance_id) {
        if(game_logic_instance_id==this.game_logic_instance_id) 
            return;
        this.game_logic_instance_id = game_logic_instance_id;
        saveField(_bm, "game_logic_instance_id", game_logic_instance_id);
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

    // 已注册
    public boolean getIsRegistered() { return this.is_registered; }
    public void setIsRegistered(BM _bm, boolean is_registered) {
        if(is_registered==this.is_registered) 
            return;
        this.is_registered = is_registered; 
        markField(_bm, FIELD_is_registered); 
    }
    public void saveIsRegistered(BM _bm, boolean is_registered) {
        if(is_registered==this.is_registered) 
            return;
        this.is_registered = is_registered;
        saveField(_bm, "is_registered", is_registered ? 1 : 0);
    }

    // 活动已完成
    public boolean getIsDone() { return this.is_done; }
    public void setIsDone(BM _bm, boolean is_done) {
        if(is_done==this.is_done) 
            return;
        this.is_done = is_done; 
        markField(_bm, FIELD_is_done); 
    }
    public void saveIsDone(BM _bm, boolean is_done) {
        if(is_done==this.is_done) 
            return;
        this.is_done = is_done;
        saveField(_bm, "is_done", is_done ? 1 : 0);
    }

    // usId列表
    public byte[] getUsIdList() { return this.us_id_list; }
    public void setUsIdList(BM _bm, byte[] us_id_list) {
        if(us_id_list==this.us_id_list) 
            return;
        this.us_id_list = us_id_list; 
        markField(_bm, FIELD_us_id_list); 
    }
    public void saveUsIdList(BM _bm, byte[] us_id_list) {
        if(us_id_list==this.us_id_list) 
            return;
        this.us_id_list = us_id_list;
        saveFieldBytes(_bm, "us_id_list", us_id_list);
    }

    // 资源文件名
    public String getResFileName() { return this.res_file_name; }
    public void setResFileName(BM _bm, String res_file_name) {
        if(res_file_name.equals(this.res_file_name)) 
            return;
        this.res_file_name = res_file_name; 
        markField(_bm, FIELD_res_file_name); 
    }
    public void saveResFileName(BM _bm, String res_file_name) {
        if(res_file_name.equals(this.res_file_name)) 
            return;
        this.res_file_name = res_file_name;
        saveField(_bm, "res_file_name", res_file_name);
    }

    // 资源文件md5
    public String getResFileMd5() { return this.res_file_md5; }
    public void setResFileMd5(BM _bm, String res_file_md5) {
        if(res_file_md5.equals(this.res_file_md5)) 
            return;
        this.res_file_md5 = res_file_md5; 
        markField(_bm, FIELD_res_file_md5); 
    }
    public void saveResFileMd5(BM _bm, String res_file_md5) {
        if(res_file_md5.equals(this.res_file_md5)) 
            return;
        this.res_file_md5 = res_file_md5;
        saveField(_bm, "res_file_md5", res_file_md5);
    }

    // 资源文件目录
    public String getResFileDir() { return this.res_file_dir; }
    public void setResFileDir(BM _bm, String res_file_dir) {
        if(res_file_dir.equals(this.res_file_dir)) 
            return;
        this.res_file_dir = res_file_dir; 
        markField(_bm, FIELD_res_file_dir); 
    }
    public void saveResFileDir(BM _bm, String res_file_dir) {
        if(res_file_dir.equals(this.res_file_dir)) 
            return;
        this.res_file_dir = res_file_dir;
        saveField(_bm, "res_file_dir", res_file_dir);
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `schedule_id` = '").append(schedule_id).append("',");
        sBuilder.append(" `us_group_id` = '").append(us_group_id).append("',");
        sBuilder.append(" `cross_instance_id` = '").append(cross_instance_id).append("',");
        sBuilder.append(" `game_logic_instance_id` = '").append(game_logic_instance_id).append("',");
        sBuilder.append(" `activity_id` = '").append(activity_id).append("',");
        sBuilder.append(" `start_time_ms` = '").append(start_time_ms).append("',");
        sBuilder.append(" `end_time_ms` = '").append(end_time_ms).append("',");
        sBuilder.append(" `close_time_ms` = '").append(close_time_ms).append("',");
        sBuilder.append(" `is_registered` = '").append(is_registered ? 1 : 0).append("',");
        sBuilder.append(" `is_done` = '").append(is_done ? 1 : 0).append("',");
        sBuilder.append(" `us_id_list` = ?,");
        sBuilder.append(" `res_file_name` = '").append(res_file_name == null ? null : res_file_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `res_file_md5` = '").append(res_file_md5 == null ? null : res_file_md5.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `res_file_dir` = '").append(res_file_dir == null ? null : res_file_dir.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `submit_count` = '").append(submit_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_schedule_id)) sBuilder.append(" `schedule_id` = '").append(schedule_id).append("',");
        if(isFieldMarked(FIELD_us_group_id)) sBuilder.append(" `us_group_id` = '").append(us_group_id).append("',");
        if(isFieldMarked(FIELD_cross_instance_id)) sBuilder.append(" `cross_instance_id` = '").append(cross_instance_id).append("',");
        if(isFieldMarked(FIELD_game_logic_instance_id)) sBuilder.append(" `game_logic_instance_id` = '").append(game_logic_instance_id).append("',");
        if(isFieldMarked(FIELD_activity_id)) sBuilder.append(" `activity_id` = '").append(activity_id).append("',");
        if(isFieldMarked(FIELD_start_time_ms)) sBuilder.append(" `start_time_ms` = '").append(start_time_ms).append("',");
        if(isFieldMarked(FIELD_end_time_ms)) sBuilder.append(" `end_time_ms` = '").append(end_time_ms).append("',");
        if(isFieldMarked(FIELD_close_time_ms)) sBuilder.append(" `close_time_ms` = '").append(close_time_ms).append("',");
        if(isFieldMarked(FIELD_is_registered)) sBuilder.append(" `is_registered` = '").append(is_registered ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_is_done)) sBuilder.append(" `is_done` = '").append(is_done ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_us_id_list)) sBuilder.append(" `us_id_list` = ?,");
        if(isFieldMarked(FIELD_res_file_name)) sBuilder.append(" `res_file_name` = '").append(res_file_name == null ? null : res_file_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_res_file_md5)) sBuilder.append(" `res_file_md5` = '").append(res_file_md5 == null ? null : res_file_md5.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_res_file_dir)) sBuilder.append(" `res_file_dir` = '").append(res_file_dir == null ? null : res_file_dir.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_submit_count)) sBuilder.append(" `submit_count` = '").append(submit_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_activity_schedule` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`schedule_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排期ID',"
                + "`us_group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT 'US分组ID',"
                + "`cross_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服实例ID',"
                + "`game_logic_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '游戏逻辑主体实例ID',"
                + "`activity_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动id',"
                + "`start_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '开始时间',"
                + "`end_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '领奖时间',"
                + "`close_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '结束时间',"
                + "`is_registered` tinyint(1) NOT NULL DEFAULT '0' COMMENT '已注册',"
                + "`is_done` tinyint(1) NOT NULL DEFAULT '0' COMMENT '活动已完成',"
                + "`us_id_list` blob NULL COMMENT 'usId列表',"
                + "`res_file_name` varchar(1024) NOT NULL DEFAULT '' COMMENT '资源文件名',"
                + "`res_file_md5` varchar(1024) NOT NULL DEFAULT '' COMMENT '资源文件md5',"
                + "`res_file_dir` varchar(1024) NOT NULL DEFAULT '' COMMENT '资源文件目录',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例id',"
                + "`submit_count` int(11) NOT NULL DEFAULT '0' COMMENT '提交次数',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='服务器排期数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//schedule_id
        _size+=8;//us_group_id
        _size+=8;//cross_instance_id
        _size+=8;//game_logic_instance_id
        _size+=8;//activity_id
        _size+=8;//start_time_ms
        _size+=8;//end_time_ms
        _size+=8;//close_time_ms
        _size+=1;//is_registered
        _size+=1;//is_done
        _size+=2;_size+=us_id_list.length;//us_id_list
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(res_file_name);//res_file_name
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(res_file_md5);//res_file_md5
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(res_file_dir);//res_file_dir
        _size+=8;//activity_instance_id
        _size+=4;//submit_count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(schedule_id);
        buff.putLong(us_group_id);
        buff.putLong(cross_instance_id);
        buff.putLong(game_logic_instance_id);
        buff.putLong(activity_id);
        buff.putLong(start_time_ms);
        buff.putLong(end_time_ms);
        buff.putLong(close_time_ms);
        buff.put((byte)(is_registered?1:0));
        buff.put((byte)(is_done?1:0));
        buff.putShort((short)(us_id_list == null ? 0 : us_id_list.length));if(null != us_id_list){buff.put(us_id_list);}
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, res_file_name);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, res_file_md5);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, res_file_dir);
        buff.putLong(activity_instance_id);
        buff.putInt(submit_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        schedule_id=buff.getLong();
        us_group_id=buff.getLong();
        cross_instance_id=buff.getLong();
        game_logic_instance_id=buff.getLong();
        activity_id=buff.getLong();
        start_time_ms=buff.getLong();
        end_time_ms=buff.getLong();
        close_time_ms=buff.getLong();
        is_registered=(buff.get()==1);
        is_done=(buff.get()==1);
        int us_id_list_count = buff.getShort();if(us_id_list_count>0){us_id_list = new byte[us_id_list_count];buff.get(us_id_list);}
        res_file_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        res_file_md5=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        res_file_dir=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        activity_instance_id=buff.getLong();
        submit_count=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
