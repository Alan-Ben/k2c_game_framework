package CSDB.Bo;
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
public class HandleServerBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_server_type =0;
    @DataBaseField(type = "int(11)", fieldname = "server_type", comment = "服务器类型")
    private int server_type;

    public static final int FIELD_type_id =1;
    @DataBaseField(type = "int(11)", fieldname = "type_id", comment = "服务器类型ID")
    private int type_id;

    public static final int FIELD_weight =2;
    @DataBaseField(type = "int(11)", fieldname = "weight", comment = "服务器负载值")
    private int weight;

    public HandleServerBO() {
        id = 0;
        server_type = 0;
        type_id = 0;
        weight = 0;
    }

    public HandleServerBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        server_type = rs.getInt(2);
        type_id = rs.getInt(3);
        weight = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new HandleServerBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `server_type`, `type_id`, `weight`";
    }

    @Override
    public String getTableName() {
        return "`handle_server`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(server_type).append("', ");
        strBuf.append("'").append(type_id).append("', ");
        strBuf.append("'").append(weight).append("', ");
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

    // 服务器类型
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

    // 服务器类型ID
    public int getTypeId() { return this.type_id; }
    public void setTypeId(BM _bm, int type_id) {
        if(type_id==this.type_id) 
            return;
        this.type_id = type_id; 
        markField(_bm, FIELD_type_id); 
    }
    public void saveTypeId(BM _bm, int type_id) {
        if(type_id==this.type_id) 
            return;
        this.type_id = type_id;
        saveField(_bm, "type_id", type_id);
    }

    // 服务器负载值
    public int getWeight() { return this.weight; }
    public void setWeight(BM _bm, int weight) {
        if(weight==this.weight) 
            return;
        this.weight = weight; 
        markField(_bm, FIELD_weight); 
    }
    public void saveWeight(BM _bm, int weight) {
        if(weight==this.weight) 
            return;
        this.weight = weight;
        saveField(_bm, "weight", weight);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `server_type` = '").append(server_type).append("',");
        sBuilder.append(" `type_id` = '").append(type_id).append("',");
        sBuilder.append(" `weight` = '").append(weight).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_server_type)) sBuilder.append(" `server_type` = '").append(server_type).append("',");
        if(isFieldMarked(FIELD_type_id)) sBuilder.append(" `type_id` = '").append(type_id).append("',");
        if(isFieldMarked(FIELD_weight)) sBuilder.append(" `weight` = '").append(weight).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `handle_server` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`server_type` int(11) NOT NULL DEFAULT '0' COMMENT '服务器类型',"
                + "`type_id` int(11) NOT NULL DEFAULT '0' COMMENT '服务器类型ID',"
                + "`weight` int(11) NOT NULL DEFAULT '0' COMMENT '服务器负载值',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='服务器负载数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.comm_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//server_type
        _size+=4;//type_id
        _size+=4;//weight
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(server_type);
        buff.putInt(type_id);
        buff.putInt(weight);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        server_type=buff.getInt();
        type_id=buff.getInt();
        weight=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
