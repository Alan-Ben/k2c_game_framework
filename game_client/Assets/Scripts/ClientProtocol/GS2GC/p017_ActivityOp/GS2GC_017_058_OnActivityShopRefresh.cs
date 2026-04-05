using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动商店刷新推送
/// </summary>
public class GS2GC_017_058_OnActivityShopRefresh : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例id
/// </summary>
private long instanceId;
/// <summary>
/// 活动商店信息
/// </summary>
private Common.ActivityObj.Activity_ShopInfo shopInfo;


public GS2GC_017_058_OnActivityShopRefresh() {
	instanceId = (long)0;
	shopInfo = new Common.ActivityObj.Activity_ShopInfo();
}

public GS2GC_017_058_OnActivityShopRefresh(
	long _instanceId
	, Common.ActivityObj.Activity_ShopInfo _shopInfo
) {	instanceId = _instanceId;
	shopInfo = _shopInfo;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)58; }

/// <summary>
/// 活动实例id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 活动商店信息
/// </summary>
public Common.ActivityObj.Activity_ShopInfo getShopInfo() { return shopInfo; }
/// <summary>
/// 活动商店信息
/// </summary>
public void setShopInfo(Common.ActivityObj.Activity_ShopInfo _shopInfo) { shopInfo = _shopInfo; }


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
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _shopInfoCustLen = _buf.getInt();
	int _shopInfoCurPos = _buf.getCurPos();
	shopInfo.ReadUnzipBuf(_buf, _shopInfoCurPos + _shopInfoCustLen);
	_buf.setPosition(_shopInfoCurPos + _shopInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(shopInfo.GetBufSize());
	shopInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)58);
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
	builder.Append("shopInfo").Append(":").Append(shopInfo == null ? "null" : shopInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

