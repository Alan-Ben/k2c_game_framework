package Common.PlayerObj;

import java.nio.ByteBuffer;
/*********
 * 玩家贸易站数据
 **/
public class Player_StationInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 等级 */
private int level;
/** 已产出数量 */
private long hadOutputNum;
/** 已产出时间 */
private long hadOutputSec;
/** 上次结算时间戳 */
private long lastSettleTimeMs;


public Player_StationInfo() {
	level = 0;
	hadOutputNum = (long)0;
	hadOutputSec = (long)0;
	lastSettleTimeMs = (long)0;
}

public Player_StationInfo(
	 int _level
	, long _hadOutputNum
	, long _hadOutputSec
	, long _lastSettleTimeMs
) {	level = _level;
	hadOutputNum = _hadOutputNum;
	hadOutputSec = _hadOutputSec;
	lastSettleTimeMs = _lastSettleTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 等级 */
public int getLevel() { return level; }
/** 等级 */
public void setLevel(int _level) { level = _level; }
/** 已产出数量 */
public long getHadOutputNum() { return hadOutputNum; }
/** 已产出数量 */
public void setHadOutputNum(long _hadOutputNum) { hadOutputNum = _hadOutputNum; }
/** 已产出时间 */
public long getHadOutputSec() { return hadOutputSec; }
/** 已产出时间 */
public void setHadOutputSec(long _hadOutputSec) { hadOutputSec = _hadOutputSec; }
/** 上次结算时间戳 */
public long getLastSettleTimeMs() { return lastSettleTimeMs; }
/** 上次结算时间戳 */
public void setLastSettleTimeMs(long _lastSettleTimeMs) { lastSettleTimeMs = _lastSettleTimeMs; }


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
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadOutputNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadOutputSec = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastSettleTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(level);
	_buf.putLong(hadOutputNum);
	_buf.putLong(hadOutputSec);
	_buf.putLong(lastSettleTimeMs);
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

