package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 玩家CD数据
 **/
public class NPCommon_PlayerLazyCD implements ALBasicProtocolPack._IALProtocolStructure {
/** CD唯一ID */
private int cdId;
/** 上次count变动时间（毫秒） */
private long lastCalTimeMS;
/** 当前数量 */
private int count;
/** 最大数量 */
private int maxCount;
/** 每次恢复点数 */
private int addCountPerTime;
/** CD时长 */
private int cdDurationMs;
/** 上次有效时长和实际计算时长之间的差额时长 */
private long fullGetNextCdRemainTimeMs;
/** 关联的活动实例ID */
private long relativeActivityInstanceId;


public NPCommon_PlayerLazyCD() {
	cdId = 0;
	lastCalTimeMS = (long)0;
	count = 0;
	maxCount = 0;
	addCountPerTime = 0;
	cdDurationMs = 0;
	fullGetNextCdRemainTimeMs = (long)0;
	relativeActivityInstanceId = (long)0;
}

public NPCommon_PlayerLazyCD(
	 int _cdId
	, long _lastCalTimeMS
	, int _count
	, int _maxCount
	, int _addCountPerTime
	, int _cdDurationMs
	, long _fullGetNextCdRemainTimeMs
	, long _relativeActivityInstanceId
) {	cdId = _cdId;
	lastCalTimeMS = _lastCalTimeMS;
	count = _count;
	maxCount = _maxCount;
	addCountPerTime = _addCountPerTime;
	cdDurationMs = _cdDurationMs;
	fullGetNextCdRemainTimeMs = _fullGetNextCdRemainTimeMs;
	relativeActivityInstanceId = _relativeActivityInstanceId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** CD唯一ID */
public int getCdId() { return cdId; }
/** CD唯一ID */
public void setCdId(int _cdId) { cdId = _cdId; }
/** 上次count变动时间（毫秒） */
public long getLastCalTimeMS() { return lastCalTimeMS; }
/** 上次count变动时间（毫秒） */
public void setLastCalTimeMS(long _lastCalTimeMS) { lastCalTimeMS = _lastCalTimeMS; }
/** 当前数量 */
public int getCount() { return count; }
/** 当前数量 */
public void setCount(int _count) { count = _count; }
/** 最大数量 */
public int getMaxCount() { return maxCount; }
/** 最大数量 */
public void setMaxCount(int _maxCount) { maxCount = _maxCount; }
/** 每次恢复点数 */
public int getAddCountPerTime() { return addCountPerTime; }
/** 每次恢复点数 */
public void setAddCountPerTime(int _addCountPerTime) { addCountPerTime = _addCountPerTime; }
/** CD时长 */
public int getCdDurationMs() { return cdDurationMs; }
/** CD时长 */
public void setCdDurationMs(int _cdDurationMs) { cdDurationMs = _cdDurationMs; }
/** 上次有效时长和实际计算时长之间的差额时长 */
public long getFullGetNextCdRemainTimeMs() { return fullGetNextCdRemainTimeMs; }
/** 上次有效时长和实际计算时长之间的差额时长 */
public void setFullGetNextCdRemainTimeMs(long _fullGetNextCdRemainTimeMs) { fullGetNextCdRemainTimeMs = _fullGetNextCdRemainTimeMs; }
/** 关联的活动实例ID */
public long getRelativeActivityInstanceId() { return relativeActivityInstanceId; }
/** 关联的活动实例ID */
public void setRelativeActivityInstanceId(long _relativeActivityInstanceId) { relativeActivityInstanceId = _relativeActivityInstanceId; }


public final int GetBufSize() {
	int _size = 44;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 46;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cdId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastCalTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addCountPerTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cdDurationMs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fullGetNextCdRemainTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) relativeActivityInstanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(cdId);
	_buf.putLong(lastCalTimeMS);
	_buf.putInt(count);
	_buf.putInt(maxCount);
	_buf.putInt(addCountPerTime);
	_buf.putInt(cdDurationMs);
	_buf.putLong(fullGetNextCdRemainTimeMs);
	_buf.putLong(relativeActivityInstanceId);
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

