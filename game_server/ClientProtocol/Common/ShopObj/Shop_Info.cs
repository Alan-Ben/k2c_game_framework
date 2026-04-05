using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ShopObj
{

/// <summary>
/// 商店数据
/// </summary>
public class Shop_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 商店配置id
/// </summary>
private long shopRefId;
/// <summary>
/// 下一次刷新时间戳
/// </summary>
private long nextRefreshTimeMs;
/// <summary>
/// 已刷新次数
/// </summary>
private int refreshNum;
/// <summary>
/// 商品列表
/// </summary>
private List<Common.ShopObj.Shop_ItemInfo> goodsList;


public Shop_Info() {
	shopRefId = (long)0;
	nextRefreshTimeMs = (long)0;
	refreshNum = 0;
	goodsList = new List<Common.ShopObj.Shop_ItemInfo>();
}

public Shop_Info(
	long _shopRefId
	, long _nextRefreshTimeMs
	, int _refreshNum
	, List<Common.ShopObj.Shop_ItemInfo> _goodsList
) {	shopRefId = _shopRefId;
	nextRefreshTimeMs = _nextRefreshTimeMs;
	refreshNum = _refreshNum;
	goodsList = _goodsList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 商店配置id
/// </summary>
public long getShopRefId() { return shopRefId; }
/// <summary>
/// 商店配置id
/// </summary>
public void setShopRefId(long _shopRefId) { shopRefId = _shopRefId; }
/// <summary>
/// 下一次刷新时间戳
/// </summary>
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/// <summary>
/// 下一次刷新时间戳
/// </summary>
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }
/// <summary>
/// 已刷新次数
/// </summary>
public int getRefreshNum() { return refreshNum; }
/// <summary>
/// 已刷新次数
/// </summary>
public void setRefreshNum(int _refreshNum) { refreshNum = _refreshNum; }
/// <summary>
/// 商品列表
/// </summary>
public List<Common.ShopObj.Shop_ItemInfo> getGoodsList() { return goodsList; }
/// <summary>
/// 商品列表
/// </summary>
public void addGoodsList(Common.ShopObj.Shop_ItemInfo _goodsList) { goodsList.Add(_goodsList); }


public int GetBufSize() {
	int _size = 20;
	_size += 2 + (goodsList.Count * 52);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (goodsList.Count * 52);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shopRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refreshNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _goodsListCount = _buf.getShort();
	for(int _i = 0; _i < _goodsListCount; _i++) { 
		Common.ShopObj.Shop_ItemInfo _goodsList = new Common.ShopObj.Shop_ItemInfo();
		int __goodsListCustLen = _buf.getInt();
	int __goodsListCurPos = _buf.getCurPos();
	_goodsList.ReadUnzipBuf(_buf, __goodsListCurPos + __goodsListCustLen);
	_buf.setPosition(__goodsListCurPos + __goodsListCustLen);

		goodsList.Add(_goodsList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(shopRefId);
	_buf.putLong(nextRefreshTimeMs);
	_buf.putInt(refreshNum);
	_buf.putShort((short)goodsList.Count);
	for(int _i = 0; _i < goodsList.Count; _i++) { 
		_buf.putInt(goodsList[_i].GetBufSize());
	goodsList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("nextRefreshTimeMs").Append(":").Append(nextRefreshTimeMs.ToString()).Append(", ");
	builder.Append("refreshNum").Append(":").Append(refreshNum.ToString()).Append(", ");
	builder.Append("goodsList").Append(":").Append(goodsList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

