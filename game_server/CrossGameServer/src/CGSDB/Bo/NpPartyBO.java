package CGSDB.Bo;
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
public class NpPartyBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instanceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "聚会实例ID")
    private long instanceId;

    public static final int FIELD_serverType =1;
    @DataBaseField(type = "int(11)", fieldname = "serverType", comment = "发起服务器类型")
    private int serverType;

    public static final int FIELD_serverTypeId =2;
    @DataBaseField(type = "int(11)", fieldname = "serverTypeId", comment = "发起服务器类型ID")
    private int serverTypeId;

    public static final int FIELD_refId =3;
    @DataBaseField(type = "bigint(20)", fieldname = "refId", comment = "配置ID")
    private long refId;

    public static final int FIELD_ownerCid =4;
    @DataBaseField(type = "bigint(20)", fieldname = "ownerCid", comment = "举办玩家CID")
    private long ownerCid;

    public static final int FIELD_ownerName =5;
    @DataBaseField(type = "varchar(50)", fieldname = "ownerName", comment = "举办玩家名称")
    private String ownerName;

    public static final int FIELD_declar =6;
    @DataBaseField(type = "varchar(200)", fieldname = "declar", comment = "宣言")
    private String declar;

    public static final int FIELD_startTs =7;
    @DataBaseField(type = "int(11)", fieldname = "startTs", comment = "开启时间戳（秒）")
    private int startTs;

    public static final int FIELD_endTs =8;
    @DataBaseField(type = "int(11)", fieldname = "endTs", comment = "结束时间戳（秒）")
    private int endTs;

    public static final int FIELD_profitEndTs =9;
    @DataBaseField(type = "int(11)", fieldname = "profitEndTs", comment = "收益结束时间戳（秒）")
    private int profitEndTs;

    public static final int FIELD_extraSeatCount =10;
    @DataBaseField(type = "int(11)", fieldname = "extraSeatCount", comment = "额外扩展席位数")
    private int extraSeatCount;

    public static final int FIELD_sceneType =11;
    @DataBaseField(type = "int(11)", fieldname = "sceneType", comment = "场景类型")
    private int sceneType;

    public static final int FIELD_sceneData =12;
    @DataBaseField(type = "blob(1024)", fieldname = "sceneData", comment = "场景数据")
    private byte[] sceneData;

    public NpPartyBO() {
        id = 0;
        instanceId = 0L;
        serverType = 0;
        serverTypeId = 0;
        refId = 0L;
        ownerCid = 0L;
        ownerName = "";
        declar = "";
        startTs = 0;
        endTs = 0;
        profitEndTs = 0;
        extraSeatCount = 0;
        sceneType = 0;
        sceneData = null;
    }

    public NpPartyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instanceId = rs.getLong(2);
        serverType = rs.getInt(3);
        serverTypeId = rs.getInt(4);
        refId = rs.getLong(5);
        ownerCid = rs.getLong(6);
        ownerName = rs.getString(7);
        declar = rs.getString(8);
        startTs = rs.getInt(9);
        endTs = rs.getInt(10);
        profitEndTs = rs.getInt(11);
        extraSeatCount = rs.getInt(12);
        sceneType = rs.getInt(13);
        sceneData = rs.getBytes(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpPartyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instanceId`, `serverType`, `serverTypeId`, `refId`, `ownerCid`, `ownerName`, `declar`, `startTs`, `endTs`, `profitEndTs`, `extraSeatCount`, `sceneType`, `sceneData`";
    }

    @Override
    public String getTableName() {
        return "`np_party`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(serverType).append("', ");
        strBuf.append("'").append(serverTypeId).append("', ");
        strBuf.append("'").append(refId).append("', ");
        strBuf.append("'").append(ownerCid).append("', ");
        strBuf.append("'").append(ownerName == null ? null : ownerName.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(declar == null ? null : declar.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(startTs).append("', ");
        strBuf.append("'").append(endTs).append("', ");
        strBuf.append("'").append(profitEndTs).append("', ");
        strBuf.append("'").append(extraSeatCount).append("', ");
        strBuf.append("'").append(sceneType).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(sceneData);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_sceneData)) ret.add(sceneData);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 聚会实例ID
    public long getInstanceId() { return this.instanceId; }
    public void setInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId; 
        markField(_bm, FIELD_instanceId); 
    }
    public void saveInstanceId(BM _bm, long instanceId) {
        if(instanceId==this.instanceId) 
            return;
        this.instanceId = instanceId;
        saveField(_bm, "instanceId", instanceId);
    }

    // 发起服务器类型
    public int getServerType() { return this.serverType; }
    public void setServerType(BM _bm, int serverType) {
        if(serverType==this.serverType) 
            return;
        this.serverType = serverType; 
        markField(_bm, FIELD_serverType); 
    }
    public void saveServerType(BM _bm, int serverType) {
        if(serverType==this.serverType) 
            return;
        this.serverType = serverType;
        saveField(_bm, "serverType", serverType);
    }

    // 发起服务器类型ID
    public int getServerTypeId() { return this.serverTypeId; }
    public void setServerTypeId(BM _bm, int serverTypeId) {
        if(serverTypeId==this.serverTypeId) 
            return;
        this.serverTypeId = serverTypeId; 
        markField(_bm, FIELD_serverTypeId); 
    }
    public void saveServerTypeId(BM _bm, int serverTypeId) {
        if(serverTypeId==this.serverTypeId) 
            return;
        this.serverTypeId = serverTypeId;
        saveField(_bm, "serverTypeId", serverTypeId);
    }

    // 配置ID
    public long getRefId() { return this.refId; }
    public void setRefId(BM _bm, long refId) {
        if(refId==this.refId) 
            return;
        this.refId = refId; 
        markField(_bm, FIELD_refId); 
    }
    public void saveRefId(BM _bm, long refId) {
        if(refId==this.refId) 
            return;
        this.refId = refId;
        saveField(_bm, "refId", refId);
    }

    // 举办玩家CID
    public long getOwnerCid() { return this.ownerCid; }
    public void setOwnerCid(BM _bm, long ownerCid) {
        if(ownerCid==this.ownerCid) 
            return;
        this.ownerCid = ownerCid; 
        markField(_bm, FIELD_ownerCid); 
    }
    public void saveOwnerCid(BM _bm, long ownerCid) {
        if(ownerCid==this.ownerCid) 
            return;
        this.ownerCid = ownerCid;
        saveField(_bm, "ownerCid", ownerCid);
    }

    // 举办玩家名称
    public String getOwnerName() { return this.ownerName; }
    public void setOwnerName(BM _bm, String ownerName) {
        if(ownerName.equals(this.ownerName)) 
            return;
        this.ownerName = ownerName; 
        markField(_bm, FIELD_ownerName); 
    }
    public void saveOwnerName(BM _bm, String ownerName) {
        if(ownerName.equals(this.ownerName)) 
            return;
        this.ownerName = ownerName;
        saveField(_bm, "ownerName", ownerName);
    }

    // 宣言
    public String getDeclar() { return this.declar; }
    public void setDeclar(BM _bm, String declar) {
        if(declar.equals(this.declar)) 
            return;
        this.declar = declar; 
        markField(_bm, FIELD_declar); 
    }
    public void saveDeclar(BM _bm, String declar) {
        if(declar.equals(this.declar)) 
            return;
        this.declar = declar;
        saveField(_bm, "declar", declar);
    }

    // 开启时间戳（秒）
    public int getStartTs() { return this.startTs; }
    public void setStartTs(BM _bm, int startTs) {
        if(startTs==this.startTs) 
            return;
        this.startTs = startTs; 
        markField(_bm, FIELD_startTs); 
    }
    public void saveStartTs(BM _bm, int startTs) {
        if(startTs==this.startTs) 
            return;
        this.startTs = startTs;
        saveField(_bm, "startTs", startTs);
    }

    // 结束时间戳（秒）
    public int getEndTs() { return this.endTs; }
    public void setEndTs(BM _bm, int endTs) {
        if(endTs==this.endTs) 
            return;
        this.endTs = endTs; 
        markField(_bm, FIELD_endTs); 
    }
    public void saveEndTs(BM _bm, int endTs) {
        if(endTs==this.endTs) 
            return;
        this.endTs = endTs;
        saveField(_bm, "endTs", endTs);
    }

    // 收益结束时间戳（秒）
    public int getProfitEndTs() { return this.profitEndTs; }
    public void setProfitEndTs(BM _bm, int profitEndTs) {
        if(profitEndTs==this.profitEndTs) 
            return;
        this.profitEndTs = profitEndTs; 
        markField(_bm, FIELD_profitEndTs); 
    }
    public void saveProfitEndTs(BM _bm, int profitEndTs) {
        if(profitEndTs==this.profitEndTs) 
            return;
        this.profitEndTs = profitEndTs;
        saveField(_bm, "profitEndTs", profitEndTs);
    }

    // 额外扩展席位数
    public int getExtraSeatCount() { return this.extraSeatCount; }
    public void setExtraSeatCount(BM _bm, int extraSeatCount) {
        if(extraSeatCount==this.extraSeatCount) 
            return;
        this.extraSeatCount = extraSeatCount; 
        markField(_bm, FIELD_extraSeatCount); 
    }
    public void saveExtraSeatCount(BM _bm, int extraSeatCount) {
        if(extraSeatCount==this.extraSeatCount) 
            return;
        this.extraSeatCount = extraSeatCount;
        saveField(_bm, "extraSeatCount", extraSeatCount);
    }

    // 场景类型
    public int getSceneType() { return this.sceneType; }
    public void setSceneType(BM _bm, int sceneType) {
        if(sceneType==this.sceneType) 
            return;
        this.sceneType = sceneType; 
        markField(_bm, FIELD_sceneType); 
    }
    public void saveSceneType(BM _bm, int sceneType) {
        if(sceneType==this.sceneType) 
            return;
        this.sceneType = sceneType;
        saveField(_bm, "sceneType", sceneType);
    }

    // 场景数据
    public byte[] getSceneData() { return this.sceneData; }
    public void setSceneData(BM _bm, byte[] sceneData) {
        if(sceneData==this.sceneData) 
            return;
        this.sceneData = sceneData; 
        markField(_bm, FIELD_sceneData); 
    }
    public void saveSceneData(BM _bm, byte[] sceneData) {
        if(sceneData==this.sceneData) 
            return;
        this.sceneData = sceneData;
        saveFieldBytes(_bm, "sceneData", sceneData);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `serverType` = '").append(serverType).append("',");
        sBuilder.append(" `serverTypeId` = '").append(serverTypeId).append("',");
        sBuilder.append(" `refId` = '").append(refId).append("',");
        sBuilder.append(" `ownerCid` = '").append(ownerCid).append("',");
        sBuilder.append(" `ownerName` = '").append(ownerName == null ? null : ownerName.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `declar` = '").append(declar == null ? null : declar.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `startTs` = '").append(startTs).append("',");
        sBuilder.append(" `endTs` = '").append(endTs).append("',");
        sBuilder.append(" `profitEndTs` = '").append(profitEndTs).append("',");
        sBuilder.append(" `extraSeatCount` = '").append(extraSeatCount).append("',");
        sBuilder.append(" `sceneType` = '").append(sceneType).append("',");
        sBuilder.append(" `sceneData` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_serverType)) sBuilder.append(" `serverType` = '").append(serverType).append("',");
        if(isFieldMarked(FIELD_serverTypeId)) sBuilder.append(" `serverTypeId` = '").append(serverTypeId).append("',");
        if(isFieldMarked(FIELD_refId)) sBuilder.append(" `refId` = '").append(refId).append("',");
        if(isFieldMarked(FIELD_ownerCid)) sBuilder.append(" `ownerCid` = '").append(ownerCid).append("',");
        if(isFieldMarked(FIELD_ownerName)) sBuilder.append(" `ownerName` = '").append(ownerName == null ? null : ownerName.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_declar)) sBuilder.append(" `declar` = '").append(declar == null ? null : declar.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_startTs)) sBuilder.append(" `startTs` = '").append(startTs).append("',");
        if(isFieldMarked(FIELD_endTs)) sBuilder.append(" `endTs` = '").append(endTs).append("',");
        if(isFieldMarked(FIELD_profitEndTs)) sBuilder.append(" `profitEndTs` = '").append(profitEndTs).append("',");
        if(isFieldMarked(FIELD_extraSeatCount)) sBuilder.append(" `extraSeatCount` = '").append(extraSeatCount).append("',");
        if(isFieldMarked(FIELD_sceneType)) sBuilder.append(" `sceneType` = '").append(sceneType).append("',");
        if(isFieldMarked(FIELD_sceneData)) sBuilder.append(" `sceneData` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_party` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '聚会实例ID',"
                + "`serverType` int(11) NOT NULL DEFAULT '0' COMMENT '发起服务器类型',"
                + "`serverTypeId` int(11) NOT NULL DEFAULT '0' COMMENT '发起服务器类型ID',"
                + "`refId` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置ID',"
                + "`ownerCid` bigint(20) NOT NULL DEFAULT '0' COMMENT '举办玩家CID',"
                + "`ownerName` varchar(50) NOT NULL DEFAULT '' COMMENT '举办玩家名称',"
                + "`declar` varchar(200) NOT NULL DEFAULT '' COMMENT '宣言',"
                + "`startTs` int(11) NOT NULL DEFAULT '0' COMMENT '开启时间戳（秒）',"
                + "`endTs` int(11) NOT NULL DEFAULT '0' COMMENT '结束时间戳（秒）',"
                + "`profitEndTs` int(11) NOT NULL DEFAULT '0' COMMENT '收益结束时间戳（秒）',"
                + "`extraSeatCount` int(11) NOT NULL DEFAULT '0' COMMENT '额外扩展席位数',"
                + "`sceneType` int(11) NOT NULL DEFAULT '0' COMMENT '场景类型',"
                + "`sceneData` blob NULL COMMENT '场景数据',"
                + "UNIQUE INDEX `instanceId` (`instanceId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Party 聚会数据表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.crossgame_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//instanceId
        _size+=4;//serverType
        _size+=4;//serverTypeId
        _size+=8;//refId
        _size+=8;//ownerCid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(ownerName);//ownerName
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(declar);//declar
        _size+=4;//startTs
        _size+=4;//endTs
        _size+=4;//profitEndTs
        _size+=4;//extraSeatCount
        _size+=4;//sceneType
        _size+=2;_size+=sceneData.length;//sceneData
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instanceId);
        buff.putInt(serverType);
        buff.putInt(serverTypeId);
        buff.putLong(refId);
        buff.putLong(ownerCid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, ownerName);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, declar);
        buff.putInt(startTs);
        buff.putInt(endTs);
        buff.putInt(profitEndTs);
        buff.putInt(extraSeatCount);
        buff.putInt(sceneType);
        buff.putShort((short)(sceneData == null ? 0 : sceneData.length));if(null != sceneData){buff.put(sceneData);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instanceId=buff.getLong();
        serverType=buff.getInt();
        serverTypeId=buff.getInt();
        refId=buff.getLong();
        ownerCid=buff.getLong();
        ownerName=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        declar=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        startTs=buff.getInt();
        endTs=buff.getInt();
        profitEndTs=buff.getInt();
        extraSeatCount=buff.getInt();
        sceneType=buff.getInt();
        int sceneData_count = buff.getShort();if(sceneData_count>0){sceneData = new byte[sceneData_count];buff.get(sceneData);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
