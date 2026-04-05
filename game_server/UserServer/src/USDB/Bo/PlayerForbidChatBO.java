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
public class PlayerForbidChatBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家账号ID")
    private long cid;

    public static final int FIELD_endMs =1;
    @DataBaseField(type = "bigint(20)", fieldname = "endMs", comment = "禁言截至时间戳（毫秒）")
    private long endMs;

    public static final int FIELD_roomType =2;
    @DataBaseField(type = "int(11)", fieldname = "roomType", comment = "聊天房间 0-全部")
    private int roomType;

    public PlayerForbidChatBO() {
        id = 0;
        cid = 0L;
        endMs = 0L;
        roomType = 0;
    }

    public PlayerForbidChatBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        endMs = rs.getLong(3);
        roomType = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerForbidChatBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `endMs`, `roomType`";
    }

    @Override
    public String getTableName() {
        return "`player_forbid_chat`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(endMs).append("', ");
        strBuf.append("'").append(roomType).append("', ");
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

    // 玩家账号ID
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

    // 禁言截至时间戳（毫秒）
    public long getEndMs() { return this.endMs; }
    public void setEndMs(BM _bm, long endMs) {
        if(endMs==this.endMs) 
            return;
        this.endMs = endMs; 
        markField(_bm, FIELD_endMs); 
    }
    public void saveEndMs(BM _bm, long endMs) {
        if(endMs==this.endMs) 
            return;
        this.endMs = endMs;
        saveField(_bm, "endMs", endMs);
    }

    // 聊天房间 0-全部
    public int getRoomType() { return this.roomType; }
    public void setRoomType(BM _bm, int roomType) {
        if(roomType==this.roomType) 
            return;
        this.roomType = roomType; 
        markField(_bm, FIELD_roomType); 
    }
    public void saveRoomType(BM _bm, int roomType) {
        if(roomType==this.roomType) 
            return;
        this.roomType = roomType;
        saveField(_bm, "roomType", roomType);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `endMs` = '").append(endMs).append("',");
        sBuilder.append(" `roomType` = '").append(roomType).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_endMs)) sBuilder.append(" `endMs` = '").append(endMs).append("',");
        if(isFieldMarked(FIELD_roomType)) sBuilder.append(" `roomType` = '").append(roomType).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_forbid_chat` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家账号ID',"
                + "`endMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '禁言截至时间戳（毫秒）',"
                + "`roomType` int(11) NOT NULL DEFAULT '0' COMMENT '聊天房间 0-全部',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家禁言数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//endMs
        _size+=4;//roomType
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(endMs);
        buff.putInt(roomType);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        endMs=buff.getLong();
        roomType=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
