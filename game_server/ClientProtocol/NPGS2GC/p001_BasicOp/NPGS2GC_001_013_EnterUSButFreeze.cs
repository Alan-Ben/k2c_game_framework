using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_013_EnterUSButFreeze : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 客户端用于识别的序列号
/// </summary>
private long clientSerialize;
/// <summary>
/// 冻结时间
/// </summary>
private long freezeTimeMs;


public NPGS2GC_001_013_EnterUSButFreeze() {
	clientSerialize = (long)0;
	freezeTimeMs = (long)0;
}

public NPGS2GC_001_013_EnterUSButFreeze(
	long _clientSerialize
	, long _freezeTimeMs
) {	clientSerialize = _clientSerialize;
	freezeTimeMs = _freezeTimeMs;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)13; }

/// <summary>
/// 客户端用于识别的序列号
/// </summary>
public long getClientSerialize() { return clientSerialize; }
/// <summary>
/// 客户端用于识别的序列号
/// </summary>
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
/// <summary>
/// 冻结时间
/// </summary>
public long getFreezeTimeMs() { return freezeTimeMs; }
/// <summary>
/// 冻结时间
/// </summary>
public void setFreezeTimeMs(long _freezeTimeMs) { freezeTimeMs = _freezeTimeMs; }


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
	clientSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	freezeTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(clientSerialize);
	_buf.putLong(freezeTimeMs);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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
	builder.Append("clientSerialize").Append(":").Append(clientSerialize.ToString()).Append(", ");
	builder.Append("freezeTimeMs").Append(":").Append(freezeTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

