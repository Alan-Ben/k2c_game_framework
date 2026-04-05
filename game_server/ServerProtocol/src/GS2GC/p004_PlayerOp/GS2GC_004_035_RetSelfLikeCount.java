package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_035_RetSelfLikeCount implements ALBasicProtocolPack._IALProtocolStructure {
private long likeCount;
/** 上次查看的点赞次数 */
private long lastLikeCount;


public GS2GC_004_035_RetSelfLikeCount() {
	likeCount = (long)0;
	lastLikeCount = (long)0;
}

public GS2GC_004_035_RetSelfLikeCount(
	 long _likeCount
	, long _lastLikeCount
) {	likeCount = _likeCount;
	lastLikeCount = _lastLikeCount;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)35; }

public long getLikeCount() { return likeCount; }
public void setLikeCount(long _likeCount) { likeCount = _likeCount; }
/** 上次查看的点赞次数 */
public long getLastLikeCount() { return lastLikeCount; }
/** 上次查看的点赞次数 */
public void setLastLikeCount(long _lastLikeCount) { lastLikeCount = _lastLikeCount; }


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
	if(_buf.remaining() > 0) likeCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastLikeCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(likeCount);
	_buf.putLong(lastLikeCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)35);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)35);
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

