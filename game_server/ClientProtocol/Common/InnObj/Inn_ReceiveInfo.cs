using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.InnObj
{

/// <summary>
/// 旅店_接待队列信息
/// </summary>
public class Inn_ReceiveInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 接待开始时间 毫秒
/// </summary>
private long startTimeMs;
/// <summary>
/// 需要接待数量
/// </summary>
private int needReceiveNum;


public Inn_ReceiveInfo() {
	startTimeMs = (long)0;
	needReceiveNum = 0;
}

public Inn_ReceiveInfo(
	long _startTimeMs
	, int _needReceiveNum
) {	startTimeMs = _startTimeMs;
	needReceiveNum = _needReceiveNum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 接待开始时间 毫秒
/// </summary>
public long getStartTimeMs() { return startTimeMs; }
/// <summary>
/// 接待开始时间 毫秒
/// </summary>
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/// <summary>
/// 需要接待数量
/// </summary>
public int getNeedReceiveNum() { return needReceiveNum; }
/// <summary>
/// 需要接待数量
/// </summary>
public void setNeedReceiveNum(int _needReceiveNum) { needReceiveNum = _needReceiveNum; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	needReceiveNum = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(startTimeMs);
	_buf.putInt(needReceiveNum);
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
	builder.Append("startTimeMs").Append(":").Append(startTimeMs.ToString()).Append(", ");
	builder.Append("needReceiveNum").Append(":").Append(needReceiveNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

