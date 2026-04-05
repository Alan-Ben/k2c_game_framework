package GC2GS.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
/*********
 * 火星建筑-主建筑开关
 **/
public class GC2GS_039_003_ReqSetHonePowerOn implements ALBasicProtocolPack._IALProtocolStructure {
/** 普通功率开启 */
private boolean isNormalOn;
/** 最高功率开启 */
private boolean isOverdriveOn;


public GC2GS_039_003_ReqSetHonePowerOn() {
	isNormalOn = false;
	isOverdriveOn = false;
}

public GC2GS_039_003_ReqSetHonePowerOn(
	 boolean _isNormalOn
	, boolean _isOverdriveOn
) {	isNormalOn = _isNormalOn;
	isOverdriveOn = _isOverdriveOn;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)3; }

/** 普通功率开启 */
public boolean getIsNormalOn() { return isNormalOn; }
/** 普通功率开启 */
public void setIsNormalOn(boolean _isNormalOn) { isNormalOn = _isNormalOn; }
/** 最高功率开启 */
public boolean getIsOverdriveOn() { return isOverdriveOn; }
/** 最高功率开启 */
public void setIsOverdriveOn(boolean _isOverdriveOn) { isOverdriveOn = _isOverdriveOn; }


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
	if(_buf.remaining() > 0) isNormalOn = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOverdriveOn = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isNormalOn?(byte)1:(byte)0);
	_buf.put(isOverdriveOn?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)3);
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

