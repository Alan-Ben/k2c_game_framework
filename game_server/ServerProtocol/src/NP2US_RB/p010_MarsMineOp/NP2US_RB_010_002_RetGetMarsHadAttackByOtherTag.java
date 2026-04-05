package NP2US_RB.p010_MarsMineOp;

import java.nio.ByteBuffer;
public class NP2US_RB_010_002_RetGetMarsHadAttackByOtherTag implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否被其他公会攻击过 */
private boolean hasAttacked;


public NP2US_RB_010_002_RetGetMarsHadAttackByOtherTag() {
	hasAttacked = false;
}

public NP2US_RB_010_002_RetGetMarsHadAttackByOtherTag(
	 boolean _hasAttacked
) {	hasAttacked = _hasAttacked;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)2; }

/** 是否被其他公会攻击过 */
public boolean getHasAttacked() { return hasAttacked; }
/** 是否被其他公会攻击过 */
public void setHasAttacked(boolean _hasAttacked) { hasAttacked = _hasAttacked; }


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
	if(_buf.remaining() > 0) hasAttacked = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(hasAttacked?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)2);
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

