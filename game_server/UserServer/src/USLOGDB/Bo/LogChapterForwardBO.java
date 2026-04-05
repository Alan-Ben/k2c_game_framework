package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogChapterForwardBO extends BaseLogBo {

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

    public static final int FIELD_point =7;
    @DataBaseField(type = "int(11)", fieldname = "point", comment = "点位")
    private int point;

    public static final int FIELD_power =8;
    @DataBaseField(type = "bigint(20)", fieldname = "power", comment = "当前战力")
    private long power;

    public static final int FIELD_earnings =9;
    @DataBaseField(type = "bigint(20)", fieldname = "earnings", comment = "当前赚速")
    private long earnings;

    public static final int FIELD_is_critical_hit =10;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_critical_hit", comment = "是否暴击")
    private boolean is_critical_hit;

    public static final int FIELD_gold_cost =11;
    @DataBaseField(type = "bigint(20)", fieldname = "gold_cost", comment = "消耗金币")
    private long gold_cost;

    public LogChapterForwardBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        level = 0;
        chapter_id = 0L;
        point = 0;
        power = 0L;
        earnings = 0L;
        is_critical_hit = false;
        gold_cost = 0L;
    }

    public LogChapterForwardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        level = rs.getInt(7);
        chapter_id = rs.getLong(8);
        point = rs.getInt(9);
        power = rs.getLong(10);
        earnings = rs.getLong(11);
        is_critical_hit = rs.getBoolean(12);
        gold_cost = rs.getLong(13);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogChapterForwardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `level`, `chapter_id`, `point`, `power`, `earnings`, `is_critical_hit`, `gold_cost`";
    }

    @Override
    public String getTableName() {
        return "`log_chapter_forward`";
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
        strBuf.append("'").append(point).append("', ");
        strBuf.append("'").append(power).append("', ");
        strBuf.append("'").append(earnings).append("', ");
        strBuf.append("'").append(is_critical_hit ? 1 : 0).append("', ");
        strBuf.append("'").append(gold_cost).append("', ");
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

    // 点位
    public int getPoint() { return this.point; }
    public void setPoint(BM _bm, int point) {
        if(point==this.point) 
            return;
        this.point = point; 
        markField(_bm, FIELD_point); 
    }
    public void savePoint(BM _bm, int point) {
        if(point==this.point) 
            return;
        this.point = point;
        saveField(_bm, "point", point);
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

    // 是否暴击
    public boolean getIsCriticalHit() { return this.is_critical_hit; }
    public void setIsCriticalHit(BM _bm, boolean is_critical_hit) {
        if(is_critical_hit==this.is_critical_hit) 
            return;
        this.is_critical_hit = is_critical_hit; 
        markField(_bm, FIELD_is_critical_hit); 
    }
    public void saveIsCriticalHit(BM _bm, boolean is_critical_hit) {
        if(is_critical_hit==this.is_critical_hit) 
            return;
        this.is_critical_hit = is_critical_hit;
        saveField(_bm, "is_critical_hit", is_critical_hit ? 1 : 0);
    }

    // 消耗金币
    public long getGoldCost() { return this.gold_cost; }
    public void setGoldCost(BM _bm, long gold_cost) {
        if(gold_cost==this.gold_cost) 
            return;
        this.gold_cost = gold_cost; 
        markField(_bm, FIELD_gold_cost); 
    }
    public void saveGoldCost(BM _bm, long gold_cost) {
        if(gold_cost==this.gold_cost) 
            return;
        this.gold_cost = gold_cost;
        saveField(_bm, "gold_cost", gold_cost);
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
        sBuilder.append(" `point` = '").append(point).append("',");
        sBuilder.append(" `power` = '").append(power).append("',");
        sBuilder.append(" `earnings` = '").append(earnings).append("',");
        sBuilder.append(" `is_critical_hit` = '").append(is_critical_hit ? 1 : 0).append("',");
        sBuilder.append(" `gold_cost` = '").append(gold_cost).append("',");
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
        if(isFieldMarked(FIELD_point)) sBuilder.append(" `point` = '").append(point).append("',");
        if(isFieldMarked(FIELD_power)) sBuilder.append(" `power` = '").append(power).append("',");
        if(isFieldMarked(FIELD_earnings)) sBuilder.append(" `earnings` = '").append(earnings).append("',");
        if(isFieldMarked(FIELD_is_critical_hit)) sBuilder.append(" `is_critical_hit` = '").append(is_critical_hit ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_gold_cost)) sBuilder.append(" `gold_cost` = '").append(gold_cost).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_chapter_forward` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`chapter_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '章节ID',"
                + "`point` int(11) NOT NULL DEFAULT '0' COMMENT '点位',"
                + "`power` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前战力',"
                + "`earnings` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前赚速',"
                + "`is_critical_hit` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否暴击',"
                + "`gold_cost` bigint(20) NOT NULL DEFAULT '0' COMMENT '消耗金币',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='关卡前进日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//point
        _size+=8;//power
        _size+=8;//earnings
        _size+=1;//is_critical_hit
        _size+=8;//gold_cost
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
        buff.putInt(point);
        buff.putLong(power);
        buff.putLong(earnings);
        buff.put((byte)(is_critical_hit?1:0));
        buff.putLong(gold_cost);        
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
        point=buff.getInt();
        power=buff.getLong();
        earnings=buff.getLong();
        is_critical_hit=(buff.get()==1);
        gold_cost=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
