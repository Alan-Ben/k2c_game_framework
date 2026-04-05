using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

/// <summary>
/// 返回单个集结信息
/// </summary>
public class GS2GC_042_062_RetQueryRallyInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 集结ID
/// </summary>
private long rallyId;
/// <summary>
/// 集结类型
/// </summary>
private Common.GuildEnum.EGuildRallyType rallyType;
/// <summary>
/// 创建者CID
/// </summary>
private long leaderCid;
/// <summary>
/// 创建者队伍ID
/// </summary>
private long leaderTeamId;
/// <summary>
/// 创建时间毫秒
/// </summary>
private long createTimeMs;
/// <summary>
/// 过期时间毫秒
/// </summary>
private long expireTimeMs;
/// <summary>
/// 最低战力限制
/// </summary>
private long minPowerLimit;
/// <summary>
/// 最大成员限制
/// </summary>
private int maxMemberLimit;
/// <summary>
/// 创建者队伍快照
/// </summary>
private Common.MarsObj.MarsBattleV2_MemberInfo leaderTeamSnapshot;
/// <summary>
/// 扩展数据
/// </summary>
private byte[] extData;
/// <summary>
/// 当前成员CID列表
/// </summary>
private List<long> memberCidList;


public GS2GC_042_062_RetQueryRallyInfo() {
	rallyId = (long)0;
	rallyType = 0;
	leaderCid = (long)0;
	leaderTeamId = (long)0;
	createTimeMs = (long)0;
	expireTimeMs = (long)0;
	minPowerLimit = (long)0;
	maxMemberLimit = 0;
	leaderTeamSnapshot = new Common.MarsObj.MarsBattleV2_MemberInfo();
	extData = null;
	memberCidList = new List<long>();
}

public GS2GC_042_062_RetQueryRallyInfo(
	long _rallyId
	, Common.GuildEnum.EGuildRallyType _rallyType
	, long _leaderCid
	, long _leaderTeamId
	, long _createTimeMs
	, long _expireTimeMs
	, long _minPowerLimit
	, int _maxMemberLimit
	, Common.MarsObj.MarsBattleV2_MemberInfo _leaderTeamSnapshot
	, byte[] _extData
	, List<long> _memberCidList
) {	rallyId = _rallyId;
	rallyType = _rallyType;
	leaderCid = _leaderCid;
	leaderTeamId = _leaderTeamId;
	createTimeMs = _createTimeMs;
	expireTimeMs = _expireTimeMs;
	minPowerLimit = _minPowerLimit;
	maxMemberLimit = _maxMemberLimit;
	leaderTeamSnapshot = _leaderTeamSnapshot;
	extData = _extData;
	memberCidList = _memberCidList;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)62; }

/// <summary>
/// 集结ID
/// </summary>
public long getRallyId() { return rallyId; }
/// <summary>
/// 集结ID
/// </summary>
public void setRallyId(long _rallyId) { rallyId = _rallyId; }
/// <summary>
/// 集结类型
/// </summary>
public Common.GuildEnum.EGuildRallyType getRallyType() { return rallyType; }
/// <summary>
/// 集结类型
/// </summary>
public void setRallyType(Common.GuildEnum.EGuildRallyType _rallyType) { rallyType = _rallyType; }
/// <summary>
/// 创建者CID
/// </summary>
public long getLeaderCid() { return leaderCid; }
/// <summary>
/// 创建者CID
/// </summary>
public void setLeaderCid(long _leaderCid) { leaderCid = _leaderCid; }
/// <summary>
/// 创建者队伍ID
/// </summary>
public long getLeaderTeamId() { return leaderTeamId; }
/// <summary>
/// 创建者队伍ID
/// </summary>
public void setLeaderTeamId(long _leaderTeamId) { leaderTeamId = _leaderTeamId; }
/// <summary>
/// 创建时间毫秒
/// </summary>
public long getCreateTimeMs() { return createTimeMs; }
/// <summary>
/// 创建时间毫秒
/// </summary>
public void setCreateTimeMs(long _createTimeMs) { createTimeMs = _createTimeMs; }
/// <summary>
/// 过期时间毫秒
/// </summary>
public long getExpireTimeMs() { return expireTimeMs; }
/// <summary>
/// 过期时间毫秒
/// </summary>
public void setExpireTimeMs(long _expireTimeMs) { expireTimeMs = _expireTimeMs; }
/// <summary>
/// 最低战力限制
/// </summary>
public long getMinPowerLimit() { return minPowerLimit; }
/// <summary>
/// 最低战力限制
/// </summary>
public void setMinPowerLimit(long _minPowerLimit) { minPowerLimit = _minPowerLimit; }
/// <summary>
/// 最大成员限制
/// </summary>
public int getMaxMemberLimit() { return maxMemberLimit; }
/// <summary>
/// 最大成员限制
/// </summary>
public void setMaxMemberLimit(int _maxMemberLimit) { maxMemberLimit = _maxMemberLimit; }
/// <summary>
/// 创建者队伍快照
/// </summary>
public Common.MarsObj.MarsBattleV2_MemberInfo getLeaderTeamSnapshot() { return leaderTeamSnapshot; }
/// <summary>
/// 创建者队伍快照
/// </summary>
public void setLeaderTeamSnapshot(Common.MarsObj.MarsBattleV2_MemberInfo _leaderTeamSnapshot) { leaderTeamSnapshot = _leaderTeamSnapshot; }
/// <summary>
/// 扩展数据
/// </summary>
public byte[] getExtData() { return extData; }

