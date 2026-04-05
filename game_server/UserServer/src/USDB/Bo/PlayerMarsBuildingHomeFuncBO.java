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
public class PlayerMarsBuildingHomeFuncBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_isNormalOn =1;
    @DataBaseField(type = "tinyint(1)", fieldname = "isNormalOn", comment = "普通功率开启")
    private boolean isNormalOn;

    public static final int FIELD_isOverdriveOn =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "isOverdriveOn", comment = "最高功率开启")
    private boolean isOverdriveOn;

    public static final int FIELD_lastCollectTimeS =3;
    @DataBaseField(type = "int(11)", fieldname = "lastCollectTimeS", comment = "最后收集资源的时间点")
    private int lastCollectTimeS;

    public PlayerMarsBuildingHomeFuncBO() {
        id = 0;
        cid = 0L;
        isNormalOn = false;
        isOverdriveOn = false;
        lastCollectTimeS = 0;
    }

    public PlayerMarsBuildingHomeFuncBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        isNormalOn = rs.getBoolean(3);
        isOverdriveOn = rs.getBoolean(4);
        lastCollectTimeS = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsBuildingHomeFuncBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `isNormalOn`, `isOverdriveOn`, `lastCollectTimeS`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_building_home_func`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(isNormalOn ? 1 : 0).append("', ");
        strBuf.append("'").append(isOverdriveOn ? 1 : 0).append("', ");
        strBuf.append("'").append(lastCollectTimeS).append("', ");
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

    // 普通功率开启
    public boolean getIsNormalOn() { return this.isNormalOn; }
    public void setIsNormalOn(BM _bm, boolean isNormalOn) {
        if(isNormalOn==this.isNormalOn) 
            return;
        this.isNormalOn = isNormalOn; 
        markField(_bm, FIELD_isNormalOn); 
    }
    public void saveIsNormalOn(BM _bm, boolean isNormalOn) {
        if(isNormalOn==this.isNormalOn) 
            return;
        this.isNormalOn = isNormalOn;
        saveField(_bm, "isNormalOn", isNormalOn ? 1 : 0);
    }

    // 最高功率开启
    public boolean getIsOverdriveOn() { return this.isOverdriveOn; }
    public void setIsOverdriveOn(BM _bm, boolean isOverdriveOn) {
        if(isOverdriveOn==this.isOverdriveOn) 
            return;
        this.isOverdriveOn = isOverdriveOn; 
        markField(_bm, FIELD_isOverdriveOn); 
    }
    public void saveIsOverdriveOn(BM _bm, boolean isOverdriveOn) {
        if(isOverdriveOn==this.isOverdriveOn) 
            return;
        this.isOverdriveOn = isOverdriveOn;
        saveField(_bm, "isOverdriveOn", isOverdriveOn ? 1 : 0);
    }

    // 最后收集资源的时间点
    public int getLastCollectTimeS() { return this.lastCollectTimeS; }
    public void setLastCollectTimeS(BM _bm, int lastCollectTimeS) {
        if(lastCollectTimeS==this.lastCollectTimeS) 
            return;
        this.lastCollectTimeS = lastCollectTimeS; 
        markField(_bm, FIELD_lastCollectTimeS); 
    }
    public void saveLastCollectTimeS(BM _bm, int lastCollectTimeS) {
        if(lastCollectTimeS==this.lastCollectTimeS) 
            return;
        this.lastCollectTimeS = lastCollectTimeS;
        saveField(_bm, "lastCollectTimeS", lastCollectTimeS);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `isNormalOn` = '").append(isNormalOn ? 1 : 0).append("',");
        sBuilder.append(" `isOverdriveOn` = '").append(isOverdriveOn ? 1 : 0).append("',");
        sBuilder.append(" `lastCollectTimeS` = '").append(lastCollectTimeS).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_isNormalOn)) sBuilder.append(" `isNormalOn` = '").append(isNormalOn ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_isOverdriveOn)) sBuilder.append(" `isOverdriveOn` = '").append(isOverdriveOn ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_lastCollectTimeS)) sBuilder.append(" `lastCollectTimeS` = '").append(lastCollectTimeS).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_building_home_func` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`isNormalOn` tinyint(1) NOT NULL DEFAULT '0' COMMENT '普通功率开启',"
                + "`isOverdriveOn` tinyint(1) NOT NULL DEFAULT '0' COMMENT '最高功率开启',"
                + "`lastCollectTimeS` int(11) NOT NULL DEFAULT '0' COMMENT '最后收集资源的时间点',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家主基地建筑功能数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=1;//isNormalOn
        _size+=1;//isOverdriveOn
        _size+=4;//lastCollectTimeS
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.put((byte)(isNormalOn?1:0));
        buff.put((byte)(isOverdriveOn?1:0));
        buff.putInt(lastCollectTimeS);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        isNormalOn=(buff.get()==1);
        isOverdriveOn=(buff.get()==1);
        lastCollectTimeS=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
