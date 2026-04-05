package NP2CS.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2CS_001_011_RemoveHandleServer implements ALBasicProtocolPack._IALProtocolStructure {
private int serverType;
private int serverTypeId;


public NP2CS_001_011_RemoveHandleServer() {
	serverType = 0;
	serverTypeId = 0;
}

public NP2CS_001_011_RemoveHandleServer(
	 int _serverType
	, int _serverTypeId
) {	serverType = _serverType;
	serverTypeId = _serverTypeId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)11; }

public int getServerType() { return serverType; }
public void setServerType(int _serverType) { serverType = _serverType; }
public int getServerTypeId() { return serverTypeId; }
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
	if(_buf.remaining() > 0) serverType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTypeId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverType);
	_buf.putInt(serverTypeId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)11);
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

