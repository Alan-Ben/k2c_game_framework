package GS2GC.p030_ShopOp;

import java.nio.ByteBuffer;
public class GS2GC_030_051_OnShopItemChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 商店配置id */
private long shopRefId;
/** 商品数据变更 */
private Common.ShopObj.Shop_ItemInfo shopItem;


public GS2GC_030_051_OnShopItemChg() {
	shopRefId = (long)0;
	shopItem = new Common.ShopObj.Shop_ItemInfo();
}

public GS2GC_030_051_OnShopItemChg(
	 long _shopRefId
	, Common.ShopObj.Shop_ItemInfo _shopItem
) {	shopRefId = _shopRefId;
	shopItem = _shopItem;
}

public final byte getMainOrder() { return (byte)30; }

public final byte getSubOrder() { return (byte)51; }

/** 商店配置id */
public long getShopRefId() { return shopRefId; }
/** 商店配置id */
public void setShopRefId(long _shopRefId) { shopRefId = _shopRefId; }
/** 商品数据变更 */
public Common.ShopObj.Shop_ItemInfo getShopItem() { return shopItem; }
/** 商品数据变更 */
public void setShopItem(Common.ShopObj.Shop_ItemInfo _shopItem) { shopItem = _shopItem; }


public final int GetBufSize() {
	int _size = 60;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 62;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shopRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _shopItemCustLen = _buf.getInt();
	int _shopItemCurPos = _buf.position();
	shopItem.ReadUnzipBuf(_buf, _shopItemCurPos + _shopItemCustLen);
	_buf.position(_shopItemCurPos + _shopItemCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(shopRefId);
	_buf.putInt(shopItem.GetBufSize());
	shopItem.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
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

