using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_005_SetIconRes : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 错误编码，0表示成功
/// </summary>
private int errCode;


public GS2GC_004_005_SetIconRes() {
	errCode = 0;
}

public GS2GC_004_005_SetIconRes(
	int _errCode
) {	errCode = _errCode;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 错误编码，0表示成功
/// </summary>
public int getErrCode() { return errCode; }
/// <summary>
/// 错误编码，0表示成功
/// </summary>
public void setErrCode(int _errCode) { errCode = _errCode; }


public int GetBufSize() {
	int _size = 4;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	errCode = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(errCode);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)5);
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
	builder.Append("errCode").Append(":").Append(errCode.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

