package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogUsDinnerJoinBO extends BaseLogBo {

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

    public static final int FIELD_instanceId =4;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "宴会实例ID")
    private long instanceId;

    public static final int FIELD_joinerType =5;
    @DataBaseField(type = "bigint(20)", fieldname = "joinerType", comment = "赴宴对象类型")
    private long joinerType;

    public static final int FIELD_joinerId =6;
    @DataBaseField(type = "bigint(20)", fieldname = "joinerId", comment = "赴宴对象ID")
    private long joinerId;

    public static final int FIELD_costId =7;
    @DataBaseField(type = "bigint(20)", fieldname = "costId", comment = "消耗类型ID")
    private long costId;

    public static final int FIELD_gainCoin =8;
    @DataBaseField(type = "bigint(20)", fieldname = "gainCoin", comment = "获得宴会币")
    private long gainCoin;

    public static final int FIELD_gainScore =9;
    @DataBaseField(type = "bigint(20)", fieldname = "gainScore", comment = "获得宴会积分")
    private long gainScore;

    public LogUsDinnerJoinBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        instanceId = 0L;
        joinerType = 0L;
        joinerId = 0L;
        costId = 0L;
        gainCoin = 0L;
        gainScore = 0L;
    }

    public LogUsDinnerJoinBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        instanceId = rs.getLong(6);
        joinerType = rs.getLong(7);
        joinerId = rs.getLong(8);
        costId = rs.getLong(9);
        gainCoin = rs.getLong(10);
        gainScore = rs.getLong(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogUsDinnerJoinBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `instanceId`, `joinerType`, `joinerId`, `costId`, `gainCoin`, `gainScore`";
    }

    @Override
    public String getTableName() {
        return "`log_us_dinner_join`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(joinerType).append("', ");
        strBuf.append("'").append(joinerId).append("', ");
        strBuf.append("'").append(costId).append("', ");
        strBuf.append("'").append(gainCoin).append("', ");
        strBuf.append("'").append(gainScore).append("', ");
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

    // 宴会实例ID
    public long getInstanceId() { return this.instanceId; }
    public void setInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId; 
        markField(_bm, FIELD_instanceId); 
    }
    public void saveInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId;
        saveField(_bm, "instanceId", instanceId);
    }

    // 赴宴对象类型
    public long getJoinerType() { return this.joinerType; }
    public void setJoinerType(BM _bm, long joinerType) {
        if(joinerType==this.joinerType) 
            return;
        this.joinerType = joinerType; 
        markField(_bm, FIELD_joinerType); 
    }
    public void saveJoinerType(BM _bm, long joinerType) {
        if(joinerType==this.joinerType) 
            return;
        this.joinerType = joinerType;
        saveField(_bm, "joinerType", joinerType);
    }

    // 赴宴对象ID
    public long getJoinerId() { return this.joinerId; }
    public void setJoinerId(BM _bm, long joinerId) {
        if(joinerId==this.joinerId) 
            return;
        this.joinerId = joinerId; 
        markField(_bm, FIELD_joinerId); 
    }
    public void saveJoinerId(BM _bm, long joinerId) {
        if(joinerId==this.joinerId) 
            return;
        this.joinerId = joinerId;
        saveField(_bm, "joinerId", joinerId);
    }

    // 消耗类型ID
    public long getCostId() { return this.costId; }
    public void setCostId(BM _bm, long costId) {
        if(costId==this.costId) 
            return;
        this.costId = costId; 
        markField(_bm, FIELD_costId); 
    }
    public void saveCostId(BM _bm, long costId) {
        if(costId==this.costId) 
            return;
        this.costId = costId;
        saveField(_bm, "costId", costId);
    }

    // 获得宴会币
    public long getGainCoin() { return this.gainCoin; }
    public void setGainCoin(BM _bm, long gainCoin) {
        if(gainCoin==this.gainCoin) 
            return;
        this.gainCoin = gainCoin; 
        markField(_bm, FIELD_gainCoin); 
    }
    public void saveGainCoin(BM _bm, long gainCoin) {
        if(gainCoin==this.gainCoin) 
            return;
        this.gainCoin = gainCoin;
        saveField(_bm, "gainCoin", gainCoin);
    }

    // 获得宴会积分
    public long getGainScore() { return this.gainScore; }
    public void setGainScore(BM _bm, long gainScore) {
        if(gainScore==this.gainScore) 
            return;
        this.gainScore = gainScore; 
        markField(_bm, FIELD_gainScore); 
    }
    public void saveGainScore(BM _bm, long gainScore) {
        if(gainScore==this.gainScore) 
            return;
        this.gainScore = gainScore;
        saveField(_bm, "gainScore", gainScore);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `joinerType` = '").append(joinerType).append("',");
        sBuilder.append(" `joinerId` = '").append(joinerId).append("',");
        sBuilder.append(" `costId` = '").append(costId).append("',");
        sBuilder.append(" `gainCoin` = '").append(gainCoin).append("',");
        sBuilder.append(" `gainScore` = '").append(gainScore).append("',");
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
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_joinerType)) sBuilder.append(" `joinerType` = '").append(joinerType).append("',");
        if(isFieldMarked(FIELD_joinerId)) sBuilder.append(" `joinerId` = '").append(joinerId).append("',");
        if(isFieldMarked(FIELD_costId)) sBuilder.append(" `costId` = '").append(costId).append("',");
        if(isFieldMarked(FIELD_gainCoin)) sBuilder.append(" `gainCoin` = '").append(gainCoin).append("',");
        if(isFieldMarked(FIELD_gainScore)) sBuilder.append(" `gainScore` = '").append(gainScore).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_us_dinner_join` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '宴会实例ID',"
                + "`joinerType` bigint(20) NOT NULL DEFAULT '0' COMMENT '赴宴对象类型',"
                + "`joinerId` bigint(20) NOT NULL DEFAULT '0' COMMENT '赴宴对象ID',"
                + "`costId` bigint(20) NOT NULL DEFAULT '0' COMMENT '消耗类型ID',"
                + "`gainCoin` bigint(20) NOT NULL DEFAULT '0' COMMENT '获得宴会币',"
                + "`gainScore` bigint(20) NOT NULL DEFAULT '0' COMMENT '获得宴会积分',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='宴会玩家赴宴日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//instanceId
        _size+=8;//joinerType
        _size+=8;//joinerId
        _size+=8;//costId
        _size+=8;//gainCoin
        _size+=8;//gainScore
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
        buff.putLong(instanceId);
        buff.putLong(joinerType);
        buff.putLong(joinerId);
        buff.putLong(costId);
        buff.putLong(gainCoin);
        buff.putLong(gainScore);        
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
        instanceId=buff.getLong();
        joinerType=buff.getLong();
        joinerId=buff.getLong();
        costId=buff.getLong();
        gainCoin=buff.getLong();
        gainScore=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
