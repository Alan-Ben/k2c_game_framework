package NP2PS_RB.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2PS_RB_001_001_RetCSRegInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 已有房间服务器的信息列表 */
private java.util.ArrayList<Common.Common_RSInfo> rsList;
/** 已注册的CrossGame服务器列表 */
private java.util.ArrayList<Integer> crossGameServerTypeIdList;
/** 已注册的CrossRank服务器列表 */
private java.util.ArrayList<Integer> crossRankServerTypeIdList;
/** 已注册的GameLogic服务器列表 */
private java.util.ArrayList<Integer> gameLogicServerTypeIdList;


public NP2PS_RB_001_001_RetCSRegInfo() {
	rsList = new java.util.ArrayList<Common.Common_RSInfo>();
	crossGameServerTypeIdList = new java.util.ArrayList<Integer>();
	crossRankServerTypeIdList = new java.util.ArrayList<Integer>();
	gameLogicServerTypeIdList = new java.util.ArrayList<Integer>();
}

public NP2PS_RB_001_001_RetCSRegInfo(
	 java.util.ArrayList<Common.Common_RSInfo> _rsList
	, java.util.ArrayList<Integer> _crossGameServerTypeIdList
	, java.util.ArrayList<Integer> _crossRankServerTypeIdList
	, java.util.ArrayList<Integer> _gameLogicServerTypeIdList
) {	rsList = _rsList;
	crossGameServerTypeIdList = _crossGameServerTypeIdList;
	crossRankServerTypeIdList = _crossRankServerTypeIdList;
	gameLogicServerTypeIdList = _gameLogicServerTypeIdList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

/** 已有房间服务器的信息列表 */
public java.util.ArrayList<Common.Common_RSInfo> getRsList() { return rsList; }
/** 已有房间服务器的信息列表 */
public void addRsList(Common.Common_RSInfo _rsList) { rsList.add(_rsList); }
/** 已注册的CrossGame服务器列表 */
public java.util.ArrayList<Integer> getCrossGameServerTypeIdList() { return crossGameServerTypeIdList; }
/** 已注册的CrossGame服务器列表 */
public void addCrossGameServerTypeIdList(int _crossGameServerTypeIdList) { crossGameServerTypeIdList.add(_crossGameServerTypeIdList); }
/** 已注册的CrossRank服务器列表 */
public java.util.ArrayList<Integer> getCrossRankServerTypeIdList() { return crossRankServerTypeIdList; }
/** 已注册的CrossRank服务器列表 */
public void addCrossRankServerTypeIdList(int _crossRankServerTypeIdList) { crossRankServerTypeIdList.add(_crossRankServerTypeIdList); }
/** 已注册的GameLogic服务器列表 */
public java.util.ArrayList<Integer> getGameLogicServerTypeIdList() { return gameLogicServerTypeIdList; }
/** 已注册的GameLogic服务器列表 */
public void addGameLogicServerTypeIdList(int _gameLogicServerTypeIdList) { gameLogicServerTypeIdList.add(_gameLogicServerTypeIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (rsList.size() * 8);
	_size += 2 + (crossGameServerTypeIdList.size() * 4);
	_size += 2 + (crossRankServerTypeIdList.size() * 4);
	_size += 2 + (gameLogicServerTypeIdList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (rsList.size() * 8);
	_size += 2 + (crossGameServerTypeIdList.size() * 4);
	_size += 2 + (crossRankServerTypeIdList.size() * 4);
	_size += 2 + (gameLogicServerTypeIdList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rsListCount = _buf.getShort();
	for(int _i = 0; _i < _rsListCount; _i++) { 
		Common.Common_RSInfo _rsList = new Common.Common_RSInfo();
		if(_buf.remaining() <= 0) return;
	int __rsListCustLen = _buf.getInt();
	int __rsListCurPos = _buf.position();
	_rsList.ReadUnzipBuf(_buf, __rsListCurPos + __rsListCustLen);
	_buf.position(__rsListCurPos + __rsListCustLen);

		rsList.add(_rsList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _crossGameServerTypeIdListCount = _buf.getShort();
	for(int _i = 0; _i < _crossGameServerTypeIdListCount; _i++) { 
		int _crossGameServerTypeIdList = 0;
		if(_buf.remaining() > 0) _crossGameServerTypeIdList = _buf.getInt();
		crossGameServerTypeIdList.add(_crossGameServerTypeIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _crossRankServerTypeIdListCount = _buf.getShort();
	for(int _i = 0; _i < _crossRankServerTypeIdListCount; _i++) { 
		int _crossRankServerTypeIdList = 0;
		if(_buf.remaining() > 0) _crossRankServerTypeIdList = _buf.getInt();
		crossRankServerTypeIdList.add(_crossRankServerTypeIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _gameLogicServerTypeIdListCount = _buf.getShort();
	for(int _i = 0; _i < _gameLogicServerTypeIdListCount; _i++) { 
		int _gameLogicServerTypeIdList = 0;
		if(_buf.remaining() > 0) _gameLogicServerTypeIdList = _buf.getInt();
		gameLogicServerTypeIdList.add(_gameLogicServerTypeIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)rsList.size());
	for(int _i = 0; _i < rsList.size(); _i++) { 
		_buf.putInt(rsList.get(_i).GetBufSize());
	rsList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)crossGameServerTypeIdList.size());
	for(int _i = 0; _i < crossGameServerTypeIdList.size(); _i++) { 
		_buf.putInt(crossGameServerTypeIdList.get(_i));
	}
	_buf.putShort((short)crossRankServerTypeIdList.size());
	for(int _i = 0; _i < crossRankServerTypeIdList.size(); _i++) { 
		_buf.putInt(crossRankServerTypeIdList.get(_i));
	}
	_buf.putShort((short)gameLogicServerTypeIdList.size());
	for(int _i = 0; _i < gameLogicServerTypeIdList.size(); _i++) { 
		_buf.putInt(gameLogicServerTypeIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)1);
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

