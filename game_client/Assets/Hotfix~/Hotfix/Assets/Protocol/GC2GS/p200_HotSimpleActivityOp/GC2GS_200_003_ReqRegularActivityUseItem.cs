using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.GC2GS.p200_HotSimpleActivityOp
{

/// <summary>
/// 万能活动使用物品
/// </summary>
public class GC2GS_200_003_ReqRegularActivityUseItem : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;
/// <summary>
/// 物品ID
/// </summary>
private long itemId;
/// <summary>
/// 是否十连
/// </summary>
private bool isTen;


public GC2GS_200_003_ReqRegularActivityUseItem() {
	activityInstanceId = (long)0;
	itemId = (long)0;
	isTen = false;
}

public GC2GS_200_003_ReqRegularActivityUseItem(
	long _activityInstanceId
	, long _itemId
	, bool _isTen
) {	activityInstanceId = _activityInstanceId;
	itemId = _itemId;
	isTen = _isTen;
}

public byte getMainOrder() { return (byte)200; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getActivityInstanceId() { return activityInstanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/// <summary>
/// 物品ID
/// </summary>
public long getItemId() { return itemId; }
/// <summary>
/// 物品ID
/// </summary>
public void setItemId(long _itemId) { itemId = _itemId; }
/// <summary>
/// 是否十连
/// </summary>
public bool getIsTen() { return isTen; }
/// <summary>
/// 是否十连
/// </summary>
public void setIsTen(bool _isTen) { isTen = _isTen; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isTen = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(itemId);
	_buf.put(isTen?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)200);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)200);
	_recBuf.put((byte)3);
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
	builder.Append("itemId").Append(":").Append(itemId.ToString()).Append(", ");
	builder.Append("isTen").Append(":").Append(isTen.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

