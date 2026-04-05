package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_060_PushRedDotRemove implements ALBasicProtocolPack._IALProtocolStructure {
/** 移除的红点类型 */
private CommonEnum.ERedDotType redDotType;


public GS2GC_007_060_PushRedDotRemove() {
	redDotType = CommonEnum.ERedDotType.values()[0];
}

public GS2GC_007_060_PushRedDotRemove(
	 CommonEnum.ERedDotType _redDotType
) {	redDotType = _redDotType;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)60; }

/** 移除的红点类型 */
public CommonEnum.ERedDotType getRedDotType() { return redDotType; }
/** 移除的红点类型 */
public void setRedDotType(CommonEnum.ERedDotType _redDotType) { redDotType = _redDotType; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) redDotType = CommonEnum.ERedDotType.ERedDotType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(redDotType.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)60);
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

