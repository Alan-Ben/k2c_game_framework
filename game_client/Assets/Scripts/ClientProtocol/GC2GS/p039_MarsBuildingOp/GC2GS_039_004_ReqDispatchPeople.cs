using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p039_MarsBuildingOp
{

/// <summary>
/// 火星建筑-派遣居民
/// </summary>
public class GC2GS_039_004_ReqDispatchPeople : ALBasicProtocolPack._IALProtocolStructure {
private long buildingId;
/// <summary>
/// 居民数量
/// </summary>
private int peopleNum;


public GC2GS_039_004_ReqDispatchPeople() {
	buildingId = (long)0;
	peopleNum = 0;
}

public GC2GS_039_004_ReqDispatchPeople(
	long _buildingId
	, int _peopleNum
) {	buildingId = _buildingId;
	peopleNum = _peopleNum;
}

public byte getMainOrder() { return (byte)39; }

public byte getSubOrder() { return (byte)4; }

public long getBuildingId() { return buildingId; }
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/// <summary>
/// 居民数量
/// </summary>
public int getPeopleNum() { return peopleNum; }
/// <summary>
/// 居民数量
/// </summary>
public void setPeopleNum(int _peopleNum) { peopleNum = _peopleNum; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	peopleNum = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buildingId);
	_buf.putInt(peopleNum);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
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
	builder.Append("buildingId").Append(":").Append(buildingId.ToString()).Append(", ");
	builder.Append("peopleNum").Append(":").Append(peopleNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

