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
public class PlayerMarsPeopleFuncBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_buildingId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "buildingId", comment = "建筑ID")
    private long buildingId;

    public static final int FIELD_dispatchedNum =2;
    @DataBaseField(type = "int(11)", fieldname = "dispatchedNum", comment = "已派遣居民数量")
    private int dispatchedNum;

    public static final int FIELD_energy =3;
    @DataBaseField(type = "bigint(20)", fieldname = "energy", comment = "累积的能量数值")
    private long energy;

    public static final int FIELD_energyLastCalMs =4;
    @DataBaseField(type = "bigint(20)", fieldname = "energyLastCalMs", comment = "能量数值最后一次计算时间（毫秒）")
    private long energyLastCalMs;

    public PlayerMarsPeopleFuncBO() {
        id = 0;
        cid = 0L;
        buildingId = 0L;
        dispatchedNum = 0;
        energy = 0L;
        energyLastCalMs = 0L;
    }

    public PlayerMarsPeopleFuncBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        buildingId = rs.getLong(3);
        dispatchedNum = rs.getInt(4);
        energy = rs.getLong(5);
        energyLastCalMs = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsPeopleFuncBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `buildingId`, `dispatchedNum`, `energy`, `energyLastCalMs`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_people_func`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(buildingId).append("', ");
        strBuf.append("'").append(dispatchedNum).append("', ");
        strBuf.append("'").append(energy).append("', ");
        strBuf.append("'").append(energyLastCalMs).append("', ");
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

    // 已派遣居民数量
    public int getDispatchedNum() { return this.dispatchedNum; }
    public void setDispatchedNum(BM _bm, int dispatchedNum) {
        if(dispatchedNum==this.dispatchedNum) 
            return;
        this.dispatchedNum = dispatchedNum; 
        markField(_bm, FIELD_dispatchedNum); 
    }
    public void saveDispatchedNum(BM _bm, int dispatchedNum) {
        if(dispatchedNum==this.dispatchedNum) 
            return;
        this.dispatchedNum = dispatchedNum;
        saveField(_bm, "dispatchedNum", dispatchedNum);
    }

    // 累积的能量数值
    public long getEnergy() { return this.energy; }
    public void setEnergy(BM _bm, long energy) {
        if(energy==this.energy) 
            return;
        this.energy = energy; 
        markField(_bm, FIELD_energy); 
    }
    public void saveEnergy(BM _bm, long energy) {
        if(energy==this.energy) 
            return;
        this.energy = energy;
        saveField(_bm, "energy", energy);
    }

    // 能量数值最后一次计算时间（毫秒）
    public long getEnergyLastCalMs() { return this.energyLastCalMs; }
    public void setEnergyLastCalMs(BM _bm, long energyLastCalMs) {
        if(energyLastCalMs==this.energyLastCalMs) 
            return;
        this.energyLastCalMs = energyLastCalMs; 
        markField(_bm, FIELD_energyLastCalMs); 
    }
    public void saveEnergyLastCalMs(BM _bm, long energyLastCalMs) {
        if(energyLastCalMs==this.energyLastCalMs) 
            return;
        this.energyLastCalMs = energyLastCalMs;
        saveField(_bm, "energyLastCalMs", energyLastCalMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `buildingId` = '").append(buildingId).append("',");
        sBuilder.append(" `dispatchedNum` = '").append(dispatchedNum).append("',");
        sBuilder.append(" `energy` = '").append(energy).append("',");
        sBuilder.append(" `energyLastCalMs` = '").append(energyLastCalMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_buildingId)) sBuilder.append(" `buildingId` = '").append(buildingId).append("',");
        if(isFieldMarked(FIELD_dispatchedNum)) sBuilder.append(" `dispatchedNum` = '").append(dispatchedNum).append("',");
        if(isFieldMarked(FIELD_energy)) sBuilder.append(" `energy` = '").append(energy).append("',");
        if(isFieldMarked(FIELD_energyLastCalMs)) sBuilder.append(" `energyLastCalMs` = '").append(energyLastCalMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_people_func` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`buildingId` bigint(20) NOT NULL DEFAULT '0' COMMENT '建筑ID',"
                + "`dispatchedNum` int(11) NOT NULL DEFAULT '0' COMMENT '已派遣居民数量',"
                + "`energy` bigint(20) NOT NULL DEFAULT '0' COMMENT '累积的能量数值',"
                + "`energyLastCalMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '能量数值最后一次计算时间（毫秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家居民建筑功能数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//dispatchedNum
        _size+=8;//energy
        _size+=8;//energyLastCalMs
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
        buff.putInt(dispatchedNum);
        buff.putLong(energy);
        buff.putLong(energyLastCalMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        buildingId=buff.getLong();
        dispatchedNum=buff.getInt();
        energy=buff.getLong();
        energyLastCalMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
