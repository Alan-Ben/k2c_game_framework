using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_041_RetRecuritInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已兑换id列表
/// </summary>
private List<long> hadRecuritIdList;


public GS2GC_002_041_RetRecuritInfo() {
	hadRecuritIdList = new List<long>();
}

public GS2GC_002_041_RetRecuritInfo(
	List<long> _hadRecuritIdList
) {	hadRecuritIdList = _hadRecuritIdList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)41; }

/// <summary>
/// 已兑换id列表
/// </summary>
public List<long> getHadRecuritIdList() { return hadRecuritIdList; }
/// <summary>
/// 已兑换id列表
/// </summary>
public void addHadRecuritIdList(long _hadRecuritIdList) { hadRecuritIdList.Add(_hadRecuritIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (hadRecuritIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (hadRecuritIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadRecuritIdListCount = _buf.getShort();
	for(int _i = 0; _i < _hadRecuritIdListCount; _i++) { 
		long _hadRecuritIdList = (long)0;
		_hadRecuritIdList = _buf.getLong();
		hadRecuritIdList.Add(_hadRecuritIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)hadRecuritIdList.Count);
	for(int _i = 0; _i < hadRecuritIdList.Count; _i++) { 
		_buf.putLong(hadRecuritIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)41);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)41);
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
	builder.Append("hadRecuritIdList").Append(":").Append(hadRecuritIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

