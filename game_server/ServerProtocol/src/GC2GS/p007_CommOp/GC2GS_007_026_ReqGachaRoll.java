package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 抽卡
 **/
public class GC2GS_007_026_ReqGachaRoll implements ALBasicProtocolPack._IALProtocolStructure {
/** 卡池id */
private long poolId;
/** 是否十连抽 */
private boolean isTen;


public GC2GS_007_026_ReqGachaRoll() {
	poolId = (long)0;
	isTen = false;
}

public GC2GS_007_026_ReqGachaRoll(
	 long _poolId
	, boolean _isTen
) {	poolId = _poolId;
	isTen = _isTen;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)26; }

/** 卡池id */
public long getPoolId() { return poolId; }
/** 卡池id */
public void setPoolId(long _poolId) { poolId = _poolId; }
/** 是否十连抽 */
public boolean getIsTen() { return isTen; }
/** 是否十连抽 */
public void setIsTen(boolean _isTen) { isTen = _isTen; }


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
	if(_buf.remaining() > 0) poolId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isTen = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(poolId);
	_buf.put(isTen?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)26);
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

