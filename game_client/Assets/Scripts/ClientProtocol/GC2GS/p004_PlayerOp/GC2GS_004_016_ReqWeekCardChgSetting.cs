using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 请求修改周卡的设置
/// </summary>
public class GC2GS_004_016_ReqWeekCardChgSetting : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 周卡设置信息
/// </summary>
private Common.WeekCardObj.WeekCard_SingleSettingInfo settingInfo;


public GC2GS_004_016_ReqWeekCardChgSetting() {
	settingInfo = new Common.WeekCardObj.WeekCard_SingleSettingInfo();
}

public GC2GS_004_016_ReqWeekCardChgSetting(
	Common.WeekCardObj.WeekCard_SingleSettingInfo _settingInfo
) {	settingInfo = _settingInfo;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)16; }

/// <summary>
/// 周卡设置信息
/// </summary>
public Common.WeekCardObj.WeekCard_SingleSettingInfo getSettingInfo() { return settingInfo; }
/// <summary>
/// 周卡设置信息
/// </summary>
public void setSettingInfo(Common.WeekCardObj.WeekCard_SingleSettingInfo _settingInfo) { settingInfo = _settingInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + settingInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + settingInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _settingInfoCustLen = _buf.getInt();
	int _settingInfoCurPos = _buf.getCurPos();
	settingInfo.ReadUnzipBuf(_buf, _settingInfoCurPos + _settingInfoCustLen);
	_buf.setPosition(_settingInfoCurPos + _settingInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(settingInfo.GetBufSize());
	settingInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)16);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)16);
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
	builder.Append("settingInfo").Append(":").Append(settingInfo == null ? "null" : settingInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

