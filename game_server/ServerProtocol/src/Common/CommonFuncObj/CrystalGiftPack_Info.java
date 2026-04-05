package Common.CommonFuncObj;

import java.nio.ByteBuffer;
/*********
 * 钻石礼包组信息
 **/
public class CrystalGiftPack_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包组id */
private long groupId;
/** 下次刷新时间 */
private long nextRefreshTimeMs;
/** 购买记录列表 */
private java.util.ArrayList<Common.CommonFuncObj.CrystalGiftPack_BuyRecord> buyRecordList;


public CrystalGiftPack_Info() {
	groupId = (long)0;
	nextRefreshTimeMs = (long)0;
	buyRecordList = new java.util.ArrayList<Common.CommonFuncObj.CrystalGiftPack_BuyRecord>();
}

public CrystalGiftPack_Info(
	 long _groupId
	, long _nextRefreshTimeMs
	, java.util.ArrayList<Common.CommonFuncObj.CrystalGiftPack_BuyRecord> _buyRecordList
) {	groupId = _groupId;
	nextRefreshTimeMs = _nextRefreshTimeMs;
	buyRecordList = _buyRecordList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 礼包组id */
public long getGroupId() { return groupId; }
/** 礼包组id */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 下次刷新时间 */
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/** 下次刷新时间 */
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }
/** 购买记录列表 */
public java.util.ArrayList<Common.CommonFuncObj.CrystalGiftPack_BuyRecord> getBuyRecordList() { return buyRecordList; }
/** 购买记录列表 */
public void addBuyRecordList(Common.CommonFuncObj.CrystalGiftPack_BuyRecord _buyRecordList) { buyRecordList.add(_buyRecordList); }


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
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _buyRecordListCount = _buf.getShort();
	for(int _i = 0; _i < _buyRecordListCount; _i++) { 
		Common.CommonFuncObj.CrystalGiftPack_BuyRecord _buyRecordList = new Common.CommonFuncObj.CrystalGiftPack_BuyRecord();
		if(_buf.remaining() <= 0) return;
	int __buyRecordListCustLen = _buf.getInt();
	int __buyRecordListCurPos = _buf.position();
	_buyRecordList.ReadUnzipBuf(_buf, __buyRecordListCurPos + __buyRecordListCustLen);
	_buf.position(__buyRecordListCurPos + __buyRecordListCustLen);

		buyRecordList.add(_buyRecordList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
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

