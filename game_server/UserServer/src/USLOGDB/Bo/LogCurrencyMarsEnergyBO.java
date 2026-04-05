package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogCurrencyMarsEnergyBO extends BaseLogBo {

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

    public static final int FIELD_player_level =5;
    @DataBaseField(type = "int(11)", fieldname = "player_level", comment = "玩家等级")
    private int player_level;

    public static final int FIELD_chgValue =6;
    @DataBaseField(type = "bigint(20)", fieldname = "chgValue", comment = "金币变化数量")
    private long chgValue;

    public static final int FIELD_value =7;
    @DataBaseField(type = "bigint(20)", fieldname = "value", comment = "金币数量")
    private long value;

    public static final int FIELD_costSpeed =8;
    @DataBaseField(type = "bigint(20)", fieldname = "costSpeed", comment = "消耗速度（单位：分钟）")
    private long costSpeed;

    public static final int FIELD_lastSettleTimeMs =9;
    @DataBaseField(type = "bigint(20)", fieldname = "lastSettleTimeMs", comment = "上次结算时间（毫秒）")
    private long lastSettleTimeMs;

    public static final int FIELD_totalGainCount =10;
    @DataBaseField(type = "bigint(20)", fieldname = "totalGainCount", comment = "总获得数量")
    private long totalGainCount;

    public static final int FIELD_totalConsumeCount =11;
    @DataBaseField(type = "bigint(20)", fieldname = "totalConsumeCount", comment = "总消耗数量")
    private long totalConsumeCount;

    public LogCurrencyMarsEnergyBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        player_level = 0;
        chgValue = 0L;
        value = 0L;
        costSpeed = 0L;
        lastSettleTimeMs = 0L;
        totalGainCount = 0L;
        totalConsumeCount = 0L;
    }

    public LogCurrencyMarsEnergyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        player_level = rs.getInt(7);
        chgValue = rs.getLong(8);
        value = rs.getLong(9);
        costSpeed = rs.getLong(10);
        lastSettleTimeMs = rs.getLong(11);
        totalGainCount = rs.getLong(12);
        totalConsumeCount = rs.getLong(13);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogCurrencyMarsEnergyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `player_level`, `chgValue`, `value`, `costSpeed`, `lastSettleTimeMs`, `totalGainCount`, `totalConsumeCount`";
    }

    @Override
    public String getTableName() {
        return "`log_currency_mars_energy`";
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
        strBuf.append("'").append(player_level).append("', ");
        strBuf.append("'").append(chgValue).append("', ");
        strBuf.append("'").append(value).append("', ");
        strBuf.append("'").append(costSpeed).append("', ");
        strBuf.append("'").append(lastSettleTimeMs).append("', ");
        strBuf.append("'").append(totalGainCount).append("', ");
        strBuf.append("'").append(totalConsumeCount).append("', ");
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

    // 金币变化数量
    public long getChgValue() { return this.chgValue; }
    public void setChgValue(BM _bm, long chgValue) {
        if(chgValue==this.chgValue) 
            return;
        this.chgValue = chgValue; 
        markField(_bm, FIELD_chgValue); 
    }
    public void saveChgValue(BM _bm, long chgValue) {
        if(chgValue==this.chgValue) 
            return;
        this.chgValue = chgValue;
        saveField(_bm, "chgValue", chgValue);
    }

    // 金币数量
    public long getValue() { return this.value; }
    public void setValue(BM _bm, long value) {
        if(value==this.value) 
            return;
        this.value = value; 
        markField(_bm, FIELD_value); 
    }
    public void saveValue(BM _bm, long value) {
        if(value==this.value) 
            return;
        this.value = value;
        saveField(_bm, "value", value);
    }

    // 消耗速度（单位：分钟）
    public long getCostSpeed() { return this.costSpeed; }
    public void setCostSpeed(BM _bm, long costSpeed) {
        if(costSpeed==this.costSpeed) 
            return;
        this.costSpeed = costSpeed; 
        markField(_bm, FIELD_costSpeed); 
    }
    public void saveCostSpeed(BM _bm, long costSpeed) {
        if(costSpeed==this.costSpeed) 
            return;
        this.costSpeed = costSpeed;
        saveField(_bm, "costSpeed", costSpeed);
    }

    // 上次结算时间（毫秒）
    public long getLastSettleTimeMs() { return this.lastSettleTimeMs; }
    public void setLastSettleTimeMs(BM _bm, long lastSettleTimeMs) {
        if(lastSettleTimeMs==this.lastSettleTimeMs) 
            return;
        this.lastSettleTimeMs = lastSettleTimeMs; 
        markField(_bm, FIELD_lastSettleTimeMs); 
    }
    public void saveLastSettleTimeMs(BM _bm, long lastSettleTimeMs) {
        if(lastSettleTimeMs==this.lastSettleTimeMs) 
            return;
        this.lastSettleTimeMs = lastSettleTimeMs;
        saveField(_bm, "lastSettleTimeMs", lastSettleTimeMs);
    }

    // 总获得数量
    public long getTotalGainCount() { return this.totalGainCount; }
    public void setTotalGainCount(BM _bm, long totalGainCount) {
        if(totalGainCount==this.totalGainCount) 
            return;
        this.totalGainCount = totalGainCount; 
        markField(_bm, FIELD_totalGainCount); 
    }
    public void saveTotalGainCount(BM _bm, long totalGainCount) {
        if(totalGainCount==this.totalGainCount) 
            return;
        this.totalGainCount = totalGainCount;
        saveField(_bm, "totalGainCount", totalGainCount);
    }

    // 总消耗数量
    public long getTotalConsumeCount() { return this.totalConsumeCount; }
    public void setTotalConsumeCount(BM _bm, long totalConsumeCount) {
        if(totalConsumeCount==this.totalConsumeCount) 
            return;
        this.totalConsumeCount = totalConsumeCount; 
        markField(_bm, FIELD_totalConsumeCount); 
    }
    public void saveTotalConsumeCount(BM _bm, long totalConsumeCount) {
        if(totalConsumeCount==this.totalConsumeCount) 
            return;
        this.totalConsumeCount = totalConsumeCount;
        saveField(_bm, "totalConsumeCount", totalConsumeCount);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `player_level` = '").append(player_level).append("',");
        sBuilder.append(" `chgValue` = '").append(chgValue).append("',");
        sBuilder.append(" `value` = '").append(value).append("',");
        sBuilder.append(" `costSpeed` = '").append(costSpeed).append("',");
        sBuilder.append(" `lastSettleTimeMs` = '").append(lastSettleTimeMs).append("',");
        sBuilder.append(" `totalGainCount` = '").append(totalGainCount).append("',");
        sBuilder.append(" `totalConsumeCount` = '").append(totalConsumeCount).append("',");
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
        if(isFieldMarked(FIELD_player_level)) sBuilder.append(" `player_level` = '").append(player_level).append("',");
        if(isFieldMarked(FIELD_chgValue)) sBuilder.append(" `chgValue` = '").append(chgValue).append("',");
        if(isFieldMarked(FIELD_value)) sBuilder.append(" `value` = '").append(value).append("',");
        if(isFieldMarked(FIELD_costSpeed)) sBuilder.append(" `costSpeed` = '").append(costSpeed).append("',");
        if(isFieldMarked(FIELD_lastSettleTimeMs)) sBuilder.append(" `lastSettleTimeMs` = '").append(lastSettleTimeMs).append("',");
        if(isFieldMarked(FIELD_totalGainCount)) sBuilder.append(" `totalGainCount` = '").append(totalGainCount).append("',");
        if(isFieldMarked(FIELD_totalConsumeCount)) sBuilder.append(" `totalConsumeCount` = '").append(totalConsumeCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_currency_mars_energy` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`player_level` int(11) NOT NULL DEFAULT '0' COMMENT '玩家等级',"
                + "`chgValue` bigint(20) NOT NULL DEFAULT '0' COMMENT '金币变化数量',"
                + "`value` bigint(20) NOT NULL DEFAULT '0' COMMENT '金币数量',"
                + "`costSpeed` bigint(20) NOT NULL DEFAULT '0' COMMENT '消耗速度（单位：分钟）',"
                + "`lastSettleTimeMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次结算时间（毫秒）',"
                + "`totalGainCount` bigint(20) NOT NULL DEFAULT '0' COMMENT '总获得数量',"
                + "`totalConsumeCount` bigint(20) NOT NULL DEFAULT '0' COMMENT '总消耗数量',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='货币（火星能量）数据日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//player_level
        _size+=8;//chgValue
        _size+=8;//value
        _size+=8;//costSpeed
        _size+=8;//lastSettleTimeMs
        _size+=8;//totalGainCount
        _size+=8;//totalConsumeCount
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
        buff.putInt(player_level);
        buff.putLong(chgValue);
        buff.putLong(value);
        buff.putLong(costSpeed);
        buff.putLong(lastSettleTimeMs);
        buff.putLong(totalGainCount);
        buff.putLong(totalConsumeCount);        
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
        player_level=buff.getInt();
        chgValue=buff.getLong();
        value=buff.getLong();
        costSpeed=buff.getLong();
        lastSettleTimeMs=buff.getLong();
        totalGainCount=buff.getLong();
        totalConsumeCount=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
