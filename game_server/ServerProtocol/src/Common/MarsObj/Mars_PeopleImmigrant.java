package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星居民-移民数据
 **/
public class Mars_PeopleImmigrant implements ALBasicProtocolPack._IALProtocolStructure {
/** 发起时间（毫秒） */
private long startMs;
/** 结束时间（毫秒） */
private long endMs;


public Mars_PeopleImmigrant() {
	startMs = (long)0;
	endMs = (long)0;
}

public Mars_PeopleImmigrant(
	 long _startMs
	, long _endMs
) {	startMs = _startMs;
	endMs = _endMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 发起时间（毫秒） */
public long getStartMs() { return startMs; }
/** 发起时间（毫秒） */
public void setStartMs(long _startMs) { startMs = _startMs; }
/** 结束时间（毫秒） */
public long getEndMs() { return endMs; }
/** 结束时间（毫秒） */
public void setEndMs(long _endMs) { endMs = _endMs; }


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
	if(_buf.remaining() > 0) startMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(startMs);
	_buf.putLong(endMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

