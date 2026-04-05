package NP2CS_RB.np_p004_PveOp;

import java.nio.ByteBuffer;
public class NP2CS_RB_004_002_RetPvpRoom implements ALBasicProtocolPack._IALProtocolStructure {
private int err;
private long roomSerial;
private int roomServerID;


public NP2CS_RB_004_002_RetPvpRoom() {
	err = 0;
	roomSerial = (long)0;
	roomServerID = 0;
}

public NP2CS_RB_004_002_RetPvpRoom(
	 int _err
	, long _roomSerial
	, int _roomServerID
) {	err = _err;
	roomSerial = _roomSerial;
	roomServerID = _roomServerID;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)2; }

public int getErr() { return err; }
public void setErr(int _err) { err = _err; }
public long getRoomSerial() { return roomSerial; }
public void setRoomSerial(long _roomSerial) { roomSerial = _roomSerial; }
public int getRoomServerID() { return roomServerID; }
public void setRoomServerID(int _roomServerID) { roomServerID = _roomServerID; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) err = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomServerID = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(err);
	_buf.putLong(roomSerial);
	_buf.putInt(roomServerID);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)2);
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

