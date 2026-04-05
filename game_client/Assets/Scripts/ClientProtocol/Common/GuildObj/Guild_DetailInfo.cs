using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟详细信息
/// </summary>
public class Guild_DetailInfo : ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_ShowInfo guildInfo;
/// <summary>
/// 联盟财富
/// </summary>
private long guildWealth;
/// <summary>
/// 公告
/// </summary>
private string announcement;
/// <summary>
/// 成员列表
/// </summary>
private List<Common.GuildObj.Guild_MemberBaseInfo> memberList;
/// <summary>
/// 入盟请求列表
/// </summary>
private List<Common.GuildObj.Guild_JoinRequestInfo> joinRequestList;
/// <summary>
/// 建造信息列表
/// </summary>
private Common.GuildObj.Guild_ConstructList constructList;
/// <summary>
/// 事件列表
/// </summary>
private List<Common.GuildObj.Guild_EventInfo> eventList;
/// <summary>
/// 下次可公开招募时间
/// </summary>
private long nextCanRecruitTimeMs;
/// <summary>
/// 杂物委托信息
/// </summary>
private Common.GuildObj.Guild_EntrustInfo entrustInfo;
/// <summary>
/// 派遣信息
/// </summary>
private Common.GuildObj.Guild_DispatchData dispatchData;
/// <summary>
/// 贡献信息
/// </summary>
private Common.GuildObj.Guild_MemberContributeInfo selfContributeInfo;
/// <summary>
/// 最新联盟火星矿战报ID，用于红点判断
/// </summary>
private long latestMarsMineBattleReportId;


public Guild_DetailInfo() {
	guildInfo = new Common.GuildObj.Guild_ShowInfo();
	guildWealth = (long)0;
	announcement = "";
	memberList = new List<Common.GuildObj.Guild_MemberBaseInfo>();
	joinRequestList = new List<Common.GuildObj.Guild_JoinRequestInfo>();
	constructList = new Common.GuildObj.Guild_ConstructList();
	eventList = new List<Common.GuildObj.Guild_EventInfo>();
	nextCanRecruitTimeMs = (long)0;
	entrustInfo = new Common.GuildObj.Guild_EntrustInfo();
	dispatchData = new Common.GuildObj.Guild_DispatchData();
	selfContributeInfo = new Common.GuildObj.Guild_MemberContributeInfo();
	latestMarsMineBattleReportId = (long)0;
}

