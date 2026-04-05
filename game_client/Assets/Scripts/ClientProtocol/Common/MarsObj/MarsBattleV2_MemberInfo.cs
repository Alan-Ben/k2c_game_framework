using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探险-队伍战斗单个成员队伍信息
/// </summary>
public class MarsBattleV2_MemberInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 成员标记Id，用于区分战损
/// </summary>
private long memberId;
/// <summary>
/// 兵种等级
/// </summary>
private int teamSoldierLvl;
/// <summary>
/// 兵种数量
/// </summary>
private long soldierNum;
/// <summary>
/// 兵种原损耗量
/// </summary>
private long soldierLossValue;


public MarsBattleV2_MemberInfo() {
	memberId = (long)0;
	teamSoldierLvl = 0;
	soldierNum = (long)0;
	soldierLossValue = (long)0;
}

public MarsBattleV2_MemberInfo(
	long _memberId
	, int _teamSoldierLvl
	, long _soldierNum
	, long _soldierLossValue
) {	memberId = _memberId;
	teamSoldierLvl = _teamSoldierLvl;
	soldierNum = _soldierNum;
	soldierLossValue = _soldierLossValue;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 成员标记Id，用于区分战损
/// </summary>
public long getMemberId() { return memberId; }
/// <summary>
/// 成员标记Id，用于区分战损
/// </summary>
public void setMemberId(long _memberId) { memberId = _memberId; }
/// <summary>
/// 兵种等级
/// </summary>
public int getTeamSoldierLvl() { return teamSoldierLvl; }
/// <summary>
/// 兵种等级
/// </summary>
public void setTeamSoldierLvl(int _teamSoldierLvl) { teamSoldierLvl = _teamSoldierLvl; }
/// <summary>
/// 兵种数量
/// </summary>
public long getSoldierNum() { return soldierNum; }
/// <summary>
/// 兵种数量
/// </summary>
public void setSoldierNum(long _soldierNum) { soldierNum = _soldierNum; }
/// <summary>
/// 兵种原损耗量
/// </summary>
public long getSoldierLossValue() { return soldierLossValue; }
/// <summary>
/// 兵种原损耗量
/// </summary>
public void setSoldierLossValue(long _soldierLossValue) { soldierLossValue = _soldierLossValue; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	memberId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamSoldierLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	soldierNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	soldierLossValue = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(memberId);
	_buf.putInt(teamSoldierLvl);
	_buf.putLong(soldierNum);
	_buf.putLong(soldierLossValue);
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
	builder.Append("memberId").Append(":").Append(memberId.ToString()).Append(", ");
	builder.Append("teamSoldierLvl").Append(":").Append(teamSoldierLvl.ToString()).Append(", ");
	builder.Append("soldierNum").Append(":").Append(soldierNum.ToString()).Append(", ");
	builder.Append("soldierLossValue").Append(":").Append(soldierLossValue.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

