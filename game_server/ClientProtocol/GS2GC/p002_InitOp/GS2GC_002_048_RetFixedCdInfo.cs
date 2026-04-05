using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 初始化定时恢复cd数据
/// </summary>
public class GS2GC_002_048_RetFixedCdInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 定时恢复cd数据列表
/// </summary>
private List<NPCommon.NPCommon_PlayerFixedCD> cdList;


public GS2GC_002_048_RetFixedCdInfo() {
	cdList = new List<NPCommon.NPCommon_PlayerFixedCD>();
}

public GS2GC_002_048_RetFixedCdInfo(
	List<NPCommon.NPCommon_PlayerFixedCD> _cdList
) {	cdList = _cdList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)48; }

/// <summary>
/// 定时恢复cd数据列表
/// </summary>
public List<NPCommon.NPCommon_PlayerFixedCD> getCdList() { return cdList; }
/// <summary>
/// 定时恢复cd数据列表
/// </summary>
public void addCdList(NPCommon.NPCommon_PlayerFixedCD _cdList) { cdList.Add(_cdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (cdList.Count * 28);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cdList.Count * 28);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _cdListCount = _buf.getShort();
	for(int _i = 0; _i < _cdListCount; _i++) { 
		NPCommon.NPCommon_PlayerFixedCD _cdList = new NPCommon.NPCommon_PlayerFixedCD();
		int __cdListCustLen = _buf.getInt();
	int __cdListCurPos = _buf.getCurPos();
	_cdList.ReadUnzipBuf(_buf, __cdListCurPos + __cdListCustLen);
	_buf.setPosition(__cdListCurPos + __cdListCustLen);

		cdList.Add(_cdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)cdList.Count);
	for(int _i = 0; _i < cdList.Count; _i++) { 
		_buf.putInt(cdList[_i].GetBufSize());
	cdList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)48);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)48);
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
	builder.Append("cdList").Append(":").Append(cdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

