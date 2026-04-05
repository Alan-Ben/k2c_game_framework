package Common.HeroObj;

import java.nio.ByteBuffer;
/*********
 * 大臣光环信息
 **/
public class Hero_HaloInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣id */
private long heroId;
/** 是否解锁 */
private boolean isUnlock;
/** 等级 */
private int level;


public Hero_HaloInfo() {
	heroId = (long)0;
	isUnlock = false;
	level = 0;
}

public Hero_HaloInfo(
	 long _heroId
	, boolean _isUnlock
	, int _level
) {	heroId = _heroId;
	isUnlock = _isUnlock;
	level = _level;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 是否解锁 */
public boolean getIsUnlock() { return isUnlock; }
/** 是否解锁 */
public void setIsUnlock(boolean _isUnlock) { isUnlock = _isUnlock; }
/** 等级 */
public int getLevel() { return level; }
/** 等级 */
public void setLevel(int _level) { level = _level; }


public final int GetBufSize() {
	int _size = 13;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isUnlock = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.put(isUnlock?(byte)1:(byte)0);
	_buf.putInt(level);
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

