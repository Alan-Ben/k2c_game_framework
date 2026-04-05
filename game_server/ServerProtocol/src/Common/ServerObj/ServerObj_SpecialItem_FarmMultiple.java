package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 特殊物品_农田暴击数据
 **/
public class ServerObj_SpecialItem_FarmMultiple implements ALBasicProtocolPack._IALProtocolStructure {
/** 已暴击次数 */
private int hadMultipleTimes;
/** 上次刷新时间戳 */
private long lastRefreshTimeMs;


public ServerObj_SpecialItem_FarmMultiple() {
	hadMultipleTimes = 0;
	lastRefreshTimeMs = (long)0;
}

public ServerObj_SpecialItem_FarmMultiple(
	 int _hadMultipleTimes
	, long _lastRefreshTimeMs
) {	hadMultipleTimes = _hadMultipleTimes;
	lastRefreshTimeMs = _lastRefreshTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 已暴击次数 */
public int getHadMultipleTimes() { return hadMultipleTimes; }
/** 已暴击次数 */
public void setHadMultipleTimes(int _hadMultipleTimes) { hadMultipleTimes = _hadMultipleTimes; }
/** 上次刷新时间戳 */
public long getLastRefreshTimeMs() { return lastRefreshTimeMs; }
/** 上次刷新时间戳 */
public void setLastRefreshTimeMs(long _lastRefreshTimeMs) { lastRefreshTimeMs = _lastRefreshTimeMs; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadMultipleTimes = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastRefreshTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(hadMultipleTimes);
	_buf.putLong(lastRefreshTimeMs);
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

