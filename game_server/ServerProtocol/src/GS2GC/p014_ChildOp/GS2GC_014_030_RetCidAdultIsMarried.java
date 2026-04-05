package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 返回其他玩家子嗣是否结婚的信息
 **/
public class GS2GC_014_030_RetCidAdultIsMarried implements ALBasicProtocolPack._IALProtocolStructure {
private boolean isGiftde;
private boolean isMarried;


public GS2GC_014_030_RetCidAdultIsMarried() {
	isGiftde = false;
	isMarried = false;
}

public GS2GC_014_030_RetCidAdultIsMarried(
	 boolean _isGiftde
	, boolean _isMarried
) {	isGiftde = _isGiftde;
	isMarried = _isMarried;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)30; }

public boolean getIsGiftde() { return isGiftde; }
public void setIsGiftde(boolean _isGiftde) { isGiftde = _isGiftde; }
public boolean getIsMarried() { return isMarried; }
public void setIsMarried(boolean _isMarried) { isMarried = _isMarried; }


public final int GetBufSize() {
	int _size = 2;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 4;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isGiftde = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isMarried = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isGiftde?(byte)1:(byte)0);
	_buf.put(isMarried?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)30);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)30);
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

