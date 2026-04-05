package GC2GS.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 提升大臣皮肤等级
 **/
public class GC2GS_013_005_ReqHeroSkinUpgrade implements ALBasicProtocolPack._IALProtocolStructure {
/** 皮肤id */
private long skinId;


public GC2GS_013_005_ReqHeroSkinUpgrade() {
	skinId = (long)0;
}

public GC2GS_013_005_ReqHeroSkinUpgrade(
	 long _skinId
) {	skinId = _skinId;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)5; }

/** 皮肤id */
public long getSkinId() { return skinId; }
/** 皮肤id */
public void setSkinId(long _skinId) { skinId = _skinId; }


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
	if(_buf.remaining() > 0) skinId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(skinId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)5);
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

