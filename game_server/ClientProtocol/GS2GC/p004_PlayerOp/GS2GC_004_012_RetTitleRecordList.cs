using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_012_RetTitleRecordList : ALBasicProtocolPack._IALProtocolStructure {
private List<long> titleIdList;


public GS2GC_004_012_RetTitleRecordList() {
	titleIdList = new List<long>();
}

public GS2GC_004_012_RetTitleRecordList(
	List<long> _titleIdList
) {	titleIdList = _titleIdList;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)12; }

public List<long> getTitleIdList() { return titleIdList; }
public void addTitleIdList(long _titleIdList) { titleIdList.Add(_titleIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (titleIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (titleIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _titleIdListCount = _buf.getShort();
	for(int _i = 0; _i < _titleIdListCount; _i++) { 
		long _titleIdList = (long)0;
		_titleIdList = _buf.getLong();
		titleIdList.Add(_titleIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)titleIdList.Count);
	for(int _i = 0; _i < titleIdList.Count; _i++) { 
		_buf.putLong(titleIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)12);
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
	builder.Append("titleIdList").Append(":").Append(titleIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

