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
public class ActivityScheduleUsInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_schedule_db_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "schedule_db_id", comment = "排期实例ID")
    private long schedule_db_id;

    public static final int FIELD_us_group_db_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "us_group_db_id", comment = "us分组实例ID")
    private long us_group_db_id;

    public static final int FIELD_us_id =2;
    @DataBaseField(type = "int(11)", fieldname = "us_id", comment = "usId")
    private int us_id;

    public static final int FIELD_had_push =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_push", comment = "是否已推送")
    private boolean had_push;

    public static final int FIELD_had_enter_settle =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_enter_settle", comment = "是否进入结算状态")
    private boolean had_enter_settle;

    public static final int FIELD_had_done =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_done", comment = "是否完成排期")
    private boolean had_done;

    public static final int FIELD_had_enter_playing =6;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_enter_playing", comment = "是否进入开启状态")
    private boolean had_enter_playing;

    public ActivityScheduleUsInfoBO() {
        id = 0;
        schedule_db_id = 0L;
        us_group_db_id = 0L;
        us_id = 0;
        had_push = false;
        had_enter_settle = false;
        had_done = false;
        had_enter_playing = false;
    }

    public ActivityScheduleUsInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        schedule_db_id = rs.getLong(2);
        us_group_db_id = rs.getLong(3);
        us_id = rs.getInt(4);
        had_push = rs.getBoolean(5);
        had_enter_settle = rs.getBoolean(6);
        had_done = rs.getBoolean(7);
        had_enter_playing = rs.getBoolean(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ActivityScheduleUsInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `schedule_db_id`, `us_group_db_id`, `us_id`, `had_push`, `had_enter_settle`, `had_done`, `had_enter_playing`";
    }

    @Override
    public String getTableName() {
        return "`activity_schedule_us_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(schedule_db_id).append("', ");
        strBuf.append("'").append(us_group_db_id).append("', ");
        strBuf.append("'").append(us_id).append("', ");
        strBuf.append("'").append(had_push ? 1 : 0).append("', ");
        strBuf.append("'").append(had_enter_settle ? 1 : 0).append("', ");
        strBuf.append("'").append(had_done ? 1 : 0).append("', ");
        strBuf.append("'").append(had_enter_playing ? 1 : 0).append("', ");
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

    // 排期实例ID
    public long getScheduleDbId() { return this.schedule_db_id; }
    public void setScheduleDbId(BM _bm, long schedule_db_id) {
        if(schedule_db_id==this.schedule_db_id) 
            return;
        this.schedule_db_id = schedule_db_id; 
        markField(_bm, FIELD_schedule_db_id); 
    }
    public void saveScheduleDbId(BM _bm, long schedule_db_id) {
        if(schedule_db_id==this.schedule_db_id) 
            return;
        this.schedule_db_id = schedule_db_id;
        saveField(_bm, "schedule_db_id", schedule_db_id);
    }

    // us分组实例ID
    public long getUsGroupDbId() { return this.us_group_db_id; }
    public void setUsGroupDbId(BM _bm, long us_group_db_id) {
        if(us_group_db_id==this.us_group_db_id) 
            return;
        this.us_group_db_id = us_group_db_id; 
        markField(_bm, FIELD_us_group_db_id); 
    }
    public void saveUsGroupDbId(BM _bm, long us_group_db_id) {
        if(us_group_db_id==this.us_group_db_id) 
            return;
        this.us_group_db_id = us_group_db_id;
        saveField(_bm, "us_group_db_id", us_group_db_id);
    }

    // usId
    public int getUsId() { return this.us_id; }
    public void setUsId(BM _bm, int us_id) {
        if(us_id==this.us_id) 
            return;
        this.us_id = us_id; 
        markField(_bm, FIELD_us_id); 
    }
    public void saveUsId(BM _bm, int us_id) {
        if(us_id==this.us_id) 
            return;
        this.us_id = us_id;
        saveField(_bm, "us_id", us_id);
    }

    // 是否已推送
    public boolean getHadPush() { return this.had_push; }
    public void setHadPush(BM _bm, boolean had_push) {
        if(had_push==this.had_push) 
            return;
        this.had_push = had_push; 
        markField(_bm, FIELD_had_push); 
    }
    public void saveHadPush(BM _bm, boolean had_push) {
        if(had_push==this.had_push) 
            return;
        this.had_push = had_push;
        saveField(_bm, "had_push", had_push ? 1 : 0);
    }

    // 是否进入结算状态
    public boolean getHadEnterSettle() { return this.had_enter_settle; }
    public void setHadEnterSettle(BM _bm, boolean had_enter_settle) {
        if(had_enter_settle==this.had_enter_settle) 
            return;
        this.had_enter_settle = had_enter_settle; 
        markField(_bm, FIELD_had_enter_settle); 
    }
    public void saveHadEnterSettle(BM _bm, boolean had_enter_settle) {
        if(had_enter_settle==this.had_enter_settle) 
            return;
        this.had_enter_settle = had_enter_settle;
        saveField(_bm, "had_enter_settle", had_enter_settle ? 1 : 0);
    }

    // 是否完成排期
    public boolean getHadDone() { return this.had_done; }
    public void setHadDone(BM _bm, boolean had_done) {
        if(had_done==this.had_done) 
            return;
        this.had_done = had_done; 
        markField(_bm, FIELD_had_done); 
    }
    public void saveHadDone(BM _bm, boolean had_done) {
        if(had_done==this.had_done) 
            return;
        this.had_done = had_done;
        saveField(_bm, "had_done", had_done ? 1 : 0);
    }

    // 是否进入开启状态
    public boolean getHadEnterPlaying() { return this.had_enter_playing; }
    public void setHadEnterPlaying(BM _bm, boolean had_enter_playing) {
        if(had_enter_playing==this.had_enter_playing) 
            return;
        this.had_enter_playing = had_enter_playing; 
        markField(_bm, FIELD_had_enter_playing); 
    }
    public void saveHadEnterPlaying(BM _bm, boolean had_enter_playing) {
        if(had_enter_playing==this.had_enter_playing) 
            return;
        this.had_enter_playing = had_enter_playing;
        saveField(_bm, "had_enter_playing", had_enter_playing ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `schedule_db_id` = '").append(schedule_db_id).append("',");
        sBuilder.append(" `us_group_db_id` = '").append(us_group_db_id).append("',");
        sBuilder.append(" `us_id` = '").append(us_id).append("',");
        sBuilder.append(" `had_push` = '").append(had_push ? 1 : 0).append("',");
        sBuilder.append(" `had_enter_settle` = '").append(had_enter_settle ? 1 : 0).append("',");
        sBuilder.append(" `had_done` = '").append(had_done ? 1 : 0).append("',");
        sBuilder.append(" `had_enter_playing` = '").append(had_enter_playing ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_schedule_db_id)) sBuilder.append(" `schedule_db_id` = '").append(schedule_db_id).append("',");
        if(isFieldMarked(FIELD_us_group_db_id)) sBuilder.append(" `us_group_db_id` = '").append(us_group_db_id).append("',");
        if(isFieldMarked(FIELD_us_id)) sBuilder.append(" `us_id` = '").append(us_id).append("',");
        if(isFieldMarked(FIELD_had_push)) sBuilder.append(" `had_push` = '").append(had_push ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_had_enter_settle)) sBuilder.append(" `had_enter_settle` = '").append(had_enter_settle ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_had_done)) sBuilder.append(" `had_done` = '").append(had_done ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_had_enter_playing)) sBuilder.append(" `had_enter_playing` = '").append(had_enter_playing ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `activity_schedule_us_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`schedule_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排期实例ID',"
                + "`us_group_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT 'us分组实例ID',"
                + "`us_id` int(11) NOT NULL DEFAULT '0' COMMENT 'usId',"
                + "`had_push` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已推送',"
                + "`had_enter_settle` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否进入结算状态',"
                + "`had_done` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否完成排期',"
                + "`had_enter_playing` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否进入开启状态',"
                + "KEY `schedule_db_id` (`schedule_db_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动排期US数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//schedule_db_id
        _size+=8;//us_group_db_id
        _size+=4;//us_id
        _size+=1;//had_push
        _size+=1;//had_enter_settle
        _size+=1;//had_done
        _size+=1;//had_enter_playing
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(schedule_db_id);
        buff.putLong(us_group_db_id);
        buff.putInt(us_id);
        buff.put((byte)(had_push?1:0));
        buff.put((byte)(had_enter_settle?1:0));
        buff.put((byte)(had_done?1:0));
        buff.put((byte)(had_enter_playing?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        schedule_db_id=buff.getLong();
        us_group_db_id=buff.getLong();
        us_id=buff.getInt();
        had_push=(buff.get()==1);
        had_enter_settle=(buff.get()==1);
        had_done=(buff.get()==1);
        had_enter_playing=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
