package GS2GC.p031_RankOp;

import java.nio.ByteBuffer;
public class GS2GC_031_004_RetRankFixedAKeyLike implements ALBasicProtocolPack._IALProtocolStructure {
/** 点赞结果列表 */
private java.util.ArrayList<Common.RankObj.RankFixed_LikeResult> likeResultList;


public GS2GC_031_004_RetRankFixedAKeyLike() {
	likeResultList = new java.util.ArrayList<Common.RankObj.RankFixed_LikeResult>();
}

public GS2GC_031_004_RetRankFixedAKeyLike(
	 java.util.ArrayList<Common.RankObj.RankFixed_LikeResult> _likeResultList
) {	likeResultList = _likeResultList;
}

public final byte getMainOrder() { return (byte)31; }

public final byte getSubOrder() { return (byte)4; }

/** 点赞结果列表 */
public java.util.ArrayList<Common.RankObj.RankFixed_LikeResult> getLikeResultList() { return likeResultList; }
/** 点赞结果列表 */
public void addLikeResultList(Common.RankObj.RankFixed_LikeResult _likeResultList) { likeResultList.add(_likeResultList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < likeResultList.size(); _i++) {
	_size += 4 + likeResultList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < likeResultList.size(); _i++) {
	_size += 4 + likeResultList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _likeResultListCount = _buf.getShort();
	for(int _i = 0; _i < _likeResultListCount; _i++) { 
		Common.RankObj.RankFixed_LikeResult _likeResultList = new Common.RankObj.RankFixed_LikeResult();
		if(_buf.remaining() <= 0) return;
	int __likeResultListCustLen = _buf.getInt();
	int __likeResultListCurPos = _buf.position();
	_likeResultList.ReadUnzipBuf(_buf, __likeResultListCurPos + __likeResultListCustLen);
	_buf.position(__likeResultListCurPos + __likeResultListCustLen);

		likeResultList.add(_likeResultList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)likeResultList.size());
	for(int _i = 0; _i < likeResultList.size(); _i++) { 
		_buf.putInt(likeResultList.get(_i).GetBufSize());
	likeResultList.get(_i).PutUnzipBuf(_buf);
	}
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

