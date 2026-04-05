package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 订单支付简要信息
 **/
public class ServerObj_OrderPaySimpleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 支付金额 */
private float money;
/** 订单ID */
private String orderId;
/** sdk档位ID */
private String sdkPayId;


public ServerObj_OrderPaySimpleInfo() {
	money = 0f;
	orderId = "";
	sdkPayId = "";
}

public ServerObj_OrderPaySimpleInfo(
	 float _money
	, String _orderId
	, String _sdkPayId
) {	money = _money;
	orderId = _orderId;
	sdkPayId = _sdkPayId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 支付金额 */
public float getMoney() { return money; }
/** 支付金额 */
public void setMoney(float _money) { money = _money; }
/** 订单ID */
public String getOrderId() { return orderId; }
/** 订单ID */
public void setOrderId(String _orderId) { orderId = _orderId; }
/** sdk档位ID */
public String getSdkPayId() { return sdkPayId; }
/** sdk档位ID */
public void setSdkPayId(String _sdkPayId) { sdkPayId = _sdkPayId; }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkPayId);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(sdkPayId);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) money = _buf.getFloat();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) orderId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sdkPayId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putFloat(money);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, orderId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, sdkPayId);
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

