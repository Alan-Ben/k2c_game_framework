package Common;

import java.nio.ByteBuffer;
public class Common_USInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int serverTypeId;
private long usServerId;


public Common_USInfo() {
	serverTypeId = 0;
	usServerId = (long)0;
}

public Common_USInfo(
	 int _serverTypeId
	, long _usServerId
) {	serverTypeId = _serverTypeId;
	usServerId = _usServerId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getServerTypeId() { return serverTypeId; }
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }
public long getUsServerId() { return usServerId; }
public void setUsServerId(long _usServerId) { usServerId = _usServerId; }


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
	if(_buf.remaining() > 0) usServerId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverTypeId);
	_buf.putLong(usServerId);
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

