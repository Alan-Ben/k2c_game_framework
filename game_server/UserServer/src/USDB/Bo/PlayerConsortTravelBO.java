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
public class PlayerConsortTravelBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_travelId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "travelId", comment = "家人游玩类型ID")
    private long travelId;

    public static final int FIELD_lastTravelMs =2;
    @DataBaseField(type = "bigint(20)", fieldname = "lastTravelMs", comment = "家人上次游玩时间")
    private long lastTravelMs;

    public static final int FIELD_travelCount =3;
    @DataBaseField(type = "int(11)", fieldname = "travelCount", comment = "家人游玩次数")
    private int travelCount;

    public PlayerConsortTravelBO() {
        id = 0;
        cid = 0L;
        travelId = 0L;
        lastTravelMs = 0L;
        travelCount = 0;
    }

    public PlayerConsortTravelBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        travelId = rs.getLong(3);
        lastTravelMs = rs.getLong(4);
        travelCount = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerConsortTravelBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `travelId`, `lastTravelMs`, `travelCount`";
    }

    @Override
    public String getTableName() {
        return "`player_consort_travel`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(travelId).append("', ");
        strBuf.append("'").append(lastTravelMs).append("', ");
        strBuf.append("'").append(travelCount).append("', ");
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

    // 家人游玩类型ID
    public long getTravelId() { return this.travelId; }
    public void setTravelId(BM _bm, long travelId) {
        if(travelId==this.travelId) 
            return;
        this.travelId = travelId; 
        markField(_bm, FIELD_travelId); 
    }
    public void saveTravelId(BM _bm, long travelId) {
        if(travelId==this.travelId) 
            return;
        this.travelId = travelId;
        saveField(_bm, "travelId", travelId);
    }

    // 家人上次游玩时间
    public long getLastTravelMs() { return this.lastTravelMs; }
    public void setLastTravelMs(BM _bm, long lastTravelMs) {
        if(lastTravelMs==this.lastTravelMs) 
            return;
        this.lastTravelMs = lastTravelMs; 
        markField(_bm, FIELD_lastTravelMs); 
    }
    public void saveLastTravelMs(BM _bm, long lastTravelMs) {
        if(lastTravelMs==this.lastTravelMs) 
            return;
        this.lastTravelMs = lastTravelMs;
        saveField(_bm, "lastTravelMs", lastTravelMs);
    }

    // 家人游玩次数
    public int getTravelCount() { return this.travelCount; }
    public void setTravelCount(BM _bm, int travelCount) {
        if(travelCount==this.travelCount) 
            return;
        this.travelCount = travelCount; 
        markField(_bm, FIELD_travelCount); 
    }
    public void saveTravelCount(BM _bm, int travelCount) {
        if(travelCount==this.travelCount) 
            return;
        this.travelCount = travelCount;
        saveField(_bm, "travelCount", travelCount);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `travelId` = '").append(travelId).append("',");
        sBuilder.append(" `lastTravelMs` = '").append(lastTravelMs).append("',");
        sBuilder.append(" `travelCount` = '").append(travelCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_travelId)) sBuilder.append(" `travelId` = '").append(travelId).append("',");
        if(isFieldMarked(FIELD_lastTravelMs)) sBuilder.append(" `lastTravelMs` = '").append(lastTravelMs).append("',");
        if(isFieldMarked(FIELD_travelCount)) sBuilder.append(" `travelCount` = '").append(travelCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_consort_travel` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`travelId` bigint(20) NOT NULL DEFAULT '0' COMMENT '家人游玩类型ID',"
                + "`lastTravelMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '家人上次游玩时间',"
                + "`travelCount` int(11) NOT NULL DEFAULT '0' COMMENT '家人游玩次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家家人出游数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//travelId
        _size+=8;//lastTravelMs
        _size+=4;//travelCount
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(travelId);
        buff.putLong(lastTravelMs);
        buff.putInt(travelCount);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        travelId=buff.getLong();
        lastTravelMs=buff.getLong();
        travelCount=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
