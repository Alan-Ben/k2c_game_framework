using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p019_DinnerOp
{

/// <summary>
/// 开启宴会（普通模式）
/// </summary>
public class GC2GS_019_001_ReqStartDinner : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宴会配置ID
/// </summary>
private long dinnerId;


public GC2GS_019_001_ReqStartDinner() {
	dinnerId = (long)0;
}

public GC2GS_019_001_ReqStartDinner(
	long _dinnerId
) {	dinnerId = _dinnerId;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 宴会配置ID
/// </summary>
public long getDinnerId() { return dinnerId; }
/// <summary>
/// 宴会配置ID
/// </summary>
public void setDinnerId(long _dinnerId) { dinnerId = _dinnerId; }


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
	dinnerId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dinnerId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)1);
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
	builder.Append("dinnerId").Append(":").Append(dinnerId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

