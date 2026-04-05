package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星居民-移民次数
 **/
public class Mars_PeopleImmigrantCount implements ALBasicProtocolPack._IALProtocolStructure {
/** 当日标记 YYYYMMDD */
private int dayTag;
/** 已移民次数 */
private int usedCount;


public Mars_PeopleImmigrantCount() {
	dayTag = 0;
	usedCount = 0;
}

public Mars_PeopleImmigrantCount(
	 int _dayTag
	, int _usedCount
) {	dayTag = _dayTag;
	usedCount = _usedCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 当日标记 YYYYMMDD */
public int getDayTag() { return dayTag; }
/** 当日标记 YYYYMMDD */
public void setDayTag(int _dayTag) { dayTag = _dayTag; }
/** 已移民次数 */
public int getUsedCount() { return usedCount; }
/** 已移民次数 */
public void setUsedCount(int _usedCount) { usedCount = _usedCount; }


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
	if(_buf.remaining() > 0) dayTag = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usedCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(dayTag);
	_buf.putInt(usedCount);
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

