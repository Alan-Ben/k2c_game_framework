using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p030_ShopOp
{

public class GC2GS_030_001_ReqBuyShopItem : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 商店配置id
/// </summary>
private long shopRefId;
/// <summary>
/// 商品id
/// </summary>
private long instanceId;
/// <summary>
/// 数量
/// </summary>
private long count;


public GC2GS_030_001_ReqBuyShopItem() {
	shopRefId = (long)0;
	instanceId = (long)0;
	count = (long)0;
}

public GC2GS_030_001_ReqBuyShopItem(
	long _shopRefId
	, long _instanceId
	, long _count
) {	shopRefId = _shopRefId;
	instanceId = _instanceId;
	count = _count;
}

public byte getMainOrder() { return (byte)30; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 商店配置id
/// </summary>
public long getShopRefId() { return shopRefId; }
/// <summary>
/// 商店配置id
/// </summary>
public void setShopRefId(long _shopRefId) { shopRefId = _shopRefId; }
/// <summary>
/// 商品id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 商品id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 数量
/// </summary>
public long getCount() { return count; }
/// <summary>
/// 数量
/// </summary>
public void setCount(long _count) { count = _count; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shopRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(shopRefId);
	_buf.putLong(instanceId);
	_buf.putLong(count);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)1);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

