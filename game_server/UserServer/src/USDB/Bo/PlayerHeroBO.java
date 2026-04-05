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
public class PlayerHeroBO extends BaseBO {
    
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

    public static final int FIELD_level =3;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public static final int FIELD_step =4;
    @DataBaseField(type = "int(11)", fieldname = "step", comment = "进阶的阶级")
    private int step;

    public static final int FIELD_star =5;
    @DataBaseField(type = "int(11)", fieldname = "star", comment = "觉醒星级")
    private int star;

    public static final int FIELD_building_id =6;
    @DataBaseField(type = "bigint(20)", fieldname = "building_id", comment = "大臣所在建筑Id")
    private long building_id;

    public static final int FIELD_serial =7;
    @DataBaseField(type = "bigint(20)", fieldname = "serial", comment = "放置序列号")
    private long serial;

    public static final int FIELD_ext_add_power =8;
    @DataBaseField(type = "bigint(20)", fieldname = "ext_add_power", comment = "道具额外加成实力")
    private long ext_add_power;

    public static final int FIELD_arena_add_power =9;
    @DataBaseField(type = "bigint(20)", fieldname = "arena_add_power", comment = "竞技场额外加成实力")
    private long arena_add_power;

    public static final int FIELD_travel_add_power =10;
    @DataBaseField(type = "bigint(20)", fieldname = "travel_add_power", comment = "游历额外加成实力")
    private long travel_add_power;

    public PlayerHeroBO() {
        id = 0;
        cid = 0L;
        hero_id = 0L;
        skin_id = 0L;
        level = 0;
        step = 0;
        star = 0;
        building_id = 0L;
        serial = 0L;
        ext_add_power = 0L;
        arena_add_power = 0L;
        travel_add_power = 0L;
    }

