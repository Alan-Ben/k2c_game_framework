package ISDB.Bo;
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
public class ChatRoomBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_server_type =0;
    @DataBaseField(type = "int(11)", fieldname = "server_type", comment = "聊天房间所在服务器ID")
    private int server_type;

    public static final int FIELD_server_type_id =1;
    @DataBaseField(type = "int(11)", fieldname = "server_type_id", comment = "聊天房间所在服务器类型ID")
    private int server_type_id;

    public static final int FIELD_room_type =2;
    @DataBaseField(type = "int(11)", fieldname = "room_type", comment = "房间类型")
    private int room_type;

    public static final int FIELD_room_type_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "room_type_id", comment = "房间类型ID（比如联盟聊天中的联盟ID）")
    private long room_type_id;

    public static final int FIELD_sdk_room_id =4;
    @DataBaseField(type = "int(11)", fieldname = "sdk_room_id", comment = "聊天服务器赋予的聊天房间唯一ID")
    private int sdk_room_id;

    public ChatRoomBO() {
        id = 0;
        server_type = 0;
        server_type_id = 0;
        room_type = 0;
        room_type_id = 0L;
        sdk_room_id = 0;
    }

    public ChatRoomBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        server_type = rs.getInt(2);
        server_type_id = rs.getInt(3);
        room_type = rs.getInt(4);
        room_type_id = rs.getLong(5);
        sdk_room_id = rs.getInt(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ChatRoomBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `server_type`, `server_type_id`, `room_type`, `room_type_id`, `sdk_room_id`";
    }

    @Override
    public String getTableName() {
        return "`chat_room`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(server_type).append("', ");
        strBuf.append("'").append(server_type_id).append("', ");
        strBuf.append("'").append(room_type).append("', ");
        strBuf.append("'").append(room_type_id).append("', ");
        strBuf.append("'").append(sdk_room_id).append("', ");
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

    // 聊天房间所在服务器ID
    public int getServerType() { return this.server_type; }
    public void setServerType(BM _bm, int server_type) {
        if(server_type==this.server_type) 
            return;
        this.server_type = server_type; 
        markField(_bm, FIELD_server_type); 
    }
    public void saveServerType(BM _bm, int server_type) {
        if(server_type==this.server_type) 
            return;
        this.server_type = server_type;
        saveField(_bm, "server_type", server_type);
    }

    // 聊天房间所在服务器类型ID
    public int getServerTypeId() { return this.server_type_id; }
    public void setServerTypeId(BM _bm, int server_type_id) {
        if(server_type_id==this.server_type_id) 
            return;
        this.server_type_id = server_type_id; 
        markField(_bm, FIELD_server_type_id); 
    }
    public void saveServerTypeId(BM _bm, int server_type_id) {
        if(server_type_id==this.server_type_id) 
            return;
        this.server_type_id = server_type_id;
        saveField(_bm, "server_type_id", server_type_id);
    }

    // 房间类型
    public int getRoomType() { return this.room_type; }
    public void setRoomType(BM _bm, int room_type) {
        if(room_type==this.room_type) 
            return;
        this.room_type = room_type; 
        markField(_bm, FIELD_room_type); 
    }
    public void saveRoomType(BM _bm, int room_type) {
        if(room_type==this.room_type) 
            return;
        this.room_type = room_type;
        saveField(_bm, "room_type", room_type);
    }

    // 房间类型ID（比如联盟聊天中的联盟ID）
    public long getRoomTypeId() { return this.room_type_id; }
    public void setRoomTypeId(BM _bm, long room_type_id) {
        if(room_type_id==this.room_type_id) 
            return;
        this.room_type_id = room_type_id; 
        markField(_bm, FIELD_room_type_id); 
    }
    public void saveRoomTypeId(BM _bm, long room_type_id) {
        if(room_type_id==this.room_type_id) 
            return;
        this.room_type_id = room_type_id;
        saveField(_bm, "room_type_id", room_type_id);
    }

    // 聊天服务器赋予的聊天房间唯一ID
    public int getSdkRoomId() { return this.sdk_room_id; }
    public void setSdkRoomId(BM _bm, int sdk_room_id) {
        if(sdk_room_id==this.sdk_room_id) 
            return;
        this.sdk_room_id = sdk_room_id; 
        markField(_bm, FIELD_sdk_room_id); 
    }
    public void saveSdkRoomId(BM _bm, int sdk_room_id) {
        if(sdk_room_id==this.sdk_room_id) 
            return;
        this.sdk_room_id = sdk_room_id;
        saveField(_bm, "sdk_room_id", sdk_room_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `server_type` = '").append(server_type).append("',");
        sBuilder.append(" `server_type_id` = '").append(server_type_id).append("',");
        sBuilder.append(" `room_type` = '").append(room_type).append("',");
        sBuilder.append(" `room_type_id` = '").append(room_type_id).append("',");
        sBuilder.append(" `sdk_room_id` = '").append(sdk_room_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_server_type)) sBuilder.append(" `server_type` = '").append(server_type).append("',");
        if(isFieldMarked(FIELD_server_type_id)) sBuilder.append(" `server_type_id` = '").append(server_type_id).append("',");
        if(isFieldMarked(FIELD_room_type)) sBuilder.append(" `room_type` = '").append(room_type).append("',");
        if(isFieldMarked(FIELD_room_type_id)) sBuilder.append(" `room_type_id` = '").append(room_type_id).append("',");
        if(isFieldMarked(FIELD_sdk_room_id)) sBuilder.append(" `sdk_room_id` = '").append(sdk_room_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `chat_room` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`server_type` int(11) NOT NULL DEFAULT '0' COMMENT '聊天房间所在服务器ID',"
                + "`server_type_id` int(11) NOT NULL DEFAULT '0' COMMENT '聊天房间所在服务器类型ID',"
                + "`room_type` int(11) NOT NULL DEFAULT '0' COMMENT '房间类型',"
                + "`room_type_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '房间类型ID（比如联盟聊天中的联盟ID）',"
                + "`sdk_room_id` int(11) NOT NULL DEFAULT '0' COMMENT '聊天服务器赋予的聊天房间唯一ID',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='IS 聊天房间数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.is_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//server_type
        _size+=4;//server_type_id
        _size+=4;//room_type
        _size+=8;//room_type_id
        _size+=4;//sdk_room_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(server_type);
        buff.putInt(server_type_id);
        buff.putInt(room_type);
        buff.putLong(room_type_id);
        buff.putInt(sdk_room_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        server_type=buff.getInt();
        server_type_id=buff.getInt();
        room_type=buff.getInt();
        room_type_id=buff.getLong();
        sdk_room_id=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
