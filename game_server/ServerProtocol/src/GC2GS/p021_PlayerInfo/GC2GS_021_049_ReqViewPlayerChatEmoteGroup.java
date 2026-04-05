package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 设置表情包已查看
 **/
public class GC2GS_021_049_ReqViewPlayerChatEmoteGroup implements ALBasicProtocolPack._IALProtocolStructure {
private long refId;


public GC2GS_021_049_ReqViewPlayerChatEmoteGroup() {
	refId = (long)0;
}

public GC2GS_021_049_ReqViewPlayerChatEmoteGroup(
	 long _refId
) {	refId = _refId;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)49; }

public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }


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
	if(_buf.remaining() > 0) refId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)49);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)49);
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

