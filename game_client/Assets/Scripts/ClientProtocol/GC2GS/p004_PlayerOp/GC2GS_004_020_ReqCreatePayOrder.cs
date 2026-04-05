using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 请求创建支付订单
/// </summary>
public class GC2GS_004_020_ReqCreatePayOrder : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包id
/// </summary>
private long giftPackId;
/// <summary>
/// 额外数据
/// </summary>
private byte[] extraData;


public GC2GS_004_020_ReqCreatePayOrder() {
	giftPackId = (long)0;
	extraData = null;
}

public GC2GS_004_020_ReqCreatePayOrder(
	long _giftPackId
	, byte[] _extraData
) {	giftPackId = _giftPackId;
	extraData = _extraData;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)20; }

/// <summary>
/// 礼包id
/// </summary>
public long getGiftPackId() { return giftPackId; }
/// <summary>
/// 礼包id
/// </summary>
public void setGiftPackId(long _giftPackId) { giftPackId = _giftPackId; }
/// <summary>
/// 额外数据
/// </summary>
public byte[] getExtraData() { return extraData; }

/// <summary>
/// 额外数据
/// </summary>
public void setExtraData(byte[] _extraData) { extraData = _extraData; }



public int GetBufSize() {
	int _size = 8;
	_size += 4 + (extraData == null ? 0 : extraData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (extraData == null ? 0 : extraData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	giftPackId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extraData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(giftPackId);
	_buf.putByteBuffer(extraData);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)20);
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
	builder.Append("giftPackId").Append(":").Append(giftPackId.ToString()).Append(", ");
	builder.Append("extraData").Append(":").Append(extraData == null ? "null" : extraData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

