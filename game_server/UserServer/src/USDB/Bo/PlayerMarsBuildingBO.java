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
public class PlayerMarsBuildingBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_buildingId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "buildingId", comment = "建筑ID")
    private long buildingId;

    public static final int FIELD_lvl =2;
    @DataBaseField(type = "int(11)", fieldname = "lvl", comment = "建筑等级")
    private int lvl;

    public static final int FIELD_isFoodPowerOn =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "isFoodPowerOn", comment = "Food部件功率开关")
    private boolean isFoodPowerOn;

    public PlayerMarsBuildingBO() {
        id = 0;
        cid = 0L;
        buildingId = 0L;
        lvl = 0;
        isFoodPowerOn = false;
    }

    public PlayerMarsBuildingBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        buildingId = rs.getLong(3);
        lvl = rs.getInt(4);
        isFoodPowerOn = rs.getBoolean(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsBuildingBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `buildingId`, `lvl`, `isFoodPowerOn`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_building`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(buildingId).append("', ");
        strBuf.append("'").append(lvl).append("', ");
        strBuf.append("'").append(isFoodPowerOn ? 1 : 0).append("', ");
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

    // 建筑等级
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

    // Food部件功率开关
    public boolean getIsFoodPowerOn() { return this.isFoodPowerOn; }
    public void setIsFoodPowerOn(BM _bm, boolean isFoodPowerOn) {
        if(isFoodPowerOn==this.isFoodPowerOn) 
            return;
        this.isFoodPowerOn = isFoodPowerOn; 
        markField(_bm, FIELD_isFoodPowerOn); 
    }
    public void saveIsFoodPowerOn(BM _bm, boolean isFoodPowerOn) {
        if(isFoodPowerOn==this.isFoodPowerOn) 
            return;
        this.isFoodPowerOn = isFoodPowerOn;
        saveField(_bm, "isFoodPowerOn", isFoodPowerOn ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `buildingId` = '").append(buildingId).append("',");
        sBuilder.append(" `lvl` = '").append(lvl).append("',");
        sBuilder.append(" `isFoodPowerOn` = '").append(isFoodPowerOn ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_buildingId)) sBuilder.append(" `buildingId` = '").append(buildingId).append("',");
        if(isFieldMarked(FIELD_lvl)) sBuilder.append(" `lvl` = '").append(lvl).append("',");
        if(isFieldMarked(FIELD_isFoodPowerOn)) sBuilder.append(" `isFoodPowerOn` = '").append(isFoodPowerOn ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_building` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`buildingId` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑ID',"
                + "`lvl` int(11) NOT NULL DEFAULT '0' COMMENT '建筑等级',"
                + "`isFoodPowerOn` tinyint(1) NOT NULL DEFAULT '0' COMMENT 'Food部件功率开关',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家建筑基础数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//lvl
        _size+=1;//isFoodPowerOn
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
        buff.putInt(lvl);
        buff.put((byte)(isFoodPowerOn?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        buildingId=buff.getLong();
        lvl=buff.getInt();
        isFoodPowerOn=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
