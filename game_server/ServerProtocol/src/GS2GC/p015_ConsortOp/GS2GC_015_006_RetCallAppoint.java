package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-指定邀约
 **/
public class GS2GC_015_006_RetCallAppoint implements ALBasicProtocolPack._IALProtocolStructure {
/** 增加的加护点数 */
private long addCharmPoint;
/** 获得子嗣的实例ID，0-未获得 */
private long childId;


public GS2GC_015_006_RetCallAppoint() {
	addCharmPoint = (long)0;
	childId = (long)0;
}

public GS2GC_015_006_RetCallAppoint(
	 long _addCharmPoint
	, long _childId
) {	addCharmPoint = _addCharmPoint;
	childId = _childId;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)6; }

/** 增加的加护点数 */
public long getAddCharmPoint() { return addCharmPoint; }
/** 增加的加护点数 */
public void setAddCharmPoint(long _addCharmPoint) { addCharmPoint = _addCharmPoint; }
/** 获得子嗣的实例ID，0-未获得 */
public long getChildId() { return childId; }
/** 获得子嗣的实例ID，0-未获得 */
public void setChildId(long _childId) { childId = _childId; }


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
	if(_buf.remaining() > 0) addCharmPoint = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) childId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(addCharmPoint);
	_buf.putLong(childId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)6);
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

