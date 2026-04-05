using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.LevyObj
{

/// <summary>
/// 征收粮食离线收益信息
/// </summary>
public class Levy_FoodOfflineInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 离线时长
/// </summary>
private long offlineMs;
/// <summary>
/// 征收数量
/// </summary>
private long count;


public Levy_FoodOfflineInfo() {
	offlineMs = (long)0;
	count = (long)0;
}

public Levy_FoodOfflineInfo(
	long _offlineMs
	, long _count
) {	offlineMs = _offlineMs;
	count = _count;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 离线时长
/// </summary>
public long getOfflineMs() { return offlineMs; }
/// <summary>
/// 离线时长
/// </summary>
public void setOfflineMs(long _offlineMs) { offlineMs = _offlineMs; }
/// <summary>
/// 征收数量
/// </summary>
public long getCount() { return count; }
/// <summary>
/// 征收数量
/// </summary>
public void setCount(long _count) { count = _count; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	offlineMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(offlineMs);
	_buf.putLong(count);
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
	builder.Append("offlineMs").Append(":").Append(offlineMs.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

