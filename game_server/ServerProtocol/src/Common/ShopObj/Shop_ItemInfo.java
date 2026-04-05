package Common.ShopObj;

import java.nio.ByteBuffer;
/*********
 * 商店商品数据
 **/
public class Shop_ItemInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 商品id */
private long instanceId;
/** 商店商品配置id */
private long shopItemRefId;
/** 已购买次数 */
private long hasBuyNum;
/** 折扣配置id */
private long discountRefId;
/** 商品组id */
private long shopItemGroupId;
/** 限购次数 */
private long canBuyNum;


public Shop_ItemInfo() {
	instanceId = (long)0;
	shopItemRefId = (long)0;
	hasBuyNum = (long)0;
	discountRefId = (long)0;
	shopItemGroupId = (long)0;
	canBuyNum = (long)0;
}

public Shop_ItemInfo(
	 long _instanceId
	, long _shopItemRefId
	, long _hasBuyNum
	, long _discountRefId
	, long _shopItemGroupId
	, long _canBuyNum
) {	instanceId = _instanceId;
	shopItemRefId = _shopItemRefId;
	hasBuyNum = _hasBuyNum;
	discountRefId = _discountRefId;
	shopItemGroupId = _shopItemGroupId;
	canBuyNum = _canBuyNum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 商品id */
public long getInstanceId() { return instanceId; }
/** 商品id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 商店商品配置id */
public long getShopItemRefId() { return shopItemRefId; }
/** 商店商品配置id */
public void setShopItemRefId(long _shopItemRefId) { shopItemRefId = _shopItemRefId; }
/** 已购买次数 */
public long getHasBuyNum() { return hasBuyNum; }
/** 已购买次数 */
public void setHasBuyNum(long _hasBuyNum) { hasBuyNum = _hasBuyNum; }
/** 折扣配置id */
public long getDiscountRefId() { return discountRefId; }
/** 折扣配置id */
public void setDiscountRefId(long _discountRefId) { discountRefId = _discountRefId; }
/** 商品组id */
public long getShopItemGroupId() { return shopItemGroupId; }
/** 商品组id */
public void setShopItemGroupId(long _shopItemGroupId) { shopItemGroupId = _shopItemGroupId; }
/** 限购次数 */
public long getCanBuyNum() { return canBuyNum; }
/** 限购次数 */
public void setCanBuyNum(long _canBuyNum) { canBuyNum = _canBuyNum; }


public final int GetBufSize() {
	int _size = 48;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 50;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shopItemRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasBuyNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) discountRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shopItemGroupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) canBuyNum = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(shopItemRefId);
	_buf.putLong(hasBuyNum);
	_buf.putLong(discountRefId);
	_buf.putLong(shopItemGroupId);
	_buf.putLong(canBuyNum);
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

