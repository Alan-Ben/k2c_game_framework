package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 处理联盟加入请求
 **/
public class GC2GS_032_013_ReqProcessGuildJoinRequest implements ALBasicProtocolPack._IALProtocolStructure {
private long requestId;
private boolean isAccept;


public GC2GS_032_013_ReqProcessGuildJoinRequest() {
	requestId = (long)0;
	isAccept = false;
}

public GC2GS_032_013_ReqProcessGuildJoinRequest(
	 long _requestId
	, boolean _isAccept
) {	requestId = _requestId;
	isAccept = _isAccept;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)13; }

public long getRequestId() { return requestId; }
public void setRequestId(long _requestId) { requestId = _requestId; }
public boolean getIsAccept() { return isAccept; }
public void setIsAccept(boolean _isAccept) { isAccept = _isAccept; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) requestId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAccept = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(requestId);
	_buf.put(isAccept?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
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

