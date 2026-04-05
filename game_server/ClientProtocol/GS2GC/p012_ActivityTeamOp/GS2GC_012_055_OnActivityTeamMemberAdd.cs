using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p012_ActivityTeamOp
{

/// <summary>
/// 活动队伍成员增加
/// </summary>
public class GS2GC_012_055_OnActivityTeamMemberAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 队伍实例ID
/// </summary>
private long teamId;
/// <summary>
/// 队伍玩家数据
/// </summary>
private Common.CrossTeamObj.CrossTeamMember_Info member;


public GS2GC_012_055_OnActivityTeamMemberAdd() {
	teamId = (long)0;
	member = new Common.CrossTeamObj.CrossTeamMember_Info();
}

public GS2GC_012_055_OnActivityTeamMemberAdd(
	long _teamId
	, Common.CrossTeamObj.CrossTeamMember_Info _member
) {	teamId = _teamId;
	member = _member;
}

public byte getMainOrder() { return (byte)12; }

public byte getSubOrder() { return (byte)55; }

/// <summary>
/// 队伍实例ID
/// </summary>
public long getTeamId() { return teamId; }
/// <summary>
/// 队伍实例ID
/// </summary>
public void setTeamId(long _teamId) { teamId = _teamId; }
/// <summary>
/// 队伍玩家数据
/// </summary>
public Common.CrossTeamObj.CrossTeamMember_Info getMember() { return member; }
/// <summary>
/// 队伍玩家数据
/// </summary>
public void setMember(Common.CrossTeamObj.CrossTeamMember_Info _member) { member = _member; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _memberCustLen = _buf.getInt();
	int _memberCurPos = _buf.getCurPos();
	member.ReadUnzipBuf(_buf, _memberCurPos + _memberCustLen);
	_buf.setPosition(_memberCurPos + _memberCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putInt(member.GetBufSize());
	member.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)55);
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
	builder.Append("member").Append(":").Append(member == null ? "null" : member.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

