package Common.ChapterObj;

import java.nio.ByteBuffer;
/*********
 * 关卡位置信息
 **/
public class Chapter_PosInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 关卡id */
private long chapterId;
/** 所在点位 */
private int point;


public Chapter_PosInfo() {
	chapterId = (long)0;
	point = 0;
}

public Chapter_PosInfo(
	 long _chapterId
	, int _point
) {	chapterId = _chapterId;
	point = _point;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 关卡id */
public long getChapterId() { return chapterId; }
/** 关卡id */
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
/** 所在点位 */
public int getPoint() { return point; }
/** 所在点位 */
public void setPoint(int _point) { point = _point; }


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
	if(_buf.remaining() > 0) chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) point = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(chapterId);
	_buf.putInt(point);
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

