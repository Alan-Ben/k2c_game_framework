using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarketObj
{

/// <summary>
/// 集市数据
/// </summary>
public class Market_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 集市ID
/// </summary>
private long marketId;
/// <summary>
/// 集市等级
/// </summary>
private long marketLvl;
/// <summary>
/// 当前操作次数
/// </summary>
private int count;
/// <summary>
/// 上次经营时间（毫秒）
/// </summary>
private long lastOperateMs;


public Market_Info() {
	marketId = (long)0;
	marketLvl = (long)0;
	count = 0;
	lastOperateMs = (long)0;
}

public Market_Info(
	long _marketId
	, long _marketLvl
	, int _count
	, long _lastOperateMs
) {	marketId = _marketId;
	marketLvl = _marketLvl;
	count = _count;
	lastOperateMs = _lastOperateMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 集市ID
/// </summary>
public long getMarketId() { return marketId; }
/// <summary>
/// 集市ID
/// </summary>
public void setMarketId(long _marketId) { marketId = _marketId; }
/// <summary>
/// 集市等级
/// </summary>
public long getMarketLvl() { return marketLvl; }
/// <summary>
/// 集市等级
/// </summary>
public void setMarketLvl(long _marketLvl) { marketLvl = _marketLvl; }
/// <summary>
/// 当前操作次数
/// </summary>
public int getCount() { return count; }
/// <summary>
/// 当前操作次数
/// </summary>
public void setCount(int _count) { count = _count; }
/// <summary>
/// 上次经营时间（毫秒）
/// </summary>
public long getLastOperateMs() { return lastOperateMs; }
/// <summary>
/// 上次经营时间（毫秒）
/// </summary>
public void setLastOperateMs(long _lastOperateMs) { lastOperateMs = _lastOperateMs; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	marketId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	marketLvl = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastOperateMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(marketId);
	_buf.putLong(marketLvl);
	_buf.putInt(count);
	_buf.putLong(lastOperateMs);
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
	builder.Append("marketId").Append(":").Append(marketId.ToString()).Append(", ");
	builder.Append("marketLvl").Append(":").Append(marketLvl.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("lastOperateMs").Append(":").Append(lastOperateMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

