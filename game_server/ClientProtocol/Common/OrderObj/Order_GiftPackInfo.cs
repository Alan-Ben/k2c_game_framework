using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.OrderObj
{

/// <summary>
/// 礼包订单信息
/// </summary>
public class Order_GiftPackInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包id
/// </summary>
private long giftPackId;
/// <summary>
/// 购买次数
/// </summary>
private long buyTimes;
/// <summary>
/// 下次刷新时间 ms
/// </summary>
private long nextRefreshTimeMs;


public Order_GiftPackInfo() {
	giftPackId = (long)0;
	buyTimes = (long)0;
	nextRefreshTimeMs = (long)0;
}

public Order_GiftPackInfo(
	long _giftPackId
	, long _buyTimes
	, long _nextRefreshTimeMs
) {	giftPackId = _giftPackId;
	buyTimes = _buyTimes;
	nextRefreshTimeMs = _nextRefreshTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 礼包id
/// </summary>
public long getGiftPackId() { return giftPackId; }
/// <summary>
/// 礼包id
/// </summary>
public void setGiftPackId(long _giftPackId) { giftPackId = _giftPackId; }
/// <summary>
/// 购买次数
/// </summary>
public long getBuyTimes() { return buyTimes; }
/// <summary>
/// 购买次数
/// </summary>
public void setBuyTimes(long _buyTimes) { buyTimes = _buyTimes; }
/// <summary>
/// 下次刷新时间 ms
/// </summary>
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/// <summary>
/// 下次刷新时间 ms
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
	giftPackId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buyTimes = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextRefreshTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(giftPackId);
	_buf.putLong(buyTimes);
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
	builder.Append("giftPackId").Append(":").Append(giftPackId.ToString()).Append(", ");
	builder.Append("buyTimes").Append(":").Append(buyTimes.ToString()).Append(", ");
	builder.Append("nextRefreshTimeMs").Append(":").Append(nextRefreshTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

