using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class TeamInfoSetting : ALBasicProtocolPack._IALProtocolStructure {
private long selectRace;
private List<Common.TeamInfoSettingRaceInfo> raceInfoList;


public TeamInfoSetting() {
	selectRace = (long)0;
	raceInfoList = new List<Common.TeamInfoSettingRaceInfo>();
}

public TeamInfoSetting(
	long _selectRace
	, List<Common.TeamInfoSettingRaceInfo> _raceInfoList
) {	selectRace = _selectRace;
	raceInfoList = _raceInfoList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getSelectRace() { return selectRace; }
public void setSelectRace(long _selectRace) { selectRace = _selectRace; }
public List<Common.TeamInfoSettingRaceInfo> getRaceInfoList() { return raceInfoList; }
public void addRaceInfoList(Common.TeamInfoSettingRaceInfo _raceInfoList) { raceInfoList.Add(_raceInfoList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2;
for(int _i = 0; _i < raceInfoList.Count; _i++) {
	_size += 4 + raceInfoList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
for(int _i = 0; _i < raceInfoList.Count; _i++) {
	_size += 4 + raceInfoList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	selectRace = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _raceInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _raceInfoListCount; _i++) { 
		Common.TeamInfoSettingRaceInfo _raceInfoList = new Common.TeamInfoSettingRaceInfo();
		int __raceInfoListCustLen = _buf.getInt();
	int __raceInfoListCurPos = _buf.getCurPos();
	_raceInfoList.ReadUnzipBuf(_buf, __raceInfoListCurPos + __raceInfoListCustLen);
	_buf.setPosition(__raceInfoListCurPos + __raceInfoListCustLen);

		raceInfoList.Add(_raceInfoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(selectRace);
	_buf.putShort((short)raceInfoList.Count);
	for(int _i = 0; _i < raceInfoList.Count; _i++) { 
		_buf.putInt(raceInfoList[_i].GetBufSize());
	raceInfoList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("selectRace").Append(":").Append(selectRace.ToString()).Append(", ");
	builder.Append("raceInfoList").Append(":").Append(raceInfoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

