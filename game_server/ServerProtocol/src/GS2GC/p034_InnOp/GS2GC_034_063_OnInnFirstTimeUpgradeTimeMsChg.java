package GS2GC.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店首次升级时间变更
 **/
public class GS2GC_034_063_OnInnFirstTimeUpgradeTimeMsChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 旅店首次升级时间（毫秒） */
private long firstTimeUpgradeTimeMs;


public GS2GC_034_063_OnInnFirstTimeUpgradeTimeMsChg() {
	firstTimeUpgradeTimeMs = (long)0;
}

public GS2GC_034_063_OnInnFirstTimeUpgradeTimeMsChg(
	 long _firstTimeUpgradeTimeMs
) {	firstTimeUpgradeTimeMs = _firstTimeUpgradeTimeMs;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)63; }

/** 旅店首次升级时间（毫秒） */
public long getFirstTimeUpgradeTimeMs() { return firstTimeUpgradeTimeMs; }
/** 旅店首次升级时间（毫秒） */
public void setFirstTimeUpgradeTimeMs(long _firstTimeUpgradeTimeMs) { firstTimeUpgradeTimeMs = _firstTimeUpgradeTimeMs; }


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
	if(_buf.remaining() > 0) firstTimeUpgradeTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(firstTimeUpgradeTimeMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)63);
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

