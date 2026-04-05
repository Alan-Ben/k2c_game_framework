package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星居民-数量
 **/
public class Mars_PeopleNum implements ALBasicProtocolPack._IALProtocolStructure {
/** 休闲居民数量 */
private long idle;
/** 生病居民数量 */
private long sick;


public Mars_PeopleNum() {
	idle = (long)0;
	sick = (long)0;
}

public Mars_PeopleNum(
	 long _idle
	, long _sick
) {	idle = _idle;
	sick = _sick;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 休闲居民数量 */
public long getIdle() { return idle; }
/** 休闲居民数量 */
public void setIdle(long _idle) { idle = _idle; }
/** 生病居民数量 */
public long getSick() { return sick; }
/** 生病居民数量 */
public void setSick(long _sick) { sick = _sick; }


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
	if(_buf.remaining() > 0) idle = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sick = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(idle);
	_buf.putLong(sick);
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

