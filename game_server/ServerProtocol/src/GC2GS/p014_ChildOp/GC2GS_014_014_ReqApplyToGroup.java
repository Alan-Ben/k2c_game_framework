package GC2GS.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 联姻请求-对指定群体发起请求
 **/
public class GC2GS_014_014_ReqApplyToGroup implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣实例ID */
private long adultId;
/** 最小比例最终值 */
private long minValue;


public GC2GS_014_014_ReqApplyToGroup() {
	adultId = (long)0;
	minValue = (long)0;
}

public GC2GS_014_014_ReqApplyToGroup(
	 long _adultId
	, long _minValue
) {	adultId = _adultId;
	minValue = _minValue;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)14; }

/** 子嗣实例ID */
public long getAdultId() { return adultId; }
/** 子嗣实例ID */
public void setAdultId(long _adultId) { adultId = _adultId; }
/** 最小比例最终值 */
public long getMinValue() { return minValue; }
/** 最小比例最终值 */
public void setMinValue(long _minValue) { minValue = _minValue; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) adultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) minValue = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(adultId);
	_buf.putLong(minValue);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)14);
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

