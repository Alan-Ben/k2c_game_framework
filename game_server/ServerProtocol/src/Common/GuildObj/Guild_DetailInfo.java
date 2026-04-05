package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟详细信息
 **/
public class Guild_DetailInfo implements ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_ShowInfo guildInfo;
/** 联盟财富 */
private long guildWealth;
/** 公告 */
private String announcement;
/** 成员列表 */
private java.util.ArrayList<Common.GuildObj.Guild_MemberBaseInfo> memberList;
/** 入盟请求列表 */
private java.util.ArrayList<Common.GuildObj.Guild_JoinRequestInfo> joinRequestList;
/** 建造信息列表 */
private Common.GuildObj.Guild_ConstructList constructList;
/** 事件列表 */
private java.util.ArrayList<Common.GuildObj.Guild_EventInfo> eventList;
/** 下次可公开招募时间 */
private long nextCanRecruitTimeMs;
/** 杂物委托信息 */
private Common.GuildObj.Guild_EntrustInfo entrustInfo;
/** 派遣信息 */
private Common.GuildObj.Guild_DispatchData dispatchData;
/** 贡献信息 */
private Common.GuildObj.Guild_MemberContributeInfo selfContributeInfo;
/** 最新联盟火星矿战报ID，用于红点判断 */
private long latestMarsMineBattleReportId;


public Guild_DetailInfo() {
	guildInfo = new Common.GuildObj.Guild_ShowInfo();
	guildWealth = (long)0;
	announcement = "";
	memberList = new java.util.ArrayList<Common.GuildObj.Guild_MemberBaseInfo>();
	joinRequestList = new java.util.ArrayList<Common.GuildObj.Guild_JoinRequestInfo>();
	constructList = new Common.GuildObj.Guild_ConstructList();
	eventList = new java.util.ArrayList<Common.GuildObj.Guild_EventInfo>();
	nextCanRecruitTimeMs = (long)0;
	entrustInfo = new Common.GuildObj.Guild_EntrustInfo();
	dispatchData = new Common.GuildObj.Guild_DispatchData();
	selfContributeInfo = new Common.GuildObj.Guild_MemberContributeInfo();
	latestMarsMineBattleReportId = (long)0;
}

