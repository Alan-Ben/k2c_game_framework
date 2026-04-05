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
public class PlayerTravelBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_finishedOnceEvents =1;
    @DataBaseField(type = "blob", fieldname = "finishedOnceEvents", comment = "已完成的一次性事件列表")
    private byte[] finishedOnceEvents;

    public static final int FIELD_finishedEarlyEvents =2;
    @DataBaseField(type = "blob", fieldname = "finishedEarlyEvents", comment = "已完成的前置事件列表")
    private byte[] finishedEarlyEvents;

    public static final int FIELD_isFinishedAllEarlyEvents =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "isFinishedAllEarlyEvents", comment = "已完成的所有前置事件列表")
    private boolean isFinishedAllEarlyEvents;

    public static final int FIELD_lastRandPosId =4;
    @DataBaseField(type = "bigint(20)", fieldname = "lastRandPosId", comment = "上次随机到的游历位置ID")
    private long lastRandPosId;

    public PlayerTravelBO() {
        id = 0;
        cid = 0L;
        finishedOnceEvents = null;
        finishedEarlyEvents = null;
        isFinishedAllEarlyEvents = false;
        lastRandPosId = 0L;
    }

    public PlayerTravelBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        finishedOnceEvents = rs.getBytes(3);
        finishedEarlyEvents = rs.getBytes(4);
        isFinishedAllEarlyEvents = rs.getBoolean(5);
        lastRandPosId = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTravelBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `finishedOnceEvents`, `finishedEarlyEvents`, `isFinishedAllEarlyEvents`, `lastRandPosId`";
    }

    @Override
    public String getTableName() {
        return "`player_travel`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("'").append(isFinishedAllEarlyEvents ? 1 : 0).append("', ");
        strBuf.append("'").append(lastRandPosId).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(finishedOnceEvents); 
        ret.add(finishedEarlyEvents);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_finishedOnceEvents)) ret.add(finishedOnceEvents); 
        if(isFieldMarked(FIELD_finishedEarlyEvents)) ret.add(finishedEarlyEvents);         return ret;
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

    // 已完成的一次性事件列表
    public byte[] getFinishedOnceEvents() { return this.finishedOnceEvents; }
    public void setFinishedOnceEvents(BM _bm, byte[] finishedOnceEvents) {
        if(finishedOnceEvents==this.finishedOnceEvents) 
            return;
        this.finishedOnceEvents = finishedOnceEvents; 
        markField(_bm, FIELD_finishedOnceEvents); 
    }
    public void saveFinishedOnceEvents(BM _bm, byte[] finishedOnceEvents) {
        if(finishedOnceEvents==this.finishedOnceEvents) 
            return;
        this.finishedOnceEvents = finishedOnceEvents;
        saveFieldBytes(_bm, "finishedOnceEvents", finishedOnceEvents);
    }

    // 已完成的前置事件列表
    public byte[] getFinishedEarlyEvents() { return this.finishedEarlyEvents; }
    public void setFinishedEarlyEvents(BM _bm, byte[] finishedEarlyEvents) {
        if(finishedEarlyEvents==this.finishedEarlyEvents) 
            return;
        this.finishedEarlyEvents = finishedEarlyEvents; 
        markField(_bm, FIELD_finishedEarlyEvents); 
    }
    public void saveFinishedEarlyEvents(BM _bm, byte[] finishedEarlyEvents) {
        if(finishedEarlyEvents==this.finishedEarlyEvents) 
            return;
        this.finishedEarlyEvents = finishedEarlyEvents;
        saveFieldBytes(_bm, "finishedEarlyEvents", finishedEarlyEvents);
    }

    // 已完成的所有前置事件列表
    public boolean getIsFinishedAllEarlyEvents() { return this.isFinishedAllEarlyEvents; }
    public void setIsFinishedAllEarlyEvents(BM _bm, boolean isFinishedAllEarlyEvents) {
        if(isFinishedAllEarlyEvents==this.isFinishedAllEarlyEvents) 
            return;
        this.isFinishedAllEarlyEvents = isFinishedAllEarlyEvents; 
        markField(_bm, FIELD_isFinishedAllEarlyEvents); 
    }
    public void saveIsFinishedAllEarlyEvents(BM _bm, boolean isFinishedAllEarlyEvents) {
        if(isFinishedAllEarlyEvents==this.isFinishedAllEarlyEvents) 
            return;
        this.isFinishedAllEarlyEvents = isFinishedAllEarlyEvents;
        saveField(_bm, "isFinishedAllEarlyEvents", isFinishedAllEarlyEvents ? 1 : 0);
    }

    // 上次随机到的游历位置ID
    public long getLastRandPosId() { return this.lastRandPosId; }
    public void setLastRandPosId(BM _bm, long lastRandPosId) {
        if(lastRandPosId==this.lastRandPosId) 
            return;
        this.lastRandPosId = lastRandPosId; 
        markField(_bm, FIELD_lastRandPosId); 
    }
    public void saveLastRandPosId(BM _bm, long lastRandPosId) {
        if(lastRandPosId==this.lastRandPosId) 
            return;
        this.lastRandPosId = lastRandPosId;
        saveField(_bm, "lastRandPosId", lastRandPosId);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `finishedOnceEvents` = ?,");
        sBuilder.append(" `finishedEarlyEvents` = ?,");
        sBuilder.append(" `isFinishedAllEarlyEvents` = '").append(isFinishedAllEarlyEvents ? 1 : 0).append("',");
        sBuilder.append(" `lastRandPosId` = '").append(lastRandPosId).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_finishedOnceEvents)) sBuilder.append(" `finishedOnceEvents` = ?,");
        if(isFieldMarked(FIELD_finishedEarlyEvents)) sBuilder.append(" `finishedEarlyEvents` = ?,");
        if(isFieldMarked(FIELD_isFinishedAllEarlyEvents)) sBuilder.append(" `isFinishedAllEarlyEvents` = '").append(isFinishedAllEarlyEvents ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_lastRandPosId)) sBuilder.append(" `lastRandPosId` = '").append(lastRandPosId).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_travel` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`finishedOnceEvents` blob NULL COMMENT '已完成的一次性事件列表',"
                + "`finishedEarlyEvents` blob NULL COMMENT '已完成的前置事件列表',"
                + "`isFinishedAllEarlyEvents` tinyint(1) NOT NULL DEFAULT '0' COMMENT '已完成的所有前置事件列表',"
                + "`lastRandPosId` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次随机到的游历位置ID',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家游历数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=2;_size+=finishedOnceEvents.length;//finishedOnceEvents
        _size+=2;_size+=finishedEarlyEvents.length;//finishedEarlyEvents
        _size+=1;//isFinishedAllEarlyEvents
        _size+=8;//lastRandPosId
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putShort((short)(finishedOnceEvents == null ? 0 : finishedOnceEvents.length));if(null != finishedOnceEvents){buff.put(finishedOnceEvents);}
        buff.putShort((short)(finishedEarlyEvents == null ? 0 : finishedEarlyEvents.length));if(null != finishedEarlyEvents){buff.put(finishedEarlyEvents);}
        buff.put((byte)(isFinishedAllEarlyEvents?1:0));
        buff.putLong(lastRandPosId);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        int finishedOnceEvents_count = buff.getShort();if(finishedOnceEvents_count>0){finishedOnceEvents = new byte[finishedOnceEvents_count];buff.get(finishedOnceEvents);}
        int finishedEarlyEvents_count = buff.getShort();if(finishedEarlyEvents_count>0){finishedEarlyEvents = new byte[finishedEarlyEvents_count];buff.get(finishedEarlyEvents);}
        isFinishedAllEarlyEvents=(buff.get()==1);
        lastRandPosId=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
