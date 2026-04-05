package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 银币征收数据
 **/
public class ServerObj_Levy_Silver implements ALBasicProtocolPack._IALProtocolStructure {
/** 上次结算时间（毫秒） */
private long lastSettleMs;
/** 本次征收开始时间（毫秒） */
private long startMs;
/** 已经结算的数量总和 */
private long settledSum;
/** 产出速度 */
private long speed;


public ServerObj_Levy_Silver() {
	lastSettleMs = (long)0;
	startMs = (long)0;
	settledSum = (long)0;
	speed = (long)0;
}

public ServerObj_Levy_Silver(
	 long _lastSettleMs
	, long _startMs
	, long _settledSum
	, long _speed
) {	lastSettleMs = _lastSettleMs;
	startMs = _startMs;
	settledSum = _settledSum;
	speed = _speed;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 上次结算时间（毫秒） */
public long getLastSettleMs() { return lastSettleMs; }
/** 上次结算时间（毫秒） */
public void setLastSettleMs(long _lastSettleMs) { lastSettleMs = _lastSettleMs; }
/** 本次征收开始时间（毫秒） */
public long getStartMs() { return startMs; }
/** 本次征收开始时间（毫秒） */
public void setStartMs(long _startMs) { startMs = _startMs; }
/** 已经结算的数量总和 */
public long getSettledSum() { return settledSum; }
/** 已经结算的数量总和 */
public void setSettledSum(long _settledSum) { settledSum = _settledSum; }
/** 产出速度 */
public long getSpeed() { return speed; }
/** 产出速度 */
public void setSpeed(long _speed) { speed = _speed; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastSettleMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) settledSum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) speed = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(lastSettleMs);
	_buf.putLong(startMs);
	_buf.putLong(settledSum);
	_buf.putLong(speed);
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

