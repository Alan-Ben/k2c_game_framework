package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 红点-检查红点状态请求
 **/
public class GC2GS_007_032_ReqCheckRedDot implements ALBasicProtocolPack._IALProtocolStructure {
/** 要检查的红点类型 */
private CommonEnum.ERedDotType redDotType;


public GC2GS_007_032_ReqCheckRedDot() {
	redDotType = CommonEnum.ERedDotType.values()[0];
}

public GC2GS_007_032_ReqCheckRedDot(
	 CommonEnum.ERedDotType _redDotType
) {	redDotType = _redDotType;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)32; }

/** 要检查的红点类型 */
public CommonEnum.ERedDotType getRedDotType() { return redDotType; }
/** 要检查的红点类型 */
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
	_buf.put((byte)32);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)32);
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

