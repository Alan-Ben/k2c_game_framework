using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p034_InnOp
{

/// <summary>
/// 旅店接待客人
/// </summary>
public class GC2GS_034_001_ReqInnReceiveGuest : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否一键
/// </summary>
private bool isAKey;


public GC2GS_034_001_ReqInnReceiveGuest() {
	isAKey = false;
}

public GC2GS_034_001_ReqInnReceiveGuest(
	bool _isAKey
) {	isAKey = _isAKey;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 是否一键
/// </summary>
public bool getIsAKey() { return isAKey; }
/// <summary>
/// 是否一键
/// </summary>
public void setIsAKey(bool _isAKey) { isAKey = _isAKey; }


public int GetBufSize() {
	int _size = 1;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAKey = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isAKey?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
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
	builder.Append("isAKey").Append(":").Append(isAKey.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

