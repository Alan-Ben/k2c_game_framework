package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogUsMarsMineOccupyBO extends BaseLogBo {

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

    public static final int FIELD_mineInstanceId =4;
    @DataBaseField(type = "bigint(20)", fieldname = "mineInstanceId", comment = "火星矿实例ID")
    private long mineInstanceId;

    public static final int FIELD_mineRefId =5;
    @DataBaseField(type = "bigint(20)", fieldname = "mineRefId", comment = "火星矿配置ID")
    private long mineRefId;

    public static final int FIELD_cid =6;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "结算玩家CID")
    private long cid;

    public static final int FIELD_teamId =7;
    @DataBaseField(type = "bigint(20)", fieldname = "teamId", comment = "结算队伍ID")
    private long teamId;

    public static final int FIELD_remainNum =8;
    @DataBaseField(type = "bigint(20)", fieldname = "remainNum", comment = "剩余物品物品数量")
    private long remainNum;

    public static final int FIELD_lossValue =9;
    @DataBaseField(type = "bigint(20)", fieldname = "lossValue", comment = "队伍损耗数量")
    private long lossValue;

    public static final int FIELD_collectSpeed =10;
    @DataBaseField(type = "bigint(20)", fieldname = "collectSpeed", comment = "采集速度")
    private long collectSpeed;

    public static final int FIELD_startCollectMs =11;
    @DataBaseField(type = "bigint(20)", fieldname = "startCollectMs", comment = "开启采集时间（毫秒）")
    private long startCollectMs;

    public static final int FIELD_endCollectMs =12;
    @DataBaseField(type = "bigint(20)", fieldname = "endCollectMs", comment = "结束采集时间（毫秒）")
    private long endCollectMs;

    public LogUsMarsMineOccupyBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        mineInstanceId = 0L;
        mineRefId = 0L;
        cid = 0L;
        teamId = 0L;
        remainNum = 0L;
        lossValue = 0L;
        collectSpeed = 0L;
        startCollectMs = 0L;
        endCollectMs = 0L;
    }

    public LogUsMarsMineOccupyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        mineInstanceId = rs.getLong(6);
        mineRefId = rs.getLong(7);
        cid = rs.getLong(8);
        teamId = rs.getLong(9);
        remainNum = rs.getLong(10);
        lossValue = rs.getLong(11);
        collectSpeed = rs.getLong(12);
        startCollectMs = rs.getLong(13);
        endCollectMs = rs.getLong(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogUsMarsMineOccupyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `mineInstanceId`, `mineRefId`, `cid`, `teamId`, `remainNum`, `lossValue`, `collectSpeed`, `startCollectMs`, `endCollectMs`";
    }

    @Override
    public String getTableName() {
        return "`log_us_mars_mine_occupy`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(mineInstanceId).append("', ");
        strBuf.append("'").append(mineRefId).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(teamId).append("', ");
        strBuf.append("'").append(remainNum).append("', ");
        strBuf.append("'").append(lossValue).append("', ");
        strBuf.append("'").append(collectSpeed).append("', ");
        strBuf.append("'").append(startCollectMs).append("', ");
        strBuf.append("'").append(endCollectMs).append("', ");
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

    // 火星矿实例ID
    public long getMineInstanceId() { return this.mineInstanceId; }
    public void setMineInstanceId(BM _bm, long mineInstanceId) {
        if(mineInstanceId==this.mineInstanceId) 
            return;
        this.mineInstanceId = mineInstanceId; 
        markField(_bm, FIELD_mineInstanceId); 
    }
    public void saveMineInstanceId(BM _bm, long mineInstanceId) {
        if(mineInstanceId==this.mineInstanceId) 
            return;
        this.mineInstanceId = mineInstanceId;
        saveField(_bm, "mineInstanceId", mineInstanceId);
    }

    // 火星矿配置ID
    public long getMineRefId() { return this.mineRefId; }
    public void setMineRefId(BM _bm, long mineRefId) {
        if(mineRefId==this.mineRefId) 
            return;
        this.mineRefId = mineRefId; 
        markField(_bm, FIELD_mineRefId); 
    }
    public void saveMineRefId(BM _bm, long mineRefId) {
        if(mineRefId==this.mineRefId) 
            return;
        this.mineRefId = mineRefId;
        saveField(_bm, "mineRefId", mineRefId);
    }

    // 结算玩家CID
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

    // 结算队伍ID
    public long getTeamId() { return this.teamId; }
    public void setTeamId(BM _bm, long teamId) {
        if(teamId==this.teamId) 
            return;
        this.teamId = teamId; 
        markField(_bm, FIELD_teamId); 
    }
    public void saveTeamId(BM _bm, long teamId) {
        if(teamId==this.teamId) 
            return;
        this.teamId = teamId;
        saveField(_bm, "teamId", teamId);
    }

    // 剩余物品物品数量
    public long getRemainNum() { return this.remainNum; }
    public void setRemainNum(BM _bm, long remainNum) {
        if(remainNum==this.remainNum) 
            return;
        this.remainNum = remainNum; 
        markField(_bm, FIELD_remainNum); 
    }
    public void saveRemainNum(BM _bm, long remainNum) {
        if(remainNum==this.remainNum) 
            return;
        this.remainNum = remainNum;
        saveField(_bm, "remainNum", remainNum);
    }

    // 队伍损耗数量
    public long getLossValue() { return this.lossValue; }
    public void setLossValue(BM _bm, long lossValue) {
        if(lossValue==this.lossValue) 
            return;
        this.lossValue = lossValue; 
        markField(_bm, FIELD_lossValue); 
    }
    public void saveLossValue(BM _bm, long lossValue) {
        if(lossValue==this.lossValue) 
            return;
        this.lossValue = lossValue;
        saveField(_bm, "lossValue", lossValue);
    }

    // 采集速度
    public long getCollectSpeed() { return this.collectSpeed; }
    public void setCollectSpeed(BM _bm, long collectSpeed) {
        if(collectSpeed==this.collectSpeed) 
            return;
        this.collectSpeed = collectSpeed; 
        markField(_bm, FIELD_collectSpeed); 
    }
    public void saveCollectSpeed(BM _bm, long collectSpeed) {
        if(collectSpeed==this.collectSpeed) 
            return;
        this.collectSpeed = collectSpeed;
        saveField(_bm, "collectSpeed", collectSpeed);
    }

    // 开启采集时间（毫秒）
    public long getStartCollectMs() { return this.startCollectMs; }
    public void setStartCollectMs(BM _bm, long startCollectMs) {
        if(startCollectMs==this.startCollectMs) 
            return;
        this.startCollectMs = startCollectMs; 
        markField(_bm, FIELD_startCollectMs); 
    }
    public void saveStartCollectMs(BM _bm, long startCollectMs) {
        if(startCollectMs==this.startCollectMs) 
            return;
        this.startCollectMs = startCollectMs;
        saveField(_bm, "startCollectMs", startCollectMs);
    }

    // 结束采集时间（毫秒）
    public long getEndCollectMs() { return this.endCollectMs; }
    public void setEndCollectMs(BM _bm, long endCollectMs) {
        if(endCollectMs==this.endCollectMs) 
            return;
        this.endCollectMs = endCollectMs; 
        markField(_bm, FIELD_endCollectMs); 
    }
    public void saveEndCollectMs(BM _bm, long endCollectMs) {
        if(endCollectMs==this.endCollectMs) 
            return;
        this.endCollectMs = endCollectMs;
        saveField(_bm, "endCollectMs", endCollectMs);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `mineInstanceId` = '").append(mineInstanceId).append("',");
        sBuilder.append(" `mineRefId` = '").append(mineRefId).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `teamId` = '").append(teamId).append("',");
        sBuilder.append(" `remainNum` = '").append(remainNum).append("',");
        sBuilder.append(" `lossValue` = '").append(lossValue).append("',");
        sBuilder.append(" `collectSpeed` = '").append(collectSpeed).append("',");
        sBuilder.append(" `startCollectMs` = '").append(startCollectMs).append("',");
        sBuilder.append(" `endCollectMs` = '").append(endCollectMs).append("',");
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
        if(isFieldMarked(FIELD_mineInstanceId)) sBuilder.append(" `mineInstanceId` = '").append(mineInstanceId).append("',");
        if(isFieldMarked(FIELD_mineRefId)) sBuilder.append(" `mineRefId` = '").append(mineRefId).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_teamId)) sBuilder.append(" `teamId` = '").append(teamId).append("',");
        if(isFieldMarked(FIELD_remainNum)) sBuilder.append(" `remainNum` = '").append(remainNum).append("',");
        if(isFieldMarked(FIELD_lossValue)) sBuilder.append(" `lossValue` = '").append(lossValue).append("',");
        if(isFieldMarked(FIELD_collectSpeed)) sBuilder.append(" `collectSpeed` = '").append(collectSpeed).append("',");
        if(isFieldMarked(FIELD_startCollectMs)) sBuilder.append(" `startCollectMs` = '").append(startCollectMs).append("',");
        if(isFieldMarked(FIELD_endCollectMs)) sBuilder.append(" `endCollectMs` = '").append(endCollectMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_us_mars_mine_occupy` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`mineInstanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '火星矿实例ID',"
                + "`mineRefId` bigint(20) NOT NULL DEFAULT '0' COMMENT '火星矿配置ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '结算玩家CID',"
                + "`teamId` bigint(20) NOT NULL DEFAULT '0' COMMENT '结算队伍ID',"
                + "`remainNum` bigint(20) NOT NULL DEFAULT '0' COMMENT '剩余物品物品数量',"
                + "`lossValue` bigint(20) NOT NULL DEFAULT '0' COMMENT '队伍损耗数量',"
                + "`collectSpeed` bigint(20) NOT NULL DEFAULT '0' COMMENT '采集速度',"
                + "`startCollectMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '开启采集时间（毫秒）',"
                + "`endCollectMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '结束采集时间（毫秒）',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='US火星矿占领日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//mineInstanceId
        _size+=8;//mineRefId
        _size+=8;//cid
        _size+=8;//teamId
        _size+=8;//remainNum
        _size+=8;//lossValue
        _size+=8;//collectSpeed
        _size+=8;//startCollectMs
        _size+=8;//endCollectMs
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
        buff.putLong(mineInstanceId);
        buff.putLong(mineRefId);
        buff.putLong(cid);
        buff.putLong(teamId);
        buff.putLong(remainNum);
        buff.putLong(lossValue);
        buff.putLong(collectSpeed);
        buff.putLong(startCollectMs);
        buff.putLong(endCollectMs);        
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
        mineInstanceId=buff.getLong();
        mineRefId=buff.getLong();
        cid=buff.getLong();
        teamId=buff.getLong();
        remainNum=buff.getLong();
        lossValue=buff.getLong();
        collectSpeed=buff.getLong();
        startCollectMs=buff.getLong();
        endCollectMs=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
