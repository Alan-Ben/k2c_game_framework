using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_MsgOp
{

public class GS2GC_002_006_ReqConfirmPrivateMsg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 错误码
/// </summary>
private int errCode;
/// <summary>
/// 回调序列号
/// </summary>
private long callBackSerial;


public GS2GC_002_006_ReqConfirmPrivateMsg() {
	errCode = 0;
	callBackSerial = (long)0;
}

public GS2GC_002_006_ReqConfirmPrivateMsg(
	int _errCode
	, long _callBackSerial
) {	errCode = _errCode;
	callBackSerial = _callBackSerial;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 错误码
/// </summary>
public int getErrCode() { return errCode; }
/// <summary>
/// 错误码
/// </summary>
public void setErrCode(int _errCode) { errCode = _errCode; }
/// <summary>
/// 回调序列号
/// </summary>
public long getCallBackSerial() { return callBackSerial; }
/// <summary>
/// 回调序列号
/// </summary>
public void setCallBackSerial(long _callBackSerial) { callBackSerial = _callBackSerial; }


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
	errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	callBackSerial = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(errCode);
	_buf.putLong(callBackSerial);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)6);
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
	builder.Append("callBackSerial").Append(":").Append(callBackSerial.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

