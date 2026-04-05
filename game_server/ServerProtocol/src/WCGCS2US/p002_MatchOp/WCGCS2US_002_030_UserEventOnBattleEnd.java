package WCGCS2US.p002_MatchOp;

import java.nio.ByteBuffer;
public class WCGCS2US_002_030_UserEventOnBattleEnd implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private int userLvl;
private long roomSerial;
private int roomSid;
private int roomType;
private long startTimeMs;
private long endTimeMs;
private boolean isWin;
private boolean isDraw;
private int battleScore;


public WCGCS2US_002_030_UserEventOnBattleEnd() {
	uid = (long)0;
	userLvl = 0;
	roomSerial = (long)0;
	roomSid = 0;
	roomType = 0;
	startTimeMs = (long)0;
	endTimeMs = (long)0;
	isWin = false;
	isDraw = false;
	battleScore = 0;
}

public WCGCS2US_002_030_UserEventOnBattleEnd(
	 long _uid
	, int _userLvl
	, long _roomSerial
	, int _roomSid
	, int _roomType
	, long _startTimeMs
	, long _endTimeMs
	, boolean _isWin
	, boolean _isDraw
	, int _battleScore
) {	uid = _uid;
	userLvl = _userLvl;
	roomSerial = _roomSerial;
	roomSid = _roomSid;
	roomType = _roomType;
	startTimeMs = _startTimeMs;
	endTimeMs = _endTimeMs;
	isWin = _isWin;
	isDraw = _isDraw;
	battleScore = _battleScore;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)30; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public int getUserLvl() { return userLvl; }
public void setUserLvl(int _userLvl) { userLvl = _userLvl; }
public long getRoomSerial() { return roomSerial; }
public void setRoomSerial(long _roomSerial) { roomSerial = _roomSerial; }
public int getRoomSid() { return roomSid; }
public void setRoomSid(int _roomSid) { roomSid = _roomSid; }
public int getRoomType() { return roomType; }
public void setRoomType(int _roomType) { roomType = _roomType; }
public long getStartTimeMs() { return startTimeMs; }
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
public long getEndTimeMs() { return endTimeMs; }
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }
public boolean getIsWin() { return isWin; }
public void setIsWin(boolean _isWin) { isWin = _isWin; }
public boolean getIsDraw() { return isDraw; }
public void setIsDraw(boolean _isDraw) { isDraw = _isDraw; }
public int getBattleScore() { return battleScore; }
public void setBattleScore(int _battleScore) { battleScore = _battleScore; }


public final int GetBufSize() {
	int _size = 50;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 52;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) userLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomSid = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isWin = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isDraw = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) battleScore = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putInt(userLvl);
	_buf.putLong(roomSerial);
	_buf.putInt(roomSid);
	_buf.putInt(roomType);
	_buf.putLong(startTimeMs);
	_buf.putLong(endTimeMs);
	_buf.put(isWin?(byte)1:(byte)0);
	_buf.put(isDraw?(byte)1:(byte)0);
	_buf.putInt(battleScore);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)30);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)30);
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

