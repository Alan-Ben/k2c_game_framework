using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p010_BuildingOp
{

/// <summary>
/// 请求点击农田产出
/// </summary>
public class GC2GS_010_003_ReqFarmClickOutput : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 建筑ID
/// </summary>
private long buildingId;
/// <summary>
/// 当前点击次数
/// </summary>
private long clickNum;
/// <summary>
/// 客户端操作时间戳
/// </summary>
private long clientOpTimeMs;


public GC2GS_010_003_ReqFarmClickOutput() {
	buildingId = (long)0;
	clickNum = (long)0;
	clientOpTimeMs = (long)0;
}

public GC2GS_010_003_ReqFarmClickOutput(
	long _buildingId
	, long _clickNum
	, long _clientOpTimeMs
) {	buildingId = _buildingId;
	clickNum = _clickNum;
	clientOpTimeMs = _clientOpTimeMs;
}

public byte getMainOrder() { return (byte)10; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 建筑ID
/// </summary>
public long getBuildingId() { return buildingId; }
/// <summary>
/// 建筑ID
/// </summary>
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/// <summary>
/// 当前点击次数
/// </summary>
public long getClickNum() { return clickNum; }
/// <summary>
/// 当前点击次数
/// </summary>
public void setClickNum(long _clickNum) { clickNum = _clickNum; }
/// <summary>
/// 客户端操作时间戳
/// </summary>
public long getClientOpTimeMs() { return clientOpTimeMs; }
/// <summary>
/// 客户端操作时间戳
/// </summary>
public void setClientOpTimeMs(long _clientOpTimeMs) { clientOpTimeMs = _clientOpTimeMs; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clickNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clientOpTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buildingId);
	_buf.putLong(clickNum);
	_buf.putLong(clientOpTimeMs);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)3);
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
	builder.Append("clickNum").Append(":").Append(clickNum.ToString()).Append(", ");
	builder.Append("clientOpTimeMs").Append(":").Append(clientOpTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

