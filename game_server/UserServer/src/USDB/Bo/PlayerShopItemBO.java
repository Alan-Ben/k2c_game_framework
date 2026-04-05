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
public class PlayerShopItemBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_shop_db_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "shop_db_id", comment = "商店数据id")
    private long shop_db_id;

    public static final int FIELD_shop_item_ref_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "shop_item_ref_id", comment = "商品配置id")
    private long shop_item_ref_id;

    public static final int FIELD_discount_refId =3;
    @DataBaseField(type = "bigint(20)", fieldname = "discount_refId", comment = "折扣配置id")
    private long discount_refId;

    public static final int FIELD_shop_item_group_id =4;
    @DataBaseField(type = "bigint(20)", fieldname = "shop_item_group_id", comment = "商品组id")
    private long shop_item_group_id;

    public static final int FIELD_can_buy_num =5;
    @DataBaseField(type = "bigint(20)", fieldname = "can_buy_num", comment = "限购数量")
    private long can_buy_num;

    public PlayerShopItemBO() {
        id = 0;
        cid = 0L;
        shop_db_id = 0L;
        shop_item_ref_id = 0L;
        discount_refId = 0L;
        shop_item_group_id = 0L;
        can_buy_num = 0L;
    }

    public PlayerShopItemBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        shop_db_id = rs.getLong(3);
        shop_item_ref_id = rs.getLong(4);
        discount_refId = rs.getLong(5);
        shop_item_group_id = rs.getLong(6);
        can_buy_num = rs.getLong(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerShopItemBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `shop_db_id`, `shop_item_ref_id`, `discount_refId`, `shop_item_group_id`, `can_buy_num`";
    }

    @Override
    public String getTableName() {
        return "`player_shop_item`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(shop_db_id).append("', ");
        strBuf.append("'").append(shop_item_ref_id).append("', ");
        strBuf.append("'").append(discount_refId).append("', ");
        strBuf.append("'").append(shop_item_group_id).append("', ");
        strBuf.append("'").append(can_buy_num).append("', ");
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

    // 商品配置id
    public long getShopItemRefId() { return this.shop_item_ref_id; }
    public void setShopItemRefId(BM _bm, long shop_item_ref_id) {
        if(shop_item_ref_id==this.shop_item_ref_id) 
            return;
        this.shop_item_ref_id = shop_item_ref_id; 
        markField(_bm, FIELD_shop_item_ref_id); 
    }
    public void saveShopItemRefId(BM _bm, long shop_item_ref_id) {
        if(shop_item_ref_id==this.shop_item_ref_id) 
            return;
        this.shop_item_ref_id = shop_item_ref_id;
        saveField(_bm, "shop_item_ref_id", shop_item_ref_id);
    }

    // 折扣配置id
    public long getDiscountRefId() { return this.discount_refId; }
    public void setDiscountRefId(BM _bm, long discount_refId) {
        if(discount_refId==this.discount_refId) 
            return;
        this.discount_refId = discount_refId; 
        markField(_bm, FIELD_discount_refId); 
    }
    public void saveDiscountRefId(BM _bm, long discount_refId) {
        if(discount_refId==this.discount_refId) 
            return;
        this.discount_refId = discount_refId;
        saveField(_bm, "discount_refId", discount_refId);
    }

    // 商品组id
    public long getShopItemGroupId() { return this.shop_item_group_id; }
    public void setShopItemGroupId(BM _bm, long shop_item_group_id) {
        if(shop_item_group_id==this.shop_item_group_id) 
            return;
        this.shop_item_group_id = shop_item_group_id; 
        markField(_bm, FIELD_shop_item_group_id); 
    }
    public void saveShopItemGroupId(BM _bm, long shop_item_group_id) {
        if(shop_item_group_id==this.shop_item_group_id) 
            return;
        this.shop_item_group_id = shop_item_group_id;
        saveField(_bm, "shop_item_group_id", shop_item_group_id);
    }

    // 限购数量
    public long getCanBuyNum() { return this.can_buy_num; }
    public void setCanBuyNum(BM _bm, long can_buy_num) {
        if(can_buy_num==this.can_buy_num) 
            return;
        this.can_buy_num = can_buy_num; 
        markField(_bm, FIELD_can_buy_num); 
    }
    public void saveCanBuyNum(BM _bm, long can_buy_num) {
        if(can_buy_num==this.can_buy_num) 
            return;
        this.can_buy_num = can_buy_num;
        saveField(_bm, "can_buy_num", can_buy_num);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `shop_db_id` = '").append(shop_db_id).append("',");
        sBuilder.append(" `shop_item_ref_id` = '").append(shop_item_ref_id).append("',");
        sBuilder.append(" `discount_refId` = '").append(discount_refId).append("',");
        sBuilder.append(" `shop_item_group_id` = '").append(shop_item_group_id).append("',");
        sBuilder.append(" `can_buy_num` = '").append(can_buy_num).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_shop_db_id)) sBuilder.append(" `shop_db_id` = '").append(shop_db_id).append("',");
        if(isFieldMarked(FIELD_shop_item_ref_id)) sBuilder.append(" `shop_item_ref_id` = '").append(shop_item_ref_id).append("',");
        if(isFieldMarked(FIELD_discount_refId)) sBuilder.append(" `discount_refId` = '").append(discount_refId).append("',");
        if(isFieldMarked(FIELD_shop_item_group_id)) sBuilder.append(" `shop_item_group_id` = '").append(shop_item_group_id).append("',");
        if(isFieldMarked(FIELD_can_buy_num)) sBuilder.append(" `can_buy_num` = '").append(can_buy_num).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_shop_item` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`shop_db_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '商店数据id',"
                + "`shop_item_ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '商品配置id',"
                + "`discount_refId` bigint(20) NOT NULL DEFAULT '0' COMMENT '折扣配置id',"
                + "`shop_item_group_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '商品组id',"
                + "`can_buy_num` bigint(20) NOT NULL DEFAULT '0' COMMENT '限购数量',"
                + "KEY `cid` (`cid`),"
                + "KEY `shop_db_id` (`shop_db_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家商店商品数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//shop_item_ref_id
        _size+=8;//discount_refId
        _size+=8;//shop_item_group_id
        _size+=8;//can_buy_num
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
        buff.putLong(shop_item_ref_id);
        buff.putLong(discount_refId);
        buff.putLong(shop_item_group_id);
        buff.putLong(can_buy_num);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        shop_db_id=buff.getLong();
        shop_item_ref_id=buff.getLong();
        discount_refId=buff.getLong();
        shop_item_group_id=buff.getLong();
        can_buy_num=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
