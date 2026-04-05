package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 支付回调信息
 **/
public class ServerObj_PayCallbackInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据库ID */
private long dbId;
/** 订单号 */
private String orderId;
/** SDK订单号 */
private String sdkOrderId;
/** 角色ID */
private long cid;
/** 用户ID */
private String uid;
/** 应用ID */
private String appId;
/** 产品ID */
private long productId;
/** 商品定价 */
private String amount;
/** 商品定价货币 */
private String amountType;
/** 支付方式（钱包聚合平台） */
private String payType;
/** 创建时间（10位时间戳） */
private long createTime;
/** 支付时间（10位时间戳） */
private long payTime;
/** 订单来源:1正常，2补单，3虚拟充值,4测试订单 */
private int sdkType;
/** 第三方订单号 */
private String tradeId;
/** 第三方内购产品id */
private String skuId;
/** sdk档位ID */
private String sdkPayId;
/** 购买类型：0普通，1测试，2促销，3奖励 */
private int purchaseType;
/** 实际支付金额 */
private String payment;
/** 实际支付货币 */
private String paymentCode;
/** 渠道标识(网页支付时玩家选择的站点标识) */
private String channelCode;
/** 支付ID（钱包ID） */
private String payId;
/** 订单类型：1 内购，2 网页充值，3 福利（虚拟充值） */
private int orderType;


public ServerObj_PayCallbackInfo() {
	dbId = (long)0;
	orderId = "";
	sdkOrderId = "";
	cid = (long)0;
	uid = "";
	appId = "";
	productId = (long)0;
	amount = "";
	amountType = "";
	payType = "";
	createTime = (long)0;
	payTime = (long)0;
	sdkType = 0;
	tradeId = "";
	skuId = "";
	sdkPayId = "";
	purchaseType = 0;
	payment = "";
	paymentCode = "";
	channelCode = "";
	payId = "";
	orderType = 0;
}

