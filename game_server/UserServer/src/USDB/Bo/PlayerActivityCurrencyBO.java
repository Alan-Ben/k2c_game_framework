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
public class PlayerActivityCurrencyBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_refId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "refId", comment = "配表id")
    private long refId;

    public static final int FIELD_count =2;
    @DataBaseField(type = "bigint(20)", fieldname = "count", comment = "货币数量")
    private long count;

    public static final int FIELD_total_gain_count =3;
    @DataBaseField(type = "bigint(20)", fieldname = "total_gain_count", comment = "总获得数量")
    private long total_gain_count;

    public static final int FIELD_total_consume_count =4;
    @DataBaseField(type = "bigint(20)", fieldname = "total_consume_count", comment = "总消耗数量")
    private long total_consume_count;

    public PlayerActivityCurrencyBO() {
        id = 0;
        cid = 0L;
        refId = 0L;
        count = 0L;
        total_gain_count = 0L;
        total_consume_count = 0L;
    }

    public PlayerActivityCurrencyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        refId = rs.getLong(3);
        count = rs.getLong(4);
        total_gain_count = rs.getLong(5);
        total_consume_count = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerActivityCurrencyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `refId`, `count`, `total_gain_count`, `total_consume_count`";
    }

    @Override
    public String getTableName() {
        return "`player_activity_currency`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(refId).append("', ");
        strBuf.append("'").append(count).append("', ");
        strBuf.append("'").append(total_gain_count).append("', ");
        strBuf.append("'").append(total_consume_count).append("', ");
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

    // 玩家CID
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

    // 配表id
    public long getRefId() { return this.refId; }
    public void setRefId(BM _bm, long refId) {
        if(refId==this.refId) 
            return;
        this.refId = refId; 
        markField(_bm, FIELD_refId); 
    }
    public void saveRefId(BM _bm, long refId) {
        if(refId==this.refId) 
            return;
        this.refId = refId;
        saveField(_bm, "refId", refId);
    }

    // 货币数量
    public long getCount() { return this.count; }
    public void setCount(BM _bm, long count) {
        if(count==this.count) 
            return;
        this.count = count; 
        markField(_bm, FIELD_count); 
    }
    public void saveCount(BM _bm, long count) {
        if(count==this.count) 
            return;
        this.count = count;
        saveField(_bm, "count", count);
    }

    // 总获得数量
    public long getTotalGainCount() { return this.total_gain_count; }
    public void setTotalGainCount(BM _bm, long total_gain_count) {
        if(total_gain_count==this.total_gain_count) 
            return;
        this.total_gain_count = total_gain_count; 
        markField(_bm, FIELD_total_gain_count); 
    }
    public void saveTotalGainCount(BM _bm, long total_gain_count) {
        if(total_gain_count==this.total_gain_count) 
            return;
        this.total_gain_count = total_gain_count;
        saveField(_bm, "total_gain_count", total_gain_count);
    }

    // 总消耗数量
    public long getTotalConsumeCount() { return this.total_consume_count; }
    public void setTotalConsumeCount(BM _bm, long total_consume_count) {
        if(total_consume_count==this.total_consume_count) 
            return;
        this.total_consume_count = total_consume_count; 
        markField(_bm, FIELD_total_consume_count); 
    }
    public void saveTotalConsumeCount(BM _bm, long total_consume_count) {
        if(total_consume_count==this.total_consume_count) 
            return;
        this.total_consume_count = total_consume_count;
        saveField(_bm, "total_consume_count", total_consume_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `refId` = '").append(refId).append("',");
        sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.append(" `total_gain_count` = '").append(total_gain_count).append("',");
        sBuilder.append(" `total_consume_count` = '").append(total_consume_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_refId)) sBuilder.append(" `refId` = '").append(refId).append("',");
        if(isFieldMarked(FIELD_count)) sBuilder.append(" `count` = '").append(count).append("',");
        if(isFieldMarked(FIELD_total_gain_count)) sBuilder.append(" `total_gain_count` = '").append(total_gain_count).append("',");
        if(isFieldMarked(FIELD_total_consume_count)) sBuilder.append(" `total_consume_count` = '").append(total_consume_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_activity_currency` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`refId` bigint(20) NOT NULL DEFAULT '0' COMMENT '配表id',"
                + "`count` bigint(20) NOT NULL DEFAULT '0' COMMENT '货币数量',"
                + "`total_gain_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '总获得数量',"
                + "`total_consume_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '总消耗数量',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家活动积分数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//refId
        _size+=8;//count
        _size+=8;//total_gain_count
        _size+=8;//total_consume_count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(refId);
        buff.putLong(count);
        buff.putLong(total_gain_count);
        buff.putLong(total_consume_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        refId=buff.getLong();
        count=buff.getLong();
        total_gain_count=buff.getLong();
        total_consume_count=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
