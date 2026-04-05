using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p039_MarsBuildingOp
{

/// <summary>
/// 火星建筑-基地开关
/// </summary>
public class GC2GS_039_013_ReqFoodBuildingPowerChg : ALBasicProtocolPack._IALProtocolStructure {
private long buildingId;
private bool powerOn;


public GC2GS_039_013_ReqFoodBuildingPowerChg() {
	buildingId = (long)0;
	powerOn = false;
}

public GC2GS_039_013_ReqFoodBuildingPowerChg(
	long _buildingId
	, bool _powerOn
) {	buildingId = _buildingId;
	powerOn = _powerOn;
}

public byte getMainOrder() { return (byte)39; }

public byte getSubOrder() { return (byte)13; }

public long getBuildingId() { return buildingId; }
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
public bool getPowerOn() { return powerOn; }
public void setPowerOn(bool _powerOn) { powerOn = _powerOn; }


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
	powerOn = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buildingId);
	_buf.put(powerOn?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)13);
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
	builder.Append("powerOn").Append(":").Append(powerOn.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

