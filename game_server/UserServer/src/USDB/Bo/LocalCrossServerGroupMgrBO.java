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
public class LocalCrossServerGroupMgrBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cross_group_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cross_group_id", comment = "当前跨服分组id")
    private long cross_group_id;

    public static final int FIELD_cross_group_info =1;
    @DataBaseField(type = "blob", fieldname = "cross_group_info", comment = "当前的分组信息")
    private byte[] cross_group_info;

    public LocalCrossServerGroupMgrBO() {
        id = 0;
        cross_group_id = 0L;
        cross_group_info = null;
    }

    public LocalCrossServerGroupMgrBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cross_group_id = rs.getLong(2);
        cross_group_info = rs.getBytes(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LocalCrossServerGroupMgrBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cross_group_id`, `cross_group_info`";
    }

    @Override
    public String getTableName() {
        return "`local_cross_server_group_mgr`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cross_group_id).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(cross_group_info);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_cross_group_info)) ret.add(cross_group_info);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 当前跨服分组id
    public long getCrossGroupId() { return this.cross_group_id; }
    public void setCrossGroupId(BM _bm, long cross_group_id) {
        if(cross_group_id==this.cross_group_id) 
            return;
        this.cross_group_id = cross_group_id; 
        markField(_bm, FIELD_cross_group_id); 
    }
    public void saveCrossGroupId(BM _bm, long cross_group_id) {
        if(cross_group_id==this.cross_group_id) 
            return;
        this.cross_group_id = cross_group_id;
        saveField(_bm, "cross_group_id", cross_group_id);
    }

    // 当前的分组信息
    public byte[] getCrossGroupInfo() { return this.cross_group_info; }
    public void setCrossGroupInfo(BM _bm, byte[] cross_group_info) {
        if(cross_group_info==this.cross_group_info) 
            return;
        this.cross_group_info = cross_group_info; 
        markField(_bm, FIELD_cross_group_info); 
    }
    public void saveCrossGroupInfo(BM _bm, byte[] cross_group_info) {
        if(cross_group_info==this.cross_group_info) 
            return;
        this.cross_group_info = cross_group_info;
        saveFieldBytes(_bm, "cross_group_info", cross_group_info);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cross_group_id` = '").append(cross_group_id).append("',");
        sBuilder.append(" `cross_group_info` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cross_group_id)) sBuilder.append(" `cross_group_id` = '").append(cross_group_id).append("',");
        if(isFieldMarked(FIELD_cross_group_info)) sBuilder.append(" `cross_group_info` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `local_cross_server_group_mgr` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cross_group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前跨服分组id',"
                + "`cross_group_info` blob NULL COMMENT '当前的分组信息',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='服务器本地跨服分组管理器' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cross_group_id
        _size+=2;_size+=cross_group_info.length;//cross_group_info
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cross_group_id);
        buff.putShort((short)(cross_group_info == null ? 0 : cross_group_info.length));if(null != cross_group_info){buff.put(cross_group_info);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cross_group_id=buff.getLong();
        int cross_group_info_count = buff.getShort();if(cross_group_info_count>0){cross_group_info = new byte[cross_group_info_count];buff.get(cross_group_info);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
