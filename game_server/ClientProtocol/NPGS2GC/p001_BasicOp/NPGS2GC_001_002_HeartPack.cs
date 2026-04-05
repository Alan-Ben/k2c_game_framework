using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_002_HeartPack : ALBasicProtocolPack._IALProtocolStructure {
private long clientTimeTag;
private long serverTimeTag;
private long clientHeartSerialize;


public NPGS2GC_001_002_HeartPack() {
	clientTimeTag = (long)0;
	serverTimeTag = (long)0;
	clientHeartSerialize = (long)0;
}

public NPGS2GC_001_002_HeartPack(
	long _clientTimeTag
	, long _serverTimeTag
	, long _clientHeartSerialize
) {	clientTimeTag = _clientTimeTag;
	serverTimeTag = _serverTimeTag;
	clientHeartSerialize = _clientHeartSerialize;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)2; }

public long getClientTimeTag() { return clientTimeTag; }
public void setClientTimeTag(long _clientTimeTag) { clientTimeTag = _clientTimeTag; }
public long getServerTimeTag() { return serverTimeTag; }
public void setServerTimeTag(long _serverTimeTag) { serverTimeTag = _serverTimeTag; }
public long getClientHeartSerialize() { return clientHeartSerialize; }
public void setClientHeartSerialize(long _clientHeartSerialize) { clientHeartSerialize = _clientHeartSerialize; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clientTimeTag = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serverTimeTag = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clientHeartSerialize = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(clientTimeTag);
	_buf.putLong(serverTimeTag);
	_buf.putLong(clientHeartSerialize);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
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
	builder.Append("clientTimeTag").Append(":").Append(clientTimeTag.ToString()).Append(", ");
	builder.Append("serverTimeTag").Append(":").Append(serverTimeTag.ToString()).Append(", ");
	builder.Append("clientHeartSerialize").Append(":").Append(clientHeartSerialize.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

