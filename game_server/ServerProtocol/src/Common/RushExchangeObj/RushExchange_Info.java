package Common.RushExchangeObj;

import java.nio.ByteBuffer;
/*********
 * 急速兑换信息
 **/
public class RushExchange_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包组ID */
private long groupId;
/** 当前礼包配置ID */
private long refId;
/** 当前兑换开始时间 如果没兑换影响刷新礼包时间 */
private long activeTimeMs;
/** 兑换时间 影响什么时候可以领奖 */
private long exchangeTimeMs;
/** 是否已领奖 */
private boolean isRewarded;
/** 当天兑换次数 */
private int todayExchangeCount;
/** 下次刷新兑换次数时间 */
private long nextResetCountTimeMs;


public RushExchange_Info() {
	groupId = (long)0;
	refId = (long)0;
	activeTimeMs = (long)0;
	exchangeTimeMs = (long)0;
	isRewarded = false;
	todayExchangeCount = 0;
	nextResetCountTimeMs = (long)0;
}

public RushExchange_Info(
	 long _groupId
	, long _refId
	, long _activeTimeMs
	, long _exchangeTimeMs
	, boolean _isRewarded
	, int _todayExchangeCount
	, long _nextResetCountTimeMs
) {	groupId = _groupId;
	refId = _refId;
	activeTimeMs = _activeTimeMs;
	exchangeTimeMs = _exchangeTimeMs;
	isRewarded = _isRewarded;
	todayExchangeCount = _todayExchangeCount;
	nextResetCountTimeMs = _nextResetCountTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 礼包组ID */
public long getGroupId() { return groupId; }
/** 礼包组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 当前礼包配置ID */
public long getRefId() { return refId; }
/** 当前礼包配置ID */
public void setRefId(long _refId) { refId = _refId; }
/** 当前兑换开始时间 如果没兑换影响刷新礼包时间 */
public long getActiveTimeMs() { return activeTimeMs; }
/** 当前兑换开始时间 如果没兑换影响刷新礼包时间 */
public void setActiveTimeMs(long _activeTimeMs) { activeTimeMs = _activeTimeMs; }
/** 兑换时间 影响什么时候可以领奖 */
public long getExchangeTimeMs() { return exchangeTimeMs; }
/** 兑换时间 影响什么时候可以领奖 */
public void setExchangeTimeMs(long _exchangeTimeMs) { exchangeTimeMs = _exchangeTimeMs; }
/** 是否已领奖 */
public boolean getIsRewarded() { return isRewarded; }
/** 是否已领奖 */
public void setIsRewarded(boolean _isRewarded) { isRewarded = _isRewarded; }
/** 当天兑换次数 */
public int getTodayExchangeCount() { return todayExchangeCount; }
/** 当天兑换次数 */
public void setTodayExchangeCount(int _todayExchangeCount) { todayExchangeCount = _todayExchangeCount; }
/** 下次刷新兑换次数时间 */
public long getNextResetCountTimeMs() { return nextResetCountTimeMs; }
/** 下次刷新兑换次数时间 */
public void setNextResetCountTimeMs(long _nextResetCountTimeMs) { nextResetCountTimeMs = _nextResetCountTimeMs; }


public final int GetBufSize() {
	int _size = 45;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 47;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) exchangeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isRewarded = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) todayExchangeCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextResetCountTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(refId);
	_buf.putLong(activeTimeMs);
	_buf.putLong(exchangeTimeMs);
	_buf.put(isRewarded?(byte)1:(byte)0);
	_buf.putInt(todayExchangeCount);
	_buf.putLong(nextResetCountTimeMs);
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

