package GS2GC.p022_ChatOp;

import java.nio.ByteBuffer;
/*********
 * 加入新房间
 **/
public class GS2GC_022_050_OnChatRoomJoin implements ALBasicProtocolPack._IALProtocolStructure {
/** 聊天房间类型 */
private int roomType;
/** 聊天房间ID */
private long roomId;
/** 额外参数 */
private long extId;


public GS2GC_022_050_OnChatRoomJoin() {
	roomType = 0;
	roomId = (long)0;
	extId = (long)0;
}

public GS2GC_022_050_OnChatRoomJoin(
	 int _roomType
	, long _roomId
	, long _extId
) {	roomType = _roomType;
	roomId = _roomId;
	extId = _extId;
}

public final byte getMainOrder() { return (byte)22; }

public final byte getSubOrder() { return (byte)50; }

/** 聊天房间类型 */
public int getRoomType() { return roomType; }
/** 聊天房间类型 */
public void setRoomType(int _roomType) { roomType = _roomType; }
/** 聊天房间ID */
public long getRoomId() { return roomId; }
/** 聊天房间ID */
public void setRoomId(long _roomId) { roomId = _roomId; }
/** 额外参数 */
public long getExtId() { return extId; }
/** 额外参数 */
public void setExtId(long _extId) { extId = _extId; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) extId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(roomType);
	_buf.putLong(roomId);
	_buf.putLong(extId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)22);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)22);
	_recBuf.put((byte)50);
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

