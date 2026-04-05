package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * NP US服务器单服聊天房间初始化信息
 **/
public class NpServerObj_USServerChatRoomInitInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 创建房间的服务器id */
private int serverTypeId;
/** 创建时间 */
private long createTimeMs;


public NpServerObj_USServerChatRoomInitInfo() {
	serverTypeId = 0;
	createTimeMs = (long)0;
}

public NpServerObj_USServerChatRoomInitInfo(
	 int _serverTypeId
	, long _createTimeMs
) {	serverTypeId = _serverTypeId;
	createTimeMs = _createTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 创建房间的服务器id */
public int getServerTypeId() { return serverTypeId; }
/** 创建房间的服务器id */
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }
/** 创建时间 */
public long getCreateTimeMs() { return createTimeMs; }
/** 创建时间 */
public void setCreateTimeMs(long _createTimeMs) { createTimeMs = _createTimeMs; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverTypeId);
	_buf.putLong(createTimeMs);
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

