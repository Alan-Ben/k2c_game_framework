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
public class PlayerHeroTalentSkillBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_hero_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "hero_id", comment = "大臣id")
    private long hero_id;

    public static final int FIELD_talent_skill_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "talent_skill_id", comment = "资质技能id")
    private long talent_skill_id;

    public static final int FIELD_level =3;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "经验")
    private int level;

    public static final int FIELD_fail_count =4;
    @DataBaseField(type = "int(11)", fieldname = "fail_count", comment = "失败次数")
    private int fail_count;

    public PlayerHeroTalentSkillBO() {
        id = 0;
        cid = 0L;
        hero_id = 0L;
        talent_skill_id = 0L;
        level = 0;
        fail_count = 0;
    }

    public PlayerHeroTalentSkillBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        hero_id = rs.getLong(3);
        talent_skill_id = rs.getLong(4);
        level = rs.getInt(5);
        fail_count = rs.getInt(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerHeroTalentSkillBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `hero_id`, `talent_skill_id`, `level`, `fail_count`";
    }

    @Override
    public String getTableName() {
        return "`player_hero_talent_skill`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(hero_id).append("', ");
        strBuf.append("'").append(talent_skill_id).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(fail_count).append("', ");
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

    // 资质技能id
    public long getTalentSkillId() { return this.talent_skill_id; }
    public void setTalentSkillId(BM _bm, long talent_skill_id) {
        if(talent_skill_id==this.talent_skill_id) 
            return;
        this.talent_skill_id = talent_skill_id; 
        markField(_bm, FIELD_talent_skill_id); 
    }
    public void saveTalentSkillId(BM _bm, long talent_skill_id) {
        if(talent_skill_id==this.talent_skill_id) 
            return;
        this.talent_skill_id = talent_skill_id;
        saveField(_bm, "talent_skill_id", talent_skill_id);
    }

    // 经验
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

    // 失败次数
    public int getFailCount() { return this.fail_count; }
    public void setFailCount(BM _bm, int fail_count) {
        if(fail_count==this.fail_count) 
            return;
        this.fail_count = fail_count; 
        markField(_bm, FIELD_fail_count); 
    }
    public void saveFailCount(BM _bm, int fail_count) {
        if(fail_count==this.fail_count) 
            return;
        this.fail_count = fail_count;
        saveField(_bm, "fail_count", fail_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        sBuilder.append(" `talent_skill_id` = '").append(talent_skill_id).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `fail_count` = '").append(fail_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_hero_id)) sBuilder.append(" `hero_id` = '").append(hero_id).append("',");
        if(isFieldMarked(FIELD_talent_skill_id)) sBuilder.append(" `talent_skill_id` = '").append(talent_skill_id).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_fail_count)) sBuilder.append(" `fail_count` = '").append(fail_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_hero_talent_skill` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`hero_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '大臣id',"
                + "`talent_skill_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '资质技能id',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '经验',"
                + "`fail_count` int(11) NOT NULL DEFAULT '0' COMMENT '失败次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家大臣资质技能数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//talent_skill_id
        _size+=4;//level
        _size+=4;//fail_count
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
        buff.putLong(talent_skill_id);
        buff.putInt(level);
        buff.putInt(fail_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        hero_id=buff.getLong();
        talent_skill_id=buff.getLong();
        level=buff.getInt();
        fail_count=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
