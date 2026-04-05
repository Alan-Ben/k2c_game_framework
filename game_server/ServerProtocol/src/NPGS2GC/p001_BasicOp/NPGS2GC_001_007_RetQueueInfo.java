package NPGS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGS2GC_001_007_RetQueueInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 客户端用于识别的序列号 */
private long clientSerialize;
/** 当前客户端进入的索引信息 */
private long curEnterIndex;


public NPGS2GC_001_007_RetQueueInfo() {
	clientSerialize = (long)0;
	curEnterIndex = (long)0;
}

public NPGS2GC_001_007_RetQueueInfo(
	 long _clientSerialize
	, long _curEnterIndex
) {	clientSerialize = _clientSerialize;
	curEnterIndex = _curEnterIndex;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)7; }

/** 客户端用于识别的序列号 */
public long getClientSerialize() { return clientSerialize; }
/** 客户端用于识别的序列号 */
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
/** 当前客户端进入的索引信息 */
public long getCurEnterIndex() { return curEnterIndex; }
/** 当前客户端进入的索引信息 */
public void setCurEnterIndex(long _curEnterIndex) { curEnterIndex = _curEnterIndex; }


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
	if(_buf.remaining() > 0) curEnterIndex = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(clientSerialize);
	_buf.putLong(curEnterIndex);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)7);
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

