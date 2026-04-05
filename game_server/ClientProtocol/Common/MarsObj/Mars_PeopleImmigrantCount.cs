using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星居民-移民次数
/// </summary>
public class Mars_PeopleImmigrantCount : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 当日标记 YYYYMMDD
/// </summary>
private int dayTag;
/// <summary>
/// 已移民次数
/// </summary>
private int usedCount;


public Mars_PeopleImmigrantCount() {
	dayTag = 0;
	usedCount = 0;
}

public Mars_PeopleImmigrantCount(
	int _dayTag
	, int _usedCount
) {	dayTag = _dayTag;
	usedCount = _usedCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 当日标记 YYYYMMDD
/// </summary>
public int getDayTag() { return dayTag; }
/// <summary>
/// 当日标记 YYYYMMDD
/// </summary>
public void setDayTag(int _dayTag) { dayTag = _dayTag; }
/// <summary>
/// 已移民次数
/// </summary>
public int getUsedCount() { return usedCount; }
/// <summary>
/// 已移民次数
/// </summary>
public void setUsedCount(int _usedCount) { usedCount = _usedCount; }


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
	dayTag = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	usedCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(dayTag);
	_buf.putInt(usedCount);
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
	builder.Append("dayTag").Append(":").Append(dayTag.ToString()).Append(", ");
	builder.Append("usedCount").Append(":").Append(usedCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

