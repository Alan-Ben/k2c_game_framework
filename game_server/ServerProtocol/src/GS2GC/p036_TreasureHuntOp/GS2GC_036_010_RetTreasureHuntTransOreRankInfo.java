package GS2GC.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
public class GS2GC_036_010_RetTreasureHuntTransOreRankInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿石排行榜前三 */
private java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_OreRankItem> topThreeList;
/** 当前排名 */
private int rank;


public GS2GC_036_010_RetTreasureHuntTransOreRankInfo() {
	topThreeList = new java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_OreRankItem>();
	rank = 0;
}

public GS2GC_036_010_RetTreasureHuntTransOreRankInfo(
	 java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_OreRankItem> _topThreeList
	, int _rank
) {	topThreeList = _topThreeList;
	rank = _rank;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)10; }

/** 矿石排行榜前三 */
public java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_OreRankItem> getTopThreeList() { return topThreeList; }
/** 矿石排行榜前三 */
public void addTopThreeList(Common.TreasureHuntObj.TreasureHunt_OreRankItem _topThreeList) { topThreeList.add(_topThreeList); }
/** 当前排名 */
public int getRank() { return rank; }
/** 当前排名 */
public void setRank(int _rank) { rank = _rank; }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (topThreeList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (topThreeList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _topThreeListCount = _buf.getShort();
	for(int _i = 0; _i < _topThreeListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_OreRankItem _topThreeList = new Common.TreasureHuntObj.TreasureHunt_OreRankItem();
		if(_buf.remaining() <= 0) return;
	int __topThreeListCustLen = _buf.getInt();
	int __topThreeListCurPos = _buf.position();
	_topThreeList.ReadUnzipBuf(_buf, __topThreeListCurPos + __topThreeListCustLen);
	_buf.position(__topThreeListCurPos + __topThreeListCustLen);

		topThreeList.add(_topThreeList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rank = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)topThreeList.size());
	for(int _i = 0; _i < topThreeList.size(); _i++) { 
		_buf.putInt(topThreeList.get(_i).GetBufSize());
	topThreeList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(rank);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)10);
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

