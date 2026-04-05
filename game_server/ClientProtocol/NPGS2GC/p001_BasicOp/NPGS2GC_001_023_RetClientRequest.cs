using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_023_RetClientRequest : ALBasicProtocolPack._IALProtocolStructure {
private int msgSerialize;
private long clientRequestSerialize;
private bool res;
private int errCode;
private byte[] msgBuffer;


public NPGS2GC_001_023_RetClientRequest() {
	msgSerialize = 0;
	clientRequestSerialize = (long)0;
	res = false;
	errCode = 0;
	msgBuffer = null;
}

public NPGS2GC_001_023_RetClientRequest(
	int _msgSerialize
	, long _clientRequestSerialize
	, bool _res
	, int _errCode
	, byte[] _msgBuffer
) {	msgSerialize = _msgSerialize;
	clientRequestSerialize = _clientRequestSerialize;
	res = _res;
	errCode = _errCode;
	msgBuffer = _msgBuffer;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)23; }

public int getMsgSerialize() { return msgSerialize; }
public void setMsgSerialize(int _msgSerialize) { msgSerialize = _msgSerialize; }
public long getClientRequestSerialize() { return clientRequestSerialize; }
public void setClientRequestSerialize(long _clientRequestSerialize) { clientRequestSerialize = _clientRequestSerialize; }
public bool getRes() { return res; }
public void setRes(bool _res) { res = _res; }
public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }
public byte[] getMsgBuffer() { return msgBuffer; }

public void setMsgBuffer(byte[] _msgBuffer) { msgBuffer = _msgBuffer; }



public int GetBufSize() {
	int _size = 17;
	_size += 4 + (msgBuffer == null ? 0 : msgBuffer.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;
	_size += 4 + (msgBuffer == null ? 0 : msgBuffer.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msgSerialize = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clientRequestSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	res = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msgBuffer = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(msgSerialize);
	_buf.putLong(clientRequestSerialize);
	_buf.put(res?(byte)1:(byte)0);
	_buf.putInt(errCode);
	_buf.putByteBuffer(msgBuffer);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)23);
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
	builder.Append("msgSerialize").Append(":").Append(msgSerialize.ToString()).Append(", ");
	builder.Append("clientRequestSerialize").Append(":").Append(clientRequestSerialize.ToString()).Append(", ");
	builder.Append("res").Append(":").Append(res.ToString()).Append(", ");
	builder.Append("errCode").Append(":").Append(errCode.ToString()).Append(", ");
	builder.Append("msgBuffer").Append(":").Append(msgBuffer == null ? "null" : msgBuffer.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

