package NP2US.p002_UserOp;

import java.nio.ByteBuffer;
public class NP2US_002_001_PveBattleEnd implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private boolean isWin;
private long roomSerial;
private long stageSerial;
private long battleMs;


public NP2US_002_001_PveBattleEnd() {
	cid = (long)0;
	isWin = false;
	roomSerial = (long)0;
	stageSerial = (long)0;
	battleMs = (long)0;
}

public NP2US_002_001_PveBattleEnd(
	 long _cid
	, boolean _isWin
	, long _roomSerial
	, long _stageSerial
	, long _battleMs
) {	cid = _cid;
	isWin = _isWin;
	roomSerial = _roomSerial;
	stageSerial = _stageSerial;
	battleMs = _battleMs;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)1; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public boolean getIsWin() { return isWin; }
public void setIsWin(boolean _isWin) { isWin = _isWin; }
public long getRoomSerial() { return roomSerial; }
public void setRoomSerial(long _roomSerial) { roomSerial = _roomSerial; }
public long getStageSerial() { return stageSerial; }
public void setStageSerial(long _stageSerial) { stageSerial = _stageSerial; }
public long getBattleMs() { return battleMs; }
public void setBattleMs(long _battleMs) { battleMs = _battleMs; }


public final int GetBufSize() {
	int _size = 33;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 35;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isWin = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stageSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) battleMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.put(isWin?(byte)1:(byte)0);
	_buf.putLong(roomSerial);
	_buf.putLong(stageSerial);
	_buf.putLong(battleMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

