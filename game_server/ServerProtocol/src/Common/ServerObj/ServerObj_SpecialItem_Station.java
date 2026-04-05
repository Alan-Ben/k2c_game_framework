package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 特殊物品竞技场贸易站数据
 **/
public class ServerObj_SpecialItem_Station implements ALBasicProtocolPack._IALProtocolStructure {
/** 等级 */
private int level;
/** 已产出数量 */
private long hadOutputNum;
/** 已产出时间 */
private long hadOutputSec;
/** 上次结算时间戳 */
private long lastSettleTimeMs;
/** 是否无限制 */
private boolean isUnlimited;


public ServerObj_SpecialItem_Station() {
	level = 0;
	hadOutputNum = (long)0;
	hadOutputSec = (long)0;
	lastSettleTimeMs = (long)0;
	isUnlimited = false;
}

public ServerObj_SpecialItem_Station(
	 int _level
	, long _hadOutputNum
	, long _hadOutputSec
	, long _lastSettleTimeMs
	, boolean _isUnlimited
) {	level = _level;
	hadOutputNum = _hadOutputNum;
	hadOutputSec = _hadOutputSec;
	lastSettleTimeMs = _lastSettleTimeMs;
	isUnlimited = _isUnlimited;
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
/** 是否无限制 */
public boolean getIsUnlimited() { return isUnlimited; }
/** 是否无限制 */
public void setIsUnlimited(boolean _isUnlimited) { isUnlimited = _isUnlimited; }


public final int GetBufSize() {
	int _size = 29;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 31;

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
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isUnlimited = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(level);
	_buf.putLong(hadOutputNum);
	_buf.putLong(hadOutputSec);
	_buf.putLong(lastSettleTimeMs);
	_buf.put(isUnlimited?(byte)1:(byte)0);
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

