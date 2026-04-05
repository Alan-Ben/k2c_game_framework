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
public class PlayerMarsExploreEventBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_exploreLvl =1;
    @DataBaseField(type = "int(11)", fieldname = "exploreLvl", comment = "探索等级")
    private int exploreLvl;

    public static final int FIELD_eventType =2;
    @DataBaseField(type = "int(11)", fieldname = "eventType", comment = "事件类型")
    private int eventType;

    public static final int FIELD_eventId =3;
    @DataBaseField(type = "bigint(20)", fieldname = "eventId", comment = "事件ID")
    private long eventId;

    public static final int FIELD_quality =4;
    @DataBaseField(type = "int(11)", fieldname = "quality", comment = "事件品质")
    private int quality;

    public static final int FIELD_pos =5;
    @DataBaseField(type = "bigint(20)", fieldname = "pos", comment = "事件位置")
    private long pos;

    public static final int FIELD_createdMs =6;
    @DataBaseField(type = "bigint(20)", fieldname = "createdMs", comment = "事件创建时间（毫秒）")
    private long createdMs;

    public static final int FIELD_isDone =7;
    @DataBaseField(type = "tinyint(1)", fieldname = "isDone", comment = "是否完成")
    private boolean isDone;

    public static final int FIELD_extData =8;
    @DataBaseField(type = "blob", fieldname = "extData", comment = "额外数据")
    private byte[] extData;

    public PlayerMarsExploreEventBO() {
        id = 0;
        cid = 0L;
        exploreLvl = 0;
        eventType = 0;
        eventId = 0L;
        quality = 0;
        pos = 0L;
        createdMs = 0L;
        isDone = false;
        extData = null;
    }

    public PlayerMarsExploreEventBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        exploreLvl = rs.getInt(3);
        eventType = rs.getInt(4);
        eventId = rs.getLong(5);
        quality = rs.getInt(6);
        pos = rs.getLong(7);
        createdMs = rs.getLong(8);
        isDone = rs.getBoolean(9);
        extData = rs.getBytes(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsExploreEventBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `exploreLvl`, `eventType`, `eventId`, `quality`, `pos`, `createdMs`, `isDone`, `extData`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_explore_event`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(exploreLvl).append("', ");
        strBuf.append("'").append(eventType).append("', ");
        strBuf.append("'").append(eventId).append("', ");
        strBuf.append("'").append(quality).append("', ");
        strBuf.append("'").append(pos).append("', ");
        strBuf.append("'").append(createdMs).append("', ");
        strBuf.append("'").append(isDone ? 1 : 0).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(extData);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_extData)) ret.add(extData);         return ret;
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

    // 探索等级
    public int getExploreLvl() { return this.exploreLvl; }
    public void setExploreLvl(BM _bm, int exploreLvl) {
        if(exploreLvl==this.exploreLvl) 
            return;
        this.exploreLvl = exploreLvl; 
        markField(_bm, FIELD_exploreLvl); 
    }
    public void saveExploreLvl(BM _bm, int exploreLvl) {
        if(exploreLvl==this.exploreLvl) 
            return;
        this.exploreLvl = exploreLvl;
        saveField(_bm, "exploreLvl", exploreLvl);
    }

    // 事件类型
    public int getEventType() { return this.eventType; }
    public void setEventType(BM _bm, int eventType) {
        if(eventType==this.eventType) 
            return;
        this.eventType = eventType; 
        markField(_bm, FIELD_eventType); 
    }
    public void saveEventType(BM _bm, int eventType) {
        if(eventType==this.eventType) 
            return;
        this.eventType = eventType;
        saveField(_bm, "eventType", eventType);
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

    // 事件品质
    public int getQuality() { return this.quality; }
    public void setQuality(BM _bm, int quality) {
        if(quality==this.quality) 
            return;
        this.quality = quality; 
        markField(_bm, FIELD_quality); 
    }
    public void saveQuality(BM _bm, int quality) {
        if(quality==this.quality) 
            return;
        this.quality = quality;
        saveField(_bm, "quality", quality);
    }

    // 事件位置
    public long getPos() { return this.pos; }
    public void setPos(BM _bm, long pos) {
        if(pos==this.pos) 
            return;
        this.pos = pos; 
        markField(_bm, FIELD_pos); 
    }
    public void savePos(BM _bm, long pos) {
        if(pos==this.pos) 
            return;
        this.pos = pos;
        saveField(_bm, "pos", pos);
    }

    // 事件创建时间（毫秒）
    public long getCreatedMs() { return this.createdMs; }
    public void setCreatedMs(BM _bm, long createdMs) {
        if(createdMs==this.createdMs) 
            return;
        this.createdMs = createdMs; 
        markField(_bm, FIELD_createdMs); 
    }
    public void saveCreatedMs(BM _bm, long createdMs) {
        if(createdMs==this.createdMs) 
            return;
        this.createdMs = createdMs;
        saveField(_bm, "createdMs", createdMs);
    }

    // 是否完成
    public boolean getIsDone() { return this.isDone; }
    public void setIsDone(BM _bm, boolean isDone) {
        if(isDone==this.isDone) 
            return;
        this.isDone = isDone; 
        markField(_bm, FIELD_isDone); 
    }
    public void saveIsDone(BM _bm, boolean isDone) {
        if(isDone==this.isDone) 
            return;
        this.isDone = isDone;
        saveField(_bm, "isDone", isDone ? 1 : 0);
    }

    // 额外数据
    public byte[] getExtData() { return this.extData; }
    public void setExtData(BM _bm, byte[] extData) {
        if(extData==this.extData) 
            return;
        this.extData = extData; 
        markField(_bm, FIELD_extData); 
    }
    public void saveExtData(BM _bm, byte[] extData) {
        if(extData==this.extData) 
            return;
        this.extData = extData;
        saveFieldBytes(_bm, "extData", extData);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `exploreLvl` = '").append(exploreLvl).append("',");
        sBuilder.append(" `eventType` = '").append(eventType).append("',");
        sBuilder.append(" `eventId` = '").append(eventId).append("',");
        sBuilder.append(" `quality` = '").append(quality).append("',");
        sBuilder.append(" `pos` = '").append(pos).append("',");
        sBuilder.append(" `createdMs` = '").append(createdMs).append("',");
        sBuilder.append(" `isDone` = '").append(isDone ? 1 : 0).append("',");
        sBuilder.append(" `extData` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_exploreLvl)) sBuilder.append(" `exploreLvl` = '").append(exploreLvl).append("',");
        if(isFieldMarked(FIELD_eventType)) sBuilder.append(" `eventType` = '").append(eventType).append("',");
        if(isFieldMarked(FIELD_eventId)) sBuilder.append(" `eventId` = '").append(eventId).append("',");
        if(isFieldMarked(FIELD_quality)) sBuilder.append(" `quality` = '").append(quality).append("',");
        if(isFieldMarked(FIELD_pos)) sBuilder.append(" `pos` = '").append(pos).append("',");
        if(isFieldMarked(FIELD_createdMs)) sBuilder.append(" `createdMs` = '").append(createdMs).append("',");
        if(isFieldMarked(FIELD_isDone)) sBuilder.append(" `isDone` = '").append(isDone ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_extData)) sBuilder.append(" `extData` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_explore_event` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`exploreLvl` int(11) NOT NULL DEFAULT '0' COMMENT '探索等级',"
                + "`eventType` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`eventId` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件ID',"
                + "`quality` int(11) NOT NULL DEFAULT '0' COMMENT '事件品质',"
                + "`pos` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件位置',"
                + "`createdMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件创建时间（毫秒）',"
                + "`isDone` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否完成',"
                + "`extData` blob NULL COMMENT '额外数据',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='火星探索-火星探索事件数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//exploreLvl
        _size+=4;//eventType
        _size+=8;//eventId
        _size+=4;//quality
        _size+=8;//pos
        _size+=8;//createdMs
        _size+=1;//isDone
        _size+=2;_size+=extData.length;//extData
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(exploreLvl);
        buff.putInt(eventType);
        buff.putLong(eventId);
        buff.putInt(quality);
        buff.putLong(pos);
        buff.putLong(createdMs);
        buff.put((byte)(isDone?1:0));
        buff.putShort((short)(extData == null ? 0 : extData.length));if(null != extData){buff.put(extData);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        exploreLvl=buff.getInt();
        eventType=buff.getInt();
        eventId=buff.getLong();
        quality=buff.getInt();
        pos=buff.getLong();
        createdMs=buff.getLong();
        isDone=(buff.get()==1);
        int extData_count = buff.getShort();if(extData_count>0){extData = new byte[extData_count];buff.get(extData);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
