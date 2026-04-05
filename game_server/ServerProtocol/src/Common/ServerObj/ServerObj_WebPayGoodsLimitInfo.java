package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 网页支付商品限购信息
 **/
public class ServerObj_WebPayGoodsLimitInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 最大购买数量 */
private int maxPurchase;
/** 已购买数量 */
private int purchased;
/** 限购结束时间（10位时间戳） */
private long endTime;


public ServerObj_WebPayGoodsLimitInfo() {
	maxPurchase = 0;
	purchased = 0;
	endTime = (long)0;
}

public ServerObj_WebPayGoodsLimitInfo(
	 int _maxPurchase
	, int _purchased
	, long _endTime
) {	maxPurchase = _maxPurchase;
	purchased = _purchased;
	endTime = _endTime;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 最大购买数量 */
public int getMaxPurchase() { return maxPurchase; }
/** 最大购买数量 */
public void setMaxPurchase(int _maxPurchase) { maxPurchase = _maxPurchase; }
/** 已购买数量 */
public int getPurchased() { return purchased; }
/** 已购买数量 */
public void setPurchased(int _purchased) { purchased = _purchased; }
/** 限购结束时间（10位时间戳） */
public long getEndTime() { return endTime; }
/** 限购结束时间（10位时间戳） */
public void setEndTime(long _endTime) { endTime = _endTime; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxPurchase = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) purchased = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTime = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(maxPurchase);
	_buf.putInt(purchased);
	_buf.putLong(endTime);
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

