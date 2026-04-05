package HSDB.Bo;
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
public class HsParamBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_tag =0;
    @DataBaseField(type = "int(11)", fieldname = "tag", comment = "参数标记")
    private int tag;

    public static final int FIELD_param =1;
    @DataBaseField(type = "bigint(20)", fieldname = "param", comment = "参数值")
    private long param;

    public HsParamBO() {
        id = 0;
        tag = 0;
        param = 0L;
    }

    public HsParamBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        tag = rs.getInt(2);
        param = rs.getLong(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new HsParamBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `tag`, `param`";
    }

    @Override
    public String getTableName() {
        return "`hs_param`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(tag).append("', ");
        strBuf.append("'").append(param).append("', ");
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

    // 参数标记
    public int getTag() { return this.tag; }
    public void setTag(BM _bm, int tag) {
        if(tag==this.tag) 
            return;
        this.tag = tag; 
        markField(_bm, FIELD_tag); 
    }
    public void saveTag(BM _bm, int tag) {
        if(tag==this.tag) 
            return;
        this.tag = tag;
        saveField(_bm, "tag", tag);
    }

    // 参数值
    public long getParam() { return this.param; }
    public void setParam(BM _bm, long param) {
        if(param==this.param) 
            return;
        this.param = param; 
        markField(_bm, FIELD_param); 
    }
    public void saveParam(BM _bm, long param) {
        if(param==this.param) 
            return;
        this.param = param;
        saveField(_bm, "param", param);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `tag` = '").append(tag).append("',");
        sBuilder.append(" `param` = '").append(param).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_tag)) sBuilder.append(" `tag` = '").append(tag).append("',");
        if(isFieldMarked(FIELD_param)) sBuilder.append(" `param` = '").append(param).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `hs_param` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`tag` int(11) NOT NULL DEFAULT '0' COMMENT '参数标记',"
                + "`param` bigint(20) NOT NULL DEFAULT '0' COMMENT '参数值',"
                + "KEY `tag` (`tag`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='http服务器参数信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.hs_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//tag
        _size+=8;//param
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(tag);
        buff.putLong(param);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        tag=buff.getInt();
        param=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
