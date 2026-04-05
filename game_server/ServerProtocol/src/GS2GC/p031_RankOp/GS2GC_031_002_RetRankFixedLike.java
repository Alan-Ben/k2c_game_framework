package GS2GC.p031_RankOp;

import java.nio.ByteBuffer;
public class GS2GC_031_002_RetRankFixedLike implements ALBasicProtocolPack._IALProtocolStructure {
/** 点赞结果 */
private Common.RankObj.RankFixed_LikeResult likeResult;


public GS2GC_031_002_RetRankFixedLike() {
	likeResult = new Common.RankObj.RankFixed_LikeResult();
}

public GS2GC_031_002_RetRankFixedLike(
	 Common.RankObj.RankFixed_LikeResult _likeResult
) {	likeResult = _likeResult;
}

public final byte getMainOrder() { return (byte)31; }

public final byte getSubOrder() { return (byte)2; }

/** 点赞结果 */
public Common.RankObj.RankFixed_LikeResult getLikeResult() { return likeResult; }
/** 点赞结果 */
public void setLikeResult(Common.RankObj.RankFixed_LikeResult _likeResult) { likeResult = _likeResult; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + likeResult.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + likeResult.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _likeResultCustLen = _buf.getInt();
	int _likeResultCurPos = _buf.position();
	likeResult.ReadUnzipBuf(_buf, _likeResultCurPos + _likeResultCustLen);
	_buf.position(_likeResultCurPos + _likeResultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(likeResult.GetBufSize());
	likeResult.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)31);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)31);
	_recBuf.put((byte)2);
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

