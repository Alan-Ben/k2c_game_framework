package NP2CS_RB.np_p002_serverInfoOp;

import java.nio.ByteBuffer;
public class NP2CS_RB_002_009_RetTryCrossGameServer implements ALBasicProtocolPack._IALProtocolStructure {
private int serverTypeId;
/** 对应生成的实例ID */
private long instanceId;


public NP2CS_RB_002_009_RetTryCrossGameServer() {
	serverTypeId = 0;
	instanceId = (long)0;
}

public NP2CS_RB_002_009_RetTryCrossGameServer(
	 int _serverTypeId
	, long _instanceId
) {	serverTypeId = _serverTypeId;
	instanceId = _instanceId;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)9; }

public int getServerTypeId() { return serverTypeId; }
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }
/** 对应生成的实例ID */
public long getInstanceId() { return instanceId; }
/** 对应生成的实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverTypeId);
	_buf.putLong(instanceId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)9);
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

