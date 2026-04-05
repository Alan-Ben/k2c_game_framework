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
public class NpPartyJoinerBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_instanceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "instanceId", comment = "聚会实例ID")
    private long instanceId;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "赴宴玩家CID")
    private long cid;

    public static final int FIELD_seatIdx =2;
    @DataBaseField(type = "int(11)", fieldname = "seatIdx", comment = "赴宴座位下标")
    private int seatIdx;

    public static final int FIELD_joinTs =3;
    @DataBaseField(type = "int(11)", fieldname = "joinTs", comment = "赴宴时间戳")
    private int joinTs;

    public static final int FIELD_protectEndTs =4;
    @DataBaseField(type = "int(11)", fieldname = "protectEndTs", comment = "保护罩截至时间戳")
    private int protectEndTs;

    public static final int FIELD_profitEndTs =5;
    @DataBaseField(type = "int(11)", fieldname = "profitEndTs", comment = "收益结束时间戳（秒）")
    private int profitEndTs;

    public NpPartyJoinerBO() {
        id = 0;
        instanceId = 0L;
        cid = 0L;
        seatIdx = 0;
        joinTs = 0;
        protectEndTs = 0;
        profitEndTs = 0;
    }

    public NpPartyJoinerBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        instanceId = rs.getLong(2);
        cid = rs.getLong(3);
        seatIdx = rs.getInt(4);
        joinTs = rs.getInt(5);
        protectEndTs = rs.getInt(6);
        profitEndTs = rs.getInt(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpPartyJoinerBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `instanceId`, `cid`, `seatIdx`, `joinTs`, `protectEndTs`, `profitEndTs`";
    }

    @Override
    public String getTableName() {
        return "`np_party_joiner`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(instanceId).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(seatIdx).append("', ");
        strBuf.append("'").append(joinTs).append("', ");
        strBuf.append("'").append(protectEndTs).append("', ");
        strBuf.append("'").append(profitEndTs).append("', ");
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

    // 赴宴玩家CID
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

    // 赴宴座位下标
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

    // 赴宴时间戳
    public int getJoinTs() { return this.joinTs; }
    public void setJoinTs(BM _bm, int joinTs) {
        if(joinTs==this.joinTs) 
            return;
        this.joinTs = joinTs; 
        markField(_bm, FIELD_joinTs); 
    }
    public void saveJoinTs(BM _bm, int joinTs) {
        if(joinTs==this.joinTs) 
            return;
        this.joinTs = joinTs;
        saveField(_bm, "joinTs", joinTs);
    }

    // 保护罩截至时间戳
    public int getProtectEndTs() { return this.protectEndTs; }
    public void setProtectEndTs(BM _bm, int protectEndTs) {
        if(protectEndTs==this.protectEndTs) 
            return;
        this.protectEndTs = protectEndTs; 
        markField(_bm, FIELD_protectEndTs); 
    }
    public void saveProtectEndTs(BM _bm, int protectEndTs) {
        if(protectEndTs==this.protectEndTs) 
            return;
        this.protectEndTs = protectEndTs;
        saveField(_bm, "protectEndTs", protectEndTs);
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `seatIdx` = '").append(seatIdx).append("',");
        sBuilder.append(" `joinTs` = '").append(joinTs).append("',");
        sBuilder.append(" `protectEndTs` = '").append(protectEndTs).append("',");
        sBuilder.append(" `profitEndTs` = '").append(profitEndTs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_instanceId)) sBuilder.append(" `instanceId` = '").append(instanceId).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_seatIdx)) sBuilder.append(" `seatIdx` = '").append(seatIdx).append("',");
        if(isFieldMarked(FIELD_joinTs)) sBuilder.append(" `joinTs` = '").append(joinTs).append("',");
        if(isFieldMarked(FIELD_protectEndTs)) sBuilder.append(" `protectEndTs` = '").append(protectEndTs).append("',");
        if(isFieldMarked(FIELD_profitEndTs)) sBuilder.append(" `profitEndTs` = '").append(profitEndTs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_party_joiner` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`instanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '聚会实例ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '赴宴玩家CID',"
                + "`seatIdx` int(11) NOT NULL DEFAULT '0' COMMENT '赴宴座位下标',"
                + "`joinTs` int(11) NOT NULL DEFAULT '0' COMMENT '赴宴时间戳',"
                + "`protectEndTs` int(11) NOT NULL DEFAULT '0' COMMENT '保护罩截至时间戳',"
                + "`profitEndTs` int(11) NOT NULL DEFAULT '0' COMMENT '收益结束时间戳（秒）',"
                + "KEY `instanceId` (`instanceId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Party 聚会赴宴玩家数据表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=4;//seatIdx
        _size+=4;//joinTs
        _size+=4;//protectEndTs
        _size+=4;//profitEndTs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(instanceId);
        buff.putLong(cid);
        buff.putInt(seatIdx);
        buff.putInt(joinTs);
        buff.putInt(protectEndTs);
        buff.putInt(profitEndTs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        instanceId=buff.getLong();
        cid=buff.getLong();
        seatIdx=buff.getInt();
        joinTs=buff.getInt();
        protectEndTs=buff.getInt();
        profitEndTs=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
