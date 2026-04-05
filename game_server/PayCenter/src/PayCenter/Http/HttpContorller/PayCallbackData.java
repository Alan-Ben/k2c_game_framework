package PayCenter.Http.HttpContorller;

import com.google.gson.JsonObject;

/**
 * PayCallbackData - SDK支付回调数据对象
 * <p>
 * 主要功能：
 * 1. 封装SDK支付回调的所有参数字段
 * 2. 提供类型安全的参数访问方法
 * 3. 简化JSON数据的解析和使用
 * 4. 支持参数验证和默认值处理
 * <p>
 * 设计特点：
 * - 对应SDK回调文档中的所有参数
 * - 提供便利的getter方法
 * - 包含参数有效性验证
 * - 支持从JsonObject构造
 */
public class PayCallbackData
{
    // 基础订单信息
    private int type;                    // 订单类型：1普通订单，3普通订单退款
    private String orderId;              // 订单id(充值平台订单)
    private String appOrderId;           // 应用订单id（项目组订单）
    private String uid;                  // 用户id
    private String serverId;             // 服务器标识
    private long roleId;                 // 角色id
    private String appId;                // 应用id

    // 商品信息
    private long productId;            // 游戏内产品id
    private String productName;          // 游戏内产品名称
    private String amount;               // 商品定价价值
    private String amountType;           // 商品定价货币类型

    // 支付信息
    private String payType;              // 支付方式（钱包聚合平台）
    private long createTime;              // 创建时间（10位时间戳）
    private long payTime;                 // 支付时间（10位时间戳）
    private String extension;            // 扩展参数
    private int sdkType;                 // 订单来源:1正常，2补单，3虚拟充值,4测试订单
    private String tradeId;              // 第三方订单号
    private String skuId;                // 第三方内购产品id
    private String sdkPayId;             // sdk档位ID
    private int purchaseType;            // 购买类型：0普通，1测试，2促销，3奖励
    private String payment;              // 实际支付金额
    private String paymentCode;          // 实际支付货币
    private String channelCode;          // 渠道标识(网页支付时玩家选择的站点标识)
    private String payId;                // 支付ID（钱包ID）
    private int orderType;               // 订单类型：1 内购，2 网页充值，3 福利（虚拟充值）

    // 退款信息
    private int refundSource;            // 退款来源：-1未知,0用户,1开发者(商户),2第三方
    private long refundTime;              // 收到退款通知时间（10位时间戳）

    // 签名
    private String sign;                 // 签名（32位MD5）

    /**
     * 从JsonObject构造PayCallbackData对象
     * @param jsonObject SDK回调的JSON数据
     * @throws IllegalArgumentException 当必需参数缺失时抛出
     */
    public PayCallbackData(JsonObject jsonObject)
    {
        // 基础订单信息（必需）
        this.type = getIntValue(jsonObject, "type", true);
        this.orderId = getStringValue(jsonObject, "order_id", true);
        this.appOrderId = getStringValue(jsonObject, "app_order_id", true);
        this.uid = getStringValue(jsonObject, "uid", true);
        this.serverId = getStringValue(jsonObject, "server_id", true);
        this.roleId = getLongValue(jsonObject, "role_id", true);
        this.appId = getStringValue(jsonObject, "app_id", true);

        // 商品信息（必需）
        this.productId = getLongValue(jsonObject, "product_id", true);
        this.productName = getStringValue(jsonObject, "product_name", false);
        this.amount = getStringValue(jsonObject, "amount", true);
        this.amountType = getStringValue(jsonObject, "amount_type", true);

        // 支付信息
        this.payType = getStringValue(jsonObject, "pay_type", false);
        this.createTime = getLongValue(jsonObject, "create_time", true);
        this.payTime = getLongValue(jsonObject, "pay_time", true);
        this.extension = getStringValue(jsonObject, "extension", false);
        this.sdkType = getIntValue(jsonObject, "sdk_type", true);
        this.tradeId = getStringValue(jsonObject, "trade_id", true);
        this.skuId = getStringValue(jsonObject, "sku_id", true);
        this.sdkPayId = getStringValue(jsonObject, "sdk_pay_id", true);
        this.purchaseType = getIntValue(jsonObject, "purchase_type", true);
        this.payment = getStringValue(jsonObject, "payment", true);
        this.paymentCode = getStringValue(jsonObject, "payment_code", true);
        this.channelCode = getStringValue(jsonObject, "channel_code", true);
        this.payId = getStringValue(jsonObject, "pay_id", true);
        this.orderType = getIntValue(jsonObject, "order_type", true);

        // 退款信息
        this.refundSource = getIntValue(jsonObject, "refund_source", false);
        this.refundTime = getLongValue(jsonObject, "refund_time", false);

        // 签名（必需）
        this.sign = getStringValue(jsonObject, "sign", true);
    }

