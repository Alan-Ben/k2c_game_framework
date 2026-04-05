package NPGS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGS2GC_001_009_ResumeUSConnectionRes implements ALBasicProtocolPack._IALProtocolStructure {
/** 错误信息，根据枚举处理 */
private int error;
/** 客户端用于识别的序列号 */
private long clientSerialize;
/** 服务器时间戳，用于心跳包处理 */
private long serverTimeTag;


public NPGS2GC_001_009_ResumeUSConnectionRes() {
	error = 0;
	clientSerialize = (long)0;
	serverTimeTag = (long)0;
}

public NPGS2GC_001_009_ResumeUSConnectionRes(
	 int _error
	, long _clientSerialize
	, long _serverTimeTag
) {	error = _error;
	clientSerialize = _clientSerialize;
	serverTimeTag = _serverTimeTag;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)9; }

/** 错误信息，根据枚举处理 */
public int getError() { return error; }
/** 错误信息，根据枚举处理 */
public void setError(int _error) { error = _error; }
/** 客户端用于识别的序列号 */
public long getClientSerialize() { return clientSerialize; }
/** 客户端用于识别的序列号 */
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
/** 服务器时间戳，用于心跳包处理 */
public long getServerTimeTag() { return serverTimeTag; }
/** 服务器时间戳，用于心跳包处理 */
public void setServerTimeTag(long _serverTimeTag) { serverTimeTag = _serverTimeTag; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) error = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTimeTag = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(error);
	_buf.putLong(clientSerialize);
	_buf.putLong(serverTimeTag);
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

