package NP2US_RB.p002_GSOp;

import java.nio.ByteBuffer;
public class NP2US_RB_002_006_RetQueueInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 当前队列索引 */
private long curQueueIndex;


public NP2US_RB_002_006_RetQueueInfo() {
	curQueueIndex = (long)0;
}

public NP2US_RB_002_006_RetQueueInfo(
	 long _curQueueIndex
) {	curQueueIndex = _curQueueIndex;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)6; }

/** 当前队列索引 */
public long getCurQueueIndex() { return curQueueIndex; }
/** 当前队列索引 */
public void setCurQueueIndex(long _curQueueIndex) { curQueueIndex = _curQueueIndex; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curQueueIndex = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(curQueueIndex);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)6);
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

