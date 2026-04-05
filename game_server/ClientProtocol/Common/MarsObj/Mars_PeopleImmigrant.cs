using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星居民-移民数据
/// </summary>
public class Mars_PeopleImmigrant : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 发起时间（毫秒）
/// </summary>
private long startMs;
/// <summary>
/// 结束时间（毫秒）
/// </summary>
private long endMs;


public Mars_PeopleImmigrant() {
	startMs = (long)0;
	endMs = (long)0;
}

public Mars_PeopleImmigrant(
	long _startMs
	, long _endMs
) {	startMs = _startMs;
	endMs = _endMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 发起时间（毫秒）
/// </summary>
public long getStartMs() { return startMs; }
/// <summary>
/// 发起时间（毫秒）
/// </summary>
public void setStartMs(long _startMs) { startMs = _startMs; }
/// <summary>
/// 结束时间（毫秒）
/// </summary>
public long getEndMs() { return endMs; }
/// <summary>
/// 结束时间（毫秒）
/// </summary>
public void setEndMs(long _endMs) { endMs = _endMs; }


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
	startMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(startMs);
	_buf.putLong(endMs);
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
	builder.Append("startMs").Append(":").Append(startMs.ToString()).Append(", ");
	builder.Append("endMs").Append(":").Append(endMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

