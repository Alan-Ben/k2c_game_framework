package GC2GS.p024_DungeonOp;

import java.nio.ByteBuffer;
/*********
 * 午间副本攻击
 **/
public class GC2GS_024_001_ReqMiddayDungeonAttack implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣ID */
private long heroId;


public GC2GS_024_001_ReqMiddayDungeonAttack() {
	heroId = (long)0;
}

public GC2GS_024_001_ReqMiddayDungeonAttack(
	 long _heroId
) {	heroId = _heroId;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)1; }

/** 大臣ID */
public long getHeroId() { return heroId; }
/** 大臣ID */
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
	_buf.put((byte)24);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
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

