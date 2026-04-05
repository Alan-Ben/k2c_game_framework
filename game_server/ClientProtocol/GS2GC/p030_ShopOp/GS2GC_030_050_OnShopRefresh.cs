using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p030_ShopOp
{

public class GS2GC_030_050_OnShopRefresh : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 商店数据
/// </summary>
private Common.ShopObj.Shop_Info shop;


public GS2GC_030_050_OnShopRefresh() {
	shop = new Common.ShopObj.Shop_Info();
}

public GS2GC_030_050_OnShopRefresh(
	Common.ShopObj.Shop_Info _shop
) {	shop = _shop;
}

public byte getMainOrder() { return (byte)30; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 商店数据
/// </summary>
public Common.ShopObj.Shop_Info getShop() { return shop; }
/// <summary>
/// 商店数据
/// </summary>
public void setShop(Common.ShopObj.Shop_Info _shop) { shop = _shop; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + shop.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + shop.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _shopCustLen = _buf.getInt();
	int _shopCurPos = _buf.getCurPos();
	shop.ReadUnzipBuf(_buf, _shopCurPos + _shopCustLen);
	_buf.setPosition(_shopCurPos + _shopCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(shop.GetBufSize());
	shop.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)50);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("shop").Append(":").Append(shop == null ? "null" : shop.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

