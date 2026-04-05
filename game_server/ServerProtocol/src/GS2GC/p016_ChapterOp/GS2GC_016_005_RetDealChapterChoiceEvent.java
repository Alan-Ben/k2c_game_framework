package GS2GC.p016_ChapterOp;

import java.nio.ByteBuffer;
public class GS2GC_016_005_RetDealChapterChoiceEvent implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件ID */
private long eventId;
/** 选项ID */
private long optionId;
/** 物品列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> itemList;


public GS2GC_016_005_RetDealChapterChoiceEvent() {
	eventId = (long)0;
	optionId = (long)0;
	itemList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_016_005_RetDealChapterChoiceEvent(
	 long _eventId
	, long _optionId
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _itemList
) {	eventId = _eventId;
	optionId = _optionId;
	itemList = _itemList;
}

public final byte getMainOrder() { return (byte)16; }

public final byte getSubOrder() { return (byte)5; }

/** 事件ID */
public long getEventId() { return eventId; }
/** 事件ID */
public void setEventId(long _eventId) { eventId = _eventId; }
/** 选项ID */
public long getOptionId() { return optionId; }
/** 选项ID */
public void setOptionId(long _optionId) { optionId = _optionId; }
/** 物品列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/** 物品列表 */
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.add(_itemList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
	for(int _i = 0; _i < itemList.size(); _i++) {
	_size += 4 + itemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) optionId = _buf.getLong();
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
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(eventId);
	_buf.putLong(optionId);
	_buf.putShort((short)itemList.size());
	for(int _i = 0; _i < itemList.size(); _i++) { 
		_buf.putInt(itemList.get(_i).GetBufSize());
	itemList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)5);
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

