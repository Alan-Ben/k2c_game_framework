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
public class PlayerMarsTechBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_techId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "techId", comment = "科技ID")
    private long techId;

    public static final int FIELD_lvl =2;
    @DataBaseField(type = "int(11)", fieldname = "lvl", comment = "科技等级")
    private int lvl;

    public static final int FIELD_isUpgrading =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "isUpgrading", comment = "是否升级中")
    private boolean isUpgrading;

    public static final int FIELD_startUpgradeLvlMs =4;
    @DataBaseField(type = "bigint(20)", fieldname = "startUpgradeLvlMs", comment = "建筑开始升级时间（毫秒）")
    private long startUpgradeLvlMs;

    public static final int FIELD_endUpgradeLvlMs =5;
    @DataBaseField(type = "bigint(20)", fieldname = "endUpgradeLvlMs", comment = "建筑结束升级时间（毫秒）")
    private long endUpgradeLvlMs;

    public static final int FIELD_upgradeCost =6;
    @DataBaseField(type = "blob", fieldname = "upgradeCost", comment = "升级消耗（玩家取消时返还）")
    private byte[] upgradeCost;

    public static final int FIELD_guildHelpType =7;
    @DataBaseField(type = "int(11)", fieldname = "guildHelpType", comment = "公会求助类型")
    private int guildHelpType;

    public static final int FIELD_guildHelpId =8;
    @DataBaseField(type = "bigint(20)", fieldname = "guildHelpId", comment = "公会求助ID")
    private long guildHelpId;

    public static final int FIELD_guildHelpSecs =9;
    @DataBaseField(type = "int(11)", fieldname = "guildHelpSecs", comment = "公会助力时间（秒）")
    private int guildHelpSecs;

    public static final int FIELD_itemHelpSecs =10;
    @DataBaseField(type = "int(11)", fieldname = "itemHelpSecs", comment = "加速道具助力时间（秒）")
    private int itemHelpSecs;

    public PlayerMarsTechBO() {
        id = 0;
        cid = 0L;
        techId = 0L;
        lvl = 0;
        isUpgrading = false;
        startUpgradeLvlMs = 0L;
        endUpgradeLvlMs = 0L;
        upgradeCost = null;
        guildHelpType = 0;
        guildHelpId = 0L;
        guildHelpSecs = 0;
        itemHelpSecs = 0;
    }

    public PlayerMarsTechBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        techId = rs.getLong(3);
        lvl = rs.getInt(4);
        isUpgrading = rs.getBoolean(5);
        startUpgradeLvlMs = rs.getLong(6);
        endUpgradeLvlMs = rs.getLong(7);
        upgradeCost = rs.getBytes(8);
        guildHelpType = rs.getInt(9);
        guildHelpId = rs.getLong(10);
        guildHelpSecs = rs.getInt(11);
        itemHelpSecs = rs.getInt(12);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsTechBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `techId`, `lvl`, `isUpgrading`, `startUpgradeLvlMs`, `endUpgradeLvlMs`, `upgradeCost`, `guildHelpType`, `guildHelpId`, `guildHelpSecs`, `itemHelpSecs`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_tech`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(techId).append("', ");
        strBuf.append("'").append(lvl).append("', ");
        strBuf.append("'").append(isUpgrading ? 1 : 0).append("', ");
        strBuf.append("'").append(startUpgradeLvlMs).append("', ");
        strBuf.append("'").append(endUpgradeLvlMs).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(guildHelpType).append("', ");
        strBuf.append("'").append(guildHelpId).append("', ");
        strBuf.append("'").append(guildHelpSecs).append("', ");
        strBuf.append("'").append(itemHelpSecs).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(upgradeCost);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_upgradeCost)) ret.add(upgradeCost);         return ret;
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

    // 科技ID
    public long getTechId() { return this.techId; }
    public void setTechId(BM _bm, long techId) {
        if(techId==this.techId) 
            return;
        this.techId = techId; 
        markField(_bm, FIELD_techId); 
    }
    public void saveTechId(BM _bm, long techId) {
        if(techId==this.techId) 
            return;
        this.techId = techId;
        saveField(_bm, "techId", techId);
    }

    // 科技等级
    public int getLvl() { return this.lvl; }
    public void setLvl(BM _bm, int lvl) {
        if(lvl==this.lvl) 
            return;
        this.lvl = lvl; 
        markField(_bm, FIELD_lvl); 
    }
    public void saveLvl(BM _bm, int lvl) {
        if(lvl==this.lvl) 
            return;
        this.lvl = lvl;
        saveField(_bm, "lvl", lvl);
    }

    // 是否升级中
    public boolean getIsUpgrading() { return this.isUpgrading; }
    public void setIsUpgrading(BM _bm, boolean isUpgrading) {
        if(isUpgrading==this.isUpgrading) 
            return;
        this.isUpgrading = isUpgrading; 
        markField(_bm, FIELD_isUpgrading); 
    }
    public void saveIsUpgrading(BM _bm, boolean isUpgrading) {
        if(isUpgrading==this.isUpgrading) 
            return;
        this.isUpgrading = isUpgrading;
        saveField(_bm, "isUpgrading", isUpgrading ? 1 : 0);
    }

    // 建筑开始升级时间（毫秒）
    public long getStartUpgradeLvlMs() { return this.startUpgradeLvlMs; }
    public void setStartUpgradeLvlMs(BM _bm, long startUpgradeLvlMs) {
        if(startUpgradeLvlMs==this.startUpgradeLvlMs) 
            return;
        this.startUpgradeLvlMs = startUpgradeLvlMs; 
        markField(_bm, FIELD_startUpgradeLvlMs); 
    }
    public void saveStartUpgradeLvlMs(BM _bm, long startUpgradeLvlMs) {
        if(startUpgradeLvlMs==this.startUpgradeLvlMs) 
            return;
        this.startUpgradeLvlMs = startUpgradeLvlMs;
        saveField(_bm, "startUpgradeLvlMs", startUpgradeLvlMs);
    }

    // 建筑结束升级时间（毫秒）
    public long getEndUpgradeLvlMs() { return this.endUpgradeLvlMs; }
    public void setEndUpgradeLvlMs(BM _bm, long endUpgradeLvlMs) {
        if(endUpgradeLvlMs==this.endUpgradeLvlMs) 
            return;
        this.endUpgradeLvlMs = endUpgradeLvlMs; 
        markField(_bm, FIELD_endUpgradeLvlMs); 
    }
    public void saveEndUpgradeLvlMs(BM _bm, long endUpgradeLvlMs) {
        if(endUpgradeLvlMs==this.endUpgradeLvlMs) 
            return;
        this.endUpgradeLvlMs = endUpgradeLvlMs;
        saveField(_bm, "endUpgradeLvlMs", endUpgradeLvlMs);
    }

    // 升级消耗（玩家取消时返还）
    public byte[] getUpgradeCost() { return this.upgradeCost; }
    public void setUpgradeCost(BM _bm, byte[] upgradeCost) {
        if(upgradeCost==this.upgradeCost) 
            return;
        this.upgradeCost = upgradeCost; 
        markField(_bm, FIELD_upgradeCost); 
    }
    public void saveUpgradeCost(BM _bm, byte[] upgradeCost) {
        if(upgradeCost==this.upgradeCost) 
            return;
        this.upgradeCost = upgradeCost;
        saveFieldBytes(_bm, "upgradeCost", upgradeCost);
    }

    // 公会求助类型
    public int getGuildHelpType() { return this.guildHelpType; }
    public void setGuildHelpType(BM _bm, int guildHelpType) {
        if(guildHelpType==this.guildHelpType) 
            return;
        this.guildHelpType = guildHelpType; 
        markField(_bm, FIELD_guildHelpType); 
    }
    public void saveGuildHelpType(BM _bm, int guildHelpType) {
        if(guildHelpType==this.guildHelpType) 
            return;
        this.guildHelpType = guildHelpType;
        saveField(_bm, "guildHelpType", guildHelpType);
    }

    // 公会求助ID
    public long getGuildHelpId() { return this.guildHelpId; }
    public void setGuildHelpId(BM _bm, long guildHelpId) {
        if(guildHelpId==this.guildHelpId) 
            return;
        this.guildHelpId = guildHelpId; 
        markField(_bm, FIELD_guildHelpId); 
    }
    public void saveGuildHelpId(BM _bm, long guildHelpId) {
        if(guildHelpId==this.guildHelpId) 
            return;
        this.guildHelpId = guildHelpId;
        saveField(_bm, "guildHelpId", guildHelpId);
    }

    // 公会助力时间（秒）
    public int getGuildHelpSecs() { return this.guildHelpSecs; }
    public void setGuildHelpSecs(BM _bm, int guildHelpSecs) {
        if(guildHelpSecs==this.guildHelpSecs) 
            return;
        this.guildHelpSecs = guildHelpSecs; 
        markField(_bm, FIELD_guildHelpSecs); 
    }
    public void saveGuildHelpSecs(BM _bm, int guildHelpSecs) {
        if(guildHelpSecs==this.guildHelpSecs) 
            return;
        this.guildHelpSecs = guildHelpSecs;
        saveField(_bm, "guildHelpSecs", guildHelpSecs);
    }

    // 加速道具助力时间（秒）
    public int getItemHelpSecs() { return this.itemHelpSecs; }
    public void setItemHelpSecs(BM _bm, int itemHelpSecs) {
        if(itemHelpSecs==this.itemHelpSecs) 
            return;
        this.itemHelpSecs = itemHelpSecs; 
        markField(_bm, FIELD_itemHelpSecs); 
    }
    public void saveItemHelpSecs(BM _bm, int itemHelpSecs) {
        if(itemHelpSecs==this.itemHelpSecs) 
            return;
        this.itemHelpSecs = itemHelpSecs;
        saveField(_bm, "itemHelpSecs", itemHelpSecs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `techId` = '").append(techId).append("',");
        sBuilder.append(" `lvl` = '").append(lvl).append("',");
        sBuilder.append(" `isUpgrading` = '").append(isUpgrading ? 1 : 0).append("',");
        sBuilder.append(" `startUpgradeLvlMs` = '").append(startUpgradeLvlMs).append("',");
        sBuilder.append(" `endUpgradeLvlMs` = '").append(endUpgradeLvlMs).append("',");
        sBuilder.append(" `upgradeCost` = ?,");
        sBuilder.append(" `guildHelpType` = '").append(guildHelpType).append("',");
        sBuilder.append(" `guildHelpId` = '").append(guildHelpId).append("',");
        sBuilder.append(" `guildHelpSecs` = '").append(guildHelpSecs).append("',");
        sBuilder.append(" `itemHelpSecs` = '").append(itemHelpSecs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_techId)) sBuilder.append(" `techId` = '").append(techId).append("',");
        if(isFieldMarked(FIELD_lvl)) sBuilder.append(" `lvl` = '").append(lvl).append("',");
        if(isFieldMarked(FIELD_isUpgrading)) sBuilder.append(" `isUpgrading` = '").append(isUpgrading ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_startUpgradeLvlMs)) sBuilder.append(" `startUpgradeLvlMs` = '").append(startUpgradeLvlMs).append("',");
        if(isFieldMarked(FIELD_endUpgradeLvlMs)) sBuilder.append(" `endUpgradeLvlMs` = '").append(endUpgradeLvlMs).append("',");
        if(isFieldMarked(FIELD_upgradeCost)) sBuilder.append(" `upgradeCost` = ?,");
        if(isFieldMarked(FIELD_guildHelpType)) sBuilder.append(" `guildHelpType` = '").append(guildHelpType).append("',");
        if(isFieldMarked(FIELD_guildHelpId)) sBuilder.append(" `guildHelpId` = '").append(guildHelpId).append("',");
        if(isFieldMarked(FIELD_guildHelpSecs)) sBuilder.append(" `guildHelpSecs` = '").append(guildHelpSecs).append("',");
        if(isFieldMarked(FIELD_itemHelpSecs)) sBuilder.append(" `itemHelpSecs` = '").append(itemHelpSecs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_tech` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`techId` bigint(20) NOT NULL DEFAULT '0' COMMENT '科技ID',"
                + "`lvl` int(11) NOT NULL DEFAULT '0' COMMENT '科技等级',"
                + "`isUpgrading` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否升级中',"
                + "`startUpgradeLvlMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑开始升级时间（毫秒）',"
                + "`endUpgradeLvlMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑结束升级时间（毫秒）',"
                + "`upgradeCost` blob NULL COMMENT '升级消耗（玩家取消时返还）',"
                + "`guildHelpType` int(11) NOT NULL DEFAULT '0' COMMENT '公会求助类型',"
                + "`guildHelpId` bigint(20) NOT NULL DEFAULT '0' COMMENT '公会求助ID',"
                + "`guildHelpSecs` int(11) NOT NULL DEFAULT '0' COMMENT '公会助力时间（秒）',"
                + "`itemHelpSecs` int(11) NOT NULL DEFAULT '0' COMMENT '加速道具助力时间（秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='火星-火星科技数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//techId
        _size+=4;//lvl
        _size+=1;//isUpgrading
        _size+=8;//startUpgradeLvlMs
        _size+=8;//endUpgradeLvlMs
        _size+=2;_size+=upgradeCost.length;//upgradeCost
        _size+=4;//guildHelpType
        _size+=8;//guildHelpId
        _size+=4;//guildHelpSecs
        _size+=4;//itemHelpSecs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(techId);
        buff.putInt(lvl);
        buff.put((byte)(isUpgrading?1:0));
        buff.putLong(startUpgradeLvlMs);
        buff.putLong(endUpgradeLvlMs);
        buff.putShort((short)(upgradeCost == null ? 0 : upgradeCost.length));if(null != upgradeCost){buff.put(upgradeCost);}
        buff.putInt(guildHelpType);
        buff.putLong(guildHelpId);
        buff.putInt(guildHelpSecs);
        buff.putInt(itemHelpSecs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        techId=buff.getLong();
        lvl=buff.getInt();
        isUpgrading=(buff.get()==1);
        startUpgradeLvlMs=buff.getLong();
        endUpgradeLvlMs=buff.getLong();
        int upgradeCost_count = buff.getShort();if(upgradeCost_count>0){upgradeCost = new byte[upgradeCost_count];buff.get(upgradeCost);}
        guildHelpType=buff.getInt();
        guildHelpId=buff.getLong();
        guildHelpSecs=buff.getInt();
        itemHelpSecs=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
