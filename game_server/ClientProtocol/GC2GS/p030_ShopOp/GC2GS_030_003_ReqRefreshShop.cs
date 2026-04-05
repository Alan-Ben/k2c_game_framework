using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p030_ShopOp
{

public class GC2GS_030_003_ReqRefreshShop : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 商店配置id
/// </summary>
private long shopRefId;


public GC2GS_030_003_ReqRefreshShop() {
	shopRefId = (long)0;
}

public GC2GS_030_003_ReqRefreshShop(
	long _shopRefId
) {	shopRefId = _shopRefId;
}

public byte getMainOrder() { return (byte)30; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 商店配置id
/// </summary>
public long getShopRefId() { return shopRefId; }
/// <summary>
/// 商店配置id
/// </summary>
public void setShopRefId(long _shopRefId) { shopRefId = _shopRefId; }


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
	shopRefId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(shopRefId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)3);
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
	builder.Append("shopRefId").Append(":").Append(shopRefId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

