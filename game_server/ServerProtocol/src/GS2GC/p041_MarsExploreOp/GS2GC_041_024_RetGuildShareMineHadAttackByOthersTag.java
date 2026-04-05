package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
public class GS2GC_041_024_RetGuildShareMineHadAttackByOthersTag implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否被其他人攻击标识 */
private boolean hadAttackByOthersTag;


public GS2GC_041_024_RetGuildShareMineHadAttackByOthersTag() {
	hadAttackByOthersTag = false;
}

public GS2GC_041_024_RetGuildShareMineHadAttackByOthersTag(
	 boolean _hadAttackByOthersTag
) {	hadAttackByOthersTag = _hadAttackByOthersTag;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)24; }

/** 是否被其他人攻击标识 */
public boolean getHadAttackByOthersTag() { return hadAttackByOthersTag; }
/** 是否被其他人攻击标识 */
public void setHadAttackByOthersTag(boolean _hadAttackByOthersTag) { hadAttackByOthersTag = _hadAttackByOthersTag; }


public final int GetBufSize() {
	int _size = 1;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadAttackByOthersTag = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(hadAttackByOthersTag?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)24);
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

