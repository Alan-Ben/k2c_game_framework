using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p016_ChapterOp
{

public class GS2GC_016_005_RetDealChapterChoiceEvent : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 事件ID
/// </summary>
private long eventId;
/// <summary>
/// 选项ID
/// </summary>
private long optionId;
/// <summary>
/// 物品列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> itemList;


public GS2GC_016_005_RetDealChapterChoiceEvent() {
	eventId = (long)0;
	optionId = (long)0;
	itemList = new List<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_016_005_RetDealChapterChoiceEvent(
	long _eventId
	, long _optionId
	, List<NPCommon.NPCommon_ItemInfo> _itemList
) {	eventId = _eventId;
	optionId = _optionId;
	itemList = _itemList;
}

public byte getMainOrder() { return (byte)16; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 事件ID
/// </summary>
public long getEventId() { return eventId; }
/// <summary>
/// 事件ID
/// </summary>
public void setEventId(long _eventId) { eventId = _eventId; }
/// <summary>
/// 选项ID
/// </summary>
public long getOptionId() { return optionId; }
/// <summary>
/// 选项ID
/// </summary>
public void setOptionId(long _optionId) { optionId = _optionId; }
/// <summary>
/// 物品列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/// <summary>
/// 物品列表
/// </summary>
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.Add(_itemList); }


public int GetBufSize() {
	int _size = 16;
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	optionId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.getCurPos();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.setPosition(__itemListCurPos + __itemListCustLen);

		itemList.Add(_itemList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(eventId);
	_buf.putLong(optionId);
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)16);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)16);
	_recBuf.put((byte)5);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("eventId").Append(":").Append(eventId.ToString()).Append(", ");
	builder.Append("optionId").Append(":").Append(optionId.ToString()).Append(", ");
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

