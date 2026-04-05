package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogEveningDungeonBossRebornBO extends BaseLogBo {

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

    public static final int FIELD_base_hp =4;
    @DataBaseField(type = "bigint(20)", fieldname = "base_hp", comment = "Boss基础血量")
    private long base_hp;

    public static final int FIELD_boss_hp =5;
    @DataBaseField(type = "bigint(20)", fieldname = "boss_hp", comment = "Boss当前血量")
    private long boss_hp;

    public static final int FIELD_reborn_times =6;
    @DataBaseField(type = "int(11)", fieldname = "reborn_times", comment = "复活次数 0就是初始化")
    private int reborn_times;

    public static final int FIELD_server_start_day =7;
    @DataBaseField(type = "int(11)", fieldname = "server_start_day", comment = "服务器开服天数")
    private int server_start_day;

    public LogEveningDungeonBossRebornBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        base_hp = 0L;
        boss_hp = 0L;
        reborn_times = 0;
        server_start_day = 0;
    }

    public LogEveningDungeonBossRebornBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        base_hp = rs.getLong(6);
        boss_hp = rs.getLong(7);
        reborn_times = rs.getInt(8);
        server_start_day = rs.getInt(9);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogEveningDungeonBossRebornBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `base_hp`, `boss_hp`, `reborn_times`, `server_start_day`";
    }

    @Override
    public String getTableName() {
        return "`log_evening_dungeon_boss_reborn`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(base_hp).append("', ");
        strBuf.append("'").append(boss_hp).append("', ");
        strBuf.append("'").append(reborn_times).append("', ");
        strBuf.append("'").append(server_start_day).append("', ");
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

    // Boss基础血量
    public long getBaseHp() { return this.base_hp; }
    public void setBaseHp(BM _bm, long base_hp) {
        if(base_hp==this.base_hp) 
            return;
        this.base_hp = base_hp; 
        markField(_bm, FIELD_base_hp); 
    }
    public void saveBaseHp(BM _bm, long base_hp) {
        if(base_hp==this.base_hp) 
            return;
        this.base_hp = base_hp;
        saveField(_bm, "base_hp", base_hp);
    }

    // Boss当前血量
    public long getBossHp() { return this.boss_hp; }
    public void setBossHp(BM _bm, long boss_hp) {
        if(boss_hp==this.boss_hp) 
            return;
        this.boss_hp = boss_hp; 
        markField(_bm, FIELD_boss_hp); 
    }
    public void saveBossHp(BM _bm, long boss_hp) {
        if(boss_hp==this.boss_hp) 
            return;
        this.boss_hp = boss_hp;
        saveField(_bm, "boss_hp", boss_hp);
    }

    // 复活次数 0就是初始化
    public int getRebornTimes() { return this.reborn_times; }
    public void setRebornTimes(BM _bm, int reborn_times) {
        if(reborn_times==this.reborn_times) 
            return;
        this.reborn_times = reborn_times; 
        markField(_bm, FIELD_reborn_times); 
    }
    public void saveRebornTimes(BM _bm, int reborn_times) {
        if(reborn_times==this.reborn_times) 
            return;
        this.reborn_times = reborn_times;
        saveField(_bm, "reborn_times", reborn_times);
    }

    // 服务器开服天数
    public int getServerStartDay() { return this.server_start_day; }
    public void setServerStartDay(BM _bm, int server_start_day) {
        if(server_start_day==this.server_start_day) 
            return;
        this.server_start_day = server_start_day; 
        markField(_bm, FIELD_server_start_day); 
    }
    public void saveServerStartDay(BM _bm, int server_start_day) {
        if(server_start_day==this.server_start_day) 
            return;
        this.server_start_day = server_start_day;
        saveField(_bm, "server_start_day", server_start_day);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `base_hp` = '").append(base_hp).append("',");
        sBuilder.append(" `boss_hp` = '").append(boss_hp).append("',");
        sBuilder.append(" `reborn_times` = '").append(reborn_times).append("',");
        sBuilder.append(" `server_start_day` = '").append(server_start_day).append("',");
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
        if(isFieldMarked(FIELD_base_hp)) sBuilder.append(" `base_hp` = '").append(base_hp).append("',");
        if(isFieldMarked(FIELD_boss_hp)) sBuilder.append(" `boss_hp` = '").append(boss_hp).append("',");
        if(isFieldMarked(FIELD_reborn_times)) sBuilder.append(" `reborn_times` = '").append(reborn_times).append("',");
        if(isFieldMarked(FIELD_server_start_day)) sBuilder.append(" `server_start_day` = '").append(server_start_day).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_evening_dungeon_boss_reborn` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`base_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT 'Boss基础血量',"
                + "`boss_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT 'Boss当前血量',"
                + "`reborn_times` int(11) NOT NULL DEFAULT '0' COMMENT '复活次数 0就是初始化',"
                + "`server_start_day` int(11) NOT NULL DEFAULT '0' COMMENT '服务器开服天数',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='晚间副本boss复活日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.us_log;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=8;//base_hp
        _size+=8;//boss_hp
        _size+=4;//reborn_times
        _size+=4;//server_start_day
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
        buff.putLong(base_hp);
        buff.putLong(boss_hp);
        buff.putInt(reborn_times);
        buff.putInt(server_start_day);        
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
        base_hp=buff.getLong();
        boss_hp=buff.getLong();
        reborn_times=buff.getInt();
        server_start_day=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
