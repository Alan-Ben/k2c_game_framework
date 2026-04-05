using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p010_BuildingOp
{

/// <summary>
/// 推送农田建筑建造
/// </summary>
public class GS2GC_010_055_OnFarmBuilt : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 农田建筑数据
/// </summary>
private Common.BuildingObj.Building_Farm farm;


public GS2GC_010_055_OnFarmBuilt() {
	farm = new Common.BuildingObj.Building_Farm();
}

public GS2GC_010_055_OnFarmBuilt(
	Common.BuildingObj.Building_Farm _farm
) {	farm = _farm;
}

public byte getMainOrder() { return (byte)10; }

public byte getSubOrder() { return (byte)55; }

/// <summary>
/// 农田建筑数据
/// </summary>
public Common.BuildingObj.Building_Farm getFarm() { return farm; }
/// <summary>
/// 农田建筑数据
/// </summary>
public void setFarm(Common.BuildingObj.Building_Farm _farm) { farm = _farm; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _farmCustLen = _buf.getInt();
	int _farmCurPos = _buf.getCurPos();
	farm.ReadUnzipBuf(_buf, _farmCurPos + _farmCustLen);
	_buf.setPosition(_farmCurPos + _farmCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(farm.GetBufSize());
	farm.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)55);
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
	builder.Append("farm").Append(":").Append(farm == null ? "null" : farm.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

