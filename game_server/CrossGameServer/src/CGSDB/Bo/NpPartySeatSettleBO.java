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
public class NpPartySeatSettleBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instanceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "聚会实例ID")
    private long instanceId;

    public static final int FIELD_seatIdx =1;
    @DataBaseField(type = "int(11)", fieldname = "seatIdx", comment = "座位下标")
    private int seatIdx;

    public static final int FIELD_continuedSecs =2;
    @DataBaseField(type = "int(11)", fieldname = "continuedSecs", comment = "持续时长（秒）")
    private int continuedSecs;

    public NpPartySeatSettleBO() {
        id = 0;
        instanceId = 0L;
        seatIdx = 0;
        continuedSecs = 0;
    }

    public NpPartySeatSettleBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instanceId = rs.getLong(2);
        seatIdx = rs.getInt(3);
        continuedSecs = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpPartySeatSettleBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instanceId`, `seatIdx`, `continuedSecs`";
    }

    @Override
    public String getTableName() {
        return "`np_party_seat_settle`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(seatIdx).append("', ");
        strBuf.append("'").append(continuedSecs).append("', ");
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

    // 座位下标
    public int getSeatIdx() { return this.seatIdx; }
    public void setSeatIdx(BM _bm, int seatIdx) {
        if(seatIdx==this.seatIdx) 
            return;
        this.seatIdx = seatIdx; 
        markField(_bm, FIELD_seatIdx); 
    }
    public void saveSeatIdx(BM _bm, int seatIdx) {
        if(seatIdx==this.seatIdx) 
            return;
        this.seatIdx = seatIdx;
        saveField(_bm, "seatIdx", seatIdx);
    }

    // 持续时长（秒）
    public int getContinuedSecs() { return this.continuedSecs; }
    public void setContinuedSecs(BM _bm, int continuedSecs) {
        if(continuedSecs==this.continuedSecs) 
            return;
        this.continuedSecs = continuedSecs; 
        markField(_bm, FIELD_continuedSecs); 
    }
    public void saveContinuedSecs(BM _bm, int continuedSecs) {
        if(continuedSecs==this.continuedSecs) 
            return;
        this.continuedSecs = continuedSecs;
        saveField(_bm, "continuedSecs", continuedSecs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `seatIdx` = '").append(seatIdx).append("',");
        sBuilder.append(" `continuedSecs` = '").append(continuedSecs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_seatIdx)) sBuilder.append(" `seatIdx` = '").append(seatIdx).append("',");
        if(isFieldMarked(FIELD_continuedSecs)) sBuilder.append(" `continuedSecs` = '").append(continuedSecs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_party_seat_settle` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '聚会实例ID',"
                + "`seatIdx` int(11) NOT NULL DEFAULT '0' COMMENT '座位下标',"
                + "`continuedSecs` int(11) NOT NULL DEFAULT '0' COMMENT '持续时长（秒）',"
                + "KEY `instanceId` (`instanceId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Party 聚会座位结算数据表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//seatIdx
        _size+=4;//continuedSecs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instanceId);
        buff.putInt(seatIdx);
        buff.putInt(continuedSecs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instanceId=buff.getLong();
        seatIdx=buff.getInt();
        continuedSecs=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
