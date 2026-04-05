package CSDB.Bo;
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
public class EliteBattleRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_data =0;
    @DataBaseField(type = "text", fieldname = "data", comment = "战斗数据")
    private String data;

    public static final int FIELD_dungeonId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "dungeonId", comment = "地图ID")
    private long dungeonId;

    public static final int FIELD_battleResVersion =2;
    @DataBaseField(type = "bigint(20)", fieldname = "battleResVersion", comment = "战斗资源版本号")
    private long battleResVersion;

    public static final int FIELD_endTimeSec =3;
    @DataBaseField(type = "int(11)", fieldname = "endTimeSec", comment = "战斗结束时间")
    private int endTimeSec;

    public static final int FIELD_battleTimeMs =4;
    @DataBaseField(type = "int(11)", fieldname = "battleTimeMs", comment = "战斗持续时间")
    private int battleTimeMs;

    public static final int FIELD_playerNum =5;
    @DataBaseField(type = "int(11)", fieldname = "playerNum", comment = "玩家数量")
    private int playerNum;

    public static final int FIELD_avgDps =6;
    @DataBaseField(type = "int(11)", fieldname = "avgDps", comment = "综合Dps")
    private int avgDps;

    public static final int FIELD_maxKillNum =7;
    @DataBaseField(type = "int(11)", fieldname = "maxKillNum", comment = "最大击杀人口数量")
    private int maxKillNum;

    public static final int FIELD_totoalKillNum =8;
    @DataBaseField(type = "int(11)", fieldname = "totoalKillNum", comment = "总击杀人口数量")
    private int totoalKillNum;

    public static final int FIELD_viewCount =9;
    @DataBaseField(type = "int(11)", fieldname = "viewCount", comment = "查看次数")
    private int viewCount;

    public static final int FIELD_voteCount =10;
    @DataBaseField(type = "int(11)", fieldname = "voteCount", comment = "点赞次数")
    private int voteCount;

    public static final int FIELD_campA1 =11;
    @DataBaseField(type = "varbinary(1024)", fieldname = "campA1", comment = "阵营A")
    private byte[] campA1;

    public static final int FIELD_campB1 =12;
    @DataBaseField(type = "varbinary(1024)", fieldname = "campB1", comment = "阵营B")
    private byte[] campB1;

    public static final int FIELD_roomType =13;
    @DataBaseField(type = "int(11)", fieldname = "roomType", comment = "比赛类型")
    private int roomType;

    public EliteBattleRecordBO() {
        id = 0;
        data = "";
        dungeonId = 0L;
        battleResVersion = 0L;
        endTimeSec = 0;
        battleTimeMs = 0;
        playerNum = 0;
        avgDps = 0;
        maxKillNum = 0;
        totoalKillNum = 0;
        viewCount = 0;
        voteCount = 0;
        campA1 = null;
        campB1 = null;
        roomType = 0;
    }

    public EliteBattleRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        data = rs.getString(2);
        dungeonId = rs.getLong(3);
        battleResVersion = rs.getLong(4);
        endTimeSec = rs.getInt(5);
        battleTimeMs = rs.getInt(6);
        playerNum = rs.getInt(7);
        avgDps = rs.getInt(8);
        maxKillNum = rs.getInt(9);
        totoalKillNum = rs.getInt(10);
        viewCount = rs.getInt(11);
        voteCount = rs.getInt(12);
        campA1 = rs.getBytes(13);
        campB1 = rs.getBytes(14);
        roomType = rs.getInt(15);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new EliteBattleRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `data`, `dungeonId`, `battleResVersion`, `endTimeSec`, `battleTimeMs`, `playerNum`, `avgDps`, `maxKillNum`, `totoalKillNum`, `viewCount`, `voteCount`, `campA1`, `campB1`, `roomType`";
    }

    @Override
    public String getTableName() {
        return "`elite_battle_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(data == null ? null : data.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(dungeonId).append("', ");
        strBuf.append("'").append(battleResVersion).append("', ");
        strBuf.append("'").append(endTimeSec).append("', ");
        strBuf.append("'").append(battleTimeMs).append("', ");
        strBuf.append("'").append(playerNum).append("', ");
        strBuf.append("'").append(avgDps).append("', ");
        strBuf.append("'").append(maxKillNum).append("', ");
        strBuf.append("'").append(totoalKillNum).append("', ");
        strBuf.append("'").append(viewCount).append("', ");
        strBuf.append("'").append(voteCount).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("'").append(roomType).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(campA1); 
        ret.add(campB1);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_campA1)) ret.add(campA1); 
        if(isFieldMarked(FIELD_campB1)) ret.add(campB1);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 战斗数据
    public String getData() { return this.data; }
    public void setData(BM _bm, String data) {
        if(data.equals(this.data)) 
            return;
        this.data = data; 
        markField(_bm, FIELD_data); 
    }
    public void saveData(BM _bm, String data) {
        if(data.equals(this.data)) 
            return;
        this.data = data;
        saveField(_bm, "data", data);
    }

    // 地图ID
    public long getDungeonId() { return this.dungeonId; }
    public void setDungeonId(BM _bm, long dungeonId) {
        if(dungeonId==this.dungeonId) 
            return;
        this.dungeonId = dungeonId; 
        markField(_bm, FIELD_dungeonId); 
    }
    public void saveDungeonId(BM _bm, long dungeonId) {
        if(dungeonId==this.dungeonId) 
            return;
        this.dungeonId = dungeonId;
        saveField(_bm, "dungeonId", dungeonId);
    }

    // 战斗资源版本号
    public long getBattleResVersion() { return this.battleResVersion; }
    public void setBattleResVersion(BM _bm, long battleResVersion) {
        if(battleResVersion==this.battleResVersion) 
            return;
        this.battleResVersion = battleResVersion; 
        markField(_bm, FIELD_battleResVersion); 
    }
    public void saveBattleResVersion(BM _bm, long battleResVersion) {
        if(battleResVersion==this.battleResVersion) 
            return;
        this.battleResVersion = battleResVersion;
        saveField(_bm, "battleResVersion", battleResVersion);
    }

    // 战斗结束时间
    public int getEndTimeSec() { return this.endTimeSec; }
    public void setEndTimeSec(BM _bm, int endTimeSec) {
        if(endTimeSec==this.endTimeSec) 
            return;
        this.endTimeSec = endTimeSec; 
        markField(_bm, FIELD_endTimeSec); 
    }
    public void saveEndTimeSec(BM _bm, int endTimeSec) {
        if(endTimeSec==this.endTimeSec) 
            return;
        this.endTimeSec = endTimeSec;
        saveField(_bm, "endTimeSec", endTimeSec);
    }

    // 战斗持续时间
    public int getBattleTimeMs() { return this.battleTimeMs; }
    public void setBattleTimeMs(BM _bm, int battleTimeMs) {
        if(battleTimeMs==this.battleTimeMs) 
            return;
        this.battleTimeMs = battleTimeMs; 
        markField(_bm, FIELD_battleTimeMs); 
    }
    public void saveBattleTimeMs(BM _bm, int battleTimeMs) {
        if(battleTimeMs==this.battleTimeMs) 
            return;
        this.battleTimeMs = battleTimeMs;
        saveField(_bm, "battleTimeMs", battleTimeMs);
    }

    // 玩家数量
    public int getPlayerNum() { return this.playerNum; }
    public void setPlayerNum(BM _bm, int playerNum) {
        if(playerNum==this.playerNum) 
            return;
        this.playerNum = playerNum; 
        markField(_bm, FIELD_playerNum); 
    }
    public void savePlayerNum(BM _bm, int playerNum) {
        if(playerNum==this.playerNum) 
            return;
        this.playerNum = playerNum;
        saveField(_bm, "playerNum", playerNum);
    }

    // 综合Dps
    public int getAvgDps() { return this.avgDps; }
    public void setAvgDps(BM _bm, int avgDps) {
        if(avgDps==this.avgDps) 
            return;
        this.avgDps = avgDps; 
        markField(_bm, FIELD_avgDps); 
    }
    public void saveAvgDps(BM _bm, int avgDps) {
        if(avgDps==this.avgDps) 
            return;
        this.avgDps = avgDps;
        saveField(_bm, "avgDps", avgDps);
    }

    // 最大击杀人口数量
    public int getMaxKillNum() { return this.maxKillNum; }
    public void setMaxKillNum(BM _bm, int maxKillNum) {
        if(maxKillNum==this.maxKillNum) 
            return;
        this.maxKillNum = maxKillNum; 
        markField(_bm, FIELD_maxKillNum); 
    }
    public void saveMaxKillNum(BM _bm, int maxKillNum) {
        if(maxKillNum==this.maxKillNum) 
            return;
        this.maxKillNum = maxKillNum;
        saveField(_bm, "maxKillNum", maxKillNum);
    }

    // 总击杀人口数量
    public int getTotoalKillNum() { return this.totoalKillNum; }
    public void setTotoalKillNum(BM _bm, int totoalKillNum) {
        if(totoalKillNum==this.totoalKillNum) 
            return;
        this.totoalKillNum = totoalKillNum; 
        markField(_bm, FIELD_totoalKillNum); 
    }
    public void saveTotoalKillNum(BM _bm, int totoalKillNum) {
        if(totoalKillNum==this.totoalKillNum) 
            return;
        this.totoalKillNum = totoalKillNum;
        saveField(_bm, "totoalKillNum", totoalKillNum);
    }

    // 查看次数
    public int getViewCount() { return this.viewCount; }
    public void setViewCount(BM _bm, int viewCount) {
        if(viewCount==this.viewCount) 
            return;
        this.viewCount = viewCount; 
        markField(_bm, FIELD_viewCount); 
    }
    public void saveViewCount(BM _bm, int viewCount) {
        if(viewCount==this.viewCount) 
            return;
        this.viewCount = viewCount;
        saveField(_bm, "viewCount", viewCount);
    }

    // 点赞次数
    public int getVoteCount() { return this.voteCount; }
    public void setVoteCount(BM _bm, int voteCount) {
        if(voteCount==this.voteCount) 
            return;
        this.voteCount = voteCount; 
        markField(_bm, FIELD_voteCount); 
    }
    public void saveVoteCount(BM _bm, int voteCount) {
        if(voteCount==this.voteCount) 
            return;
        this.voteCount = voteCount;
        saveField(_bm, "voteCount", voteCount);
    }

    // 阵营A
    public byte[] getCampA1() { return this.campA1; }
    public void setCampA1(BM _bm, byte[] campA1) {
        if(campA1==this.campA1) 
            return;
        this.campA1 = campA1; 
        markField(_bm, FIELD_campA1); 
    }
    public void saveCampA1(BM _bm, byte[] campA1) {
        if(campA1==this.campA1) 
            return;
        this.campA1 = campA1;
        saveFieldBytes(_bm, "campA1", campA1);
    }

    // 阵营B
    public byte[] getCampB1() { return this.campB1; }
    public void setCampB1(BM _bm, byte[] campB1) {
        if(campB1==this.campB1) 
            return;
        this.campB1 = campB1; 
        markField(_bm, FIELD_campB1); 
    }
    public void saveCampB1(BM _bm, byte[] campB1) {
        if(campB1==this.campB1) 
            return;
        this.campB1 = campB1;
        saveFieldBytes(_bm, "campB1", campB1);
    }

    // 比赛类型
    public int getRoomType() { return this.roomType; }
    public void setRoomType(BM _bm, int roomType) {
        if(roomType==this.roomType) 
            return;
        this.roomType = roomType; 
        markField(_bm, FIELD_roomType); 
    }
    public void saveRoomType(BM _bm, int roomType) {
        if(roomType==this.roomType) 
            return;
        this.roomType = roomType;
        saveField(_bm, "roomType", roomType);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `data` = '").append(data == null ? null : data.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `dungeonId` = '").append(dungeonId).append("',");
        sBuilder.append(" `battleResVersion` = '").append(battleResVersion).append("',");
        sBuilder.append(" `endTimeSec` = '").append(endTimeSec).append("',");
        sBuilder.append(" `battleTimeMs` = '").append(battleTimeMs).append("',");
        sBuilder.append(" `playerNum` = '").append(playerNum).append("',");
        sBuilder.append(" `avgDps` = '").append(avgDps).append("',");
        sBuilder.append(" `maxKillNum` = '").append(maxKillNum).append("',");
        sBuilder.append(" `totoalKillNum` = '").append(totoalKillNum).append("',");
        sBuilder.append(" `viewCount` = '").append(viewCount).append("',");
        sBuilder.append(" `voteCount` = '").append(voteCount).append("',");
        sBuilder.append(" `campA1` = ?,");
        sBuilder.append(" `campB1` = ?,");
        sBuilder.append(" `roomType` = '").append(roomType).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_data)) sBuilder.append(" `data` = '").append(data == null ? null : data.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_dungeonId)) sBuilder.append(" `dungeonId` = '").append(dungeonId).append("',");
        if(isFieldMarked(FIELD_battleResVersion)) sBuilder.append(" `battleResVersion` = '").append(battleResVersion).append("',");
        if(isFieldMarked(FIELD_endTimeSec)) sBuilder.append(" `endTimeSec` = '").append(endTimeSec).append("',");
        if(isFieldMarked(FIELD_battleTimeMs)) sBuilder.append(" `battleTimeMs` = '").append(battleTimeMs).append("',");
        if(isFieldMarked(FIELD_playerNum)) sBuilder.append(" `playerNum` = '").append(playerNum).append("',");
        if(isFieldMarked(FIELD_avgDps)) sBuilder.append(" `avgDps` = '").append(avgDps).append("',");
        if(isFieldMarked(FIELD_maxKillNum)) sBuilder.append(" `maxKillNum` = '").append(maxKillNum).append("',");
        if(isFieldMarked(FIELD_totoalKillNum)) sBuilder.append(" `totoalKillNum` = '").append(totoalKillNum).append("',");
        if(isFieldMarked(FIELD_viewCount)) sBuilder.append(" `viewCount` = '").append(viewCount).append("',");
        if(isFieldMarked(FIELD_voteCount)) sBuilder.append(" `voteCount` = '").append(voteCount).append("',");
        if(isFieldMarked(FIELD_campA1)) sBuilder.append(" `campA1` = ?,");
        if(isFieldMarked(FIELD_campB1)) sBuilder.append(" `campB1` = ?,");
        if(isFieldMarked(FIELD_roomType)) sBuilder.append(" `roomType` = '").append(roomType).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `elite_battle_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`data` text NULL COMMENT '战斗数据',"
                + "`dungeonId` bigint(20) NOT NULL DEFAULT '0' COMMENT '地图ID',"
                + "`battleResVersion` bigint(20) NOT NULL DEFAULT '0' COMMENT '战斗资源版本号',"
                + "`endTimeSec` int(11) NOT NULL DEFAULT '0' COMMENT '战斗结束时间',"
                + "`battleTimeMs` int(11) NOT NULL DEFAULT '0' COMMENT '战斗持续时间',"
                + "`playerNum` int(11) NOT NULL DEFAULT '0' COMMENT '玩家数量',"
                + "`avgDps` int(11) NOT NULL DEFAULT '0' COMMENT '综合Dps',"
                + "`maxKillNum` int(11) NOT NULL DEFAULT '0' COMMENT '最大击杀人口数量',"
                + "`totoalKillNum` int(11) NOT NULL DEFAULT '0' COMMENT '总击杀人口数量',"
                + "`viewCount` int(11) NOT NULL DEFAULT '0' COMMENT '查看次数',"
                + "`voteCount` int(11) NOT NULL DEFAULT '0' COMMENT '点赞次数',"
                + "`campA1` varbinary(1024) NOT NULL DEFAULT '' COMMENT '阵营A',"
                + "`campB1` varbinary(1024) NOT NULL DEFAULT '' COMMENT '阵营B',"
                + "`roomType` int(11) NOT NULL DEFAULT '0' COMMENT '比赛类型',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='精彩比赛录像' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.comm_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(data);//data
        _size+=8;//dungeonId
        _size+=8;//battleResVersion
        _size+=4;//endTimeSec
        _size+=4;//battleTimeMs
        _size+=4;//playerNum
        _size+=4;//avgDps
        _size+=4;//maxKillNum
        _size+=4;//totoalKillNum
        _size+=4;//viewCount
        _size+=4;//voteCount
        _size+=2;_size+=campA1.length;//campA1
        _size+=2;_size+=campB1.length;//campB1
        _size+=4;//roomType
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, data);
        buff.putLong(dungeonId);
        buff.putLong(battleResVersion);
        buff.putInt(endTimeSec);
        buff.putInt(battleTimeMs);
        buff.putInt(playerNum);
        buff.putInt(avgDps);
        buff.putInt(maxKillNum);
        buff.putInt(totoalKillNum);
        buff.putInt(viewCount);
        buff.putInt(voteCount);
        buff.putShort((short)(campA1 == null ? 0 : campA1.length));if(null != campA1){buff.put(campA1);}
        buff.putShort((short)(campB1 == null ? 0 : campB1.length));if(null != campB1){buff.put(campB1);}
        buff.putInt(roomType);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        data=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        dungeonId=buff.getLong();
        battleResVersion=buff.getLong();
        endTimeSec=buff.getInt();
        battleTimeMs=buff.getInt();
        playerNum=buff.getInt();
        avgDps=buff.getInt();
        maxKillNum=buff.getInt();
        totoalKillNum=buff.getInt();
        viewCount=buff.getInt();
        voteCount=buff.getInt();
        int campA1_count = buff.getShort();if(campA1_count>0){campA1 = new byte[campA1_count];buff.get(campA1);}
        int campB1_count = buff.getShort();if(campB1_count>0){campB1 = new byte[campB1_count];buff.get(campB1);}
        roomType=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
