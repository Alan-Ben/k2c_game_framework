package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 商店初始化
 **/
public class GS2GC_002_051_RetShopInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 商店列表数据 */
private java.util.ArrayList<Common.ShopObj.Shop_Info> shopList;


public GS2GC_002_051_RetShopInit() {
	shopList = new java.util.ArrayList<Common.ShopObj.Shop_Info>();
}

public GS2GC_002_051_RetShopInit(
	 java.util.ArrayList<Common.ShopObj.Shop_Info> _shopList
) {	shopList = _shopList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)51; }

/** 商店列表数据 */
public java.util.ArrayList<Common.ShopObj.Shop_Info> getShopList() { return shopList; }
/** 商店列表数据 */
public void addShopList(Common.ShopObj.Shop_Info _shopList) { shopList.add(_shopList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < shopList.size(); _i++) {
	_size += 4 + shopList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < shopList.size(); _i++) {
	_size += 4 + shopList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _shopListCount = _buf.getShort();
	for(int _i = 0; _i < _shopListCount; _i++) { 
		Common.ShopObj.Shop_Info _shopList = new Common.ShopObj.Shop_Info();
		if(_buf.remaining() <= 0) return;
	int __shopListCustLen = _buf.getInt();
	int __shopListCurPos = _buf.position();
	_shopList.ReadUnzipBuf(_buf, __shopListCurPos + __shopListCustLen);
	_buf.position(__shopListCurPos + __shopListCustLen);

		shopList.add(_shopList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)shopList.size());
	for(int _i = 0; _i < shopList.size(); _i++) { 
		_buf.putInt(shopList.get(_i).GetBufSize());
	shopList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)51);
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

