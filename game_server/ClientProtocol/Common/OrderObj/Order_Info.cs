using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.OrderObj
{

/// <summary>
/// 订单信息
/// </summary>
public class Order_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 订单号
/// </summary>
private string orderId;
/// <summary>
/// 支付id
/// </summary>
private long payId;
/// <summary>
/// 礼包id
/// </summary>
private long giftPackId;
/// <summary>
/// 订单状态
/// </summary>
private CommonEnum.EOrderStatus status;
/// <summary>
/// 创建时间 ms
/// </summary>
private long createTimeMs;
/// <summary>
/// 支付时间 ms
/// </summary>
private long payTimeMs;


public Order_Info() {
	orderId = "";
	payId = (long)0;
	giftPackId = (long)0;
	status = 0;
	createTimeMs = (long)0;
	payTimeMs = (long)0;
}

public Order_Info(
	string _orderId
	, long _payId
	, long _giftPackId
	, CommonEnum.EOrderStatus _status
	, long _createTimeMs
	, long _payTimeMs
) {	orderId = _orderId;
	payId = _payId;
	giftPackId = _giftPackId;
	status = _status;
	createTimeMs = _createTimeMs;
	payTimeMs = _payTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 订单号
/// </summary>
public string getOrderId() { return orderId; }
/// <summary>
/// 订单号
/// </summary>
public void setOrderId(string _orderId) { orderId = _orderId; }
/// <summary>
/// 支付id
/// </summary>
public long getPayId() { return payId; }
/// <summary>
/// 支付id
/// </summary>
public void setPayId(long _payId) { payId = _payId; }
/// <summary>
/// 礼包id
/// </summary>
public long getGiftPackId() { return giftPackId; }
/// <summary>
/// 礼包id
/// </summary>
public void setGiftPackId(long _giftPackId) { giftPackId = _giftPackId; }
/// <summary>
/// 订单状态
/// </summary>
public CommonEnum.EOrderStatus getStatus() { return status; }
/// <summary>
/// 订单状态
/// </summary>
public void setStatus(CommonEnum.EOrderStatus _status) { status = _status; }
/// <summary>
/// 创建时间 ms
/// </summary>
public long getCreateTimeMs() { return createTimeMs; }
/// <summary>
/// 创建时间 ms
/// </summary>
public void setCreateTimeMs(long _createTimeMs) { createTimeMs = _createTimeMs; }
/// <summary>
/// 支付时间 ms
/// </summary>
public long getPayTimeMs() { return payTimeMs; }
/// <summary>
/// 支付时间 ms
/// </summary>
public void setPayTimeMs(long _payTimeMs) { payTimeMs = _payTimeMs; }


public int GetBufSize() {
	int _size = 36;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	orderId = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	payId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	giftPackId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	status = (CommonEnum.EOrderStatus)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	payTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(orderId);
	_buf.putLong(payId);
	_buf.putLong(giftPackId);
	_buf.putInt((int)status);

	_buf.putLong(createTimeMs);
	_buf.putLong(payTimeMs);
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
	builder.Append("payId").Append(":").Append(payId.ToString()).Append(", ");
	builder.Append("giftPackId").Append(":").Append(giftPackId.ToString()).Append(", ");
	builder.Append("status").Append(":").Append(status.ToString()).Append(", ");
	builder.Append("createTimeMs").Append(":").Append(createTimeMs.ToString()).Append(", ");
	builder.Append("payTimeMs").Append(":").Append(payTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

