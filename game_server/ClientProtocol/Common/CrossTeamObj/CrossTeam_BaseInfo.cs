using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CrossTeamObj
{

/// <summary>
/// 组队基础数据
/// </summary>
public class CrossTeam_BaseInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 队伍实例ID
/// </summary>
private long teamId;
/// <summary>
/// 队伍成员上限
/// </summary>
private int memberLimit;
/// <summary>
/// 队伍名称
/// </summary>
private string teamName;
/// <summary>
/// 队伍宣言
/// </summary>
private string teamDec;


public CrossTeam_BaseInfo() {
	teamId = (long)0;
	memberLimit = 0;
	teamName = "";
	teamDec = "";
}

public CrossTeam_BaseInfo(
	long _teamId
	, int _memberLimit
	, string _teamName
	, string _teamDec
) {	teamId = _teamId;
	memberLimit = _memberLimit;
	teamName = _teamName;
	teamDec = _teamDec;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 队伍实例ID
/// </summary>
public long getTeamId() { return teamId; }
/// <summary>
/// 队伍实例ID
/// </summary>
public void setTeamId(long _teamId) { teamId = _teamId; }
/// <summary>
/// 队伍成员上限
/// </summary>
public int getMemberLimit() { return memberLimit; }
/// <summary>
/// 队伍成员上限
/// </summary>
public void setMemberLimit(int _memberLimit) { memberLimit = _memberLimit; }
/// <summary>
/// 队伍名称
/// </summary>
public string getTeamName() { return teamName; }
/// <summary>
/// 队伍名称
/// </summary>
public void setTeamName(string _teamName) { teamName = _teamName; }
/// <summary>
/// 队伍宣言
/// </summary>
public string getTeamDec() { return teamDec; }
/// <summary>
/// 队伍宣言
/// </summary>
public void setTeamDec(string _teamDec) { teamDec = _teamDec; }


public int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamDec);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamDec);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	memberLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamDec = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putInt(memberLimit);
	_buf.putString(teamName);
	_buf.putString(teamDec);
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
	builder.Append("teamId").Append(":").Append(teamId.ToString()).Append(", ");
	builder.Append("memberLimit").Append(":").Append(memberLimit.ToString()).Append(", ");
	builder.Append("teamName").Append(":").Append(teamName.ToString()).Append(", ");
	builder.Append("teamDec").Append(":").Append(teamDec.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

