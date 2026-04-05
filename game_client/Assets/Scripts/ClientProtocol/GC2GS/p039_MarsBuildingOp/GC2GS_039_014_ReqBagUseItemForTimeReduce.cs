using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p039_MarsBuildingOp
{

/// <summary>
/// 使用时间减少道具
/// </summary>
public class GC2GS_039_014_ReqBagUseItemForTimeReduce : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 使用道具列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> useItemList;
private Common.MarsEnum.EMarsBagItemUseTimeType objType;
/// <summary>
/// 对象ID
/// </summary>
private long objId;


public GC2GS_039_014_ReqBagUseItemForTimeReduce() {
	useItemList = new List<NPCommon.NPCommon_ItemInfo>();
	objType = 0;
	objId = (long)0;
}

public GC2GS_039_014_ReqBagUseItemForTimeReduce(
	List<NPCommon.NPCommon_ItemInfo> _useItemList
	, Common.MarsEnum.EMarsBagItemUseTimeType _objType
	, long _objId
) {	useItemList = _useItemList;
	objType = _objType;
	objId = _objId;
}

public byte getMainOrder() { return (byte)39; }

public byte getSubOrder() { return (byte)14; }

/// <summary>
/// 使用道具列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getUseItemList() { return useItemList; }
/// <summary>
/// 使用道具列表
/// </summary>
public void addUseItemList(NPCommon.NPCommon_ItemInfo _useItemList) { useItemList.Add(_useItemList); }
public Common.MarsEnum.EMarsBagItemUseTimeType getObjType() { return objType; }
public void setObjType(Common.MarsEnum.EMarsBagItemUseTimeType _objType) { objType = _objType; }
/// <summary>
/// 对象ID
/// </summary>
public long getObjId() { return objId; }
/// <summary>
/// 对象ID
/// </summary>
public void setObjId(long _objId) { objId = _objId; }


public int GetBufSize() {
	int _size = 12;
	_size += 2;
for(int _i = 0; _i < useItemList.Count; _i++) {
	_size += 4 + useItemList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2;
for(int _i = 0; _i < useItemList.Count; _i++) {
	_size += 4 + useItemList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _useItemListCount = _buf.getShort();
	for(int _i = 0; _i < _useItemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _useItemList = new NPCommon.NPCommon_ItemInfo();
		int __useItemListCustLen = _buf.getInt();
	int __useItemListCurPos = _buf.getCurPos();
	_useItemList.ReadUnzipBuf(_buf, __useItemListCurPos + __useItemListCustLen);
	_buf.setPosition(__useItemListCurPos + __useItemListCustLen);

		useItemList.Add(_useItemList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objType = (Common.MarsEnum.EMarsBagItemUseTimeType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)useItemList.Count);
	for(int _i = 0; _i < useItemList.Count; _i++) { 
		_buf.putInt(useItemList[_i].GetBufSize());
	useItemList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt((int)objType);

	_buf.putLong(objId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)14);
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
	builder.Append("useItemList").Append(":").Append(useItemList.ToString()).Append(", ");
	builder.Append("objType").Append(":").Append(objType.ToString()).Append(", ");
	builder.Append("objId").Append(":").Append(objId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

