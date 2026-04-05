package NP2HS_RB.p001_HSOp;

import java.nio.ByteBuffer;
public class NP2HS_RB_001_004_RetCheckIsInWhiteList implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否在白名单中 */
private boolean isInWhiteList;


public NP2HS_RB_001_004_RetCheckIsInWhiteList() {
	isInWhiteList = false;
}

public NP2HS_RB_001_004_RetCheckIsInWhiteList(
	 boolean _isInWhiteList
) {	isInWhiteList = _isInWhiteList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)4; }

/** 是否在白名单中 */
public boolean getIsInWhiteList() { return isInWhiteList; }
/** 是否在白名单中 */
public void setIsInWhiteList(boolean _isInWhiteList) { isInWhiteList = _isInWhiteList; }


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
	if(_buf.remaining() > 0) isInWhiteList = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isInWhiteList?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)4);
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

