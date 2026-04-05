package ShareCodeDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;
import NPCommon.DB.Annotation.RefBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

@RefBo(isIdAuto = false)
public class ClothesShareDataBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_serial =0;
    @DataBaseField(type = "bigint(20)", fieldname = "serial", comment = "实际分享id")
    private long serial;

    public static final int FIELD_data =1;
    @DataBaseField(type = "blob", fieldname = "data", comment = "数据")
    private byte[] data;

    public static final int FIELD_timestamp =2;
    @DataBaseField(type = "bigint(20)", fieldname = "timestamp", comment = "生成时间戳")
    private long timestamp;

    public ClothesShareDataBO() {
        id = 0;
        serial = 0L;
        data = null;
        timestamp = 0L;
    }

    public ClothesShareDataBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        serial = rs.getLong(2);
        data = rs.getBytes(3);
        timestamp = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ClothesShareDataBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `serial`, `data`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`clothes_share_data`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(serial).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(timestamp).append("', ");
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

    // 实际分享id
    public long getSerial() { return this.serial; }
    public void setSerial(BM _bm, long serial) {
        if(serial==this.serial) 
            return;
        this.serial = serial; 
        markField(_bm, FIELD_serial); 
    }
    public void saveSerial(BM _bm, long serial) {
        if(serial==this.serial) 
            return;
        this.serial = serial;
        saveField(_bm, "serial", serial);
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

    // 生成时间戳
    public long getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, long timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, long timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `serial` = '").append(serial).append("',");
        sBuilder.append(" `data` = ?,");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_serial)) sBuilder.append(" `serial` = '").append(serial).append("',");
        if(isFieldMarked(FIELD_data)) sBuilder.append(" `data` = ?,");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `clothes_share_data` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`serial` bigint(20) NOT NULL DEFAULT '0' COMMENT '实际分享id',"
                + "`data` blob NULL COMMENT '数据',"
                + "`timestamp` bigint(20) NOT NULL DEFAULT '0' COMMENT '生成时间戳',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='时装分享数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.scc_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//serial
        _size+=2;_size+=data.length;//data
        _size+=8;//timestamp
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(serial);
        buff.putShort((short)(data == null ? 0 : data.length));if(null != data){buff.put(data);}
        buff.putLong(timestamp);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        serial=buff.getLong();
        int data_count = buff.getShort();if(data_count>0){data = new byte[data_count];buff.get(data);}
        timestamp=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
