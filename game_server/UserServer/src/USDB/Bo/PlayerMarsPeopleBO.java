package USDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;
import NPCommon.DB.Annotation.RefBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

@RefBo(isIdAuto= true)
public class PlayerMarsPeopleBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_startMs =1;
    @DataBaseField(type = "bigint(20)", fieldname = "startMs", comment = "开启时间（毫秒）")
    private long startMs;

    public static final int FIELD_endMs =2;
    @DataBaseField(type = "bigint(20)", fieldname = "endMs", comment = "截至时间（毫秒）")
    private long endMs;

    public static final int FIELD_curUsedCount =3;
    @DataBaseField(type = "int(11)", fieldname = "curUsedCount", comment = "本次移民次数，用于计算本次的奖励数据")
    private int curUsedCount;

    public static final int FIELD_dayTag =4;
    @DataBaseField(type = "int(11)", fieldname = "dayTag", comment = "当日tag")
    private int dayTag;

    public static final int FIELD_usedCount =5;
    @DataBaseField(type = "int(11)", fieldname = "usedCount", comment = "当日移民次数")
    private int usedCount;

    public static final int FIELD_immigrantNum =6;
    @DataBaseField(type = "bigint(20)", fieldname = "immigrantNum", comment = "移民居民数量")
    private long immigrantNum;

    public static final int FIELD_idleNum =7;
    @DataBaseField(type = "bigint(20)", fieldname = "idleNum", comment = "休闲居民数量")
    private long idleNum;

    public static final int FIELD_sickNum =8;
    @DataBaseField(type = "bigint(20)", fieldname = "sickNum", comment = "生病居民数量")
    private long sickNum;

    public static final int FIELD_satisfaction =9;
    @DataBaseField(type = "int(11)", fieldname = "satisfaction", comment = "满意度")
    private int satisfaction;

    public static final int FIELD_lastCalMs =10;
    @DataBaseField(type = "bigint(20)", fieldname = "lastCalMs", comment = "上次计算满意度时间（毫秒）")
    private long lastCalMs;

    public static final int FIELD_dailyEventInfo =11;
    @DataBaseField(type = "blob", fieldname = "dailyEventInfo", comment = "每日事件数据")
    private byte[] dailyEventInfo;

    public static final int FIELD_lastLetterBuildMs =12;
    @DataBaseField(type = "bigint(20)", fieldname = "lastLetterBuildMs", comment = "上次信件创建时间（毫秒）")
    private long lastLetterBuildMs;

    public static final int FIELD_lastHelpBuildMs =13;
    @DataBaseField(type = "bigint(20)", fieldname = "lastHelpBuildMs", comment = "上次求助创建时间（毫秒）")
    private long lastHelpBuildMs;

    public static final int FIELD_lastEventBuildMs =14;
    @DataBaseField(type = "bigint(20)", fieldname = "lastEventBuildMs", comment = "上次事件创建时间（毫秒）")
    private long lastEventBuildMs;

    public static final int FIELD_lastCureSickPeopleMs =15;
    @DataBaseField(type = "bigint(20)", fieldname = "lastCureSickPeopleMs", comment = "上次治愈居民时间（毫秒）")
    private long lastCureSickPeopleMs;

    public PlayerMarsPeopleBO() {
        id = 0;
        cid = 0L;
        startMs = 0L;
        endMs = 0L;
        curUsedCount = 0;
        dayTag = 0;
        usedCount = 0;
        immigrantNum = 0L;
        idleNum = 0L;
        sickNum = 0L;
        satisfaction = 0;
        lastCalMs = 0L;
        dailyEventInfo = null;
        lastLetterBuildMs = 0L;
        lastHelpBuildMs = 0L;
        lastEventBuildMs = 0L;
        lastCureSickPeopleMs = 0L;
    }

    public PlayerMarsPeopleBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        startMs = rs.getLong(3);
        endMs = rs.getLong(4);
        curUsedCount = rs.getInt(5);
        dayTag = rs.getInt(6);
        usedCount = rs.getInt(7);
        immigrantNum = rs.getLong(8);
        idleNum = rs.getLong(9);
        sickNum = rs.getLong(10);
        satisfaction = rs.getInt(11);
        lastCalMs = rs.getLong(12);
        dailyEventInfo = rs.getBytes(13);
        lastLetterBuildMs = rs.getLong(14);
        lastHelpBuildMs = rs.getLong(15);
        lastEventBuildMs = rs.getLong(16);
        lastCureSickPeopleMs = rs.getLong(17);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsPeopleBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `startMs`, `endMs`, `curUsedCount`, `dayTag`, `usedCount`, `immigrantNum`, `idleNum`, `sickNum`, `satisfaction`, `lastCalMs`, `dailyEventInfo`, `lastLetterBuildMs`, `lastHelpBuildMs`, `lastEventBuildMs`, `lastCureSickPeopleMs`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_people`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(startMs).append("', ");
        strBuf.append("'").append(endMs).append("', ");
        strBuf.append("'").append(curUsedCount).append("', ");
        strBuf.append("'").append(dayTag).append("', ");
        strBuf.append("'").append(usedCount).append("', ");
        strBuf.append("'").append(immigrantNum).append("', ");
        strBuf.append("'").append(idleNum).append("', ");
        strBuf.append("'").append(sickNum).append("', ");
        strBuf.append("'").append(satisfaction).append("', ");
        strBuf.append("'").append(lastCalMs).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(lastLetterBuildMs).append("', ");
        strBuf.append("'").append(lastHelpBuildMs).append("', ");
        strBuf.append("'").append(lastEventBuildMs).append("', ");
        strBuf.append("'").append(lastCureSickPeopleMs).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(dailyEventInfo);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_dailyEventInfo)) ret.add(dailyEventInfo);         return ret;
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

    // 开启时间（毫秒）
    public long getStartMs() { return this.startMs; }
    public void setStartMs(BM _bm, long startMs) {
        if(startMs==this.startMs) 
            return;
        this.startMs = startMs; 
        markField(_bm, FIELD_startMs); 
    }
    public void saveStartMs(BM _bm, long startMs) {
        if(startMs==this.startMs) 
            return;
        this.startMs = startMs;
        saveField(_bm, "startMs", startMs);
    }

    // 截至时间（毫秒）
    public long getEndMs() { return this.endMs; }
    public void setEndMs(BM _bm, long endMs) {
        if(endMs==this.endMs) 
            return;
        this.endMs = endMs; 
        markField(_bm, FIELD_endMs); 
    }
    public void saveEndMs(BM _bm, long endMs) {
        if(endMs==this.endMs) 
            return;
        this.endMs = endMs;
        saveField(_bm, "endMs", endMs);
    }

    // 本次移民次数，用于计算本次的奖励数据
    public int getCurUsedCount() { return this.curUsedCount; }
    public void setCurUsedCount(BM _bm, int curUsedCount) {
        if(curUsedCount==this.curUsedCount) 
            return;
        this.curUsedCount = curUsedCount; 
        markField(_bm, FIELD_curUsedCount); 
    }
    public void saveCurUsedCount(BM _bm, int curUsedCount) {
        if(curUsedCount==this.curUsedCount) 
            return;
        this.curUsedCount = curUsedCount;
        saveField(_bm, "curUsedCount", curUsedCount);
    }

    // 当日tag
    public int getDayTag() { return this.dayTag; }
    public void setDayTag(BM _bm, int dayTag) {
        if(dayTag==this.dayTag) 
            return;
        this.dayTag = dayTag; 
        markField(_bm, FIELD_dayTag); 
    }
    public void saveDayTag(BM _bm, int dayTag) {
        if(dayTag==this.dayTag) 
            return;
        this.dayTag = dayTag;
        saveField(_bm, "dayTag", dayTag);
    }

    // 当日移民次数
    public int getUsedCount() { return this.usedCount; }
    public void setUsedCount(BM _bm, int usedCount) {
        if(usedCount==this.usedCount) 
            return;
        this.usedCount = usedCount; 
        markField(_bm, FIELD_usedCount); 
    }
    public void saveUsedCount(BM _bm, int usedCount) {
        if(usedCount==this.usedCount) 
            return;
        this.usedCount = usedCount;
        saveField(_bm, "usedCount", usedCount);
    }

    // 移民居民数量
    public long getImmigrantNum() { return this.immigrantNum; }
    public void setImmigrantNum(BM _bm, long immigrantNum) {
        if(immigrantNum==this.immigrantNum) 
            return;
        this.immigrantNum = immigrantNum; 
        markField(_bm, FIELD_immigrantNum); 
    }
    public void saveImmigrantNum(BM _bm, long immigrantNum) {
        if(immigrantNum==this.immigrantNum) 
            return;
        this.immigrantNum = immigrantNum;
        saveField(_bm, "immigrantNum", immigrantNum);
    }

    // 休闲居民数量
    public long getIdleNum() { return this.idleNum; }
    public void setIdleNum(BM _bm, long idleNum) {
        if(idleNum==this.idleNum) 
            return;
        this.idleNum = idleNum; 
        markField(_bm, FIELD_idleNum); 
    }
    public void saveIdleNum(BM _bm, long idleNum) {
        if(idleNum==this.idleNum) 
            return;
        this.idleNum = idleNum;
        saveField(_bm, "idleNum", idleNum);
    }

    // 生病居民数量
    public long getSickNum() { return this.sickNum; }
    public void setSickNum(BM _bm, long sickNum) {
        if(sickNum==this.sickNum) 
            return;
        this.sickNum = sickNum; 
        markField(_bm, FIELD_sickNum); 
    }
    public void saveSickNum(BM _bm, long sickNum) {
        if(sickNum==this.sickNum) 
            return;
        this.sickNum = sickNum;
        saveField(_bm, "sickNum", sickNum);
    }

    // 满意度
    public int getSatisfaction() { return this.satisfaction; }
    public void setSatisfaction(BM _bm, int satisfaction) {
        if(satisfaction==this.satisfaction) 
            return;
        this.satisfaction = satisfaction; 
        markField(_bm, FIELD_satisfaction); 
    }
    public void saveSatisfaction(BM _bm, int satisfaction) {
        if(satisfaction==this.satisfaction) 
            return;
        this.satisfaction = satisfaction;
        saveField(_bm, "satisfaction", satisfaction);
    }

    // 上次计算满意度时间（毫秒）
    public long getLastCalMs() { return this.lastCalMs; }
    public void setLastCalMs(BM _bm, long lastCalMs) {
        if(lastCalMs==this.lastCalMs) 
            return;
        this.lastCalMs = lastCalMs; 
        markField(_bm, FIELD_lastCalMs); 
    }
    public void saveLastCalMs(BM _bm, long lastCalMs) {
        if(lastCalMs==this.lastCalMs) 
            return;
        this.lastCalMs = lastCalMs;
        saveField(_bm, "lastCalMs", lastCalMs);
    }

    // 每日事件数据
    public byte[] getDailyEventInfo() { return this.dailyEventInfo; }
    public void setDailyEventInfo(BM _bm, byte[] dailyEventInfo) {
        if(dailyEventInfo==this.dailyEventInfo) 
            return;
        this.dailyEventInfo = dailyEventInfo; 
        markField(_bm, FIELD_dailyEventInfo); 
    }
    public void saveDailyEventInfo(BM _bm, byte[] dailyEventInfo) {
        if(dailyEventInfo==this.dailyEventInfo) 
            return;
        this.dailyEventInfo = dailyEventInfo;
        saveFieldBytes(_bm, "dailyEventInfo", dailyEventInfo);
    }

    // 上次信件创建时间（毫秒）
    public long getLastLetterBuildMs() { return this.lastLetterBuildMs; }
    public void setLastLetterBuildMs(BM _bm, long lastLetterBuildMs) {
        if(lastLetterBuildMs==this.lastLetterBuildMs) 
            return;
        this.lastLetterBuildMs = lastLetterBuildMs; 
        markField(_bm, FIELD_lastLetterBuildMs); 
    }
    public void saveLastLetterBuildMs(BM _bm, long lastLetterBuildMs) {
        if(lastLetterBuildMs==this.lastLetterBuildMs) 
            return;
        this.lastLetterBuildMs = lastLetterBuildMs;
        saveField(_bm, "lastLetterBuildMs", lastLetterBuildMs);
    }

    // 上次求助创建时间（毫秒）
    public long getLastHelpBuildMs() { return this.lastHelpBuildMs; }
    public void setLastHelpBuildMs(BM _bm, long lastHelpBuildMs) {
        if(lastHelpBuildMs==this.lastHelpBuildMs) 
            return;
        this.lastHelpBuildMs = lastHelpBuildMs; 
        markField(_bm, FIELD_lastHelpBuildMs); 
    }
    public void saveLastHelpBuildMs(BM _bm, long lastHelpBuildMs) {
        if(lastHelpBuildMs==this.lastHelpBuildMs) 
            return;
        this.lastHelpBuildMs = lastHelpBuildMs;
        saveField(_bm, "lastHelpBuildMs", lastHelpBuildMs);
    }

    // 上次事件创建时间（毫秒）
    public long getLastEventBuildMs() { return this.lastEventBuildMs; }
    public void setLastEventBuildMs(BM _bm, long lastEventBuildMs) {
        if(lastEventBuildMs==this.lastEventBuildMs) 
            return;
        this.lastEventBuildMs = lastEventBuildMs; 
        markField(_bm, FIELD_lastEventBuildMs); 
    }
    public void saveLastEventBuildMs(BM _bm, long lastEventBuildMs) {
        if(lastEventBuildMs==this.lastEventBuildMs) 
            return;
        this.lastEventBuildMs = lastEventBuildMs;
        saveField(_bm, "lastEventBuildMs", lastEventBuildMs);
    }

    // 上次治愈居民时间（毫秒）
    public long getLastCureSickPeopleMs() { return this.lastCureSickPeopleMs; }
    public void setLastCureSickPeopleMs(BM _bm, long lastCureSickPeopleMs) {
        if(lastCureSickPeopleMs==this.lastCureSickPeopleMs) 
            return;
        this.lastCureSickPeopleMs = lastCureSickPeopleMs; 
        markField(_bm, FIELD_lastCureSickPeopleMs); 
    }
    public void saveLastCureSickPeopleMs(BM _bm, long lastCureSickPeopleMs) {
        if(lastCureSickPeopleMs==this.lastCureSickPeopleMs) 
            return;
        this.lastCureSickPeopleMs = lastCureSickPeopleMs;
        saveField(_bm, "lastCureSickPeopleMs", lastCureSickPeopleMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `startMs` = '").append(startMs).append("',");
        sBuilder.append(" `endMs` = '").append(endMs).append("',");
        sBuilder.append(" `curUsedCount` = '").append(curUsedCount).append("',");
        sBuilder.append(" `dayTag` = '").append(dayTag).append("',");
        sBuilder.append(" `usedCount` = '").append(usedCount).append("',");
        sBuilder.append(" `immigrantNum` = '").append(immigrantNum).append("',");
        sBuilder.append(" `idleNum` = '").append(idleNum).append("',");
        sBuilder.append(" `sickNum` = '").append(sickNum).append("',");
        sBuilder.append(" `satisfaction` = '").append(satisfaction).append("',");
        sBuilder.append(" `lastCalMs` = '").append(lastCalMs).append("',");
        sBuilder.append(" `dailyEventInfo` = ?,");
        sBuilder.append(" `lastLetterBuildMs` = '").append(lastLetterBuildMs).append("',");
        sBuilder.append(" `lastHelpBuildMs` = '").append(lastHelpBuildMs).append("',");
        sBuilder.append(" `lastEventBuildMs` = '").append(lastEventBuildMs).append("',");
        sBuilder.append(" `lastCureSickPeopleMs` = '").append(lastCureSickPeopleMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_startMs)) sBuilder.append(" `startMs` = '").append(startMs).append("',");
        if(isFieldMarked(FIELD_endMs)) sBuilder.append(" `endMs` = '").append(endMs).append("',");
        if(isFieldMarked(FIELD_curUsedCount)) sBuilder.append(" `curUsedCount` = '").append(curUsedCount).append("',");
        if(isFieldMarked(FIELD_dayTag)) sBuilder.append(" `dayTag` = '").append(dayTag).append("',");
        if(isFieldMarked(FIELD_usedCount)) sBuilder.append(" `usedCount` = '").append(usedCount).append("',");
        if(isFieldMarked(FIELD_immigrantNum)) sBuilder.append(" `immigrantNum` = '").append(immigrantNum).append("',");
        if(isFieldMarked(FIELD_idleNum)) sBuilder.append(" `idleNum` = '").append(idleNum).append("',");
        if(isFieldMarked(FIELD_sickNum)) sBuilder.append(" `sickNum` = '").append(sickNum).append("',");
        if(isFieldMarked(FIELD_satisfaction)) sBuilder.append(" `satisfaction` = '").append(satisfaction).append("',");
        if(isFieldMarked(FIELD_lastCalMs)) sBuilder.append(" `lastCalMs` = '").append(lastCalMs).append("',");
        if(isFieldMarked(FIELD_dailyEventInfo)) sBuilder.append(" `dailyEventInfo` = ?,");
        if(isFieldMarked(FIELD_lastLetterBuildMs)) sBuilder.append(" `lastLetterBuildMs` = '").append(lastLetterBuildMs).append("',");
        if(isFieldMarked(FIELD_lastHelpBuildMs)) sBuilder.append(" `lastHelpBuildMs` = '").append(lastHelpBuildMs).append("',");
        if(isFieldMarked(FIELD_lastEventBuildMs)) sBuilder.append(" `lastEventBuildMs` = '").append(lastEventBuildMs).append("',");
        if(isFieldMarked(FIELD_lastCureSickPeopleMs)) sBuilder.append(" `lastCureSickPeopleMs` = '").append(lastCureSickPeopleMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_people` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`startMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '开启时间（毫秒）',"
                + "`endMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '截至时间（毫秒）',"
                + "`curUsedCount` int(11) NOT NULL DEFAULT '0' COMMENT '本次移民次数，用于计算本次的奖励数据',"
                + "`dayTag` int(11) NOT NULL DEFAULT '0' COMMENT '当日tag',"
                + "`usedCount` int(11) NOT NULL DEFAULT '0' COMMENT '当日移民次数',"
                + "`immigrantNum` bigint(20) NOT NULL DEFAULT '0' COMMENT '移民居民数量',"
                + "`idleNum` bigint(20) NOT NULL DEFAULT '0' COMMENT '休闲居民数量',"
                + "`sickNum` bigint(20) NOT NULL DEFAULT '0' COMMENT '生病居民数量',"
                + "`satisfaction` int(11) NOT NULL DEFAULT '0' COMMENT '满意度',"
                + "`lastCalMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次计算满意度时间（毫秒）',"
                + "`dailyEventInfo` blob NULL COMMENT '每日事件数据',"
                + "`lastLetterBuildMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次信件创建时间（毫秒）',"
                + "`lastHelpBuildMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次求助创建时间（毫秒）',"
                + "`lastEventBuildMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次事件创建时间（毫秒）',"
                + "`lastCureSickPeopleMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次治愈居民时间（毫秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='火星-火星居民数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//cid
        _size+=8;//startMs
        _size+=8;//endMs
        _size+=4;//curUsedCount
        _size+=4;//dayTag
        _size+=4;//usedCount
        _size+=8;//immigrantNum
        _size+=8;//idleNum
        _size+=8;//sickNum
        _size+=4;//satisfaction
        _size+=8;//lastCalMs
        _size+=2;_size+=dailyEventInfo.length;//dailyEventInfo
        _size+=8;//lastLetterBuildMs
        _size+=8;//lastHelpBuildMs
        _size+=8;//lastEventBuildMs
        _size+=8;//lastCureSickPeopleMs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(startMs);
        buff.putLong(endMs);
        buff.putInt(curUsedCount);
        buff.putInt(dayTag);
        buff.putInt(usedCount);
        buff.putLong(immigrantNum);
        buff.putLong(idleNum);
        buff.putLong(sickNum);
        buff.putInt(satisfaction);
        buff.putLong(lastCalMs);
        buff.putShort((short)(dailyEventInfo == null ? 0 : dailyEventInfo.length));if(null != dailyEventInfo){buff.put(dailyEventInfo);}
        buff.putLong(lastLetterBuildMs);
        buff.putLong(lastHelpBuildMs);
        buff.putLong(lastEventBuildMs);
        buff.putLong(lastCureSickPeopleMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        startMs=buff.getLong();
        endMs=buff.getLong();
        curUsedCount=buff.getInt();
        dayTag=buff.getInt();
        usedCount=buff.getInt();
        immigrantNum=buff.getLong();
        idleNum=buff.getLong();
        sickNum=buff.getLong();
        satisfaction=buff.getInt();
        lastCalMs=buff.getLong();
        int dailyEventInfo_count = buff.getShort();if(dailyEventInfo_count>0){dailyEventInfo = new byte[dailyEventInfo_count];buff.get(dailyEventInfo);}
        lastLetterBuildMs=buff.getLong();
        lastHelpBuildMs=buff.getLong();
        lastEventBuildMs=buff.getLong();
        lastCureSickPeopleMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
