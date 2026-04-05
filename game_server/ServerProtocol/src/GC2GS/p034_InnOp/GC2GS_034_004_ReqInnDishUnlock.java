package GC2GS.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店菜品解锁
 **/
public class GC2GS_034_004_ReqInnDishUnlock implements ALBasicProtocolPack._IALProtocolStructure {
/** 菜品ID */
private long dishId;


public GC2GS_034_004_ReqInnDishUnlock() {
	dishId = (long)0;
}

public GC2GS_034_004_ReqInnDishUnlock(
	 long _dishId
) {	dishId = _dishId;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)4; }

/** 菜品ID */
public long getDishId() { return dishId; }
/** 菜品ID */
public void setDishId(long _dishId) { dishId = _dishId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dishId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dishId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)4);
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

