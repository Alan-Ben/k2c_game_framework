package NPGS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGS2GC_001_008_QuitQueue implements ALBasicProtocolPack._IALProtocolStructure {
/** 客户端用于识别的序列号 */
private long clientSerialize;
private int errCode;


public NPGS2GC_001_008_QuitQueue() {
	clientSerialize = (long)0;
	errCode = 0;
}

public NPGS2GC_001_008_QuitQueue(
	 long _clientSerialize
	, int _errCode
) {	clientSerialize = _clientSerialize;
	errCode = _errCode;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)8; }

/** 客户端用于识别的序列号 */
public long getClientSerialize() { return clientSerialize; }
/** 客户端用于识别的序列号 */
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(clientSerialize);
	_buf.putInt(errCode);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)8);
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

