package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟活跃宝箱信息
 **/
public class Guild_ActiveBoxInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 发送者cid */
private long senderCid;
/** 发放时间 */
private long sendTimeMs;
/** 印章数量 */
private long stamp;


public Guild_ActiveBoxInfo() {
	dbId = (long)0;
	senderCid = (long)0;
	sendTimeMs = (long)0;
	stamp = (long)0;
}

public Guild_ActiveBoxInfo(
	 long _dbId
	, long _senderCid
	, long _sendTimeMs
	, long _stamp
) {	dbId = _dbId;
	senderCid = _senderCid;
	sendTimeMs = _sendTimeMs;
	stamp = _stamp;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 发送者cid */
public long getSenderCid() { return senderCid; }
/** 发送者cid */
public void setSenderCid(long _senderCid) { senderCid = _senderCid; }
/** 发放时间 */
public long getSendTimeMs() { return sendTimeMs; }
/** 发放时间 */
public void setSendTimeMs(long _sendTimeMs) { sendTimeMs = _sendTimeMs; }
/** 印章数量 */
public long getStamp() { return stamp; }
/** 印章数量 */
public void setStamp(long _stamp) { stamp = _stamp; }


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
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) senderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sendTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stamp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(senderCid);
	_buf.putLong(sendTimeMs);
	_buf.putLong(stamp);
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

