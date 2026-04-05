package GS2GC.p016_ChapterOp;

import java.nio.ByteBuffer;
public class GS2GC_016_001_RetChapterForward implements ALBasicProtocolPack._IALProtocolStructure {
private long chapterId;
private int point;
private int coefficient;
/** 奖励大臣经验 */
private long rewardExp;
/** 奖励玩家经验 */
private long rewardPlayerExp;
/** 触发事件id */
private long eventId;
/** 物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> itemList;
/** 花费金币数量 */
private long costGoldNum;


public GS2GC_016_001_RetChapterForward() {
	chapterId = (long)0;
	point = 0;
	coefficient = 0;
	rewardExp = (long)0;
	rewardPlayerExp = (long)0;
	eventId = (long)0;
	itemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	costGoldNum = (long)0;
}

public GS2GC_016_001_RetChapterForward(
	 long _chapterId
	, int _point
	, int _coefficient
	, long _rewardExp
	, long _rewardPlayerExp
	, long _eventId
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _itemList
	, long _costGoldNum
) {	chapterId = _chapterId;
	point = _point;
	coefficient = _coefficient;
	rewardExp = _rewardExp;
	rewardPlayerExp = _rewardPlayerExp;
	eventId = _eventId;
	itemList = _itemList;
	costGoldNum = _costGoldNum;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)1; }

public long getChapterId() { return chapterId; }
public void setChapterId(long _chapterId) { chapterId = _chapterId; }
public int getPoint() { return point; }
public void setPoint(int _point) { point = _point; }
public int getCoefficient() { return coefficient; }
public void setCoefficient(int _coefficient) { coefficient = _coefficient; }
/** 奖励大臣经验 */
public long getRewardExp() { return rewardExp; }
/** 奖励大臣经验 */
public void setRewardExp(long _rewardExp) { rewardExp = _rewardExp; }
/** 奖励玩家经验 */
public long getRewardPlayerExp() { return rewardPlayerExp; }
/** 奖励玩家经验 */
public void setRewardPlayerExp(long _rewardPlayerExp) { rewardPlayerExp = _rewardPlayerExp; }
/** 触发事件id */
public long getEventId() { return eventId; }
/** 触发事件id */
public void setEventId(long _eventId) { eventId = _eventId; }
/** 物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/** 物品列表 */
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.add(_itemList); }
/** 花费金币数量 */
public long getCostGoldNum() { return costGoldNum; }
/** 花费金币数量 */
public void setCostGoldNum(long _costGoldNum) { costGoldNum = _costGoldNum; }


public final int GetBufSize() {
	int _size = 48;
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 50;
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chapterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) point = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) coefficient = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rewardExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rewardPlayerExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventId = _buf.getLong();
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
	if(_buf.remaining() > 0) costGoldNum = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(chapterId);
	_buf.putInt(point);
	_buf.putInt(coefficient);
	_buf.putLong(rewardExp);
	_buf.putLong(rewardPlayerExp);
	_buf.putLong(eventId);
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(costGoldNum);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)1);
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

