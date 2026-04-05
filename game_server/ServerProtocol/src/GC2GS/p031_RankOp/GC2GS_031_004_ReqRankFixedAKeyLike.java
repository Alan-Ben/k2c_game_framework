package GC2GS.p031_RankOp;

import java.nio.ByteBuffer;
/*********
 * 请求常驻排行榜一键点赞
 **/
public class GC2GS_031_004_ReqRankFixedAKeyLike implements ALBasicProtocolPack._IALProtocolStructure {
/** 常驻排行榜id列表 */
private java.util.ArrayList<Long> rankFixedIdList;
/** 是否跨服 */
private boolean isCross;


public GC2GS_031_004_ReqRankFixedAKeyLike() {
	rankFixedIdList = new java.util.ArrayList<Long>();
	isCross = false;
}

public GC2GS_031_004_ReqRankFixedAKeyLike(
	 java.util.ArrayList<Long> _rankFixedIdList
	, boolean _isCross
) {	rankFixedIdList = _rankFixedIdList;
	isCross = _isCross;
}

public final byte getMainOrder() { return (byte)31; }

public final byte getSubOrder() { return (byte)4; }

/** 常驻排行榜id列表 */
public java.util.ArrayList<Long> getRankFixedIdList() { return rankFixedIdList; }
/** 常驻排行榜id列表 */
public void addRankFixedIdList(long _rankFixedIdList) { rankFixedIdList.add(_rankFixedIdList); }
/** 是否跨服 */
public boolean getIsCross() { return isCross; }
/** 是否跨服 */
public void setIsCross(boolean _isCross) { isCross = _isCross; }


public final int GetBufSize() {
	int _size = 1;
	_size += 2 + (rankFixedIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += 2 + (rankFixedIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rankFixedIdListCount = _buf.getShort();
	for(int _i = 0; _i < _rankFixedIdListCount; _i++) { 
		long _rankFixedIdList = (long)0;
		if(_buf.remaining() > 0) _rankFixedIdList = _buf.getLong();
		rankFixedIdList.add(_rankFixedIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isCross = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)rankFixedIdList.size());
	for(int _i = 0; _i < rankFixedIdList.size(); _i++) { 
		_buf.putLong(rankFixedIdList.get(_i));
	}
	_buf.put(isCross?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)31);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)31);
	_recBuf.put((byte)4);
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

