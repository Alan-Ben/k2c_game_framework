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
public class PlayerEquipBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_equip_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "equip_id", comment = "藏品id")
    private long equip_id;

    public static final int FIELD_level =2;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public static final int FIELD_awaken_level =3;
    @DataBaseField(type = "int(11)", fieldname = "awaken_level", comment = "觉醒星级")
    private int awaken_level;

    public static final int FIELD_wear_hero_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "wear_hero_id", comment = "佩戴大臣id")
    private long wear_hero_id;

    public static final int FIELD_is_locked =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_locked", comment = "是否锁定")
    private boolean is_locked;

    public PlayerEquipBO() {
        id = 0;
        cid = 0L;
        equip_id = 0L;
        level = 0;
        awaken_level = 0;
        wear_hero_id = 0L;
        is_locked = false;
    }

    public PlayerEquipBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        equip_id = rs.getLong(3);
        level = rs.getInt(4);
        awaken_level = rs.getInt(5);
        wear_hero_id = rs.getLong(6);
        is_locked = rs.getBoolean(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerEquipBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `equip_id`, `level`, `awaken_level`, `wear_hero_id`, `is_locked`";
    }

    @Override
    public String getTableName() {
        return "`player_equip`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(equip_id).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(awaken_level).append("', ");
        strBuf.append("'").append(wear_hero_id).append("', ");
        strBuf.append("'").append(is_locked ? 1 : 0).append("', ");
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

    // 藏品id
    public long getEquipId() { return this.equip_id; }
    public void setEquipId(BM _bm, long equip_id) {
        if(equip_id==this.equip_id) 
            return;
        this.equip_id = equip_id; 
        markField(_bm, FIELD_equip_id); 
    }
    public void saveEquipId(BM _bm, long equip_id) {
        if(equip_id==this.equip_id) 
            return;
        this.equip_id = equip_id;
        saveField(_bm, "equip_id", equip_id);
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

    // 觉醒星级
    public int getAwakenLevel() { return this.awaken_level; }
    public void setAwakenLevel(BM _bm, int awaken_level) {
        if(awaken_level==this.awaken_level) 
            return;
        this.awaken_level = awaken_level; 
        markField(_bm, FIELD_awaken_level); 
    }
    public void saveAwakenLevel(BM _bm, int awaken_level) {
        if(awaken_level==this.awaken_level) 
            return;
        this.awaken_level = awaken_level;
        saveField(_bm, "awaken_level", awaken_level);
    }

    // 佩戴大臣id
    public long getWearHeroId() { return this.wear_hero_id; }
    public void setWearHeroId(BM _bm, long wear_hero_id) {
        if(wear_hero_id==this.wear_hero_id) 
            return;
        this.wear_hero_id = wear_hero_id; 
        markField(_bm, FIELD_wear_hero_id); 
    }
    public void saveWearHeroId(BM _bm, long wear_hero_id) {
        if(wear_hero_id==this.wear_hero_id) 
            return;
        this.wear_hero_id = wear_hero_id;
        saveField(_bm, "wear_hero_id", wear_hero_id);
    }

    // 是否锁定
    public boolean getIsLocked() { return this.is_locked; }
    public void setIsLocked(BM _bm, boolean is_locked) {
        if(is_locked==this.is_locked) 
            return;
        this.is_locked = is_locked; 
        markField(_bm, FIELD_is_locked); 
    }
    public void saveIsLocked(BM _bm, boolean is_locked) {
        if(is_locked==this.is_locked) 
            return;
        this.is_locked = is_locked;
        saveField(_bm, "is_locked", is_locked ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `equip_id` = '").append(equip_id).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `awaken_level` = '").append(awaken_level).append("',");
        sBuilder.append(" `wear_hero_id` = '").append(wear_hero_id).append("',");
        sBuilder.append(" `is_locked` = '").append(is_locked ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_equip_id)) sBuilder.append(" `equip_id` = '").append(equip_id).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_awaken_level)) sBuilder.append(" `awaken_level` = '").append(awaken_level).append("',");
        if(isFieldMarked(FIELD_wear_hero_id)) sBuilder.append(" `wear_hero_id` = '").append(wear_hero_id).append("',");
        if(isFieldMarked(FIELD_is_locked)) sBuilder.append(" `is_locked` = '").append(is_locked ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_equip` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`equip_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '藏品id',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`awaken_level` int(11) NOT NULL DEFAULT '0' COMMENT '觉醒星级',"
                + "`wear_hero_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '佩戴大臣id',"
                + "`is_locked` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否锁定',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家藏品数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//equip_id
        _size+=4;//level
        _size+=4;//awaken_level
        _size+=8;//wear_hero_id
        _size+=1;//is_locked
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(equip_id);
        buff.putInt(level);
        buff.putInt(awaken_level);
        buff.putLong(wear_hero_id);
        buff.put((byte)(is_locked?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        equip_id=buff.getLong();
        level=buff.getInt();
        awaken_level=buff.getInt();
        wear_hero_id=buff.getLong();
        is_locked=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
