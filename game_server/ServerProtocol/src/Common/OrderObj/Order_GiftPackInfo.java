package Common.OrderObj;

import java.nio.ByteBuffer;
/*********
 * 礼包订单信息
 **/
public class Order_GiftPackInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包id */
private long giftPackId;
/** 购买次数 */
private long buyTimes;
/** 下次刷新时间 ms */
private long nextRefreshTimeMs;


public Order_GiftPackInfo() {
	giftPackId = (long)0;
	buyTimes = (long)0;
	nextRefreshTimeMs = (long)0;
}

public Order_GiftPackInfo(
	 long _giftPackId
	, long _buyTimes
	, long _nextRefreshTimeMs
) {	giftPackId = _giftPackId;
	buyTimes = _buyTimes;
	nextRefreshTimeMs = _nextRefreshTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 礼包id */
public long getGiftPackId() { return giftPackId; }
/** 礼包id */
public void setGiftPackId(long _giftPackId) { giftPackId = _giftPackId; }
/** 购买次数 */
public long getBuyTimes() { return buyTimes; }
/** 购买次数 */
public void setBuyTimes(long _buyTimes) { buyTimes = _buyTimes; }
/** 下次刷新时间 ms */
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/** 下次刷新时间 ms */
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) giftPackId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buyTimes = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextRefreshTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(giftPackId);
	_buf.putLong(buyTimes);
	_buf.putLong(nextRefreshTimeMs);
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

