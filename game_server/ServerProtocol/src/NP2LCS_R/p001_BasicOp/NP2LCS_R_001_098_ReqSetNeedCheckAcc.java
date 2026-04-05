package NP2LCS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2LCS_R_001_098_ReqSetNeedCheckAcc implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否需要检测账号 */
private boolean needCheck;


public NP2LCS_R_001_098_ReqSetNeedCheckAcc() {
	needCheck = false;
}

public NP2LCS_R_001_098_ReqSetNeedCheckAcc(
	 boolean _needCheck
) {	needCheck = _needCheck;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)98; }

/** 是否需要检测账号 */
public boolean getNeedCheck() { return needCheck; }
/** 是否需要检测账号 */
public void setNeedCheck(boolean _needCheck) { needCheck = _needCheck; }


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
	if(_buf.remaining() > 0) needCheck = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(needCheck?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)98);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)98);
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

