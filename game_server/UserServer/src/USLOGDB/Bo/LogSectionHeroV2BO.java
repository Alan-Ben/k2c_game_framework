package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogSectionHeroV2BO extends BaseLogBo {

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

    public static final int FIELD_heroId =6;
    @DataBaseField(type = "bigint(20)", fieldname = "heroId", comment = "骑士ID")
    private long heroId;

    public static final int FIELD_heroLvl =7;
    @DataBaseField(type = "int(11)", fieldname = "heroLvl", comment = "等级")
    private int heroLvl;

    public static final int FIELD_heroTalent =8;
    @DataBaseField(type = "bigint(20)", fieldname = "heroTalent", comment = "资质")
    private long heroTalent;

    public static final int FIELD_heroPower =9;
    @DataBaseField(type = "bigint(20)", fieldname = "heroPower", comment = "战力")
    private long heroPower;

    public static final int FIELD_heroBusinessSkillLvl =10;
    @DataBaseField(type = "text", fieldname = "heroBusinessSkillLvl", comment = "经营技能等级")
    private String heroBusinessSkillLvl;

    public static final int FIELD_equipID =11;
    @DataBaseField(type = "bigint(20)", fieldname = "equipID", comment = "佩戴的藏品id")
    private long equipID;

    public static final int FIELD_equipLvl =12;
    @DataBaseField(type = "int(11)", fieldname = "equipLvl", comment = "佩戴的藏品等级")
    private int equipLvl;

    public LogSectionHeroV2BO() {
        id = 0;
        sectionType = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        cid = 0L;
        heroId = 0L;
        heroLvl = 0;
        heroTalent = 0L;
        heroPower = 0L;
        heroBusinessSkillLvl = "";
        equipID = 0L;
        equipLvl = 0;
    }

    public LogSectionHeroV2BO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        sectionType = rs.getInt(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        cid = rs.getLong(7);
        heroId = rs.getLong(8);
        heroLvl = rs.getInt(9);
        heroTalent = rs.getLong(10);
        heroPower = rs.getLong(11);
        heroBusinessSkillLvl = rs.getString(12);
        equipID = rs.getLong(13);
        equipLvl = rs.getInt(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogSectionHeroV2BO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `sectionType`, `event_id`, `guid`, `date_time`, `timestamp`, `cid`, `heroId`, `heroLvl`, `heroTalent`, `heroPower`, `heroBusinessSkillLvl`, `equipID`, `equipLvl`";
    }

    @Override
    public String getTableName() {
        return "`log_section_hero_v2`";
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
        strBuf.append("'").append(heroId).append("', ");
        strBuf.append("'").append(heroLvl).append("', ");
        strBuf.append("'").append(heroTalent).append("', ");
        strBuf.append("'").append(heroPower).append("', ");
        strBuf.append("'").append(heroBusinessSkillLvl == null ? null : heroBusinessSkillLvl.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(equipID).append("', ");
        strBuf.append("'").append(equipLvl).append("', ");
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

    // 骑士ID
    public long getHeroId() { return this.heroId; }
    public void setHeroId(BM _bm, long heroId) {
        if(heroId==this.heroId) 
            return;
        this.heroId = heroId; 
        markField(_bm, FIELD_heroId); 
    }
    public void saveHeroId(BM _bm, long heroId) {
        if(heroId==this.heroId) 
            return;
        this.heroId = heroId;
        saveField(_bm, "heroId", heroId);
    }

    // 等级
    public int getHeroLvl() { return this.heroLvl; }
    public void setHeroLvl(BM _bm, int heroLvl) {
        if(heroLvl==this.heroLvl) 
            return;
        this.heroLvl = heroLvl; 
        markField(_bm, FIELD_heroLvl); 
    }
    public void saveHeroLvl(BM _bm, int heroLvl) {
        if(heroLvl==this.heroLvl) 
            return;
        this.heroLvl = heroLvl;
        saveField(_bm, "heroLvl", heroLvl);
    }

    // 资质
    public long getHeroTalent() { return this.heroTalent; }
    public void setHeroTalent(BM _bm, long heroTalent) {
        if(heroTalent==this.heroTalent) 
            return;
        this.heroTalent = heroTalent; 
        markField(_bm, FIELD_heroTalent); 
    }
    public void saveHeroTalent(BM _bm, long heroTalent) {
        if(heroTalent==this.heroTalent) 
            return;
        this.heroTalent = heroTalent;
        saveField(_bm, "heroTalent", heroTalent);
    }

    // 战力
    public long getHeroPower() { return this.heroPower; }
    public void setHeroPower(BM _bm, long heroPower) {
        if(heroPower==this.heroPower) 
            return;
        this.heroPower = heroPower; 
        markField(_bm, FIELD_heroPower); 
    }
    public void saveHeroPower(BM _bm, long heroPower) {
        if(heroPower==this.heroPower) 
            return;
        this.heroPower = heroPower;
        saveField(_bm, "heroPower", heroPower);
    }

    // 经营技能等级
    public String getHeroBusinessSkillLvl() { return this.heroBusinessSkillLvl; }
    public void setHeroBusinessSkillLvl(BM _bm, String heroBusinessSkillLvl) {
        if(heroBusinessSkillLvl.equals(this.heroBusinessSkillLvl)) 
            return;
        this.heroBusinessSkillLvl = heroBusinessSkillLvl; 
        markField(_bm, FIELD_heroBusinessSkillLvl); 
    }
    public void saveHeroBusinessSkillLvl(BM _bm, String heroBusinessSkillLvl) {
        if(heroBusinessSkillLvl.equals(this.heroBusinessSkillLvl)) 
            return;
        this.heroBusinessSkillLvl = heroBusinessSkillLvl;
        saveField(_bm, "heroBusinessSkillLvl", heroBusinessSkillLvl);
    }

    // 佩戴的藏品id
    public long getEquipID() { return this.equipID; }
    public void setEquipID(BM _bm, long equipID) {
        if(equipID==this.equipID) 
            return;
        this.equipID = equipID; 
        markField(_bm, FIELD_equipID); 
    }
    public void saveEquipID(BM _bm, long equipID) {
        if(equipID==this.equipID) 
            return;
        this.equipID = equipID;
        saveField(_bm, "equipID", equipID);
    }

    // 佩戴的藏品等级
    public int getEquipLvl() { return this.equipLvl; }
    public void setEquipLvl(BM _bm, int equipLvl) {
        if(equipLvl==this.equipLvl) 
            return;
        this.equipLvl = equipLvl; 
        markField(_bm, FIELD_equipLvl); 
    }
    public void saveEquipLvl(BM _bm, int equipLvl) {
        if(equipLvl==this.equipLvl) 
            return;
        this.equipLvl = equipLvl;
        saveField(_bm, "equipLvl", equipLvl);
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
        sBuilder.append(" `heroId` = '").append(heroId).append("',");
        sBuilder.append(" `heroLvl` = '").append(heroLvl).append("',");
        sBuilder.append(" `heroTalent` = '").append(heroTalent).append("',");
        sBuilder.append(" `heroPower` = '").append(heroPower).append("',");
        sBuilder.append(" `heroBusinessSkillLvl` = '").append(heroBusinessSkillLvl == null ? null : heroBusinessSkillLvl.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `equipID` = '").append(equipID).append("',");
        sBuilder.append(" `equipLvl` = '").append(equipLvl).append("',");
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
        if(isFieldMarked(FIELD_heroId)) sBuilder.append(" `heroId` = '").append(heroId).append("',");
        if(isFieldMarked(FIELD_heroLvl)) sBuilder.append(" `heroLvl` = '").append(heroLvl).append("',");
        if(isFieldMarked(FIELD_heroTalent)) sBuilder.append(" `heroTalent` = '").append(heroTalent).append("',");
        if(isFieldMarked(FIELD_heroPower)) sBuilder.append(" `heroPower` = '").append(heroPower).append("',");
        if(isFieldMarked(FIELD_heroBusinessSkillLvl)) sBuilder.append(" `heroBusinessSkillLvl` = '").append(heroBusinessSkillLvl == null ? null : heroBusinessSkillLvl.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_equipID)) sBuilder.append(" `equipID` = '").append(equipID).append("',");
        if(isFieldMarked(FIELD_equipLvl)) sBuilder.append(" `equipLvl` = '").append(equipLvl).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_section_hero_v2` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`sectionType` int(11) NOT NULL DEFAULT '0' COMMENT '截面日志类型，枚举ELogSectionType',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`heroId` bigint(20) NOT NULL DEFAULT '0' COMMENT '骑士ID',"
                + "`heroLvl` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`heroTalent` bigint(20) NOT NULL DEFAULT '0' COMMENT '资质',"
                + "`heroPower` bigint(20) NOT NULL DEFAULT '0' COMMENT '战力',"
                + "`heroBusinessSkillLvl` text NULL COMMENT '经营技能等级',"
                + "`equipID` bigint(20) NOT NULL DEFAULT '0' COMMENT '佩戴的藏品id',"
                + "`equipLvl` int(11) NOT NULL DEFAULT '0' COMMENT '佩戴的藏品等级',"
                + "KEY `sectionType` (`sectionType`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='伙伴截面数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//heroId
        _size+=4;//heroLvl
        _size+=8;//heroTalent
        _size+=8;//heroPower
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(heroBusinessSkillLvl);//heroBusinessSkillLvl
        _size+=8;//equipID
        _size+=4;//equipLvl
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
        buff.putLong(heroId);
        buff.putInt(heroLvl);
        buff.putLong(heroTalent);
        buff.putLong(heroPower);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, heroBusinessSkillLvl);
        buff.putLong(equipID);
        buff.putInt(equipLvl);        
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
        heroId=buff.getLong();
        heroLvl=buff.getInt();
        heroTalent=buff.getLong();
        heroPower=buff.getLong();
        heroBusinessSkillLvl=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        equipID=buff.getLong();
        equipLvl=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
