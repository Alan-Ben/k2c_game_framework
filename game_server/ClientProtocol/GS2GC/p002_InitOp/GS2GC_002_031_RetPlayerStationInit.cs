using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_031_RetPlayerStationInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 贸易站信息
/// </summary>
private Common.PlayerObj.Player_StationInfo stationInfo;


public GS2GC_002_031_RetPlayerStationInit() {
	stationInfo = new Common.PlayerObj.Player_StationInfo();
}

public GS2GC_002_031_RetPlayerStationInit(
	Common.PlayerObj.Player_StationInfo _stationInfo
) {	stationInfo = _stationInfo;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)31; }

/// <summary>
/// 贸易站信息
/// </summary>
public Common.PlayerObj.Player_StationInfo getStationInfo() { return stationInfo; }
/// <summary>
/// 贸易站信息
/// </summary>
public void setStationInfo(Common.PlayerObj.Player_StationInfo _stationInfo) { stationInfo = _stationInfo; }


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
	_buf.put((byte)2);
	_buf.put((byte)31);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)31);
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

