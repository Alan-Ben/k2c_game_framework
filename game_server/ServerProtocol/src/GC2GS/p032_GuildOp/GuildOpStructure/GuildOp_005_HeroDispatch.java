package GC2GS.p032_GuildOp.GuildOpStructure;

import java.nio.ByteBuffer;
/*********
 * 更换派遣大臣
 **/
public class GuildOp_005_HeroDispatch implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
private int dispatchValue;
private int level;
private long power;
private long skinId;


public GuildOp_005_HeroDispatch() {
	heroId = (long)0;
	dispatchValue = 0;
	level = 0;
	power = (long)0;
	skinId = (long)0;
}

public GuildOp_005_HeroDispatch(
	 long _heroId
	, int _dispatchValue
	, int _level
	, long _power
	, long _skinId
) {	heroId = _heroId;
	dispatchValue = _dispatchValue;
	level = _level;
	power = _power;
	skinId = _skinId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public int getDispatchValue() { return dispatchValue; }
public void setDispatchValue(int _dispatchValue) { dispatchValue = _dispatchValue; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public long getPower() { return power; }
public void setPower(long _power) { power = _power; }
public long getSkinId() { return skinId; }
public void setSkinId(long _skinId) { skinId = _skinId; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dispatchValue = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) power = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skinId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putInt(dispatchValue);
	_buf.putInt(level);
	_buf.putLong(power);
	_buf.putLong(skinId);
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

