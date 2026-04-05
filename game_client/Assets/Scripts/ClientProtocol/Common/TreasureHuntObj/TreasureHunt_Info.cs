using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-信息
/// </summary>
public class TreasureHunt_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 太空舱信息
/// </summary>
private Common.TreasureHuntObj.TreasureHunt_StationInfo stationInfo;
/// <summary>
/// 矿石列表
/// </summary>
private List<Common.TreasureHuntObj.TreasureHunt_OreInfo> oreList;
/// <summary>
/// 奇物列表
/// </summary>
private List<Common.TreasureHuntObj.TreasureHunt_TreasureInfo> treasureList;
/// <summary>
/// 组合列表
/// </summary>
private List<Common.TreasureHuntObj.TreasureHunt_CompositeInfo> compositeList;
/// <summary>
/// 奇物产出列表
/// </summary>
private List<Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo> treasureOutputList;


public TreasureHunt_Info() {
	stationInfo = new Common.TreasureHuntObj.TreasureHunt_StationInfo();
	oreList = new List<Common.TreasureHuntObj.TreasureHunt_OreInfo>();
	treasureList = new List<Common.TreasureHuntObj.TreasureHunt_TreasureInfo>();
	compositeList = new List<Common.TreasureHuntObj.TreasureHunt_CompositeInfo>();
	treasureOutputList = new List<Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo>();
}

