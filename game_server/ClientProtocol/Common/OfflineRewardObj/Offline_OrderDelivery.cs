using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.OfflineRewardObj
{

/// <summary>
/// 订单发货
/// </summary>
public class Offline_OrderDelivery : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 订单ID
/// </summary>
private string orderId;
/// <summary>
/// 礼包ID
/// </summary>
private long giftPackId;
/// <summary>
/// 物品列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> itemList;
/// <summary>
/// sdk订单ID
/// </summary>
private string sdkOrderId;
/// <summary>
/// 支付金额
/// </summary>
private float payMoney;
/// <summary>
/// 支付币种
/// </summary>
private string payCurrency;
/// <summary>
/// 订单类型：1 内购，2 网页充值，3 福利（虚拟充值）
/// </summary>
private short orderType;


public Offline_OrderDelivery() {
	orderId = "";
	giftPackId = (long)0;
	itemList = new List<NPCommon.NPCommon_ItemInfo>();
	sdkOrderId = "";
	payMoney = 0f;
	payCurrency = "";
	orderType = (short)0;
}

public Offline_OrderDelivery(
	string _orderId
	, long _giftPackId
	, List<NPCommon.NPCommon_ItemInfo> _itemList
	, string _sdkOrderId
	, float _payMoney
	, string _payCurrency
	, short _orderType
) {	orderId = _orderId;
	giftPackId = _giftPackId;
	itemList = _itemList;
	sdkOrderId = _sdkOrderId;
	payMoney = _payMoney;
	payCurrency = _payCurrency;
	orderType = _orderType;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 订单ID
/// </summary>
public string getOrderId() { return orderId; }
/// <summary>
/// 订单ID
/// </summary>
public void setOrderId(string _orderId) { orderId = _orderId; }
/// <summary>
/// 礼包ID
/// </summary>
public long getGiftPackId() { return giftPackId; }
/// <summary>
/// 礼包ID
/// </summary>
public void setGiftPackId(long _giftPackId) { giftPackId = _giftPackId; }
/// <summary>
/// 物品列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/// <summary>
/// 物品列表
/// </summary>
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.Add(_itemList); }
/// <summary>
/// sdk订单ID
/// </summary>
public string getSdkOrderId() { return sdkOrderId; }
/// <summary>
/// sdk订单ID
/// </summary>
public void setSdkOrderId(string _sdkOrderId) { sdkOrderId = _sdkOrderId; }
/// <summary>
/// 支付金额
/// </summary>
public float getPayMoney() { return payMoney; }
/// <summary>
/// 支付金额
/// </summary>
public void setPayMoney(float _payMoney) { payMoney = _payMoney; }
/// <summary>
/// 支付币种
/// </summary>
public string getPayCurrency() { return payCurrency; }
/// <summary>
/// 支付币种
/// </summary>
public void setPayCurrency(string _payCurrency) { payCurrency = _payCurrency; }
/// <summary>
/// 订单类型：1 内购，2 网页充值，3 福利（虚拟充值）
/// </summary>
public short getOrderType() { return orderType; }
/// <summary>
/// 订单类型：1 内购，2 网页充值，3 福利（虚拟充值）
/// </summary>
public void setOrderType(short _orderType) { orderType = _orderType; }


public int GetBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkOrderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payCurrency);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkOrderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(payCurrency);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	orderId = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	giftPackId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.getCurPos();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.setPosition(__itemListCurPos + __itemListCustLen);

		itemList.Add(_itemList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sdkOrderId = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	payMoney = _buf.getFloat();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	payCurrency = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	orderType = _buf.getShort();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(orderId);
	_buf.putLong(giftPackId);
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
	_buf.putString(sdkOrderId);
	_buf.putFloat(payMoney);
	_buf.putString(payCurrency);
	_buf.putShort(orderType);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("orderId").Append(":").Append(orderId.ToString()).Append(", ");
	builder.Append("giftPackId").Append(":").Append(giftPackId.ToString()).Append(", ");
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("sdkOrderId").Append(":").Append(sdkOrderId.ToString()).Append(", ");
	builder.Append("payMoney").Append(":").Append(payMoney.ToString()).Append(", ");
	builder.Append("payCurrency").Append(":").Append(payCurrency.ToString()).Append(", ");
	builder.Append("orderType").Append(":").Append(orderType.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

