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
public class PlayerHeroCacheBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_hero_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "hero_id", comment = "大臣id")
    private long hero_id;

    public static final int FIELD_skin_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "skin_id", comment = "皮肤id")
    private long skin_id;

    public static final int FIELD_power =3;
    @DataBaseField(type = "bigint(20)", fieldname = "power", comment = "实力")
    private long power;

    public static final int FIELD_level =4;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public PlayerHeroCacheBO() {
        id = 0;
        cid = 0L;
        hero_id = 0L;
        skin_id = 0L;
        power = 0L;
        level = 0;
    }

    public PlayerHeroCacheBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        hero_id = rs.getLong(3);
        skin_id = rs.getLong(4);
        power = rs.getLong(5);
        level = rs.getInt(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerHeroCacheBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `hero_id`, `skin_id`, `power`, `level`";
    }

    @Override
    public String getTableName() {
        return "`player_hero_cache`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(hero_id).append("', ");
        strBuf.append("'").append(skin_id).append("', ");
        strBuf.append("'").append(power).append("', ");
        strBuf.append("'").append(level).append("', ");
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        sBuilder.append(" `skin_id` = '").append(skin_id).append("',");
        sBuilder.append(" `power` = '").append(power).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_hero_id)) sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        if(isFieldMarked(FIELD_skin_id)) sBuilder.append(" `skin_id` = '").append(skin_id).append("',");
        if(isFieldMarked(FIELD_power)) sBuilder.append(" `power` = '").append(power).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_hero_cache` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`hero_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '大臣id',"
                + "`skin_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '皮肤id',"
                + "`power` bigint(20) NOT NULL DEFAULT '0' COMMENT '实力',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家大臣缓存数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//hero_id
        _size+=8;//skin_id
        _size+=8;//power
        _size+=4;//level
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(hero_id);
        buff.putLong(skin_id);
        buff.putLong(power);
        buff.putInt(level);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        hero_id=buff.getLong();
        skin_id=buff.getLong();
        power=buff.getLong();
        level=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
