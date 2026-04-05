package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogMarsTeamBattleBO extends BaseLogBo {

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

    public static final int FIELD_teamId =6;
    @DataBaseField(type = "bigint(20)", fieldname = "teamId", comment = "队伍ID")
    private long teamId;

    public static final int FIELD_eventInstanceId =7;
    @DataBaseField(type = "bigint(20)", fieldname = "eventInstanceId", comment = "事件实例ID")
    private long eventInstanceId;

    public static final int FIELD_battleEventId =8;
    @DataBaseField(type = "bigint(20)", fieldname = "battleEventId", comment = "事件ID")
    private long battleEventId;

    public static final int FIELD_isWin =9;
    @DataBaseField(type = "tinyint(1)", fieldname = "isWin", comment = "true-攻击方胜利")
    private boolean isWin;

    public static final int FIELD_troopNum =10;
    @DataBaseField(type = "bigint(20)", fieldname = "troopNum", comment = "防守方当前精兵")
    private long troopNum;

    public static final int FIELD_teamPower =11;
    @DataBaseField(type = "bigint(20)", fieldname = "teamPower", comment = "攻击方队伍实力")
    private long teamPower;

    public static final int FIELD_defencePower =12;
    @DataBaseField(type = "bigint(20)", fieldname = "defencePower", comment = "防守方实力")
    private long defencePower;

    public LogMarsTeamBattleBO() {
        id = 0;
        cid = 0L;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        player_level = 0;
        teamId = 0L;
        eventInstanceId = 0L;
        battleEventId = 0L;
        isWin = false;
        troopNum = 0L;
        teamPower = 0L;
        defencePower = 0L;
    }

    public LogMarsTeamBattleBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        player_level = rs.getInt(7);
        teamId = rs.getLong(8);
        eventInstanceId = rs.getLong(9);
        battleEventId = rs.getLong(10);
        isWin = rs.getBoolean(11);
        troopNum = rs.getLong(12);
        teamPower = rs.getLong(13);
        defencePower = rs.getLong(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogMarsTeamBattleBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `event_id`, `guid`, `date_time`, `timestamp`, `player_level`, `teamId`, `eventInstanceId`, `battleEventId`, `isWin`, `troopNum`, `teamPower`, `defencePower`";
    }

    @Override
    public String getTableName() {
        return "`log_mars_team_battle`";
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
        strBuf.append("'").append(teamId).append("', ");
        strBuf.append("'").append(eventInstanceId).append("', ");
        strBuf.append("'").append(battleEventId).append("', ");
        strBuf.append("'").append(isWin ? 1 : 0).append("', ");
        strBuf.append("'").append(troopNum).append("', ");
        strBuf.append("'").append(teamPower).append("', ");
        strBuf.append("'").append(defencePower).append("', ");
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

    // 队伍ID
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

    // 事件实例ID
    public long getEventInstanceId() { return this.eventInstanceId; }
    public void setEventInstanceId(BM _bm, long eventInstanceId) {
        if(eventInstanceId==this.eventInstanceId) 
            return;
        this.eventInstanceId = eventInstanceId; 
        markField(_bm, FIELD_eventInstanceId); 
    }
    public void saveEventInstanceId(BM _bm, long eventInstanceId) {
        if(eventInstanceId==this.eventInstanceId) 
            return;
        this.eventInstanceId = eventInstanceId;
        saveField(_bm, "eventInstanceId", eventInstanceId);
    }

    // 事件ID
    public long getBattleEventId() { return this.battleEventId; }
    public void setBattleEventId(BM _bm, long battleEventId) {
        if(battleEventId==this.battleEventId) 
            return;
        this.battleEventId = battleEventId; 
        markField(_bm, FIELD_battleEventId); 
    }
    public void saveBattleEventId(BM _bm, long battleEventId) {
        if(battleEventId==this.battleEventId) 
            return;
        this.battleEventId = battleEventId;
        saveField(_bm, "battleEventId", battleEventId);
    }

    // true-攻击方胜利
    public boolean getIsWin() { return this.isWin; }
    public void setIsWin(BM _bm, boolean isWin) {
        if(isWin==this.isWin) 
            return;
        this.isWin = isWin; 
        markField(_bm, FIELD_isWin); 
    }
    public void saveIsWin(BM _bm, boolean isWin) {
        if(isWin==this.isWin) 
            return;
        this.isWin = isWin;
        saveField(_bm, "isWin", isWin ? 1 : 0);
    }

    // 防守方当前精兵
    public long getTroopNum() { return this.troopNum; }
    public void setTroopNum(BM _bm, long troopNum) {
        if(troopNum==this.troopNum) 
            return;
        this.troopNum = troopNum; 
        markField(_bm, FIELD_troopNum); 
    }
    public void saveTroopNum(BM _bm, long troopNum) {
        if(troopNum==this.troopNum) 
            return;
        this.troopNum = troopNum;
        saveField(_bm, "troopNum", troopNum);
    }

    // 攻击方队伍实力
    public long getTeamPower() { return this.teamPower; }
    public void setTeamPower(BM _bm, long teamPower) {
        if(teamPower==this.teamPower) 
            return;
        this.teamPower = teamPower; 
        markField(_bm, FIELD_teamPower); 
    }
    public void saveTeamPower(BM _bm, long teamPower) {
        if(teamPower==this.teamPower) 
            return;
        this.teamPower = teamPower;
        saveField(_bm, "teamPower", teamPower);
    }

    // 防守方实力
    public long getDefencePower() { return this.defencePower; }
    public void setDefencePower(BM _bm, long defencePower) {
        if(defencePower==this.defencePower) 
            return;
        this.defencePower = defencePower; 
        markField(_bm, FIELD_defencePower); 
    }
    public void saveDefencePower(BM _bm, long defencePower) {
        if(defencePower==this.defencePower) 
            return;
        this.defencePower = defencePower;
        saveField(_bm, "defencePower", defencePower);
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
        sBuilder.append(" `teamId` = '").append(teamId).append("',");
        sBuilder.append(" `eventInstanceId` = '").append(eventInstanceId).append("',");
        sBuilder.append(" `battleEventId` = '").append(battleEventId).append("',");
        sBuilder.append(" `isWin` = '").append(isWin ? 1 : 0).append("',");
        sBuilder.append(" `troopNum` = '").append(troopNum).append("',");
        sBuilder.append(" `teamPower` = '").append(teamPower).append("',");
        sBuilder.append(" `defencePower` = '").append(defencePower).append("',");
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
        if(isFieldMarked(FIELD_teamId)) sBuilder.append(" `teamId` = '").append(teamId).append("',");
        if(isFieldMarked(FIELD_eventInstanceId)) sBuilder.append(" `eventInstanceId` = '").append(eventInstanceId).append("',");
        if(isFieldMarked(FIELD_battleEventId)) sBuilder.append(" `battleEventId` = '").append(battleEventId).append("',");
        if(isFieldMarked(FIELD_isWin)) sBuilder.append(" `isWin` = '").append(isWin ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_troopNum)) sBuilder.append(" `troopNum` = '").append(troopNum).append("',");
        if(isFieldMarked(FIELD_teamPower)) sBuilder.append(" `teamPower` = '").append(teamPower).append("',");
        if(isFieldMarked(FIELD_defencePower)) sBuilder.append(" `defencePower` = '").append(defencePower).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_mars_team_battle` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`player_level` int(11) NOT NULL DEFAULT '0' COMMENT '玩家等级',"
                + "`teamId` bigint(20) NOT NULL DEFAULT '0' COMMENT '队伍ID',"
                + "`eventInstanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件实例ID',"
                + "`battleEventId` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件ID',"
                + "`isWin` tinyint(1) NOT NULL DEFAULT '0' COMMENT 'true-攻击方胜利',"
                + "`troopNum` bigint(20) NOT NULL DEFAULT '0' COMMENT '防守方当前精兵',"
                + "`teamPower` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击方队伍实力',"
                + "`defencePower` bigint(20) NOT NULL DEFAULT '0' COMMENT '防守方实力',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='火星队伍battle事件数据日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//teamId
        _size+=8;//eventInstanceId
        _size+=8;//battleEventId
        _size+=1;//isWin
        _size+=8;//troopNum
        _size+=8;//teamPower
        _size+=8;//defencePower
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
        buff.putLong(teamId);
        buff.putLong(eventInstanceId);
        buff.putLong(battleEventId);
        buff.put((byte)(isWin?1:0));
        buff.putLong(troopNum);
        buff.putLong(teamPower);
        buff.putLong(defencePower);        
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
        teamId=buff.getLong();
        eventInstanceId=buff.getLong();
        battleEventId=buff.getLong();
        isWin=(buff.get()==1);
        troopNum=buff.getLong();
        teamPower=buff.getLong();
        defencePower=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
