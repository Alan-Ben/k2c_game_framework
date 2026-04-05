package Common.CommonFuncObj;

import java.nio.ByteBuffer;
/*********
 * 目标奖励信息
 **/
public class CommonFunc_TargetReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标id */
private long id;
/** 计数 */
private long value;
/** 是否领取 */
private boolean hadDraw;


public CommonFunc_TargetReward() {
	id = (long)0;
	value = (long)0;
	hadDraw = false;
}

public CommonFunc_TargetReward(
	 long _id
	, long _value
	, boolean _hadDraw
) {	id = _id;
	value = _value;
	hadDraw = _hadDraw;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 目标id */
public long getId() { return id; }
/** 目标id */
public void setId(long _id) { id = _id; }
/** 计数 */
public long getValue() { return value; }
/** 计数 */
public void setValue(long _value) { value = _value; }
/** 是否领取 */
public boolean getHadDraw() { return hadDraw; }
/** 是否领取 */
public void setHadDraw(boolean _hadDraw) { hadDraw = _hadDraw; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDraw = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(value);
	_buf.put(hadDraw?(byte)1:(byte)0);
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

