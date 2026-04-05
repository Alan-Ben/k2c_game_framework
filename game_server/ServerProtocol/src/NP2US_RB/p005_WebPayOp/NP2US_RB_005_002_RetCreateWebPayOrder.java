package NP2US_RB.p005_WebPayOp;

import java.nio.ByteBuffer;
/*********
 * 返回网页支付订单创建结果
 **/
public class NP2US_RB_005_002_RetCreateWebPayOrder implements ALBasicProtocolPack._IALProtocolStructure {
/** 错误码,0表示成功 */
private int errCode;
/** 用户ID */
private String uid;
/** 订单列表 */
private Common.ServerObj.ServerObj_WebPayOrderList orderList;
/** 失败的商品ID列表 */
private java.util.ArrayList<Long> failedGoodsIds;


public NP2US_RB_005_002_RetCreateWebPayOrder() {
	errCode = 0;
	uid = "";
	orderList = new Common.ServerObj.ServerObj_WebPayOrderList();
	failedGoodsIds = new java.util.ArrayList<Long>();
}

public NP2US_RB_005_002_RetCreateWebPayOrder(
	 int _errCode
	, String _uid
	, Common.ServerObj.ServerObj_WebPayOrderList _orderList
	, java.util.ArrayList<Long> _failedGoodsIds
) {	errCode = _errCode;
	uid = _uid;
	orderList = _orderList;
	failedGoodsIds = _failedGoodsIds;
}

public final byte getMainOrder() { return (byte)5; }

public final byte getSubOrder() { return (byte)2; }

/** 错误码,0表示成功 */
public int getErrCode() { return errCode; }
/** 错误码,0表示成功 */
public void setErrCode(int _errCode) { errCode = _errCode; }
/** 用户ID */
public String getUid() { return uid; }
/** 用户ID */
public void setUid(String _uid) { uid = _uid; }
/** 订单列表 */
public Common.ServerObj.ServerObj_WebPayOrderList getOrderList() { return orderList; }
/** 订单列表 */
public void setOrderList(Common.ServerObj.ServerObj_WebPayOrderList _orderList) { orderList = _orderList; }
/** 失败的商品ID列表 */
public java.util.ArrayList<Long> getFailedGoodsIds() { return failedGoodsIds; }
/** 失败的商品ID列表 */
public void addFailedGoodsIds(long _failedGoodsIds) { failedGoodsIds.add(_failedGoodsIds); }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += 4 + orderList.GetBufSize();
	_size += 2 + (failedGoodsIds.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += 4 + orderList.GetBufSize();
	_size += 2 + (failedGoodsIds.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _orderListCustLen = _buf.getInt();
	int _orderListCurPos = _buf.position();
	orderList.ReadUnzipBuf(_buf, _orderListCurPos + _orderListCustLen);
	_buf.position(_orderListCurPos + _orderListCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _failedGoodsIdsCount = _buf.getShort();
	for(int _i = 0; _i < _failedGoodsIdsCount; _i++) { 
		long _failedGoodsIds = (long)0;
		if(_buf.remaining() > 0) _failedGoodsIds = _buf.getLong();
		failedGoodsIds.add(_failedGoodsIds);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(errCode);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, uid);
	_buf.putInt(orderList.GetBufSize());
	orderList.PutUnzipBuf(_buf);
	_buf.putShort((short)failedGoodsIds.size());
	for(int _i = 0; _i < failedGoodsIds.size(); _i++) { 
		_buf.putLong(failedGoodsIds.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)5);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)5);
	_recBuf.put((byte)2);
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

