using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p012_ActivityTeamOp
{

/// <summary>
/// 加入活动队伍
/// </summary>
public class GS2GC_012_051_OnJoinActivityTeam : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 队伍实例ID
/// </summary>
private long teamId;
/// <summary>
/// 活动队伍数据
/// </summary>
private Common.CrossTeamObj.CrossTeam_Info team;


public GS2GC_012_051_OnJoinActivityTeam() {
	teamId = (long)0;
	team = new Common.CrossTeamObj.CrossTeam_Info();
}

public GS2GC_012_051_OnJoinActivityTeam(
	long _teamId
	, Common.CrossTeamObj.CrossTeam_Info _team
) {	teamId = _teamId;
	team = _team;
}

public byte getMainOrder() { return (byte)12; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 队伍实例ID
/// </summary>
public long getTeamId() { return teamId; }
/// <summary>
/// 队伍实例ID
/// </summary>
public void setTeamId(long _teamId) { teamId = _teamId; }
/// <summary>
/// 活动队伍数据
/// </summary>
public Common.CrossTeamObj.CrossTeam_Info getTeam() { return team; }
/// <summary>
/// 活动队伍数据
/// </summary>
public void setTeam(Common.CrossTeamObj.CrossTeam_Info _team) { team = _team; }


public int GetBufSize() {
	int _size = 8;
	_size += 4 + team.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + team.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _teamCustLen = _buf.getInt();
	int _teamCurPos = _buf.getCurPos();
	team.ReadUnzipBuf(_buf, _teamCurPos + _teamCustLen);
	_buf.setPosition(_teamCurPos + _teamCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putInt(team.GetBufSize());
	team.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)51);
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
	builder.Append("team").Append(":").Append(team == null ? "null" : team.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

