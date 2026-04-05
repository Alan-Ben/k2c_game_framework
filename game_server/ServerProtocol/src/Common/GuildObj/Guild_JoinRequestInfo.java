package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟加入请求信息
 **/
public class Guild_JoinRequestInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
private long cid;
/** 请求入盟时间 */
private long requestTimeMs;


public Guild_JoinRequestInfo() {
	dbId = (long)0;
	cid = (long)0;
	requestTimeMs = (long)0;
}

public Guild_JoinRequestInfo(
	 long _dbId
	, long _cid
	, long _requestTimeMs
) {	dbId = _dbId;
	cid = _cid;
	requestTimeMs = _requestTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/** 请求入盟时间 */
public long getRequestTimeMs() { return requestTimeMs; }
/** 请求入盟时间 */
public void setRequestTimeMs(long _requestTimeMs) { requestTimeMs = _requestTimeMs; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) requestTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(cid);
	_buf.putLong(requestTimeMs);
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

