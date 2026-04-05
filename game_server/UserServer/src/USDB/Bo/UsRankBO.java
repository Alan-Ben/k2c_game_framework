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
public class UsRankBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_rank_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "rank_id", comment = "排行榜配置ID")
    private long rank_id;

    public static final int FIELD_cross_instance_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cross_instance_id", comment = "跨服分组实例ID")
    private long cross_instance_id;

    public static final int FIELD_has_reg =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "has_reg", comment = "是否已经注册跨服排行榜，如未注册需要确保注册")
    private boolean has_reg;

    public static final int FIELD_is_discard =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_discard", comment = "是否需要释放，涉及跨服同步")
    private boolean is_discard;

    public UsRankBO() {
        id = 0;
        rank_id = 0L;
        cross_instance_id = 0L;
        has_reg = false;
        is_discard = false;
    }

    public UsRankBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        rank_id = rs.getLong(2);
        cross_instance_id = rs.getLong(3);
        has_reg = rs.getBoolean(4);
        is_discard = rs.getBoolean(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsRankBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `rank_id`, `cross_instance_id`, `has_reg`, `is_discard`";
    }

    @Override
    public String getTableName() {
        return "`us_rank`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(rank_id).append("', ");
        strBuf.append("'").append(cross_instance_id).append("', ");
        strBuf.append("'").append(has_reg ? 1 : 0).append("', ");
        strBuf.append("'").append(is_discard ? 1 : 0).append("', ");
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

    // 排行榜配置ID
    public long getRankId() { return this.rank_id; }
    public void setRankId(BM _bm, long rank_id) {
        if(rank_id==this.rank_id) 
            return;
        this.rank_id = rank_id; 
        markField(_bm, FIELD_rank_id); 
    }
    public void saveRankId(BM _bm, long rank_id) {
        if(rank_id==this.rank_id) 
            return;
        this.rank_id = rank_id;
        saveField(_bm, "rank_id", rank_id);
    }

    // 跨服分组实例ID
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

    // 是否已经注册跨服排行榜，如未注册需要确保注册
    public boolean getHasReg() { return this.has_reg; }
    public void setHasReg(BM _bm, boolean has_reg) {
        if(has_reg==this.has_reg) 
            return;
        this.has_reg = has_reg; 
        markField(_bm, FIELD_has_reg); 
    }
    public void saveHasReg(BM _bm, boolean has_reg) {
        if(has_reg==this.has_reg) 
            return;
        this.has_reg = has_reg;
        saveField(_bm, "has_reg", has_reg ? 1 : 0);
    }

    // 是否需要释放，涉及跨服同步
    public boolean getIsDiscard() { return this.is_discard; }
    public void setIsDiscard(BM _bm, boolean is_discard) {
        if(is_discard==this.is_discard) 
            return;
        this.is_discard = is_discard; 
        markField(_bm, FIELD_is_discard); 
    }
    public void saveIsDiscard(BM _bm, boolean is_discard) {
        if(is_discard==this.is_discard) 
            return;
        this.is_discard = is_discard;
        saveField(_bm, "is_discard", is_discard ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `rank_id` = '").append(rank_id).append("',");
        sBuilder.append(" `cross_instance_id` = '").append(cross_instance_id).append("',");
        sBuilder.append(" `has_reg` = '").append(has_reg ? 1 : 0).append("',");
        sBuilder.append(" `is_discard` = '").append(is_discard ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_rank_id)) sBuilder.append(" `rank_id` = '").append(rank_id).append("',");
        if(isFieldMarked(FIELD_cross_instance_id)) sBuilder.append(" `cross_instance_id` = '").append(cross_instance_id).append("',");
        if(isFieldMarked(FIELD_has_reg)) sBuilder.append(" `has_reg` = '").append(has_reg ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_is_discard)) sBuilder.append(" `is_discard` = '").append(is_discard ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_rank` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`rank_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '排行榜配置ID',"
                + "`cross_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '跨服分组实例ID',"
                + "`has_reg` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已经注册跨服排行榜，如未注册需要确保注册',"
                + "`is_discard` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否需要释放，涉及跨服同步',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='排行榜数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//rank_id
        _size+=8;//cross_instance_id
        _size+=1;//has_reg
        _size+=1;//is_discard
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(rank_id);
        buff.putLong(cross_instance_id);
        buff.put((byte)(has_reg?1:0));
        buff.put((byte)(is_discard?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        rank_id=buff.getLong();
        cross_instance_id=buff.getLong();
        has_reg=(buff.get()==1);
        is_discard=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
