package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_068_RetGraveInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否可领奖新晋杰出者 */
private boolean hasGraveNewReward;
/** 是否有杰出者记录 */
private boolean hasGraveRecord;


public GS2GC_002_068_RetGraveInit() {
	hasGraveNewReward = false;
	hasGraveRecord = false;
}

public GS2GC_002_068_RetGraveInit(
	 boolean _hasGraveNewReward
	, boolean _hasGraveRecord
) {	hasGraveNewReward = _hasGraveNewReward;
	hasGraveRecord = _hasGraveRecord;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)68; }

/** 是否可领奖新晋杰出者 */
public boolean getHasGraveNewReward() { return hasGraveNewReward; }
/** 是否可领奖新晋杰出者 */
public void setHasGraveNewReward(boolean _hasGraveNewReward) { hasGraveNewReward = _hasGraveNewReward; }
/** 是否有杰出者记录 */
public boolean getHasGraveRecord() { return hasGraveRecord; }
/** 是否有杰出者记录 */
public void setHasGraveRecord(boolean _hasGraveRecord) { hasGraveRecord = _hasGraveRecord; }


public final int GetBufSize() {
	int _size = 2;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 4;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasGraveNewReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasGraveRecord = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(hasGraveNewReward?(byte)1:(byte)0);
	_buf.put(hasGraveRecord?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)68);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)68);
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

