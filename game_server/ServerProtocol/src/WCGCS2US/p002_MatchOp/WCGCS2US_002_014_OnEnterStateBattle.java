package WCGCS2US.p002_MatchOp;

import java.nio.ByteBuffer;
public class WCGCS2US_002_014_OnEnterStateBattle implements ALBasicProtocolPack._IALProtocolStructure {
private boolean isStackCurrentState;
private long uid;
private int roomSid;
private long roomId;


public WCGCS2US_002_014_OnEnterStateBattle() {
	isStackCurrentState = false;
	uid = (long)0;
	roomSid = 0;
	roomId = (long)0;
}

public WCGCS2US_002_014_OnEnterStateBattle(
	 boolean _isStackCurrentState
	, long _uid
	, int _roomSid
	, long _roomId
) {	isStackCurrentState = _isStackCurrentState;
	uid = _uid;
	roomSid = _roomSid;
	roomId = _roomId;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)14; }

public boolean getIsStackCurrentState() { return isStackCurrentState; }
public void setIsStackCurrentState(boolean _isStackCurrentState) { isStackCurrentState = _isStackCurrentState; }
public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public int getRoomSid() { return roomSid; }
public void setRoomSid(int _roomSid) { roomSid = _roomSid; }
public long getRoomId() { return roomId; }
public void setRoomId(long _roomId) { roomId = _roomId; }


public final int GetBufSize() {
	int _size = 21;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isStackCurrentState = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomSid = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isStackCurrentState?(byte)1:(byte)0);
	_buf.putLong(uid);
	_buf.putInt(roomSid);
	_buf.putLong(roomId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)14);
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

