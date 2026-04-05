package Common.ConsortObj;

import java.nio.ByteBuffer;
/*********
 * 家人邀约结果
 **/
public class Consort_CallRes implements ALBasicProtocolPack._IALProtocolStructure {
/** 家人ID */
private long consortId;
/** 家人剧情ID */
private long consortStoryId;
/** 增加的加护点数 */
private long addCharmPoint;
/** 是否获取CG */
private boolean isGainCg;
/** 获得子嗣的实例ID，0-未获得 */
private long childId;


public Consort_CallRes() {
	consortId = (long)0;
	consortStoryId = (long)0;
	addCharmPoint = (long)0;
	isGainCg = false;
	childId = (long)0;
}

public Consort_CallRes(
	 long _consortId
	, long _consortStoryId
	, long _addCharmPoint
	, boolean _isGainCg
	, long _childId
) {	consortId = _consortId;
	consortStoryId = _consortStoryId;
	addCharmPoint = _addCharmPoint;
	isGainCg = _isGainCg;
	childId = _childId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 家人ID */
public long getConsortId() { return consortId; }
/** 家人ID */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 家人剧情ID */
public long getConsortStoryId() { return consortStoryId; }
/** 家人剧情ID */
public void setConsortStoryId(long _consortStoryId) { consortStoryId = _consortStoryId; }
/** 增加的加护点数 */
public long getAddCharmPoint() { return addCharmPoint; }
/** 增加的加护点数 */
public void setAddCharmPoint(long _addCharmPoint) { addCharmPoint = _addCharmPoint; }
/** 是否获取CG */
public boolean getIsGainCg() { return isGainCg; }
/** 是否获取CG */
public void setIsGainCg(boolean _isGainCg) { isGainCg = _isGainCg; }
/** 获得子嗣的实例ID，0-未获得 */
public long getChildId() { return childId; }
/** 获得子嗣的实例ID，0-未获得 */
public void setChildId(long _childId) { childId = _childId; }


public final int GetBufSize() {
	int _size = 33;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 35;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortStoryId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addCharmPoint = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isGainCg = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) childId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(consortStoryId);
	_buf.putLong(addCharmPoint);
	_buf.put(isGainCg?(byte)1:(byte)0);
	_buf.putLong(childId);
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

