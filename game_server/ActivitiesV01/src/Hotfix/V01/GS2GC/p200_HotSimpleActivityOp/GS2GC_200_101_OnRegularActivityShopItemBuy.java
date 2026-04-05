package Hotfix.V01.GS2GC.p200_HotSimpleActivityOp;

import java.nio.ByteBuffer;
/*********
 * 万能活动商店购买物品推送
 **/
public class GS2GC_200_101_OnRegularActivityShopItemBuy implements ALBasicProtocolPack._IALProtocolStructure {
private long activityInstanceId;
/** 购买记录 */
private Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord buyRecord;


public GS2GC_200_101_OnRegularActivityShopItemBuy() {
	activityInstanceId = (long)0;
	buyRecord = new Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord();
}

public GS2GC_200_101_OnRegularActivityShopItemBuy(
	 long _activityInstanceId
	, Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord _buyRecord
) {	activityInstanceId = _activityInstanceId;
	buyRecord = _buyRecord;
}

public final byte getMainOrder() { return (byte)200; }

public final byte getSubOrder() { return (byte)101; }

public long getActivityInstanceId() { return activityInstanceId; }
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/** 购买记录 */
public Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord getBuyRecord() { return buyRecord; }
/** 购买记录 */
public void setBuyRecord(Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord _buyRecord) { buyRecord = _buyRecord; }


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
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _buyRecordCustLen = _buf.getInt();
	int _buyRecordCurPos = _buf.position();
	buyRecord.ReadUnzipBuf(_buf, _buyRecordCurPos + _buyRecordCustLen);
	_buf.position(_buyRecordCurPos + _buyRecordCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putInt(buyRecord.GetBufSize());
	buyRecord.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)200);
	_buf.put((byte)101);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)200);
	_recBuf.put((byte)101);
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

