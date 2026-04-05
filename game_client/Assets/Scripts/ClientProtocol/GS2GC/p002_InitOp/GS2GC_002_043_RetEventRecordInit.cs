using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_043_RetEventRecordInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据列表
/// </summary>
private List<Common.PlayerObj.Player_EventRecordInfo> recordList;


public GS2GC_002_043_RetEventRecordInit() {
	recordList = new List<Common.PlayerObj.Player_EventRecordInfo>();
}

public GS2GC_002_043_RetEventRecordInit(
	List<Common.PlayerObj.Player_EventRecordInfo> _recordList
) {	recordList = _recordList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)43; }

/// <summary>
/// 数据列表
/// </summary>
public List<Common.PlayerObj.Player_EventRecordInfo> getRecordList() { return recordList; }
/// <summary>
/// 数据列表
/// </summary>
public void addRecordList(Common.PlayerObj.Player_EventRecordInfo _recordList) { recordList.Add(_recordList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (recordList.Count * 24);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (recordList.Count * 24);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _recordListCount = _buf.getShort();
	for(int _i = 0; _i < _recordListCount; _i++) { 
		Common.PlayerObj.Player_EventRecordInfo _recordList = new Common.PlayerObj.Player_EventRecordInfo();
		int __recordListCustLen = _buf.getInt();
	int __recordListCurPos = _buf.getCurPos();
	_recordList.ReadUnzipBuf(_buf, __recordListCurPos + __recordListCustLen);
	_buf.setPosition(__recordListCurPos + __recordListCustLen);

		recordList.Add(_recordList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)recordList.Count);
	for(int _i = 0; _i < recordList.Count; _i++) { 
		_buf.putInt(recordList[_i].GetBufSize());
	recordList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)43);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)43);
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
	builder.Append("recordList").Append(":").Append(recordList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

