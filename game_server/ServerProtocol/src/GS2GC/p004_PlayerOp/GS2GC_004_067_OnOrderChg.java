package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_067_OnOrderChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 订单信息 */
private Common.CommonFuncObj.Order_Info orderInfo;


public GS2GC_004_067_OnOrderChg() {
	orderInfo = new Common.CommonFuncObj.Order_Info();
}

public GS2GC_004_067_OnOrderChg(
	 Common.CommonFuncObj.Order_Info _orderInfo
) {	orderInfo = _orderInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)67; }

/** 订单信息 */
public Common.CommonFuncObj.Order_Info getOrderInfo() { return orderInfo; }
/** 订单信息 */
public void setOrderInfo(Common.CommonFuncObj.Order_Info _orderInfo) { orderInfo = _orderInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + orderInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + orderInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _orderInfoCustLen = _buf.getInt();
	int _orderInfoCurPos = _buf.position();
	orderInfo.ReadUnzipBuf(_buf, _orderInfoCurPos + _orderInfoCustLen);
	_buf.position(_orderInfoCurPos + _orderInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(orderInfo.GetBufSize());
	orderInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)67);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)67);
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

