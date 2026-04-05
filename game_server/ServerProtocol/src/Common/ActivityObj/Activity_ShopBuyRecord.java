package Common.ActivityObj;

import java.nio.ByteBuffer;
/*********
 * 活动商店购买记录
 **/
public class Activity_ShopBuyRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 商品id */
private long itemId;
/** 已购买数量 */
private long hadBuyCount;


public Activity_ShopBuyRecord() {
	itemId = (long)0;
	hadBuyCount = (long)0;
}

public Activity_ShopBuyRecord(
	 long _itemId
	, long _hadBuyCount
) {	itemId = _itemId;
	hadBuyCount = _hadBuyCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 商品id */
public long getItemId() { return itemId; }
/** 商品id */
public void setItemId(long _itemId) { itemId = _itemId; }
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
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadBuyCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(itemId);
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

