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
public class PlayerTravelCanDealBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_eventId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "eventId", comment = "事件ID")
    private long eventId;

    public static final int FIELD_posId =2;
    @DataBaseField(type = "bigint(20)", fieldname = "posId", comment = "位置ID")
    private long posId;

    public PlayerTravelCanDealBO() {
        id = 0;
        cid = 0L;
        eventId = 0L;
        posId = 0L;
    }

    public PlayerTravelCanDealBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        eventId = rs.getLong(3);
        posId = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTravelCanDealBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `eventId`, `posId`";
    }

    @Override
    public String getTableName() {
        return "`player_travel_can_deal`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(eventId).append("', ");
        strBuf.append("'").append(posId).append("', ");
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

    // 事件ID
    public long getEventId() { return this.eventId; }
    public void setEventId(BM _bm, long eventId) {
        if(eventId==this.eventId) 
            return;
        this.eventId = eventId; 
        markField(_bm, FIELD_eventId); 
    }
    public void saveEventId(BM _bm, long eventId) {
        if(eventId==this.eventId) 
            return;
        this.eventId = eventId;
        saveField(_bm, "eventId", eventId);
    }

    // 位置ID
    public long getPosId() { return this.posId; }
    public void setPosId(BM _bm, long posId) {
        if(posId==this.posId) 
            return;
        this.posId = posId; 
        markField(_bm, FIELD_posId); 
    }
    public void savePosId(BM _bm, long posId) {
        if(posId==this.posId) 
            return;
        this.posId = posId;
        saveField(_bm, "posId", posId);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `eventId` = '").append(eventId).append("',");
        sBuilder.append(" `posId` = '").append(posId).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_eventId)) sBuilder.append(" `eventId` = '").append(eventId).append("',");
        if(isFieldMarked(FIELD_posId)) sBuilder.append(" `posId` = '").append(posId).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_travel_can_deal` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`eventId` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件ID',"
                + "`posId` bigint(20) NOT NULL DEFAULT '0' COMMENT '位置ID',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家游历待处理数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//eventId
        _size+=8;//posId
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(eventId);
        buff.putLong(posId);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        eventId=buff.getLong();
        posId=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
