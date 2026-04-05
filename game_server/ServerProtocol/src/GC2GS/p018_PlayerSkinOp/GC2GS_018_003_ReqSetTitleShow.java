package GC2GS.p018_PlayerSkinOp;

import java.nio.ByteBuffer;
/*********
 * 设置称号是否可展示
 **/
public class GC2GS_018_003_ReqSetTitleShow implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private boolean isShow;


public GC2GS_018_003_ReqSetTitleShow() {
	isShow = false;
}

public GC2GS_018_003_ReqSetTitleShow(
	 boolean _isShow
) {	isShow = _isShow;
}

public final byte getMainOrder() { return (byte)18; }

public final byte getSubOrder() { return (byte)3; }

/** 空 */
public boolean getIsShow() { return isShow; }
/** 空 */
public void setIsShow(boolean _isShow) { isShow = _isShow; }


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
	if(_buf.remaining() > 0) isShow = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isShow?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)3);
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

