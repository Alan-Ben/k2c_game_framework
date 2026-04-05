package GC2GS.p016_ChapterOp;

import java.nio.ByteBuffer;
/*********
 * 关卡前进
 **/
public class GC2GS_016_001_ReqChapterForward implements ALBasicProtocolPack._IALProtocolStructure {
private long chapterId;
private int point;
private boolean isAKey;


public GC2GS_016_001_ReqChapterForward() {
	chapterId = (long)0;
	point = 0;
	isAKey = false;
}

public GC2GS_016_001_ReqChapterForward(
	 long _chapterId
	, int _point
	, boolean _isAKey
) {	chapterId = _chapterId;
	point = _point;
	isAKey = _isAKey;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)1; }

public long getChapterId() { return chapterId; }
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
public int getPoint() { return point; }
public void setPoint(int _point) { point = _point; }
public boolean getIsAKey() { return isAKey; }
public void setIsAKey(boolean _isAKey) { isAKey = _isAKey; }


public final int GetBufSize() {
	int _size = 13;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) point = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAKey = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(chapterId);
	_buf.putInt(point);
	_buf.put(isAKey?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
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

