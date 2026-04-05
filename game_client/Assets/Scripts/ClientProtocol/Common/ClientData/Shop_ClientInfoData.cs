using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

public class Shop_ClientInfoData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 商店id
/// </summary>
private long shopId;
/// <summary>
/// 最后一次显示的时间戳
/// </summary>
private long lastShowTimeS;
/// <summary>
/// 下一次刷新时间戳
/// </summary>
private long nextRefreshTimeMs;


public Shop_ClientInfoData() {
	shopId = (long)0;
	lastShowTimeS = (long)0;
	nextRefreshTimeMs = (long)0;
}

public Shop_ClientInfoData(
	long _shopId
	, long _lastShowTimeS
	, long _nextRefreshTimeMs
) {	shopId = _shopId;
	lastShowTimeS = _lastShowTimeS;
	nextRefreshTimeMs = _nextRefreshTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 商店id
/// </summary>
public long getShopId() { return shopId; }
/// <summary>
/// 商店id
/// </summary>
public void setShopId(long _shopId) { shopId = _shopId; }
/// <summary>
/// 最后一次显示的时间戳
/// </summary>
public long getLastShowTimeS() { return lastShowTimeS; }
/// <summary>
/// 最后一次显示的时间戳
/// </summary>
public void setLastShowTimeS(long _lastShowTimeS) { lastShowTimeS = _lastShowTimeS; }
/// <summary>
/// 下一次刷新时间戳
/// </summary>
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/// <summary>
/// 下一次刷新时间戳
/// </summary>
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shopId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastShowTimeS = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextRefreshTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(shopId);
	_buf.putLong(lastShowTimeS);
	_buf.putLong(nextRefreshTimeMs);
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
	builder.Append("shopId").Append(":").Append(shopId.ToString()).Append(", ");
	builder.Append("lastShowTimeS").Append(":").Append(lastShowTimeS.ToString()).Append(", ");
	builder.Append("nextRefreshTimeMs").Append(":").Append(nextRefreshTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

