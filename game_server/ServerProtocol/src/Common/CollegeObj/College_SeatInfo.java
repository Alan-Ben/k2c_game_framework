package Common.CollegeObj;

import java.nio.ByteBuffer;
/*********
 * 大学座位信息
 **/
public class College_SeatInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 索引 */
private int index;
/** 大臣id */
private long heroId;
/** 剩余时间 */
private long remainTimeMs;


public College_SeatInfo() {
	index = 0;
	heroId = (long)0;
	remainTimeMs = (long)0;
}

public College_SeatInfo(
	 int _index
	, long _heroId
	, long _remainTimeMs
) {	index = _index;
	heroId = _heroId;
	remainTimeMs = _remainTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 索引 */
public int getIndex() { return index; }
/** 索引 */
public void setIndex(int _index) { index = _index; }
/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 剩余时间 */
public long getRemainTimeMs() { return remainTimeMs; }
/** 剩余时间 */
public void setRemainTimeMs(long _remainTimeMs) { remainTimeMs = _remainTimeMs; }


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
	if(_buf.remaining() > 0) index = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) remainTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(index);
	_buf.putLong(heroId);
	_buf.putLong(remainTimeMs);
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

