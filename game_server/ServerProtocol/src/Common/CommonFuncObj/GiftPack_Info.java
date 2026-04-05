package Common.CommonFuncObj;

import java.nio.ByteBuffer;
/*********
 * 礼包信息
 **/
public class GiftPack_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包id */
private long giftPackId;
/** 下次刷新时间 */
private long nextRefreshTimeMs;
/** 已购买数量 */
private int hadBuyCount;


public GiftPack_Info() {
	giftPackId = (long)0;
	nextRefreshTimeMs = (long)0;
	hadBuyCount = 0;
}

public GiftPack_Info(
	 long _giftPackId
	, long _nextRefreshTimeMs
	, int _hadBuyCount
) {	giftPackId = _giftPackId;
	nextRefreshTimeMs = _nextRefreshTimeMs;
	hadBuyCount = _hadBuyCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 礼包id */
public long getGiftPackId() { return giftPackId; }
/** 礼包id */
public void setGiftPackId(long _giftPackId) { giftPackId = _giftPackId; }
/** 下次刷新时间 */
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/** 下次刷新时间 */
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }
/** 已购买数量 */
public int getHadBuyCount() { return hadBuyCount; }
/** 已购买数量 */
public void setHadBuyCount(int _hadBuyCount) { hadBuyCount = _hadBuyCount; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) giftPackId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadBuyCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(giftPackId);
	_buf.putLong(nextRefreshTimeMs);
	_buf.putInt(hadBuyCount);
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

