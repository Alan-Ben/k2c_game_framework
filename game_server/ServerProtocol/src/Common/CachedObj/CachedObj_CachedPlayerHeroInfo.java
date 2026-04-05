package Common.CachedObj;

import java.nio.ByteBuffer;
public class CachedObj_CachedPlayerHeroInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
private long skinId;
private long power;
private int level;


public CachedObj_CachedPlayerHeroInfo() {
	heroId = (long)0;
	skinId = (long)0;
	power = (long)0;
	level = 0;
}

public CachedObj_CachedPlayerHeroInfo(
	 long _heroId
	, long _skinId
	, long _power
	, int _level
) {	heroId = _heroId;
	skinId = _skinId;
	power = _power;
	level = _level;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public long getSkinId() { return skinId; }
public void setSkinId(long _skinId) { skinId = _skinId; }
public long getPower() { return power; }
public void setPower(long _power) { power = _power; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }


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
	if(_buf.remaining() > 0) power = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putLong(skinId);
	_buf.putLong(power);
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

