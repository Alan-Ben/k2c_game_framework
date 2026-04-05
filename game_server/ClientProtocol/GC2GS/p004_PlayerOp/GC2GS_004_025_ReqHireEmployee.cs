using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 请求雇佣员工
/// </summary>
public class GC2GS_004_025_ReqHireEmployee : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 雇佣数量
/// </summary>
private int num;
/// <summary>
/// 建筑ID
/// </summary>
private long buildingId;


public GC2GS_004_025_ReqHireEmployee() {
	num = 0;
	buildingId = (long)0;
}

public GC2GS_004_025_ReqHireEmployee(
	int _num
	, long _buildingId
) {	num = _num;
	buildingId = _buildingId;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)25; }

/// <summary>
/// 雇佣数量
/// </summary>
public int getNum() { return num; }
/// <summary>
/// 雇佣数量
/// </summary>
public void setNum(int _num) { num = _num; }
/// <summary>
/// 建筑ID
/// </summary>
public long getBuildingId() { return buildingId; }
/// <summary>
/// 建筑ID
/// </summary>
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }


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
	num = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buildingId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(num);
	_buf.putLong(buildingId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)25);
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
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("buildingId").Append(":").Append(buildingId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

