using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 通知客户端支付完成
/// </summary>
public class GC2GS_004_021_ReqClientPayDone : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 订单ID
/// </summary>
private string orderId;


public GC2GS_004_021_ReqClientPayDone() {
	orderId = "";
}

public GC2GS_004_021_ReqClientPayDone(
	string _orderId
) {	orderId = _orderId;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)21; }

/// <summary>
/// 订单ID
/// </summary>
public string getOrderId() { return orderId; }
/// <summary>
/// 订单ID
/// </summary>
public void setOrderId(string _orderId) { orderId = _orderId; }


public int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	orderId = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(orderId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)21);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

