package NPLS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPLS2GC_001_005_RetQueueHeadIdx implements ALBasicProtocolPack._IALProtocolStructure {
private long headQueueIdx;


public NPLS2GC_001_005_RetQueueHeadIdx() {
	headQueueIdx = (long)0;
}

public NPLS2GC_001_005_RetQueueHeadIdx(
	 long _headQueueIdx
) {	headQueueIdx = _headQueueIdx;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)5; }

public long getHeadQueueIdx() { return headQueueIdx; }
public void setHeadQueueIdx(long _headQueueIdx) { headQueueIdx = _headQueueIdx; }


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
	if(_buf.remaining() > 0) headQueueIdx = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(headQueueIdx);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)5);
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

