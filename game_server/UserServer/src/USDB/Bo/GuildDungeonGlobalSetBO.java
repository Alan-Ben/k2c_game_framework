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
public class GuildDungeonGlobalSetBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guildId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guildId", comment = "联盟ID")
    private long guildId;

    public static final int FIELD_autoHour =1;
    @DataBaseField(type = "int(11)", fieldname = "autoHour", comment = "自动开启时间：小时")
    private int autoHour;

    public static final int FIELD_autoMin =2;
    @DataBaseField(type = "int(11)", fieldname = "autoMin", comment = "自动开启时间：分钟")
    private int autoMin;

    public static final int FIELD_lastAutoStartedMs =3;
    @DataBaseField(type = "bigint(20)", fieldname = "lastAutoStartedMs", comment = "上次自动开启时间（毫秒）")
    private long lastAutoStartedMs;

    public static final int FIELD_lastAutoSettledMs =4;
    @DataBaseField(type = "bigint(20)", fieldname = "lastAutoSettledMs", comment = "上次自动结算时间（毫秒）")
    private long lastAutoSettledMs;

    public GuildDungeonGlobalSetBO() {
        id = 0;
        guildId = 0L;
        autoHour = 0;
        autoMin = 0;
        lastAutoStartedMs = 0L;
        lastAutoSettledMs = 0L;
    }

    public GuildDungeonGlobalSetBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guildId = rs.getLong(2);
        autoHour = rs.getInt(3);
        autoMin = rs.getInt(4);
        lastAutoStartedMs = rs.getLong(5);
        lastAutoSettledMs = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildDungeonGlobalSetBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guildId`, `autoHour`, `autoMin`, `lastAutoStartedMs`, `lastAutoSettledMs`";
    }

    @Override
    public String getTableName() {
        return "`guild_dungeon_global_set`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guildId).append("', ");
        strBuf.append("'").append(autoHour).append("', ");
        strBuf.append("'").append(autoMin).append("', ");
        strBuf.append("'").append(lastAutoStartedMs).append("', ");
        strBuf.append("'").append(lastAutoSettledMs).append("', ");
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

    // 自动开启时间：小时
    public int getAutoHour() { return this.autoHour; }
    public void setAutoHour(BM _bm, int autoHour) {
        if(autoHour==this.autoHour) 
            return;
        this.autoHour = autoHour; 
        markField(_bm, FIELD_autoHour); 
    }
    public void saveAutoHour(BM _bm, int autoHour) {
        if(autoHour==this.autoHour) 
            return;
        this.autoHour = autoHour;
        saveField(_bm, "autoHour", autoHour);
    }

    // 自动开启时间：分钟
    public int getAutoMin() { return this.autoMin; }
    public void setAutoMin(BM _bm, int autoMin) {
        if(autoMin==this.autoMin) 
            return;
        this.autoMin = autoMin; 
        markField(_bm, FIELD_autoMin); 
    }
    public void saveAutoMin(BM _bm, int autoMin) {
        if(autoMin==this.autoMin) 
            return;
        this.autoMin = autoMin;
        saveField(_bm, "autoMin", autoMin);
    }

    // 上次自动开启时间（毫秒）
    public long getLastAutoStartedMs() { return this.lastAutoStartedMs; }
    public void setLastAutoStartedMs(BM _bm, long lastAutoStartedMs) {
        if(lastAutoStartedMs==this.lastAutoStartedMs) 
            return;
        this.lastAutoStartedMs = lastAutoStartedMs; 
        markField(_bm, FIELD_lastAutoStartedMs); 
    }
    public void saveLastAutoStartedMs(BM _bm, long lastAutoStartedMs) {
        if(lastAutoStartedMs==this.lastAutoStartedMs) 
            return;
        this.lastAutoStartedMs = lastAutoStartedMs;
        saveField(_bm, "lastAutoStartedMs", lastAutoStartedMs);
    }

    // 上次自动结算时间（毫秒）
    public long getLastAutoSettledMs() { return this.lastAutoSettledMs; }
    public void setLastAutoSettledMs(BM _bm, long lastAutoSettledMs) {
        if(lastAutoSettledMs==this.lastAutoSettledMs) 
            return;
        this.lastAutoSettledMs = lastAutoSettledMs; 
        markField(_bm, FIELD_lastAutoSettledMs); 
    }
    public void saveLastAutoSettledMs(BM _bm, long lastAutoSettledMs) {
        if(lastAutoSettledMs==this.lastAutoSettledMs) 
            return;
        this.lastAutoSettledMs = lastAutoSettledMs;
        saveField(_bm, "lastAutoSettledMs", lastAutoSettledMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guildId` = '").append(guildId).append("',");
        sBuilder.append(" `autoHour` = '").append(autoHour).append("',");
        sBuilder.append(" `autoMin` = '").append(autoMin).append("',");
        sBuilder.append(" `lastAutoStartedMs` = '").append(lastAutoStartedMs).append("',");
        sBuilder.append(" `lastAutoSettledMs` = '").append(lastAutoSettledMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guildId)) sBuilder.append(" `guildId` = '").append(guildId).append("',");
        if(isFieldMarked(FIELD_autoHour)) sBuilder.append(" `autoHour` = '").append(autoHour).append("',");
        if(isFieldMarked(FIELD_autoMin)) sBuilder.append(" `autoMin` = '").append(autoMin).append("',");
        if(isFieldMarked(FIELD_lastAutoStartedMs)) sBuilder.append(" `lastAutoStartedMs` = '").append(lastAutoStartedMs).append("',");
        if(isFieldMarked(FIELD_lastAutoSettledMs)) sBuilder.append(" `lastAutoSettledMs` = '").append(lastAutoSettledMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_dungeon_global_set` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guildId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`autoHour` int(11) NOT NULL DEFAULT '0' COMMENT '自动开启时间：小时',"
                + "`autoMin` int(11) NOT NULL DEFAULT '0' COMMENT '自动开启时间：分钟',"
                + "`lastAutoStartedMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次自动开启时间（毫秒）',"
                + "`lastAutoSettledMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次自动结算时间（毫秒）',"
                + "KEY `guildId` (`guildId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟副本全局配置数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//autoHour
        _size+=4;//autoMin
        _size+=8;//lastAutoStartedMs
        _size+=8;//lastAutoSettledMs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guildId);
        buff.putInt(autoHour);
        buff.putInt(autoMin);
        buff.putLong(lastAutoStartedMs);
        buff.putLong(lastAutoSettledMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guildId=buff.getLong();
        autoHour=buff.getInt();
        autoMin=buff.getInt();
        lastAutoStartedMs=buff.getLong();
        lastAutoSettledMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
