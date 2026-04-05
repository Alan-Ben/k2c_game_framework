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
public class UsBoxBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instanceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "宝箱实例ID")
    private long instanceId;

    public static final int FIELD_refId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "refId", comment = "宝箱配置ID")
    private long refId;

    public static final int FIELD_createdAt =2;
    @DataBaseField(type = "bigint(20)", fieldname = "createdAt", comment = "创建时间（毫秒）")
    private long createdAt;

    public static final int FIELD_senderCid =3;
    @DataBaseField(type = "bigint(20)", fieldname = "senderCid", comment = "发送玩家CID")
    private long senderCid;

    public static final int FIELD_gainedCidList =4;
    @DataBaseField(type = "varbinary(1024)", fieldname = "gainedCidList", comment = "领取玩家CID列表")
    private byte[] gainedCidList;

    public UsBoxBO() {
        id = 0;
        instanceId = 0L;
        refId = 0L;
        createdAt = 0L;
        senderCid = 0L;
        gainedCidList = null;
    }

    public UsBoxBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instanceId = rs.getLong(2);
        refId = rs.getLong(3);
        createdAt = rs.getLong(4);
        senderCid = rs.getLong(5);
        gainedCidList = rs.getBytes(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsBoxBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instanceId`, `refId`, `createdAt`, `senderCid`, `gainedCidList`";
    }

    @Override
    public String getTableName() {
        return "`us_box`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(refId).append("', ");
        strBuf.append("'").append(createdAt).append("', ");
        strBuf.append("'").append(senderCid).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(gainedCidList);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_gainedCidList)) ret.add(gainedCidList);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 宝箱实例ID
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

    // 宝箱配置ID
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

    // 创建时间（毫秒）
    public long getCreatedAt() { return this.createdAt; }
    public void setCreatedAt(BM _bm, long createdAt) {
        if(createdAt==this.createdAt) 
            return;
        this.createdAt = createdAt; 
        markField(_bm, FIELD_createdAt); 
    }
    public void saveCreatedAt(BM _bm, long createdAt) {
        if(createdAt==this.createdAt) 
            return;
        this.createdAt = createdAt;
        saveField(_bm, "createdAt", createdAt);
    }

    // 发送玩家CID
    public long getSenderCid() { return this.senderCid; }
    public void setSenderCid(BM _bm, long senderCid) {
        if(senderCid==this.senderCid) 
            return;
        this.senderCid = senderCid; 
        markField(_bm, FIELD_senderCid); 
    }
    public void saveSenderCid(BM _bm, long senderCid) {
        if(senderCid==this.senderCid) 
            return;
        this.senderCid = senderCid;
        saveField(_bm, "senderCid", senderCid);
    }

    // 领取玩家CID列表
    public byte[] getGainedCidList() { return this.gainedCidList; }
    public void setGainedCidList(BM _bm, byte[] gainedCidList) {
        if(gainedCidList==this.gainedCidList) 
            return;
        this.gainedCidList = gainedCidList; 
        markField(_bm, FIELD_gainedCidList); 
    }
    public void saveGainedCidList(BM _bm, byte[] gainedCidList) {
        if(gainedCidList==this.gainedCidList) 
            return;
        this.gainedCidList = gainedCidList;
        saveFieldBytes(_bm, "gainedCidList", gainedCidList);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `refId` = '").append(refId).append("',");
        sBuilder.append(" `createdAt` = '").append(createdAt).append("',");
        sBuilder.append(" `senderCid` = '").append(senderCid).append("',");
        sBuilder.append(" `gainedCidList` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_refId)) sBuilder.append(" `refId` = '").append(refId).append("',");
        if(isFieldMarked(FIELD_createdAt)) sBuilder.append(" `createdAt` = '").append(createdAt).append("',");
        if(isFieldMarked(FIELD_senderCid)) sBuilder.append(" `senderCid` = '").append(senderCid).append("',");
        if(isFieldMarked(FIELD_gainedCidList)) sBuilder.append(" `gainedCidList` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_box` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '宝箱实例ID',"
                + "`refId` bigint(20) NOT NULL DEFAULT '0' COMMENT '宝箱配置ID',"
                + "`createdAt` bigint(20) NOT NULL DEFAULT '0' COMMENT '创建时间（毫秒）',"
                + "`senderCid` bigint(20) NOT NULL DEFAULT '0' COMMENT '发送玩家CID',"
                + "`gainedCidList` varbinary(1024) NOT NULL DEFAULT '' COMMENT '领取玩家CID列表',"
                + "UNIQUE INDEX `instanceId` (`instanceId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='User Info 通用宝箱数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//instanceId
        _size+=8;//refId
        _size+=8;//createdAt
        _size+=8;//senderCid
        _size+=2;_size+=gainedCidList.length;//gainedCidList
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instanceId);
        buff.putLong(refId);
        buff.putLong(createdAt);
        buff.putLong(senderCid);
        buff.putShort((short)(gainedCidList == null ? 0 : gainedCidList.length));if(null != gainedCidList){buff.put(gainedCidList);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instanceId=buff.getLong();
        refId=buff.getLong();
        createdAt=buff.getLong();
        senderCid=buff.getLong();
        int gainedCidList_count = buff.getShort();if(gainedCidList_count>0){gainedCidList = new byte[gainedCidList_count];buff.get(gainedCidList);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
