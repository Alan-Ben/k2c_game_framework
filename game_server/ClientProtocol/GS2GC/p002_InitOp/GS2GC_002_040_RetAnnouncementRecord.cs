using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_040_RetAnnouncementRecord : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已领取公告id列表
/// </summary>
private List<long> hadDrawIdList;


public GS2GC_002_040_RetAnnouncementRecord() {
	hadDrawIdList = new List<long>();
}

public GS2GC_002_040_RetAnnouncementRecord(
	List<long> _hadDrawIdList
) {	hadDrawIdList = _hadDrawIdList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)40; }

/// <summary>
/// 已领取公告id列表
/// </summary>
public List<long> getHadDrawIdList() { return hadDrawIdList; }
/// <summary>
/// 已领取公告id列表
/// </summary>
public void addHadDrawIdList(long _hadDrawIdList) { hadDrawIdList.Add(_hadDrawIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (hadDrawIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (hadDrawIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawIdListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawIdListCount; _i++) { 
		long _hadDrawIdList = (long)0;
		_hadDrawIdList = _buf.getLong();
		hadDrawIdList.Add(_hadDrawIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)hadDrawIdList.Count);
	for(int _i = 0; _i < hadDrawIdList.Count; _i++) { 
		_buf.putLong(hadDrawIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)40);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)40);
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
	builder.Append("hadDrawIdList").Append(":").Append(hadDrawIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