    public PlayerHeroBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        hero_id = rs.getLong(3);
        skin_id = rs.getLong(4);
        level = rs.getInt(5);
        step = rs.getInt(6);
        star = rs.getInt(7);
        building_id = rs.getLong(8);
        serial = rs.getLong(9);
        ext_add_power = rs.getLong(10);
        arena_add_power = rs.getLong(11);
        travel_add_power = rs.getLong(12);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerHeroBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `hero_id`, `skin_id`, `level`, `step`, `star`, `building_id`, `serial`, `ext_add_power`, `arena_add_power`, `travel_add_power`";
    }

    @Override
    public String getTableName() {
        return "`player_hero`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(hero_id).append("', ");
        strBuf.append("'").append(skin_id).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(step).append("', ");
        strBuf.append("'").append(star).append("', ");
        strBuf.append("'").append(building_id).append("', ");
        strBuf.append("'").append(serial).append("', ");
        strBuf.append("'").append(ext_add_power).append("', ");
        strBuf.append("'").append(arena_add_power).append("', ");
        strBuf.append("'").append(travel_add_power).append("', ");
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

    // 进阶的阶级
    public int getStep() { return this.step; }
    public void setStep(BM _bm, int step) {
        if(step==this.step) 
            return;
        this.step = step; 
        markField(_bm, FIELD_step); 
    }
    public void saveStep(BM _bm, int step) {
        if(step==this.step) 
            return;
        this.step = step;
        saveField(_bm, "step", step);
    }

    // 觉醒星级
    public int getStar() { return this.star; }
    public void setStar(BM _bm, int star) {
        if(star==this.star) 
            return;
        this.star = star; 
        markField(_bm, FIELD_star); 
    }
    public void saveStar(BM _bm, int star) {
        if(star==this.star) 
            return;
        this.star = star;
        saveField(_bm, "star", star);
    }

    // 大臣所在建筑Id
    public long getBuildingId() { return this.building_id; }
    public void setBuildingId(BM _bm, long building_id) {
        if(building_id==this.building_id) 
            return;
        this.building_id = building_id; 
        markField(_bm, FIELD_building_id); 
    }
    public void saveBuildingId(BM _bm, long building_id) {
        if(building_id==this.building_id) 
            return;
        this.building_id = building_id;
        saveField(_bm, "building_id", building_id);
    }

    // 放置序列号
    public long getSerial() { return this.serial; }
    public void setSerial(BM _bm, long serial) {
        if(serial==this.serial) 
            return;
        this.serial = serial; 
        markField(_bm, FIELD_serial); 
    }
    public void saveSerial(BM _bm, long serial) {
        if(serial==this.serial) 
            return;
        this.serial = serial;
        saveField(_bm, "serial", serial);
    }

    // 道具额外加成实力
    public long getExtAddPower() { return this.ext_add_power; }
    public void setExtAddPower(BM _bm, long ext_add_power) {
        if(ext_add_power==this.ext_add_power) 
            return;
        this.ext_add_power = ext_add_power; 
        markField(_bm, FIELD_ext_add_power); 
    }
    public void saveExtAddPower(BM _bm, long ext_add_power) {
        if(ext_add_power==this.ext_add_power) 
            return;
        this.ext_add_power = ext_add_power;
        saveField(_bm, "ext_add_power", ext_add_power);
    }

    // 竞技场额外加成实力
    public long getArenaAddPower() { return this.arena_add_power; }
    public void setArenaAddPower(BM _bm, long arena_add_power) {
        if(arena_add_power==this.arena_add_power) 
            return;
        this.arena_add_power = arena_add_power; 
        markField(_bm, FIELD_arena_add_power); 
    }
    public void saveArenaAddPower(BM _bm, long arena_add_power) {
        if(arena_add_power==this.arena_add_power) 
            return;
        this.arena_add_power = arena_add_power;
        saveField(_bm, "arena_add_power", arena_add_power);
    }

    // 游历额外加成实力
    public long getTravelAddPower() { return this.travel_add_power; }
    public void setTravelAddPower(BM _bm, long travel_add_power) {
        if(travel_add_power==this.travel_add_power) 
            return;
        this.travel_add_power = travel_add_power; 
        markField(_bm, FIELD_travel_add_power); 
    }
    public void saveTravelAddPower(BM _bm, long travel_add_power) {
        if(travel_add_power==this.travel_add_power) 
            return;
        this.travel_add_power = travel_add_power;
        saveField(_bm, "travel_add_power", travel_add_power);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        sBuilder.append(" `skin_id` = '").append(skin_id).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `step` = '").append(step).append("',");
        sBuilder.append(" `star` = '").append(star).append("',");
        sBuilder.append(" `building_id` = '").append(building_id).append("',");
        sBuilder.append(" `serial` = '").append(serial).append("',");
        sBuilder.append(" `ext_add_power` = '").append(ext_add_power).append("',");
        sBuilder.append(" `arena_add_power` = '").append(arena_add_power).append("',");
        sBuilder.append(" `travel_add_power` = '").append(travel_add_power).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_hero_id)) sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        if(isFieldMarked(FIELD_skin_id)) sBuilder.append(" `skin_id` = '").append(skin_id).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_step)) sBuilder.append(" `step` = '").append(step).append("',");
        if(isFieldMarked(FIELD_star)) sBuilder.append(" `star` = '").append(star).append("',");
        if(isFieldMarked(FIELD_building_id)) sBuilder.append(" `building_id` = '").append(building_id).append("',");
        if(isFieldMarked(FIELD_serial)) sBuilder.append(" `serial` = '").append(serial).append("',");
        if(isFieldMarked(FIELD_ext_add_power)) sBuilder.append(" `ext_add_power` = '").append(ext_add_power).append("',");
        if(isFieldMarked(FIELD_arena_add_power)) sBuilder.append(" `arena_add_power` = '").append(arena_add_power).append("',");
        if(isFieldMarked(FIELD_travel_add_power)) sBuilder.append(" `travel_add_power` = '").append(travel_add_power).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_hero` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`hero_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '大臣id',"
                + "`skin_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '皮肤id',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`step` int(11) NOT NULL DEFAULT '0' COMMENT '进阶的阶级',"
                + "`star` int(11) NOT NULL DEFAULT '0' COMMENT '觉醒星级',"
                + "`building_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '大臣所在建筑Id',"
                + "`serial` bigint(20) NOT NULL DEFAULT '0' COMMENT '放置序列号',"
                + "`ext_add_power` bigint(20) NOT NULL DEFAULT '0' COMMENT '道具额外加成实力',"
                + "`arena_add_power` bigint(20) NOT NULL DEFAULT '0' COMMENT '竞技场额外加成实力',"
                + "`travel_add_power` bigint(20) NOT NULL DEFAULT '0' COMMENT '游历额外加成实力',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家大臣数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//level
        _size+=4;//step
        _size+=4;//star
        _size+=8;//building_id
        _size+=8;//serial
        _size+=8;//ext_add_power
        _size+=8;//arena_add_power
        _size+=8;//travel_add_power
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
        buff.putInt(level);
        buff.putInt(step);
        buff.putInt(star);
        buff.putLong(building_id);
        buff.putLong(serial);
        buff.putLong(ext_add_power);
        buff.putLong(arena_add_power);
        buff.putLong(travel_add_power);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        hero_id=buff.getLong();
        skin_id=buff.getLong();
        level=buff.getInt();
        step=buff.getInt();
        star=buff.getInt();
        building_id=buff.getLong();
        serial=buff.getLong();
        ext_add_power=buff.getLong();
        arena_add_power=buff.getLong();
        travel_add_power=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
