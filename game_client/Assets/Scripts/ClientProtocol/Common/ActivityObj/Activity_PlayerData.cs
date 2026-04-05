using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ActivityObj
{

/// <summary>
/// 活动玩家相关信息
/// </summary>
public class Activity_PlayerData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;
/// <summary>
/// 商店信息
/// </summary>
private Common.ActivityObj.Activity_ShopInfo shopInfo;
/// <summary>
/// 钻石礼包信息
/// </summary>
private Common.CommonFuncObj.CrystalGiftPack_Info crystalGiftPackInfo;


public Activity_PlayerData() {
	activityInstanceId = (long)0;
	shopInfo = new Common.ActivityObj.Activity_ShopInfo();
	crystalGiftPackInfo = new Common.CommonFuncObj.CrystalGiftPack_Info();
}

public Activity_PlayerData(
	long _activityInstanceId
	, Common.ActivityObj.Activity_ShopInfo _shopInfo
	, Common.CommonFuncObj.CrystalGiftPack_Info _crystalGiftPackInfo
) {	activityInstanceId = _activityInstanceId;
	shopInfo = _shopInfo;
	crystalGiftPackInfo = _crystalGiftPackInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getActivityInstanceId() { return activityInstanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/// <summary>
/// 商店信息
/// </summary>
public Common.ActivityObj.Activity_ShopInfo getShopInfo() { return shopInfo; }
/// <summary>
/// 商店信息
/// </summary>
public void setShopInfo(Common.ActivityObj.Activity_ShopInfo _shopInfo) { shopInfo = _shopInfo; }
/// <summary>
/// 钻石礼包信息
/// </summary>
public Common.CommonFuncObj.CrystalGiftPack_Info getCrystalGiftPackInfo() { return crystalGiftPackInfo; }
/// <summary>
/// 钻石礼包信息
/// </summary>
public void setCrystalGiftPackInfo(Common.CommonFuncObj.CrystalGiftPack_Info _crystalGiftPackInfo) { crystalGiftPackInfo = _crystalGiftPackInfo; }


public int GetBufSize() {
	int _size = 8;
	_size += 4 + shopInfo.GetBufSize();
	_size += 4 + crystalGiftPackInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + shopInfo.GetBufSize();
	_size += 4 + crystalGiftPackInfo.GetBufSize();

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

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _crystalGiftPackInfoCustLen = _buf.getInt();
	int _crystalGiftPackInfoCurPos = _buf.getCurPos();
	crystalGiftPackInfo.ReadUnzipBuf(_buf, _crystalGiftPackInfoCurPos + _crystalGiftPackInfoCustLen);
	_buf.setPosition(_crystalGiftPackInfoCurPos + _crystalGiftPackInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putInt(shopInfo.GetBufSize());
	shopInfo.PutUnzipBuf(_buf);
	_buf.putInt(crystalGiftPackInfo.GetBufSize());
	crystalGiftPackInfo.PutUnzipBuf(_buf);
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
	builder.Append("activityInstanceId").Append(":").Append(activityInstanceId.ToString()).Append(", ");
	builder.Append("shopInfo").Append(":").Append(shopInfo == null ? "null" : shopInfo.ToString()).Append(", ");
	builder.Append("crystalGiftPackInfo").Append(":").Append(crystalGiftPackInfo == null ? "null" : crystalGiftPackInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

