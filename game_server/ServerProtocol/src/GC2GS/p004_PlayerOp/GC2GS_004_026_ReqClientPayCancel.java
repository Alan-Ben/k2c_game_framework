package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 客户端通知支付取消
 **/
public class GC2GS_004_026_ReqClientPayCancel implements ALBasicProtocolPack._IALProtocolStructure {
/** 订单ID */
private String orderId;


public GC2GS_004_026_ReqClientPayCancel() {
	orderId = "";
}

public GC2GS_004_026_ReqClientPayCancel(
	 String _orderId
) {	orderId = _orderId;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)26; }

/** 订单ID */
public String getOrderId() { return orderId; }
/** 订单ID */
public void setOrderId(String _orderId) { orderId = _orderId; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(orderId);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) orderId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, orderId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)26);
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

