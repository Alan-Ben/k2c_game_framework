package GC2GS.p024_DungeonOp;

import java.nio.ByteBuffer;
/*********
 * 查询午间副本宝箱是否可以领取
 **/
public class GC2GS_024_005_ReqMiddayDungeonBoxCanDraw implements ALBasicProtocolPack._IALProtocolStructure {
/** 宝箱数据ID */
private long dbId;


public GC2GS_024_005_ReqMiddayDungeonBoxCanDraw() {
	dbId = (long)0;
}

public GC2GS_024_005_ReqMiddayDungeonBoxCanDraw(
	 long _dbId
) {	dbId = _dbId;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)5; }

/** 宝箱数据ID */
public long getDbId() { return dbId; }
/** 宝箱数据ID */
public void setDbId(long _dbId) { dbId = _dbId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)5);
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

