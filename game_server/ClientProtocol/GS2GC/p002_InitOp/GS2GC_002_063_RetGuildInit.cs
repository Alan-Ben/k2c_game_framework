using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_063_RetGuildInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 自己请求加入联盟列表
/// </summary>
private List<long> selfRequestJoinGuildList;
/// <summary>
/// 加入联盟CD信息
/// </summary>
private Common.GuildObj.Guild_JoinCdInfo joinCdInfo;
/// <summary>
/// 每日数据
/// </summary>
private Common.GuildObj.Guild_MemberDailyData dailyData;
private Common.GuildObj.Guild_DetailInfo guildInfo;


public GS2GC_002_063_RetGuildInit() {
	selfRequestJoinGuildList = new List<long>();
	joinCdInfo = new Common.GuildObj.Guild_JoinCdInfo();
	dailyData = new Common.GuildObj.Guild_MemberDailyData();
	guildInfo = new Common.GuildObj.Guild_DetailInfo();
}

public GS2GC_002_063_RetGuildInit(
	List<long> _selfRequestJoinGuildList
	, Common.GuildObj.Guild_JoinCdInfo _joinCdInfo
	, Common.GuildObj.Guild_MemberDailyData _dailyData
	, Common.GuildObj.Guild_DetailInfo _guildInfo
) {	selfRequestJoinGuildList = _selfRequestJoinGuildList;
	joinCdInfo = _joinCdInfo;
	dailyData = _dailyData;
	guildInfo = _guildInfo;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)63; }

/// <summary>
/// 自己请求加入联盟列表
/// </summary>
public List<long> getSelfRequestJoinGuildList() { return selfRequestJoinGuildList; }
/// <summary>
/// 自己请求加入联盟列表
/// </summary>
public void addSelfRequestJoinGuildList(long _selfRequestJoinGuildList) { selfRequestJoinGuildList.Add(_selfRequestJoinGuildList); }
/// <summary>
/// 加入联盟CD信息
/// </summary>
public Common.GuildObj.Guild_JoinCdInfo getJoinCdInfo() { return joinCdInfo; }
/// <summary>
/// 加入联盟CD信息
/// </summary>
public void setJoinCdInfo(Common.GuildObj.Guild_JoinCdInfo _joinCdInfo) { joinCdInfo = _joinCdInfo; }
/// <summary>
/// 每日数据
/// </summary>
public Common.GuildObj.Guild_MemberDailyData getDailyData() { return dailyData; }
/// <summary>
/// 每日数据
/// </summary>
public void setDailyData(Common.GuildObj.Guild_MemberDailyData _dailyData) { dailyData = _dailyData; }
public Common.GuildObj.Guild_DetailInfo getGuildInfo() { return guildInfo; }
public void setGuildInfo(Common.GuildObj.Guild_DetailInfo _guildInfo) { guildInfo = _guildInfo; }


public int GetBufSize() {
	int _size = 16;
	_size += 2 + (selfRequestJoinGuildList.Count * 8);
	_size += 4 + dailyData.GetBufSize();
	_size += 4 + guildInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (selfRequestJoinGuildList.Count * 8);
	_size += 4 + dailyData.GetBufSize();
	_size += 4 + guildInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _selfRequestJoinGuildListCount = _buf.getShort();
	for(int _i = 0; _i < _selfRequestJoinGuildListCount; _i++) { 
		long _selfRequestJoinGuildList = (long)0;
		_selfRequestJoinGuildList = _buf.getLong();
		selfRequestJoinGuildList.Add(_selfRequestJoinGuildList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _joinCdInfoCustLen = _buf.getInt();
	int _joinCdInfoCurPos = _buf.getCurPos();
	joinCdInfo.ReadUnzipBuf(_buf, _joinCdInfoCurPos + _joinCdInfoCustLen);
	_buf.setPosition(_joinCdInfoCurPos + _joinCdInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _dailyDataCustLen = _buf.getInt();
	int _dailyDataCurPos = _buf.getCurPos();
	dailyData.ReadUnzipBuf(_buf, _dailyDataCurPos + _dailyDataCustLen);
	_buf.setPosition(_dailyDataCurPos + _dailyDataCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _guildInfoCustLen = _buf.getInt();
	int _guildInfoCurPos = _buf.getCurPos();
	guildInfo.ReadUnzipBuf(_buf, _guildInfoCurPos + _guildInfoCustLen);
	_buf.setPosition(_guildInfoCurPos + _guildInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)selfRequestJoinGuildList.Count);
	for(int _i = 0; _i < selfRequestJoinGuildList.Count; _i++) { 
		_buf.putLong(selfRequestJoinGuildList[_i]);
	}
	_buf.putInt(joinCdInfo.GetBufSize());
	joinCdInfo.PutUnzipBuf(_buf);
	_buf.putInt(dailyData.GetBufSize());
	dailyData.PutUnzipBuf(_buf);
	_buf.putInt(guildInfo.GetBufSize());
	guildInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)63);
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
	builder.Append("selfRequestJoinGuildList").Append(":").Append(selfRequestJoinGuildList.ToString()).Append(", ");
	builder.Append("joinCdInfo").Append(":").Append(joinCdInfo == null ? "null" : joinCdInfo.ToString()).Append(", ");
	builder.Append("dailyData").Append(":").Append(dailyData == null ? "null" : dailyData.ToString()).Append(", ");
	builder.Append("guildInfo").Append(":").Append(guildInfo == null ? "null" : guildInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