/// <summary>
/// 扩展数据
/// </summary>
public void setExtData(byte[] _extData) { extData = _extData; }

/// <summary>
/// 当前成员CID列表
/// </summary>
public List<long> getMemberCidList() { return memberCidList; }
/// <summary>
/// 当前成员CID列表
/// </summary>
public void addMemberCidList(long _memberCidList) { memberCidList.Add(_memberCidList); }


public int GetBufSize() {
	int _size = 88;
	_size += 4 + (extData == null ? 0 : extData.Length);
	_size += 2 + (memberCidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 90;
	_size += 4 + (extData == null ? 0 : extData.Length);
	_size += 2 + (memberCidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rallyId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rallyType = (Common.GuildEnum.EGuildRallyType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	leaderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	leaderTeamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expireTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	minPowerLimit = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxMemberLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _leaderTeamSnapshotCustLen = _buf.getInt();
	int _leaderTeamSnapshotCurPos = _buf.getCurPos();
	leaderTeamSnapshot.ReadUnzipBuf(_buf, _leaderTeamSnapshotCurPos + _leaderTeamSnapshotCustLen);
	_buf.setPosition(_leaderTeamSnapshotCurPos + _leaderTeamSnapshotCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extData = _buf.getByteBuffer();

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _memberCidListCount = _buf.getShort();
	for(int _i = 0; _i < _memberCidListCount; _i++) { 
		long _memberCidList = (long)0;
		_memberCidList = _buf.getLong();
		memberCidList.Add(_memberCidList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(rallyId);
	_buf.putInt((int)rallyType);

	_buf.putLong(leaderCid);
	_buf.putLong(leaderTeamId);
	_buf.putLong(createTimeMs);
	_buf.putLong(expireTimeMs);
	_buf.putLong(minPowerLimit);
	_buf.putInt(maxMemberLimit);
	_buf.putInt(leaderTeamSnapshot.GetBufSize());
	leaderTeamSnapshot.PutUnzipBuf(_buf);
	_buf.putByteBuffer(extData);

	_buf.putShort((short)memberCidList.Count);
	for(int _i = 0; _i < memberCidList.Count; _i++) { 
		_buf.putLong(memberCidList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
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
	builder.Append("rallyId").Append(":").Append(rallyId.ToString()).Append(", ");
	builder.Append("rallyType").Append(":").Append(rallyType.ToString()).Append(", ");
	builder.Append("leaderCid").Append(":").Append(leaderCid.ToString()).Append(", ");
	builder.Append("leaderTeamId").Append(":").Append(leaderTeamId.ToString()).Append(", ");
	builder.Append("createTimeMs").Append(":").Append(createTimeMs.ToString()).Append(", ");
	builder.Append("expireTimeMs").Append(":").Append(expireTimeMs.ToString()).Append(", ");
	builder.Append("minPowerLimit").Append(":").Append(minPowerLimit.ToString()).Append(", ");
	builder.Append("maxMemberLimit").Append(":").Append(maxMemberLimit.ToString()).Append(", ");
	builder.Append("leaderTeamSnapshot").Append(":").Append(leaderTeamSnapshot == null ? "null" : leaderTeamSnapshot.ToString()).Append(", ");
	builder.Append("extData").Append(":").Append(extData == null ? "null" : extData.ToString()).Append(", ");
	builder.Append("memberCidList").Append(":").Append(memberCidList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

