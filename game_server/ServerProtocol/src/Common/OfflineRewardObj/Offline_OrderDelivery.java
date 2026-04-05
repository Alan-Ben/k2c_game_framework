package Common.OfflineRewardObj;

import java.nio.ByteBuffer;
/*********
 * 订单发货
 **/
public class Offline_OrderDelivery implements ALBasicProtocolPack._IALProtocolStructure {
/** 订单ID */
private String orderId;
/** 礼包ID */
private long giftPackId;
/** 物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> itemList;
/** sdk订单ID */
private String sdkOrderId;
/** 支付金额 */
private float payMoney;
/** 支付币种 */
private String payCurrency;
/** 订单类型：1 内购，2 网页充值，3 福利（虚拟充值） */
private short orderType;


public Offline_OrderDelivery() {
	orderId = "";
	giftPackId = (long)0;
	itemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	sdkOrderId = "";
	payMoney = 0f;
	payCurrency = "";
	orderType = (short)0;
}

public Offline_OrderDelivery(
	 String _orderId
	, long _giftPackId
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _itemList
	, String _sdkOrderId
	, float _payMoney
	, String _payCurrency
	, short _orderType
) {	orderId = _orderId;
	giftPackId = _giftPackId;
	itemList = _itemList;
	sdkOrderId = _sdkOrderId;
	payMoney = _payMoney;
	payCurrency = _payCurrency;
	orderType = _orderType;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 订单ID */
public String getOrderId() { return orderId; }
/** 订单ID */
public void setOrderId(String _orderId) { orderId = _orderId; }
/** 礼包ID */
public long getGiftPackId() { return giftPackId; }
/** 礼包ID */
public void setGiftPackId(long _giftPackId) { giftPackId = _giftPackId; }
/** 物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/** 物品列表 */
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.add(_itemList); }
/** sdk订单ID */
public String getSdkOrderId() { return sdkOrderId; }
/** sdk订单ID */
public void setSdkOrderId(String _sdkOrderId) { sdkOrderId = _sdkOrderId; }
/** 支付金额 */
public float getPayMoney() { return payMoney; }
/** 支付金额 */
public void setPayMoney(float _payMoney) { payMoney = _payMoney; }
/** 支付币种 */
public String getPayCurrency() { return payCurrency; }
/** 支付币种 */
public void setPayCurrency(String _payCurrency) { payCurrency = _payCurrency; }
/** 订单类型：1 内购，2 网页充值，3 福利（虚拟充值） */
public short getOrderType() { return orderType; }
/** 订单类型：1 内购，2 网页充值，3 福利（虚拟充值） */
public void setOrderType(short _orderType) { orderType = _orderType; }


public final int GetBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkOrderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payCurrency);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkOrderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payCurrency);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) orderId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) giftPackId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.position();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.position(__itemListCurPos + __itemListCustLen);

		itemList.add(_itemList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sdkOrderId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) payMoney = _buf.getFloat();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) payCurrency = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) orderType = _buf.getShort();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, orderId);
	_buf.putLong(giftPackId);
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, sdkOrderId);
	_buf.putFloat(payMoney);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, payCurrency);
	_buf.putShort(orderType);
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

