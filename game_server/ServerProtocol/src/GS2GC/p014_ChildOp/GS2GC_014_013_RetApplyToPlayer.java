package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 联姻请求-对指定玩家发起请求
 **/
public class GS2GC_014_013_RetApplyToPlayer implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否拒绝所有联姻请求 */
private boolean isPlayerRefuseAllRequest;


public GS2GC_014_013_RetApplyToPlayer() {
	isPlayerRefuseAllRequest = false;
}

public GS2GC_014_013_RetApplyToPlayer(
	 boolean _isPlayerRefuseAllRequest
) {	isPlayerRefuseAllRequest = _isPlayerRefuseAllRequest;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)13; }

/** 是否拒绝所有联姻请求 */
public boolean getIsPlayerRefuseAllRequest() { return isPlayerRefuseAllRequest; }
/** 是否拒绝所有联姻请求 */
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
	_buf.put((byte)14);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)13);
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

