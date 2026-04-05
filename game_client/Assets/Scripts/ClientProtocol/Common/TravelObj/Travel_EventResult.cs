using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TravelObj
{

/// <summary>
/// 游历事件结果
/// </summary>
public class Travel_EventResult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 事件ID
/// </summary>
private long eventId;
/// <summary>
/// 奖励物品列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> itemList;
/// <summary>
/// 额外数据
/// </summary>
private byte[] ext;


public Travel_EventResult() {
	eventId = (long)0;
	itemList = new List<NPCommon.NPCommon_ItemInfo>();
	ext = null;
}

public Travel_EventResult(
	long _eventId
	, List<NPCommon.NPCommon_ItemInfo> _itemList
	, byte[] _ext
) {	eventId = _eventId;
	itemList = _itemList;
	ext = _ext;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 事件ID
/// </summary>
public long getEventId() { return eventId; }
/// <summary>
/// 事件ID
/// </summary>
public void setEventId(long _eventId) { eventId = _eventId; }
/// <summary>
/// 奖励物品列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/// <summary>
/// 奖励物品列表
/// </summary>
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.Add(_itemList); }
/// <summary>
/// 额外数据
/// </summary>
public byte[] getExt() { return ext; }

/// <summary>
/// 额外数据
/// </summary>
public void setExt(byte[] _ext) { ext = _ext; }



public int GetBufSize() {
	int _size = 8;
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}

	_size += 4 + (ext == null ? 0 : ext.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}

	_size += 4 + (ext == null ? 0 : ext.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventId = _buf.getLong();
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
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	ext = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(eventId);
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
	_buf.putByteBuffer(ext);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("ext").Append(":").Append(ext == null ? "null" : ext.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

