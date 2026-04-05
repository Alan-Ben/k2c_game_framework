using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_058_RetMuseumInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 物品列表
/// </summary>
private List<Common.MuseumObj.Museum_ItemInfo> itemList;


public GS2GC_002_058_RetMuseumInit() {
	itemList = new List<Common.MuseumObj.Museum_ItemInfo>();
}

public GS2GC_002_058_RetMuseumInit(
	List<Common.MuseumObj.Museum_ItemInfo> _itemList
) {	itemList = _itemList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)58; }

/// <summary>
/// 物品列表
/// </summary>
public List<Common.MuseumObj.Museum_ItemInfo> getItemList() { return itemList; }
/// <summary>
/// 物品列表
/// </summary>
public void addItemList(Common.MuseumObj.Museum_ItemInfo _itemList) { itemList.Add(_itemList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (itemList.Count * 25);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (itemList.Count * 25);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		Common.MuseumObj.Museum_ItemInfo _itemList = new Common.MuseumObj.Museum_ItemInfo();
		int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.getCurPos();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.setPosition(__itemListCurPos + __itemListCustLen);

		itemList.Add(_itemList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)58);
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
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

