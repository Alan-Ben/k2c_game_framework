using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_044_RetGraveConNewInfoList : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.GraveObj.GraveObj_NewInfo> newInfoList;


public GS2GC_004_044_RetGraveConNewInfoList() {
	newInfoList = new List<Common.GraveObj.GraveObj_NewInfo>();
}

public GS2GC_004_044_RetGraveConNewInfoList(
	List<Common.GraveObj.GraveObj_NewInfo> _newInfoList
) {	newInfoList = _newInfoList;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)44; }

public List<Common.GraveObj.GraveObj_NewInfo> getNewInfoList() { return newInfoList; }
public void addNewInfoList(Common.GraveObj.GraveObj_NewInfo _newInfoList) { newInfoList.Add(_newInfoList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (newInfoList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (newInfoList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _newInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _newInfoListCount; _i++) { 
		Common.GraveObj.GraveObj_NewInfo _newInfoList = new Common.GraveObj.GraveObj_NewInfo();
		int __newInfoListCustLen = _buf.getInt();
	int __newInfoListCurPos = _buf.getCurPos();
	_newInfoList.ReadUnzipBuf(_buf, __newInfoListCurPos + __newInfoListCustLen);
	_buf.setPosition(__newInfoListCurPos + __newInfoListCustLen);

		newInfoList.Add(_newInfoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)newInfoList.Count);
	for(int _i = 0; _i < newInfoList.Count; _i++) { 
		_buf.putInt(newInfoList[_i].GetBufSize());
	newInfoList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)44);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)44);
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
	builder.Append("newInfoList").Append(":").Append(newInfoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

