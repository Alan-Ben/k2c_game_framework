using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p006_BagItemOp
{

public class GS2GC_006_002_RetBagUseItem : ALBasicProtocolPack._IALProtocolStructure {
private long itemId;
private long count;
/// <summary>
/// 获得物品列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> itemList;


public GS2GC_006_002_RetBagUseItem() {
	itemId = (long)0;
	count = (long)0;
	itemList = new List<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_006_002_RetBagUseItem(
	long _itemId
	, long _count
	, List<NPCommon.NPCommon_ItemInfo> _itemList
) {	itemId = _itemId;
	count = _count;
	itemList = _itemList;
}

public byte getMainOrder() { return (byte)6; }

public byte getSubOrder() { return (byte)2; }

public long getItemId() { return itemId; }
public void setItemId(long _itemId) { itemId = _itemId; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }
/// <summary>
/// 获得物品列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/// <summary>
/// 获得物品列表
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
	itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
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
	_buf.putLong(itemId);
	_buf.putLong(count);
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)2);
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
	builder.Append("itemId").Append(":").Append(itemId.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

