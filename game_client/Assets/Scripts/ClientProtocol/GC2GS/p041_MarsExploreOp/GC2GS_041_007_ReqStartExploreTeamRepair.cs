using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p041_MarsExploreOp
{

/// <summary>
/// 火星探险-修复队伍
/// </summary>
public class GC2GS_041_007_ReqStartExploreTeamRepair : ALBasicProtocolPack._IALProtocolStructure {
private long teamId;
/// <summary>
/// 修复数量，0-当前全部伤兵
/// </summary>
private long repairNum;


public GC2GS_041_007_ReqStartExploreTeamRepair() {
	teamId = (long)0;
	repairNum = (long)0;
}

public GC2GS_041_007_ReqStartExploreTeamRepair(
	long _teamId
	, long _repairNum
) {	teamId = _teamId;
	repairNum = _repairNum;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)7; }

public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }
/// <summary>
/// 修复数量，0-当前全部伤兵
/// </summary>
public long getRepairNum() { return repairNum; }
/// <summary>
/// 修复数量，0-当前全部伤兵
/// </summary>
public void setRepairNum(long _repairNum) { repairNum = _repairNum; }


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
	repairNum = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putLong(repairNum);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)7);
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
	builder.Append("repairNum").Append(":").Append(repairNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

