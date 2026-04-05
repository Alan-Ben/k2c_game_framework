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
public class GuildLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟id")
    private long guild_id;

    public static final int FIELD_type =1;
    @DataBaseField(type = "int(11)", fieldname = "type", comment = "日志类型")
    private int type;

    public static final int FIELD_data =2;
    @DataBaseField(type = "blob", fieldname = "data", comment = "数据")
    private byte[] data;

    public static final int FIELD_send_time_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "send_time_ms", comment = "发送时间")
    private long send_time_ms;

    public GuildLogBO() {
        id = 0;
        guild_id = 0L;
        type = 0;
        data = null;
        send_time_ms = 0L;
    }

    public GuildLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        type = rs.getInt(3);
        data = rs.getBytes(4);
        send_time_ms = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `type`, `data`, `send_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`guild_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(type).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(send_time_ms).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(data);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_data)) ret.add(data);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 联盟id
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
    }

    // 日志类型
    public int getType() { return this.type; }
    public void setType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type; 
        markField(_bm, FIELD_type); 
    }
    public void saveType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type;
        saveField(_bm, "type", type);
    }

    // 数据
    public byte[] getData() { return this.data; }
    public void setData(BM _bm, byte[] data) {
        if(data==this.data) 
            return;
        this.data = data; 
        markField(_bm, FIELD_data); 
    }
    public void saveData(BM _bm, byte[] data) {
        if(data==this.data) 
            return;
        this.data = data;
        saveFieldBytes(_bm, "data", data);
    }

    // 发送时间
    public long getSendTimeMs() { return this.send_time_ms; }
    public void setSendTimeMs(BM _bm, long send_time_ms) {
        if(send_time_ms==this.send_time_ms) 
            return;
        this.send_time_ms = send_time_ms; 
        markField(_bm, FIELD_send_time_ms); 
    }
    public void saveSendTimeMs(BM _bm, long send_time_ms) {
        if(send_time_ms==this.send_time_ms) 
            return;
        this.send_time_ms = send_time_ms;
        saveField(_bm, "send_time_ms", send_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `type` = '").append(type).append("',");
        sBuilder.append(" `data` = ?,");
        sBuilder.append(" `send_time_ms` = '").append(send_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_type)) sBuilder.append(" `type` = '").append(type).append("',");
        if(isFieldMarked(FIELD_data)) sBuilder.append(" `data` = ?,");
        if(isFieldMarked(FIELD_send_time_ms)) sBuilder.append(" `send_time_ms` = '").append(send_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟id',"
                + "`type` int(11) NOT NULL DEFAULT '0' COMMENT '日志类型',"
                + "`data` blob NULL COMMENT '数据',"
                + "`send_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '发送时间',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟日志数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guild_id
        _size+=4;//type
        _size+=2;_size+=data.length;//data
        _size+=8;//send_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putInt(type);
        buff.putShort((short)(data == null ? 0 : data.length));if(null != data){buff.put(data);}
        buff.putLong(send_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        type=buff.getInt();
        int data_count = buff.getShort();if(data_count>0){data = new byte[data_count];buff.get(data);}
        send_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
