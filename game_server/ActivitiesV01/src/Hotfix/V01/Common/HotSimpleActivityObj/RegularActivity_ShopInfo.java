package Hotfix.V01.Common.HotSimpleActivityObj;

import java.nio.ByteBuffer;
/*********
 * 万能活动商店信息
 **/
public class RegularActivity_ShopInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 下次刷新时间 */
private long nextRefreshTimeMs;
/** 购买记录列表 */
private java.util.ArrayList<Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord> buyRecordList;


public RegularActivity_ShopInfo() {
	nextRefreshTimeMs = (long)0;
	buyRecordList = new java.util.ArrayList<Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord>();
}

public RegularActivity_ShopInfo(
	 long _nextRefreshTimeMs
	, java.util.ArrayList<Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord> _buyRecordList
) {	nextRefreshTimeMs = _nextRefreshTimeMs;
	buyRecordList = _buyRecordList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 下次刷新时间 */
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/** 下次刷新时间 */
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }
/** 购买记录列表 */
public java.util.ArrayList<Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord> getBuyRecordList() { return buyRecordList; }
/** 购买记录列表 */
public void addBuyRecordList(Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord _buyRecordList) { buyRecordList.add(_buyRecordList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (buyRecordList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (buyRecordList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _buyRecordListCount = _buf.getShort();
	for(int _i = 0; _i < _buyRecordListCount; _i++) { 
		Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord _buyRecordList = new Hotfix.V01.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord();
		if(_buf.remaining() <= 0) return;
	int __buyRecordListCustLen = _buf.getInt();
	int __buyRecordListCurPos = _buf.position();
	_buyRecordList.ReadUnzipBuf(_buf, __buyRecordListCurPos + __buyRecordListCustLen);
	_buf.position(__buyRecordListCurPos + __buyRecordListCustLen);

		buyRecordList.add(_buyRecordList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
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

