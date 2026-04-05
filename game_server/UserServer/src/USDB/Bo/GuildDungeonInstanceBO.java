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
public class GuildDungeonInstanceBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guildId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guildId", comment = "联盟ID")
    private long guildId;

    public static final int FIELD_dungeonId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "dungeonId", comment = "联盟副本ID")
    private long dungeonId;

    public static final int FIELD_dungeonLvl =2;
    @DataBaseField(type = "int(11)", fieldname = "dungeonLvl", comment = "联盟副本等级")
    private int dungeonLvl;

    public static final int FIELD_startMs =3;
    @DataBaseField(type = "bigint(20)", fieldname = "startMs", comment = "联盟副本开始时间")
    private long startMs;

    public static final int FIELD_isSettled =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "isSettled", comment = "已结算")
    private boolean isSettled;

    public GuildDungeonInstanceBO() {
        id = 0;
        guildId = 0L;
        dungeonId = 0L;
        dungeonLvl = 0;
        startMs = 0L;
        isSettled = false;
    }

    public GuildDungeonInstanceBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guildId = rs.getLong(2);
        dungeonId = rs.getLong(3);
        dungeonLvl = rs.getInt(4);
        startMs = rs.getLong(5);
        isSettled = rs.getBoolean(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildDungeonInstanceBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guildId`, `dungeonId`, `dungeonLvl`, `startMs`, `isSettled`";
    }

    @Override
    public String getTableName() {
        return "`guild_dungeon_instance`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guildId).append("', ");
        strBuf.append("'").append(dungeonId).append("', ");
        strBuf.append("'").append(dungeonLvl).append("', ");
        strBuf.append("'").append(startMs).append("', ");
        strBuf.append("'").append(isSettled ? 1 : 0).append("', ");
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

    // 联盟ID
    public long getGuildId() { return this.guildId; }
    public void setGuildId(BM _bm, long guildId) {
        if(guildId==this.guildId) 
            return;
        this.guildId = guildId; 
        markField(_bm, FIELD_guildId); 
    }
    public void saveGuildId(BM _bm, long guildId) {
        if(guildId==this.guildId) 
            return;
        this.guildId = guildId;
        saveField(_bm, "guildId", guildId);
    }

    // 联盟副本ID
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

    // 联盟副本等级
    public int getDungeonLvl() { return this.dungeonLvl; }
    public void setDungeonLvl(BM _bm, int dungeonLvl) {
        if(dungeonLvl==this.dungeonLvl) 
            return;
        this.dungeonLvl = dungeonLvl; 
        markField(_bm, FIELD_dungeonLvl); 
    }
    public void saveDungeonLvl(BM _bm, int dungeonLvl) {
        if(dungeonLvl==this.dungeonLvl) 
            return;
        this.dungeonLvl = dungeonLvl;
        saveField(_bm, "dungeonLvl", dungeonLvl);
    }

    // 联盟副本开始时间
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

    // 已结算
    public boolean getIsSettled() { return this.isSettled; }
    public void setIsSettled(BM _bm, boolean isSettled) {
        if(isSettled==this.isSettled) 
            return;
        this.isSettled = isSettled; 
        markField(_bm, FIELD_isSettled); 
    }
    public void saveIsSettled(BM _bm, boolean isSettled) {
        if(isSettled==this.isSettled) 
            return;
        this.isSettled = isSettled;
        saveField(_bm, "isSettled", isSettled ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guildId` = '").append(guildId).append("',");
        sBuilder.append(" `dungeonId` = '").append(dungeonId).append("',");
        sBuilder.append(" `dungeonLvl` = '").append(dungeonLvl).append("',");
        sBuilder.append(" `startMs` = '").append(startMs).append("',");
        sBuilder.append(" `isSettled` = '").append(isSettled ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guildId)) sBuilder.append(" `guildId` = '").append(guildId).append("',");
        if(isFieldMarked(FIELD_dungeonId)) sBuilder.append(" `dungeonId` = '").append(dungeonId).append("',");
        if(isFieldMarked(FIELD_dungeonLvl)) sBuilder.append(" `dungeonLvl` = '").append(dungeonLvl).append("',");
        if(isFieldMarked(FIELD_startMs)) sBuilder.append(" `startMs` = '").append(startMs).append("',");
        if(isFieldMarked(FIELD_isSettled)) sBuilder.append(" `isSettled` = '").append(isSettled ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_dungeon_instance` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guildId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`dungeonId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟副本ID',"
                + "`dungeonLvl` int(11) NOT NULL DEFAULT '0' COMMENT '联盟副本等级',"
                + "`startMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟副本开始时间',"
                + "`isSettled` tinyint(1) NOT NULL DEFAULT '0' COMMENT '已结算',"
                + "KEY `guildId` (`guildId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟副本实例数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guildId
        _size+=8;//dungeonId
        _size+=4;//dungeonLvl
        _size+=8;//startMs
        _size+=1;//isSettled
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guildId);
        buff.putLong(dungeonId);
        buff.putInt(dungeonLvl);
        buff.putLong(startMs);
        buff.put((byte)(isSettled?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guildId=buff.getLong();
        dungeonId=buff.getLong();
        dungeonLvl=buff.getInt();
        startMs=buff.getLong();
        isSettled=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
