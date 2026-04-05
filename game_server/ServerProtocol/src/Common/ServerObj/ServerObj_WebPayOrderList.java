package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 网页支付订单列表
 **/
public class ServerObj_WebPayOrderList implements ALBasicProtocolPack._IALProtocolStructure {
/** 订单列表 */
private java.util.ArrayList<Common.ServerObj.ServerObj_WebPayOrderInfo> orderList;


public ServerObj_WebPayOrderList() {
	orderList = new java.util.ArrayList<Common.ServerObj.ServerObj_WebPayOrderInfo>();
}

public ServerObj_WebPayOrderList(
	 java.util.ArrayList<Common.ServerObj.ServerObj_WebPayOrderInfo> _orderList
) {	orderList = _orderList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 订单列表 */
public java.util.ArrayList<Common.ServerObj.ServerObj_WebPayOrderInfo> getOrderList() { return orderList; }
/** 订单列表 */
public void addOrderList(Common.ServerObj.ServerObj_WebPayOrderInfo _orderList) { orderList.add(_orderList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < orderList.size(); _i++) {
	_size += 4 + orderList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < orderList.size(); _i++) {
	_size += 4 + orderList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _orderListCount = _buf.getShort();
	for(int _i = 0; _i < _orderListCount; _i++) { 
		Common.ServerObj.ServerObj_WebPayOrderInfo _orderList = new Common.ServerObj.ServerObj_WebPayOrderInfo();
		if(_buf.remaining() <= 0) return;
	int __orderListCustLen = _buf.getInt();
	int __orderListCurPos = _buf.position();
	_orderList.ReadUnzipBuf(_buf, __orderListCurPos + __orderListCustLen);
	_buf.position(__orderListCurPos + __orderListCustLen);

		orderList.add(_orderList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)orderList.size());
	for(int _i = 0; _i < orderList.size(); _i++) { 
		_buf.putInt(orderList.get(_i).GetBufSize());
	orderList.get(_i).PutUnzipBuf(_buf);
	}
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

