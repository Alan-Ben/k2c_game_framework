package GC2GS.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 升级大臣
 **/
public class GC2GS_013_001_ReqHeroUpgrade implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣id */
private long heroId;
/** 是否十连升级 */
private boolean isTen;


public GC2GS_013_001_ReqHeroUpgrade() {
	heroId = (long)0;
	isTen = false;
}

public GC2GS_013_001_ReqHeroUpgrade(
	 long _heroId
	, boolean _isTen
) {	heroId = _heroId;
	isTen = _isTen;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)1; }

/** 大臣id */
public long getHeroId() { return heroId; }
/** 大臣id */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 是否十连升级 */
public boolean getIsTen() { return isTen; }
/** 是否十连升级 */
public void setIsTen(boolean _isTen) { isTen = _isTen; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isTen = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.put(isTen?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)1);
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

