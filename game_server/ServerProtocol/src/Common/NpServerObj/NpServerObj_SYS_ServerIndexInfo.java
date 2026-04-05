package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 服务器索引信息
 **/
public class NpServerObj_SYS_ServerIndexInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 外部赋予的服务器id */
private int serverLogicId;
/** 服务器typeId */
private int serverTypeId;


public NpServerObj_SYS_ServerIndexInfo() {
	serverLogicId = 0;
	serverTypeId = 0;
}

public NpServerObj_SYS_ServerIndexInfo(
	 int _serverLogicId
	, int _serverTypeId
) {	serverLogicId = _serverLogicId;
	serverTypeId = _serverTypeId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 外部赋予的服务器id */
public int getServerLogicId() { return serverLogicId; }
/** 外部赋予的服务器id */
public void setServerLogicId(int _serverLogicId) { serverLogicId = _serverLogicId; }
/** 服务器typeId */
public int getServerTypeId() { return serverTypeId; }
/** 服务器typeId */
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }


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
	if(_buf.remaining() > 0) serverLogicId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTypeId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverLogicId);
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

