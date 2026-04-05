using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GS2GC.p200_HotSimpleActivityOp
{

/// <summary>
/// 万能活动商店购买物品推送
/// </summary>
public class GS2GC_200_101_OnRegularActivityShopItemBuy : ALBasicProtocolPack._IALProtocolStructure {
private long activityInstanceId;
/// <summary>
/// 购买记录
/// </summary>
private Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord buyRecord;


public GS2GC_200_101_OnRegularActivityShopItemBuy() {
	activityInstanceId = (long)0;
	buyRecord = new Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord();
}

public GS2GC_200_101_OnRegularActivityShopItemBuy(
	long _activityInstanceId
	, Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord _buyRecord
) {	activityInstanceId = _activityInstanceId;
	buyRecord = _buyRecord;
}

public byte getMainOrder() { return (byte)200; }

public byte getSubOrder() { return (byte)101; }

public long getActivityInstanceId() { return activityInstanceId; }
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/// <summary>
/// 购买记录
/// </summary>
public Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord getBuyRecord() { return buyRecord; }
/// <summary>
/// 购买记录
/// </summary>
public void setBuyRecord(Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord _buyRecord) { buyRecord = _buyRecord; }


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
	activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _buyRecordCustLen = _buf.getInt();
	int _buyRecordCurPos = _buf.getCurPos();
	buyRecord.ReadUnzipBuf(_buf, _buyRecordCurPos + _buyRecordCustLen);
	_buf.setPosition(_buyRecordCurPos + _buyRecordCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putInt(buyRecord.GetBufSize());
	buyRecord.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)200);
	_buf.put((byte)101);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)200);
	_recBuf.put((byte)101);
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
	builder.Append("activityInstanceId").Append(":").Append(activityInstanceId.ToString()).Append(", ");
	builder.Append("buyRecord").Append(":").Append(buyRecord == null ? "null" : buyRecord.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

