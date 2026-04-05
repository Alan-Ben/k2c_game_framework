package NPGS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGS2GC_001_002_HeartPack implements ALBasicProtocolPack._IALProtocolStructure {
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

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

public long getClientTimeTag() { return clientTimeTag; }
public void setClientTimeTag(long _clientTimeTag) { clientTimeTag = _clientTimeTag; }
public long getServerTimeTag() { return serverTimeTag; }
public void setServerTimeTag(long _serverTimeTag) { serverTimeTag = _serverTimeTag; }
public long getClientHeartSerialize() { return clientHeartSerialize; }
public void setClientHeartSerialize(long _clientHeartSerialize) { clientHeartSerialize = _clientHeartSerialize; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientTimeTag = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTimeTag = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientHeartSerialize = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(clientTimeTag);
	_buf.putLong(serverTimeTag);
	_buf.putLong(clientHeartSerialize);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

