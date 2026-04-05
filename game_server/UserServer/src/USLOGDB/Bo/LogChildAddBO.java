package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogChildAddBO extends BaseLogBo {

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

    public static final int FIELD_childId =5;
    @DataBaseField(type = "bigint(20)", fieldname = "childId", comment = "子嗣实例ID")
    private long childId;

    public static final int FIELD_consortId =6;
    @DataBaseField(type = "bigint(20)", fieldname = "consortId", comment = "关联妃子ID")
    private long consortId;

    public static final int FIELD_initRes =7;
    @DataBaseField(type = "bigint(20)", fieldname = "initRes", comment = "初始形象")
    private long initRes;

    public static final int FIELD_quality =8;
    @DataBaseField(type = "bigint(20)", fieldname = "quality", comment = "子嗣品质")
    private long quality;

    public static final int FIELD_attr =9;
    @DataBaseField(type = "int(11)", fieldname = "attr", comment = "子嗣相性")
    private int attr;

    public static final int FIELD_career =10;
    @DataBaseField(type = "bigint(20)", fieldname = "career", comment = "子嗣职业")
    private long career;

    public static final int FIELD_seatId =11;
    @DataBaseField(type = "bigint(20)", fieldname = "seatId", comment = "训练席位")
    private long seatId;

    public static final int FIELD_isGiftde =12;
    @DataBaseField(type = "tinyint(1)", fieldname = "isGiftde", comment = "是否卷王")
    private boolean isGiftde;

    public static final int FIELD_baseBonus =13;
    @DataBaseField(type = "bigint(20)", fieldname = "baseBonus", comment = "基础收益")
    private long baseBonus;

    public static final int FIELD_initStudyBonus =14;
    @DataBaseField(type = "bigint(20)", fieldname = "initStudyBonus", comment = "教学经验加成（万分比）")
    private long initStudyBonus;

    public LogChildAddBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        childId = 0L;
        consortId = 0L;
        initRes = 0L;
        quality = 0L;
        attr = 0;
        career = 0L;
        seatId = 0L;
        isGiftde = false;
        baseBonus = 0L;
        initStudyBonus = 0L;
    }

    public LogChildAddBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        childId = rs.getLong(7);
        consortId = rs.getLong(8);
        initRes = rs.getLong(9);
        quality = rs.getLong(10);
        attr = rs.getInt(11);
        career = rs.getLong(12);
        seatId = rs.getLong(13);
        isGiftde = rs.getBoolean(14);
        baseBonus = rs.getLong(15);
        initStudyBonus = rs.getLong(16);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogChildAddBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `childId`, `consortId`, `initRes`, `quality`, `attr`, `career`, `seatId`, `isGiftde`, `baseBonus`, `initStudyBonus`";
    }

    @Override
    public String getTableName() {
        return "`log_child_add`";
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
        strBuf.append("'").append(childId).append("', ");
        strBuf.append("'").append(consortId).append("', ");
        strBuf.append("'").append(initRes).append("', ");
        strBuf.append("'").append(quality).append("', ");
        strBuf.append("'").append(attr).append("', ");
        strBuf.append("'").append(career).append("', ");
        strBuf.append("'").append(seatId).append("', ");
        strBuf.append("'").append(isGiftde ? 1 : 0).append("', ");
        strBuf.append("'").append(baseBonus).append("', ");
        strBuf.append("'").append(initStudyBonus).append("', ");
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

    // 关联妃子ID
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

    // 初始形象
    public long getInitRes() { return this.initRes; }
    public void setInitRes(BM _bm, long initRes) {
        if(initRes==this.initRes) 
            return;
        this.initRes = initRes; 
        markField(_bm, FIELD_initRes); 
    }
    public void saveInitRes(BM _bm, long initRes) {
        if(initRes==this.initRes) 
            return;
        this.initRes = initRes;
        saveField(_bm, "initRes", initRes);
    }

    // 子嗣品质
    public long getQuality() { return this.quality; }
    public void setQuality(BM _bm, long quality) {
        if(quality==this.quality) 
            return;
        this.quality = quality; 
        markField(_bm, FIELD_quality); 
    }
    public void saveQuality(BM _bm, long quality) {
        if(quality==this.quality) 
            return;
        this.quality = quality;
        saveField(_bm, "quality", quality);
    }

    // 子嗣相性
    public int getAttr() { return this.attr; }
    public void setAttr(BM _bm, int attr) {
        if(attr==this.attr) 
            return;
        this.attr = attr; 
        markField(_bm, FIELD_attr); 
    }
    public void saveAttr(BM _bm, int attr) {
        if(attr==this.attr) 
            return;
        this.attr = attr;
        saveField(_bm, "attr", attr);
    }

    // 子嗣职业
    public long getCareer() { return this.career; }
    public void setCareer(BM _bm, long career) {
        if(career==this.career) 
            return;
        this.career = career; 
        markField(_bm, FIELD_career); 
    }
    public void saveCareer(BM _bm, long career) {
        if(career==this.career) 
            return;
        this.career = career;
        saveField(_bm, "career", career);
    }

    // 训练席位
    public long getSeatId() { return this.seatId; }
    public void setSeatId(BM _bm, long seatId) {
        if(seatId==this.seatId) 
            return;
        this.seatId = seatId; 
        markField(_bm, FIELD_seatId); 
    }
    public void saveSeatId(BM _bm, long seatId) {
        if(seatId==this.seatId) 
            return;
        this.seatId = seatId;
        saveField(_bm, "seatId", seatId);
    }

    // 是否卷王
    public boolean getIsGiftde() { return this.isGiftde; }
    public void setIsGiftde(BM _bm, boolean isGiftde) {
        if(isGiftde==this.isGiftde) 
            return;
        this.isGiftde = isGiftde; 
        markField(_bm, FIELD_isGiftde); 
    }
    public void saveIsGiftde(BM _bm, boolean isGiftde) {
        if(isGiftde==this.isGiftde) 
            return;
        this.isGiftde = isGiftde;
        saveField(_bm, "isGiftde", isGiftde ? 1 : 0);
    }

    // 基础收益
    public long getBaseBonus() { return this.baseBonus; }
    public void setBaseBonus(BM _bm, long baseBonus) {
        if(baseBonus==this.baseBonus) 
            return;
        this.baseBonus = baseBonus; 
        markField(_bm, FIELD_baseBonus); 
    }
    public void saveBaseBonus(BM _bm, long baseBonus) {
        if(baseBonus==this.baseBonus) 
            return;
        this.baseBonus = baseBonus;
        saveField(_bm, "baseBonus", baseBonus);
    }

    // 教学经验加成（万分比）
    public long getInitStudyBonus() { return this.initStudyBonus; }
    public void setInitStudyBonus(BM _bm, long initStudyBonus) {
        if(initStudyBonus==this.initStudyBonus) 
            return;
        this.initStudyBonus = initStudyBonus; 
        markField(_bm, FIELD_initStudyBonus); 
    }
    public void saveInitStudyBonus(BM _bm, long initStudyBonus) {
        if(initStudyBonus==this.initStudyBonus) 
            return;
        this.initStudyBonus = initStudyBonus;
        saveField(_bm, "initStudyBonus", initStudyBonus);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `childId` = '").append(childId).append("',");
        sBuilder.append(" `consortId` = '").append(consortId).append("',");
        sBuilder.append(" `initRes` = '").append(initRes).append("',");
        sBuilder.append(" `quality` = '").append(quality).append("',");
        sBuilder.append(" `attr` = '").append(attr).append("',");
        sBuilder.append(" `career` = '").append(career).append("',");
        sBuilder.append(" `seatId` = '").append(seatId).append("',");
        sBuilder.append(" `isGiftde` = '").append(isGiftde ? 1 : 0).append("',");
        sBuilder.append(" `baseBonus` = '").append(baseBonus).append("',");
        sBuilder.append(" `initStudyBonus` = '").append(initStudyBonus).append("',");
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
        if(isFieldMarked(FIELD_childId)) sBuilder.append(" `childId` = '").append(childId).append("',");
        if(isFieldMarked(FIELD_consortId)) sBuilder.append(" `consortId` = '").append(consortId).append("',");
        if(isFieldMarked(FIELD_initRes)) sBuilder.append(" `initRes` = '").append(initRes).append("',");
        if(isFieldMarked(FIELD_quality)) sBuilder.append(" `quality` = '").append(quality).append("',");
        if(isFieldMarked(FIELD_attr)) sBuilder.append(" `attr` = '").append(attr).append("',");
        if(isFieldMarked(FIELD_career)) sBuilder.append(" `career` = '").append(career).append("',");
        if(isFieldMarked(FIELD_seatId)) sBuilder.append(" `seatId` = '").append(seatId).append("',");
        if(isFieldMarked(FIELD_isGiftde)) sBuilder.append(" `isGiftde` = '").append(isGiftde ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_baseBonus)) sBuilder.append(" `baseBonus` = '").append(baseBonus).append("',");
        if(isFieldMarked(FIELD_initStudyBonus)) sBuilder.append(" `initStudyBonus` = '").append(initStudyBonus).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_child_add` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`childId` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣实例ID',"
                + "`consortId` bigint(20) NOT NULL DEFAULT '0' COMMENT '关联妃子ID',"
                + "`initRes` bigint(20) NOT NULL DEFAULT '0' COMMENT '初始形象',"
                + "`quality` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣品质',"
                + "`attr` int(11) NOT NULL DEFAULT '0' COMMENT '子嗣相性',"
                + "`career` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣职业',"
                + "`seatId` bigint(20) NOT NULL DEFAULT '0' COMMENT '训练席位',"
                + "`isGiftde` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否卷王',"
                + "`baseBonus` bigint(20) NOT NULL DEFAULT '0' COMMENT '基础收益',"
                + "`initStudyBonus` bigint(20) NOT NULL DEFAULT '0' COMMENT '教学经验加成（万分比）',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家获取未成年子嗣日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//childId
        _size+=8;//consortId
        _size+=8;//initRes
        _size+=8;//quality
        _size+=4;//attr
        _size+=8;//career
        _size+=8;//seatId
        _size+=1;//isGiftde
        _size+=8;//baseBonus
        _size+=8;//initStudyBonus
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
        buff.putLong(childId);
        buff.putLong(consortId);
        buff.putLong(initRes);
        buff.putLong(quality);
        buff.putInt(attr);
        buff.putLong(career);
        buff.putLong(seatId);
        buff.put((byte)(isGiftde?1:0));
        buff.putLong(baseBonus);
        buff.putLong(initStudyBonus);        
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
        childId=buff.getLong();
        consortId=buff.getLong();
        initRes=buff.getLong();
        quality=buff.getLong();
        attr=buff.getInt();
        career=buff.getLong();
        seatId=buff.getLong();
        isGiftde=(buff.get()==1);
        baseBonus=buff.getLong();
        initStudyBonus=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
