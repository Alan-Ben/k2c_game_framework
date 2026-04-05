package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_007_RetCelebrityRank implements ALBasicProtocolPack._IALProtocolStructure {
/** 名人榜列表 */
private java.util.ArrayList<Common.ArenaObj.Arena_CelebrityRankInfo> rankList;


public GS2GC_023_007_RetCelebrityRank() {
	rankList = new java.util.ArrayList<Common.ArenaObj.Arena_CelebrityRankInfo>();
}

public GS2GC_023_007_RetCelebrityRank(
	 java.util.ArrayList<Common.ArenaObj.Arena_CelebrityRankInfo> _rankList
) {	rankList = _rankList;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)7; }

/** 名人榜列表 */
public java.util.ArrayList<Common.ArenaObj.Arena_CelebrityRankInfo> getRankList() { return rankList; }
/** 名人榜列表 */
public void addRankList(Common.ArenaObj.Arena_CelebrityRankInfo _rankList) { rankList.add(_rankList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < rankList.size(); _i++) {
	_size += 4 + rankList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < rankList.size(); _i++) {
	_size += 4 + rankList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rankListCount = _buf.getShort();
	for(int _i = 0; _i < _rankListCount; _i++) { 
		Common.ArenaObj.Arena_CelebrityRankInfo _rankList = new Common.ArenaObj.Arena_CelebrityRankInfo();
		if(_buf.remaining() <= 0) return;
	int __rankListCustLen = _buf.getInt();
	int __rankListCurPos = _buf.position();
	_rankList.ReadUnzipBuf(_buf, __rankListCurPos + __rankListCustLen);
	_buf.position(__rankListCurPos + __rankListCustLen);

		rankList.add(_rankList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)rankList.size());
	for(int _i = 0; _i < rankList.size(); _i++) { 
		_buf.putInt(rankList.get(_i).GetBufSize());
	rankList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)7);
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

