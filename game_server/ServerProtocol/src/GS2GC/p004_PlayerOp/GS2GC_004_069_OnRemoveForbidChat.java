package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 解除禁言数据推送
 **/
public class GS2GC_004_069_OnRemoveForbidChat implements ALBasicProtocolPack._IALProtocolStructure {
private int roomType;


public GS2GC_004_069_OnRemoveForbidChat() {
	roomType = 0;
}

public GS2GC_004_069_OnRemoveForbidChat(
	 int _roomType
) {	roomType = _roomType;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)69; }

public int getRoomType() { return roomType; }
public void setRoomType(int _roomType) { roomType = _roomType; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roomType = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(roomType);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)69);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)69);
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

