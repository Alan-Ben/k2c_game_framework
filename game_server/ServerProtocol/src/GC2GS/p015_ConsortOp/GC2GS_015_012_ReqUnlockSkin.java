package GC2GS.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-解锁皮肤
 **/
public class GC2GS_015_012_ReqUnlockSkin implements ALBasicProtocolPack._IALProtocolStructure {
/** 皮肤ID */
private long skinId;


public GC2GS_015_012_ReqUnlockSkin() {
	skinId = (long)0;
}

public GC2GS_015_012_ReqUnlockSkin(
	 long _skinId
) {	skinId = _skinId;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)12; }

/** 皮肤ID */
public long getSkinId() { return skinId; }
/** 皮肤ID */
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
	_buf.put((byte)15);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)12);
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

