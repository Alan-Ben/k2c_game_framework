package Common.GuildCooperateObj;

import java.nio.ByteBuffer;
/*********
 * 联盟协作大臣使用信息
 **/
public class GuildCooperate_HeroUseInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 上次刷新时间 ms */
private long lastRefreshTimeMs;
/** 大臣ID */
private long heroId;
/** 已使用次数 */
private int useCount;
/** 已恢复次数 */
private int recoveredCount;


public GuildCooperate_HeroUseInfo() {
	lastRefreshTimeMs = (long)0;
	heroId = (long)0;
	useCount = 0;
	recoveredCount = 0;
}

public GuildCooperate_HeroUseInfo(
	 long _lastRefreshTimeMs
	, long _heroId
	, int _useCount
	, int _recoveredCount
) {	lastRefreshTimeMs = _lastRefreshTimeMs;
	heroId = _heroId;
	useCount = _useCount;
	recoveredCount = _recoveredCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 上次刷新时间 ms */
public long getLastRefreshTimeMs() { return lastRefreshTimeMs; }
/** 上次刷新时间 ms */
public void setLastRefreshTimeMs(long _lastRefreshTimeMs) { lastRefreshTimeMs = _lastRefreshTimeMs; }
/** 大臣ID */
public long getHeroId() { return heroId; }
/** 大臣ID */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 已使用次数 */
public int getUseCount() { return useCount; }
/** 已使用次数 */
public void setUseCount(int _useCount) { useCount = _useCount; }
/** 已恢复次数 */
public int getRecoveredCount() { return recoveredCount; }
/** 已恢复次数 */
public void setRecoveredCount(int _recoveredCount) { recoveredCount = _recoveredCount; }


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
	if(_buf.remaining() > 0) lastRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) useCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) recoveredCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(lastRefreshTimeMs);
	_buf.putLong(heroId);
	_buf.putInt(useCount);
	_buf.putInt(recoveredCount);
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

