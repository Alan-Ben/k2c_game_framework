package Common.CommonFuncObj;

import java.nio.ByteBuffer;
/*********
 * 钻石礼包购买记录
 **/
public class CrystalGiftPack_BuyRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包id */
private long giftPackId;
/** 已购买数量 */
private long hadBuyCount;


public CrystalGiftPack_BuyRecord() {
	giftPackId = (long)0;
	hadBuyCount = (long)0;
}

public CrystalGiftPack_BuyRecord(
	 long _giftPackId
	, long _hadBuyCount
) {	giftPackId = _giftPackId;
	hadBuyCount = _hadBuyCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 礼包id */
public long getGiftPackId() { return giftPackId; }
/** 礼包id */
public void setGiftPackId(long _giftPackId) { giftPackId = _giftPackId; }
/** 已购买数量 */
public long getHadBuyCount() { return hadBuyCount; }
/** 已购买数量 */
public void setHadBuyCount(long _hadBuyCount) { hadBuyCount = _hadBuyCount; }


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
	if(_buf.remaining() > 0) giftPackId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadBuyCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(giftPackId);
	_buf.putLong(hadBuyCount);
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

