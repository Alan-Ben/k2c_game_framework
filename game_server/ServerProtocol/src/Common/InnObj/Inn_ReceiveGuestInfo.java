package Common.InnObj;

import java.nio.ByteBuffer;
/*********
 * 旅店_接待客人信息
 **/
public class Inn_ReceiveGuestInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 队列中的索引 */
private int index;
/** 客人ID */
private long guestId;
/** 菜品ID */
private long dishId;


public Inn_ReceiveGuestInfo() {
	index = 0;
	guestId = (long)0;
	dishId = (long)0;
}

public Inn_ReceiveGuestInfo(
	 int _index
	, long _guestId
	, long _dishId
) {	index = _index;
	guestId = _guestId;
	dishId = _dishId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队列中的索引 */
public int getIndex() { return index; }
/** 队列中的索引 */
public void setIndex(int _index) { index = _index; }
/** 客人ID */
public long getGuestId() { return guestId; }
/** 客人ID */
public void setGuestId(long _guestId) { guestId = _guestId; }
/** 菜品ID */
public long getDishId() { return dishId; }
/** 菜品ID */
public void setDishId(long _dishId) { dishId = _dishId; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) index = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guestId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dishId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(index);
	_buf.putLong(guestId);
	_buf.putLong(dishId);
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

