using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p010_BuildingOp
{

/// <summary>
/// 推送建筑完成
/// </summary>
public class GS2GC_010_050_OnBuidingBuilt : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private long buildingId;


public GS2GC_010_050_OnBuidingBuilt() {
	buildingId = (long)0;
}

public GS2GC_010_050_OnBuidingBuilt(
	long _buildingId
) {	buildingId = _buildingId;
}

public byte getMainOrder() { return (byte)10; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 空
/// </summary>
public long getBuildingId() { return buildingId; }
/// <summary>
/// 空
/// </summary>
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buildingId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buildingId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)50);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

