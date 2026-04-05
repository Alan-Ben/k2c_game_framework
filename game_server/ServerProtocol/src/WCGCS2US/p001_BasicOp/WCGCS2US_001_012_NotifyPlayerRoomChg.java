package WCGCS2US.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2US_001_012_NotifyPlayerRoomChg implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private int roomSid;


public WCGCS2US_001_012_NotifyPlayerRoomChg() {
	uid = (long)0;
	roomSid = 0;
}

public WCGCS2US_001_012_NotifyPlayerRoomChg(
	 long _uid
	, int _roomSid
) {	uid = _uid;
	roomSid = _roomSid;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)12; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public int getRoomSid() { return roomSid; }
public void setRoomSid(int _roomSid) { roomSid = _roomSid; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomSid = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putInt(roomSid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)12);
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

