package SSLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogActivitySchedulePushBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_event_id =0;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =2;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =3;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_file_name =4;
    @DataBaseField(type = "varchar(500)", fieldname = "file_name", comment = "文件名")
    private String file_name;

    public static final int FIELD_php_serial =5;
    @DataBaseField(type = "varchar(500)", fieldname = "php_serial", comment = "运营推送序列号")
    private String php_serial;

    public static final int FIELD_zone_id =6;
    @DataBaseField(type = "bigint(20)", fieldname = "zone_id", comment = "大区ID")
    private long zone_id;

    public LogActivitySchedulePushBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        file_name = "";
        php_serial = "";
        zone_id = 0L;
    }

    public LogActivitySchedulePushBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        file_name = rs.getString(6);
        php_serial = rs.getString(7);
        zone_id = rs.getLong(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogActivitySchedulePushBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `file_name`, `php_serial`, `zone_id`";
    }

    @Override
    public String getTableName() {
        return "`log_activity_schedule_push`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(file_name == null ? null : file_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(php_serial == null ? null : php_serial.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(zone_id).append("', ");
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

    // 事件类型
    public int getEventId() { return this.event_id; }
    public void setEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id; 
        markField(_bm, FIELD_event_id); 
    }
    public void saveEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id;
        saveField(_bm, "event_id", event_id);
    }

    // 事件唯一id
    public long getGuid() { return this.guid; }
    public void setGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid; 
        markField(_bm, FIELD_guid); 
    }
    public void saveGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid;
        saveField(_bm, "guid", guid);
    }

    // 日期
    public int getDateTime() { return this.date_time; }
    public void setDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time; 
        markField(_bm, FIELD_date_time); 
    }
    public void saveDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time;
        saveField(_bm, "date_time", date_time);
    }

    // 时间戳
    public int getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }

    // 文件名
    public String getFileName() { return this.file_name; }
    public void setFileName(BM _bm, String file_name) {
        if(file_name.equals(this.file_name)) 
            return;
        this.file_name = file_name; 
        markField(_bm, FIELD_file_name); 
    }
    public void saveFileName(BM _bm, String file_name) {
        if(file_name.equals(this.file_name)) 
            return;
        this.file_name = file_name;
        saveField(_bm, "file_name", file_name);
    }

    // 运营推送序列号
    public String getPhpSerial() { return this.php_serial; }
    public void setPhpSerial(BM _bm, String php_serial) {
        if(php_serial.equals(this.php_serial)) 
            return;
        this.php_serial = php_serial; 
        markField(_bm, FIELD_php_serial); 
    }
    public void savePhpSerial(BM _bm, String php_serial) {
        if(php_serial.equals(this.php_serial)) 
            return;
        this.php_serial = php_serial;
        saveField(_bm, "php_serial", php_serial);
    }

    // 大区ID
    public long getZoneId() { return this.zone_id; }
    public void setZoneId(BM _bm, long zone_id) {
        if(zone_id==this.zone_id) 
            return;
        this.zone_id = zone_id; 
        markField(_bm, FIELD_zone_id); 
    }
    public void saveZoneId(BM _bm, long zone_id) {
        if(zone_id==this.zone_id) 
            return;
        this.zone_id = zone_id;
        saveField(_bm, "zone_id", zone_id);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `file_name` = '").append(file_name == null ? null : file_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `php_serial` = '").append(php_serial == null ? null : php_serial.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `zone_id` = '").append(zone_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_file_name)) sBuilder.append(" `file_name` = '").append(file_name == null ? null : file_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_php_serial)) sBuilder.append(" `php_serial` = '").append(php_serial == null ? null : php_serial.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_zone_id)) sBuilder.append(" `zone_id` = '").append(zone_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_activity_schedule_push` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`file_name` varchar(500) NOT NULL DEFAULT '' COMMENT '文件名',"
                + "`php_serial` varchar(500) NOT NULL DEFAULT '' COMMENT '运营推送序列号',"
                + "`zone_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '大区ID',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='活动排期-推送日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.ss_log;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(file_name);//file_name
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(php_serial);//php_serial
        _size+=8;//zone_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, file_name);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, php_serial);
        buff.putLong(zone_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        file_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        php_serial=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        zone_id=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
