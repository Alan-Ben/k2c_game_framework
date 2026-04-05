package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 士兵征收数据
 **/
public class ServerObj_Levy_Soldier implements ALBasicProtocolPack._IALProtocolStructure {
/** 上次结算时间（毫秒） */
private long lastSettleMs;
/** 下次结算时间（毫秒） */
private long nextSettleMs;
/** 使用次数 */
private int count;
/** 当前周期用时（毫秒） */
private long durationMs;


public ServerObj_Levy_Soldier() {
	lastSettleMs = (long)0;
	nextSettleMs = (long)0;
	count = 0;
	durationMs = (long)0;
}

public ServerObj_Levy_Soldier(
	 long _lastSettleMs
	, long _nextSettleMs
	, int _count
	, long _durationMs
) {	lastSettleMs = _lastSettleMs;
	nextSettleMs = _nextSettleMs;
	count = _count;
	durationMs = _durationMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 上次结算时间（毫秒） */
public long getLastSettleMs() { return lastSettleMs; }
/** 上次结算时间（毫秒） */
public void setLastSettleMs(long _lastSettleMs) { lastSettleMs = _lastSettleMs; }
/** 下次结算时间（毫秒） */
public long getNextSettleMs() { return nextSettleMs; }
/** 下次结算时间（毫秒） */
public void setNextSettleMs(long _nextSettleMs) { nextSettleMs = _nextSettleMs; }
/** 使用次数 */
public int getCount() { return count; }
/** 使用次数 */
public void setCount(int _count) { count = _count; }
/** 当前周期用时（毫秒） */
public long getDurationMs() { return durationMs; }
/** 当前周期用时（毫秒） */
public void setDurationMs(long _durationMs) { durationMs = _durationMs; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastSettleMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextSettleMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) durationMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(lastSettleMs);
	_buf.putLong(nextSettleMs);
	_buf.putInt(count);
	_buf.putLong(durationMs);
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

