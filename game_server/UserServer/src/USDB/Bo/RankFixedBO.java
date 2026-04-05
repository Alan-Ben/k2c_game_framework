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
public class RankFixedBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_ref_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "ref_id", comment = "配置id")
    private long ref_id;

    public static final int FIELD_rank_instance_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "rank_instance_id", comment = "排行榜实例id")
    private long rank_instance_id;

    public RankFixedBO() {
        id = 0;
        ref_id = 0L;
        rank_instance_id = 0L;
    }

    public RankFixedBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        ref_id = rs.getLong(2);
        rank_instance_id = rs.getLong(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new RankFixedBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `ref_id`, `rank_instance_id`";
    }

    @Override
    public String getTableName() {
        return "`rank_fixed`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(ref_id).append("', ");
        strBuf.append("'").append(rank_instance_id).append("', ");
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

    // 配置id
    public long getRefId() { return this.ref_id; }
    public void setRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id; 
        markField(_bm, FIELD_ref_id); 
    }
    public void saveRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id;
        saveField(_bm, "ref_id", ref_id);
    }

    // 排行榜实例id
    public long getRankInstanceId() { return this.rank_instance_id; }
    public void setRankInstanceId(BM _bm, long rank_instance_id) {
        if(rank_instance_id==this.rank_instance_id) 
            return;
        this.rank_instance_id = rank_instance_id; 
        markField(_bm, FIELD_rank_instance_id); 
    }
    public void saveRankInstanceId(BM _bm, long rank_instance_id) {
        if(rank_instance_id==this.rank_instance_id) 
            return;
        this.rank_instance_id = rank_instance_id;
        saveField(_bm, "rank_instance_id", rank_instance_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        sBuilder.append(" `rank_instance_id` = '").append(rank_instance_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_ref_id)) sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        if(isFieldMarked(FIELD_rank_instance_id)) sBuilder.append(" `rank_instance_id` = '").append(rank_instance_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `rank_fixed` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置id',"
                + "`rank_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜实例id',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='常驻排行榜信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//ref_id
        _size+=8;//rank_instance_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(ref_id);
        buff.putLong(rank_instance_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        ref_id=buff.getLong();
        rank_instance_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