public TreasureHunt_Info(
	Common.TreasureHuntObj.TreasureHunt_StationInfo _stationInfo
	, List<Common.TreasureHuntObj.TreasureHunt_OreInfo> _oreList
	, List<Common.TreasureHuntObj.TreasureHunt_TreasureInfo> _treasureList
	, List<Common.TreasureHuntObj.TreasureHunt_CompositeInfo> _compositeList
	, List<Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo> _treasureOutputList
) {	stationInfo = _stationInfo;
	oreList = _oreList;
	treasureList = _treasureList;
	compositeList = _compositeList;
	treasureOutputList = _treasureOutputList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 太空舱信息
/// </summary>
public Common.TreasureHuntObj.TreasureHunt_StationInfo getStationInfo() { return stationInfo; }
/// <summary>
/// 太空舱信息
/// </summary>
public void setStationInfo(Common.TreasureHuntObj.TreasureHunt_StationInfo _stationInfo) { stationInfo = _stationInfo; }
/// <summary>
/// 矿石列表
/// </summary>
public List<Common.TreasureHuntObj.TreasureHunt_OreInfo> getOreList() { return oreList; }
/// <summary>
/// 矿石列表
/// </summary>
public void addOreList(Common.TreasureHuntObj.TreasureHunt_OreInfo _oreList) { oreList.Add(_oreList); }
/// <summary>
/// 奇物列表
/// </summary>
public List<Common.TreasureHuntObj.TreasureHunt_TreasureInfo> getTreasureList() { return treasureList; }
/// <summary>
/// 奇物列表
/// </summary>
public void addTreasureList(Common.TreasureHuntObj.TreasureHunt_TreasureInfo _treasureList) { treasureList.Add(_treasureList); }
/// <summary>
/// 组合列表
/// </summary>
public List<Common.TreasureHuntObj.TreasureHunt_CompositeInfo> getCompositeList() { return compositeList; }
/// <summary>
/// 组合列表
/// </summary>
public void addCompositeList(Common.TreasureHuntObj.TreasureHunt_CompositeInfo _compositeList) { compositeList.Add(_compositeList); }
/// <summary>
/// 奇物产出列表
/// </summary>
public List<Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo> getTreasureOutputList() { return treasureOutputList; }
/// <summary>
/// 奇物产出列表
/// </summary>
public void addTreasureOutputList(Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo _treasureOutputList) { treasureOutputList.Add(_treasureOutputList); }


public int GetBufSize() {
	int _size = 16;
	_size += 2;
for(int _i = 0; _i < oreList.Count; _i++) {
	_size += 4 + oreList[_i].GetBufSize();
	}

	_size += 2 + (treasureList.Count * 24);
	_size += 2 + (compositeList.Count * 22);
	_size += 2 + (treasureOutputList.Count * 24);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
for(int _i = 0; _i < oreList.Count; _i++) {
	_size += 4 + oreList[_i].GetBufSize();
	}

	_size += 2 + (treasureList.Count * 24);
	_size += 2 + (compositeList.Count * 22);
	_size += 2 + (treasureOutputList.Count * 24);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _stationInfoCustLen = _buf.getInt();
	int _stationInfoCurPos = _buf.getCurPos();
	stationInfo.ReadUnzipBuf(_buf, _stationInfoCurPos + _stationInfoCustLen);
	_buf.setPosition(_stationInfoCurPos + _stationInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _oreListCount = _buf.getShort();
	for(int _i = 0; _i < _oreListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_OreInfo _oreList = new Common.TreasureHuntObj.TreasureHunt_OreInfo();
		int __oreListCustLen = _buf.getInt();
	int __oreListCurPos = _buf.getCurPos();
	_oreList.ReadUnzipBuf(_buf, __oreListCurPos + __oreListCustLen);
	_buf.setPosition(__oreListCurPos + __oreListCustLen);

		oreList.Add(_oreList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _treasureListCount = _buf.getShort();
	for(int _i = 0; _i < _treasureListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_TreasureInfo _treasureList = new Common.TreasureHuntObj.TreasureHunt_TreasureInfo();
		int __treasureListCustLen = _buf.getInt();
	int __treasureListCurPos = _buf.getCurPos();
	_treasureList.ReadUnzipBuf(_buf, __treasureListCurPos + __treasureListCustLen);
	_buf.setPosition(__treasureListCurPos + __treasureListCustLen);

		treasureList.Add(_treasureList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _compositeListCount = _buf.getShort();
	for(int _i = 0; _i < _compositeListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_CompositeInfo _compositeList = new Common.TreasureHuntObj.TreasureHunt_CompositeInfo();
		int __compositeListCustLen = _buf.getInt();
	int __compositeListCurPos = _buf.getCurPos();
	_compositeList.ReadUnzipBuf(_buf, __compositeListCurPos + __compositeListCustLen);
	_buf.setPosition(__compositeListCurPos + __compositeListCustLen);

		compositeList.Add(_compositeList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _treasureOutputListCount = _buf.getShort();
	for(int _i = 0; _i < _treasureOutputListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo _treasureOutputList = new Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo();
		int __treasureOutputListCustLen = _buf.getInt();
	int __treasureOutputListCurPos = _buf.getCurPos();
	_treasureOutputList.ReadUnzipBuf(_buf, __treasureOutputListCurPos + __treasureOutputListCustLen);
	_buf.setPosition(__treasureOutputListCurPos + __treasureOutputListCustLen);

		treasureOutputList.Add(_treasureOutputList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(stationInfo.GetBufSize());
	stationInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)oreList.Count);
	for(int _i = 0; _i < oreList.Count; _i++) { 
		_buf.putInt(oreList[_i].GetBufSize());
	oreList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)treasureList.Count);
	for(int _i = 0; _i < treasureList.Count; _i++) { 
		_buf.putInt(treasureList[_i].GetBufSize());
	treasureList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)compositeList.Count);
	for(int _i = 0; _i < compositeList.Count; _i++) { 
		_buf.putInt(compositeList[_i].GetBufSize());
	compositeList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)treasureOutputList.Count);
	for(int _i = 0; _i < treasureOutputList.Count; _i++) { 
		_buf.putInt(treasureOutputList[_i].GetBufSize());
	treasureOutputList[_i].PutUnzipBuf(_buf);
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
	builder.Append("stationInfo").Append(":").Append(stationInfo == null ? "null" : stationInfo.ToString()).Append(", ");
	builder.Append("oreList").Append(":").Append(oreList.ToString()).Append(", ");
	builder.Append("treasureList").Append(":").Append(treasureList.ToString()).Append(", ");
	builder.Append("compositeList").Append(":").Append(compositeList.ToString()).Append(", ");
	builder.Append("treasureOutputList").Append(":").Append(treasureOutputList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

