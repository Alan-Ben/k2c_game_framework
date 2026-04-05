package GC2GS.p031_RankOp;

import java.nio.ByteBuffer;
/*********
 * 请求常驻排行榜基础信息
 **/
public class GC2GS_031_001_ReqRankFixedBaseList implements ALBasicProtocolPack._IALProtocolStructure {
private long rankFixedId;
/** 是否跨服 */
private boolean isCross;


public GC2GS_031_001_ReqRankFixedBaseList() {
	rankFixedId = (long)0;
	isCross = false;
}

public GC2GS_031_001_ReqRankFixedBaseList(
	 long _rankFixedId
	, boolean _isCross
) {	rankFixedId = _rankFixedId;
	isCross = _isCross;
}

public final byte getMainOrder() { return (byte)31; }

public final byte getSubOrder() { return (byte)1; }

public long getRankFixedId() { return rankFixedId; }
public void setRankFixedId(long _rankFixedId) { rankFixedId = _rankFixedId; }
/** 是否跨服 */
public boolean getIsCross() { return isCross; }
/** 是否跨服 */
public void setIsCross(boolean _isCross) { isCross = _isCross; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rankFixedId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isCross = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(rankFixedId);
	_buf.put(isCross?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)31);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)31);
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