    /**
     * 从JsonObject安全获取字符串值
     */
    private String getStringValue(JsonObject json, String key, boolean required)
    {
        if (!json.has(key) || json.get(key).isJsonNull())
        {
            if (required)
            {
                throw new IllegalArgumentException("Missing required parameter: " + key);
            }
            return "";
        }
        return json.get(key).getAsString();
    }

    /**
     * 从JsonObject安全获取整数值
     */
    private int getIntValue(JsonObject json, String key, boolean required)
    {
        if (!json.has(key) || json.get(key).isJsonNull())
        {
            if (required)
            {
                throw new IllegalArgumentException("Missing required parameter: " + key);
            }
            return 0;
        }
        return json.get(key).getAsInt();
    }

    /**
     * 从JsonObject安全获取整数值
     */
    private long getLongValue(JsonObject json, String key, boolean required)
    {
        if (!json.has(key) || json.get(key).isJsonNull())
        {
            if (required)
            {
                throw new IllegalArgumentException("Missing required parameter: " + key);
            }
            return 0;
        }
        return json.get(key).getAsLong();
    }

    /**
     * 检查是否为付款成功订单
     */
    public boolean isPaymentSuccess()
    {
        return type == 1;
    }

    /**
     * 检查是否为退款订单
     */
    public boolean isRefund()
    {
        return type == 3;
    }

    // Getter方法
    public int getType()
    {
        return type;
    }

    public String getOrderId()
    {
        return orderId;
    }

    public String getAppOrderId()
    {
        return appOrderId;
    }

    public String getUid()
    {
        return uid;
    }

    public String getServerId()
    {
        return serverId;
    }

    public long getRoleId()
    {
        return roleId;
    }

    public String getAppId()
    {
        return appId;
    }

    public long getProductId()
    {
        return productId;
    }

    public String getProductName()
    {
        return productName;
    }

    public String getAmount()
    {
        return amount;
    }

    public String getAmountType()
    {
        return amountType;
    }

    public String getPayType()
    {
        return payType;
    }

    public long getCreateTime()
    {
        return createTime;
    }

    public long getPayTime()
    {
        return payTime;
    }

    public String getExtension()
    {
        return extension;
    }

    public int getSdkType()
    {
        return sdkType;
    }

    public String getTradeId()
    {
        return tradeId;
    }

    public String getSkuId()
    {
        return skuId;
    }

    public String getSdkPayId()
    {
        return sdkPayId;
    }

    public int getPurchaseType()
    {
        return purchaseType;
    }

    public String getPayment()
    {
        return payment;
    }

    public String getPaymentCode()
    {
        return paymentCode;
    }

    public String getChannelCode()
    {
        return channelCode;
    }

    public String getPayId()
    {
        return payId;
    }

    public int getOrderType()
    {
        return orderType;
    }

    public int getRefundSource()
    {
        return refundSource;
    }

    public long getRefundTime()
    {
        return refundTime;
    }

    public String getSign()
    {
        return sign;
    }

    @Override
    public String toString()
    {
        return String.format("PayCallbackData{type=%d, orderId='%s', appOrderId='%s', uid='%s', serverId='%s', roleId='%s', productId='%s', amount=%.2f}",
                type, orderId, appOrderId, uid, serverId, roleId, productId, amount);
    }
}