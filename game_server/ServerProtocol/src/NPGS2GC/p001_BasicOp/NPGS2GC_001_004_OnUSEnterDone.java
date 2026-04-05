package NPGS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGS2GC_001_004_OnUSEnterDone implements ALBasicProtocolPack._IALProtocolStructure {
private int errCode;
/** 客户端登录操作的序列号 */
private long clientInitSerialize;
/** 服务器时间戳，用于心跳包处理 */
private long serverTimeTag;
private int usId;
/** 玩家cid */
private long cid;


public NPGS2GC_001_004_OnUSEnterDone() {
	errCode = 0;
	clientInitSerialize = (long)0;
	serverTimeTag = (long)0;
	usId = 0;
	cid = (long)0;
}

public NPGS2GC_001_004_OnUSEnterDone(
	 int _errCode
	, long _clientInitSerialize
	, long _serverTimeTag
	, int _usId
	, long _cid
) {	errCode = _errCode;
	clientInitSerialize = _clientInitSerialize;
	serverTimeTag = _serverTimeTag;
	usId = _usId;
	cid = _cid;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)4; }

public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }
/** 客户端登录操作的序列号 */
public long getClientInitSerialize() { return clientInitSerialize; }
/** 客户端登录操作的序列号 */
public void setClientInitSerialize(long _clientInitSerialize) { clientInitSerialize = _clientInitSerialize; }
/** 服务器时间戳，用于心跳包处理 */
public long getServerTimeTag() { return serverTimeTag; }
/** 服务器时间戳，用于心跳包处理 */
public void setServerTimeTag(long _serverTimeTag) { serverTimeTag = _serverTimeTag; }
public int getUsId() { return usId; }
public void setUsId(int _usId) { usId = _usId; }
/** 玩家cid */
public long getCid() { return cid; }
/** 玩家cid */
public void setCid(long _cid) { cid = _cid; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientInitSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTimeTag = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(errCode);
	_buf.putLong(clientInitSerialize);
	_buf.putLong(serverTimeTag);
	_buf.putInt(usId);
	_buf.putLong(cid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)4);
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

