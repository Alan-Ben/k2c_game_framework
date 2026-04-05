package GC2GS.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 藏品锁定状态修改
 **/
public class GC2GS_013_030_ReqEquipLockStateChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 是否锁定 */
private boolean isLock;


public GC2GS_013_030_ReqEquipLockStateChg() {
	dbId = (long)0;
	isLock = false;
}

public GC2GS_013_030_ReqEquipLockStateChg(
	 long _dbId
	, boolean _isLock
) {	dbId = _dbId;
	isLock = _isLock;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)30; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 是否锁定 */
public boolean getIsLock() { return isLock; }
/** 是否锁定 */
public void setIsLock(boolean _isLock) { isLock = _isLock; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isLock = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.put(isLock?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)30);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)30);
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

