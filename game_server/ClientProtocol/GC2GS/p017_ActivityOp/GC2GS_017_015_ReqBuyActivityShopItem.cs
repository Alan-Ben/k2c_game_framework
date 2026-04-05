using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p017_ActivityOp
{

/// <summary>
/// 购买活动商店商品
/// </summary>
public class GC2GS_017_015_ReqBuyActivityShopItem : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例id
/// </summary>
private long instanceId;
/// <summary>
/// 商店id
/// </summary>
private long shopId;
/// <summary>
/// 商品id
/// </summary>
private long itemId;
/// <summary>
/// 购买数量
/// </summary>
private int num;


public GC2GS_017_015_ReqBuyActivityShopItem() {
	instanceId = (long)0;
	shopId = (long)0;
	itemId = (long)0;
	num = 0;
}

public GC2GS_017_015_ReqBuyActivityShopItem(
	long _instanceId
	, long _shopId
	, long _itemId
	, int _num
) {	instanceId = _instanceId;
	shopId = _shopId;
	itemId = _itemId;
	num = _num;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)15; }

/// <summary>
/// 活动实例id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 商店id
/// </summary>
public long getShopId() { return shopId; }
/// <summary>
/// 商店id
/// </summary>
public void setShopId(long _shopId) { shopId = _shopId; }
/// <summary>
/// 商品id
/// </summary>
public long getItemId() { return itemId; }
/// <summary>
/// 商品id
/// </summary>
public void setItemId(long _itemId) { itemId = _itemId; }
/// <summary>
/// 购买数量
/// </summary>
public int getNum() { return num; }
/// <summary>
/// 购买数量
/// </summary>
public void setNum(int _num) { num = _num; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shopId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(shopId);
	_buf.putLong(itemId);
	_buf.putInt(num);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)15);
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
	builder.Append("shopId").Append(":").Append(shopId.ToString()).Append(", ");
	builder.Append("itemId").Append(":").Append(itemId.ToString()).Append(", ");
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

