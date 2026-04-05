package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 大臣升级
 **/
public class GS2GC_013_050_OnHeroLevelChg implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
/** 当前等级 */
private int level;


public GS2GC_013_050_OnHeroLevelChg() {
	heroId = (long)0;
	level = 0;
}

public GS2GC_013_050_OnHeroLevelChg(
	 long _heroId
	, int _level
) {	heroId = _heroId;
	level = _level;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)50; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 当前等级 */
public int getLevel() { return level; }
/** 当前等级 */
public void setLevel(int _level) { level = _level; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) level = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putInt(level);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)50);
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

