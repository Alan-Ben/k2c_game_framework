package Common.NpChatObj;

import java.nio.ByteBuffer;
/*********
 * 午间副本宝箱
 **/
public class ChatObj_MiddayDungeonBox implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 宝箱ID */
private long boxId;
/** 过期时间 */
private long expiredTimeMs;


public ChatObj_MiddayDungeonBox() {
	dbId = (long)0;
	boxId = (long)0;
	expiredTimeMs = (long)0;
}

public ChatObj_MiddayDungeonBox(
	 long _dbId
	, long _boxId
	, long _expiredTimeMs
) {	dbId = _dbId;
	boxId = _boxId;
	expiredTimeMs = _expiredTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 宝箱ID */
public long getBoxId() { return boxId; }
/** 宝箱ID */
public void setBoxId(long _boxId) { boxId = _boxId; }
/** 过期时间 */
public long getExpiredTimeMs() { return expiredTimeMs; }
/** 过期时间 */
public void setExpiredTimeMs(long _expiredTimeMs) { expiredTimeMs = _expiredTimeMs; }


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
	if(_buf.remaining() > 0) boxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expiredTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(boxId);
	_buf.putLong(expiredTimeMs);
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

