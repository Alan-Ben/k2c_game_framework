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
public class GuildHeroDispatchBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟id")
    private long guild_id;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家")
    private long cid;

    public static final int FIELD_hero_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "hero_id", comment = "大臣id")
    private long hero_id;

    public static final int FIELD_add_value =3;
    @DataBaseField(type = "int(11)", fieldname = "add_value", comment = "加成值")
    private int add_value;

    public static final int FIELD_level =4;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public static final int FIELD_power =5;
    @DataBaseField(type = "bigint(20)", fieldname = "power", comment = "实力")
    private long power;

    public static final int FIELD_skin_id =6;
    @DataBaseField(type = "bigint(20)", fieldname = "skin_id", comment = "皮肤id")
    private long skin_id;

    public GuildHeroDispatchBO() {
        id = 0;
        guild_id = 0L;
        cid = 0L;
        hero_id = 0L;
        add_value = 0;
        level = 0;
        power = 0L;
        skin_id = 0L;
    }

    public GuildHeroDispatchBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        cid = rs.getLong(3);
        hero_id = rs.getLong(4);
        add_value = rs.getInt(5);
        level = rs.getInt(6);
        power = rs.getLong(7);
        skin_id = rs.getLong(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildHeroDispatchBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `cid`, `hero_id`, `add_value`, `level`, `power`, `skin_id`";
    }

    @Override
    public String getTableName() {
        return "`guild_hero_dispatch`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(hero_id).append("', ");
        strBuf.append("'").append(add_value).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(power).append("', ");
        strBuf.append("'").append(skin_id).append("', ");
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

    // 联盟id
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

    // 玩家
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

    // 大臣id
    public long getHeroId() { return this.hero_id; }
    public void setHeroId(BM _bm, long hero_id) {
        if(hero_id==this.hero_id) 
            return;
        this.hero_id = hero_id; 
        markField(_bm, FIELD_hero_id); 
    }
    public void saveHeroId(BM _bm, long hero_id) {
        if(hero_id==this.hero_id) 
            return;
        this.hero_id = hero_id;
        saveField(_bm, "hero_id", hero_id);
    }

    // 加成值
    public int getAddValue() { return this.add_value; }
    public void setAddValue(BM _bm, int add_value) {
        if(add_value==this.add_value) 
            return;
        this.add_value = add_value; 
        markField(_bm, FIELD_add_value); 
    }
    public void saveAddValue(BM _bm, int add_value) {
        if(add_value==this.add_value) 
            return;
        this.add_value = add_value;
        saveField(_bm, "add_value", add_value);
    }

    // 等级
    public int getLevel() { return this.level; }
    public void setLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level; 
        markField(_bm, FIELD_level); 
    }
    public void saveLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level;
        saveField(_bm, "level", level);
    }

    // 实力
    public long getPower() { return this.power; }
    public void setPower(BM _bm, long power) {
        if(power==this.power) 
            return;
        this.power = power; 
        markField(_bm, FIELD_power); 
    }
    public void savePower(BM _bm, long power) {
        if(power==this.power) 
            return;
        this.power = power;
        saveField(_bm, "power", power);
    }

    // 皮肤id
    public long getSkinId() { return this.skin_id; }
    public void setSkinId(BM _bm, long skin_id) {
        if(skin_id==this.skin_id) 
            return;
        this.skin_id = skin_id; 
        markField(_bm, FIELD_skin_id); 
    }
    public void saveSkinId(BM _bm, long skin_id) {
        if(skin_id==this.skin_id) 
            return;
        this.skin_id = skin_id;
        saveField(_bm, "skin_id", skin_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        sBuilder.append(" `add_value` = '").append(add_value).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `power` = '").append(power).append("',");
        sBuilder.append(" `skin_id` = '").append(skin_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_hero_id)) sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        if(isFieldMarked(FIELD_add_value)) sBuilder.append(" `add_value` = '").append(add_value).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_power)) sBuilder.append(" `power` = '").append(power).append("',");
        if(isFieldMarked(FIELD_skin_id)) sBuilder.append(" `skin_id` = '").append(skin_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_hero_dispatch` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家',"
                + "`hero_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '大臣id',"
                + "`add_value` int(11) NOT NULL DEFAULT '0' COMMENT '加成值',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`power` bigint(20) NOT NULL DEFAULT '0' COMMENT '实力',"
                + "`skin_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '皮肤id',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟大臣派遣' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//hero_id
        _size+=4;//add_value
        _size+=4;//level
        _size+=8;//power
        _size+=8;//skin_id
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
        buff.putLong(hero_id);
        buff.putInt(add_value);
        buff.putInt(level);
        buff.putLong(power);
        buff.putLong(skin_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        cid=buff.getLong();
        hero_id=buff.getLong();
        add_value=buff.getInt();
        level=buff.getInt();
        power=buff.getLong();
        skin_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