public Guild_DetailInfo(
	 Common.GuildObj.Guild_ShowInfo _guildInfo
	, long _guildWealth
	, String _announcement
	, java.util.ArrayList<Common.GuildObj.Guild_MemberBaseInfo> _memberList
	, java.util.ArrayList<Common.GuildObj.Guild_JoinRequestInfo> _joinRequestList
	, Common.GuildObj.Guild_ConstructList _constructList
	, java.util.ArrayList<Common.GuildObj.Guild_EventInfo> _eventList
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.GuildObj.Guild_ShowInfo getGuildInfo() { return guildInfo; }
public void setGuildInfo(Common.GuildObj.Guild_ShowInfo _guildInfo) { guildInfo = _guildInfo; }
/** 联盟财富 */
public long getGuildWealth() { return guildWealth; }
/** 联盟财富 */
public void setGuildWealth(long _guildWealth) { guildWealth = _guildWealth; }
/** 公告 */
public String getAnnouncement() { return announcement; }
/** 公告 */
public void setAnnouncement(String _announcement) { announcement = _announcement; }
/** 成员列表 */
public java.util.ArrayList<Common.GuildObj.Guild_MemberBaseInfo> getMemberList() { return memberList; }
/** 成员列表 */
public void addMemberList(Common.GuildObj.Guild_MemberBaseInfo _memberList) { memberList.add(_memberList); }
/** 入盟请求列表 */
public java.util.ArrayList<Common.GuildObj.Guild_JoinRequestInfo> getJoinRequestList() { return joinRequestList; }
/** 入盟请求列表 */
public void addJoinRequestList(Common.GuildObj.Guild_JoinRequestInfo _joinRequestList) { joinRequestList.add(_joinRequestList); }
/** 建造信息列表 */
public Common.GuildObj.Guild_ConstructList getConstructList() { return constructList; }
/** 建造信息列表 */
public void setConstructList(Common.GuildObj.Guild_ConstructList _constructList) { constructList = _constructList; }
/** 事件列表 */
public java.util.ArrayList<Common.GuildObj.Guild_EventInfo> getEventList() { return eventList; }
/** 事件列表 */
public void addEventList(Common.GuildObj.Guild_EventInfo _eventList) { eventList.add(_eventList); }
/** 下次可公开招募时间 */
public long getNextCanRecruitTimeMs() { return nextCanRecruitTimeMs; }
/** 下次可公开招募时间 */
public void setNextCanRecruitTimeMs(long _nextCanRecruitTimeMs) { nextCanRecruitTimeMs = _nextCanRecruitTimeMs; }
/** 杂物委托信息 */
public Common.GuildObj.Guild_EntrustInfo getEntrustInfo() { return entrustInfo; }
/** 杂物委托信息 */
public void setEntrustInfo(Common.GuildObj.Guild_EntrustInfo _entrustInfo) { entrustInfo = _entrustInfo; }
/** 派遣信息 */
public Common.GuildObj.Guild_DispatchData getDispatchData() { return dispatchData; }
/** 派遣信息 */
public void setDispatchData(Common.GuildObj.Guild_DispatchData _dispatchData) { dispatchData = _dispatchData; }
/** 贡献信息 */
public Common.GuildObj.Guild_MemberContributeInfo getSelfContributeInfo() { return selfContributeInfo; }
/** 贡献信息 */
public void setSelfContributeInfo(Common.GuildObj.Guild_MemberContributeInfo _selfContributeInfo) { selfContributeInfo = _selfContributeInfo; }
/** 最新联盟火星矿战报ID，用于红点判断 */
public long getLatestMarsMineBattleReportId() { return latestMarsMineBattleReportId; }
/** 最新联盟火星矿战报ID，用于红点判断 */
public void setLatestMarsMineBattleReportId(long _latestMarsMineBattleReportId) { latestMarsMineBattleReportId = _latestMarsMineBattleReportId; }


public final int GetBufSize() {
	int _size = 84;
	_size += 4 + guildInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(announcement);
	_size += 2 + (memberList.size() * 16);
	_size += 2 + (joinRequestList.size() * 28);
	_size += 4 + constructList.GetBufSize();
	_size += 2;
	for(int _i = 0; _i < eventList.size(); _i++) {
	_size += 4 + eventList.get(_i).GetBufSize();
	}

	_size += 4 + dispatchData.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 86;
	_size += 4 + guildInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(announcement);
	_size += 2 + (memberList.size() * 16);
	_size += 2 + (joinRequestList.size() * 28);
	_size += 4 + constructList.GetBufSize();
	_size += 2;
	for(int _i = 0; _i < eventList.size(); _i++) {
	_size += 4 + eventList.get(_i).GetBufSize();
	}

	_size += 4 + dispatchData.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _guildInfoCustLen = _buf.getInt();
	int _guildInfoCurPos = _buf.position();
	guildInfo.ReadUnzipBuf(_buf, _guildInfoCurPos + _guildInfoCustLen);
	_buf.position(_guildInfoCurPos + _guildInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildWealth = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) announcement = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _memberListCount = _buf.getShort();
	for(int _i = 0; _i < _memberListCount; _i++) { 
		Common.GuildObj.Guild_MemberBaseInfo _memberList = new Common.GuildObj.Guild_MemberBaseInfo();
		if(_buf.remaining() <= 0) return;
	int __memberListCustLen = _buf.getInt();
	int __memberListCurPos = _buf.position();
	_memberList.ReadUnzipBuf(_buf, __memberListCurPos + __memberListCustLen);
	_buf.position(__memberListCurPos + __memberListCustLen);

		memberList.add(_memberList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _joinRequestListCount = _buf.getShort();
	for(int _i = 0; _i < _joinRequestListCount; _i++) { 
		Common.GuildObj.Guild_JoinRequestInfo _joinRequestList = new Common.GuildObj.Guild_JoinRequestInfo();
		if(_buf.remaining() <= 0) return;
	int __joinRequestListCustLen = _buf.getInt();
	int __joinRequestListCurPos = _buf.position();
	_joinRequestList.ReadUnzipBuf(_buf, __joinRequestListCurPos + __joinRequestListCustLen);
	_buf.position(__joinRequestListCurPos + __joinRequestListCustLen);

		joinRequestList.add(_joinRequestList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _constructListCustLen = _buf.getInt();
	int _constructListCurPos = _buf.position();
	constructList.ReadUnzipBuf(_buf, _constructListCurPos + _constructListCustLen);
	_buf.position(_constructListCurPos + _constructListCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _eventListCount = _buf.getShort();
	for(int _i = 0; _i < _eventListCount; _i++) { 
		Common.GuildObj.Guild_EventInfo _eventList = new Common.GuildObj.Guild_EventInfo();
		if(_buf.remaining() <= 0) return;
	int __eventListCustLen = _buf.getInt();
	int __eventListCurPos = _buf.position();
	_eventList.ReadUnzipBuf(_buf, __eventListCurPos + __eventListCustLen);
	_buf.position(__eventListCurPos + __eventListCustLen);

		eventList.add(_eventList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) nextCanRecruitTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _entrustInfoCustLen = _buf.getInt();
	int _entrustInfoCurPos = _buf.position();
	entrustInfo.ReadUnzipBuf(_buf, _entrustInfoCurPos + _entrustInfoCustLen);
	_buf.position(_entrustInfoCurPos + _entrustInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _dispatchDataCustLen = _buf.getInt();
	int _dispatchDataCurPos = _buf.position();
	dispatchData.ReadUnzipBuf(_buf, _dispatchDataCurPos + _dispatchDataCustLen);
	_buf.position(_dispatchDataCurPos + _dispatchDataCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _selfContributeInfoCustLen = _buf.getInt();
	int _selfContributeInfoCurPos = _buf.position();
	selfContributeInfo.ReadUnzipBuf(_buf, _selfContributeInfoCurPos + _selfContributeInfoCustLen);
	_buf.position(_selfContributeInfoCurPos + _selfContributeInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) latestMarsMineBattleReportId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(guildInfo.GetBufSize());
	guildInfo.PutUnzipBuf(_buf);
	_buf.putLong(guildWealth);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, announcement);
	_buf.putShort((short)memberList.size());
	for(int _i = 0; _i < memberList.size(); _i++) { 
		_buf.putInt(memberList.get(_i).GetBufSize());
	memberList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)joinRequestList.size());
	for(int _i = 0; _i < joinRequestList.size(); _i++) { 
		_buf.putInt(joinRequestList.get(_i).GetBufSize());
	joinRequestList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(constructList.GetBufSize());
	constructList.PutUnzipBuf(_buf);
	_buf.putShort((short)eventList.size());
	for(int _i = 0; _i < eventList.size(); _i++) { 
		_buf.putInt(eventList.get(_i).GetBufSize());
	eventList.get(_i).PutUnzipBuf(_buf);
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

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

