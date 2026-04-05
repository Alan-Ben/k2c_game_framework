using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p012_ActivityTeamOp
{

/// <summary>
/// 创建队伍
/// </summary>
public class GC2GS_012_005_ReqCreateActivityTeam : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 加入方式
/// </summary>
private Common.CrossTeamEnum.ENPCrossTeamJoinType joinType;
/// <summary>
/// 申请条件
/// </summary>
private Common.CrossTeamObj.CrossTeam_SetInfo_Join joinCond;
/// <summary>
/// 队伍名称
/// </summary>
private string teamName;
/// <summary>
/// 队伍宣言
/// </summary>
private string teamDec;


public GC2GS_012_005_ReqCreateActivityTeam() {
	instanceId = (long)0;
	joinType = 0;
	joinCond = new Common.CrossTeamObj.CrossTeam_SetInfo_Join();
	teamName = "";
	teamDec = "";
}

public GC2GS_012_005_ReqCreateActivityTeam(
	long _instanceId
	, Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType
	, Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond
	, string _teamName
	, string _teamDec
) {	instanceId = _instanceId;
	joinType = _joinType;
	joinCond = _joinCond;
	teamName = _teamName;
	teamDec = _teamDec;
}

public byte getMainOrder() { return (byte)12; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 加入方式
/// </summary>
public Common.CrossTeamEnum.ENPCrossTeamJoinType getJoinType() { return joinType; }
/// <summary>
/// 加入方式
/// </summary>
public void setJoinType(Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType) { joinType = _joinType; }
/// <summary>
/// 申请条件
/// </summary>
public Common.CrossTeamObj.CrossTeam_SetInfo_Join getJoinCond() { return joinCond; }
/// <summary>
/// 申请条件
/// </summary>
public void setJoinCond(Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond) { joinCond = _joinCond; }
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
	int _size = 28;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamDec);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamDec);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinType = (Common.CrossTeamEnum.ENPCrossTeamJoinType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _joinCondCustLen = _buf.getInt();
	int _joinCondCurPos = _buf.getCurPos();
	joinCond.ReadUnzipBuf(_buf, _joinCondCurPos + _joinCondCustLen);
	_buf.setPosition(_joinCondCurPos + _joinCondCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamDec = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putInt((int)joinType);

	_buf.putInt(joinCond.GetBufSize());
	joinCond.PutUnzipBuf(_buf);
	_buf.putString(teamName);
	_buf.putString(teamDec);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)5);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("joinType").Append(":").Append(joinType.ToString()).Append(", ");
	builder.Append("joinCond").Append(":").Append(joinCond == null ? "null" : joinCond.ToString()).Append(", ");
	builder.Append("teamName").Append(":").Append(teamName.ToString()).Append(", ");
	builder.Append("teamDec").Append(":").Append(teamDec.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

