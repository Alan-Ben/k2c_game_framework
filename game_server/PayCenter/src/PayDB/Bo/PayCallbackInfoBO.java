package PayDB.Bo;
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
public class PayCallbackInfoBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_order_id =0;
    @DataBaseField(type = "varchar(64)", fieldname = "order_id", comment = "订单id")
    private String order_id;

    public static final int FIELD_sdk_order_id =1;
    @DataBaseField(type = "varchar(32)", fieldname = "sdk_order_id", comment = "SDK订单id")
    private String sdk_order_id;

    public static final int FIELD_uid =2;
    @DataBaseField(type = "varchar(32)", fieldname = "uid", comment = "用户id")
    private String uid;

    public static final int FIELD_cid =3;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "角色id")
    private long cid;

    public static final int FIELD_app_id =4;
    @DataBaseField(type = "varchar(32)", fieldname = "app_id", comment = "应用ID")
    private String app_id;

    public static final int FIELD_product_id =5;
    @DataBaseField(type = "bigint(20)", fieldname = "product_id", comment = "产品ID")
    private long product_id;

    public static final int FIELD_amount =6;
    @DataBaseField(type = "varchar(32)", fieldname = "amount", comment = "商品定价")
    private String amount;

    public static final int FIELD_amount_type =7;
    @DataBaseField(type = "varchar(32)", fieldname = "amount_type", comment = "商品定价货币")
    private String amount_type;

    public static final int FIELD_pay_type =8;
    @DataBaseField(type = "varchar(32)", fieldname = "pay_type", comment = "支付方式（钱包聚合平台）")
    private String pay_type;

    public static final int FIELD_create_time =9;
    @DataBaseField(type = "bigint(20)", fieldname = "create_time", comment = "创建时间（10位时间戳）")
    private long create_time;

    public static final int FIELD_pay_time =10;
    @DataBaseField(type = "bigint(20)", fieldname = "pay_time", comment = "支付时间（10位时间戳）")
    private long pay_time;

    public static final int FIELD_extension =11;
    @DataBaseField(type = "varchar(255)", fieldname = "extension", comment = "扩展参数")
    private String extension;

    public static final int FIELD_sdk_type =12;
    @DataBaseField(type = "int(11)", fieldname = "sdk_type", comment = "订单来源:1正常，2补单，3虚拟充值,4测试订单")
    private int sdk_type;

    public static final int FIELD_trade_id =13;
    @DataBaseField(type = "varchar(64)", fieldname = "trade_id", comment = "第三方订单号")
    private String trade_id;

    public static final int FIELD_sku_id =14;
    @DataBaseField(type = "varchar(64)", fieldname = "sku_id", comment = "第三方内购产品id")
    private String sku_id;

    public static final int FIELD_sdk_pay_id =15;
    @DataBaseField(type = "varchar(32)", fieldname = "sdk_pay_id", comment = "sdk档位ID")
    private String sdk_pay_id;

    public static final int FIELD_purchase_type =16;
    @DataBaseField(type = "int(11)", fieldname = "purchase_type", comment = "购买类型：0普通，1测试，2促销，3奖励")
    private int purchase_type;

    public static final int FIELD_payment =17;
    @DataBaseField(type = "varchar(32)", fieldname = "payment", comment = "实际支付金额")
    private String payment;

    public static final int FIELD_payment_code =18;
    @DataBaseField(type = "varchar(32)", fieldname = "payment_code", comment = "实际支付货币")
    private String payment_code;

    public static final int FIELD_channel_code =19;
    @DataBaseField(type = "varchar(64)", fieldname = "channel_code", comment = "渠道标识(网页支付时玩家选择的站点标识)")
    private String channel_code;

    public static final int FIELD_pay_id =20;
    @DataBaseField(type = "varchar(64)", fieldname = "pay_id", comment = "支付ID（钱包ID）")
    private String pay_id;

    public static final int FIELD_order_type =21;
    @DataBaseField(type = "int(11)", fieldname = "order_type", comment = "订单类型：1 内购，2 网页充值，3 福利（虚拟充值）")
    private int order_type;

    public static final int FIELD_had_push =22;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_push", comment = "是否已推送")
    private boolean had_push;

    public static final int FIELD_delivery_fail =23;
    @DataBaseField(type = "tinyint(1)", fieldname = "delivery_fail", comment = "是否发货失败")
    private boolean delivery_fail;

    public PayCallbackInfoBO() {
        id = 0;
        order_id = "";
        sdk_order_id = "";
        uid = "";
        cid = 0L;
        app_id = "";
        product_id = 0L;
        amount = "";
        amount_type = "";
        pay_type = "";
        create_time = 0L;
        pay_time = 0L;
        extension = "";
        sdk_type = 0;
        trade_id = "";
        sku_id = "";
        sdk_pay_id = "";
        purchase_type = 0;
        payment = "";
        payment_code = "";
        channel_code = "";
        pay_id = "";
        order_type = 0;
        had_push = false;
        delivery_fail = false;
    }

    public PayCallbackInfoBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        order_id = rs.getString(2);
        sdk_order_id = rs.getString(3);
        uid = rs.getString(4);
        cid = rs.getLong(5);
        app_id = rs.getString(6);
        product_id = rs.getLong(7);
        amount = rs.getString(8);
        amount_type = rs.getString(9);
        pay_type = rs.getString(10);
        create_time = rs.getLong(11);
        pay_time = rs.getLong(12);
        extension = rs.getString(13);
        sdk_type = rs.getInt(14);
        trade_id = rs.getString(15);
        sku_id = rs.getString(16);
        sdk_pay_id = rs.getString(17);
        purchase_type = rs.getInt(18);
        payment = rs.getString(19);
        payment_code = rs.getString(20);
        channel_code = rs.getString(21);
        pay_id = rs.getString(22);
        order_type = rs.getInt(23);
        had_push = rs.getBoolean(24);
        delivery_fail = rs.getBoolean(25);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PayCallbackInfoBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `order_id`, `sdk_order_id`, `uid`, `cid`, `app_id`, `product_id`, `amount`, `amount_type`, `pay_type`, `create_time`, `pay_time`, `extension`, `sdk_type`, `trade_id`, `sku_id`, `sdk_pay_id`, `purchase_type`, `payment`, `payment_code`, `channel_code`, `pay_id`, `order_type`, `had_push`, `delivery_fail`";
    }

    @Override
    public String getTableName() {
        return "`pay_callback_info`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(sdk_order_id == null ? null : sdk_order_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(app_id == null ? null : app_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(product_id).append("', ");
        strBuf.append("'").append(amount == null ? null : amount.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(amount_type == null ? null : amount_type.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(pay_type == null ? null : pay_type.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(create_time).append("', ");
        strBuf.append("'").append(pay_time).append("', ");
        strBuf.append("'").append(extension == null ? null : extension.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(sdk_type).append("', ");
        strBuf.append("'").append(trade_id == null ? null : trade_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(sku_id == null ? null : sku_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(sdk_pay_id == null ? null : sdk_pay_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(purchase_type).append("', ");
        strBuf.append("'").append(payment == null ? null : payment.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(payment_code == null ? null : payment_code.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(channel_code == null ? null : channel_code.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(pay_id == null ? null : pay_id.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(order_type).append("', ");
        strBuf.append("'").append(had_push ? 1 : 0).append("', ");
        strBuf.append("'").append(delivery_fail ? 1 : 0).append("', ");
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

    // 订单id
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

    // SDK订单id
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

    // 用户id
    public String getUid() { return this.uid; }
    public void setUid(BM _bm, String uid) {
        if(uid.equals(this.uid)) 
            return;
        this.uid = uid; 
        markField(_bm, FIELD_uid); 
    }
    public void saveUid(BM _bm, String uid) {
        if(uid.equals(this.uid)) 
            return;
        this.uid = uid;
        saveField(_bm, "uid", uid);
    }

    // 角色id
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

    // 应用ID
    public String getAppId() { return this.app_id; }
    public void setAppId(BM _bm, String app_id) {
        if(app_id.equals(this.app_id)) 
            return;
        this.app_id = app_id; 
        markField(_bm, FIELD_app_id); 
    }
    public void saveAppId(BM _bm, String app_id) {
        if(app_id.equals(this.app_id)) 
            return;
        this.app_id = app_id;
        saveField(_bm, "app_id", app_id);
    }

    // 产品ID
    public long getProductId() { return this.product_id; }
    public void setProductId(BM _bm, long product_id) {
        if(product_id==this.product_id) 
            return;
        this.product_id = product_id; 
        markField(_bm, FIELD_product_id); 
    }
    public void saveProductId(BM _bm, long product_id) {
        if(product_id==this.product_id) 
            return;
        this.product_id = product_id;
        saveField(_bm, "product_id", product_id);
    }

    // 商品定价
    public String getAmount() { return this.amount; }
    public void setAmount(BM _bm, String amount) {
        if(amount.equals(this.amount)) 
            return;
        this.amount = amount; 
        markField(_bm, FIELD_amount); 
    }
    public void saveAmount(BM _bm, String amount) {
        if(amount.equals(this.amount)) 
            return;
        this.amount = amount;
        saveField(_bm, "amount", amount);
    }

    // 商品定价货币
    public String getAmountType() { return this.amount_type; }
    public void setAmountType(BM _bm, String amount_type) {
        if(amount_type.equals(this.amount_type)) 
            return;
        this.amount_type = amount_type; 
        markField(_bm, FIELD_amount_type); 
    }
    public void saveAmountType(BM _bm, String amount_type) {
        if(amount_type.equals(this.amount_type)) 
            return;
        this.amount_type = amount_type;
        saveField(_bm, "amount_type", amount_type);
    }

    // 支付方式（钱包聚合平台）
    public String getPayType() { return this.pay_type; }
    public void setPayType(BM _bm, String pay_type) {
        if(pay_type.equals(this.pay_type)) 
            return;
        this.pay_type = pay_type; 
        markField(_bm, FIELD_pay_type); 
    }
    public void savePayType(BM _bm, String pay_type) {
        if(pay_type.equals(this.pay_type)) 
            return;
        this.pay_type = pay_type;
        saveField(_bm, "pay_type", pay_type);
    }

    // 创建时间（10位时间戳）
    public long getCreateTime() { return this.create_time; }
    public void setCreateTime(BM _bm, long create_time) {
        if(create_time==this.create_time) 
            return;
        this.create_time = create_time; 
        markField(_bm, FIELD_create_time); 
    }
    public void saveCreateTime(BM _bm, long create_time) {
        if(create_time==this.create_time) 
            return;
        this.create_time = create_time;
        saveField(_bm, "create_time", create_time);
    }

    // 支付时间（10位时间戳）
    public long getPayTime() { return this.pay_time; }
    public void setPayTime(BM _bm, long pay_time) {
        if(pay_time==this.pay_time) 
            return;
        this.pay_time = pay_time; 
        markField(_bm, FIELD_pay_time); 
    }
    public void savePayTime(BM _bm, long pay_time) {
        if(pay_time==this.pay_time) 
            return;
        this.pay_time = pay_time;
        saveField(_bm, "pay_time", pay_time);
    }

    // 扩展参数
    public String getExtension() { return this.extension; }
    public void setExtension(BM _bm, String extension) {
        if(extension.equals(this.extension)) 
            return;
        this.extension = extension; 
        markField(_bm, FIELD_extension); 
    }
    public void saveExtension(BM _bm, String extension) {
        if(extension.equals(this.extension)) 
            return;
        this.extension = extension;
        saveField(_bm, "extension", extension);
    }

    // 订单来源:1正常，2补单，3虚拟充值,4测试订单
    public int getSdkType() { return this.sdk_type; }
    public void setSdkType(BM _bm, int sdk_type) {
        if(sdk_type==this.sdk_type) 
            return;
        this.sdk_type = sdk_type; 
        markField(_bm, FIELD_sdk_type); 
    }
    public void saveSdkType(BM _bm, int sdk_type) {
        if(sdk_type==this.sdk_type) 
            return;
        this.sdk_type = sdk_type;
        saveField(_bm, "sdk_type", sdk_type);
    }

    // 第三方订单号
    public String getTradeId() { return this.trade_id; }
    public void setTradeId(BM _bm, String trade_id) {
        if(trade_id.equals(this.trade_id)) 
            return;
        this.trade_id = trade_id; 
        markField(_bm, FIELD_trade_id); 
    }
    public void saveTradeId(BM _bm, String trade_id) {
        if(trade_id.equals(this.trade_id)) 
            return;
        this.trade_id = trade_id;
        saveField(_bm, "trade_id", trade_id);
    }

    // 第三方内购产品id
    public String getSkuId() { return this.sku_id; }
    public void setSkuId(BM _bm, String sku_id) {
        if(sku_id.equals(this.sku_id)) 
            return;
        this.sku_id = sku_id; 
        markField(_bm, FIELD_sku_id); 
    }
    public void saveSkuId(BM _bm, String sku_id) {
        if(sku_id.equals(this.sku_id)) 
            return;
        this.sku_id = sku_id;
        saveField(_bm, "sku_id", sku_id);
    }

    // sdk档位ID
    public String getSdkPayId() { return this.sdk_pay_id; }
    public void setSdkPayId(BM _bm, String sdk_pay_id) {
        if(sdk_pay_id.equals(this.sdk_pay_id)) 
            return;
        this.sdk_pay_id = sdk_pay_id; 
        markField(_bm, FIELD_sdk_pay_id); 
    }
    public void saveSdkPayId(BM _bm, String sdk_pay_id) {
        if(sdk_pay_id.equals(this.sdk_pay_id)) 
            return;
        this.sdk_pay_id = sdk_pay_id;
        saveField(_bm, "sdk_pay_id", sdk_pay_id);
    }

    // 购买类型：0普通，1测试，2促销，3奖励
    public int getPurchaseType() { return this.purchase_type; }
    public void setPurchaseType(BM _bm, int purchase_type) {
        if(purchase_type==this.purchase_type) 
            return;
        this.purchase_type = purchase_type; 
        markField(_bm, FIELD_purchase_type); 
    }
    public void savePurchaseType(BM _bm, int purchase_type) {
        if(purchase_type==this.purchase_type) 
            return;
        this.purchase_type = purchase_type;
        saveField(_bm, "purchase_type", purchase_type);
    }

    // 实际支付金额
    public String getPayment() { return this.payment; }
    public void setPayment(BM _bm, String payment) {
        if(payment.equals(this.payment)) 
            return;
        this.payment = payment; 
        markField(_bm, FIELD_payment); 
    }
    public void savePayment(BM _bm, String payment) {
        if(payment.equals(this.payment)) 
            return;
        this.payment = payment;
        saveField(_bm, "payment", payment);
    }

    // 实际支付货币
    public String getPaymentCode() { return this.payment_code; }
    public void setPaymentCode(BM _bm, String payment_code) {
        if(payment_code.equals(this.payment_code)) 
            return;
        this.payment_code = payment_code; 
        markField(_bm, FIELD_payment_code); 
    }
    public void savePaymentCode(BM _bm, String payment_code) {
        if(payment_code.equals(this.payment_code)) 
            return;
        this.payment_code = payment_code;
        saveField(_bm, "payment_code", payment_code);
    }

    // 渠道标识(网页支付时玩家选择的站点标识)
    public String getChannelCode() { return this.channel_code; }
    public void setChannelCode(BM _bm, String channel_code) {
        if(channel_code.equals(this.channel_code)) 
            return;
        this.channel_code = channel_code; 
        markField(_bm, FIELD_channel_code); 
    }
    public void saveChannelCode(BM _bm, String channel_code) {
        if(channel_code.equals(this.channel_code)) 
            return;
        this.channel_code = channel_code;
        saveField(_bm, "channel_code", channel_code);
    }

    // 支付ID（钱包ID）
    public String getPayId() { return this.pay_id; }
    public void setPayId(BM _bm, String pay_id) {
        if(pay_id.equals(this.pay_id)) 
            return;
        this.pay_id = pay_id; 
        markField(_bm, FIELD_pay_id); 
    }
    public void savePayId(BM _bm, String pay_id) {
        if(pay_id.equals(this.pay_id)) 
            return;
        this.pay_id = pay_id;
        saveField(_bm, "pay_id", pay_id);
    }

    // 订单类型：1 内购，2 网页充值，3 福利（虚拟充值）
    public int getOrderType() { return this.order_type; }
    public void setOrderType(BM _bm, int order_type) {
        if(order_type==this.order_type) 
            return;
        this.order_type = order_type; 
        markField(_bm, FIELD_order_type); 
    }
    public void saveOrderType(BM _bm, int order_type) {
        if(order_type==this.order_type) 
            return;
        this.order_type = order_type;
        saveField(_bm, "order_type", order_type);
    }

    // 是否已推送
    public boolean getHadPush() { return this.had_push; }
    public void setHadPush(BM _bm, boolean had_push) {
        if(had_push==this.had_push) 
            return;
        this.had_push = had_push; 
        markField(_bm, FIELD_had_push); 
    }
    public void saveHadPush(BM _bm, boolean had_push) {
        if(had_push==this.had_push) 
            return;
        this.had_push = had_push;
        saveField(_bm, "had_push", had_push ? 1 : 0);
    }

    // 是否发货失败
    public boolean getDeliveryFail() { return this.delivery_fail; }
    public void setDeliveryFail(BM _bm, boolean delivery_fail) {
        if(delivery_fail==this.delivery_fail) 
            return;
        this.delivery_fail = delivery_fail; 
        markField(_bm, FIELD_delivery_fail); 
    }
    public void saveDeliveryFail(BM _bm, boolean delivery_fail) {
        if(delivery_fail==this.delivery_fail) 
            return;
        this.delivery_fail = delivery_fail;
        saveField(_bm, "delivery_fail", delivery_fail ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `order_id` = '").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `sdk_order_id` = '").append(sdk_order_id == null ? null : sdk_order_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `app_id` = '").append(app_id == null ? null : app_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `product_id` = '").append(product_id).append("',");
        sBuilder.append(" `amount` = '").append(amount == null ? null : amount.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `amount_type` = '").append(amount_type == null ? null : amount_type.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `pay_type` = '").append(pay_type == null ? null : pay_type.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `create_time` = '").append(create_time).append("',");
        sBuilder.append(" `pay_time` = '").append(pay_time).append("',");
        sBuilder.append(" `extension` = '").append(extension == null ? null : extension.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `sdk_type` = '").append(sdk_type).append("',");
        sBuilder.append(" `trade_id` = '").append(trade_id == null ? null : trade_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `sku_id` = '").append(sku_id == null ? null : sku_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `sdk_pay_id` = '").append(sdk_pay_id == null ? null : sdk_pay_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `purchase_type` = '").append(purchase_type).append("',");
        sBuilder.append(" `payment` = '").append(payment == null ? null : payment.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `payment_code` = '").append(payment_code == null ? null : payment_code.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `channel_code` = '").append(channel_code == null ? null : channel_code.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `pay_id` = '").append(pay_id == null ? null : pay_id.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `order_type` = '").append(order_type).append("',");
        sBuilder.append(" `had_push` = '").append(had_push ? 1 : 0).append("',");
        sBuilder.append(" `delivery_fail` = '").append(delivery_fail ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_order_id)) sBuilder.append(" `order_id` = '").append(order_id == null ? null : order_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_sdk_order_id)) sBuilder.append(" `sdk_order_id` = '").append(sdk_order_id == null ? null : sdk_order_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid == null ? null : uid.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_app_id)) sBuilder.append(" `app_id` = '").append(app_id == null ? null : app_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_product_id)) sBuilder.append(" `product_id` = '").append(product_id).append("',");
        if(isFieldMarked(FIELD_amount)) sBuilder.append(" `amount` = '").append(amount == null ? null : amount.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_amount_type)) sBuilder.append(" `amount_type` = '").append(amount_type == null ? null : amount_type.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_pay_type)) sBuilder.append(" `pay_type` = '").append(pay_type == null ? null : pay_type.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_create_time)) sBuilder.append(" `create_time` = '").append(create_time).append("',");
        if(isFieldMarked(FIELD_pay_time)) sBuilder.append(" `pay_time` = '").append(pay_time).append("',");
        if(isFieldMarked(FIELD_extension)) sBuilder.append(" `extension` = '").append(extension == null ? null : extension.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_sdk_type)) sBuilder.append(" `sdk_type` = '").append(sdk_type).append("',");
        if(isFieldMarked(FIELD_trade_id)) sBuilder.append(" `trade_id` = '").append(trade_id == null ? null : trade_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_sku_id)) sBuilder.append(" `sku_id` = '").append(sku_id == null ? null : sku_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_sdk_pay_id)) sBuilder.append(" `sdk_pay_id` = '").append(sdk_pay_id == null ? null : sdk_pay_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_purchase_type)) sBuilder.append(" `purchase_type` = '").append(purchase_type).append("',");
        if(isFieldMarked(FIELD_payment)) sBuilder.append(" `payment` = '").append(payment == null ? null : payment.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_payment_code)) sBuilder.append(" `payment_code` = '").append(payment_code == null ? null : payment_code.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_channel_code)) sBuilder.append(" `channel_code` = '").append(channel_code == null ? null : channel_code.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_pay_id)) sBuilder.append(" `pay_id` = '").append(pay_id == null ? null : pay_id.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_order_type)) sBuilder.append(" `order_type` = '").append(order_type).append("',");
        if(isFieldMarked(FIELD_had_push)) sBuilder.append(" `had_push` = '").append(had_push ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_delivery_fail)) sBuilder.append(" `delivery_fail` = '").append(delivery_fail ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `pay_callback_info` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`order_id` varchar(64) NOT NULL DEFAULT '' COMMENT '订单id',"
                + "`sdk_order_id` varchar(32) NOT NULL DEFAULT '' COMMENT 'SDK订单id',"
                + "`uid` varchar(32) NOT NULL DEFAULT '' COMMENT '用户id',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '角色id',"
                + "`app_id` varchar(32) NOT NULL DEFAULT '' COMMENT '应用ID',"
                + "`product_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '产品ID',"
                + "`amount` varchar(32) NOT NULL DEFAULT '' COMMENT '商品定价',"
                + "`amount_type` varchar(32) NOT NULL DEFAULT '' COMMENT '商品定价货币',"
                + "`pay_type` varchar(32) NOT NULL DEFAULT '' COMMENT '支付方式（钱包聚合平台）',"
                + "`create_time` bigint(20) NOT NULL DEFAULT '0' COMMENT '创建时间（10位时间戳）',"
                + "`pay_time` bigint(20) NOT NULL DEFAULT '0' COMMENT '支付时间（10位时间戳）',"
                + "`extension` varchar(255) NOT NULL DEFAULT '' COMMENT '扩展参数',"
                + "`sdk_type` int(11) NOT NULL DEFAULT '0' COMMENT '订单来源:1正常，2补单，3虚拟充值,4测试订单',"
                + "`trade_id` varchar(64) NOT NULL DEFAULT '' COMMENT '第三方订单号',"
                + "`sku_id` varchar(64) NOT NULL DEFAULT '' COMMENT '第三方内购产品id',"
                + "`sdk_pay_id` varchar(32) NOT NULL DEFAULT '' COMMENT 'sdk档位ID',"
                + "`purchase_type` int(11) NOT NULL DEFAULT '0' COMMENT '购买类型：0普通，1测试，2促销，3奖励',"
                + "`payment` varchar(32) NOT NULL DEFAULT '' COMMENT '实际支付金额',"
                + "`payment_code` varchar(32) NOT NULL DEFAULT '' COMMENT '实际支付货币',"
                + "`channel_code` varchar(64) NOT NULL DEFAULT '' COMMENT '渠道标识(网页支付时玩家选择的站点标识)',"
                + "`pay_id` varchar(64) NOT NULL DEFAULT '' COMMENT '支付ID（钱包ID）',"
                + "`order_type` int(11) NOT NULL DEFAULT '0' COMMENT '订单类型：1 内购，2 网页充值，3 福利（虚拟充值）',"
                + "`had_push` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已推送',"
                + "`delivery_fail` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否发货失败',"
                + "KEY `order_id` (`order_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='支付回调信息记录表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.pc_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(order_id);//order_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdk_order_id);//sdk_order_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);//uid
        _size+=8;//cid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(app_id);//app_id
        _size+=8;//product_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(amount);//amount
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(amount_type);//amount_type
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(pay_type);//pay_type
        _size+=8;//create_time
        _size+=8;//pay_time
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(extension);//extension
        _size+=4;//sdk_type
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(trade_id);//trade_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sku_id);//sku_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdk_pay_id);//sdk_pay_id
        _size+=4;//purchase_type
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payment);//payment
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payment_code);//payment_code
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channel_code);//channel_code
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(pay_id);//pay_id
        _size+=4;//order_type
        _size+=1;//had_push
        _size+=1;//delivery_fail
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, order_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sdk_order_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, uid);
        buff.putLong(cid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, app_id);
        buff.putLong(product_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, amount);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, amount_type);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, pay_type);
        buff.putLong(create_time);
        buff.putLong(pay_time);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, extension);
        buff.putInt(sdk_type);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, trade_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sku_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, sdk_pay_id);
        buff.putInt(purchase_type);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, payment);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, payment_code);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, channel_code);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, pay_id);
        buff.putInt(order_type);
        buff.put((byte)(had_push?1:0));
        buff.put((byte)(delivery_fail?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        order_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        sdk_order_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        uid=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        cid=buff.getLong();
        app_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        product_id=buff.getLong();
        amount=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        amount_type=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        pay_type=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        create_time=buff.getLong();
        pay_time=buff.getLong();
        extension=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        sdk_type=buff.getInt();
        trade_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        sku_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        sdk_pay_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        purchase_type=buff.getInt();
        payment=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        payment_code=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        channel_code=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        pay_id=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        order_type=buff.getInt();
        had_push=(buff.get()==1);
        delivery_fail=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
