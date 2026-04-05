using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p010_BuildingOp
{

/// <summary>
/// 农田暴击信息重置
/// </summary>
public class GS2GC_010_058_OnFarmMultipleInfoReset : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 农田建筑暴击信息
/// </summary>
private Common.BuildingObj.Building_FarmMultipleInfo farmMultipleInfo;


public GS2GC_010_058_OnFarmMultipleInfoReset() {
	farmMultipleInfo = new Common.BuildingObj.Building_FarmMultipleInfo();
}

public GS2GC_010_058_OnFarmMultipleInfoReset(
	Common.BuildingObj.Building_FarmMultipleInfo _farmMultipleInfo
) {	farmMultipleInfo = _farmMultipleInfo;
}

public byte getMainOrder() { return (byte)10; }

public byte getSubOrder() { return (byte)58; }

/// <summary>
/// 农田建筑暴击信息
/// </summary>
public Common.BuildingObj.Building_FarmMultipleInfo getFarmMultipleInfo() { return farmMultipleInfo; }
/// <summary>
/// 农田建筑暴击信息
/// </summary>
public void setFarmMultipleInfo(Common.BuildingObj.Building_FarmMultipleInfo _farmMultipleInfo) { farmMultipleInfo = _farmMultipleInfo; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _farmMultipleInfoCustLen = _buf.getInt();
	int _farmMultipleInfoCurPos = _buf.getCurPos();
	farmMultipleInfo.ReadUnzipBuf(_buf, _farmMultipleInfoCurPos + _farmMultipleInfoCustLen);
	_buf.setPosition(_farmMultipleInfoCurPos + _farmMultipleInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(farmMultipleInfo.GetBufSize());
	farmMultipleInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
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
	builder.Append("farmMultipleInfo").Append(":").Append(farmMultipleInfo == null ? "null" : farmMultipleInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

