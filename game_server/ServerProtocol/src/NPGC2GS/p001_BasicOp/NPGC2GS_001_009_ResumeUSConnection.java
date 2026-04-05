package NPGC2GS.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGC2GS_001_009_ResumeUSConnection implements ALBasicProtocolPack._IALProtocolStructure {
/** 客户端用于识别的序列号 */
private long clientSerialize;
/** 用户挑选的US服务器LogicId */
private int serverLogicId;


public NPGC2GS_001_009_ResumeUSConnection() {
	clientSerialize = (long)0;
	serverLogicId = 0;
}

public NPGC2GS_001_009_ResumeUSConnection(
	 long _clientSerialize
	, int _serverLogicId
) {	clientSerialize = _clientSerialize;
	serverLogicId = _serverLogicId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)9; }

/** 客户端用于识别的序列号 */
public long getClientSerialize() { return clientSerialize; }
/** 客户端用于识别的序列号 */
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
/** 用户挑选的US服务器LogicId */
public int getServerLogicId() { return serverLogicId; }
/** 用户挑选的US服务器LogicId */
public void setServerLogicId(int _serverLogicId) { serverLogicId = _serverLogicId; }


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
	if(_buf.remaining() > 0) clientSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverLogicId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(clientSerialize);
	_buf.putInt(serverLogicId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

