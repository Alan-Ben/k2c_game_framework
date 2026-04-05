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
public class GuildDungeonSetBO extends BaseBO {
    
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

    public static final int FIELD_isAutoStart =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "isAutoStart", comment = "设置自动开启")
    private boolean isAutoStart;

    public GuildDungeonSetBO() {
        id = 0;
        guildId = 0L;
        dungeonId = 0L;
        dungeonLvl = 0;
        isAutoStart = false;
    }

    public GuildDungeonSetBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guildId = rs.getLong(2);
        dungeonId = rs.getLong(3);
        dungeonLvl = rs.getInt(4);
        isAutoStart = rs.getBoolean(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildDungeonSetBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guildId`, `dungeonId`, `dungeonLvl`, `isAutoStart`";
    }

    @Override
    public String getTableName() {
        return "`guild_dungeon_set`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guildId).append("', ");
        strBuf.append("'").append(dungeonId).append("', ");
        strBuf.append("'").append(dungeonLvl).append("', ");
        strBuf.append("'").append(isAutoStart ? 1 : 0).append("', ");
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

    // 设置自动开启
    public boolean getIsAutoStart() { return this.isAutoStart; }
    public void setIsAutoStart(BM _bm, boolean isAutoStart) {
        if(isAutoStart==this.isAutoStart) 
            return;
        this.isAutoStart = isAutoStart; 
        markField(_bm, FIELD_isAutoStart); 
    }
    public void saveIsAutoStart(BM _bm, boolean isAutoStart) {
        if(isAutoStart==this.isAutoStart) 
            return;
        this.isAutoStart = isAutoStart;
        saveField(_bm, "isAutoStart", isAutoStart ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guildId` = '").append(guildId).append("',");
        sBuilder.append(" `dungeonId` = '").append(dungeonId).append("',");
        sBuilder.append(" `dungeonLvl` = '").append(dungeonLvl).append("',");
        sBuilder.append(" `isAutoStart` = '").append(isAutoStart ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guildId)) sBuilder.append(" `guildId` = '").append(guildId).append("',");
        if(isFieldMarked(FIELD_dungeonId)) sBuilder.append(" `dungeonId` = '").append(dungeonId).append("',");
        if(isFieldMarked(FIELD_dungeonLvl)) sBuilder.append(" `dungeonLvl` = '").append(dungeonLvl).append("',");
        if(isFieldMarked(FIELD_isAutoStart)) sBuilder.append(" `isAutoStart` = '").append(isAutoStart ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_dungeon_set` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guildId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`dungeonId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟副本ID',"
                + "`dungeonLvl` int(11) NOT NULL DEFAULT '0' COMMENT '联盟副本等级',"
                + "`isAutoStart` tinyint(1) NOT NULL DEFAULT '0' COMMENT '设置自动开启',"
                + "KEY `guildId` (`guildId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟副本配置数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=1;//isAutoStart
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
        buff.put((byte)(isAutoStart?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guildId=buff.getLong();
        dungeonId=buff.getLong();
        dungeonLvl=buff.getInt();
        isAutoStart=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
