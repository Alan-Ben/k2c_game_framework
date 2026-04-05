package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogChapterFightBossBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_event_id =1;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =3;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =4;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_level =5;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public static final int FIELD_chapter_id =6;
    @DataBaseField(type = "bigint(20)", fieldname = "chapter_id", comment = "章节ID")
    private long chapter_id;

    public static final int FIELD_power =7;
    @DataBaseField(type = "bigint(20)", fieldname = "power", comment = "当前战力")
    private long power;

    public static final int FIELD_earnings =8;
    @DataBaseField(type = "bigint(20)", fieldname = "earnings", comment = "当前赚速")
    private long earnings;

    public static final int FIELD_gold_inspire_times =9;
    @DataBaseField(type = "int(11)", fieldname = "gold_inspire_times", comment = "金币鼓舞次数")
    private int gold_inspire_times;

    public static final int FIELD_item_inspire_times =10;
    @DataBaseField(type = "int(11)", fieldname = "item_inspire_times", comment = "物品鼓舞次数")
    private int item_inspire_times;

    public static final int FIELD_crystal_inspire_times =11;
    @DataBaseField(type = "int(11)", fieldname = "crystal_inspire_times", comment = "钻石鼓舞次数")
    private int crystal_inspire_times;

    public static final int FIELD_final_power =12;
    @DataBaseField(type = "bigint(20)", fieldname = "final_power", comment = "最终战力")
    private long final_power;

    public LogChapterFightBossBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        level = 0;
        chapter_id = 0L;
        power = 0L;
        earnings = 0L;
        gold_inspire_times = 0;
        item_inspire_times = 0;
        crystal_inspire_times = 0;
        final_power = 0L;
    }

    public LogChapterFightBossBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        level = rs.getInt(7);
        chapter_id = rs.getLong(8);
        power = rs.getLong(9);
        earnings = rs.getLong(10);
        gold_inspire_times = rs.getInt(11);
        item_inspire_times = rs.getInt(12);
        crystal_inspire_times = rs.getInt(13);
        final_power = rs.getLong(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogChapterFightBossBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `level`, `chapter_id`, `power`, `earnings`, `gold_inspire_times`, `item_inspire_times`, `crystal_inspire_times`, `final_power`";
    }

    @Override
    public String getTableName() {
        return "`log_chapter_fight_boss`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(chapter_id).append("', ");
        strBuf.append("'").append(power).append("', ");
        strBuf.append("'").append(earnings).append("', ");
        strBuf.append("'").append(gold_inspire_times).append("', ");
        strBuf.append("'").append(item_inspire_times).append("', ");
        strBuf.append("'").append(crystal_inspire_times).append("', ");
        strBuf.append("'").append(final_power).append("', ");
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

    // 等级
    public int getLevel() { return this.level; }
    public void setLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level; 
        markField(_bm, FIELD_level); 
    }
    public void saveLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level;
        saveField(_bm, "level", level);
    }

    // 章节ID
    public long getChapterId() { return this.chapter_id; }
    public void setChapterId(BM _bm, long chapter_id) {
        if(chapter_id==this.chapter_id) 
            return;
        this.chapter_id = chapter_id; 
        markField(_bm, FIELD_chapter_id); 
    }
    public void saveChapterId(BM _bm, long chapter_id) {
        if(chapter_id==this.chapter_id) 
            return;
        this.chapter_id = chapter_id;
        saveField(_bm, "chapter_id", chapter_id);
    }

    // 当前战力
    public long getPower() { return this.power; }
    public void setPower(BM _bm, long power) {
        if(power==this.power) 
            return;
        this.power = power; 
        markField(_bm, FIELD_power); 
    }
    public void savePower(BM _bm, long power) {
        if(power==this.power) 
            return;
        this.power = power;
        saveField(_bm, "power", power);
    }

    // 当前赚速
    public long getEarnings() { return this.earnings; }
    public void setEarnings(BM _bm, long earnings) {
        if(earnings==this.earnings) 
            return;
        this.earnings = earnings; 
        markField(_bm, FIELD_earnings); 
    }
    public void saveEarnings(BM _bm, long earnings) {
        if(earnings==this.earnings) 
            return;
        this.earnings = earnings;
        saveField(_bm, "earnings", earnings);
    }

    // 金币鼓舞次数
    public int getGoldInspireTimes() { return this.gold_inspire_times; }
    public void setGoldInspireTimes(BM _bm, int gold_inspire_times) {
        if(gold_inspire_times==this.gold_inspire_times) 
            return;
        this.gold_inspire_times = gold_inspire_times; 
        markField(_bm, FIELD_gold_inspire_times); 
    }
    public void saveGoldInspireTimes(BM _bm, int gold_inspire_times) {
        if(gold_inspire_times==this.gold_inspire_times) 
            return;
        this.gold_inspire_times = gold_inspire_times;
        saveField(_bm, "gold_inspire_times", gold_inspire_times);
    }

    // 物品鼓舞次数
    public int getItemInspireTimes() { return this.item_inspire_times; }
    public void setItemInspireTimes(BM _bm, int item_inspire_times) {
        if(item_inspire_times==this.item_inspire_times) 
            return;
        this.item_inspire_times = item_inspire_times; 
        markField(_bm, FIELD_item_inspire_times); 
    }
    public void saveItemInspireTimes(BM _bm, int item_inspire_times) {
        if(item_inspire_times==this.item_inspire_times) 
            return;
        this.item_inspire_times = item_inspire_times;
        saveField(_bm, "item_inspire_times", item_inspire_times);
    }

    // 钻石鼓舞次数
    public int getCrystalInspireTimes() { return this.crystal_inspire_times; }
    public void setCrystalInspireTimes(BM _bm, int crystal_inspire_times) {
        if(crystal_inspire_times==this.crystal_inspire_times) 
            return;
        this.crystal_inspire_times = crystal_inspire_times; 
        markField(_bm, FIELD_crystal_inspire_times); 
    }
    public void saveCrystalInspireTimes(BM _bm, int crystal_inspire_times) {
        if(crystal_inspire_times==this.crystal_inspire_times) 
            return;
        this.crystal_inspire_times = crystal_inspire_times;
        saveField(_bm, "crystal_inspire_times", crystal_inspire_times);
    }

    // 最终战力
    public long getFinalPower() { return this.final_power; }
    public void setFinalPower(BM _bm, long final_power) {
        if(final_power==this.final_power) 
            return;
        this.final_power = final_power; 
        markField(_bm, FIELD_final_power); 
    }
    public void saveFinalPower(BM _bm, long final_power) {
        if(final_power==this.final_power) 
            return;
        this.final_power = final_power;
        saveField(_bm, "final_power", final_power);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `chapter_id` = '").append(chapter_id).append("',");
        sBuilder.append(" `power` = '").append(power).append("',");
        sBuilder.append(" `earnings` = '").append(earnings).append("',");
        sBuilder.append(" `gold_inspire_times` = '").append(gold_inspire_times).append("',");
        sBuilder.append(" `item_inspire_times` = '").append(item_inspire_times).append("',");
        sBuilder.append(" `crystal_inspire_times` = '").append(crystal_inspire_times).append("',");
        sBuilder.append(" `final_power` = '").append(final_power).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_chapter_id)) sBuilder.append(" `chapter_id` = '").append(chapter_id).append("',");
        if(isFieldMarked(FIELD_power)) sBuilder.append(" `power` = '").append(power).append("',");
        if(isFieldMarked(FIELD_earnings)) sBuilder.append(" `earnings` = '").append(earnings).append("',");
        if(isFieldMarked(FIELD_gold_inspire_times)) sBuilder.append(" `gold_inspire_times` = '").append(gold_inspire_times).append("',");
        if(isFieldMarked(FIELD_item_inspire_times)) sBuilder.append(" `item_inspire_times` = '").append(item_inspire_times).append("',");
        if(isFieldMarked(FIELD_crystal_inspire_times)) sBuilder.append(" `crystal_inspire_times` = '").append(crystal_inspire_times).append("',");
        if(isFieldMarked(FIELD_final_power)) sBuilder.append(" `final_power` = '").append(final_power).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_chapter_fight_boss` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`chapter_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '章节ID',"
                + "`power` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前战力',"
                + "`earnings` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前赚速',"
                + "`gold_inspire_times` int(11) NOT NULL DEFAULT '0' COMMENT '金币鼓舞次数',"
                + "`item_inspire_times` int(11) NOT NULL DEFAULT '0' COMMENT '物品鼓舞次数',"
                + "`crystal_inspire_times` int(11) NOT NULL DEFAULT '0' COMMENT '钻石鼓舞次数',"
                + "`final_power` bigint(20) NOT NULL DEFAULT '0' COMMENT '最终战力',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='关卡打BOSS日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=4;//level
        _size+=8;//chapter_id
        _size+=8;//power
        _size+=8;//earnings
        _size+=4;//gold_inspire_times
        _size+=4;//item_inspire_times
        _size+=4;//crystal_inspire_times
        _size+=8;//final_power
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putInt(level);
        buff.putLong(chapter_id);
        buff.putLong(power);
        buff.putLong(earnings);
        buff.putInt(gold_inspire_times);
        buff.putInt(item_inspire_times);
        buff.putInt(crystal_inspire_times);
        buff.putLong(final_power);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        level=buff.getInt();
        chapter_id=buff.getLong();
        power=buff.getLong();
        earnings=buff.getLong();
        gold_inspire_times=buff.getInt();
        item_inspire_times=buff.getInt();
        crystal_inspire_times=buff.getInt();
        final_power=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
