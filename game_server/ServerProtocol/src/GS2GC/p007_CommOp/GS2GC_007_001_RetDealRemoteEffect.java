package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_001_RetDealRemoteEffect implements ALBasicProtocolPack._IALProtocolStructure {
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

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)1; }

public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }
public long getClientSerialize() { return clientSerialize; }
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(errCode);
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

