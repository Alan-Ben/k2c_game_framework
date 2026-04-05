using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星居民-数量
/// </summary>
public class Mars_PeopleNum : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 休闲居民数量
/// </summary>
private long idle;
/// <summary>
/// 生病居民数量
/// </summary>
private long sick;


public Mars_PeopleNum() {
	idle = (long)0;
	sick = (long)0;
}

public Mars_PeopleNum(
	long _idle
	, long _sick
) {	idle = _idle;
	sick = _sick;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 休闲居民数量
/// </summary>
public long getIdle() { return idle; }
/// <summary>
/// 休闲居民数量
/// </summary>
public void setIdle(long _idle) { idle = _idle; }
/// <summary>
/// 生病居民数量
/// </summary>
public long getSick() { return sick; }
/// <summary>
/// 生病居民数量
/// </summary>
public void setSick(long _sick) { sick = _sick; }


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
	idle = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sick = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(idle);
	_buf.putLong(sick);
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
	builder.Append("idle").Append(":").Append(idle.ToString()).Append(", ");
	builder.Append("sick").Append(":").Append(sick.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

