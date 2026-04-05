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
public class PlayerMarsBuildingEquipmentBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_buildingId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "buildingId", comment = "建筑ID")
    private long buildingId;

    public static final int FIELD_equipmentId =2;
    @DataBaseField(type = "bigint(20)", fieldname = "equipmentId", comment = "建筑部件ID")
    private long equipmentId;

    public static final int FIELD_lvl =3;
    @DataBaseField(type = "int(11)", fieldname = "lvl", comment = "建筑部件等级")
    private int lvl;

    public PlayerMarsBuildingEquipmentBO() {
        id = 0;
        cid = 0L;
        buildingId = 0L;
        equipmentId = 0L;
        lvl = 0;
    }

    public PlayerMarsBuildingEquipmentBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        buildingId = rs.getLong(3);
        equipmentId = rs.getLong(4);
        lvl = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsBuildingEquipmentBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `buildingId`, `equipmentId`, `lvl`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_building_equipment`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(buildingId).append("', ");
        strBuf.append("'").append(equipmentId).append("', ");
        strBuf.append("'").append(lvl).append("', ");
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

    // 建筑部件ID
    public long getEquipmentId() { return this.equipmentId; }
    public void setEquipmentId(BM _bm, long equipmentId) {
        if(equipmentId==this.equipmentId) 
            return;
        this.equipmentId = equipmentId; 
        markField(_bm, FIELD_equipmentId); 
    }
    public void saveEquipmentId(BM _bm, long equipmentId) {
        if(equipmentId==this.equipmentId) 
            return;
        this.equipmentId = equipmentId;
        saveField(_bm, "equipmentId", equipmentId);
    }

    // 建筑部件等级
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `buildingId` = '").append(buildingId).append("',");
        sBuilder.append(" `equipmentId` = '").append(equipmentId).append("',");
        sBuilder.append(" `lvl` = '").append(lvl).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_buildingId)) sBuilder.append(" `buildingId` = '").append(buildingId).append("',");
        if(isFieldMarked(FIELD_equipmentId)) sBuilder.append(" `equipmentId` = '").append(equipmentId).append("',");
        if(isFieldMarked(FIELD_lvl)) sBuilder.append(" `lvl` = '").append(lvl).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_building_equipment` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`buildingId` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑ID',"
                + "`equipmentId` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑部件ID',"
                + "`lvl` int(11) NOT NULL DEFAULT '0' COMMENT '建筑部件等级',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家居民建筑部件数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//equipmentId
        _size+=4;//lvl
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
        buff.putLong(equipmentId);
        buff.putInt(lvl);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        buildingId=buff.getLong();
        equipmentId=buff.getLong();
        lvl=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
