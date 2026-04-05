using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.InnObj
{

/// <summary>
/// 旅店_信息
/// </summary>
public class Inn_Info : ALBasicProtocolPack._IALProtocolStructure {
private int level;
/// <summary>
/// 人气值
/// </summary>
private long popularity;
/// <summary>
/// 奖牌等级
/// </summary>
private int medalLevel;
/// <summary>
/// 接待信息
/// </summary>
private Common.InnObj.Inn_ReceiveList receiveList;
/// <summary>
/// 设施列表
/// </summary>
private List<Common.InnObj.Inn_StationInfo> stationList;
/// <summary>
/// 菜品列表
/// </summary>
private List<Common.InnObj.Inn_DishInfo> dishList;
/// <summary>
/// 客人列表
/// </summary>
private List<Common.InnObj.Inn_GuestInfo> guestList;
/// <summary>
/// 特殊客人列表
/// </summary>
private List<Common.InnObj.Inn_SpecialGuestInfo> specialGuestList;
/// <summary>
/// 首次升级时间 ms
/// </summary>
private long firstTimeUpgradeTimeMs;
/// <summary>
/// 随机种子
/// </summary>
private long randomSeed;


public Inn_Info() {
	level = 0;
	popularity = (long)0;
	medalLevel = 0;
	receiveList = new Common.InnObj.Inn_ReceiveList();
	stationList = new List<Common.InnObj.Inn_StationInfo>();
	dishList = new List<Common.InnObj.Inn_DishInfo>();
	guestList = new List<Common.InnObj.Inn_GuestInfo>();
	specialGuestList = new List<Common.InnObj.Inn_SpecialGuestInfo>();
	firstTimeUpgradeTimeMs = (long)0;
	randomSeed = (long)0;
}

public Inn_Info(
	int _level
	, long _popularity
	, int _medalLevel
	, Common.InnObj.Inn_ReceiveList _receiveList
	, List<Common.InnObj.Inn_StationInfo> _stationList
	, List<Common.InnObj.Inn_DishInfo> _dishList
	, List<Common.InnObj.Inn_GuestInfo> _guestList
	, List<Common.InnObj.Inn_SpecialGuestInfo> _specialGuestList
	, long _firstTimeUpgradeTimeMs
	, long _randomSeed
) {	level = _level;
	popularity = _popularity;
	medalLevel = _medalLevel;
	receiveList = _receiveList;
	stationList = _stationList;
	dishList = _dishList;
	guestList = _guestList;
	specialGuestList = _specialGuestList;
	firstTimeUpgradeTimeMs = _firstTimeUpgradeTimeMs;
	randomSeed = _randomSeed;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
/// <summary>
/// 人气值
/// </summary>
public long getPopularity() { return popularity; }
/// <summary>
/// 人气值
/// </summary>
public void setPopularity(long _popularity) { popularity = _popularity; }
/// <summary>
/// 奖牌等级
/// </summary>
public int getMedalLevel() { return medalLevel; }
/// <summary>
/// 奖牌等级
/// </summary>
public void setMedalLevel(int _medalLevel) { medalLevel = _medalLevel; }
/// <summary>
/// 接待信息
/// </summary>
public Common.InnObj.Inn_ReceiveList getReceiveList() { return receiveList; }
/// <summary>
/// 接待信息
/// </summary>
public void setReceiveList(Common.InnObj.Inn_ReceiveList _receiveList) { receiveList = _receiveList; }
/// <summary>
/// 设施列表
/// </summary>
public List<Common.InnObj.Inn_StationInfo> getStationList() { return stationList; }
/// <summary>
/// 设施列表
/// </summary>
public void addStationList(Common.InnObj.Inn_StationInfo _stationList) { stationList.Add(_stationList); }
/// <summary>
/// 菜品列表
/// </summary>
public List<Common.InnObj.Inn_DishInfo> getDishList() { return dishList; }
/// <summary>
/// 菜品列表
/// </summary>
public void addDishList(Common.InnObj.Inn_DishInfo _dishList) { dishList.Add(_dishList); }
/// <summary>
/// 客人列表
/// </summary>
public List<Common.InnObj.Inn_GuestInfo> getGuestList() { return guestList; }
/// <summary>
/// 客人列表
/// </summary>
public void addGuestList(Common.InnObj.Inn_GuestInfo _guestList) { guestList.Add(_guestList); }
/// <summary>
/// 特殊客人列表
/// </summary>
public List<Common.InnObj.Inn_SpecialGuestInfo> getSpecialGuestList() { return specialGuestList; }
/// <summary>
/// 特殊客人列表
/// </summary>
public void addSpecialGuestList(Common.InnObj.Inn_SpecialGuestInfo _specialGuestList) { specialGuestList.Add(_specialGuestList); }
/// <summary>
/// 首次升级时间 ms
/// </summary>
public long getFirstTimeUpgradeTimeMs() { return firstTimeUpgradeTimeMs; }
/// <summary>
/// 首次升级时间 ms
/// </summary>
public void setFirstTimeUpgradeTimeMs(long _firstTimeUpgradeTimeMs) { firstTimeUpgradeTimeMs = _firstTimeUpgradeTimeMs; }
/// <summary>
/// 随机种子
/// </summary>
public long getRandomSeed() { return randomSeed; }
/// <summary>
/// 随机种子
/// </summary>
public void setRandomSeed(long _randomSeed) { randomSeed = _randomSeed; }


public int GetBufSize() {
	int _size = 32;
	_size += 4 + receiveList.GetBufSize();
	_size += 2 + (stationList.Count * 16);
	_size += 2 + (dishList.Count * 34);
	_size += 2 + (guestList.Count * 21);
	_size += 2 + (specialGuestList.Count * 14);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;
	_size += 4 + receiveList.GetBufSize();
	_size += 2 + (stationList.Count * 16);
	_size += 2 + (dishList.Count * 34);
	_size += 2 + (guestList.Count * 21);
	_size += 2 + (specialGuestList.Count * 14);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	popularity = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	medalLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _receiveListCustLen = _buf.getInt();
	int _receiveListCurPos = _buf.getCurPos();
	receiveList.ReadUnzipBuf(_buf, _receiveListCurPos + _receiveListCustLen);
	_buf.setPosition(_receiveListCurPos + _receiveListCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _stationListCount = _buf.getShort();
	for(int _i = 0; _i < _stationListCount; _i++) { 
		Common.InnObj.Inn_StationInfo _stationList = new Common.InnObj.Inn_StationInfo();
		int __stationListCustLen = _buf.getInt();
	int __stationListCurPos = _buf.getCurPos();
	_stationList.ReadUnzipBuf(_buf, __stationListCurPos + __stationListCustLen);
	_buf.setPosition(__stationListCurPos + __stationListCustLen);

		stationList.Add(_stationList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dishListCount = _buf.getShort();
	for(int _i = 0; _i < _dishListCount; _i++) { 
		Common.InnObj.Inn_DishInfo _dishList = new Common.InnObj.Inn_DishInfo();
		int __dishListCustLen = _buf.getInt();
	int __dishListCurPos = _buf.getCurPos();
	_dishList.ReadUnzipBuf(_buf, __dishListCurPos + __dishListCustLen);
	_buf.setPosition(__dishListCurPos + __dishListCustLen);

		dishList.Add(_dishList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _guestListCount = _buf.getShort();
	for(int _i = 0; _i < _guestListCount; _i++) { 
		Common.InnObj.Inn_GuestInfo _guestList = new Common.InnObj.Inn_GuestInfo();
		int __guestListCustLen = _buf.getInt();
	int __guestListCurPos = _buf.getCurPos();
	_guestList.ReadUnzipBuf(_buf, __guestListCurPos + __guestListCustLen);
	_buf.setPosition(__guestListCurPos + __guestListCustLen);

		guestList.Add(_guestList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _specialGuestListCount = _buf.getShort();
	for(int _i = 0; _i < _specialGuestListCount; _i++) { 
		Common.InnObj.Inn_SpecialGuestInfo _specialGuestList = new Common.InnObj.Inn_SpecialGuestInfo();
		int __specialGuestListCustLen = _buf.getInt();
	int __specialGuestListCurPos = _buf.getCurPos();
	_specialGuestList.ReadUnzipBuf(_buf, __specialGuestListCurPos + __specialGuestListCustLen);
	_buf.setPosition(__specialGuestListCurPos + __specialGuestListCustLen);

		specialGuestList.Add(_specialGuestList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	firstTimeUpgradeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	randomSeed = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(level);
	_buf.putLong(popularity);
	_buf.putInt(medalLevel);
	_buf.putInt(receiveList.GetBufSize());
	receiveList.PutUnzipBuf(_buf);
	_buf.putShort((short)stationList.Count);
	for(int _i = 0; _i < stationList.Count; _i++) { 
		_buf.putInt(stationList[_i].GetBufSize());
	stationList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)dishList.Count);
	for(int _i = 0; _i < dishList.Count; _i++) { 
		_buf.putInt(dishList[_i].GetBufSize());
	dishList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)guestList.Count);
	for(int _i = 0; _i < guestList.Count; _i++) { 
		_buf.putInt(guestList[_i].GetBufSize());
	guestList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)specialGuestList.Count);
	for(int _i = 0; _i < specialGuestList.Count; _i++) { 
		_buf.putInt(specialGuestList[_i].GetBufSize());
	specialGuestList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(firstTimeUpgradeTimeMs);
	_buf.putLong(randomSeed);
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
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("popularity").Append(":").Append(popularity.ToString()).Append(", ");
	builder.Append("medalLevel").Append(":").Append(medalLevel.ToString()).Append(", ");
	builder.Append("receiveList").Append(":").Append(receiveList == null ? "null" : receiveList.ToString()).Append(", ");
	builder.Append("stationList").Append(":").Append(stationList.ToString()).Append(", ");
	builder.Append("dishList").Append(":").Append(dishList.ToString()).Append(", ");
	builder.Append("guestList").Append(":").Append(guestList.ToString()).Append(", ");
	builder.Append("specialGuestList").Append(":").Append(specialGuestList.ToString()).Append(", ");
	builder.Append("firstTimeUpgradeTimeMs").Append(":").Append(firstTimeUpgradeTimeMs.ToString()).Append(", ");
	builder.Append("randomSeed").Append(":").Append(randomSeed.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

