package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_061_OnGuildEventRemove implements ALBasicProtocolPack._IALProtocolStructure {
private long eventDbId;


public GS2GC_032_061_OnGuildEventRemove() {
	eventDbId = (long)0;
}

public GS2GC_032_061_OnGuildEventRemove(
	 long _eventDbId
) {	eventDbId = _eventDbId;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)61; }

public long getEventDbId() { return eventDbId; }
public void setEventDbId(long _eventDbId) { eventDbId = _eventDbId; }


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
	if(_buf.remaining() > 0) eventDbId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(eventDbId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)61);
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

