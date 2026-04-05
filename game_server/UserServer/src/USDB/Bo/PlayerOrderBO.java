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
public class PlayerOrderBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_order_id =1;
    @DataBaseField(type = "varchar(500)", fieldname = "order_id", comment = "订单号")
    private String order_id;

    public static final int FIELD_pay_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "pay_id", comment = "支付id")
    private long pay_id;

    public static final int FIELD_goods_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "goods_id", comment = "商品id")
    private long goods_id;

    public static final int FIELD_order_status =4;
    @DataBaseField(type = "int(11)", fieldname = "order_status", comment = "订单状态")
    private int order_status;

    public static final int FIELD_pay_type =5;
    @DataBaseField(type = "int(11)", fieldname = "pay_type", comment = "支付方式类型")
    private int pay_type;

    public static final int FIELD_create_time_ms =6;
    @DataBaseField(type = "bigint(20)", fieldname = "create_time_ms", comment = "创建时间 ms")
    private long create_time_ms;

    public static final int FIELD_sdk_order_id =7;
    @DataBaseField(type = "varchar(500)", fieldname = "sdk_order_id", comment = "sdk订单号")
    private String sdk_order_id;

    public static final int FIELD_pay_time_ms =8;
    @DataBaseField(type = "bigint(20)", fieldname = "pay_time_ms", comment = "支付时间 ms")
    private long pay_time_ms;

    public static final int FIELD_arrive_time_ms =9;
    @DataBaseField(type = "bigint(20)", fieldname = "arrive_time_ms", comment = "收到支付回调时间 ms")
    private long arrive_time_ms;

    public static final int FIELD_is_offline_pay =10;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_offline_pay", comment = "是否离线时支付")
    private boolean is_offline_pay;

    public static final int FIELD_item_list =11;
    @DataBaseField(type = "text", fieldname = "item_list", comment = "对应物品列表")
    private String item_list;

    public static final int FIELD_had_client_notify_pay =12;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_client_notify_pay", comment = "是否客户端已通知支付")
    private boolean had_client_notify_pay;

    public PlayerOrderBO() {
        id = 0;
        cid = 0L;
        order_id = "";
        pay_id = 0L;
        goods_id = 0L;
        order_status = 0;
        pay_type = 0;
        create_time_ms = 0L;
        sdk_order_id = "";
        pay_time_ms = 0L;
        arrive_time_ms = 0L;
        is_offline_pay = false;
        item_list = "";
        had_client_notify_pay = false;
    }

    public PlayerOrderBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        order_id = rs.getString(3);
        pay_id = rs.getLong(4);
        goods_id = rs.getLong(5);
        order_status = rs.getInt(6);
        pay_type = rs.getInt(7);
        create_time_ms = rs.getLong(8);
        sdk_order_id = rs.getString(9);
        pay_time_ms = rs.getLong(10);
        arrive_time_ms = rs.getLong(11);
        is_offline_pay = rs.getBoolean(12);
        item_list = rs.getString(13);
        had_client_notify_pay = rs.getBoolean(14);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerOrderBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `order_id`, `pay_id`, `goods_id`, `order_status`, `pay_type`, `create_time_ms`, `sdk_order_id`, `pay_time_ms`, `arrive_time_ms`, `is_offline_pay`, `item_list`, `had_client_notify_pay`";
    }

    @Override
    public String getTableName() {
        return "`player_order`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(pay_id).append("', ");
        strBuf.append("'").append(goods_id).append("', ");
        strBuf.append("'").append(order_status).append("', ");
        strBuf.append("'").append(pay_type).append("', ");
        strBuf.append("'").append(create_time_ms).append("', ");
        strBuf.append("'").append(sdk_order_id == null ? null : sdk_order_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(pay_time_ms).append("', ");
        strBuf.append("'").append(arrive_time_ms).append("', ");
        strBuf.append("'").append(is_offline_pay ? 1 : 0).append("', ");
        strBuf.append("'").append(item_list == null ? null : item_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(had_client_notify_pay ? 1 : 0).append("', ");
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

    // 订单号
    public String getOrderId() { return this.order_id; }
    public void setOrderId(BM _bm, String order_id) {
        if(order_id.equals(this.order_id)) 
            return;
        this.order_id = order_id; 
        markField(_bm, FIELD_order_id); 
    }
    public void saveOrderId(BM _bm, String order_id) {
        if(order_id.equals(this.order_id)) 
            return;
        this.order_id = order_id;
        saveField(_bm, "order_id", order_id);
    }

    // 支付id
    public long getPayId() { return this.pay_id; }
    public void setPayId(BM _bm, long pay_id) {
        if(pay_id==this.pay_id) 
            return;
        this.pay_id = pay_id; 
        markField(_bm, FIELD_pay_id); 
    }
    public void savePayId(BM _bm, long pay_id) {
        if(pay_id==this.pay_id) 
            return;
        this.pay_id = pay_id;
        saveField(_bm, "pay_id", pay_id);
    }

    // 商品id
    public long getGoodsId() { return this.goods_id; }
    public void setGoodsId(BM _bm, long goods_id) {
        if(goods_id==this.goods_id) 
            return;
        this.goods_id = goods_id; 
        markField(_bm, FIELD_goods_id); 
    }
    public void saveGoodsId(BM _bm, long goods_id) {
        if(goods_id==this.goods_id) 
            return;
        this.goods_id = goods_id;
        saveField(_bm, "goods_id", goods_id);
    }

    // 订单状态
    public int getOrderStatus() { return this.order_status; }
    public void setOrderStatus(BM _bm, int order_status) {
        if(order_status==this.order_status) 
            return;
        this.order_status = order_status; 
        markField(_bm, FIELD_order_status); 
    }
    public void saveOrderStatus(BM _bm, int order_status) {
        if(order_status==this.order_status) 
            return;
        this.order_status = order_status;
        saveField(_bm, "order_status", order_status);
    }

    // 支付方式类型
    public int getPayType() { return this.pay_type; }
    public void setPayType(BM _bm, int pay_type) {
        if(pay_type==this.pay_type) 
            return;
        this.pay_type = pay_type; 
        markField(_bm, FIELD_pay_type); 
    }
    public void savePayType(BM _bm, int pay_type) {
        if(pay_type==this.pay_type) 
            return;
        this.pay_type = pay_type;
        saveField(_bm, "pay_type", pay_type);
    }

    // 创建时间 ms
    public long getCreateTimeMs() { return this.create_time_ms; }
    public void setCreateTimeMs(BM _bm, long create_time_ms) {
        if(create_time_ms==this.create_time_ms) 
            return;
        this.create_time_ms = create_time_ms; 
        markField(_bm, FIELD_create_time_ms); 
    }
    public void saveCreateTimeMs(BM _bm, long create_time_ms) {
        if(create_time_ms==this.create_time_ms) 
            return;
        this.create_time_ms = create_time_ms;
        saveField(_bm, "create_time_ms", create_time_ms);
    }

    // sdk订单号
    public String getSdkOrderId() { return this.sdk_order_id; }
    public void setSdkOrderId(BM _bm, String sdk_order_id) {
        if(sdk_order_id.equals(this.sdk_order_id)) 
            return;
        this.sdk_order_id = sdk_order_id; 
        markField(_bm, FIELD_sdk_order_id); 
    }
    public void saveSdkOrderId(BM _bm, String sdk_order_id) {
        if(sdk_order_id.equals(this.sdk_order_id)) 
            return;
        this.sdk_order_id = sdk_order_id;
        saveField(_bm, "sdk_order_id", sdk_order_id);
    }

    // 支付时间 ms
    public long getPayTimeMs() { return this.pay_time_ms; }
    public void setPayTimeMs(BM _bm, long pay_time_ms) {
        if(pay_time_ms==this.pay_time_ms) 
            return;
        this.pay_time_ms = pay_time_ms; 
        markField(_bm, FIELD_pay_time_ms); 
    }
    public void savePayTimeMs(BM _bm, long pay_time_ms) {
        if(pay_time_ms==this.pay_time_ms) 
            return;
        this.pay_time_ms = pay_time_ms;
        saveField(_bm, "pay_time_ms", pay_time_ms);
    }

    // 收到支付回调时间 ms
    public long getArriveTimeMs() { return this.arrive_time_ms; }
    public void setArriveTimeMs(BM _bm, long arrive_time_ms) {
        if(arrive_time_ms==this.arrive_time_ms) 
            return;
        this.arrive_time_ms = arrive_time_ms; 
        markField(_bm, FIELD_arrive_time_ms); 
    }
    public void saveArriveTimeMs(BM _bm, long arrive_time_ms) {
        if(arrive_time_ms==this.arrive_time_ms) 
            return;
        this.arrive_time_ms = arrive_time_ms;
        saveField(_bm, "arrive_time_ms", arrive_time_ms);
    }

    // 是否离线时支付
    public boolean getIsOfflinePay() { return this.is_offline_pay; }
    public void setIsOfflinePay(BM _bm, boolean is_offline_pay) {
        if(is_offline_pay==this.is_offline_pay) 
            return;
        this.is_offline_pay = is_offline_pay; 
        markField(_bm, FIELD_is_offline_pay); 
    }
    public void saveIsOfflinePay(BM _bm, boolean is_offline_pay) {
        if(is_offline_pay==this.is_offline_pay) 
            return;
        this.is_offline_pay = is_offline_pay;
        saveField(_bm, "is_offline_pay", is_offline_pay ? 1 : 0);
    }

    // 对应物品列表
    public String getItemList() { return this.item_list; }
    public void setItemList(BM _bm, String item_list) {
        if(item_list.equals(this.item_list)) 
            return;
        this.item_list = item_list; 
        markField(_bm, FIELD_item_list); 
    }
    public void saveItemList(BM _bm, String item_list) {
        if(item_list.equals(this.item_list)) 
            return;
        this.item_list = item_list;
        saveField(_bm, "item_list", item_list);
    }

    // 是否客户端已通知支付
    public boolean getHadClientNotifyPay() { return this.had_client_notify_pay; }
    public void setHadClientNotifyPay(BM _bm, boolean had_client_notify_pay) {
        if(had_client_notify_pay==this.had_client_notify_pay) 
            return;
        this.had_client_notify_pay = had_client_notify_pay; 
        markField(_bm, FIELD_had_client_notify_pay); 
    }
    public void saveHadClientNotifyPay(BM _bm, boolean had_client_notify_pay) {
        if(had_client_notify_pay==this.had_client_notify_pay) 
            return;
        this.had_client_notify_pay = had_client_notify_pay;
        saveField(_bm, "had_client_notify_pay", had_client_notify_pay ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `order_id` = '").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `pay_id` = '").append(pay_id).append("',");
        sBuilder.append(" `goods_id` = '").append(goods_id).append("',");
        sBuilder.append(" `order_status` = '").append(order_status).append("',");
        sBuilder.append(" `pay_type` = '").append(pay_type).append("',");
        sBuilder.append(" `create_time_ms` = '").append(create_time_ms).append("',");
        sBuilder.append(" `sdk_order_id` = '").append(sdk_order_id == null ? null : sdk_order_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `pay_time_ms` = '").append(pay_time_ms).append("',");
        sBuilder.append(" `arrive_time_ms` = '").append(arrive_time_ms).append("',");
        sBuilder.append(" `is_offline_pay` = '").append(is_offline_pay ? 1 : 0).append("',");
        sBuilder.append(" `item_list` = '").append(item_list == null ? null : item_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `had_client_notify_pay` = '").append(had_client_notify_pay ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_order_id)) sBuilder.append(" `order_id` = '").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_pay_id)) sBuilder.append(" `pay_id` = '").append(pay_id).append("',");
        if(isFieldMarked(FIELD_goods_id)) sBuilder.append(" `goods_id` = '").append(goods_id).append("',");
        if(isFieldMarked(FIELD_order_status)) sBuilder.append(" `order_status` = '").append(order_status).append("',");
        if(isFieldMarked(FIELD_pay_type)) sBuilder.append(" `pay_type` = '").append(pay_type).append("',");
        if(isFieldMarked(FIELD_create_time_ms)) sBuilder.append(" `create_time_ms` = '").append(create_time_ms).append("',");
        if(isFieldMarked(FIELD_sdk_order_id)) sBuilder.append(" `sdk_order_id` = '").append(sdk_order_id == null ? null : sdk_order_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_pay_time_ms)) sBuilder.append(" `pay_time_ms` = '").append(pay_time_ms).append("',");
        if(isFieldMarked(FIELD_arrive_time_ms)) sBuilder.append(" `arrive_time_ms` = '").append(arrive_time_ms).append("',");
        if(isFieldMarked(FIELD_is_offline_pay)) sBuilder.append(" `is_offline_pay` = '").append(is_offline_pay ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_item_list)) sBuilder.append(" `item_list` = '").append(item_list == null ? null : item_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_had_client_notify_pay)) sBuilder.append(" `had_client_notify_pay` = '").append(had_client_notify_pay ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_order` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`order_id` varchar(500) NOT NULL DEFAULT '' COMMENT '订单号',"
                + "`pay_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '支付id',"
                + "`goods_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '商品id',"
                + "`order_status` int(11) NOT NULL DEFAULT '0' COMMENT '订单状态',"
                + "`pay_type` int(11) NOT NULL DEFAULT '0' COMMENT '支付方式类型',"
                + "`create_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '创建时间 ms',"
                + "`sdk_order_id` varchar(500) NOT NULL DEFAULT '' COMMENT 'sdk订单号',"
                + "`pay_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '支付时间 ms',"
                + "`arrive_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '收到支付回调时间 ms',"
                + "`is_offline_pay` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否离线时支付',"
                + "`item_list` text NULL COMMENT '对应物品列表',"
                + "`had_client_notify_pay` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否客户端已通知支付',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家订单数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(order_id);//order_id
        _size+=8;//pay_id
        _size+=8;//goods_id
        _size+=4;//order_status
        _size+=4;//pay_type
        _size+=8;//create_time_ms
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdk_order_id);//sdk_order_id
        _size+=8;//pay_time_ms
        _size+=8;//arrive_time_ms
        _size+=1;//is_offline_pay
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(item_list);//item_list
        _size+=1;//had_client_notify_pay
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, order_id);
        buff.putLong(pay_id);
        buff.putLong(goods_id);
        buff.putInt(order_status);
        buff.putInt(pay_type);
        buff.putLong(create_time_ms);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sdk_order_id);
        buff.putLong(pay_time_ms);
        buff.putLong(arrive_time_ms);
        buff.put((byte)(is_offline_pay?1:0));
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, item_list);
        buff.put((byte)(had_client_notify_pay?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        order_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        pay_id=buff.getLong();
        goods_id=buff.getLong();
        order_status=buff.getInt();
        pay_type=buff.getInt();
        create_time_ms=buff.getLong();
        sdk_order_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        pay_time_ms=buff.getLong();
        arrive_time_ms=buff.getLong();
        is_offline_pay=(buff.get()==1);
        item_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        had_client_notify_pay=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
