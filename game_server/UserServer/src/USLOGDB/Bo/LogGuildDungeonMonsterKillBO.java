package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogGuildDungeonMonsterKillBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_event_id =0;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一ID")
    private long guid;

    public static final int FIELD_date_time =2;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =3;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_guild_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "公会ID")
    private long guild_id;

    public static final int FIELD_guild_level =5;
    @DataBaseField(type = "int(11)", fieldname = "guild_level", comment = "公会等级")
    private int guild_level;

    public static final int FIELD_dungeon_id =6;
    @DataBaseField(type = "bigint(20)", fieldname = "dungeon_id", comment = "副本ID")
    private long dungeon_id;

    public static final int FIELD_dungeon_level =7;
    @DataBaseField(type = "int(11)", fieldname = "dungeon_level", comment = "副本等级")
    private int dungeon_level;

    public static final int FIELD_monster_id =8;
    @DataBaseField(type = "bigint(20)", fieldname = "monster_id", comment = "怪物ID")
    private long monster_id;

    public static final int FIELD_type =9;
    @DataBaseField(type = "int(11)", fieldname = "type", comment = "类型")
    private int type;

    public LogGuildDungeonMonsterKillBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        guild_id = 0L;
        guild_level = 0;
        dungeon_id = 0L;
        dungeon_level = 0;
        monster_id = 0L;
        type = 0;
    }

    public LogGuildDungeonMonsterKillBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        guild_id = rs.getLong(6);
        guild_level = rs.getInt(7);
        dungeon_id = rs.getLong(8);
        dungeon_level = rs.getInt(9);
        monster_id = rs.getLong(10);
        type = rs.getInt(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogGuildDungeonMonsterKillBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `guild_id`, `guild_level`, `dungeon_id`, `dungeon_level`, `monster_id`, `type`";
    }

    @Override
    public String getTableName() {
        return "`log_guild_dungeon_monster_kill`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(guild_level).append("', ");
        strBuf.append("'").append(dungeon_id).append("', ");
        strBuf.append("'").append(dungeon_level).append("', ");
        strBuf.append("'").append(monster_id).append("', ");
        strBuf.append("'").append(type).append("', ");
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

    // 事件唯一ID
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

    // 公会ID
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
    }

    // 公会等级
    public int getGuildLevel() { return this.guild_level; }
    public void setGuildLevel(BM _bm, int guild_level) {
        if(guild_level==this.guild_level) 
            return;
        this.guild_level = guild_level; 
        markField(_bm, FIELD_guild_level); 
    }
    public void saveGuildLevel(BM _bm, int guild_level) {
        if(guild_level==this.guild_level) 
            return;
        this.guild_level = guild_level;
        saveField(_bm, "guild_level", guild_level);
    }

    // 副本ID
    public long getDungeonId() { return this.dungeon_id; }
    public void setDungeonId(BM _bm, long dungeon_id) {
        if(dungeon_id==this.dungeon_id) 
            return;
        this.dungeon_id = dungeon_id; 
        markField(_bm, FIELD_dungeon_id); 
    }
    public void saveDungeonId(BM _bm, long dungeon_id) {
        if(dungeon_id==this.dungeon_id) 
            return;
        this.dungeon_id = dungeon_id;
        saveField(_bm, "dungeon_id", dungeon_id);
    }

    // 副本等级
    public int getDungeonLevel() { return this.dungeon_level; }
    public void setDungeonLevel(BM _bm, int dungeon_level) {
        if(dungeon_level==this.dungeon_level) 
            return;
        this.dungeon_level = dungeon_level; 
        markField(_bm, FIELD_dungeon_level); 
    }
    public void saveDungeonLevel(BM _bm, int dungeon_level) {
        if(dungeon_level==this.dungeon_level) 
            return;
        this.dungeon_level = dungeon_level;
        saveField(_bm, "dungeon_level", dungeon_level);
    }

    // 怪物ID
    public long getMonsterId() { return this.monster_id; }
    public void setMonsterId(BM _bm, long monster_id) {
        if(monster_id==this.monster_id) 
            return;
        this.monster_id = monster_id; 
        markField(_bm, FIELD_monster_id); 
    }
    public void saveMonsterId(BM _bm, long monster_id) {
        if(monster_id==this.monster_id) 
            return;
        this.monster_id = monster_id;
        saveField(_bm, "monster_id", monster_id);
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



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `guild_level` = '").append(guild_level).append("',");
        sBuilder.append(" `dungeon_id` = '").append(dungeon_id).append("',");
        sBuilder.append(" `dungeon_level` = '").append(dungeon_level).append("',");
        sBuilder.append(" `monster_id` = '").append(monster_id).append("',");
        sBuilder.append(" `type` = '").append(type).append("',");
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
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_guild_level)) sBuilder.append(" `guild_level` = '").append(guild_level).append("',");
        if(isFieldMarked(FIELD_dungeon_id)) sBuilder.append(" `dungeon_id` = '").append(dungeon_id).append("',");
        if(isFieldMarked(FIELD_dungeon_level)) sBuilder.append(" `dungeon_level` = '").append(dungeon_level).append("',");
        if(isFieldMarked(FIELD_monster_id)) sBuilder.append(" `monster_id` = '").append(monster_id).append("',");
        if(isFieldMarked(FIELD_type)) sBuilder.append(" `type` = '").append(type).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_guild_dungeon_monster_kill` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一ID',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '公会ID',"
                + "`guild_level` int(11) NOT NULL DEFAULT '0' COMMENT '公会等级',"
                + "`dungeon_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '副本ID',"
                + "`dungeon_level` int(11) NOT NULL DEFAULT '0' COMMENT '副本等级',"
                + "`monster_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '怪物ID',"
                + "`type` int(11) NOT NULL DEFAULT '0' COMMENT '类型',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='公会副本击杀日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guild_id
        _size+=4;//guild_level
        _size+=8;//dungeon_id
        _size+=4;//dungeon_level
        _size+=8;//monster_id
        _size+=4;//type
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
        buff.putLong(guild_id);
        buff.putInt(guild_level);
        buff.putLong(dungeon_id);
        buff.putInt(dungeon_level);
        buff.putLong(monster_id);
        buff.putInt(type);        
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
        guild_id=buff.getLong();
        guild_level=buff.getInt();
        dungeon_id=buff.getLong();
        dungeon_level=buff.getInt();
        monster_id=buff.getLong();
        type=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
