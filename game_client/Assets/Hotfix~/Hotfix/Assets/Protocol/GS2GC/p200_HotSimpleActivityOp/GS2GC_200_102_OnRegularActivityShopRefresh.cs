using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GS2GC.p200_HotSimpleActivityOp
{

/// <summary>
/// 万能活动商店刷新推送
/// </summary>
public class GS2GC_200_102_OnRegularActivityShopRefresh : ALBasicProtocolPack._IALProtocolStructure {
private long activityInstanceId;
/// <summary>
/// 万能活动商店信息
/// </summary>
private Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopInfo shopInfo;


public GS2GC_200_102_OnRegularActivityShopRefresh() {
	activityInstanceId = (long)0;
	shopInfo = new Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopInfo();
}

public GS2GC_200_102_OnRegularActivityShopRefresh(
	long _activityInstanceId
	, Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopInfo _shopInfo
) {	activityInstanceId = _activityInstanceId;
	shopInfo = _shopInfo;
}

public byte getMainOrder() { return (byte)200; }

public byte getSubOrder() { return (byte)102; }

public long getActivityInstanceId() { return activityInstanceId; }
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/// <summary>
/// 万能活动商店信息
/// </summary>
public Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopInfo getShopInfo() { return shopInfo; }
/// <summary>
/// 万能活动商店信息
/// </summary>
public void setShopInfo(Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopInfo _shopInfo) { shopInfo = _shopInfo; }


public int GetBufSize() {
	int _size = 8;
	_size += 4 + shopInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + shopInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _shopInfoCustLen = _buf.getInt();
	int _shopInfoCurPos = _buf.getCurPos();
	shopInfo.ReadUnzipBuf(_buf, _shopInfoCurPos + _shopInfoCustLen);
	_buf.setPosition(_shopInfoCurPos + _shopInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putInt(shopInfo.GetBufSize());
	shopInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)200);
	_buf.put((byte)102);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)200);
	_recBuf.put((byte)102);
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
	builder.Append("shopInfo").Append(":").Append(shopInfo == null ? "null" : shopInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

