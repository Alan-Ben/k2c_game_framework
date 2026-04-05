package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 常驻排行榜点赞
 **/
public class NP2US_R_003_006_ReqRankFixedLike implements ALBasicProtocolPack._IALProtocolStructure {
private long rankFixedId;
private long key;
/** 是否跨服 */
private boolean isCross;


public NP2US_R_003_006_ReqRankFixedLike() {
	rankFixedId = (long)0;
	key = (long)0;
	isCross = false;
}

public NP2US_R_003_006_ReqRankFixedLike(
	 long _rankFixedId
	, long _key
	, boolean _isCross
) {	rankFixedId = _rankFixedId;
	key = _key;
	isCross = _isCross;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)6; }

public long getRankFixedId() { return rankFixedId; }
public void setRankFixedId(long _rankFixedId) { rankFixedId = _rankFixedId; }
public long getKey() { return key; }
public void setKey(long _key) { key = _key; }
/** 是否跨服 */
public boolean getIsCross() { return isCross; }
/** 是否跨服 */
public void setIsCross(boolean _isCross) { isCross = _isCross; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rankFixedId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) key = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isCross = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(rankFixedId);
	_buf.putLong(key);
	_buf.put(isCross?(byte)1:(byte)0);
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

