using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p030_ShopOp
{

public class GS2GC_030_051_OnShopItemChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 商店配置id
/// </summary>
private long shopRefId;
/// <summary>
/// 商品数据变更
/// </summary>
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

public byte getMainOrder() { return (byte)30; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 商店配置id
/// </summary>
public long getShopRefId() { return shopRefId; }
/// <summary>
/// 商店配置id
/// </summary>
public void setShopRefId(long _shopRefId) { shopRefId = _shopRefId; }
/// <summary>
/// 商品数据变更
/// </summary>
public Common.ShopObj.Shop_ItemInfo getShopItem() { return shopItem; }
/// <summary>
/// 商品数据变更
/// </summary>
public void setShopItem(Common.ShopObj.Shop_ItemInfo _shopItem) { shopItem = _shopItem; }


public int GetBufSize() {
	int _size = 60;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 62;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shopRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _shopItemCustLen = _buf.getInt();
	int _shopItemCurPos = _buf.getCurPos();
	shopItem.ReadUnzipBuf(_buf, _shopItemCurPos + _shopItemCustLen);
	_buf.setPosition(_shopItemCurPos + _shopItemCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(shopRefId);
	_buf.putInt(shopItem.GetBufSize());
	shopItem.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)51);
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
	builder.Append("shopRefId").Append(":").Append(shopRefId.ToString()).Append(", ");
	builder.Append("shopItem").Append(":").Append(shopItem == null ? "null" : shopItem.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

