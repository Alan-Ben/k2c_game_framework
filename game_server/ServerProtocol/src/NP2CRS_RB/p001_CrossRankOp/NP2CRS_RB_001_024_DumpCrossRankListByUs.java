package NP2CRS_RB.p001_CrossRankOp;

import java.nio.ByteBuffer;
public class NP2CRS_RB_001_024_DumpCrossRankListByUs implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.RankObj.Rank_ItemDump> rankItemList;


public NP2CRS_RB_001_024_DumpCrossRankListByUs() {
	rankItemList = new java.util.ArrayList<Common.RankObj.Rank_ItemDump>();
}

public NP2CRS_RB_001_024_DumpCrossRankListByUs(
	 java.util.ArrayList<Common.RankObj.Rank_ItemDump> _rankItemList
) {	rankItemList = _rankItemList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)24; }

public java.util.ArrayList<Common.RankObj.Rank_ItemDump> getRankItemList() { return rankItemList; }
public void addRankItemList(Common.RankObj.Rank_ItemDump _rankItemList) { rankItemList.add(_rankItemList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (rankItemList.size() * 32);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (rankItemList.size() * 32);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rankItemListCount = _buf.getShort();
	for(int _i = 0; _i < _rankItemListCount; _i++) { 
		Common.RankObj.Rank_ItemDump _rankItemList = new Common.RankObj.Rank_ItemDump();
		if(_buf.remaining() <= 0) return;
	int __rankItemListCustLen = _buf.getInt();
	int __rankItemListCurPos = _buf.position();
	_rankItemList.ReadUnzipBuf(_buf, __rankItemListCurPos + __rankItemListCustLen);
	_buf.position(__rankItemListCurPos + __rankItemListCustLen);

		rankItemList.add(_rankItemList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)rankItemList.size());
	for(int _i = 0; _i < rankItemList.size(); _i++) { 
		_buf.putInt(rankItemList.get(_i).GetBufSize());
	rankItemList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)24);
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

