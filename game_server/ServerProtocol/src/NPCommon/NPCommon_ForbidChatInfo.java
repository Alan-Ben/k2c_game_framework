package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 禁言数据
 **/
public class NPCommon_ForbidChatInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 截至时间 -1表示永久 */
private long endMs;
/** 聊天房间类型 */
private int roomType;


public NPCommon_ForbidChatInfo() {
	endMs = (long)0;
	roomType = 0;
}

public NPCommon_ForbidChatInfo(
	 long _endMs
	, int _roomType
) {	endMs = _endMs;
	roomType = _roomType;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 截至时间 -1表示永久 */
public long getEndMs() { return endMs; }
/** 截至时间 -1表示永久 */
public void setEndMs(long _endMs) { endMs = _endMs; }
/** 聊天房间类型 */
public int getRoomType() { return roomType; }
/** 聊天房间类型 */
public void setRoomType(int _roomType) { roomType = _roomType; }


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
	if(_buf.remaining() > 0) endMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomType = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(endMs);
	_buf.putInt(roomType);
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

