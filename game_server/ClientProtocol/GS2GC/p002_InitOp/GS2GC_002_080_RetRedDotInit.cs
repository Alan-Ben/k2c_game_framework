using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_080_RetRedDotInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 红点列表
/// </summary>
private List<Common.Common_RedDotInfo> redDotList;


public GS2GC_002_080_RetRedDotInit() {
	redDotList = new List<Common.Common_RedDotInfo>();
}

public GS2GC_002_080_RetRedDotInit(
	List<Common.Common_RedDotInfo> _redDotList
) {	redDotList = _redDotList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)80; }

/// <summary>
/// 红点列表
/// </summary>
public List<Common.Common_RedDotInfo> getRedDotList() { return redDotList; }
/// <summary>
/// 红点列表
/// </summary>
public void addRedDotList(Common.Common_RedDotInfo _redDotList) { redDotList.Add(_redDotList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (redDotList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (redDotList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _redDotListCount = _buf.getShort();
	for(int _i = 0; _i < _redDotListCount; _i++) { 
		Common.Common_RedDotInfo _redDotList = new Common.Common_RedDotInfo();
		int __redDotListCustLen = _buf.getInt();
	int __redDotListCurPos = _buf.getCurPos();
	_redDotList.ReadUnzipBuf(_buf, __redDotListCurPos + __redDotListCustLen);
	_buf.setPosition(__redDotListCurPos + __redDotListCustLen);

		redDotList.Add(_redDotList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)redDotList.Count);
	for(int _i = 0; _i < redDotList.Count; _i++) { 
		_buf.putInt(redDotList[_i].GetBufSize());
	redDotList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)80);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)80);
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
	builder.Append("redDotList").Append(":").Append(redDotList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

