using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_001_RetDealRemoteEffect : ALBasicProtocolPack._IALProtocolStructure {
private int errCode;
private long clientSerialize;
private long refId;


public GS2GC_007_001_RetDealRemoteEffect() {
	errCode = 0;
	clientSerialize = (long)0;
	refId = (long)0;
}

public GS2GC_007_001_RetDealRemoteEffect(
	int _errCode
	, long _clientSerialize
	, long _refId
) {	errCode = _errCode;
	clientSerialize = _clientSerialize;
	refId = _refId;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)1; }

public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }
public long getClientSerialize() { return clientSerialize; }
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clientSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(errCode);
	_buf.putLong(clientSerialize);
	_buf.putLong(refId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
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
	builder.Append("errCode").Append(":").Append(errCode.ToString()).Append(", ");
	builder.Append("clientSerialize").Append(":").Append(clientSerialize.ToString()).Append(", ");
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

