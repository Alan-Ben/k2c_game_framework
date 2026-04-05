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
public class PlayerMarsBuildingUpQueueBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_buildingId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "buildingId", comment = "建筑ID")
    private long buildingId;

    public static final int FIELD_targetLvl =2;
    @DataBaseField(type = "int(11)", fieldname = "targetLvl", comment = "建筑目标等级")
    private int targetLvl;

    public static final int FIELD_startUpgradeLvlMs =3;
    @DataBaseField(type = "bigint(20)", fieldname = "startUpgradeLvlMs", comment = "建筑开始升级时间（毫秒）")
    private long startUpgradeLvlMs;

    public static final int FIELD_endUpgradeLvlMs =4;
    @DataBaseField(type = "bigint(20)", fieldname = "endUpgradeLvlMs", comment = "建筑结束升级时间（毫秒）")
    private long endUpgradeLvlMs;

    public static final int FIELD_upgradeCost =5;
    @DataBaseField(type = "blob", fieldname = "upgradeCost", comment = "升级消耗（玩家取消时返还）")
    private byte[] upgradeCost;

    public static final int FIELD_guildHelpId =6;
    @DataBaseField(type = "bigint(20)", fieldname = "guildHelpId", comment = "公会求助ID")
    private long guildHelpId;

    public static final int FIELD_guildHelpSecs =7;
    @DataBaseField(type = "int(11)", fieldname = "guildHelpSecs", comment = "公会助力时间（秒）")
    private int guildHelpSecs;

    public static final int FIELD_itemHelpSecs =8;
    @DataBaseField(type = "int(11)", fieldname = "itemHelpSecs", comment = "加速道具助力时间（秒）")
    private int itemHelpSecs;

    public PlayerMarsBuildingUpQueueBO() {
        id = 0;
        cid = 0L;
        buildingId = 0L;
        targetLvl = 0;
        startUpgradeLvlMs = 0L;
        endUpgradeLvlMs = 0L;
        upgradeCost = null;
        guildHelpId = 0L;
        guildHelpSecs = 0;
        itemHelpSecs = 0;
    }

    public PlayerMarsBuildingUpQueueBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        buildingId = rs.getLong(3);
        targetLvl = rs.getInt(4);
        startUpgradeLvlMs = rs.getLong(5);
        endUpgradeLvlMs = rs.getLong(6);
        upgradeCost = rs.getBytes(7);
        guildHelpId = rs.getLong(8);
        guildHelpSecs = rs.getInt(9);
        itemHelpSecs = rs.getInt(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsBuildingUpQueueBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `buildingId`, `targetLvl`, `startUpgradeLvlMs`, `endUpgradeLvlMs`, `upgradeCost`, `guildHelpId`, `guildHelpSecs`, `itemHelpSecs`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_building_up_queue`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(buildingId).append("', ");
        strBuf.append("'").append(targetLvl).append("', ");
        strBuf.append("'").append(startUpgradeLvlMs).append("', ");
        strBuf.append("'").append(endUpgradeLvlMs).append("', ");
        strBuf.append("?, ");
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

    // 建筑ID
    public long getBuildingId() { return this.buildingId; }
    public void setBuildingId(BM _bm, long buildingId) {
        if(buildingId==this.buildingId) 
            return;
        this.buildingId = buildingId; 
        markField(_bm, FIELD_buildingId); 
    }
    public void saveBuildingId(BM _bm, long buildingId) {
        if(buildingId==this.buildingId) 
            return;
        this.buildingId = buildingId;
        saveField(_bm, "buildingId", buildingId);
    }

    // 建筑目标等级
    public int getTargetLvl() { return this.targetLvl; }
    public void setTargetLvl(BM _bm, int targetLvl) {
        if(targetLvl==this.targetLvl) 
            return;
        this.targetLvl = targetLvl; 
        markField(_bm, FIELD_targetLvl); 
    }
    public void saveTargetLvl(BM _bm, int targetLvl) {
        if(targetLvl==this.targetLvl) 
            return;
        this.targetLvl = targetLvl;
        saveField(_bm, "targetLvl", targetLvl);
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
        sBuilder.append(" `buildingId` = '").append(buildingId).append("',");
        sBuilder.append(" `targetLvl` = '").append(targetLvl).append("',");
        sBuilder.append(" `startUpgradeLvlMs` = '").append(startUpgradeLvlMs).append("',");
        sBuilder.append(" `endUpgradeLvlMs` = '").append(endUpgradeLvlMs).append("',");
        sBuilder.append(" `upgradeCost` = ?,");
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
        if(isFieldMarked(FIELD_buildingId)) sBuilder.append(" `buildingId` = '").append(buildingId).append("',");
        if(isFieldMarked(FIELD_targetLvl)) sBuilder.append(" `targetLvl` = '").append(targetLvl).append("',");
        if(isFieldMarked(FIELD_startUpgradeLvlMs)) sBuilder.append(" `startUpgradeLvlMs` = '").append(startUpgradeLvlMs).append("',");
        if(isFieldMarked(FIELD_endUpgradeLvlMs)) sBuilder.append(" `endUpgradeLvlMs` = '").append(endUpgradeLvlMs).append("',");
        if(isFieldMarked(FIELD_upgradeCost)) sBuilder.append(" `upgradeCost` = ?,");
        if(isFieldMarked(FIELD_guildHelpId)) sBuilder.append(" `guildHelpId` = '").append(guildHelpId).append("',");
        if(isFieldMarked(FIELD_guildHelpSecs)) sBuilder.append(" `guildHelpSecs` = '").append(guildHelpSecs).append("',");
        if(isFieldMarked(FIELD_itemHelpSecs)) sBuilder.append(" `itemHelpSecs` = '").append(itemHelpSecs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_building_up_queue` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`buildingId` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑ID',"
                + "`targetLvl` int(11) NOT NULL DEFAULT '0' COMMENT '建筑目标等级',"
                + "`startUpgradeLvlMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑开始升级时间（毫秒）',"
                + "`endUpgradeLvlMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑结束升级时间（毫秒）',"
                + "`upgradeCost` blob NULL COMMENT '升级消耗（玩家取消时返还）',"
                + "`guildHelpId` bigint(20) NOT NULL DEFAULT '0' COMMENT '公会求助ID',"
                + "`guildHelpSecs` int(11) NOT NULL DEFAULT '0' COMMENT '公会助力时间（秒）',"
                + "`itemHelpSecs` int(11) NOT NULL DEFAULT '0' COMMENT '加速道具助力时间（秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家建筑建造/升级队列数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//buildingId
        _size+=4;//targetLvl
        _size+=8;//startUpgradeLvlMs
        _size+=8;//endUpgradeLvlMs
        _size+=2;_size+=upgradeCost.length;//upgradeCost
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
        buff.putLong(buildingId);
        buff.putInt(targetLvl);
        buff.putLong(startUpgradeLvlMs);
        buff.putLong(endUpgradeLvlMs);
        buff.putShort((short)(upgradeCost == null ? 0 : upgradeCost.length));if(null != upgradeCost){buff.put(upgradeCost);}
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
        buildingId=buff.getLong();
        targetLvl=buff.getInt();
        startUpgradeLvlMs=buff.getLong();
        endUpgradeLvlMs=buff.getLong();
        int upgradeCost_count = buff.getShort();if(upgradeCost_count>0){upgradeCost = new byte[upgradeCost_count];buff.get(upgradeCost);}
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
