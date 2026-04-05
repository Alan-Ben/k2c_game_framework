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
public class NpCrossRankJoinerBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cross_rank_instance_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cross_rank_instance_id", comment = "跨服排行榜Id")
    private long cross_rank_instance_id;

    public static final int FIELD_joiner_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "joiner_id", comment = "参与者Id标记")
    private long joiner_id;

    public NpCrossRankJoinerBO() {
        id = 0;
        cross_rank_instance_id = 0L;
        joiner_id = 0L;
    }

    public NpCrossRankJoinerBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cross_rank_instance_id = rs.getLong(2);
        joiner_id = rs.getLong(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new NpCrossRankJoinerBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cross_rank_instance_id`, `joiner_id`";
    }

    @Override
    public String getTableName() {
        return "`np_cross_rank_joiner`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cross_rank_instance_id).append("', ");
        strBuf.append("'").append(joiner_id).append("', ");
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

    // 跨服排行榜Id
    public long getCrossRankInstanceId() { return this.cross_rank_instance_id; }
    public void setCrossRankInstanceId(BM _bm, long cross_rank_instance_id) {
        if(cross_rank_instance_id==this.cross_rank_instance_id) 
            return;
        this.cross_rank_instance_id = cross_rank_instance_id; 
        markField(_bm, FIELD_cross_rank_instance_id); 
    }
    public void saveCrossRankInstanceId(BM _bm, long cross_rank_instance_id) {
        if(cross_rank_instance_id==this.cross_rank_instance_id) 
            return;
        this.cross_rank_instance_id = cross_rank_instance_id;
        saveField(_bm, "cross_rank_instance_id", cross_rank_instance_id);
    }

    // 参与者Id标记
    public long getJoinerId() { return this.joiner_id; }
    public void setJoinerId(BM _bm, long joiner_id) {
        if(joiner_id==this.joiner_id) 
            return;
        this.joiner_id = joiner_id; 
        markField(_bm, FIELD_joiner_id); 
    }
    public void saveJoinerId(BM _bm, long joiner_id) {
        if(joiner_id==this.joiner_id) 
            return;
        this.joiner_id = joiner_id;
        saveField(_bm, "joiner_id", joiner_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cross_rank_instance_id` = '").append(cross_rank_instance_id).append("',");
        sBuilder.append(" `joiner_id` = '").append(joiner_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cross_rank_instance_id)) sBuilder.append(" `cross_rank_instance_id` = '").append(cross_rank_instance_id).append("',");
        if(isFieldMarked(FIELD_joiner_id)) sBuilder.append(" `joiner_id` = '").append(joiner_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `np_cross_rank_joiner` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cross_rank_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服排行榜Id',"
                + "`joiner_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '参与者Id标记',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='加入跨服排行分组的参与者，只有参与者都退出之后分组才能关闭' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cross_rank_instance_id
        _size+=8;//joiner_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cross_rank_instance_id);
        buff.putLong(joiner_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cross_rank_instance_id=buff.getLong();
        joiner_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
