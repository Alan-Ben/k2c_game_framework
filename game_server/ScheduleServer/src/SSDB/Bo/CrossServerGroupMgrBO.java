package SSDB.Bo;
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
public class CrossServerGroupMgrBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_max_expired_cross_server_group_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "max_expired_cross_server_group_id", comment = "跨服分组过期的最大分组ID")
    private long max_expired_cross_server_group_id;

    public static final int FIELD_max_work_cross_server_group_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "max_work_cross_server_group_id", comment = "跨服分组生效中的最大分组ID")
    private long max_work_cross_server_group_id;

    public CrossServerGroupMgrBO() {
        id = 0;
        max_expired_cross_server_group_id = 0L;
        max_work_cross_server_group_id = 0L;
    }

    public CrossServerGroupMgrBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        max_expired_cross_server_group_id = rs.getLong(2);
        max_work_cross_server_group_id = rs.getLong(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new CrossServerGroupMgrBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `max_expired_cross_server_group_id`, `max_work_cross_server_group_id`";
    }

    @Override
    public String getTableName() {
        return "`cross_server_group_mgr`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(max_expired_cross_server_group_id).append("', ");
        strBuf.append("'").append(max_work_cross_server_group_id).append("', ");
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

    // 跨服分组过期的最大分组ID
    public long getMaxExpiredCrossServerGroupId() { return this.max_expired_cross_server_group_id; }
    public void setMaxExpiredCrossServerGroupId(BM _bm, long max_expired_cross_server_group_id) {
        if(max_expired_cross_server_group_id==this.max_expired_cross_server_group_id) 
            return;
        this.max_expired_cross_server_group_id = max_expired_cross_server_group_id; 
        markField(_bm, FIELD_max_expired_cross_server_group_id); 
    }
    public void saveMaxExpiredCrossServerGroupId(BM _bm, long max_expired_cross_server_group_id) {
        if(max_expired_cross_server_group_id==this.max_expired_cross_server_group_id) 
            return;
        this.max_expired_cross_server_group_id = max_expired_cross_server_group_id;
        saveField(_bm, "max_expired_cross_server_group_id", max_expired_cross_server_group_id);
    }

    // 跨服分组生效中的最大分组ID
    public long getMaxWorkCrossServerGroupId() { return this.max_work_cross_server_group_id; }
    public void setMaxWorkCrossServerGroupId(BM _bm, long max_work_cross_server_group_id) {
        if(max_work_cross_server_group_id==this.max_work_cross_server_group_id) 
            return;
        this.max_work_cross_server_group_id = max_work_cross_server_group_id; 
        markField(_bm, FIELD_max_work_cross_server_group_id); 
    }
    public void saveMaxWorkCrossServerGroupId(BM _bm, long max_work_cross_server_group_id) {
        if(max_work_cross_server_group_id==this.max_work_cross_server_group_id) 
            return;
        this.max_work_cross_server_group_id = max_work_cross_server_group_id;
        saveField(_bm, "max_work_cross_server_group_id", max_work_cross_server_group_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `max_expired_cross_server_group_id` = '").append(max_expired_cross_server_group_id).append("',");
        sBuilder.append(" `max_work_cross_server_group_id` = '").append(max_work_cross_server_group_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_max_expired_cross_server_group_id)) sBuilder.append(" `max_expired_cross_server_group_id` = '").append(max_expired_cross_server_group_id).append("',");
        if(isFieldMarked(FIELD_max_work_cross_server_group_id)) sBuilder.append(" `max_work_cross_server_group_id` = '").append(max_work_cross_server_group_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `cross_server_group_mgr` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`max_expired_cross_server_group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服分组过期的最大分组ID',"
                + "`max_work_cross_server_group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服分组生效中的最大分组ID',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='跨服分组对象' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.ss_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//max_expired_cross_server_group_id
        _size+=8;//max_work_cross_server_group_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(max_expired_cross_server_group_id);
        buff.putLong(max_work_cross_server_group_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        max_expired_cross_server_group_id=buff.getLong();
        max_work_cross_server_group_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
