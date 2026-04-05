package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogSectionConsortV2BO extends BaseLogBo {

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

    public static final int FIELD_consortId =6;
    @DataBaseField(type = "bigint(20)", fieldname = "consortId", comment = "知己id")
    private long consortId;

    public static final int FIELD_consortIntimacy =7;
    @DataBaseField(type = "bigint(20)", fieldname = "consortIntimacy", comment = "亲密度")
    private long consortIntimacy;

    public static final int FIELD_consortCharm =8;
    @DataBaseField(type = "bigint(20)", fieldname = "consortCharm", comment = "魅力值")
    private long consortCharm;

    public static final int FIELD_consortSkillPointGain =9;
    @DataBaseField(type = "bigint(20)", fieldname = "consortSkillPointGain", comment = "累计获得加护点")
    private long consortSkillPointGain;

    public static final int FIELD_consortFettersLvl =10;
    @DataBaseField(type = "int(11)", fieldname = "consortFettersLvl", comment = "羁绊技能等级")
    private int consortFettersLvl;

    public static final int FIELD_consortBusinessSkillLvl =11;
    @DataBaseField(type = "text", fieldname = "consortBusinessSkillLvl", comment = "经营加成（相性:加成%）")
    private String consortBusinessSkillLvl;

    public static final int FIELD_consortBlessSkillLvl =12;
    @DataBaseField(type = "text", fieldname = "consortBlessSkillLvl", comment = "加护技能(id:等级)")
    private String consortBlessSkillLvl;

    public LogSectionConsortV2BO() {
        id = 0;
        sectionType = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        cid = 0L;
        consortId = 0L;
        consortIntimacy = 0L;
        consortCharm = 0L;
        consortSkillPointGain = 0L;
        consortFettersLvl = 0;
        consortBusinessSkillLvl = "";
        consortBlessSkillLvl = "";
    }

    public LogSectionConsortV2BO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        sectionType = rs.getInt(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        cid = rs.getLong(7);
        consortId = rs.getLong(8);
        consortIntimacy = rs.getLong(9);
        consortCharm = rs.getLong(10);
        consortSkillPointGain = rs.getLong(11);
        consortFettersLvl = rs.getInt(12);
        consortBusinessSkillLvl = rs.getString(13);
        consortBlessSkillLvl = rs.getString(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogSectionConsortV2BO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `sectionType`, `event_id`, `guid`, `date_time`, `timestamp`, `cid`, `consortId`, `consortIntimacy`, `consortCharm`, `consortSkillPointGain`, `consortFettersLvl`, `consortBusinessSkillLvl`, `consortBlessSkillLvl`";
    }

    @Override
    public String getTableName() {
        return "`log_section_consort_v2`";
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
        strBuf.append("'").append(consortId).append("', ");
        strBuf.append("'").append(consortIntimacy).append("', ");
        strBuf.append("'").append(consortCharm).append("', ");
        strBuf.append("'").append(consortSkillPointGain).append("', ");
        strBuf.append("'").append(consortFettersLvl).append("', ");
        strBuf.append("'").append(consortBusinessSkillLvl == null ? null : consortBusinessSkillLvl.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(consortBlessSkillLvl == null ? null : consortBlessSkillLvl.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 知己id
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

    // 亲密度
    public long getConsortIntimacy() { return this.consortIntimacy; }
    public void setConsortIntimacy(BM _bm, long consortIntimacy) {
        if(consortIntimacy==this.consortIntimacy) 
            return;
        this.consortIntimacy = consortIntimacy; 
        markField(_bm, FIELD_consortIntimacy); 
    }
    public void saveConsortIntimacy(BM _bm, long consortIntimacy) {
        if(consortIntimacy==this.consortIntimacy) 
            return;
        this.consortIntimacy = consortIntimacy;
        saveField(_bm, "consortIntimacy", consortIntimacy);
    }

    // 魅力值
    public long getConsortCharm() { return this.consortCharm; }
    public void setConsortCharm(BM _bm, long consortCharm) {
        if(consortCharm==this.consortCharm) 
            return;
        this.consortCharm = consortCharm; 
        markField(_bm, FIELD_consortCharm); 
    }
    public void saveConsortCharm(BM _bm, long consortCharm) {
        if(consortCharm==this.consortCharm) 
            return;
        this.consortCharm = consortCharm;
        saveField(_bm, "consortCharm", consortCharm);
    }

    // 累计获得加护点
    public long getConsortSkillPointGain() { return this.consortSkillPointGain; }
    public void setConsortSkillPointGain(BM _bm, long consortSkillPointGain) {
        if(consortSkillPointGain==this.consortSkillPointGain) 
            return;
        this.consortSkillPointGain = consortSkillPointGain; 
        markField(_bm, FIELD_consortSkillPointGain); 
    }
    public void saveConsortSkillPointGain(BM _bm, long consortSkillPointGain) {
        if(consortSkillPointGain==this.consortSkillPointGain) 
            return;
        this.consortSkillPointGain = consortSkillPointGain;
        saveField(_bm, "consortSkillPointGain", consortSkillPointGain);
    }

    // 羁绊技能等级
    public int getConsortFettersLvl() { return this.consortFettersLvl; }
    public void setConsortFettersLvl(BM _bm, int consortFettersLvl) {
        if(consortFettersLvl==this.consortFettersLvl) 
            return;
        this.consortFettersLvl = consortFettersLvl; 
        markField(_bm, FIELD_consortFettersLvl); 
    }
    public void saveConsortFettersLvl(BM _bm, int consortFettersLvl) {
        if(consortFettersLvl==this.consortFettersLvl) 
            return;
        this.consortFettersLvl = consortFettersLvl;
        saveField(_bm, "consortFettersLvl", consortFettersLvl);
    }

    // 经营加成（相性:加成%）
    public String getConsortBusinessSkillLvl() { return this.consortBusinessSkillLvl; }
    public void setConsortBusinessSkillLvl(BM _bm, String consortBusinessSkillLvl) {
        if(consortBusinessSkillLvl.equals(this.consortBusinessSkillLvl)) 
            return;
        this.consortBusinessSkillLvl = consortBusinessSkillLvl; 
        markField(_bm, FIELD_consortBusinessSkillLvl); 
    }
    public void saveConsortBusinessSkillLvl(BM _bm, String consortBusinessSkillLvl) {
        if(consortBusinessSkillLvl.equals(this.consortBusinessSkillLvl)) 
            return;
        this.consortBusinessSkillLvl = consortBusinessSkillLvl;
        saveField(_bm, "consortBusinessSkillLvl", consortBusinessSkillLvl);
    }

    // 加护技能(id:等级)
    public String getConsortBlessSkillLvl() { return this.consortBlessSkillLvl; }
    public void setConsortBlessSkillLvl(BM _bm, String consortBlessSkillLvl) {
        if(consortBlessSkillLvl.equals(this.consortBlessSkillLvl)) 
            return;
        this.consortBlessSkillLvl = consortBlessSkillLvl; 
        markField(_bm, FIELD_consortBlessSkillLvl); 
    }
    public void saveConsortBlessSkillLvl(BM _bm, String consortBlessSkillLvl) {
        if(consortBlessSkillLvl.equals(this.consortBlessSkillLvl)) 
            return;
        this.consortBlessSkillLvl = consortBlessSkillLvl;
        saveField(_bm, "consortBlessSkillLvl", consortBlessSkillLvl);
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
        sBuilder.append(" `consortId` = '").append(consortId).append("',");
        sBuilder.append(" `consortIntimacy` = '").append(consortIntimacy).append("',");
        sBuilder.append(" `consortCharm` = '").append(consortCharm).append("',");
        sBuilder.append(" `consortSkillPointGain` = '").append(consortSkillPointGain).append("',");
        sBuilder.append(" `consortFettersLvl` = '").append(consortFettersLvl).append("',");
        sBuilder.append(" `consortBusinessSkillLvl` = '").append(consortBusinessSkillLvl == null ? null : consortBusinessSkillLvl.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `consortBlessSkillLvl` = '").append(consortBlessSkillLvl == null ? null : consortBlessSkillLvl.replace("'","''").replace("\\","\\\\")).append("',");
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
        if(isFieldMarked(FIELD_consortId)) sBuilder.append(" `consortId` = '").append(consortId).append("',");
        if(isFieldMarked(FIELD_consortIntimacy)) sBuilder.append(" `consortIntimacy` = '").append(consortIntimacy).append("',");
        if(isFieldMarked(FIELD_consortCharm)) sBuilder.append(" `consortCharm` = '").append(consortCharm).append("',");
        if(isFieldMarked(FIELD_consortSkillPointGain)) sBuilder.append(" `consortSkillPointGain` = '").append(consortSkillPointGain).append("',");
        if(isFieldMarked(FIELD_consortFettersLvl)) sBuilder.append(" `consortFettersLvl` = '").append(consortFettersLvl).append("',");
        if(isFieldMarked(FIELD_consortBusinessSkillLvl)) sBuilder.append(" `consortBusinessSkillLvl` = '").append(consortBusinessSkillLvl == null ? null : consortBusinessSkillLvl.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_consortBlessSkillLvl)) sBuilder.append(" `consortBlessSkillLvl` = '").append(consortBlessSkillLvl == null ? null : consortBlessSkillLvl.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_section_consort_v2` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`sectionType` int(11) NOT NULL DEFAULT '0' COMMENT '截面日志类型，枚举ELogSectionType',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`consortId` bigint(20) NOT NULL DEFAULT '0' COMMENT '知己id',"
                + "`consortIntimacy` bigint(20) NOT NULL DEFAULT '0' COMMENT '亲密度',"
                + "`consortCharm` bigint(20) NOT NULL DEFAULT '0' COMMENT '魅力值',"
                + "`consortSkillPointGain` bigint(20) NOT NULL DEFAULT '0' COMMENT '累计获得加护点',"
                + "`consortFettersLvl` int(11) NOT NULL DEFAULT '0' COMMENT '羁绊技能等级',"
                + "`consortBusinessSkillLvl` text NULL COMMENT '经营加成（相性:加成%）',"
                + "`consortBlessSkillLvl` text NULL COMMENT '加护技能(id:等级)',"
                + "KEY `sectionType` (`sectionType`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='妃子截面数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//consortId
        _size+=8;//consortIntimacy
        _size+=8;//consortCharm
        _size+=8;//consortSkillPointGain
        _size+=4;//consortFettersLvl
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(consortBusinessSkillLvl);//consortBusinessSkillLvl
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(consortBlessSkillLvl);//consortBlessSkillLvl
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
        buff.putLong(consortId);
        buff.putLong(consortIntimacy);
        buff.putLong(consortCharm);
        buff.putLong(consortSkillPointGain);
        buff.putInt(consortFettersLvl);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, consortBusinessSkillLvl);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, consortBlessSkillLvl);        
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
        consortId=buff.getLong();
        consortIntimacy=buff.getLong();
        consortCharm=buff.getLong();
        consortSkillPointGain=buff.getLong();
        consortFettersLvl=buff.getInt();
        consortBusinessSkillLvl=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        consortBlessSkillLvl=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
