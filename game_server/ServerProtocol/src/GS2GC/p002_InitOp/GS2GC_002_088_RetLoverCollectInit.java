package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_088_RetLoverCollectInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 当前选中的情人配置ID，0=未选择 */
private long targetLoverId;
/** 是否已领取当前目标情人 */
private boolean isClaimed;


public GS2GC_002_088_RetLoverCollectInit() {
	targetLoverId = (long)0;
	isClaimed = false;
}

public GS2GC_002_088_RetLoverCollectInit(
	 long _targetLoverId
	, boolean _isClaimed
) {	targetLoverId = _targetLoverId;
	isClaimed = _isClaimed;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)88; }

/** 当前选中的情人配置ID，0=未选择 */
public long getTargetLoverId() { return targetLoverId; }
/** 当前选中的情人配置ID，0=未选择 */
public void setTargetLoverId(long _targetLoverId) { targetLoverId = _targetLoverId; }
/** 是否已领取当前目标情人 */
public boolean getIsClaimed() { return isClaimed; }
/** 是否已领取当前目标情人 */
public void setIsClaimed(boolean _isClaimed) { isClaimed = _isClaimed; }


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
	if(_buf.remaining() > 0) targetLoverId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isClaimed = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(targetLoverId);
	_buf.put(isClaimed?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)88);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)88);
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

