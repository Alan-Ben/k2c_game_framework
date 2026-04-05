using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_007_RetBagItemList : ALBasicProtocolPack._IALProtocolStructure {
private List<NPCommon.NPCommon_BagItemInfo> bagItemList;


public GS2GC_002_007_RetBagItemList() {
	bagItemList = new List<NPCommon.NPCommon_BagItemInfo>();
}

public GS2GC_002_007_RetBagItemList(
	List<NPCommon.NPCommon_BagItemInfo> _bagItemList
) {	bagItemList = _bagItemList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)7; }

public List<NPCommon.NPCommon_BagItemInfo> getBagItemList() { return bagItemList; }
public void addBagItemList(NPCommon.NPCommon_BagItemInfo _bagItemList) { bagItemList.Add(_bagItemList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (bagItemList.Count * 32);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (bagItemList.Count * 32);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _bagItemListCount = _buf.getShort();
	for(int _i = 0; _i < _bagItemListCount; _i++) { 
		NPCommon.NPCommon_BagItemInfo _bagItemList = new NPCommon.NPCommon_BagItemInfo();
		int __bagItemListCustLen = _buf.getInt();
	int __bagItemListCurPos = _buf.getCurPos();
	_bagItemList.ReadUnzipBuf(_buf, __bagItemListCurPos + __bagItemListCustLen);
	_buf.setPosition(__bagItemListCurPos + __bagItemListCustLen);

		bagItemList.Add(_bagItemList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)bagItemList.Count);
	for(int _i = 0; _i < bagItemList.Count; _i++) { 
		_buf.putInt(bagItemList[_i].GetBufSize());
	bagItemList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)7);
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
	builder.Append("bagItemList").Append(":").Append(bagItemList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

