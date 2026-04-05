package NP2LCS_RB.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2LCS_RB_001_100_RetOpRes implements ALBasicProtocolPack._IALProtocolStructure {
private int errCode;


public NP2LCS_RB_001_100_RetOpRes() {
	errCode = 0;
}

public NP2LCS_RB_001_100_RetOpRes(
	 int _errCode
) {	errCode = _errCode;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)100; }

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
	_buf.put((byte)1);
	_buf.put((byte)100);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)100);
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

