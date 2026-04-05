package GS2GC.p006_BagItemOp;

import java.nio.ByteBuffer;
public class GS2GC_006_009_RetBagUseItemForSelectHero implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.BagItemUseObj.BagItemUse_HeroShowInfo> itemList;


public GS2GC_006_009_RetBagUseItemForSelectHero() {
	itemList = new java.util.ArrayList<Common.BagItemUseObj.BagItemUse_HeroShowInfo>();
}

public GS2GC_006_009_RetBagUseItemForSelectHero(
	 java.util.ArrayList<Common.BagItemUseObj.BagItemUse_HeroShowInfo> _itemList
) {	itemList = _itemList;
}

public final byte getMainOrder() { return (byte)6; }

public final byte getSubOrder() { return (byte)9; }

public java.util.ArrayList<Common.BagItemUseObj.BagItemUse_HeroShowInfo> getItemList() { return itemList; }
public void addItemList(Common.BagItemUseObj.BagItemUse_HeroShowInfo _itemList) { itemList.add(_itemList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (itemList.size() * 24);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (itemList.size() * 24);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		Common.BagItemUseObj.BagItemUse_HeroShowInfo _itemList = new Common.BagItemUseObj.BagItemUse_HeroShowInfo();
		if(_buf.remaining() <= 0) return;
	int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.position();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.position(__itemListCurPos + __itemListCustLen);

		itemList.add(_itemList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)9);
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

