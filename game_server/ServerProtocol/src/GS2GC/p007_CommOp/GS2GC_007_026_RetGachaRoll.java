package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_026_RetGachaRoll implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否十连抽 */
private boolean isTen;
/** 物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> itemList;
/** 物品ID列表 */
private java.util.ArrayList<Long> poolItemIdList;


public GS2GC_007_026_RetGachaRoll() {
	isTen = false;
	itemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	poolItemIdList = new java.util.ArrayList<Long>();
}

public GS2GC_007_026_RetGachaRoll(
	 boolean _isTen
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _itemList
	, java.util.ArrayList<Long> _poolItemIdList
) {	isTen = _isTen;
	itemList = _itemList;
	poolItemIdList = _poolItemIdList;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)26; }

/** 是否十连抽 */
public boolean getIsTen() { return isTen; }
/** 是否十连抽 */
public void setIsTen(boolean _isTen) { isTen = _isTen; }
/** 物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/** 物品列表 */
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.add(_itemList); }
/** 物品ID列表 */
public java.util.ArrayList<Long> getPoolItemIdList() { return poolItemIdList; }
/** 物品ID列表 */
public void addPoolItemIdList(long _poolItemIdList) { poolItemIdList.add(_poolItemIdList); }


public final int GetBufSize() {
	int _size = 1;
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}

	_size += 2 + (poolItemIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}

	_size += 2 + (poolItemIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isTen = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.position();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.position(__itemListCurPos + __itemListCustLen);

		itemList.add(_itemList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _poolItemIdListCount = _buf.getShort();
	for(int _i = 0; _i < _poolItemIdListCount; _i++) { 
		long _poolItemIdList = (long)0;
		if(_buf.remaining() > 0) _poolItemIdList = _buf.getLong();
		poolItemIdList.add(_poolItemIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isTen?(byte)1:(byte)0);
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)poolItemIdList.size());
	for(int _i = 0; _i < poolItemIdList.size(); _i++) { 
		_buf.putLong(poolItemIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)26);
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

