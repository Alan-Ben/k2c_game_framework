using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_006_QuitUSRes : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 客户端用于识别的序列号
/// </summary>
private long clientSerialize;
/// <summary>
/// 错误信息，根据枚举处理
/// </summary>
private int error;


public NPGS2GC_001_006_QuitUSRes() {
	clientSerialize = (long)0;
	error = 0;
}

public NPGS2GC_001_006_QuitUSRes(
	long _clientSerialize
	, int _error
) {	clientSerialize = _clientSerialize;
	error = _error;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 客户端用于识别的序列号
/// </summary>
public long getClientSerialize() { return clientSerialize; }
/// <summary>
/// 客户端用于识别的序列号
/// </summary>
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
/// <summary>
/// 错误信息，根据枚举处理
/// </summary>
public int getError() { return error; }
/// <summary>
/// 错误信息，根据枚举处理
/// </summary>
public void setError(int _error) { error = _error; }


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
	clientSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	error = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(clientSerialize);
	_buf.putInt(error);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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
	builder.Append("clientSerialize").Append(":").Append(clientSerialize.ToString()).Append(", ");
	builder.Append("error").Append(":").Append(error.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

