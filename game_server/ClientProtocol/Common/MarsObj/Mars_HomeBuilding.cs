using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星建筑-主基地
/// </summary>
public class Mars_HomeBuilding : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 建筑ID
/// </summary>
private long buildingId;
/// <summary>
/// 普通功率开启
/// </summary>
private bool isNormalOn;
/// <summary>
/// 最高功率开启
/// </summary>
private bool isOverdriveOn;
/// <summary>
/// 最后一次收集资源的时间（秒）
/// </summary>
private int lastCollectTimeS;


public Mars_HomeBuilding() {
	buildingId = (long)0;
	isNormalOn = false;
	isOverdriveOn = false;
	lastCollectTimeS = 0;
}

public Mars_HomeBuilding(
	long _buildingId
	, bool _isNormalOn
	, bool _isOverdriveOn
	, int _lastCollectTimeS
) {	buildingId = _buildingId;
	isNormalOn = _isNormalOn;
	isOverdriveOn = _isOverdriveOn;
	lastCollectTimeS = _lastCollectTimeS;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 建筑ID
/// </summary>
public long getBuildingId() { return buildingId; }
/// <summary>
/// 建筑ID
/// </summary>
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/// <summary>
/// 普通功率开启
/// </summary>
public bool getIsNormalOn() { return isNormalOn; }
/// <summary>
/// 普通功率开启
/// </summary>
public void setIsNormalOn(bool _isNormalOn) { isNormalOn = _isNormalOn; }
/// <summary>
/// 最高功率开启
/// </summary>
public bool getIsOverdriveOn() { return isOverdriveOn; }
/// <summary>
/// 最高功率开启
/// </summary>
public void setIsOverdriveOn(bool _isOverdriveOn) { isOverdriveOn = _isOverdriveOn; }
/// <summary>
/// 最后一次收集资源的时间（秒）
/// </summary>
public int getLastCollectTimeS() { return lastCollectTimeS; }
/// <summary>
/// 最后一次收集资源的时间（秒）
/// </summary>
public void setLastCollectTimeS(int _lastCollectTimeS) { lastCollectTimeS = _lastCollectTimeS; }


public int GetBufSize() {
	int _size = 14;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 16;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isNormalOn = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isOverdriveOn = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastCollectTimeS = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buildingId);
	_buf.put(isNormalOn?(byte)1:(byte)0);
	_buf.put(isOverdriveOn?(byte)1:(byte)0);
	_buf.putInt(lastCollectTimeS);
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
	builder.Append("buildingId").Append(":").Append(buildingId.ToString()).Append(", ");
	builder.Append("isNormalOn").Append(":").Append(isNormalOn.ToString()).Append(", ");
	builder.Append("isOverdriveOn").Append(":").Append(isOverdriveOn.ToString()).Append(", ");
	builder.Append("lastCollectTimeS").Append(":").Append(lastCollectTimeS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

