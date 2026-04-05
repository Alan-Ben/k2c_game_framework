package Common;

import java.nio.ByteBuffer;
public class Common_CustomRoomListItem implements ALBasicProtocolPack._IALProtocolStructure {
private long roomId;
private boolean isNeedPassord;
private int minGrade;
private int matchType;
private int roomState;
private long hostUid;
private String hostName;
private int maxFightNum;
private int curFightNum;
private int maxObNum;
private int curObNum;
private long dungeonId;
private boolean isCanOb;
private boolean isEquit;


public Common_CustomRoomListItem() {
	roomId = (long)0;
	isNeedPassord = false;
	minGrade = 0;
	matchType = 0;
	roomState = 0;
	hostUid = (long)0;
	hostName = "";
	maxFightNum = 0;
	curFightNum = 0;
	maxObNum = 0;
	curObNum = 0;
	dungeonId = (long)0;
	isCanOb = false;
	isEquit = false;
}

public Common_CustomRoomListItem(
	 long _roomId
	, boolean _isNeedPassord
	, int _minGrade
	, int _matchType
	, int _roomState
	, long _hostUid
	, String _hostName
	, int _maxFightNum
	, int _curFightNum
	, int _maxObNum
	, int _curObNum
	, long _dungeonId
	, boolean _isCanOb
	, boolean _isEquit
) {	roomId = _roomId;
	isNeedPassord = _isNeedPassord;
	minGrade = _minGrade;
	matchType = _matchType;
	roomState = _roomState;
	hostUid = _hostUid;
	hostName = _hostName;
	maxFightNum = _maxFightNum;
	curFightNum = _curFightNum;
	maxObNum = _maxObNum;
	curObNum = _curObNum;
	dungeonId = _dungeonId;
	isCanOb = _isCanOb;
	isEquit = _isEquit;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getRoomId() { return roomId; }
public void setRoomId(long _roomId) { roomId = _roomId; }
public boolean getIsNeedPassord() { return isNeedPassord; }
public void setIsNeedPassord(boolean _isNeedPassord) { isNeedPassord = _isNeedPassord; }
public int getMinGrade() { return minGrade; }
public void setMinGrade(int _minGrade) { minGrade = _minGrade; }
public int getMatchType() { return matchType; }
public void setMatchType(int _matchType) { matchType = _matchType; }
public int getRoomState() { return roomState; }
public void setRoomState(int _roomState) { roomState = _roomState; }
public long getHostUid() { return hostUid; }
public void setHostUid(long _hostUid) { hostUid = _hostUid; }
public String getHostName() { return hostName; }
public void setHostName(String _hostName) { hostName = _hostName; }
public int getMaxFightNum() { return maxFightNum; }
public void setMaxFightNum(int _maxFightNum) { maxFightNum = _maxFightNum; }
public int getCurFightNum() { return curFightNum; }
public void setCurFightNum(int _curFightNum) { curFightNum = _curFightNum; }
public int getMaxObNum() { return maxObNum; }
public void setMaxObNum(int _maxObNum) { maxObNum = _maxObNum; }
public int getCurObNum() { return curObNum; }
public void setCurObNum(int _curObNum) { curObNum = _curObNum; }
public long getDungeonId() { return dungeonId; }
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
public boolean getIsCanOb() { return isCanOb; }
public void setIsCanOb(boolean _isCanOb) { isCanOb = _isCanOb; }
public boolean getIsEquit() { return isEquit; }
public void setIsEquit(boolean _isEquit) { isEquit = _isEquit; }


public final int GetBufSize() {
	int _size = 55;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(hostName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 57;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(hostName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isNeedPassord = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) minGrade = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) matchType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomState = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hostUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hostName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxFightNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curFightNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxObNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curObNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isCanOb = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isEquit = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(roomId);
	_buf.put(isNeedPassord?(byte)1:(byte)0);
	_buf.putInt(minGrade);
	_buf.putInt(matchType);
	_buf.putInt(roomState);
	_buf.putLong(hostUid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, hostName);
	_buf.putInt(maxFightNum);
	_buf.putInt(curFightNum);
	_buf.putInt(maxObNum);
	_buf.putInt(curObNum);
	_buf.putLong(dungeonId);
	_buf.put(isCanOb?(byte)1:(byte)0);
	_buf.put(isEquit?(byte)1:(byte)0);
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

