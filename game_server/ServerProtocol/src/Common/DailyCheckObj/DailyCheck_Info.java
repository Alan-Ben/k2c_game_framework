package Common.DailyCheckObj;

import java.nio.ByteBuffer;
/*********
 * 每日签到信息
 **/
public class DailyCheck_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 未签到天数 */
private int notCheckDays;
/** 情人id */
private long consortId;
/** 甜品列表 */
private java.util.ArrayList<Long> dessertList;
/** 今日是否已签到 */
private boolean hasCheck;
/** 选择的甜品id */
private long chooseDessertId;
/** 奖励列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> itemList;
/** 下一次刷新时间 */
private long nextRefreshTimeMs;


public DailyCheck_Info() {
	notCheckDays = 0;
	consortId = (long)0;
	dessertList = new java.util.ArrayList<Long>();
	hasCheck = false;
	chooseDessertId = (long)0;
	itemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	nextRefreshTimeMs = (long)0;
}

public DailyCheck_Info(
	 int _notCheckDays
	, long _consortId
	, java.util.ArrayList<Long> _dessertList
	, boolean _hasCheck
	, long _chooseDessertId
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _itemList
	, long _nextRefreshTimeMs
) {	notCheckDays = _notCheckDays;
	consortId = _consortId;
	dessertList = _dessertList;
	hasCheck = _hasCheck;
	chooseDessertId = _chooseDessertId;
	itemList = _itemList;
	nextRefreshTimeMs = _nextRefreshTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 未签到天数 */
public int getNotCheckDays() { return notCheckDays; }
/** 未签到天数 */
public void setNotCheckDays(int _notCheckDays) { notCheckDays = _notCheckDays; }
/** 情人id */
public long getConsortId() { return consortId; }
/** 情人id */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 甜品列表 */
public java.util.ArrayList<Long> getDessertList() { return dessertList; }
/** 甜品列表 */
public void addDessertList(long _dessertList) { dessertList.add(_dessertList); }
/** 今日是否已签到 */
public boolean getHasCheck() { return hasCheck; }
/** 今日是否已签到 */
public void setHasCheck(boolean _hasCheck) { hasCheck = _hasCheck; }
/** 选择的甜品id */
public long getChooseDessertId() { return chooseDessertId; }
/** 选择的甜品id */
public void setChooseDessertId(long _chooseDessertId) { chooseDessertId = _chooseDessertId; }
/** 奖励列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/** 奖励列表 */
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.add(_itemList); }
/** 下一次刷新时间 */
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/** 下一次刷新时间 */
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }


public final int GetBufSize() {
	int _size = 29;
	_size += 2 + (dessertList.size() * 8);
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 31;
	_size += 2 + (dessertList.size() * 8);
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) notCheckDays = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dessertListCount = _buf.getShort();
	for(int _i = 0; _i < _dessertListCount; _i++) { 
		long _dessertList = (long)0;
		if(_buf.remaining() > 0) _dessertList = _buf.getLong();
		dessertList.add(_dessertList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasCheck = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chooseDessertId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.position();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.position(__itemListCurPos + __itemListCustLen);

		itemList.add(_itemList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextRefreshTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(notCheckDays);
	_buf.putLong(consortId);
	_buf.putShort((short)dessertList.size());
	for(int _i = 0; _i < dessertList.size(); _i++) { 
		_buf.putLong(dessertList.get(_i));
	}
	_buf.put(hasCheck?(byte)1:(byte)0);
	_buf.putLong(chooseDessertId);
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(nextRefreshTimeMs);
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

