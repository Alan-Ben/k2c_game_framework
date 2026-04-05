using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ActivityObj
{

/// <summary>
/// 活动商店购买记录
/// </summary>
public class Activity_ShopBuyRecord : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 商品id
/// </summary>
private long itemId;
/// <summary>
/// 已购买数量
/// </summary>
private long hadBuyCount;


public Activity_ShopBuyRecord() {
	itemId = (long)0;
	hadBuyCount = (long)0;
}

public Activity_ShopBuyRecord(
	long _itemId
	, long _hadBuyCount
) {	itemId = _itemId;
	hadBuyCount = _hadBuyCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 商品id
/// </summary>
public long getItemId() { return itemId; }
/// <summary>
/// 商品id
/// </summary>
public void setItemId(long _itemId) { itemId = _itemId; }
/// <summary>
/// 已购买数量
/// </summary>
public long getHadBuyCount() { return hadBuyCount; }
/// <summary>
/// 已购买数量
/// </summary>
public void setHadBuyCount(long _hadBuyCount) { hadBuyCount = _hadBuyCount; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadBuyCount = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(itemId);
	_buf.putLong(hadBuyCount);
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
	builder.Append("itemId").Append(":").Append(itemId.ToString()).Append(", ");
	builder.Append("hadBuyCount").Append(":").Append(hadBuyCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

