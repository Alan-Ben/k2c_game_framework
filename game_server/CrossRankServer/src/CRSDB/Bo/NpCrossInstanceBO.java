package CRSDB.Bo;
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
public class NpCrossInstanceBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cross_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cross_instance_id", comment = "跨服实例id")
    private long cross_instance_id;

    public static final int FIELD_need_discard =1;
    @DataBaseField(type = "tinyint(1)", fieldname = "need_discard", comment = "是否需要删除")
    private boolean need_discard;

    public NpCrossInstanceBO() {
        id = 0;
        cross_instance_id = 0L;
        need_discard = false;
    }

    public NpCrossInstanceBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cross_instance_id = rs.getLong(2);
        need_discard = rs.getBoolean(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpCrossInstanceBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cross_instance_id`, `need_discard`";
    }

    @Override
    public String getTableName() {
        return "`np_cross_instance`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cross_instance_id).append("', ");
        strBuf.append("'").append(need_discard ? 1 : 0).append("', ");
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

    // 跨服实例id
    public long getCrossInstanceId() { return this.cross_instance_id; }
    public void setCrossInstanceId(BM _bm, long cross_instance_id) {
        if(cross_instance_id==this.cross_instance_id) 
            return;
        this.cross_instance_id = cross_instance_id; 
        markField(_bm, FIELD_cross_instance_id); 
    }
    public void saveCrossInstanceId(BM _bm, long cross_instance_id) {
        if(cross_instance_id==this.cross_instance_id) 
            return;
        this.cross_instance_id = cross_instance_id;
        saveField(_bm, "cross_instance_id", cross_instance_id);
    }

    // 是否需要删除
    public boolean getNeedDiscard() { return this.need_discard; }
    public void setNeedDiscard(BM _bm, boolean need_discard) {
        if(need_discard==this.need_discard) 
            return;
        this.need_discard = need_discard; 
        markField(_bm, FIELD_need_discard); 
    }
    public void saveNeedDiscard(BM _bm, boolean need_discard) {
        if(need_discard==this.need_discard) 
            return;
        this.need_discard = need_discard;
        saveField(_bm, "need_discard", need_discard ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cross_instance_id` = '").append(cross_instance_id).append("',");
        sBuilder.append(" `need_discard` = '").append(need_discard ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cross_instance_id)) sBuilder.append(" `cross_instance_id` = '").append(cross_instance_id).append("',");
        if(isFieldMarked(FIELD_need_discard)) sBuilder.append(" `need_discard` = '").append(need_discard ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_cross_instance` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cross_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服实例id',"
                + "`need_discard` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否需要删除',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='跨服排行服务器分组数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.crossrank_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//cross_instance_id
        _size+=1;//need_discard
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cross_instance_id);
        buff.put((byte)(need_discard?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cross_instance_id=buff.getLong();
        need_discard=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
