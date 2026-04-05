package USLOGDB.OptBo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.NPLogDB.BaseOptLogBo;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class Opt015006ConsortAppointBO extends BaseOptLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_level =1;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "玩家等级")
    private int level;

    public static final int FIELD_vip_lvl =2;
    @DataBaseField(type = "int(11)", fieldname = "vip_lvl", comment = "玩家vip等级")
    private int vip_lvl;

    public static final int FIELD_event_id =3;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =4;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =5;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =6;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_consortId =7;
    @DataBaseField(type = "bigint(20)", fieldname = "consortId", comment = "妃子ID")
    private long consortId;

    public static final int FIELD_consortTravelId =8;
    @DataBaseField(type = "bigint(20)", fieldname = "consortTravelId", comment = "家人邀约类型ID")
    private long consortTravelId;

    public static final int FIELD_travelCount =9;
    @DataBaseField(type = "int(11)", fieldname = "travelCount", comment = "邀约计算次数")
    private int travelCount;

    public static final int FIELD_addCharmPoint =10;
    @DataBaseField(type = "bigint(20)", fieldname = "addCharmPoint", comment = "增加邀约点")
    private long addCharmPoint;

    public static final int FIELD_childId =11;
    @DataBaseField(type = "bigint(20)", fieldname = "childId", comment = "子嗣实例ID")
    private long childId;

    public Opt015006ConsortAppointBO() {
        id = 0;
        cid = 0L;
        level = 0;
        vip_lvl = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        consortId = 0L;
        consortTravelId = 0L;
        travelCount = 0;
        addCharmPoint = 0L;
        childId = 0L;
    }

    public Opt015006ConsortAppointBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        level = rs.getInt(3);
        vip_lvl = rs.getInt(4);
        event_id = rs.getInt(5);
        guid = rs.getLong(6);
        date_time = rs.getInt(7);
        timestamp = rs.getInt(8);
        consortId = rs.getLong(9);
        consortTravelId = rs.getLong(10);
        travelCount = rs.getInt(11);
        addCharmPoint = rs.getLong(12);
        childId = rs.getLong(13);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new Opt015006ConsortAppointBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `level`, `vip_lvl`, `event_id`, `guid`, `date_time`, `timestamp`, `consortId`, `consortTravelId`, `travelCount`, `addCharmPoint`, `childId`";
    }

    @Override
    public String getTableName() {
        return "`opt_015_006_consort_appoint`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(vip_lvl).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(consortId).append("', ");
        strBuf.append("'").append(consortTravelId).append("', ");
        strBuf.append("'").append(travelCount).append("', ");
        strBuf.append("'").append(addCharmPoint).append("', ");
        strBuf.append("'").append(childId).append("', ");
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

    // 玩家等级
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

    // 玩家vip等级
    public int getVipLvl() { return this.vip_lvl; }
    public void setVipLvl(BM _bm, int vip_lvl) {
        if(vip_lvl==this.vip_lvl) 
            return;
        this.vip_lvl = vip_lvl; 
        markField(_bm, FIELD_vip_lvl); 
    }
    public void saveVipLvl(BM _bm, int vip_lvl) {
        if(vip_lvl==this.vip_lvl) 
            return;
        this.vip_lvl = vip_lvl;
        saveField(_bm, "vip_lvl", vip_lvl);
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

    // 妃子ID
    public long getConsortId() { return this.consortId; }
    public void setConsortId(BM _bm, long consortId) {
        if(consortId==this.consortId) 
            return;
        this.consortId = consortId; 
        markField(_bm, FIELD_consortId); 
    }
    public void saveConsortId(BM _bm, long consortId) {
        if(consortId==this.consortId) 
            return;
        this.consortId = consortId;
        saveField(_bm, "consortId", consortId);
    }

    // 家人邀约类型ID
    public long getConsortTravelId() { return this.consortTravelId; }
    public void setConsortTravelId(BM _bm, long consortTravelId) {
        if(consortTravelId==this.consortTravelId) 
            return;
        this.consortTravelId = consortTravelId; 
        markField(_bm, FIELD_consortTravelId); 
    }
    public void saveConsortTravelId(BM _bm, long consortTravelId) {
        if(consortTravelId==this.consortTravelId) 
            return;
        this.consortTravelId = consortTravelId;
        saveField(_bm, "consortTravelId", consortTravelId);
    }

    // 邀约计算次数
    public int getTravelCount() { return this.travelCount; }
    public void setTravelCount(BM _bm, int travelCount) {
        if(travelCount==this.travelCount) 
            return;
        this.travelCount = travelCount; 
        markField(_bm, FIELD_travelCount); 
    }
    public void saveTravelCount(BM _bm, int travelCount) {
        if(travelCount==this.travelCount) 
            return;
        this.travelCount = travelCount;
        saveField(_bm, "travelCount", travelCount);
    }

    // 增加邀约点
    public long getAddCharmPoint() { return this.addCharmPoint; }
    public void setAddCharmPoint(BM _bm, long addCharmPoint) {
        if(addCharmPoint==this.addCharmPoint) 
            return;
        this.addCharmPoint = addCharmPoint; 
        markField(_bm, FIELD_addCharmPoint); 
    }
    public void saveAddCharmPoint(BM _bm, long addCharmPoint) {
        if(addCharmPoint==this.addCharmPoint) 
            return;
        this.addCharmPoint = addCharmPoint;
        saveField(_bm, "addCharmPoint", addCharmPoint);
    }

    // 子嗣实例ID
    public long getChildId() { return this.childId; }
    public void setChildId(BM _bm, long childId) {
        if(childId==this.childId) 
            return;
        this.childId = childId; 
        markField(_bm, FIELD_childId); 
    }
    public void saveChildId(BM _bm, long childId) {
        if(childId==this.childId) 
            return;
        this.childId = childId;
        saveField(_bm, "childId", childId);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `vip_lvl` = '").append(vip_lvl).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `consortId` = '").append(consortId).append("',");
        sBuilder.append(" `consortTravelId` = '").append(consortTravelId).append("',");
        sBuilder.append(" `travelCount` = '").append(travelCount).append("',");
        sBuilder.append(" `addCharmPoint` = '").append(addCharmPoint).append("',");
        sBuilder.append(" `childId` = '").append(childId).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_vip_lvl)) sBuilder.append(" `vip_lvl` = '").append(vip_lvl).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_consortId)) sBuilder.append(" `consortId` = '").append(consortId).append("',");
        if(isFieldMarked(FIELD_consortTravelId)) sBuilder.append(" `consortTravelId` = '").append(consortTravelId).append("',");
        if(isFieldMarked(FIELD_travelCount)) sBuilder.append(" `travelCount` = '").append(travelCount).append("',");
        if(isFieldMarked(FIELD_addCharmPoint)) sBuilder.append(" `addCharmPoint` = '").append(addCharmPoint).append("',");
        if(isFieldMarked(FIELD_childId)) sBuilder.append(" `childId` = '").append(childId).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `opt_015_006_consort_appoint` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '玩家等级',"
                + "`vip_lvl` int(11) NOT NULL DEFAULT '0' COMMENT '玩家vip等级',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`consortId` bigint(20) NOT NULL DEFAULT '0' COMMENT '妃子ID',"
                + "`consortTravelId` bigint(20) NOT NULL DEFAULT '0' COMMENT '家人邀约类型ID',"
                + "`travelCount` int(11) NOT NULL DEFAULT '0' COMMENT '邀约计算次数',"
                + "`addCharmPoint` bigint(20) NOT NULL DEFAULT '0' COMMENT '增加邀约点',"
                + "`childId` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣实例ID',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='妃子-指定邀约' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//level
        _size+=4;//vip_lvl
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=8;//consortId
        _size+=8;//consortTravelId
        _size+=4;//travelCount
        _size+=8;//addCharmPoint
        _size+=8;//childId
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(level);
        buff.putInt(vip_lvl);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putLong(consortId);
        buff.putLong(consortTravelId);
        buff.putInt(travelCount);
        buff.putLong(addCharmPoint);
        buff.putLong(childId);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        level=buff.getInt();
        vip_lvl=buff.getInt();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        consortId=buff.getLong();
        consortTravelId=buff.getLong();
        travelCount=buff.getInt();
        addCharmPoint=buff.getLong();
        childId=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
