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
public class PlayerShopBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_shop_ref_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "shop_ref_id", comment = "商店配置id")
    private long shop_ref_id;

    public static final int FIELD_next_fresh_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "next_fresh_time_ms", comment = "下次刷新时间")
    private long next_fresh_time_ms;

    public static final int FIELD_has_refresh_num =3;
    @DataBaseField(type = "int(11)", fieldname = "has_refresh_num", comment = "已付费刷新次数")
    private int has_refresh_num;

    public PlayerShopBO() {
        id = 0;
        cid = 0L;
        shop_ref_id = 0L;
        next_fresh_time_ms = 0L;
        has_refresh_num = 0;
    }

    public PlayerShopBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        shop_ref_id = rs.getLong(3);
        next_fresh_time_ms = rs.getLong(4);
        has_refresh_num = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerShopBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `shop_ref_id`, `next_fresh_time_ms`, `has_refresh_num`";
    }

    @Override
    public String getTableName() {
        return "`player_shop`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(shop_ref_id).append("', ");
        strBuf.append("'").append(next_fresh_time_ms).append("', ");
        strBuf.append("'").append(has_refresh_num).append("', ");
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

    // 商店配置id
    public long getShopRefId() { return this.shop_ref_id; }
    public void setShopRefId(BM _bm, long shop_ref_id) {
        if(shop_ref_id==this.shop_ref_id) 
            return;
        this.shop_ref_id = shop_ref_id; 
        markField(_bm, FIELD_shop_ref_id); 
    }
    public void saveShopRefId(BM _bm, long shop_ref_id) {
        if(shop_ref_id==this.shop_ref_id) 
            return;
        this.shop_ref_id = shop_ref_id;
        saveField(_bm, "shop_ref_id", shop_ref_id);
    }

    // 下次刷新时间
    public long getNextFreshTimeMs() { return this.next_fresh_time_ms; }
    public void setNextFreshTimeMs(BM _bm, long next_fresh_time_ms) {
        if(next_fresh_time_ms==this.next_fresh_time_ms) 
            return;
        this.next_fresh_time_ms = next_fresh_time_ms; 
        markField(_bm, FIELD_next_fresh_time_ms); 
    }
    public void saveNextFreshTimeMs(BM _bm, long next_fresh_time_ms) {
        if(next_fresh_time_ms==this.next_fresh_time_ms) 
            return;
        this.next_fresh_time_ms = next_fresh_time_ms;
        saveField(_bm, "next_fresh_time_ms", next_fresh_time_ms);
    }

    // 已付费刷新次数
    public int getHasRefreshNum() { return this.has_refresh_num; }
    public void setHasRefreshNum(BM _bm, int has_refresh_num) {
        if(has_refresh_num==this.has_refresh_num) 
            return;
        this.has_refresh_num = has_refresh_num; 
        markField(_bm, FIELD_has_refresh_num); 
    }
    public void saveHasRefreshNum(BM _bm, int has_refresh_num) {
        if(has_refresh_num==this.has_refresh_num) 
            return;
        this.has_refresh_num = has_refresh_num;
        saveField(_bm, "has_refresh_num", has_refresh_num);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `shop_ref_id` = '").append(shop_ref_id).append("',");
        sBuilder.append(" `next_fresh_time_ms` = '").append(next_fresh_time_ms).append("',");
        sBuilder.append(" `has_refresh_num` = '").append(has_refresh_num).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_shop_ref_id)) sBuilder.append(" `shop_ref_id` = '").append(shop_ref_id).append("',");
        if(isFieldMarked(FIELD_next_fresh_time_ms)) sBuilder.append(" `next_fresh_time_ms` = '").append(next_fresh_time_ms).append("',");
        if(isFieldMarked(FIELD_has_refresh_num)) sBuilder.append(" `has_refresh_num` = '").append(has_refresh_num).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_shop` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`shop_ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '商店配置id',"
                + "`next_fresh_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '下次刷新时间',"
                + "`has_refresh_num` int(11) NOT NULL DEFAULT '0' COMMENT '已付费刷新次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家商店数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//shop_ref_id
        _size+=8;//next_fresh_time_ms
        _size+=4;//has_refresh_num
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(shop_ref_id);
        buff.putLong(next_fresh_time_ms);
        buff.putInt(has_refresh_num);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        shop_ref_id=buff.getLong();
        next_fresh_time_ms=buff.getLong();
        has_refresh_num=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
