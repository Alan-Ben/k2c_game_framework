using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_066_OnOrderAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 订单信息
/// </summary>
private Common.CommonFuncObj.Order_Info orderInfo;


public GS2GC_004_066_OnOrderAdd() {
	orderInfo = new Common.CommonFuncObj.Order_Info();
}

public GS2GC_004_066_OnOrderAdd(
	Common.CommonFuncObj.Order_Info _orderInfo
) {	orderInfo = _orderInfo;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)66; }

/// <summary>
/// 订单信息
/// </summary>
public Common.CommonFuncObj.Order_Info getOrderInfo() { return orderInfo; }
/// <summary>
/// 订单信息
/// </summary>
public void setOrderInfo(Common.CommonFuncObj.Order_Info _orderInfo) { orderInfo = _orderInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + orderInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + orderInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _orderInfoCustLen = _buf.getInt();
	int _orderInfoCurPos = _buf.getCurPos();
	orderInfo.ReadUnzipBuf(_buf, _orderInfoCurPos + _orderInfoCustLen);
	_buf.setPosition(_orderInfoCurPos + _orderInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(orderInfo.GetBufSize());
	orderInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)66);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)66);
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
	builder.Append("orderInfo").Append(":").Append(orderInfo == null ? "null" : orderInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

