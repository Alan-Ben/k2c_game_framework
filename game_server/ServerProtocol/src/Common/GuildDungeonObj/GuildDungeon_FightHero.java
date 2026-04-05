package Common.GuildDungeonObj;

import java.nio.ByteBuffer;
/*********
 * 出战大臣数据
 **/
public class GuildDungeon_FightHero implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
/** 出战次数 */
private int fightedCount;
/** 已恢复次数 */
private int recoveredCount;
/** 最后一次出战时间（毫秒） */
private long lastFightedMs;


public GuildDungeon_FightHero() {
	heroId = (long)0;
	fightedCount = 0;
	recoveredCount = 0;
	lastFightedMs = (long)0;
}

public GuildDungeon_FightHero(
	 long _heroId
	, int _fightedCount
	, int _recoveredCount
	, long _lastFightedMs
) {	heroId = _heroId;
	fightedCount = _fightedCount;
	recoveredCount = _recoveredCount;
	lastFightedMs = _lastFightedMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 出战次数 */
public int getFightedCount() { return fightedCount; }
/** 出战次数 */
public void setFightedCount(int _fightedCount) { fightedCount = _fightedCount; }
/** 已恢复次数 */
public int getRecoveredCount() { return recoveredCount; }
/** 已恢复次数 */
public void setRecoveredCount(int _recoveredCount) { recoveredCount = _recoveredCount; }
/** 最后一次出战时间（毫秒） */
public long getLastFightedMs() { return lastFightedMs; }
/** 最后一次出战时间（毫秒） */
public void setLastFightedMs(long _lastFightedMs) { lastFightedMs = _lastFightedMs; }


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
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fightedCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) recoveredCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastFightedMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putInt(fightedCount);
	_buf.putInt(recoveredCount);
	_buf.putLong(lastFightedMs);
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

