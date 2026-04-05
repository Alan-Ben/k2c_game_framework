using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p006_BagItemOp
{

/// <summary>
/// 一键合成转换
/// </summary>
public class GC2GS_006_008_ReqAKeyConvert : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 道具合成列表
/// </summary>
private List<NPCommon.NPCommon_SingleItemConvert> itemConvertList;


public GC2GS_006_008_ReqAKeyConvert() {
	itemConvertList = new List<NPCommon.NPCommon_SingleItemConvert>();
}

public GC2GS_006_008_ReqAKeyConvert(
	List<NPCommon.NPCommon_SingleItemConvert> _itemConvertList
) {	itemConvertList = _itemConvertList;
}

public byte getMainOrder() { return (byte)6; }

public byte getSubOrder() { return (byte)8; }

/// <summary>
/// 道具合成列表
/// </summary>
public List<NPCommon.NPCommon_SingleItemConvert> getItemConvertList() { return itemConvertList; }
/// <summary>
/// 道具合成列表
/// </summary>
public void addItemConvertList(NPCommon.NPCommon_SingleItemConvert _itemConvertList) { itemConvertList.Add(_itemConvertList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < itemConvertList.Count; _i++) {
	_size += 4 + itemConvertList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < itemConvertList.Count; _i++) {
	_size += 4 + itemConvertList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _itemConvertListCount = _buf.getShort();
	for(int _i = 0; _i < _itemConvertListCount; _i++) { 
		NPCommon.NPCommon_SingleItemConvert _itemConvertList = new NPCommon.NPCommon_SingleItemConvert();
		int __itemConvertListCustLen = _buf.getInt();
	int __itemConvertListCurPos = _buf.getCurPos();
	_itemConvertList.ReadUnzipBuf(_buf, __itemConvertListCurPos + __itemConvertListCustLen);
	_buf.setPosition(__itemConvertListCurPos + __itemConvertListCustLen);

		itemConvertList.Add(_itemConvertList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)itemConvertList.Count);
	for(int _i = 0; _i < itemConvertList.Count; _i++) { 
		_buf.putInt(itemConvertList[_i].GetBufSize());
	itemConvertList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)8);
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
	builder.Append("itemConvertList").Append(":").Append(itemConvertList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

