using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CrossTeamObj
{

/// <summary>
/// 组队数据
/// </summary>
public class CrossTeam_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 组队基础数据
/// </summary>
private Common.CrossTeamObj.CrossTeam_BaseInfo teamBase;
/// <summary>
/// 加入方式
/// </summary>
private Common.CrossTeamEnum.ENPCrossTeamJoinType joinType;
/// <summary>
/// 加入条件
/// </summary>
private Common.CrossTeamObj.CrossTeam_SetInfo_Join joinCond;
/// <summary>
/// 成员列表
/// </summary>
private List<Common.CrossTeamObj.CrossTeamMember_Info> memberList;


public CrossTeam_Info() {
	teamBase = new Common.CrossTeamObj.CrossTeam_BaseInfo();
	joinType = 0;
	joinCond = new Common.CrossTeamObj.CrossTeam_SetInfo_Join();
	memberList = new List<Common.CrossTeamObj.CrossTeamMember_Info>();
}

public CrossTeam_Info(
	Common.CrossTeamObj.CrossTeam_BaseInfo _teamBase
	, Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType
	, Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond
	, List<Common.CrossTeamObj.CrossTeamMember_Info> _memberList
) {	teamBase = _teamBase;
	joinType = _joinType;
	joinCond = _joinCond;
	memberList = _memberList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 组队基础数据
/// </summary>
public Common.CrossTeamObj.CrossTeam_BaseInfo getTeamBase() { return teamBase; }
/// <summary>
/// 组队基础数据
/// </summary>
public void setTeamBase(Common.CrossTeamObj.CrossTeam_BaseInfo _teamBase) { teamBase = _teamBase; }
/// <summary>
/// 加入方式
/// </summary>
public Common.CrossTeamEnum.ENPCrossTeamJoinType getJoinType() { return joinType; }
/// <summary>
/// 加入方式
/// </summary>
public void setJoinType(Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType) { joinType = _joinType; }
/// <summary>
/// 加入条件
/// </summary>
public Common.CrossTeamObj.CrossTeam_SetInfo_Join getJoinCond() { return joinCond; }
/// <summary>
/// 加入条件
/// </summary>
public void setJoinCond(Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond) { joinCond = _joinCond; }
/// <summary>
/// 成员列表
/// </summary>
public List<Common.CrossTeamObj.CrossTeamMember_Info> getMemberList() { return memberList; }
/// <summary>
/// 成员列表
/// </summary>
public void addMemberList(Common.CrossTeamObj.CrossTeamMember_Info _memberList) { memberList.Add(_memberList); }


public int GetBufSize() {
	int _size = 20;
	_size += 4 + teamBase.GetBufSize();
	_size += 2 + (memberList.Count * 24);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += 4 + teamBase.GetBufSize();
	_size += 2 + (memberList.Count * 24);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _teamBaseCustLen = _buf.getInt();
	int _teamBaseCurPos = _buf.getCurPos();
	teamBase.ReadUnzipBuf(_buf, _teamBaseCurPos + _teamBaseCustLen);
	_buf.setPosition(_teamBaseCurPos + _teamBaseCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinType = (Common.CrossTeamEnum.ENPCrossTeamJoinType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _joinCondCustLen = _buf.getInt();
	int _joinCondCurPos = _buf.getCurPos();
	joinCond.ReadUnzipBuf(_buf, _joinCondCurPos + _joinCondCustLen);
	_buf.setPosition(_joinCondCurPos + _joinCondCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _memberListCount = _buf.getShort();
	for(int _i = 0; _i < _memberListCount; _i++) { 
		Common.CrossTeamObj.CrossTeamMember_Info _memberList = new Common.CrossTeamObj.CrossTeamMember_Info();
		int __memberListCustLen = _buf.getInt();
	int __memberListCurPos = _buf.getCurPos();
	_memberList.ReadUnzipBuf(_buf, __memberListCurPos + __memberListCustLen);
	_buf.setPosition(__memberListCurPos + __memberListCustLen);

		memberList.Add(_memberList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(teamBase.GetBufSize());
	teamBase.PutUnzipBuf(_buf);
	_buf.putInt((int)joinType);

	_buf.putInt(joinCond.GetBufSize());
	joinCond.PutUnzipBuf(_buf);
	_buf.putShort((short)memberList.Count);
	for(int _i = 0; _i < memberList.Count; _i++) { 
		_buf.putInt(memberList[_i].GetBufSize());
	memberList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("teamBase").Append(":").Append(teamBase == null ? "null" : teamBase.ToString()).Append(", ");
	builder.Append("joinType").Append(":").Append(joinType.ToString()).Append(", ");
	builder.Append("joinCond").Append(":").Append(joinCond == null ? "null" : joinCond.ToString()).Append(", ");
	builder.Append("memberList").Append(":").Append(memberList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

