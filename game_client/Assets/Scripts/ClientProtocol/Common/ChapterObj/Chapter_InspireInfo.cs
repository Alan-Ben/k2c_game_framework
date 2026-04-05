using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChapterObj
{

/// <summary>
/// 关卡鼓舞信息
/// </summary>
public class Chapter_InspireInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 鼓舞信息列表
/// </summary>
private List<Common.ChapterObj.Chapter_SingleInspireInfo> inspireList;


public Chapter_InspireInfo() {
	inspireList = new List<Common.ChapterObj.Chapter_SingleInspireInfo>();
}

public Chapter_InspireInfo(
	List<Common.ChapterObj.Chapter_SingleInspireInfo> _inspireList
) {	inspireList = _inspireList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 鼓舞信息列表
/// </summary>
public List<Common.ChapterObj.Chapter_SingleInspireInfo> getInspireList() { return inspireList; }
/// <summary>
/// 鼓舞信息列表
/// </summary>
public void addInspireList(Common.ChapterObj.Chapter_SingleInspireInfo _inspireList) { inspireList.Add(_inspireList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (inspireList.Count * 12);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (inspireList.Count * 12);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _inspireListCount = _buf.getShort();
	for(int _i = 0; _i < _inspireListCount; _i++) { 
		Common.ChapterObj.Chapter_SingleInspireInfo _inspireList = new Common.ChapterObj.Chapter_SingleInspireInfo();
		int __inspireListCustLen = _buf.getInt();
	int __inspireListCurPos = _buf.getCurPos();
	_inspireList.ReadUnzipBuf(_buf, __inspireListCurPos + __inspireListCustLen);
	_buf.setPosition(__inspireListCurPos + __inspireListCustLen);

		inspireList.Add(_inspireList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)inspireList.Count);
	for(int _i = 0; _i < inspireList.Count; _i++) { 
		_buf.putInt(inspireList[_i].GetBufSize());
	inspireList[_i].PutUnzipBuf(_buf);
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
	builder.Append("inspireList").Append(":").Append(inspireList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

