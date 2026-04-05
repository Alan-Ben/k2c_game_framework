package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogSectionBuildingV2BO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_sectionType =0;
    @DataBaseField(type = "int(11)", fieldname = "sectionType", comment = "截面日志类型，枚举ELogSectionType")
    private int sectionType;

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

    public static final int FIELD_cid =5;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_buildingId =6;
    @DataBaseField(type = "bigint(20)", fieldname = "buildingId", comment = "建筑ID")
    private long buildingId;

    public static final int FIELD_buildingLvl =7;
    @DataBaseField(type = "int(11)", fieldname = "buildingLvl", comment = "建筑等级")
    private int buildingLvl;

    public static final int FIELD_buildingHireNum =8;
    @DataBaseField(type = "int(11)", fieldname = "buildingHireNum", comment = "建筑人数")
    private int buildingHireNum;

    public LogSectionBuildingV2BO() {
        id = 0;
        sectionType = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        cid = 0L;
        buildingId = 0L;
        buildingLvl = 0;
        buildingHireNum = 0;
    }

    public LogSectionBuildingV2BO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        sectionType = rs.getInt(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        cid = rs.getLong(7);
        buildingId = rs.getLong(8);
        buildingLvl = rs.getInt(9);
        buildingHireNum = rs.getInt(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogSectionBuildingV2BO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `sectionType`, `event_id`, `guid`, `date_time`, `timestamp`, `cid`, `buildingId`, `buildingLvl`, `buildingHireNum`";
    }

    @Override
    public String getTableName() {
        return "`log_section_building_v2`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(sectionType).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(buildingId).append("', ");
        strBuf.append("'").append(buildingLvl).append("', ");
        strBuf.append("'").append(buildingHireNum).append("', ");
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

    // 截面日志类型，枚举ELogSectionType
    public int getSectionType() { return this.sectionType; }
    public void setSectionType(BM _bm, int sectionType) {
        if(sectionType==this.sectionType) 
            return;
        this.sectionType = sectionType; 
        markField(_bm, FIELD_sectionType); 
    }
    public void saveSectionType(BM _bm, int sectionType) {
        if(sectionType==this.sectionType) 
            return;
        this.sectionType = sectionType;
        saveField(_bm, "sectionType", sectionType);
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

    // 建筑ID
    public long getBuildingId() { return this.buildingId; }
    public void setBuildingId(BM _bm, long buildingId) {
        if(buildingId==this.buildingId) 
            return;
        this.buildingId = buildingId; 
        markField(_bm, FIELD_buildingId); 
    }
    public void saveBuildingId(BM _bm, long buildingId) {
        if(buildingId==this.buildingId) 
            return;
        this.buildingId = buildingId;
        saveField(_bm, "buildingId", buildingId);
    }

    // 建筑等级
    public int getBuildingLvl() { return this.buildingLvl; }
    public void setBuildingLvl(BM _bm, int buildingLvl) {
        if(buildingLvl==this.buildingLvl) 
            return;
        this.buildingLvl = buildingLvl; 
        markField(_bm, FIELD_buildingLvl); 
    }
    public void saveBuildingLvl(BM _bm, int buildingLvl) {
        if(buildingLvl==this.buildingLvl) 
            return;
        this.buildingLvl = buildingLvl;
        saveField(_bm, "buildingLvl", buildingLvl);
    }

    // 建筑人数
    public int getBuildingHireNum() { return this.buildingHireNum; }
    public void setBuildingHireNum(BM _bm, int buildingHireNum) {
        if(buildingHireNum==this.buildingHireNum) 
            return;
        this.buildingHireNum = buildingHireNum; 
        markField(_bm, FIELD_buildingHireNum); 
    }
    public void saveBuildingHireNum(BM _bm, int buildingHireNum) {
        if(buildingHireNum==this.buildingHireNum) 
            return;
        this.buildingHireNum = buildingHireNum;
        saveField(_bm, "buildingHireNum", buildingHireNum);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `sectionType` = '").append(sectionType).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `buildingId` = '").append(buildingId).append("',");
        sBuilder.append(" `buildingLvl` = '").append(buildingLvl).append("',");
        sBuilder.append(" `buildingHireNum` = '").append(buildingHireNum).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_sectionType)) sBuilder.append(" `sectionType` = '").append(sectionType).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_buildingId)) sBuilder.append(" `buildingId` = '").append(buildingId).append("',");
        if(isFieldMarked(FIELD_buildingLvl)) sBuilder.append(" `buildingLvl` = '").append(buildingLvl).append("',");
        if(isFieldMarked(FIELD_buildingHireNum)) sBuilder.append(" `buildingHireNum` = '").append(buildingHireNum).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_section_building_v2` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`sectionType` int(11) NOT NULL DEFAULT '0' COMMENT '截面日志类型，枚举ELogSectionType',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`buildingId` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑ID',"
                + "`buildingLvl` int(11) NOT NULL DEFAULT '0' COMMENT '建筑等级',"
                + "`buildingHireNum` int(11) NOT NULL DEFAULT '0' COMMENT '建筑人数',"
                + "KEY `sectionType` (`sectionType`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='建筑截面数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//sectionType
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=8;//cid
        _size+=8;//buildingId
        _size+=4;//buildingLvl
        _size+=4;//buildingHireNum
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(sectionType);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putLong(cid);
        buff.putLong(buildingId);
        buff.putInt(buildingLvl);
        buff.putInt(buildingHireNum);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        sectionType=buff.getInt();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        cid=buff.getLong();
        buildingId=buff.getLong();
        buildingLvl=buff.getInt();
        buildingHireNum=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
