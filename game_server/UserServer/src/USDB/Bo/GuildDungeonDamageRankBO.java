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
public class GuildDungeonDamageRankBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guildId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guildId", comment = "联盟ID")
    private long guildId;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_value =2;
    @DataBaseField(type = "bigint(20)", fieldname = "value", comment = "玩家数值")
    private long value;

    public static final int FIELD_updatedMs =3;
    @DataBaseField(type = "bigint(20)", fieldname = "updatedMs", comment = "最后一次修改时间")
    private long updatedMs;

    public GuildDungeonDamageRankBO() {
        id = 0;
        guildId = 0L;
        cid = 0L;
        value = 0L;
        updatedMs = 0L;
    }

    public GuildDungeonDamageRankBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guildId = rs.getLong(2);
        cid = rs.getLong(3);
        value = rs.getLong(4);
        updatedMs = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildDungeonDamageRankBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guildId`, `cid`, `value`, `updatedMs`";
    }

    @Override
    public String getTableName() {
        return "`guild_dungeon_damage_rank`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guildId).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(value).append("', ");
        strBuf.append("'").append(updatedMs).append("', ");
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

    // 玩家数值
    public long getValue() { return this.value; }
    public void setValue(BM _bm, long value) {
        if(value==this.value) 
            return;
        this.value = value; 
        markField(_bm, FIELD_value); 
    }
    public void saveValue(BM _bm, long value) {
        if(value==this.value) 
            return;
        this.value = value;
        saveField(_bm, "value", value);
    }

    // 最后一次修改时间
    public long getUpdatedMs() { return this.updatedMs; }
    public void setUpdatedMs(BM _bm, long updatedMs) {
        if(updatedMs==this.updatedMs) 
            return;
        this.updatedMs = updatedMs; 
        markField(_bm, FIELD_updatedMs); 
    }
    public void saveUpdatedMs(BM _bm, long updatedMs) {
        if(updatedMs==this.updatedMs) 
            return;
        this.updatedMs = updatedMs;
        saveField(_bm, "updatedMs", updatedMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guildId` = '").append(guildId).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `value` = '").append(value).append("',");
        sBuilder.append(" `updatedMs` = '").append(updatedMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guildId)) sBuilder.append(" `guildId` = '").append(guildId).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_value)) sBuilder.append(" `value` = '").append(value).append("',");
        if(isFieldMarked(FIELD_updatedMs)) sBuilder.append(" `updatedMs` = '").append(updatedMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_dungeon_damage_rank` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guildId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`value` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家数值',"
                + "`updatedMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后一次修改时间',"
                + "KEY `guildId` (`guildId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟副本伤害排行数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=8;//value
        _size+=8;//updatedMs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guildId);
        buff.putLong(cid);
        buff.putLong(value);
        buff.putLong(updatedMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guildId=buff.getLong();
        cid=buff.getLong();
        value=buff.getLong();
        updatedMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
