package Common.PlayerObj;

import java.nio.ByteBuffer;
/*********
 * 玩家火星能量信息
 **/
public class Player_MarsEnergyInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 上次结算时间 */
private long lastSettleTimeMs;
private long count;
/** 消耗速度（单位：分钟） */
private long costSpeed;


public Player_MarsEnergyInfo() {
	lastSettleTimeMs = (long)0;
	count = (long)0;
	costSpeed = (long)0;
}

public Player_MarsEnergyInfo(
	 long _lastSettleTimeMs
	, long _count
	, long _costSpeed
) {	lastSettleTimeMs = _lastSettleTimeMs;
	count = _count;
	costSpeed = _costSpeed;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 上次结算时间 */
public long getLastSettleTimeMs() { return lastSettleTimeMs; }
/** 上次结算时间 */
public void setLastSettleTimeMs(long _lastSettleTimeMs) { lastSettleTimeMs = _lastSettleTimeMs; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }
/** 消耗速度（单位：分钟） */
public long getCostSpeed() { return costSpeed; }
/** 消耗速度（单位：分钟） */
public void setCostSpeed(long _costSpeed) { costSpeed = _costSpeed; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastSettleTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) costSpeed = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(lastSettleTimeMs);
	_buf.putLong(count);
	_buf.putLong(costSpeed);
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

