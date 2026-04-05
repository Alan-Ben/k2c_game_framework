package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 服务器负载信息
 **/
public class NpServerObj_SYS_ServerHoldInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 服务器typeId */
private int serverTypeId;
/** 负载计数 */
private int holdCount;
/** 总负载 */
private int totalCount;


public NpServerObj_SYS_ServerHoldInfo() {
	serverTypeId = 0;
	holdCount = 0;
	totalCount = 0;
}

public NpServerObj_SYS_ServerHoldInfo(
	 int _serverTypeId
	, int _holdCount
	, int _totalCount
) {	serverTypeId = _serverTypeId;
	holdCount = _holdCount;
	totalCount = _totalCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 服务器typeId */
public int getServerTypeId() { return serverTypeId; }
/** 服务器typeId */
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }
/** 负载计数 */
public int getHoldCount() { return holdCount; }
/** 负载计数 */
public void setHoldCount(int _holdCount) { holdCount = _holdCount; }
/** 总负载 */
public int getTotalCount() { return totalCount; }
/** 总负载 */
public void setTotalCount(int _totalCount) { totalCount = _totalCount; }


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
	if(_buf.remaining() > 0) holdCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverTypeId);
	_buf.putInt(holdCount);
	_buf.putInt(totalCount);
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

