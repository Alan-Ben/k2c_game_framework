package Common.PlayerObj;

import java.nio.ByteBuffer;
/*********
 * 玩家金币信息
 **/
public class Player_GoldInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 上次结算时间 */
private long lastSettleTimeMs;
private long count;
/** 产出速度 */
private long outputSpeed;
/** 总消耗数量 */
private long totalConsumeCount;


public Player_GoldInfo() {
	lastSettleTimeMs = (long)0;
	count = (long)0;
	outputSpeed = (long)0;
	totalConsumeCount = (long)0;
}

public Player_GoldInfo(
	 long _lastSettleTimeMs
	, long _count
	, long _outputSpeed
	, long _totalConsumeCount
) {	lastSettleTimeMs = _lastSettleTimeMs;
	count = _count;
	outputSpeed = _outputSpeed;
	totalConsumeCount = _totalConsumeCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 上次结算时间 */
public long getLastSettleTimeMs() { return lastSettleTimeMs; }
/** 上次结算时间 */
public void setLastSettleTimeMs(long _lastSettleTimeMs) { lastSettleTimeMs = _lastSettleTimeMs; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }
/** 产出速度 */
public long getOutputSpeed() { return outputSpeed; }
/** 产出速度 */
public void setOutputSpeed(long _outputSpeed) { outputSpeed = _outputSpeed; }
/** 总消耗数量 */
public long getTotalConsumeCount() { return totalConsumeCount; }
/** 总消耗数量 */
public void setTotalConsumeCount(long _totalConsumeCount) { totalConsumeCount = _totalConsumeCount; }


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
	if(_buf.remaining() > 0) lastSettleTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) outputSpeed = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalConsumeCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(lastSettleTimeMs);
	_buf.putLong(count);
	_buf.putLong(outputSpeed);
	_buf.putLong(totalConsumeCount);
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

