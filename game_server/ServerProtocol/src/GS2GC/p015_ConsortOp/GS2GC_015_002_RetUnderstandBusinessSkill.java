package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-领悟经营技能等级
 **/
public class GS2GC_015_002_RetUnderstandBusinessSkill implements ALBasicProtocolPack._IALProtocolStructure {
/** 操作是否成功 */
private boolean isProUp;


public GS2GC_015_002_RetUnderstandBusinessSkill() {
	isProUp = false;
}

public GS2GC_015_002_RetUnderstandBusinessSkill(
	 boolean _isProUp
) {	isProUp = _isProUp;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)2; }

/** 操作是否成功 */
public boolean getIsProUp() { return isProUp; }
/** 操作是否成功 */
public void setIsProUp(boolean _isProUp) { isProUp = _isProUp; }


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
	if(_buf.remaining() > 0) isProUp = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isProUp?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)2);
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

