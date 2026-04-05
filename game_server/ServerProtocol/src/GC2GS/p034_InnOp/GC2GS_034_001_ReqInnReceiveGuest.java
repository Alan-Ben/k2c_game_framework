package GC2GS.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店接待客人
 **/
public class GC2GS_034_001_ReqInnReceiveGuest implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否一键 */
private boolean isAKey;


public GC2GS_034_001_ReqInnReceiveGuest() {
	isAKey = false;
}

public GC2GS_034_001_ReqInnReceiveGuest(
	 boolean _isAKey
) {	isAKey = _isAKey;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)1; }

/** 是否一键 */
public boolean getIsAKey() { return isAKey; }
/** 是否一键 */
public void setIsAKey(boolean _isAKey) { isAKey = _isAKey; }


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
	if(_buf.remaining() > 0) isAKey = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isAKey?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)1);
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

