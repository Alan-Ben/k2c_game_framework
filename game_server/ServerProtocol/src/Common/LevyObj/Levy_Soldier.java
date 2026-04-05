package Common.LevyObj;

import java.nio.ByteBuffer;
/*********
 * 士兵征收信息
 **/
public class Levy_Soldier implements ALBasicProtocolPack._IALProtocolStructure {
/** 下次结算时间（毫秒） */
private long nextSettleMs;
/** 当前周期用时（毫秒） */
private long curDurationMs;
/** 使用次数 */
private int count;


public Levy_Soldier() {
	nextSettleMs = (long)0;
	curDurationMs = (long)0;
	count = 0;
}

public Levy_Soldier(
	 long _nextSettleMs
	, long _curDurationMs
	, int _count
) {	nextSettleMs = _nextSettleMs;
	curDurationMs = _curDurationMs;
	count = _count;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 下次结算时间（毫秒） */
public long getNextSettleMs() { return nextSettleMs; }
/** 下次结算时间（毫秒） */
public void setNextSettleMs(long _nextSettleMs) { nextSettleMs = _nextSettleMs; }
/** 当前周期用时（毫秒） */
public long getCurDurationMs() { return curDurationMs; }
/** 当前周期用时（毫秒） */
public void setCurDurationMs(long _curDurationMs) { curDurationMs = _curDurationMs; }
/** 使用次数 */
public int getCount() { return count; }
/** 使用次数 */
public void setCount(int _count) { count = _count; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextSettleMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curDurationMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(nextSettleMs);
	_buf.putLong(curDurationMs);
	_buf.putInt(count);
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

