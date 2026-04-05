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
public class CollectLikeInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家cid")
    private long cid;

    public static final int FIELD_like_count =1;
    @DataBaseField(type = "bigint(20)", fieldname = "like_count", comment = "被点赞总数")
    private long like_count;

    public static final int FIELD_last_read_like_count =2;
    @DataBaseField(type = "bigint(20)", fieldname = "last_read_like_count", comment = "上次查看被点赞总数")
    private long last_read_like_count;

    public CollectLikeInfoBO() {
        id = 0;
        cid = 0L;
        like_count = 0L;
        last_read_like_count = 0L;
    }

    public CollectLikeInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        like_count = rs.getLong(3);
        last_read_like_count = rs.getLong(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new CollectLikeInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `like_count`, `last_read_like_count`";
    }

    @Override
    public String getTableName() {
        return "`collect_like_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(like_count).append("', ");
        strBuf.append("'").append(last_read_like_count).append("', ");
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

    // 玩家cid
    public long getCid() { return this.cid; }
    public void setCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid; 
        markField(_bm, FIELD_cid); 
    }
    public void saveCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid;
        saveField(_bm, "cid", cid);
    }

    // 被点赞总数
    public long getLikeCount() { return this.like_count; }
    public void setLikeCount(BM _bm, long like_count) {
        if(like_count==this.like_count) 
            return;
        this.like_count = like_count; 
        markField(_bm, FIELD_like_count); 
    }
    public void saveLikeCount(BM _bm, long like_count) {
        if(like_count==this.like_count) 
            return;
        this.like_count = like_count;
        saveField(_bm, "like_count", like_count);
    }

    // 上次查看被点赞总数
    public long getLastReadLikeCount() { return this.last_read_like_count; }
    public void setLastReadLikeCount(BM _bm, long last_read_like_count) {
        if(last_read_like_count==this.last_read_like_count) 
            return;
        this.last_read_like_count = last_read_like_count; 
        markField(_bm, FIELD_last_read_like_count); 
    }
    public void saveLastReadLikeCount(BM _bm, long last_read_like_count) {
        if(last_read_like_count==this.last_read_like_count) 
            return;
        this.last_read_like_count = last_read_like_count;
        saveField(_bm, "last_read_like_count", last_read_like_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `like_count` = '").append(like_count).append("',");
        sBuilder.append(" `last_read_like_count` = '").append(last_read_like_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_like_count)) sBuilder.append(" `like_count` = '").append(like_count).append("',");
        if(isFieldMarked(FIELD_last_read_like_count)) sBuilder.append(" `last_read_like_count` = '").append(last_read_like_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `collect_like_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家cid',"
                + "`like_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '被点赞总数',"
                + "`last_read_like_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次查看被点赞总数',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='集赞信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=8;//like_count
        _size+=8;//last_read_like_count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(like_count);
        buff.putLong(last_read_like_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        like_count=buff.getLong();
        last_read_like_count=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
