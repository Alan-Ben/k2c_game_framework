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
public class UsProtocolShieldBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_main_protocol =0;
    @DataBaseField(type = "int(11)", fieldname = "main_protocol", comment = "主协议号")
    private int main_protocol;

    public static final int FIELD_sub_protocol =1;
    @DataBaseField(type = "int(11)", fieldname = "sub_protocol", comment = "副协议号")
    private int sub_protocol;

    public static final int FIELD_create_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "create_time_ms", comment = "创建时间(毫秒)")
    private long create_time_ms;

    public UsProtocolShieldBO() {
        id = 0;
        main_protocol = 0;
        sub_protocol = 0;
        create_time_ms = 0L;
    }

    public UsProtocolShieldBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        main_protocol = rs.getInt(2);
        sub_protocol = rs.getInt(3);
        create_time_ms = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsProtocolShieldBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `main_protocol`, `sub_protocol`, `create_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`us_protocol_shield`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(main_protocol).append("', ");
        strBuf.append("'").append(sub_protocol).append("', ");
        strBuf.append("'").append(create_time_ms).append("', ");
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

    // 主协议号
    public int getMainProtocol() { return this.main_protocol; }
    public void setMainProtocol(BM _bm, int main_protocol) {
        if(main_protocol==this.main_protocol) 
            return;
        this.main_protocol = main_protocol; 
        markField(_bm, FIELD_main_protocol); 
    }
    public void saveMainProtocol(BM _bm, int main_protocol) {
        if(main_protocol==this.main_protocol) 
            return;
        this.main_protocol = main_protocol;
        saveField(_bm, "main_protocol", main_protocol);
    }

    // 副协议号
    public int getSubProtocol() { return this.sub_protocol; }
    public void setSubProtocol(BM _bm, int sub_protocol) {
        if(sub_protocol==this.sub_protocol) 
            return;
        this.sub_protocol = sub_protocol; 
        markField(_bm, FIELD_sub_protocol); 
    }
    public void saveSubProtocol(BM _bm, int sub_protocol) {
        if(sub_protocol==this.sub_protocol) 
            return;
        this.sub_protocol = sub_protocol;
        saveField(_bm, "sub_protocol", sub_protocol);
    }

    // 创建时间(毫秒)
    public long getCreateTimeMs() { return this.create_time_ms; }
    public void setCreateTimeMs(BM _bm, long create_time_ms) {
        if(create_time_ms==this.create_time_ms) 
            return;
        this.create_time_ms = create_time_ms; 
        markField(_bm, FIELD_create_time_ms); 
    }
    public void saveCreateTimeMs(BM _bm, long create_time_ms) {
        if(create_time_ms==this.create_time_ms) 
            return;
        this.create_time_ms = create_time_ms;
        saveField(_bm, "create_time_ms", create_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `main_protocol` = '").append(main_protocol).append("',");
        sBuilder.append(" `sub_protocol` = '").append(sub_protocol).append("',");
        sBuilder.append(" `create_time_ms` = '").append(create_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_main_protocol)) sBuilder.append(" `main_protocol` = '").append(main_protocol).append("',");
        if(isFieldMarked(FIELD_sub_protocol)) sBuilder.append(" `sub_protocol` = '").append(sub_protocol).append("',");
        if(isFieldMarked(FIELD_create_time_ms)) sBuilder.append(" `create_time_ms` = '").append(create_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_protocol_shield` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`main_protocol` int(11) NOT NULL DEFAULT '0' COMMENT '主协议号',"
                + "`sub_protocol` int(11) NOT NULL DEFAULT '0' COMMENT '副协议号',"
                + "`create_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '创建时间(毫秒)',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='协议屏蔽表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//main_protocol
        _size+=4;//sub_protocol
        _size+=8;//create_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(main_protocol);
        buff.putInt(sub_protocol);
        buff.putLong(create_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        main_protocol=buff.getInt();
        sub_protocol=buff.getInt();
        create_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
