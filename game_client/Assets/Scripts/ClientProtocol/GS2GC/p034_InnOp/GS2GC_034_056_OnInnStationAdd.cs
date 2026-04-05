using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p034_InnOp
{

/// <summary>
/// 旅店设施信息新增
/// </summary>
public class GS2GC_034_056_OnInnStationAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 设施信息
/// </summary>
private Common.InnObj.Inn_StationInfo stationInfo;


public GS2GC_034_056_OnInnStationAdd() {
	stationInfo = new Common.InnObj.Inn_StationInfo();
}

public GS2GC_034_056_OnInnStationAdd(
	Common.InnObj.Inn_StationInfo _stationInfo
) {	stationInfo = _stationInfo;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)56; }

/// <summary>
/// 设施信息
/// </summary>
public Common.InnObj.Inn_StationInfo getStationInfo() { return stationInfo; }
/// <summary>
/// 设施信息
/// </summary>
public void setStationInfo(Common.InnObj.Inn_StationInfo _stationInfo) { stationInfo = _stationInfo; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _stationInfoCustLen = _buf.getInt();
	int _stationInfoCurPos = _buf.getCurPos();
	stationInfo.ReadUnzipBuf(_buf, _stationInfoCurPos + _stationInfoCustLen);
	_buf.setPosition(_stationInfoCurPos + _stationInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(stationInfo.GetBufSize());
	stationInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)56);
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
	builder.Append("stationInfo").Append(":").Append(stationInfo == null ? "null" : stationInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

