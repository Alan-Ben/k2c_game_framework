package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动商店商品购买记录推送
 **/
public class GS2GC_017_060_OnActivityShopItemBuy implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例id */
private long instanceId;
/** 商店id */
private long shopId;
/** 购买记录 */
private Common.ActivityObj.Activity_ShopBuyRecord buyRecord;


public GS2GC_017_060_OnActivityShopItemBuy() {
	instanceId = (long)0;
	shopId = (long)0;
	buyRecord = new Common.ActivityObj.Activity_ShopBuyRecord();
}

public GS2GC_017_060_OnActivityShopItemBuy(
	 long _instanceId
	, long _shopId
	, Common.ActivityObj.Activity_ShopBuyRecord _buyRecord
) {	instanceId = _instanceId;
	shopId = _shopId;
	buyRecord = _buyRecord;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)60; }

/** 活动实例id */
public long getInstanceId() { return instanceId; }
/** 活动实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 商店id */
public long getShopId() { return shopId; }
/** 商店id */
public void setShopId(long _shopId) { shopId = _shopId; }
/** 购买记录 */
public Common.ActivityObj.Activity_ShopBuyRecord getBuyRecord() { return buyRecord; }
/** 购买记录 */
public void setBuyRecord(Common.ActivityObj.Activity_ShopBuyRecord _buyRecord) { buyRecord = _buyRecord; }


public final int GetBufSize() {
	int _size = 36;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shopId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _buyRecordCustLen = _buf.getInt();
	int _buyRecordCurPos = _buf.position();
	buyRecord.ReadUnzipBuf(_buf, _buyRecordCurPos + _buyRecordCustLen);
	_buf.position(_buyRecordCurPos + _buyRecordCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(shopId);
	_buf.putInt(buyRecord.GetBufSize());
	buyRecord.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)60);
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

