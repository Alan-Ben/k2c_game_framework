package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 公会副本伤害排行榜
 **/
public class GS2GC_037_007_RetDamageRank implements ALBasicProtocolPack._IALProtocolStructure {
/** 排行数据 */
private java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DamageRankItem> itemList;
private int myRank;
private long myValue;


public GS2GC_037_007_RetDamageRank() {
	itemList = new java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DamageRankItem>();
	myRank = 0;
	myValue = (long)0;
}

public GS2GC_037_007_RetDamageRank(
	 java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DamageRankItem> _itemList
	, int _myRank
	, long _myValue
) {	itemList = _itemList;
	myRank = _myRank;
	myValue = _myValue;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)7; }

/** 排行数据 */
public java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DamageRankItem> getItemList() { return itemList; }
/** 排行数据 */
public void addItemList(Common.GuildDungeonObj.GuildDungeon_DamageRankItem _itemList) { itemList.add(_itemList); }
public int getMyRank() { return myRank; }
public void setMyRank(int _myRank) { myRank = _myRank; }
public long getMyValue() { return myValue; }
public void setMyValue(long _myValue) { myValue = _myValue; }


public final int GetBufSize() {
	int _size = 12;
	_size += 2 + (itemList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (itemList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_DamageRankItem _itemList = new Common.GuildDungeonObj.GuildDungeon_DamageRankItem();
		if(_buf.remaining() <= 0) return;
	int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.position();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.position(__itemListCurPos + __itemListCustLen);

		itemList.add(_itemList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) myRank = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) myValue = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(myRank);
	_buf.putLong(myValue);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
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

