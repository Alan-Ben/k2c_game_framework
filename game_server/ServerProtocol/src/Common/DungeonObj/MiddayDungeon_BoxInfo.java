package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 午间副本_宝箱信息
 **/
public class MiddayDungeon_BoxInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 宝箱ID */
private long boxId;
/** 发送者ID */
private long senderCid;
/** 发送者名字 */
private String senderName;
/** 过期时间戳 */
private long expireTimeMS;


public MiddayDungeon_BoxInfo() {
	dbId = (long)0;
	boxId = (long)0;
	senderCid = (long)0;
	senderName = "";
	expireTimeMS = (long)0;
}

public MiddayDungeon_BoxInfo(
	 long _dbId
	, long _boxId
	, long _senderCid
	, String _senderName
	, long _expireTimeMS
) {	dbId = _dbId;
	boxId = _boxId;
	senderCid = _senderCid;
	senderName = _senderName;
	expireTimeMS = _expireTimeMS;
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
/** 发送者ID */
public long getSenderCid() { return senderCid; }
/** 发送者ID */
public void setSenderCid(long _senderCid) { senderCid = _senderCid; }
/** 发送者名字 */
public String getSenderName() { return senderName; }
/** 发送者名字 */
public void setSenderName(String _senderName) { senderName = _senderName; }
/** 过期时间戳 */
public long getExpireTimeMS() { return expireTimeMS; }
/** 过期时间戳 */
public void setExpireTimeMS(long _expireTimeMS) { expireTimeMS = _expireTimeMS; }


public final int GetBufSize() {
	int _size = 32;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) senderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) senderName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expireTimeMS = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(boxId);
	_buf.putLong(senderCid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, senderName);
	_buf.putLong(expireTimeMS);
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

