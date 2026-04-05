package ALLRPC.US.Friend;

import java.nio.ByteBuffer;
public class SendFriendApply_Return implements ALBasicProtocolPack._IALProtocolStructure {
private boolean isSuc;
private int errCode;


public SendFriendApply_Return() {
	isSuc = false;
	errCode = 0;
}

public SendFriendApply_Return(
	 boolean _isSuc
	, int _errCode
) {	isSuc = _isSuc;
	errCode = _errCode;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public boolean getIsSuc() { return isSuc; }
public void setIsSuc(boolean _isSuc) { isSuc = _isSuc; }
public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }


public final int GetBufSize() {
	int _size = 5;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 7;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSuc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isSuc?(byte)1:(byte)0);
	_buf.putInt(errCode);
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

