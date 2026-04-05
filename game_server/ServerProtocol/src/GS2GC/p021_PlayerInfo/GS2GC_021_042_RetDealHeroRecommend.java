package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GS2GC_021_042_RetDealHeroRecommend implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;


public GS2GC_021_042_RetDealHeroRecommend() {
	heroId = (long)0;
}

public GS2GC_021_042_RetDealHeroRecommend(
	 long _heroId
) {	heroId = _heroId;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)42; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)42);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)42);
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

