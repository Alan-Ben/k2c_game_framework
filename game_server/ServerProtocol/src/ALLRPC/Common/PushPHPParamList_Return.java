package ALLRPC.Common;

import java.nio.ByteBuffer;
public class PushPHPParamList_Return implements ALBasicProtocolPack._IALProtocolStructure {
private boolean isSucc;


public PushPHPParamList_Return() {
	isSucc = false;
}

public PushPHPParamList_Return(
	 boolean _isSucc
) {	isSucc = _isSucc;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public boolean getIsSucc() { return isSucc; }
public void setIsSucc(boolean _isSucc) { isSucc = _isSucc; }


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
	if(_buf.remaining() > 0) isSucc = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isSucc?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

