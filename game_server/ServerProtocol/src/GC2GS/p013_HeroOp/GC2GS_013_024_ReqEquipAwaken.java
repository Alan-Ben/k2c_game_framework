package GC2GS.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 藏品觉醒
 **/
public class GC2GS_013_024_ReqEquipAwaken implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 消耗组ID */
private long groupId;


public GC2GS_013_024_ReqEquipAwaken() {
	dbId = (long)0;
	groupId = (long)0;
}

public GC2GS_013_024_ReqEquipAwaken(
	 long _dbId
	, long _groupId
) {	dbId = _dbId;
	groupId = _groupId;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)24; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 消耗组ID */
public long getGroupId() { return groupId; }
/** 消耗组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }


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
	if(_buf.remaining() > 0) groupId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(groupId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)24);
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

