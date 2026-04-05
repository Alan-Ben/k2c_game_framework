package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 比武场初始化信息
 **/
public class Arena_InstanceInitInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 比武场配置id */
private long arenaRefId;
/** 服务器类型 */
private int serverType;
/** 服务器id */
private int serverTypeId;


public Arena_InstanceInitInfo() {
	arenaRefId = (long)0;
	serverType = 0;
	serverTypeId = 0;
}

public Arena_InstanceInitInfo(
	 long _arenaRefId
	, int _serverType
	, int _serverTypeId
) {	arenaRefId = _arenaRefId;
	serverType = _serverType;
	serverTypeId = _serverTypeId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 比武场配置id */
public long getArenaRefId() { return arenaRefId; }
/** 比武场配置id */
public void setArenaRefId(long _arenaRefId) { arenaRefId = _arenaRefId; }
/** 服务器类型 */
public int getServerType() { return serverType; }
/** 服务器类型 */
public void setServerType(int _serverType) { serverType = _serverType; }
/** 服务器id */
public int getServerTypeId() { return serverTypeId; }
/** 服务器id */
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) arenaRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTypeId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(arenaRefId);
	_buf.putInt(serverType);
	_buf.putInt(serverTypeId);
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

