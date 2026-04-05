package Common.InnObj;

import java.nio.ByteBuffer;
/*********
 * 旅店_信息
 **/
public class Inn_Info implements ALBasicProtocolPack._IALProtocolStructure {
private int level;
/** 人气值 */
private long popularity;
/** 奖牌等级 */
private int medalLevel;
/** 接待信息 */
private Common.InnObj.Inn_ReceiveList receiveList;
/** 设施列表 */
private java.util.ArrayList<Common.InnObj.Inn_StationInfo> stationList;
/** 菜品列表 */
private java.util.ArrayList<Common.InnObj.Inn_DishInfo> dishList;
/** 客人列表 */
private java.util.ArrayList<Common.InnObj.Inn_GuestInfo> guestList;
/** 特殊客人列表 */
private java.util.ArrayList<Common.InnObj.Inn_SpecialGuestInfo> specialGuestList;
/** 首次升级时间 ms */
private long firstTimeUpgradeTimeMs;
/** 随机种子 */
private long randomSeed;


public Inn_Info() {
	level = 0;
	popularity = (long)0;
	medalLevel = 0;
	receiveList = new Common.InnObj.Inn_ReceiveList();
	stationList = new java.util.ArrayList<Common.InnObj.Inn_StationInfo>();
	dishList = new java.util.ArrayList<Common.InnObj.Inn_DishInfo>();
	guestList = new java.util.ArrayList<Common.InnObj.Inn_GuestInfo>();
	specialGuestList = new java.util.ArrayList<Common.InnObj.Inn_SpecialGuestInfo>();
	firstTimeUpgradeTimeMs = (long)0;
	randomSeed = (long)0;
}

public Inn_Info(
	 int _level
	, long _popularity
	, int _medalLevel
	, Common.InnObj.Inn_ReceiveList _receiveList
	, java.util.ArrayList<Common.InnObj.Inn_StationInfo> _stationList
	, java.util.ArrayList<Common.InnObj.Inn_DishInfo> _dishList
	, java.util.ArrayList<Common.InnObj.Inn_GuestInfo> _guestList
	, java.util.ArrayList<Common.InnObj.Inn_SpecialGuestInfo> _specialGuestList
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
/** 人气值 */
public long getPopularity() { return popularity; }
/** 人气值 */
public void setPopularity(long _popularity) { popularity = _popularity; }
/** 奖牌等级 */
public int getMedalLevel() { return medalLevel; }
/** 奖牌等级 */
public void setMedalLevel(int _medalLevel) { medalLevel = _medalLevel; }
/** 接待信息 */
public Common.InnObj.Inn_ReceiveList getReceiveList() { return receiveList; }
/** 接待信息 */
public void setReceiveList(Common.InnObj.Inn_ReceiveList _receiveList) { receiveList = _receiveList; }
/** 设施列表 */
public java.util.ArrayList<Common.InnObj.Inn_StationInfo> getStationList() { return stationList; }
/** 设施列表 */
public void addStationList(Common.InnObj.Inn_StationInfo _stationList) { stationList.add(_stationList); }
/** 菜品列表 */
public java.util.ArrayList<Common.InnObj.Inn_DishInfo> getDishList() { return dishList; }
/** 菜品列表 */
public void addDishList(Common.InnObj.Inn_DishInfo _dishList) { dishList.add(_dishList); }
/** 客人列表 */
public java.util.ArrayList<Common.InnObj.Inn_GuestInfo> getGuestList() { return guestList; }
/** 客人列表 */
public void addGuestList(Common.InnObj.Inn_GuestInfo _guestList) { guestList.add(_guestList); }
/** 特殊客人列表 */
public java.util.ArrayList<Common.InnObj.Inn_SpecialGuestInfo> getSpecialGuestList() { return specialGuestList; }
/** 特殊客人列表 */
public void addSpecialGuestList(Common.InnObj.Inn_SpecialGuestInfo _specialGuestList) { specialGuestList.add(_specialGuestList); }
/** 首次升级时间 ms */
public long getFirstTimeUpgradeTimeMs() { return firstTimeUpgradeTimeMs; }
/** 首次升级时间 ms */
public void setFirstTimeUpgradeTimeMs(long _firstTimeUpgradeTimeMs) { firstTimeUpgradeTimeMs = _firstTimeUpgradeTimeMs; }
/** 随机种子 */
public long getRandomSeed() { return randomSeed; }
/** 随机种子 */
public void setRandomSeed(long _randomSeed) { randomSeed = _randomSeed; }


public final int GetBufSize() {
	int _size = 32;
	_size += 4 + receiveList.GetBufSize();
	_size += 2 + (stationList.size() * 16);
	_size += 2 + (dishList.size() * 34);
	_size += 2 + (guestList.size() * 21);
	_size += 2 + (specialGuestList.size() * 14);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += 4 + receiveList.GetBufSize();
	_size += 2 + (stationList.size() * 16);
	_size += 2 + (dishList.size() * 34);
	_size += 2 + (guestList.size() * 21);
	_size += 2 + (specialGuestList.size() * 14);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) popularity = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) medalLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _receiveListCustLen = _buf.getInt();
	int _receiveListCurPos = _buf.position();
	receiveList.ReadUnzipBuf(_buf, _receiveListCurPos + _receiveListCustLen);
	_buf.position(_receiveListCurPos + _receiveListCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _stationListCount = _buf.getShort();
	for(int _i = 0; _i < _stationListCount; _i++) { 
		Common.InnObj.Inn_StationInfo _stationList = new Common.InnObj.Inn_StationInfo();
		if(_buf.remaining() <= 0) return;
	int __stationListCustLen = _buf.getInt();
	int __stationListCurPos = _buf.position();
	_stationList.ReadUnzipBuf(_buf, __stationListCurPos + __stationListCustLen);
	_buf.position(__stationListCurPos + __stationListCustLen);

		stationList.add(_stationList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dishListCount = _buf.getShort();
	for(int _i = 0; _i < _dishListCount; _i++) { 
		Common.InnObj.Inn_DishInfo _dishList = new Common.InnObj.Inn_DishInfo();
		if(_buf.remaining() <= 0) return;
	int __dishListCustLen = _buf.getInt();
	int __dishListCurPos = _buf.position();
	_dishList.ReadUnzipBuf(_buf, __dishListCurPos + __dishListCustLen);
	_buf.position(__dishListCurPos + __dishListCustLen);

		dishList.add(_dishList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _guestListCount = _buf.getShort();
	for(int _i = 0; _i < _guestListCount; _i++) { 
		Common.InnObj.Inn_GuestInfo _guestList = new Common.InnObj.Inn_GuestInfo();
		if(_buf.remaining() <= 0) return;
	int __guestListCustLen = _buf.getInt();
	int __guestListCurPos = _buf.position();
	_guestList.ReadUnzipBuf(_buf, __guestListCurPos + __guestListCustLen);
	_buf.position(__guestListCurPos + __guestListCustLen);

		guestList.add(_guestList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _specialGuestListCount = _buf.getShort();
	for(int _i = 0; _i < _specialGuestListCount; _i++) { 
		Common.InnObj.Inn_SpecialGuestInfo _specialGuestList = new Common.InnObj.Inn_SpecialGuestInfo();
		if(_buf.remaining() <= 0) return;
	int __specialGuestListCustLen = _buf.getInt();
	int __specialGuestListCurPos = _buf.position();
	_specialGuestList.ReadUnzipBuf(_buf, __specialGuestListCurPos + __specialGuestListCustLen);
	_buf.position(__specialGuestListCurPos + __specialGuestListCustLen);

		specialGuestList.add(_specialGuestList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) firstTimeUpgradeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) randomSeed = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(level);
	_buf.putLong(popularity);
	_buf.putInt(medalLevel);
	_buf.putInt(receiveList.GetBufSize());
	receiveList.PutUnzipBuf(_buf);
	_buf.putShort((short)stationList.size());
	for(int _i = 0; _i < stationList.size(); _i++) { 
		_buf.putInt(stationList.get(_i).GetBufSize());
	stationList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)dishList.size());
	for(int _i = 0; _i < dishList.size(); _i++) { 
		_buf.putInt(dishList.get(_i).GetBufSize());
	dishList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)guestList.size());
	for(int _i = 0; _i < guestList.size(); _i++) { 
		_buf.putInt(guestList.get(_i).GetBufSize());
	guestList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)specialGuestList.size());
	for(int _i = 0; _i < specialGuestList.size(); _i++) { 
		_buf.putInt(specialGuestList.get(_i).GetBufSize());
	specialGuestList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(firstTimeUpgradeTimeMs);
	_buf.putLong(randomSeed);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

