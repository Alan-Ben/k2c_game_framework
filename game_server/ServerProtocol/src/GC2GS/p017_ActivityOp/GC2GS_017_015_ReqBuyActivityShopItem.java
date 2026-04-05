package GC2GS.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 购买活动商店商品
 **/
public class GC2GS_017_015_ReqBuyActivityShopItem implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例id */
private long instanceId;
/** 商店id */
private long shopId;
/** 商品id */
private long itemId;
/** 购买数量 */
private int num;


public GC2GS_017_015_ReqBuyActivityShopItem() {
	instanceId = (long)0;
	shopId = (long)0;
	itemId = (long)0;
	num = 0;
}

public GC2GS_017_015_ReqBuyActivityShopItem(
	 long _instanceId
	, long _shopId
	, long _itemId
	, int _num
) {	instanceId = _instanceId;
	shopId = _shopId;
	itemId = _itemId;
	num = _num;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)15; }

/** 活动实例id */
public long getInstanceId() { return instanceId; }
/** 活动实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 商店id */
public long getShopId() { return shopId; }
/** 商店id */
public void setShopId(long _shopId) { shopId = _shopId; }
/** 商品id */
public long getItemId() { return itemId; }
/** 商品id */
public void setItemId(long _itemId) { itemId = _itemId; }
/** 购买数量 */
public int getNum() { return num; }
/** 购买数量 */
public void setNum(int _num) { num = _num; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shopId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(shopId);
	_buf.putLong(itemId);
	_buf.putInt(num);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)15);
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

