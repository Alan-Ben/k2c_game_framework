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
public class PlayerComboTitleBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_unitType =1;
    @DataBaseField(type = "int(11)", fieldname = "unitType", comment = "类型 ENPPlayerComboTitleType")
    private int unitType;

    public static final int FIELD_unitId =2;
    @DataBaseField(type = "bigint(20)", fieldname = "unitId", comment = "单位ID")
    private long unitId;

    public static final int FIELD_viewed =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "viewed", comment = "是否已查看")
    private boolean viewed;

    public PlayerComboTitleBO() {
        id = 0;
        cid = 0L;
        unitType = 0;
        unitId = 0L;
        viewed = false;
    }

    public PlayerComboTitleBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        unitType = rs.getInt(3);
        unitId = rs.getLong(4);
        viewed = rs.getBoolean(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerComboTitleBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `unitType`, `unitId`, `viewed`";
    }

    @Override
    public String getTableName() {
        return "`player_combo_title`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(unitType).append("', ");
        strBuf.append("'").append(unitId).append("', ");
        strBuf.append("'").append(viewed ? 1 : 0).append("', ");
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

    // 类型 ENPPlayerComboTitleType
    public int getUnitType() { return this.unitType; }
    public void setUnitType(BM _bm, int unitType) {
        if(unitType==this.unitType) 
            return;
        this.unitType = unitType; 
        markField(_bm, FIELD_unitType); 
    }
    public void saveUnitType(BM _bm, int unitType) {
        if(unitType==this.unitType) 
            return;
        this.unitType = unitType;
        saveField(_bm, "unitType", unitType);
    }

    // 单位ID
    public long getUnitId() { return this.unitId; }
    public void setUnitId(BM _bm, long unitId) {
        if(unitId==this.unitId) 
            return;
        this.unitId = unitId; 
        markField(_bm, FIELD_unitId); 
    }
    public void saveUnitId(BM _bm, long unitId) {
        if(unitId==this.unitId) 
            return;
        this.unitId = unitId;
        saveField(_bm, "unitId", unitId);
    }

    // 是否已查看
    public boolean getViewed() { return this.viewed; }
    public void setViewed(BM _bm, boolean viewed) {
        if(viewed==this.viewed) 
            return;
        this.viewed = viewed; 
        markField(_bm, FIELD_viewed); 
    }
    public void saveViewed(BM _bm, boolean viewed) {
        if(viewed==this.viewed) 
            return;
        this.viewed = viewed;
        saveField(_bm, "viewed", viewed ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `unitType` = '").append(unitType).append("',");
        sBuilder.append(" `unitId` = '").append(unitId).append("',");
        sBuilder.append(" `viewed` = '").append(viewed ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_unitType)) sBuilder.append(" `unitType` = '").append(unitType).append("',");
        if(isFieldMarked(FIELD_unitId)) sBuilder.append(" `unitId` = '").append(unitId).append("',");
        if(isFieldMarked(FIELD_viewed)) sBuilder.append(" `viewed` = '").append(viewed ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_combo_title` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`unitType` int(11) NOT NULL DEFAULT '0' COMMENT '类型 ENPPlayerComboTitleType',"
                + "`unitId` bigint(20) NOT NULL DEFAULT '0' COMMENT '单位ID',"
                + "`viewed` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已查看',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='User Info 玩家组合称号数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//unitType
        _size+=8;//unitId
        _size+=1;//viewed
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(unitType);
        buff.putLong(unitId);
        buff.put((byte)(viewed?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        unitType=buff.getInt();
        unitId=buff.getLong();
        viewed=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
