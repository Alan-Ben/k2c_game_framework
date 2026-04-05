package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
public class GC2GS_007_001_ReqDealRemoteEffect implements ALBasicProtocolPack._IALProtocolStructure {
private long clientSerialize;
private long refId;


public GC2GS_007_001_ReqDealRemoteEffect() {
	clientSerialize = (long)0;
	refId = (long)0;
}

public GC2GS_007_001_ReqDealRemoteEffect(
	 long _clientSerialize
	, long _refId
) {	clientSerialize = _clientSerialize;
	refId = _refId;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)1; }

public long getClientSerialize() { return clientSerialize; }
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(clientSerialize);
	_buf.putLong(refId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)1);
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

