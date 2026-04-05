package Common.ActivityObj;

import java.nio.ByteBuffer;
/*********
 * 活动商店信息
 **/
public class Activity_ShopInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 商店ID */
private long shopId;
/** 下次刷新时间 */
private long nextRefreshTimeMs;
/** 购买记录列表 */
private java.util.ArrayList<Common.ActivityObj.Activity_ShopBuyRecord> buyRecordList;


public Activity_ShopInfo() {
	shopId = (long)0;
	nextRefreshTimeMs = (long)0;
	buyRecordList = new java.util.ArrayList<Common.ActivityObj.Activity_ShopBuyRecord>();
}

public Activity_ShopInfo(
	 long _shopId
	, long _nextRefreshTimeMs
	, java.util.ArrayList<Common.ActivityObj.Activity_ShopBuyRecord> _buyRecordList
) {	shopId = _shopId;
	nextRefreshTimeMs = _nextRefreshTimeMs;
	buyRecordList = _buyRecordList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 商店ID */
public long getShopId() { return shopId; }
/** 商店ID */
public void setShopId(long _shopId) { shopId = _shopId; }
/** 下次刷新时间 */
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/** 下次刷新时间 */
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }
/** 购买记录列表 */
public java.util.ArrayList<Common.ActivityObj.Activity_ShopBuyRecord> getBuyRecordList() { return buyRecordList; }
/** 购买记录列表 */
public void addBuyRecordList(Common.ActivityObj.Activity_ShopBuyRecord _buyRecordList) { buyRecordList.add(_buyRecordList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2 + (buyRecordList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (buyRecordList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shopId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _buyRecordListCount = _buf.getShort();
	for(int _i = 0; _i < _buyRecordListCount; _i++) { 
		Common.ActivityObj.Activity_ShopBuyRecord _buyRecordList = new Common.ActivityObj.Activity_ShopBuyRecord();
		if(_buf.remaining() <= 0) return;
	int __buyRecordListCustLen = _buf.getInt();
	int __buyRecordListCurPos = _buf.position();
	_buyRecordList.ReadUnzipBuf(_buf, __buyRecordListCurPos + __buyRecordListCustLen);
	_buf.position(__buyRecordListCurPos + __buyRecordListCustLen);

		buyRecordList.add(_buyRecordList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(shopId);
	_buf.putLong(nextRefreshTimeMs);
	_buf.putShort((short)buyRecordList.size());
	for(int _i = 0; _i < buyRecordList.size(); _i++) { 
		_buf.putInt(buyRecordList.get(_i).GetBufSize());
	buyRecordList.get(_i).PutUnzipBuf(_buf);
	}
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

