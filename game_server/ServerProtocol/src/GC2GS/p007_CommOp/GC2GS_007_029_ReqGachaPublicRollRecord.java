package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 抽卡公屏记录
 **/
public class GC2GS_007_029_ReqGachaPublicRollRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 卡池id */
private long poolId;
/** 抽卡记录数据id 查询这个id之后的数据 */
private long dbId;


public GC2GS_007_029_ReqGachaPublicRollRecord() {
	poolId = (long)0;
	dbId = (long)0;
}

public GC2GS_007_029_ReqGachaPublicRollRecord(
	 long _poolId
	, long _dbId
) {	poolId = _poolId;
	dbId = _dbId;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)29; }

/** 卡池id */
public long getPoolId() { return poolId; }
/** 卡池id */
public void setPoolId(long _poolId) { poolId = _poolId; }
/** 抽卡记录数据id 查询这个id之后的数据 */
public long getDbId() { return dbId; }
/** 抽卡记录数据id 查询这个id之后的数据 */
public void setDbId(long _dbId) { dbId = _dbId; }


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
	if(_buf.remaining() > 0) poolId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(poolId);
	_buf.putLong(dbId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)29);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)29);
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

