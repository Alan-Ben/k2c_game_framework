package GC2GS.p022_ChatOp;

import java.nio.ByteBuffer;
/*********
 * 请求加入聊天房间
 **/
public class GC2GS_022_002_ReqPlayerJoinChatRoom implements ALBasicProtocolPack._IALProtocolStructure {
/** 聊天房间类型 */
private int roomType;
/** 额外参数，活动组队聊天-活动实例ID，其他-0 */
private long extId;


public GC2GS_022_002_ReqPlayerJoinChatRoom() {
	roomType = 0;
	extId = (long)0;
}

public GC2GS_022_002_ReqPlayerJoinChatRoom(
	 int _roomType
	, long _extId
) {	roomType = _roomType;
	extId = _extId;
}

public final byte getMainOrder() { return (byte)22; }

public final byte getSubOrder() { return (byte)2; }

/** 聊天房间类型 */
public int getRoomType() { return roomType; }
/** 聊天房间类型 */
public void setRoomType(int _roomType) { roomType = _roomType; }
/** 额外参数，活动组队聊天-活动实例ID，其他-0 */
public long getExtId() { return extId; }
/** 额外参数，活动组队聊天-活动实例ID，其他-0 */
public void setExtId(long _extId) { extId = _extId; }


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
	if(_buf.remaining() > 0) roomType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) extId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(roomType);
	_buf.putLong(extId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)22);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)22);
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

