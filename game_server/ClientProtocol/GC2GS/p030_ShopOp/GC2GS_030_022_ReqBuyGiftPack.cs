using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p030_ShopOp
{

/// <summary>
/// 购买礼包
/// </summary>
public class GC2GS_030_022_ReqBuyGiftPack : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包id
/// </summary>
private long packId;


public GC2GS_030_022_ReqBuyGiftPack() {
	packId = (long)0;
}

public GC2GS_030_022_ReqBuyGiftPack(
	long _packId
) {	packId = _packId;
}

public byte getMainOrder() { return (byte)30; }

public byte getSubOrder() { return (byte)22; }

/// <summary>
/// 礼包id
/// </summary>
public long getPackId() { return packId; }
/// <summary>
/// 礼包id
/// </summary>
public void setPackId(long _packId) { packId = _packId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	packId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(packId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)22);
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
	builder.Append("packId").Append(":").Append(packId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

