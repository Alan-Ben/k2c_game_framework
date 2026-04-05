package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 网页支付订单信息
 **/
public class ServerObj_WebPayOrderInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 订单号 */
private String orderId;
/** 商品ID */
private long goodsId;
/** sdk档位ID */
private String sdkPayId;
/** 价格 */
private float amount;


public ServerObj_WebPayOrderInfo() {
	orderId = "";
	goodsId = (long)0;
	sdkPayId = "";
	amount = 0f;
}

public ServerObj_WebPayOrderInfo(
	 String _orderId
	, long _goodsId
	, String _sdkPayId
	, float _amount
) {	orderId = _orderId;
	goodsId = _goodsId;
	sdkPayId = _sdkPayId;
	amount = _amount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 订单号 */
public String getOrderId() { return orderId; }
/** 订单号 */
public void setOrderId(String _orderId) { orderId = _orderId; }
/** 商品ID */
public long getGoodsId() { return goodsId; }
/** 商品ID */
public void setGoodsId(long _goodsId) { goodsId = _goodsId; }
/** sdk档位ID */
public String getSdkPayId() { return sdkPayId; }
/** sdk档位ID */
public void setSdkPayId(String _sdkPayId) { sdkPayId = _sdkPayId; }
/** 价格 */
public float getAmount() { return amount; }
/** 价格 */
public void setAmount(float _amount) { amount = _amount; }


public final int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkPayId);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkPayId);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) orderId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) goodsId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sdkPayId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) amount = _buf.getFloat();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, orderId);
	_buf.putLong(goodsId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, sdkPayId);
	_buf.putFloat(amount);
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

