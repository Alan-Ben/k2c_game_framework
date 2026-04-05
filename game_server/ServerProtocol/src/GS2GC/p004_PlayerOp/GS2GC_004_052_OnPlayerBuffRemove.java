package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_052_OnPlayerBuffRemove implements ALBasicProtocolPack._IALProtocolStructure {
private long buffId;


public GS2GC_004_052_OnPlayerBuffRemove() {
	buffId = (long)0;
}

public GS2GC_004_052_OnPlayerBuffRemove(
	 long _buffId
) {	buffId = _buffId;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)52; }

public long getBuffId() { return buffId; }
public void setBuffId(long _buffId) { buffId = _buffId; }


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
	if(_buf.remaining() > 0) buffId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(buffId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)52);
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

