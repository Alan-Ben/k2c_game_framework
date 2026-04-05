package Common.InnObj;

import java.nio.ByteBuffer;
/*********
 * 旅店_菜品结算信息
 **/
public class Inn_DishSettleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 菜品ID */
private long dishId;
/** 接待数量 */
private int receiveNum;
/** 获得熟练度 */
private long addFinesse;


public Inn_DishSettleInfo() {
	dishId = (long)0;
	receiveNum = 0;
	addFinesse = (long)0;
}

public Inn_DishSettleInfo(
	 long _dishId
	, int _receiveNum
	, long _addFinesse
) {	dishId = _dishId;
	receiveNum = _receiveNum;
	addFinesse = _addFinesse;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 菜品ID */
public long getDishId() { return dishId; }
/** 菜品ID */
public void setDishId(long _dishId) { dishId = _dishId; }
/** 接待数量 */
public int getReceiveNum() { return receiveNum; }
/** 接待数量 */
public void setReceiveNum(int _receiveNum) { receiveNum = _receiveNum; }
/** 获得熟练度 */
public long getAddFinesse() { return addFinesse; }
/** 获得熟练度 */
public void setAddFinesse(long _addFinesse) { addFinesse = _addFinesse; }


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
	if(_buf.remaining() > 0) dishId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) receiveNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addFinesse = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dishId);
	_buf.putInt(receiveNum);
	_buf.putLong(addFinesse);
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

