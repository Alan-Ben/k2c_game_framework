package Common.BuildingObj;

import java.nio.ByteBuffer;
/*********
 * 农田暴击信息
 **/
public class Building_FarmMultipleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 暴击次数 */
private int critCount;
/** 上次刷新暴击次数时间 毫秒 */
private long lastRefreshTimeMs;
/** 点击次数 */
private long clickNum;
/** 随机种子 */
private long randomSeed;


public Building_FarmMultipleInfo() {
	critCount = 0;
	lastRefreshTimeMs = (long)0;
	clickNum = (long)0;
	randomSeed = (long)0;
}

public Building_FarmMultipleInfo(
	 int _critCount
	, long _lastRefreshTimeMs
	, long _clickNum
	, long _randomSeed
) {	critCount = _critCount;
	lastRefreshTimeMs = _lastRefreshTimeMs;
	clickNum = _clickNum;
	randomSeed = _randomSeed;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 暴击次数 */
public int getCritCount() { return critCount; }
/** 暴击次数 */
public void setCritCount(int _critCount) { critCount = _critCount; }
/** 上次刷新暴击次数时间 毫秒 */
public long getLastRefreshTimeMs() { return lastRefreshTimeMs; }
/** 上次刷新暴击次数时间 毫秒 */
public void setLastRefreshTimeMs(long _lastRefreshTimeMs) { lastRefreshTimeMs = _lastRefreshTimeMs; }
/** 点击次数 */
public long getClickNum() { return clickNum; }
/** 点击次数 */
public void setClickNum(long _clickNum) { clickNum = _clickNum; }
/** 随机种子 */
public long getRandomSeed() { return randomSeed; }
/** 随机种子 */
public void setRandomSeed(long _randomSeed) { randomSeed = _randomSeed; }


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
	if(_buf.remaining() > 0) critCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clickNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) randomSeed = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(critCount);
	_buf.putLong(lastRefreshTimeMs);
	_buf.putLong(clickNum);
	_buf.putLong(randomSeed);
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

