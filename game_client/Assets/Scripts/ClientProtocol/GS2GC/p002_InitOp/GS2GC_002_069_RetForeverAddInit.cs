using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_069_RetForeverAddInit : ALBasicProtocolPack._IALProtocolStructure {
private List<NPCommon.NPCommon_ForeverAddInfo> addList;


public GS2GC_002_069_RetForeverAddInit() {
	addList = new List<NPCommon.NPCommon_ForeverAddInfo>();
}

public GS2GC_002_069_RetForeverAddInit(
	List<NPCommon.NPCommon_ForeverAddInfo> _addList
) {	addList = _addList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)69; }

public List<NPCommon.NPCommon_ForeverAddInfo> getAddList() { return addList; }
public void addAddList(NPCommon.NPCommon_ForeverAddInfo _addList) { addList.Add(_addList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (addList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (addList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _addListCount = _buf.getShort();
	for(int _i = 0; _i < _addListCount; _i++) { 
		NPCommon.NPCommon_ForeverAddInfo _addList = new NPCommon.NPCommon_ForeverAddInfo();
		int __addListCustLen = _buf.getInt();
	int __addListCurPos = _buf.getCurPos();
	_addList.ReadUnzipBuf(_buf, __addListCurPos + __addListCustLen);
	_buf.setPosition(__addListCurPos + __addListCustLen);

		addList.Add(_addList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)addList.Count);
	for(int _i = 0; _i < addList.Count; _i++) { 
		_buf.putInt(addList[_i].GetBufSize());
	addList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)69);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)69);
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
	builder.Append("addList").Append(":").Append(addList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

