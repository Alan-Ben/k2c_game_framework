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
public class PlayerShopBuyRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_shop_db_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "shop_db_id", comment = "商店数据id")
    private long shop_db_id;

    public static final int FIELD_shop_item_db_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "shop_item_db_id", comment = "商品数据id")
    private long shop_item_db_id;

    public static final int FIELD_buy_count =3;
    @DataBaseField(type = "int(11)", fieldname = "buy_count", comment = "购买次数")
    private int buy_count;

    public PlayerShopBuyRecordBO() {
        id = 0;
        cid = 0L;
        shop_db_id = 0L;
        shop_item_db_id = 0L;
        buy_count = 0;
    }

    public PlayerShopBuyRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        shop_db_id = rs.getLong(3);
        shop_item_db_id = rs.getLong(4);
        buy_count = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerShopBuyRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `shop_db_id`, `shop_item_db_id`, `buy_count`";
    }

    @Override
    public String getTableName() {
        return "`player_shop_buy_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(shop_db_id).append("', ");
        strBuf.append("'").append(shop_item_db_id).append("', ");
        strBuf.append("'").append(buy_count).append("', ");
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

    // 商店数据id
    public long getShopDbId() { return this.shop_db_id; }
    public void setShopDbId(BM _bm, long shop_db_id) {
        if(shop_db_id==this.shop_db_id) 
            return;
        this.shop_db_id = shop_db_id; 
        markField(_bm, FIELD_shop_db_id); 
    }
    public void saveShopDbId(BM _bm, long shop_db_id) {
        if(shop_db_id==this.shop_db_id) 
            return;
        this.shop_db_id = shop_db_id;
        saveField(_bm, "shop_db_id", shop_db_id);
    }

    // 商品数据id
    public long getShopItemDbId() { return this.shop_item_db_id; }
    public void setShopItemDbId(BM _bm, long shop_item_db_id) {
        if(shop_item_db_id==this.shop_item_db_id) 
            return;
        this.shop_item_db_id = shop_item_db_id; 
        markField(_bm, FIELD_shop_item_db_id); 
    }
    public void saveShopItemDbId(BM _bm, long shop_item_db_id) {
        if(shop_item_db_id==this.shop_item_db_id) 
            return;
        this.shop_item_db_id = shop_item_db_id;
        saveField(_bm, "shop_item_db_id", shop_item_db_id);
    }

    // 购买次数
    public int getBuyCount() { return this.buy_count; }
    public void setBuyCount(BM _bm, int buy_count) {
        if(buy_count==this.buy_count) 
            return;
        this.buy_count = buy_count; 
        markField(_bm, FIELD_buy_count); 
    }
    public void saveBuyCount(BM _bm, int buy_count) {
        if(buy_count==this.buy_count) 
            return;
        this.buy_count = buy_count;
        saveField(_bm, "buy_count", buy_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `shop_db_id` = '").append(shop_db_id).append("',");
        sBuilder.append(" `shop_item_db_id` = '").append(shop_item_db_id).append("',");
        sBuilder.append(" `buy_count` = '").append(buy_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_shop_db_id)) sBuilder.append(" `shop_db_id` = '").append(shop_db_id).append("',");
        if(isFieldMarked(FIELD_shop_item_db_id)) sBuilder.append(" `shop_item_db_id` = '").append(shop_item_db_id).append("',");
        if(isFieldMarked(FIELD_buy_count)) sBuilder.append(" `buy_count` = '").append(buy_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_shop_buy_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`shop_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '商店数据id',"
                + "`shop_item_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '商品数据id',"
                + "`buy_count` int(11) NOT NULL DEFAULT '0' COMMENT '购买次数',"
                + "KEY `cid` (`cid`),"
                + "KEY `shop_db_id` (`shop_db_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家商店购买记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//shop_db_id
        _size+=8;//shop_item_db_id
        _size+=4;//buy_count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(shop_db_id);
        buff.putLong(shop_item_db_id);
        buff.putInt(buy_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        shop_db_id=buff.getLong();
        shop_item_db_id=buff.getLong();
        buy_count=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
