package Common;

import java.nio.ByteBuffer;
/*********
 * 红点信息
 **/
public class Common_RedDotInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 红点类型 */
private CommonEnum.ERedDotType redDotType;
/** 需要检查的时间点 */
private long needCheckTimeMs;


public Common_RedDotInfo() {
	redDotType = CommonEnum.ERedDotType.values()[0];
	needCheckTimeMs = (long)0;
}

public Common_RedDotInfo(
	 CommonEnum.ERedDotType _redDotType
	, long _needCheckTimeMs
) {	redDotType = _redDotType;
	needCheckTimeMs = _needCheckTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 红点类型 */
public CommonEnum.ERedDotType getRedDotType() { return redDotType; }
/** 红点类型 */
public void setRedDotType(CommonEnum.ERedDotType _redDotType) { redDotType = _redDotType; }
/** 需要检查的时间点 */
public long getNeedCheckTimeMs() { return needCheckTimeMs; }
/** 需要检查的时间点 */
public void setNeedCheckTimeMs(long _needCheckTimeMs) { needCheckTimeMs = _needCheckTimeMs; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) redDotType = CommonEnum.ERedDotType.ERedDotType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) needCheckTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(redDotType.ordinal());

	_buf.putLong(needCheckTimeMs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

