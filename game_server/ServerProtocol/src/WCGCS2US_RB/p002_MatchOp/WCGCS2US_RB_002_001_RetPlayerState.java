package WCGCS2US_RB.p002_MatchOp;

import java.nio.ByteBuffer;
public class WCGCS2US_RB_002_001_RetPlayerState implements ALBasicProtocolPack._IALProtocolStructure {
private int errCode;
private int forbidLeftTime;


public WCGCS2US_RB_002_001_RetPlayerState() {
	errCode = 0;
	forbidLeftTime = 0;
}

public WCGCS2US_RB_002_001_RetPlayerState(
	 int _errCode
	, int _forbidLeftTime
) {	errCode = _errCode;
	forbidLeftTime = _forbidLeftTime;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)1; }

public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }
public int getForbidLeftTime() { return forbidLeftTime; }
public void setForbidLeftTime(int _forbidLeftTime) { forbidLeftTime = _forbidLeftTime; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) forbidLeftTime = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(errCode);
	_buf.putInt(forbidLeftTime);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

