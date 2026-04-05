package WCGCS2US_RB.p003_CommOp;

import java.nio.ByteBuffer;
public class NP2US_RB_003_019_RetPlayerDetailLike implements ALBasicProtocolPack._IALProtocolStructure {
private long likeCount;


public NP2US_RB_003_019_RetPlayerDetailLike() {
	likeCount = (long)0;
}

public NP2US_RB_003_019_RetPlayerDetailLike(
	 long _likeCount
) {	likeCount = _likeCount;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)19; }

public long getLikeCount() { return likeCount; }
public void setLikeCount(long _likeCount) { likeCount = _likeCount; }


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
	if(_buf.remaining() > 0) likeCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(likeCount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)19);
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

