package Common.HeroObj;

import java.nio.ByteBuffer;
/*********
 * 大臣竞技场展示信息
 **/
public class Hero_ArenaShowInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣id */
private long heroId;
/** 皮肤id */
private long skinId;
/** 等级 */
private int level;
/** 实力 */
private long power;


public Hero_ArenaShowInfo() {
	heroId = (long)0;
	skinId = (long)0;
	level = 0;
	power = (long)0;
}

public Hero_ArenaShowInfo(
	 long _heroId
	, long _skinId
	, int _level
	, long _power
) {	heroId = _heroId;
	skinId = _skinId;
	level = _level;
	power = _power;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 皮肤id */
public long getSkinId() { return skinId; }
/** 皮肤id */
public void setSkinId(long _skinId) { skinId = _skinId; }
/** 等级 */
public int getLevel() { return level; }
/** 等级 */
public void setLevel(int _level) { level = _level; }
/** 实力 */
public long getPower() { return power; }
/** 实力 */
public void setPower(long _power) { power = _power; }


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
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) power = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putLong(skinId);
	_buf.putInt(level);
	_buf.putLong(power);
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

