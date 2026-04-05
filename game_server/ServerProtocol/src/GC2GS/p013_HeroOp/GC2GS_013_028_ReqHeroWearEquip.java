package GC2GS.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 大臣装备藏品
 **/
public class GC2GS_013_028_ReqHeroWearEquip implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 大臣id */
private long heroId;


public GC2GS_013_028_ReqHeroWearEquip() {
	dbId = (long)0;
	heroId = (long)0;
}

public GC2GS_013_028_ReqHeroWearEquip(
	 long _dbId
	, long _heroId
) {	dbId = _dbId;
	heroId = _heroId;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)28; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(heroId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)28);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)28);
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