public ServerObj_PayCallbackInfo(
	 long _dbId
	, String _orderId
	, String _sdkOrderId
	, long _cid
	, String _uid
	, String _appId
	, long _productId
	, String _amount
	, String _amountType
	, String _payType
	, long _createTime
	, long _payTime
	, int _sdkType
	, String _tradeId
	, String _skuId
	, String _sdkPayId
	, int _purchaseType
	, String _payment
	, String _paymentCode
	, String _channelCode
	, String _payId
	, int _orderType
) {	dbId = _dbId;
	orderId = _orderId;
	sdkOrderId = _sdkOrderId;
	cid = _cid;
	uid = _uid;
	appId = _appId;
	productId = _productId;
	amount = _amount;
	amountType = _amountType;
	payType = _payType;
	createTime = _createTime;
	payTime = _payTime;
	sdkType = _sdkType;
	tradeId = _tradeId;
	skuId = _skuId;
	sdkPayId = _sdkPayId;
	purchaseType = _purchaseType;
	payment = _payment;
	paymentCode = _paymentCode;
	channelCode = _channelCode;
	payId = _payId;
	orderType = _orderType;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数据库ID */
public long getDbId() { return dbId; }
/** 数据库ID */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 订单号 */
public String getOrderId() { return orderId; }
/** 订单号 */
public void setOrderId(String _orderId) { orderId = _orderId; }
/** SDK订单号 */
public String getSdkOrderId() { return sdkOrderId; }
/** SDK订单号 */
public void setSdkOrderId(String _sdkOrderId) { sdkOrderId = _sdkOrderId; }
/** 角色ID */
public long getCid() { return cid; }
/** 角色ID */
public void setCid(long _cid) { cid = _cid; }
/** 用户ID */
public String getUid() { return uid; }
/** 用户ID */
public void setUid(String _uid) { uid = _uid; }
/** 应用ID */
public String getAppId() { return appId; }
/** 应用ID */
public void setAppId(String _appId) { appId = _appId; }
/** 产品ID */
public long getProductId() { return productId; }
/** 产品ID */
public void setProductId(long _productId) { productId = _productId; }
/** 商品定价 */
public String getAmount() { return amount; }
/** 商品定价 */
public void setAmount(String _amount) { amount = _amount; }
/** 商品定价货币 */
public String getAmountType() { return amountType; }
/** 商品定价货币 */
public void setAmountType(String _amountType) { amountType = _amountType; }
/** 支付方式（钱包聚合平台） */
public String getPayType() { return payType; }
/** 支付方式（钱包聚合平台） */
public void setPayType(String _payType) { payType = _payType; }
/** 创建时间（10位时间戳） */
public long getCreateTime() { return createTime; }
/** 创建时间（10位时间戳） */
public void setCreateTime(long _createTime) { createTime = _createTime; }
/** 支付时间（10位时间戳） */
public long getPayTime() { return payTime; }
/** 支付时间（10位时间戳） */
public void setPayTime(long _payTime) { payTime = _payTime; }
/** 订单来源:1正常，2补单，3虚拟充值,4测试订单 */
public int getSdkType() { return sdkType; }
/** 订单来源:1正常，2补单，3虚拟充值,4测试订单 */
public void setSdkType(int _sdkType) { sdkType = _sdkType; }
/** 第三方订单号 */
public String getTradeId() { return tradeId; }
/** 第三方订单号 */
public void setTradeId(String _tradeId) { tradeId = _tradeId; }
/** 第三方内购产品id */
public String getSkuId() { return skuId; }
/** 第三方内购产品id */
public void setSkuId(String _skuId) { skuId = _skuId; }
/** sdk档位ID */
public String getSdkPayId() { return sdkPayId; }
/** sdk档位ID */
public void setSdkPayId(String _sdkPayId) { sdkPayId = _sdkPayId; }
/** 购买类型：0普通，1测试，2促销，3奖励 */
public int getPurchaseType() { return purchaseType; }
/** 购买类型：0普通，1测试，2促销，3奖励 */
public void setPurchaseType(int _purchaseType) { purchaseType = _purchaseType; }
/** 实际支付金额 */
public String getPayment() { return payment; }
/** 实际支付金额 */
public void setPayment(String _payment) { payment = _payment; }
/** 实际支付货币 */
public String getPaymentCode() { return paymentCode; }
/** 实际支付货币 */
public void setPaymentCode(String _paymentCode) { paymentCode = _paymentCode; }
/** 渠道标识(网页支付时玩家选择的站点标识) */
public String getChannelCode() { return channelCode; }
/** 渠道标识(网页支付时玩家选择的站点标识) */
public void setChannelCode(String _channelCode) { channelCode = _channelCode; }
/** 支付ID（钱包ID） */
public String getPayId() { return payId; }
/** 支付ID（钱包ID） */
public void setPayId(String _payId) { payId = _payId; }
/** 订单类型：1 内购，2 网页充值，3 福利（虚拟充值） */
public int getOrderType() { return orderType; }
/** 订单类型：1 内购，2 网页充值，3 福利（虚拟充值） */
public void setOrderType(int _orderType) { orderType = _orderType; }


public final int GetBufSize() {
	int _size = 52;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkOrderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(appId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(amount);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(amountType);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payType);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(tradeId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(skuId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkPayId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payment);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(paymentCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payId);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 54;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkOrderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(appId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(amount);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(amountType);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payType);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(tradeId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(skuId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkPayId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payment);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(paymentCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelCode);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payId);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) orderId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sdkOrderId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) appId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) productId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) amount = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) amountType = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) payType = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTime = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) payTime = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sdkType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) tradeId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skuId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sdkPayId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) purchaseType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) payment = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) paymentCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) channelCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) payId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) orderType = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, orderId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, sdkOrderId);
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, appId);
	_buf.putLong(productId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, amount);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, amountType);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, payType);
	_buf.putLong(createTime);
	_buf.putLong(payTime);
	_buf.putInt(sdkType);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, tradeId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, skuId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, sdkPayId);
	_buf.putInt(purchaseType);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, payment);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, paymentCode);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, channelCode);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, payId);
	_buf.putInt(orderType);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

