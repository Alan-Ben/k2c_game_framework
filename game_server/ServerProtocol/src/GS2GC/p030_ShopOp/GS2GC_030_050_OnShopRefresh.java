package GS2GC.p030_ShopOp;

import java.nio.ByteBuffer;
public class GS2GC_030_050_OnShopRefresh implements ALBasicProtocolPack._IALProtocolStructure {
/** 商店数据 */
private Common.ShopObj.Shop_Info shop;


public GS2GC_030_050_OnShopRefresh() {
	shop = new Common.ShopObj.Shop_Info();
}

public GS2GC_030_050_OnShopRefresh(
	 Common.ShopObj.Shop_Info _shop
) {	shop = _shop;
}

public final byte getMainOrder() { return (byte)30; }

public final byte getSubOrder() { return (byte)50; }

/** 商店数据 */
public Common.ShopObj.Shop_Info getShop() { return shop; }
/** 商店数据 */
public void setShop(Common.ShopObj.Shop_Info _shop) { shop = _shop; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + shop.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + shop.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _shopCustLen = _buf.getInt();
	int _shopCurPos = _buf.position();
	shop.ReadUnzipBuf(_buf, _shopCurPos + _shopCustLen);
	_buf.position(_shopCurPos + _shopCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(shop.GetBufSize());
	shop.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)50);
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

