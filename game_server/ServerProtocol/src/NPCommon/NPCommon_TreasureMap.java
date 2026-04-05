package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 藏宝图数据
 **/
public class NPCommon_TreasureMap implements ALBasicProtocolPack._IALProtocolStructure {
/** 唯一id */
private long dbId;
/** 藏宝图配置id */
private long refId;
/** 藏宝图空间位置 */
private long spaceId;
/** 藏宝图位置x */
private int posX;
/** 藏宝图位置y */
private int posY;


public NPCommon_TreasureMap() {
	dbId = (long)0;
	refId = (long)0;
	spaceId = (long)0;
	posX = 0;
	posY = 0;
}

public NPCommon_TreasureMap(
	 long _dbId
	, long _refId
	, long _spaceId
	, int _posX
	, int _posY
) {	dbId = _dbId;
	refId = _refId;
	spaceId = _spaceId;
	posX = _posX;
	posY = _posY;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 唯一id */
public long getDbId() { return dbId; }
/** 唯一id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 藏宝图配置id */
public long getRefId() { return refId; }
/** 藏宝图配置id */
public void setRefId(long _refId) { refId = _refId; }
/** 藏宝图空间位置 */
public long getSpaceId() { return spaceId; }
/** 藏宝图空间位置 */
public void setSpaceId(long _spaceId) { spaceId = _spaceId; }
/** 藏宝图位置x */
public int getPosX() { return posX; }
/** 藏宝图位置x */
public void setPosX(int _posX) { posX = _posX; }
/** 藏宝图位置y */
public int getPosY() { return posY; }
/** 藏宝图位置y */
public void setPosY(int _posY) { posY = _posY; }


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
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) spaceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) posX = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) posY = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(refId);
	_buf.putLong(spaceId);
	_buf.putInt(posX);
	_buf.putInt(posY);
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

