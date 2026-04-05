package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-信息
 **/
public class TreasureHunt_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 太空舱信息 */
private Common.TreasureHuntObj.TreasureHunt_StationInfo stationInfo;
/** 矿石列表 */
private java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_OreInfo> oreList;
/** 奇物列表 */
private java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TreasureInfo> treasureList;
/** 组合列表 */
private java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CompositeInfo> compositeList;
/** 奇物产出列表 */
private java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo> treasureOutputList;


public TreasureHunt_Info() {
	stationInfo = new Common.TreasureHuntObj.TreasureHunt_StationInfo();
	oreList = new java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_OreInfo>();
	treasureList = new java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TreasureInfo>();
	compositeList = new java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CompositeInfo>();
	treasureOutputList = new java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo>();
}

public TreasureHunt_Info(
	 Common.TreasureHuntObj.TreasureHunt_StationInfo _stationInfo
	, java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_OreInfo> _oreList
	, java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TreasureInfo> _treasureList
	, java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CompositeInfo> _compositeList
	, java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo> _treasureOutputList
) {	stationInfo = _stationInfo;
	oreList = _oreList;
	treasureList = _treasureList;
	compositeList = _compositeList;
	treasureOutputList = _treasureOutputList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 太空舱信息 */
public Common.TreasureHuntObj.TreasureHunt_StationInfo getStationInfo() { return stationInfo; }
/** 太空舱信息 */
public void setStationInfo(Common.TreasureHuntObj.TreasureHunt_StationInfo _stationInfo) { stationInfo = _stationInfo; }
/** 矿石列表 */
public java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_OreInfo> getOreList() { return oreList; }
/** 矿石列表 */
public void addOreList(Common.TreasureHuntObj.TreasureHunt_OreInfo _oreList) { oreList.add(_oreList); }
/** 奇物列表 */
public java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TreasureInfo> getTreasureList() { return treasureList; }
/** 奇物列表 */
public void addTreasureList(Common.TreasureHuntObj.TreasureHunt_TreasureInfo _treasureList) { treasureList.add(_treasureList); }
/** 组合列表 */
public java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_CompositeInfo> getCompositeList() { return compositeList; }
/** 组合列表 */
public void addCompositeList(Common.TreasureHuntObj.TreasureHunt_CompositeInfo _compositeList) { compositeList.add(_compositeList); }
/** 奇物产出列表 */
public java.util.ArrayList<Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo> getTreasureOutputList() { return treasureOutputList; }
/** 奇物产出列表 */
public void addTreasureOutputList(Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo _treasureOutputList) { treasureOutputList.add(_treasureOutputList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2;
	for(int _i = 0; _i < oreList.size(); _i++) {
	_size += 4 + oreList.get(_i).GetBufSize();
	}

	_size += 2 + (treasureList.size() * 24);
	_size += 2 + (compositeList.size() * 22);
	_size += 2 + (treasureOutputList.size() * 24);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
	for(int _i = 0; _i < oreList.size(); _i++) {
	_size += 4 + oreList.get(_i).GetBufSize();
	}

	_size += 2 + (treasureList.size() * 24);
	_size += 2 + (compositeList.size() * 22);
	_size += 2 + (treasureOutputList.size() * 24);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _stationInfoCustLen = _buf.getInt();
	int _stationInfoCurPos = _buf.position();
	stationInfo.ReadUnzipBuf(_buf, _stationInfoCurPos + _stationInfoCustLen);
	_buf.position(_stationInfoCurPos + _stationInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _oreListCount = _buf.getShort();
	for(int _i = 0; _i < _oreListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_OreInfo _oreList = new Common.TreasureHuntObj.TreasureHunt_OreInfo();
		if(_buf.remaining() <= 0) return;
	int __oreListCustLen = _buf.getInt();
	int __oreListCurPos = _buf.position();
	_oreList.ReadUnzipBuf(_buf, __oreListCurPos + __oreListCustLen);
	_buf.position(__oreListCurPos + __oreListCustLen);

		oreList.add(_oreList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _treasureListCount = _buf.getShort();
	for(int _i = 0; _i < _treasureListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_TreasureInfo _treasureList = new Common.TreasureHuntObj.TreasureHunt_TreasureInfo();
		if(_buf.remaining() <= 0) return;
	int __treasureListCustLen = _buf.getInt();
	int __treasureListCurPos = _buf.position();
	_treasureList.ReadUnzipBuf(_buf, __treasureListCurPos + __treasureListCustLen);
	_buf.position(__treasureListCurPos + __treasureListCustLen);

		treasureList.add(_treasureList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _compositeListCount = _buf.getShort();
	for(int _i = 0; _i < _compositeListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_CompositeInfo _compositeList = new Common.TreasureHuntObj.TreasureHunt_CompositeInfo();
		if(_buf.remaining() <= 0) return;
	int __compositeListCustLen = _buf.getInt();
	int __compositeListCurPos = _buf.position();
	_compositeList.ReadUnzipBuf(_buf, __compositeListCurPos + __compositeListCustLen);
	_buf.position(__compositeListCurPos + __compositeListCustLen);

		compositeList.add(_compositeList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _treasureOutputListCount = _buf.getShort();
	for(int _i = 0; _i < _treasureOutputListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo _treasureOutputList = new Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo();
		if(_buf.remaining() <= 0) return;
	int __treasureOutputListCustLen = _buf.getInt();
	int __treasureOutputListCurPos = _buf.position();
	_treasureOutputList.ReadUnzipBuf(_buf, __treasureOutputListCurPos + __treasureOutputListCustLen);
	_buf.position(__treasureOutputListCurPos + __treasureOutputListCustLen);

		treasureOutputList.add(_treasureOutputList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(stationInfo.GetBufSize());
	stationInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)oreList.size());
	for(int _i = 0; _i < oreList.size(); _i++) { 
		_buf.putInt(oreList.get(_i).GetBufSize());
	oreList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)treasureList.size());
	for(int _i = 0; _i < treasureList.size(); _i++) { 
		_buf.putInt(treasureList.get(_i).GetBufSize());
	treasureList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)compositeList.size());
	for(int _i = 0; _i < compositeList.size(); _i++) { 
		_buf.putInt(compositeList.get(_i).GetBufSize());
	compositeList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)treasureOutputList.size());
	for(int _i = 0; _i < treasureOutputList.size(); _i++) { 
		_buf.putInt(treasureOutputList.get(_i).GetBufSize());
	treasureOutputList.get(_i).PutUnzipBuf(_buf);
	}
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

