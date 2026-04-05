using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 金币初始化
/// </summary>
public class GS2GC_002_011_RetGoldInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 金币信息
/// </summary>
private Common.PlayerObj.Player_GoldInfo goldInfo;
/// <summary>
/// 离线产出数量
/// </summary>
private long offlineProduceCount;
/// <summary>
/// 离线产出时间
/// </summary>
private long offlineProduceTime;


public GS2GC_002_011_RetGoldInit() {
	goldInfo = new Common.PlayerObj.Player_GoldInfo();
	offlineProduceCount = (long)0;
	offlineProduceTime = (long)0;
}

public GS2GC_002_011_RetGoldInit(
	Common.PlayerObj.Player_GoldInfo _goldInfo
	, long _offlineProduceCount
	, long _offlineProduceTime
) {	goldInfo = _goldInfo;
	offlineProduceCount = _offlineProduceCount;
	offlineProduceTime = _offlineProduceTime;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)11; }

/// <summary>
/// 金币信息
/// </summary>
public Common.PlayerObj.Player_GoldInfo getGoldInfo() { return goldInfo; }
/// <summary>
/// 金币信息
/// </summary>
public void setGoldInfo(Common.PlayerObj.Player_GoldInfo _goldInfo) { goldInfo = _goldInfo; }
/// <summary>
/// 离线产出数量
/// </summary>
public long getOfflineProduceCount() { return offlineProduceCount; }
/// <summary>
/// 离线产出数量
/// </summary>
public void setOfflineProduceCount(long _offlineProduceCount) { offlineProduceCount = _offlineProduceCount; }
/// <summary>
/// 离线产出时间
/// </summary>
public long getOfflineProduceTime() { return offlineProduceTime; }
/// <summary>
/// 离线产出时间
/// </summary>
public void setOfflineProduceTime(long _offlineProduceTime) { offlineProduceTime = _offlineProduceTime; }


public int GetBufSize() {
	int _size = 52;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 54;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _goldInfoCustLen = _buf.getInt();
	int _goldInfoCurPos = _buf.getCurPos();
	goldInfo.ReadUnzipBuf(_buf, _goldInfoCurPos + _goldInfoCustLen);
	_buf.setPosition(_goldInfoCurPos + _goldInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	offlineProduceCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	offlineProduceTime = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(goldInfo.GetBufSize());
	goldInfo.PutUnzipBuf(_buf);
	_buf.putLong(offlineProduceCount);
	_buf.putLong(offlineProduceTime);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)11);
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
	builder.Append("goldInfo").Append(":").Append(goldInfo == null ? "null" : goldInfo.ToString()).Append(", ");
	builder.Append("offlineProduceCount").Append(":").Append(offlineProduceCount.ToString()).Append(", ");
	builder.Append("offlineProduceTime").Append(":").Append(offlineProduceTime.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

