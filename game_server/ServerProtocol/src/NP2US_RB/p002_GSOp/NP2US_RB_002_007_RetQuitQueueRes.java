package NP2US_RB.p002_GSOp;

import java.nio.ByteBuffer;
public class NP2US_RB_002_007_RetQuitQueueRes implements ALBasicProtocolPack._IALProtocolStructure {
private int errCode;


public NP2US_RB_002_007_RetQuitQueueRes() {
	errCode = 0;
}

public NP2US_RB_002_007_RetQuitQueueRes(
	 int _errCode
) {	errCode = _errCode;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)7; }

public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(errCode);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)7);
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

