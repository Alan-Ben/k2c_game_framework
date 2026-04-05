using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动商店商品购买记录推送
/// </summary>
public class GS2GC_017_060_OnActivityShopItemBuy : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例id
/// </summary>
private long instanceId;
/// <summary>
/// 商店id
/// </summary>
private long shopId;
/// <summary>
/// 购买记录
/// </summary>
private Common.ActivityObj.Activity_ShopBuyRecord buyRecord;


public GS2GC_017_060_OnActivityShopItemBuy() {
	instanceId = (long)0;
	shopId = (long)0;
	buyRecord = new Common.ActivityObj.Activity_ShopBuyRecord();
}

public GS2GC_017_060_OnActivityShopItemBuy(
	long _instanceId
	, long _shopId
	, Common.ActivityObj.Activity_ShopBuyRecord _buyRecord
) {	instanceId = _instanceId;
	shopId = _shopId;
	buyRecord = _buyRecord;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)60; }

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
/// 购买记录
/// </summary>
public Common.ActivityObj.Activity_ShopBuyRecord getBuyRecord() { return buyRecord; }
/// <summary>
/// 购买记录
/// </summary>
public void setBuyRecord(Common.ActivityObj.Activity_ShopBuyRecord _buyRecord) { buyRecord = _buyRecord; }


public int GetBufSize() {
	int _size = 36;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shopId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _buyRecordCustLen = _buf.getInt();
	int _buyRecordCurPos = _buf.getCurPos();
	buyRecord.ReadUnzipBuf(_buf, _buyRecordCurPos + _buyRecordCustLen);
	_buf.setPosition(_buyRecordCurPos + _buyRecordCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(shopId);
	_buf.putInt(buyRecord.GetBufSize());
	buyRecord.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)60);
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
	builder.Append("buyRecord").Append(":").Append(buyRecord == null ? "null" : buyRecord.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

