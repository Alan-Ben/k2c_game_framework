package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 房间皮肤删除推送
 **/
public class GS2GC_021_094_OnRoomSkinDel implements ALBasicProtocolPack._IALProtocolStructure {
/** 房间皮肤ID */
private long roomSkinId;


public GS2GC_021_094_OnRoomSkinDel() {
	roomSkinId = (long)0;
}

public GS2GC_021_094_OnRoomSkinDel(
	 long _roomSkinId
) {	roomSkinId = _roomSkinId;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)94; }

/** 房间皮肤ID */
public long getRoomSkinId() { return roomSkinId; }
/** 房间皮肤ID */
public void setRoomSkinId(long _roomSkinId) { roomSkinId = _roomSkinId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomSkinId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(roomSkinId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)94);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)94);
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

