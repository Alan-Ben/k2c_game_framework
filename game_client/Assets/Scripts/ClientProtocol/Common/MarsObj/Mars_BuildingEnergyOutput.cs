using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星建筑-建筑能源产出
/// </summary>
public class Mars_BuildingEnergyOutput : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 建筑ID
/// </summary>
private long buildingId;
/// <summary>
/// 已产出数量
/// </summary>
private long output;
/// <summary>
/// 上次结算时间（毫秒）
/// </summary>
private long lastSettleMs;


public Mars_BuildingEnergyOutput() {
	buildingId = (long)0;
	output = (long)0;
	lastSettleMs = (long)0;
}

public Mars_BuildingEnergyOutput(
	long _buildingId
	, long _output
	, long _lastSettleMs
) {	buildingId = _buildingId;
	output = _output;
	lastSettleMs = _lastSettleMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 建筑ID
/// </summary>
public long getBuildingId() { return buildingId; }
/// <summary>
/// 建筑ID
/// </summary>
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/// <summary>
/// 已产出数量
/// </summary>
public long getOutput() { return output; }
/// <summary>
/// 已产出数量
/// </summary>
public void setOutput(long _output) { output = _output; }
/// <summary>
/// 上次结算时间（毫秒）
/// </summary>
public long getLastSettleMs() { return lastSettleMs; }
/// <summary>
/// 上次结算时间（毫秒）
/// </summary>
public void setLastSettleMs(long _lastSettleMs) { lastSettleMs = _lastSettleMs; }


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
	output = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastSettleMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buildingId);
	_buf.putLong(output);
	_buf.putLong(lastSettleMs);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("output").Append(":").Append(output.ToString()).Append(", ");
	builder.Append("lastSettleMs").Append(":").Append(lastSettleMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

