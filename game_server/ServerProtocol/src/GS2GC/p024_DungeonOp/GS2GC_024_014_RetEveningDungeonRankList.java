package GS2GC.p024_DungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_024_014_RetEveningDungeonRankList implements ALBasicProtocolPack._IALProtocolStructure {
/** 基础数据列表 */
private java.util.ArrayList<Common.RankObj.Rank_BaseItem> rankList;
/** 玩家排行数据 */
private Common.RankObj.Rank_BaseItem selfRankItem;


public GS2GC_024_014_RetEveningDungeonRankList() {
	rankList = new java.util.ArrayList<Common.RankObj.Rank_BaseItem>();
	selfRankItem = new Common.RankObj.Rank_BaseItem();
}

public GS2GC_024_014_RetEveningDungeonRankList(
	 java.util.ArrayList<Common.RankObj.Rank_BaseItem> _rankList
	, Common.RankObj.Rank_BaseItem _selfRankItem
) {	rankList = _rankList;
	selfRankItem = _selfRankItem;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)14; }

/** 基础数据列表 */
public java.util.ArrayList<Common.RankObj.Rank_BaseItem> getRankList() { return rankList; }
/** 基础数据列表 */
public void addRankList(Common.RankObj.Rank_BaseItem _rankList) { rankList.add(_rankList); }
/** 玩家排行数据 */
public Common.RankObj.Rank_BaseItem getSelfRankItem() { return selfRankItem; }
/** 玩家排行数据 */
public void setSelfRankItem(Common.RankObj.Rank_BaseItem _selfRankItem) { selfRankItem = _selfRankItem; }


public final int GetBufSize() {
	int _size = 32;
	_size += 2 + (rankList.size() * 32);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
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
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _selfRankItemCustLen = _buf.getInt();
	int _selfRankItemCurPos = _buf.position();
	selfRankItem.ReadUnzipBuf(_buf, _selfRankItemCurPos + _selfRankItemCustLen);
	_buf.position(_selfRankItemCurPos + _selfRankItemCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)rankList.size());
	for(int _i = 0; _i < rankList.size(); _i++) { 
		_buf.putInt(rankList.get(_i).GetBufSize());
	rankList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(selfRankItem.GetBufSize());
	selfRankItem.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)14);
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

