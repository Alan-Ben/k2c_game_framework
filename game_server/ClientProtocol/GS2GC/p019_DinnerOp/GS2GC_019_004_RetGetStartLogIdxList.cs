using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p019_DinnerOp
{

/// <summary>
/// 开宴索引记录列表
/// </summary>
public class GS2GC_019_004_RetGetStartLogIdxList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 开宴索引记录列表
/// </summary>
private List<Common.DinnerObj.Dinner_StartLogIdx> idxList;


public GS2GC_019_004_RetGetStartLogIdxList() {
	idxList = new List<Common.DinnerObj.Dinner_StartLogIdx>();
}

public GS2GC_019_004_RetGetStartLogIdxList(
	List<Common.DinnerObj.Dinner_StartLogIdx> _idxList
) {	idxList = _idxList;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 开宴索引记录列表
/// </summary>
public List<Common.DinnerObj.Dinner_StartLogIdx> getIdxList() { return idxList; }
/// <summary>
/// 开宴索引记录列表
/// </summary>
public void addIdxList(Common.DinnerObj.Dinner_StartLogIdx _idxList) { idxList.Add(_idxList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (idxList.Count * 48);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (idxList.Count * 48);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _idxListCount = _buf.getShort();
	for(int _i = 0; _i < _idxListCount; _i++) { 
		Common.DinnerObj.Dinner_StartLogIdx _idxList = new Common.DinnerObj.Dinner_StartLogIdx();
		int __idxListCustLen = _buf.getInt();
	int __idxListCurPos = _buf.getCurPos();
	_idxList.ReadUnzipBuf(_buf, __idxListCurPos + __idxListCustLen);
	_buf.setPosition(__idxListCurPos + __idxListCustLen);

		idxList.Add(_idxList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)idxList.Count);
	for(int _i = 0; _i < idxList.Count; _i++) { 
		_buf.putInt(idxList[_i].GetBufSize());
	idxList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)4);
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
	builder.Append("idxList").Append(":").Append(idxList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

