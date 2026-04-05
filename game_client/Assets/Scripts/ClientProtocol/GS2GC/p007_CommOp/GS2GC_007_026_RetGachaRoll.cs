using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_026_RetGachaRoll : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否十连抽
/// </summary>
private bool isTen;
/// <summary>
/// 物品列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> itemList;
/// <summary>
/// 物品ID列表
/// </summary>
private List<long> poolItemIdList;


public GS2GC_007_026_RetGachaRoll() {
	isTen = false;
	itemList = new List<NPCommon.NPCommon_ItemInfo>();
	poolItemIdList = new List<long>();
}

public GS2GC_007_026_RetGachaRoll(
	bool _isTen
	, List<NPCommon.NPCommon_ItemInfo> _itemList
	, List<long> _poolItemIdList
) {	isTen = _isTen;
	itemList = _itemList;
	poolItemIdList = _poolItemIdList;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)26; }

/// <summary>
/// 是否十连抽
/// </summary>
public bool getIsTen() { return isTen; }
/// <summary>
/// 是否十连抽
/// </summary>
public void setIsTen(bool _isTen) { isTen = _isTen; }
/// <summary>
/// 物品列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/// <summary>
/// 物品列表
/// </summary>
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.Add(_itemList); }
/// <summary>
/// 物品ID列表
/// </summary>
public List<long> getPoolItemIdList() { return poolItemIdList; }
/// <summary>
/// 物品ID列表
/// </summary>
public void addPoolItemIdList(long _poolItemIdList) { poolItemIdList.Add(_poolItemIdList); }


public int GetBufSize() {
	int _size = 1;
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}

	_size += 2 + (poolItemIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;
	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}

	_size += 2 + (poolItemIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isTen = (_buf.get() != 0);
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
	short _poolItemIdListCount = _buf.getShort();
	for(int _i = 0; _i < _poolItemIdListCount; _i++) { 
		long _poolItemIdList = (long)0;
		_poolItemIdList = _buf.getLong();
		poolItemIdList.Add(_poolItemIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isTen?(byte)1:(byte)0);
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)poolItemIdList.Count);
	for(int _i = 0; _i < poolItemIdList.Count; _i++) { 
		_buf.putLong(poolItemIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)26);
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
	builder.Append("isTen").Append(":").Append(isTen.ToString()).Append(", ");
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("poolItemIdList").Append(":").Append(poolItemIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

