package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 大臣游历额外加成实力推送
 **/
public class GS2GC_013_069_OnHeroTravelAddPowerChg implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
private long value;


public GS2GC_013_069_OnHeroTravelAddPowerChg() {
	heroId = (long)0;
	value = (long)0;
}

public GS2GC_013_069_OnHeroTravelAddPowerChg(
	 long _heroId
	, long _value
) {	heroId = _heroId;
	value = _value;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)69; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public long getValue() { return value; }
public void setValue(long _value) { value = _value; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putLong(value);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)69);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)69);
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

