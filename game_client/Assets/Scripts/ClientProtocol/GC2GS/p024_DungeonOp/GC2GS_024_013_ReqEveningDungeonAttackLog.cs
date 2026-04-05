using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p024_DungeonOp
{

/// <summary>
/// 晚间副本攻击日志
/// </summary>
public class GC2GS_024_013_ReqEveningDungeonAttackLog : ALBasicProtocolPack._IALProtocolStructure {
private long serial;
/// <summary>
/// 需要数量
/// </summary>
private int needNum;


public GC2GS_024_013_ReqEveningDungeonAttackLog() {
	serial = (long)0;
	needNum = 0;
}

public GC2GS_024_013_ReqEveningDungeonAttackLog(
	long _serial
	, int _needNum
) {	serial = _serial;
	needNum = _needNum;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)13; }

public long getSerial() { return serial; }
public void setSerial(long _serial) { serial = _serial; }
/// <summary>
/// 需要数量
/// </summary>
public int getNeedNum() { return needNum; }
/// <summary>
/// 需要数量
/// </summary>
public void setNeedNum(int _needNum) { needNum = _needNum; }


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
	serial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	needNum = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(serial);
	_buf.putInt(needNum);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)13);
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
	builder.Append("serial").Append(":").Append(serial.ToString()).Append(", ");
	builder.Append("needNum").Append(":").Append(needNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

