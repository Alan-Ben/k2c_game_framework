using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CommObj
{

/// <summary>
/// 错误码以及信息
/// </summary>
public class RESULT : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 错误号
/// </summary>
private int code;
/// <summary>
/// 错误消息
/// </summary>
private string msg;


public RESULT() {
	code = 0;
	msg = "";
}

public RESULT(
	int _code
	, string _msg
) {	code = _code;
	msg = _msg;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 错误号
/// </summary>
public int getCode() { return code; }
/// <summary>
/// 错误号
/// </summary>
public void setCode(int _code) { code = _code; }
/// <summary>
/// 错误消息
/// </summary>
public string getMsg() { return msg; }
/// <summary>
/// 错误消息
/// </summary>
public void setMsg(string _msg) { msg = _msg; }


public int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(msg);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(msg);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	code = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msg = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(code);
	_buf.putString(msg);
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
	builder.Append("code").Append(":").Append(code.ToString()).Append(", ");
	builder.Append("msg").Append(":").Append(msg.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

