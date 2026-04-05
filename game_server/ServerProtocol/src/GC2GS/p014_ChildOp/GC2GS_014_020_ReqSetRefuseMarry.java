package GC2GS.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 设置拒绝联姻
 **/
public class GC2GS_014_020_ReqSetRefuseMarry implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否拒绝联姻 */
private boolean isRefuse;


public GC2GS_014_020_ReqSetRefuseMarry() {
	isRefuse = false;
}

public GC2GS_014_020_ReqSetRefuseMarry(
	 boolean _isRefuse
) {	isRefuse = _isRefuse;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)20; }

/** 是否拒绝联姻 */
public boolean getIsRefuse() { return isRefuse; }
/** 是否拒绝联姻 */
public void setIsRefuse(boolean _isRefuse) { isRefuse = _isRefuse; }


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
	if(_buf.remaining() > 0) isRefuse = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isRefuse?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)20);
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

