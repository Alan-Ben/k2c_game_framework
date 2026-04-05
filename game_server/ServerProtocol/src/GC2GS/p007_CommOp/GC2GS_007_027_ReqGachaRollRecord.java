package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 抽卡记录
 **/
public class GC2GS_007_027_ReqGachaRollRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 卡池id */
private long poolId;


public GC2GS_007_027_ReqGachaRollRecord() {
	poolId = (long)0;
}

public GC2GS_007_027_ReqGachaRollRecord(
	 long _poolId
) {	poolId = _poolId;
}

public final byte getMainOrder() { return (byte)18; }

public final byte getSubOrder() { return (byte)27; }

/** 卡池id */
public long getPoolId() { return poolId; }
/** 卡池id */
public void setPoolId(long _poolId) { poolId = _poolId; }


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
	if(_buf.remaining() > 0) poolId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(poolId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)18);
	_buf.put((byte)27);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)18);
	_recBuf.put((byte)27);
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

