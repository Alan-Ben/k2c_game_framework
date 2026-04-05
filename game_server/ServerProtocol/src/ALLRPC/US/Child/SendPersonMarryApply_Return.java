package ALLRPC.US.Child;

import java.nio.ByteBuffer;
public class SendPersonMarryApply_Return implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家是否拒绝所有联姻请求 */
private boolean isPlayerRefuseAllRequest;


public SendPersonMarryApply_Return() {
	isPlayerRefuseAllRequest = false;
}

public SendPersonMarryApply_Return(
	 boolean _isPlayerRefuseAllRequest
) {	isPlayerRefuseAllRequest = _isPlayerRefuseAllRequest;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家是否拒绝所有联姻请求 */
public boolean getIsPlayerRefuseAllRequest() { return isPlayerRefuseAllRequest; }
/** 玩家是否拒绝所有联姻请求 */
public void setIsPlayerRefuseAllRequest(boolean _isPlayerRefuseAllRequest) { isPlayerRefuseAllRequest = _isPlayerRefuseAllRequest; }


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
	if(_buf.remaining() > 0) isPlayerRefuseAllRequest = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isPlayerRefuseAllRequest?(byte)1:(byte)0);
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

