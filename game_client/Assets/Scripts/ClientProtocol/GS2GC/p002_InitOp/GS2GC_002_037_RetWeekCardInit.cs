using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_037_RetWeekCardInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 周卡信息
/// </summary>
private Common.WeekCardObj.WeekCard_Info info;
/// <summary>
/// 设置信息列表
/// </summary>
private List<Common.WeekCardObj.WeekCard_SingleSettingInfo> settingList;


public GS2GC_002_037_RetWeekCardInit() {
	info = new Common.WeekCardObj.WeekCard_Info();
	settingList = new List<Common.WeekCardObj.WeekCard_SingleSettingInfo>();
}

public GS2GC_002_037_RetWeekCardInit(
	Common.WeekCardObj.WeekCard_Info _info
	, List<Common.WeekCardObj.WeekCard_SingleSettingInfo> _settingList
) {	info = _info;
	settingList = _settingList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)37; }

/// <summary>
/// 周卡信息
/// </summary>
public Common.WeekCardObj.WeekCard_Info getInfo() { return info; }
/// <summary>
/// 周卡信息
/// </summary>
public void setInfo(Common.WeekCardObj.WeekCard_Info _info) { info = _info; }
/// <summary>
/// 设置信息列表
/// </summary>
public List<Common.WeekCardObj.WeekCard_SingleSettingInfo> getSettingList() { return settingList; }
/// <summary>
/// 设置信息列表
/// </summary>
public void addSettingList(Common.WeekCardObj.WeekCard_SingleSettingInfo _settingList) { settingList.Add(_settingList); }


public int GetBufSize() {
	int _size = 21;
	_size += 2;
for(int _i = 0; _i < settingList.Count; _i++) {
	_size += 4 + settingList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 23;
	_size += 2;
for(int _i = 0; _i < settingList.Count; _i++) {
	_size += 4 + settingList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.getCurPos();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.setPosition(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _settingListCount = _buf.getShort();
	for(int _i = 0; _i < _settingListCount; _i++) { 
		Common.WeekCardObj.WeekCard_SingleSettingInfo _settingList = new Common.WeekCardObj.WeekCard_SingleSettingInfo();
		int __settingListCustLen = _buf.getInt();
	int __settingListCurPos = _buf.getCurPos();
	_settingList.ReadUnzipBuf(_buf, __settingListCurPos + __settingListCustLen);
	_buf.setPosition(__settingListCurPos + __settingListCustLen);

		settingList.Add(_settingList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putShort((short)settingList.Count);
	for(int _i = 0; _i < settingList.Count; _i++) { 
		_buf.putInt(settingList[_i].GetBufSize());
	settingList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)37);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)37);
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
	builder.Append("info").Append(":").Append(info == null ? "null" : info.ToString()).Append(", ");
	builder.Append("settingList").Append(":").Append(settingList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

