package Common.CommonFuncObj;

import java.nio.ByteBuffer;
/*********
 * 订单信息
 **/
public class Order_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 订单号 */
private String orderId;
/** 支付id */
private long payId;
/** 礼包id */
private long giftPackId;
/** 订单状态 */
private CommonEnum.EOrderStatus status;
/** 创建时间 ms */
private long createTimeMs;
/** 支付时间 ms */
private long payTimeMs;


public Order_Info() {
	orderId = "";
	payId = (long)0;
	giftPackId = (long)0;
	status = CommonEnum.EOrderStatus.values()[0];
	createTimeMs = (long)0;
	payTimeMs = (long)0;
}

public Order_Info(
	 String _orderId
	, long _payId
	, long _giftPackId
	, CommonEnum.EOrderStatus _status
	, long _createTimeMs
	, long _payTimeMs
) {	orderId = _orderId;
	payId = _payId;
	giftPackId = _giftPackId;
	status = _status;
	createTimeMs = _createTimeMs;
	payTimeMs = _payTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 订单号 */
public String getOrderId() { return orderId; }
/** 订单号 */
public void setOrderId(String _orderId) { orderId = _orderId; }
/** 支付id */
public long getPayId() { return payId; }
/** 支付id */
public void setPayId(long _payId) { payId = _payId; }
/** 礼包id */
public long getGiftPackId() { return giftPackId; }
/** 礼包id */
public void setGiftPackId(long _giftPackId) { giftPackId = _giftPackId; }
/** 订单状态 */
public CommonEnum.EOrderStatus getStatus() { return status; }
/** 订单状态 */
public void setStatus(CommonEnum.EOrderStatus _status) { status = _status; }
/** 创建时间 ms */
public long getCreateTimeMs() { return createTimeMs; }
/** 创建时间 ms */
public void setCreateTimeMs(long _createTimeMs) { createTimeMs = _createTimeMs; }
/** 支付时间 ms */
public long getPayTimeMs() { return payTimeMs; }
/** 支付时间 ms */
public void setPayTimeMs(long _payTimeMs) { payTimeMs = _payTimeMs; }


public final int GetBufSize() {
	int _size = 36;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) orderId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) payId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) giftPackId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) status = CommonEnum.EOrderStatus.EOrderStatus_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) payTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, orderId);
	_buf.putLong(payId);
	_buf.putLong(giftPackId);
	_buf.putInt(status.ordinal());

	_buf.putLong(createTimeMs);
	_buf.putLong(payTimeMs);
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

