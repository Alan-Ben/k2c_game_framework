package WCGCS2US_RB.p003_CommOp;

import java.nio.ByteBuffer;
public class NP2US_RB_003_006_RetRankFixedLike implements ALBasicProtocolPack._IALProtocolStructure {
/** 点赞积分 */
private long likeScore;


public NP2US_RB_003_006_RetRankFixedLike() {
	likeScore = (long)0;
}

public NP2US_RB_003_006_RetRankFixedLike(
	 long _likeScore
) {	likeScore = _likeScore;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)6; }

/** 点赞积分 */
public long getLikeScore() { return likeScore; }
/** 点赞积分 */
public void setLikeScore(long _likeScore) { likeScore = _likeScore; }


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
	if(_buf.remaining() > 0) likeScore = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(likeScore);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
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

