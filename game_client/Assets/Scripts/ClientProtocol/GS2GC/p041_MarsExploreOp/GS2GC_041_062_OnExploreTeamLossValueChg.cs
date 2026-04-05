using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p041_MarsExploreOp
{

/// <summary>
/// 火星探索队伍-损耗数量变更
/// </summary>
public class GS2GC_041_062_OnExploreTeamLossValueChg : ALBasicProtocolPack._IALProtocolStructure {
private long teamId;
/// <summary>
/// 队伍损耗数量
/// </summary>
private long lossValue;


public GS2GC_041_062_OnExploreTeamLossValueChg() {
	teamId = (long)0;
	lossValue = (long)0;
}

public GS2GC_041_062_OnExploreTeamLossValueChg(
	long _teamId
	, long _lossValue
) {	teamId = _teamId;
	lossValue = _lossValue;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)62; }

public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }
/// <summary>
/// 队伍损耗数量
/// </summary>
public long getLossValue() { return lossValue; }
/// <summary>
/// 队伍损耗数量
/// </summary>
public void setLossValue(long _lossValue) { lossValue = _lossValue; }


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
	teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lossValue = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putLong(lossValue);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)62);
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
	builder.Append("teamId").Append(":").Append(teamId.ToString()).Append(", ");
	builder.Append("lossValue").Append(":").Append(lossValue.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

