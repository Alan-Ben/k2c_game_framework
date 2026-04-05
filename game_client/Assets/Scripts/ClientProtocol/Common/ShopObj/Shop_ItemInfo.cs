using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ShopObj
{

/// <summary>
/// 商店商品数据
/// </summary>
public class Shop_ItemInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 商品id
/// </summary>
private long instanceId;
/// <summary>
/// 商店商品配置id
/// </summary>
private long shopItemRefId;
/// <summary>
/// 已购买次数
/// </summary>
private long hasBuyNum;
/// <summary>
/// 折扣配置id
/// </summary>
private long discountRefId;
/// <summary>
/// 商品组id
/// </summary>
private long shopItemGroupId;
/// <summary>
/// 限购次数
/// </summary>
private long canBuyNum;


public Shop_ItemInfo() {
	instanceId = (long)0;
	shopItemRefId = (long)0;
	hasBuyNum = (long)0;
	discountRefId = (long)0;
	shopItemGroupId = (long)0;
	canBuyNum = (long)0;
}

public Shop_ItemInfo(
	long _instanceId
	, long _shopItemRefId
	, long _hasBuyNum
	, long _discountRefId
	, long _shopItemGroupId
	, long _canBuyNum
) {	instanceId = _instanceId;
	shopItemRefId = _shopItemRefId;
	hasBuyNum = _hasBuyNum;
	discountRefId = _discountRefId;
	shopItemGroupId = _shopItemGroupId;
	canBuyNum = _canBuyNum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 商品id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 商品id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 商店商品配置id
/// </summary>
public long getShopItemRefId() { return shopItemRefId; }
/// <summary>
/// 商店商品配置id
/// </summary>
public void setShopItemRefId(long _shopItemRefId) { shopItemRefId = _shopItemRefId; }
/// <summary>
/// 已购买次数
/// </summary>
public long getHasBuyNum() { return hasBuyNum; }
/// <summary>
/// 已购买次数
/// </summary>
public void setHasBuyNum(long _hasBuyNum) { hasBuyNum = _hasBuyNum; }
/// <summary>
/// 折扣配置id
/// </summary>
public long getDiscountRefId() { return discountRefId; }
/// <summary>
/// 折扣配置id
/// </summary>
public void setDiscountRefId(long _discountRefId) { discountRefId = _discountRefId; }
/// <summary>
/// 商品组id
/// </summary>
public long getShopItemGroupId() { return shopItemGroupId; }
/// <summary>
/// 商品组id
/// </summary>
public void setShopItemGroupId(long _shopItemGroupId) { shopItemGroupId = _shopItemGroupId; }
/// <summary>
/// 限购次数
/// </summary>
public long getCanBuyNum() { return canBuyNum; }
/// <summary>
/// 限购次数
/// </summary>
public void setCanBuyNum(long _canBuyNum) { canBuyNum = _canBuyNum; }


public int GetBufSize() {
	int _size = 48;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 50;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shopItemRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasBuyNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	discountRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shopItemGroupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	canBuyNum = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(shopItemRefId);
	_buf.putLong(hasBuyNum);
	_buf.putLong(discountRefId);
	_buf.putLong(shopItemGroupId);
	_buf.putLong(canBuyNum);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("shopItemRefId").Append(":").Append(shopItemRefId.ToString()).Append(", ");
	builder.Append("hasBuyNum").Append(":").Append(hasBuyNum.ToString()).Append(", ");
	builder.Append("discountRefId").Append(":").Append(discountRefId.ToString()).Append(", ");
	builder.Append("shopItemGroupId").Append(":").Append(shopItemGroupId.ToString()).Append(", ");
	builder.Append("canBuyNum").Append(":").Append(canBuyNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

