using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 单道具合成
/// </summary>
public class NPCommon_SingleItemConvert : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 目标道具
/// </summary>
private NPCommon.NPCommon_ItemInfo targetItem;
/// <summary>
/// 原道具列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> originItemList;


public NPCommon_SingleItemConvert() {
	targetItem = new NPCommon.NPCommon_ItemInfo();
	originItemList = new List<NPCommon.NPCommon_ItemInfo>();
}

public NPCommon_SingleItemConvert(
	NPCommon.NPCommon_ItemInfo _targetItem
	, List<NPCommon.NPCommon_ItemInfo> _originItemList
) {	targetItem = _targetItem;
	originItemList = _originItemList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 目标道具
/// </summary>
public NPCommon.NPCommon_ItemInfo getTargetItem() { return targetItem; }
/// <summary>
/// 目标道具
/// </summary>
public void setTargetItem(NPCommon.NPCommon_ItemInfo _targetItem) { targetItem = _targetItem; }
/// <summary>
/// 原道具列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getOriginItemList() { return originItemList; }
/// <summary>
/// 原道具列表
/// </summary>
public void addOriginItemList(NPCommon.NPCommon_ItemInfo _originItemList) { originItemList.Add(_originItemList); }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + targetItem.GetBufSize();
	_size += 2;
for(int _i = 0; _i < originItemList.Count; _i++) {
	_size += 4 + originItemList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + targetItem.GetBufSize();
	_size += 2;
for(int _i = 0; _i < originItemList.Count; _i++) {
	_size += 4 + originItemList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _targetItemCustLen = _buf.getInt();
	int _targetItemCurPos = _buf.getCurPos();
	targetItem.ReadUnzipBuf(_buf, _targetItemCurPos + _targetItemCustLen);
	_buf.setPosition(_targetItemCurPos + _targetItemCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _originItemListCount = _buf.getShort();
	for(int _i = 0; _i < _originItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _originItemList = new NPCommon.NPCommon_ItemInfo();
		int __originItemListCustLen = _buf.getInt();
	int __originItemListCurPos = _buf.getCurPos();
	_originItemList.ReadUnzipBuf(_buf, __originItemListCurPos + __originItemListCustLen);
	_buf.setPosition(__originItemListCurPos + __originItemListCustLen);

		originItemList.Add(_originItemList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(targetItem.GetBufSize());
	targetItem.PutUnzipBuf(_buf);
	_buf.putShort((short)originItemList.Count);
	for(int _i = 0; _i < originItemList.Count; _i++) { 
		_buf.putInt(originItemList[_i].GetBufSize());
	originItemList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("targetItem").Append(":").Append(targetItem == null ? "null" : targetItem.ToString()).Append(", ");
	builder.Append("originItemList").Append(":").Append(originItemList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

