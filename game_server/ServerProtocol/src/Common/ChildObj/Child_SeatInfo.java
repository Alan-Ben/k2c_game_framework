package Common.ChildObj;

import java.nio.ByteBuffer;
/*********
 * 子嗣训练位数据
 **/
public class Child_SeatInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 训练位ID */
private long seatId;
/** 最大脑力值 */
private int maxEnergy;
/** 脑力值 */
private int energy;
/** 上次计算时间（毫秒） */
private long lastCalMs;
/** 脑力值已满时，获得下一点脑力值所需时间 */
private long fullGetNextRemainMs;


public Child_SeatInfo() {
	seatId = (long)0;
	maxEnergy = 0;
	energy = 0;
	lastCalMs = (long)0;
	fullGetNextRemainMs = (long)0;
}

public Child_SeatInfo(
	 long _seatId
	, int _maxEnergy
	, int _energy
	, long _lastCalMs
	, long _fullGetNextRemainMs
) {	seatId = _seatId;
	maxEnergy = _maxEnergy;
	energy = _energy;
	lastCalMs = _lastCalMs;
	fullGetNextRemainMs = _fullGetNextRemainMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 训练位ID */
public long getSeatId() { return seatId; }
/** 训练位ID */
public void setSeatId(long _seatId) { seatId = _seatId; }
/** 最大脑力值 */
public int getMaxEnergy() { return maxEnergy; }
/** 最大脑力值 */
public void setMaxEnergy(int _maxEnergy) { maxEnergy = _maxEnergy; }
/** 脑力值 */
public int getEnergy() { return energy; }
/** 脑力值 */
public void setEnergy(int _energy) { energy = _energy; }
/** 上次计算时间（毫秒） */
public long getLastCalMs() { return lastCalMs; }
/** 上次计算时间（毫秒） */
public void setLastCalMs(long _lastCalMs) { lastCalMs = _lastCalMs; }
/** 脑力值已满时，获得下一点脑力值所需时间 */
public long getFullGetNextRemainMs() { return fullGetNextRemainMs; }
/** 脑力值已满时，获得下一点脑力值所需时间 */
public void setFullGetNextRemainMs(long _fullGetNextRemainMs) { fullGetNextRemainMs = _fullGetNextRemainMs; }


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
	if(_buf.remaining() > 0) seatId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxEnergy = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) energy = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastCalMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fullGetNextRemainMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(seatId);
	_buf.putInt(maxEnergy);
	_buf.putInt(energy);
	_buf.putLong(lastCalMs);
	_buf.putLong(fullGetNextRemainMs);
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

