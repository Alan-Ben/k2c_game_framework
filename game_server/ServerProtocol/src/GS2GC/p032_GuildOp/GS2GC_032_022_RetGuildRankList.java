package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_022_RetGuildRankList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.RankObj.Rank_BaseItem> rankList;


public GS2GC_032_022_RetGuildRankList() {
	rankList = new java.util.ArrayList<Common.RankObj.Rank_BaseItem>();
}

public GS2GC_032_022_RetGuildRankList(
	 java.util.ArrayList<Common.RankObj.Rank_BaseItem> _rankList
) {	rankList = _rankList;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)22; }

public java.util.ArrayList<Common.RankObj.Rank_BaseItem> getRankList() { return rankList; }
public void addRankList(Common.RankObj.Rank_BaseItem _rankList) { rankList.add(_rankList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (rankList.size() * 32);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (rankList.size() * 32);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rankListCount = _buf.getShort();
	for(int _i = 0; _i < _rankListCount; _i++) { 
		Common.RankObj.Rank_BaseItem _rankList = new Common.RankObj.Rank_BaseItem();
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
	_buf.put((byte)32);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)22);
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

