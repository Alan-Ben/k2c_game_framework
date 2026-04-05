package ALLRPC.Common.Chat;

import java.nio.ByteBuffer;
public class GetChatRoom_Req implements ALBasicProtocolPack._IALProtocolStructure {
private int roomType;
private long roomTypeId;


public GetChatRoom_Req() {
	roomType = 0;
	roomTypeId = (long)0;
}

public GetChatRoom_Req(
	 int _roomType
	, long _roomTypeId
) {	roomType = _roomType;
	roomTypeId = _roomTypeId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getRoomType() { return roomType; }
public void setRoomType(int _roomType) { roomType = _roomType; }
public long getRoomTypeId() { return roomTypeId; }
public void setRoomTypeId(long _roomTypeId) { roomTypeId = _roomTypeId; }


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
	if(_buf.remaining() > 0) roomTypeId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(roomType);
	_buf.putLong(roomTypeId);
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

