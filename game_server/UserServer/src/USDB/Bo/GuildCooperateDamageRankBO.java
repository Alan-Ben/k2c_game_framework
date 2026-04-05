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
public class GuildCooperateDamageRankBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "公会ID")
    private long guild_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_total_damage =2;
    @DataBaseField(type = "bigint(20)", fieldname = "total_damage", comment = "累计总伤害")
    private long total_damage;

    public static final int FIELD_last_update_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "last_update_time_ms", comment = "最后更新时间")
    private long last_update_time_ms;

    public GuildCooperateDamageRankBO() {
        id = 0;
        guild_id = 0L;
        cid = 0L;
        total_damage = 0L;
        last_update_time_ms = 0L;
    }

    public GuildCooperateDamageRankBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        cid = rs.getLong(3);
        total_damage = rs.getLong(4);
        last_update_time_ms = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildCooperateDamageRankBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `cid`, `total_damage`, `last_update_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`guild_cooperate_damage_rank`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(total_damage).append("', ");
        strBuf.append("'").append(last_update_time_ms).append("', ");
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

    // 公会ID
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
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

    // 累计总伤害
    public long getTotalDamage() { return this.total_damage; }
    public void setTotalDamage(BM _bm, long total_damage) {
        if(total_damage==this.total_damage) 
            return;
        this.total_damage = total_damage; 
        markField(_bm, FIELD_total_damage); 
    }
    public void saveTotalDamage(BM _bm, long total_damage) {
        if(total_damage==this.total_damage) 
            return;
        this.total_damage = total_damage;
        saveField(_bm, "total_damage", total_damage);
    }

    // 最后更新时间
    public long getLastUpdateTimeMs() { return this.last_update_time_ms; }
    public void setLastUpdateTimeMs(BM _bm, long last_update_time_ms) {
        if(last_update_time_ms==this.last_update_time_ms) 
            return;
        this.last_update_time_ms = last_update_time_ms; 
        markField(_bm, FIELD_last_update_time_ms); 
    }
    public void saveLastUpdateTimeMs(BM _bm, long last_update_time_ms) {
        if(last_update_time_ms==this.last_update_time_ms) 
            return;
        this.last_update_time_ms = last_update_time_ms;
        saveField(_bm, "last_update_time_ms", last_update_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `total_damage` = '").append(total_damage).append("',");
        sBuilder.append(" `last_update_time_ms` = '").append(last_update_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_total_damage)) sBuilder.append(" `total_damage` = '").append(total_damage).append("',");
        if(isFieldMarked(FIELD_last_update_time_ms)) sBuilder.append(" `last_update_time_ms` = '").append(last_update_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_cooperate_damage_rank` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '公会ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`total_damage` bigint(20) NOT NULL DEFAULT '0' COMMENT '累计总伤害',"
                + "`last_update_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '最后更新时间',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟协作伤害排行榜' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guild_id
        _size+=8;//cid
        _size+=8;//total_damage
        _size+=8;//last_update_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(cid);
        buff.putLong(total_damage);
        buff.putLong(last_update_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        cid=buff.getLong();
        total_damage=buff.getLong();
        last_update_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
