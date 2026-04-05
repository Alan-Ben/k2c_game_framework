package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 特殊物品火星能量数据
 **/
public class ServerObj_SpecialItem_MarsEnergy implements ALBasicProtocolPack._IALProtocolStructure {
/** 上次结算时间（毫秒） */
private long lastSettleTimeMs;
/** 数量 */
private long count;
/** 消耗速度（单位：分钟） */
private long costSpeed;
/** 总获得数量 */
private long totalGainCount;
/** 总消耗数量 */
private long totalConsumeCount;


public ServerObj_SpecialItem_MarsEnergy() {
	lastSettleTimeMs = (long)0;
	count = (long)0;
	costSpeed = (long)0;
	totalGainCount = (long)0;
	totalConsumeCount = (long)0;
}

public ServerObj_SpecialItem_MarsEnergy(
	 long _lastSettleTimeMs
	, long _count
	, long _costSpeed
	, long _totalGainCount
	, long _totalConsumeCount
) {	lastSettleTimeMs = _lastSettleTimeMs;
	count = _count;
	costSpeed = _costSpeed;
	totalGainCount = _totalGainCount;
	totalConsumeCount = _totalConsumeCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 上次结算时间（毫秒） */
public long getLastSettleTimeMs() { return lastSettleTimeMs; }
/** 上次结算时间（毫秒） */
public void setLastSettleTimeMs(long _lastSettleTimeMs) { lastSettleTimeMs = _lastSettleTimeMs; }
/** 数量 */
public long getCount() { return count; }
/** 数量 */
public void setCount(long _count) { count = _count; }
/** 消耗速度（单位：分钟） */
public long getCostSpeed() { return costSpeed; }
/** 消耗速度（单位：分钟） */
public void setCostSpeed(long _costSpeed) { costSpeed = _costSpeed; }
/** 总获得数量 */
public long getTotalGainCount() { return totalGainCount; }
/** 总获得数量 */
public void setTotalGainCount(long _totalGainCount) { totalGainCount = _totalGainCount; }
/** 总消耗数量 */
public long getTotalConsumeCount() { return totalConsumeCount; }
/** 总消耗数量 */
public void setTotalConsumeCount(long _totalConsumeCount) { totalConsumeCount = _totalConsumeCount; }


public final int GetBufSize() {
	int _size = 40;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastSettleTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) costSpeed = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalGainCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalConsumeCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(lastSettleTimeMs);
	_buf.putLong(count);
	_buf.putLong(costSpeed);
	_buf.putLong(totalGainCount);
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

