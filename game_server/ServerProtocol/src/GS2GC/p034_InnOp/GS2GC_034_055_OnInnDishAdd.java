package GS2GC.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店菜品信息新增
 **/
public class GS2GC_034_055_OnInnDishAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 菜品信息 */
private Common.InnObj.Inn_DishInfo dishInfo;


public GS2GC_034_055_OnInnDishAdd() {
	dishInfo = new Common.InnObj.Inn_DishInfo();
}

public GS2GC_034_055_OnInnDishAdd(
	 Common.InnObj.Inn_DishInfo _dishInfo
) {	dishInfo = _dishInfo;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)55; }

/** 菜品信息 */
public Common.InnObj.Inn_DishInfo getDishInfo() { return dishInfo; }
/** 菜品信息 */
public void setDishInfo(Common.InnObj.Inn_DishInfo _dishInfo) { dishInfo = _dishInfo; }


public final int GetBufSize() {
	int _size = 34;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 36;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _dishInfoCustLen = _buf.getInt();
	int _dishInfoCurPos = _buf.position();
	dishInfo.ReadUnzipBuf(_buf, _dishInfoCurPos + _dishInfoCustLen);
	_buf.position(_dishInfoCurPos + _dishInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(dishInfo.GetBufSize());
	dishInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)55);
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

