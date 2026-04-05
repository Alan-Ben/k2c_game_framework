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
public class GuildDungeonLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guildId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guildId", comment = "联盟ID")
    private long guildId;

    public static final int FIELD_instaceId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "instaceId", comment = "联盟副本实例ID")
    private long instaceId;

    public static final int FIELD_logType =2;
    @DataBaseField(type = "int(11)", fieldname = "logType", comment = "日志类型")
    private int logType;

    public static final int FIELD_createdAt =3;
    @DataBaseField(type = "int(11)", fieldname = "createdAt", comment = "创建时间")
    private int createdAt;

    public static final int FIELD_info =4;
    @DataBaseField(type = "blob", fieldname = "info", comment = "日志数据")
    private byte[] info;

    public GuildDungeonLogBO() {
        id = 0;
        guildId = 0L;
        instaceId = 0L;
        logType = 0;
        createdAt = 0;
        info = null;
    }

    public GuildDungeonLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guildId = rs.getLong(2);
        instaceId = rs.getLong(3);
        logType = rs.getInt(4);
        createdAt = rs.getInt(5);
        info = rs.getBytes(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildDungeonLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guildId`, `instaceId`, `logType`, `createdAt`, `info`";
    }

    @Override
    public String getTableName() {
        return "`guild_dungeon_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guildId).append("', ");
        strBuf.append("'").append(instaceId).append("', ");
        strBuf.append("'").append(logType).append("', ");
        strBuf.append("'").append(createdAt).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(info);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_info)) ret.add(info);         return ret;
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

    // 联盟副本实例ID
    public long getInstaceId() { return this.instaceId; }
    public void setInstaceId(BM _bm, long instaceId) {
        if(instaceId==this.instaceId) 
            return;
        this.instaceId = instaceId; 
        markField(_bm, FIELD_instaceId); 
    }
    public void saveInstaceId(BM _bm, long instaceId) {
        if(instaceId==this.instaceId) 
            return;
        this.instaceId = instaceId;
        saveField(_bm, "instaceId", instaceId);
    }

    // 日志类型
    public int getLogType() { return this.logType; }
    public void setLogType(BM _bm, int logType) {
        if(logType==this.logType) 
            return;
        this.logType = logType; 
        markField(_bm, FIELD_logType); 
    }
    public void saveLogType(BM _bm, int logType) {
        if(logType==this.logType) 
            return;
        this.logType = logType;
        saveField(_bm, "logType", logType);
    }

    // 创建时间
    public int getCreatedAt() { return this.createdAt; }
    public void setCreatedAt(BM _bm, int createdAt) {
        if(createdAt==this.createdAt) 
            return;
        this.createdAt = createdAt; 
        markField(_bm, FIELD_createdAt); 
    }
    public void saveCreatedAt(BM _bm, int createdAt) {
        if(createdAt==this.createdAt) 
            return;
        this.createdAt = createdAt;
        saveField(_bm, "createdAt", createdAt);
    }

    // 日志数据
    public byte[] getInfo() { return this.info; }
    public void setInfo(BM _bm, byte[] info) {
        if(info==this.info) 
            return;
        this.info = info; 
        markField(_bm, FIELD_info); 
    }
    public void saveInfo(BM _bm, byte[] info) {
        if(info==this.info) 
            return;
        this.info = info;
        saveFieldBytes(_bm, "info", info);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guildId` = '").append(guildId).append("',");
        sBuilder.append(" `instaceId` = '").append(instaceId).append("',");
        sBuilder.append(" `logType` = '").append(logType).append("',");
        sBuilder.append(" `createdAt` = '").append(createdAt).append("',");
        sBuilder.append(" `info` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guildId)) sBuilder.append(" `guildId` = '").append(guildId).append("',");
        if(isFieldMarked(FIELD_instaceId)) sBuilder.append(" `instaceId` = '").append(instaceId).append("',");
        if(isFieldMarked(FIELD_logType)) sBuilder.append(" `logType` = '").append(logType).append("',");
        if(isFieldMarked(FIELD_createdAt)) sBuilder.append(" `createdAt` = '").append(createdAt).append("',");
        if(isFieldMarked(FIELD_info)) sBuilder.append(" `info` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_dungeon_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guildId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`instaceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟副本实例ID',"
                + "`logType` int(11) NOT NULL DEFAULT '0' COMMENT '日志类型',"
                + "`createdAt` int(11) NOT NULL DEFAULT '0' COMMENT '创建时间',"
                + "`info` blob NULL COMMENT '日志数据',"
                + "KEY `guildId` (`guildId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟副本数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//instaceId
        _size+=4;//logType
        _size+=4;//createdAt
        _size+=2;_size+=info.length;//info
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guildId);
        buff.putLong(instaceId);
        buff.putInt(logType);
        buff.putInt(createdAt);
        buff.putShort((short)(info == null ? 0 : info.length));if(null != info){buff.put(info);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guildId=buff.getLong();
        instaceId=buff.getLong();
        logType=buff.getInt();
        createdAt=buff.getInt();
        int info_count = buff.getShort();if(info_count>0){info = new byte[info_count];buff.get(info);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