public Guild_DetailInfo(
	Common.GuildObj.Guild_ShowInfo _guildInfo
	, long _guildWealth
	, string _announcement
	, List<Common.GuildObj.Guild_MemberBaseInfo> _memberList
	, List<Common.GuildObj.Guild_JoinRequestInfo> _joinRequestList
	, Common.GuildObj.Guild_ConstructList _constructList
	, List<Common.GuildObj.Guild_EventInfo> _eventList
	, long _nextCanRecruitTimeMs
	, Common.GuildObj.Guild_EntrustInfo _entrustInfo
	, Common.GuildObj.Guild_DispatchData _dispatchData
	, Common.GuildObj.Guild_MemberContributeInfo _selfContributeInfo
	, long _latestMarsMineBattleReportId
) {	guildInfo = _guildInfo;
	guildWealth = _guildWealth;
	announcement = _announcement;
	memberList = _memberList;
	joinRequestList = _joinRequestList;
	constructList = _constructList;
	eventList = _eventList;
	nextCanRecruitTimeMs = _nextCanRecruitTimeMs;
	entrustInfo = _entrustInfo;
	dispatchData = _dispatchData;
	selfContributeInfo = _selfContributeInfo;
	latestMarsMineBattleReportId = _latestMarsMineBattleReportId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public Common.GuildObj.Guild_ShowInfo getGuildInfo() { return guildInfo; }
public void setGuildInfo(Common.GuildObj.Guild_ShowInfo _guildInfo) { guildInfo = _guildInfo; }
/// <summary>
/// 联盟财富
/// </summary>
public long getGuildWealth() { return guildWealth; }
/// <summary>
/// 联盟财富
/// </summary>
public void setGuildWealth(long _guildWealth) { guildWealth = _guildWealth; }
/// <summary>
/// 公告
/// </summary>
public string getAnnouncement() { return announcement; }
/// <summary>
/// 公告
/// </summary>
public void setAnnouncement(string _announcement) { announcement = _announcement; }
/// <summary>
/// 成员列表
/// </summary>
public List<Common.GuildObj.Guild_MemberBaseInfo> getMemberList() { return memberList; }
/// <summary>
/// 成员列表
/// </summary>
public void addMemberList(Common.GuildObj.Guild_MemberBaseInfo _memberList) { memberList.Add(_memberList); }
/// <summary>
/// 入盟请求列表
/// </summary>
public List<Common.GuildObj.Guild_JoinRequestInfo> getJoinRequestList() { return joinRequestList; }
/// <summary>
/// 入盟请求列表
/// </summary>
public void addJoinRequestList(Common.GuildObj.Guild_JoinRequestInfo _joinRequestList) { joinRequestList.Add(_joinRequestList); }
/// <summary>
/// 建造信息列表
/// </summary>
public Common.GuildObj.Guild_ConstructList getConstructList() { return constructList; }
/// <summary>
/// 建造信息列表
/// </summary>
public void setConstructList(Common.GuildObj.Guild_ConstructList _constructList) { constructList = _constructList; }
/// <summary>
/// 事件列表
/// </summary>
public List<Common.GuildObj.Guild_EventInfo> getEventList() { return eventList; }
/// <summary>
/// 事件列表
/// </summary>
public void addEventList(Common.GuildObj.Guild_EventInfo _eventList) { eventList.Add(_eventList); }
/// <summary>
/// 下次可公开招募时间
/// </summary>
public long getNextCanRecruitTimeMs() { return nextCanRecruitTimeMs; }
/// <summary>
/// 下次可公开招募时间
/// </summary>
public void setNextCanRecruitTimeMs(long _nextCanRecruitTimeMs) { nextCanRecruitTimeMs = _nextCanRecruitTimeMs; }
/// <summary>
/// 杂物委托信息
/// </summary>
public Common.GuildObj.Guild_EntrustInfo getEntrustInfo() { return entrustInfo; }
/// <summary>
/// 杂物委托信息
/// </summary>
public void setEntrustInfo(Common.GuildObj.Guild_EntrustInfo _entrustInfo) { entrustInfo = _entrustInfo; }
/// <summary>
/// 派遣信息
/// </summary>
public Common.GuildObj.Guild_DispatchData getDispatchData() { return dispatchData; }
/// <summary>
/// 派遣信息
/// </summary>
public void setDispatchData(Common.GuildObj.Guild_DispatchData _dispatchData) { dispatchData = _dispatchData; }
/// <summary>
/// 贡献信息
/// </summary>
public Common.GuildObj.Guild_MemberContributeInfo getSelfContributeInfo() { return selfContributeInfo; }
/// <summary>
/// 贡献信息
/// </summary>
public void setSelfContributeInfo(Common.GuildObj.Guild_MemberContributeInfo _selfContributeInfo) { selfContributeInfo = _selfContributeInfo; }
/// <summary>
/// 最新联盟火星矿战报ID，用于红点判断
/// </summary>
public long getLatestMarsMineBattleReportId() { return latestMarsMineBattleReportId; }
/// <summary>
/// 最新联盟火星矿战报ID，用于红点判断
/// </summary>
public void setLatestMarsMineBattleReportId(long _latestMarsMineBattleReportId) { latestMarsMineBattleReportId = _latestMarsMineBattleReportId; }


public int GetBufSize() {
	int _size = 84;
	_size += 4 + guildInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(announcement);
	_size += 2 + (memberList.Count * 16);
	_size += 2 + (joinRequestList.Count * 28);
	_size += 4 + constructList.GetBufSize();
	_size += 2;
for(int _i = 0; _i < eventList.Count; _i++) {
	_size += 4 + eventList[_i].GetBufSize();
	}

	_size += 4 + dispatchData.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 86;
	_size += 4 + guildInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(announcement);
	_size += 2 + (memberList.Count * 16);
	_size += 2 + (joinRequestList.Count * 28);
	_size += 4 + constructList.GetBufSize();
	_size += 2;
for(int _i = 0; _i < eventList.Count; _i++) {
	_size += 4 + eventList[_i].GetBufSize();
	}

	_size += 4 + dispatchData.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _guildInfoCustLen = _buf.getInt();
	int _guildInfoCurPos = _buf.getCurPos();
	guildInfo.ReadUnzipBuf(_buf, _guildInfoCurPos + _guildInfoCustLen);
	_buf.setPosition(_guildInfoCurPos + _guildInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildWealth = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	announcement = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _memberListCount = _buf.getShort();
	for(int _i = 0; _i < _memberListCount; _i++) { 
		Common.GuildObj.Guild_MemberBaseInfo _memberList = new Common.GuildObj.Guild_MemberBaseInfo();
		int __memberListCustLen = _buf.getInt();
	int __memberListCurPos = _buf.getCurPos();
	_memberList.ReadUnzipBuf(_buf, __memberListCurPos + __memberListCustLen);
	_buf.setPosition(__memberListCurPos + __memberListCustLen);

		memberList.Add(_memberList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _joinRequestListCount = _buf.getShort();
	for(int _i = 0; _i < _joinRequestListCount; _i++) { 
		Common.GuildObj.Guild_JoinRequestInfo _joinRequestList = new Common.GuildObj.Guild_JoinRequestInfo();
		int __joinRequestListCustLen = _buf.getInt();
	int __joinRequestListCurPos = _buf.getCurPos();
	_joinRequestList.ReadUnzipBuf(_buf, __joinRequestListCurPos + __joinRequestListCustLen);
	_buf.setPosition(__joinRequestListCurPos + __joinRequestListCustLen);

		joinRequestList.Add(_joinRequestList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _constructListCustLen = _buf.getInt();
	int _constructListCurPos = _buf.getCurPos();
	constructList.ReadUnzipBuf(_buf, _constructListCurPos + _constructListCustLen);
	_buf.setPosition(_constructListCurPos + _constructListCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _eventListCount = _buf.getShort();
	for(int _i = 0; _i < _eventListCount; _i++) { 
		Common.GuildObj.Guild_EventInfo _eventList = new Common.GuildObj.Guild_EventInfo();
		int __eventListCustLen = _buf.getInt();
	int __eventListCurPos = _buf.getCurPos();
	_eventList.ReadUnzipBuf(_buf, __eventListCurPos + __eventListCustLen);
	_buf.setPosition(__eventListCurPos + __eventListCustLen);

		eventList.Add(_eventList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextCanRecruitTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _entrustInfoCustLen = _buf.getInt();
	int _entrustInfoCurPos = _buf.getCurPos();
	entrustInfo.ReadUnzipBuf(_buf, _entrustInfoCurPos + _entrustInfoCustLen);
	_buf.setPosition(_entrustInfoCurPos + _entrustInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _dispatchDataCustLen = _buf.getInt();
	int _dispatchDataCurPos = _buf.getCurPos();
	dispatchData.ReadUnzipBuf(_buf, _dispatchDataCurPos + _dispatchDataCustLen);
	_buf.setPosition(_dispatchDataCurPos + _dispatchDataCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _selfContributeInfoCustLen = _buf.getInt();
	int _selfContributeInfoCurPos = _buf.getCurPos();
	selfContributeInfo.ReadUnzipBuf(_buf, _selfContributeInfoCurPos + _selfContributeInfoCustLen);
	_buf.setPosition(_selfContributeInfoCurPos + _selfContributeInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	latestMarsMineBattleReportId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(guildInfo.GetBufSize());
	guildInfo.PutUnzipBuf(_buf);
	_buf.putLong(guildWealth);
	_buf.putString(announcement);
	_buf.putShort((short)memberList.Count);
	for(int _i = 0; _i < memberList.Count; _i++) { 
		_buf.putInt(memberList[_i].GetBufSize());
	memberList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)joinRequestList.Count);
	for(int _i = 0; _i < joinRequestList.Count; _i++) { 
		_buf.putInt(joinRequestList[_i].GetBufSize());
	joinRequestList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(constructList.GetBufSize());
	constructList.PutUnzipBuf(_buf);
	_buf.putShort((short)eventList.Count);
	for(int _i = 0; _i < eventList.Count; _i++) { 
		_buf.putInt(eventList[_i].GetBufSize());
	eventList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(nextCanRecruitTimeMs);
	_buf.putInt(entrustInfo.GetBufSize());
	entrustInfo.PutUnzipBuf(_buf);
	_buf.putInt(dispatchData.GetBufSize());
	dispatchData.PutUnzipBuf(_buf);
	_buf.putInt(selfContributeInfo.GetBufSize());
	selfContributeInfo.PutUnzipBuf(_buf);
	_buf.putLong(latestMarsMineBattleReportId);
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
	builder.Append("guildInfo").Append(":").Append(guildInfo == null ? "null" : guildInfo.ToString()).Append(", ");
	builder.Append("guildWealth").Append(":").Append(guildWealth.ToString()).Append(", ");
	builder.Append("announcement").Append(":").Append(announcement.ToString()).Append(", ");
	builder.Append("memberList").Append(":").Append(memberList.ToString()).Append(", ");
	builder.Append("joinRequestList").Append(":").Append(joinRequestList.ToString()).Append(", ");
	builder.Append("constructList").Append(":").Append(constructList == null ? "null" : constructList.ToString()).Append(", ");
	builder.Append("eventList").Append(":").Append(eventList.ToString()).Append(", ");
	builder.Append("nextCanRecruitTimeMs").Append(":").Append(nextCanRecruitTimeMs.ToString()).Append(", ");
	builder.Append("entrustInfo").Append(":").Append(entrustInfo == null ? "null" : entrustInfo.ToString()).Append(", ");
	builder.Append("dispatchData").Append(":").Append(dispatchData == null ? "null" : dispatchData.ToString()).Append(", ");
	builder.Append("selfContributeInfo").Append(":").Append(selfContributeInfo == null ? "null" : selfContributeInfo.ToString()).Append(", ");
	builder.Append("latestMarsMineBattleReportId").Append(":").Append(latestMarsMineBattleReportId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

