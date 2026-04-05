using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星建筑-部件-Food
/// </summary>
public class Mars_Mars_BuildingEquipment_Food : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 建筑ID
/// </summary>
private long buildingId;
/// <summary>
/// 功率开关
/// </summary>
private bool isPowerOn;


public Mars_Mars_BuildingEquipment_Food() {
	buildingId = (long)0;
	isPowerOn = false;
}

public Mars_Mars_BuildingEquipment_Food(
	long _buildingId
	, bool _isPowerOn
) {	buildingId = _buildingId;
	isPowerOn = _isPowerOn;
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
/// 功率开关
/// </summary>
public bool getIsPowerOn() { return isPowerOn; }
/// <summary>
/// 功率开关
/// </summary>
public void setIsPowerOn(bool _isPowerOn) { isPowerOn = _isPowerOn; }


public int GetBufSize() {
	int _size = 9;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isPowerOn = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buildingId);
	_buf.put(isPowerOn?(byte)1:(byte)0);
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
	builder.Append("isPowerOn").Append(":").Append(isPowerOn.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

