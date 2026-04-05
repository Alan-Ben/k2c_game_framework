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

@RefBo(isIdAuto = false)
public class PlayerGuildDungeonHeroBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_heroId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "heroId", comment = "大臣ID")
    private long heroId;

    public static final int FIELD_fightedCount =2;
    @DataBaseField(type = "int(11)", fieldname = "fightedCount", comment = "战斗次数")
    private int fightedCount;

    public static final int FIELD_recoveredCount =3;
    @DataBaseField(type = "int(11)", fieldname = "recoveredCount", comment = "恢复次数")
    private int recoveredCount;

    public static final int FIELD_lastFightedMs =4;
    @DataBaseField(type = "bigint(20)", fieldname = "lastFightedMs", comment = "最近一次战斗时间")
    private long lastFightedMs;

    public PlayerGuildDungeonHeroBO() {
        id = 0;
        cid = 0L;
        heroId = 0L;
        fightedCount = 0;
        recoveredCount = 0;
        lastFightedMs = 0L;
    }

    public PlayerGuildDungeonHeroBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        heroId = rs.getLong(3);
        fightedCount = rs.getInt(4);
        recoveredCount = rs.getInt(5);
        lastFightedMs = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerGuildDungeonHeroBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `heroId`, `fightedCount`, `recoveredCount`, `lastFightedMs`";
    }

    @Override
    public String getTableName() {
        return "`player_guild_dungeon_hero`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(heroId).append("', ");
        strBuf.append("'").append(fightedCount).append("', ");
        strBuf.append("'").append(recoveredCount).append("', ");
        strBuf.append("'").append(lastFightedMs).append("', ");
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

    // 大臣ID
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

    // 战斗次数
    public int getFightedCount() { return this.fightedCount; }
    public void setFightedCount(BM _bm, int fightedCount) {
        if(fightedCount==this.fightedCount) 
            return;
        this.fightedCount = fightedCount; 
        markField(_bm, FIELD_fightedCount); 
    }
    public void saveFightedCount(BM _bm, int fightedCount) {
        if(fightedCount==this.fightedCount) 
            return;
        this.fightedCount = fightedCount;
        saveField(_bm, "fightedCount", fightedCount);
    }

    // 恢复次数
    public int getRecoveredCount() { return this.recoveredCount; }
    public void setRecoveredCount(BM _bm, int recoveredCount) {
        if(recoveredCount==this.recoveredCount) 
            return;
        this.recoveredCount = recoveredCount; 
        markField(_bm, FIELD_recoveredCount); 
    }
    public void saveRecoveredCount(BM _bm, int recoveredCount) {
        if(recoveredCount==this.recoveredCount) 
            return;
        this.recoveredCount = recoveredCount;
        saveField(_bm, "recoveredCount", recoveredCount);
    }

    // 最近一次战斗时间
    public long getLastFightedMs() { return this.lastFightedMs; }
    public void setLastFightedMs(BM _bm, long lastFightedMs) {
        if(lastFightedMs==this.lastFightedMs) 
            return;
        this.lastFightedMs = lastFightedMs; 
        markField(_bm, FIELD_lastFightedMs); 
    }
    public void saveLastFightedMs(BM _bm, long lastFightedMs) {
        if(lastFightedMs==this.lastFightedMs) 
            return;
        this.lastFightedMs = lastFightedMs;
        saveField(_bm, "lastFightedMs", lastFightedMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `heroId` = '").append(heroId).append("',");
        sBuilder.append(" `fightedCount` = '").append(fightedCount).append("',");
        sBuilder.append(" `recoveredCount` = '").append(recoveredCount).append("',");
        sBuilder.append(" `lastFightedMs` = '").append(lastFightedMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_heroId)) sBuilder.append(" `heroId` = '").append(heroId).append("',");
        if(isFieldMarked(FIELD_fightedCount)) sBuilder.append(" `fightedCount` = '").append(fightedCount).append("',");
        if(isFieldMarked(FIELD_recoveredCount)) sBuilder.append(" `recoveredCount` = '").append(recoveredCount).append("',");
        if(isFieldMarked(FIELD_lastFightedMs)) sBuilder.append(" `lastFightedMs` = '").append(lastFightedMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_guild_dungeon_hero` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`heroId` bigint(20) NOT NULL DEFAULT '0' COMMENT '大臣ID',"
                + "`fightedCount` int(11) NOT NULL DEFAULT '0' COMMENT '战斗次数',"
                + "`recoveredCount` int(11) NOT NULL DEFAULT '0' COMMENT '恢复次数',"
                + "`lastFightedMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '最近一次战斗时间',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家公会副本大臣出战数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//heroId
        _size+=4;//fightedCount
        _size+=4;//recoveredCount
        _size+=8;//lastFightedMs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(heroId);
        buff.putInt(fightedCount);
        buff.putInt(recoveredCount);
        buff.putLong(lastFightedMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        heroId=buff.getLong();
        fightedCount=buff.getInt();
        recoveredCount=buff.getInt();
        lastFightedMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
