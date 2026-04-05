package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_066_OnLeaveGuild implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否被踢出 */
private boolean isKick;


public GS2GC_032_066_OnLeaveGuild() {
	isKick = false;
}

public GS2GC_032_066_OnLeaveGuild(
	 boolean _isKick
) {	isKick = _isKick;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)66; }

/** 是否被踢出 */
public boolean getIsKick() { return isKick; }
/** 是否被踢出 */
public void setIsKick(boolean _isKick) { isKick = _isKick; }


public final int GetBufSize() {
	int _size = 1;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isKick = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isKick?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)66);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)66);
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

