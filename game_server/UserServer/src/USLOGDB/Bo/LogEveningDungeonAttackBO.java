package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogEveningDungeonAttackBO extends BaseLogBo {

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

    public static final int FIELD_cid =4;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_player_level =5;
    @DataBaseField(type = "int(11)", fieldname = "player_level", comment = "玩家等级")
    private int player_level;

    public static final int FIELD_total_hero_power =6;
    @DataBaseField(type = "bigint(20)", fieldname = "total_hero_power", comment = "玩家总伙伴实力")
    private long total_hero_power;

    public static final int FIELD_damage =7;
    @DataBaseField(type = "bigint(20)", fieldname = "damage", comment = "造成的实际伤害")
    private long damage;

    public static final int FIELD_hero_exp_reward =8;
    @DataBaseField(type = "bigint(20)", fieldname = "hero_exp_reward", comment = "获得伙伴经验奖励数量")
    private long hero_exp_reward;

    public static final int FIELD_is_killed =9;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_killed", comment = "是否击杀")
    private boolean is_killed;

    public LogEveningDungeonAttackBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        cid = 0L;
        player_level = 0;
        total_hero_power = 0L;
        damage = 0L;
        hero_exp_reward = 0L;
        is_killed = false;
    }

    public LogEveningDungeonAttackBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        cid = rs.getLong(6);
        player_level = rs.getInt(7);
        total_hero_power = rs.getLong(8);
        damage = rs.getLong(9);
        hero_exp_reward = rs.getLong(10);
        is_killed = rs.getBoolean(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogEveningDungeonAttackBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `cid`, `player_level`, `total_hero_power`, `damage`, `hero_exp_reward`, `is_killed`";
    }

    @Override
    public String getTableName() {
        return "`log_evening_dungeon_attack`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(player_level).append("', ");
        strBuf.append("'").append(total_hero_power).append("', ");
        strBuf.append("'").append(damage).append("', ");
        strBuf.append("'").append(hero_exp_reward).append("', ");
        strBuf.append("'").append(is_killed ? 1 : 0).append("', ");
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

    // 玩家等级
    public int getPlayerLevel() { return this.player_level; }
    public void setPlayerLevel(BM _bm, int player_level) {
        if(player_level==this.player_level) 
            return;
        this.player_level = player_level; 
        markField(_bm, FIELD_player_level); 
    }
    public void savePlayerLevel(BM _bm, int player_level) {
        if(player_level==this.player_level) 
            return;
        this.player_level = player_level;
        saveField(_bm, "player_level", player_level);
    }

    // 玩家总伙伴实力
    public long getTotalHeroPower() { return this.total_hero_power; }
    public void setTotalHeroPower(BM _bm, long total_hero_power) {
        if(total_hero_power==this.total_hero_power) 
            return;
        this.total_hero_power = total_hero_power; 
        markField(_bm, FIELD_total_hero_power); 
    }
    public void saveTotalHeroPower(BM _bm, long total_hero_power) {
        if(total_hero_power==this.total_hero_power) 
            return;
        this.total_hero_power = total_hero_power;
        saveField(_bm, "total_hero_power", total_hero_power);
    }

    // 造成的实际伤害
    public long getDamage() { return this.damage; }
    public void setDamage(BM _bm, long damage) {
        if(damage==this.damage) 
            return;
        this.damage = damage; 
        markField(_bm, FIELD_damage); 
    }
    public void saveDamage(BM _bm, long damage) {
        if(damage==this.damage) 
            return;
        this.damage = damage;
        saveField(_bm, "damage", damage);
    }

    // 获得伙伴经验奖励数量
    public long getHeroExpReward() { return this.hero_exp_reward; }
    public void setHeroExpReward(BM _bm, long hero_exp_reward) {
        if(hero_exp_reward==this.hero_exp_reward) 
            return;
        this.hero_exp_reward = hero_exp_reward; 
        markField(_bm, FIELD_hero_exp_reward); 
    }
    public void saveHeroExpReward(BM _bm, long hero_exp_reward) {
        if(hero_exp_reward==this.hero_exp_reward) 
            return;
        this.hero_exp_reward = hero_exp_reward;
        saveField(_bm, "hero_exp_reward", hero_exp_reward);
    }

    // 是否击杀
    public boolean getIsKilled() { return this.is_killed; }
    public void setIsKilled(BM _bm, boolean is_killed) {
        if(is_killed==this.is_killed) 
            return;
        this.is_killed = is_killed; 
        markField(_bm, FIELD_is_killed); 
    }
    public void saveIsKilled(BM _bm, boolean is_killed) {
        if(is_killed==this.is_killed) 
            return;
        this.is_killed = is_killed;
        saveField(_bm, "is_killed", is_killed ? 1 : 0);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `player_level` = '").append(player_level).append("',");
        sBuilder.append(" `total_hero_power` = '").append(total_hero_power).append("',");
        sBuilder.append(" `damage` = '").append(damage).append("',");
        sBuilder.append(" `hero_exp_reward` = '").append(hero_exp_reward).append("',");
        sBuilder.append(" `is_killed` = '").append(is_killed ? 1 : 0).append("',");
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
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_player_level)) sBuilder.append(" `player_level` = '").append(player_level).append("',");
        if(isFieldMarked(FIELD_total_hero_power)) sBuilder.append(" `total_hero_power` = '").append(total_hero_power).append("',");
        if(isFieldMarked(FIELD_damage)) sBuilder.append(" `damage` = '").append(damage).append("',");
        if(isFieldMarked(FIELD_hero_exp_reward)) sBuilder.append(" `hero_exp_reward` = '").append(hero_exp_reward).append("',");
        if(isFieldMarked(FIELD_is_killed)) sBuilder.append(" `is_killed` = '").append(is_killed ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_evening_dungeon_attack` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`player_level` int(11) NOT NULL DEFAULT '0' COMMENT '玩家等级',"
                + "`total_hero_power` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家总伙伴实力',"
                + "`damage` bigint(20) NOT NULL DEFAULT '0' COMMENT '造成的实际伤害',"
                + "`hero_exp_reward` bigint(20) NOT NULL DEFAULT '0' COMMENT '获得伙伴经验奖励数量',"
                + "`is_killed` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否击杀',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='晚间副本攻击日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=4;//player_level
        _size+=8;//total_hero_power
        _size+=8;//damage
        _size+=8;//hero_exp_reward
        _size+=1;//is_killed
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
        buff.putLong(cid);
        buff.putInt(player_level);
        buff.putLong(total_hero_power);
        buff.putLong(damage);
        buff.putLong(hero_exp_reward);
        buff.put((byte)(is_killed?1:0));        
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
        cid=buff.getLong();
        player_level=buff.getInt();
        total_hero_power=buff.getLong();
        damage=buff.getLong();
        hero_exp_reward=buff.getLong();
        is_killed=(buff.get()==1); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
