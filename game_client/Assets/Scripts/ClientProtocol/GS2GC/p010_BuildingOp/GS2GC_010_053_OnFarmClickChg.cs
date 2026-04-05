using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p010_BuildingOp
{

/// <summary>
/// 推送农田建筑点击数据变更
/// </summary>
public class GS2GC_010_053_OnFarmClickChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 建筑ID
/// </summary>
private long buildingId;
/// <summary>
/// 上次点击时间点（毫秒）
/// </summary>
private long lastClickMS;


public GS2GC_010_053_OnFarmClickChg() {
	buildingId = (long)0;
	lastClickMS = (long)0;
}

public GS2GC_010_053_OnFarmClickChg(
	long _buildingId
	, long _lastClickMS
) {	buildingId = _buildingId;
	lastClickMS = _lastClickMS;
}

public byte getMainOrder() { return (byte)10; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 建筑ID
/// </summary>
public long getBuildingId() { return buildingId; }
/// <summary>
/// 建筑ID
/// </summary>
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/// <summary>
/// 上次点击时间点（毫秒）
/// </summary>
public long getLastClickMS() { return lastClickMS; }
/// <summary>
/// 上次点击时间点（毫秒）
/// </summary>
public void setLastClickMS(long _lastClickMS) { lastClickMS = _lastClickMS; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastClickMS = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buildingId);
	_buf.putLong(lastClickMS);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)53);
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
	builder.Append("lastClickMS").Append(":").Append(lastClickMS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

