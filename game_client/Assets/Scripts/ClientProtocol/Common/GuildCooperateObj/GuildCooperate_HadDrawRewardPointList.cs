using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildCooperateObj
{

/// <summary>
/// 联盟协作已领取奖励点
/// </summary>
public class GuildCooperate_HadDrawRewardPointList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 上次刷新时间 ms
/// </summary>
private long lastRefreshTimeMs;
/// <summary>
/// 已领取奖励点列表
/// </summary>
private List<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> hadDrawList;


public GuildCooperate_HadDrawRewardPointList() {
	lastRefreshTimeMs = (long)0;
	hadDrawList = new List<Common.GuildCooperateObj.GuildCooperate_RewardPointPos>();
}

public GuildCooperate_HadDrawRewardPointList(
	long _lastRefreshTimeMs
	, List<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> _hadDrawList
) {	lastRefreshTimeMs = _lastRefreshTimeMs;
	hadDrawList = _hadDrawList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 上次刷新时间 ms
/// </summary>
public long getLastRefreshTimeMs() { return lastRefreshTimeMs; }
/// <summary>
/// 上次刷新时间 ms
/// </summary>
public void setLastRefreshTimeMs(long _lastRefreshTimeMs) { lastRefreshTimeMs = _lastRefreshTimeMs; }
/// <summary>
/// 已领取奖励点列表
/// </summary>
public List<Common.GuildCooperateObj.GuildCooperate_RewardPointPos> getHadDrawList() { return hadDrawList; }
/// <summary>
/// 已领取奖励点列表
/// </summary>
public void addHadDrawList(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _hadDrawList) { hadDrawList.Add(_hadDrawList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (hadDrawList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (hadDrawList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_RewardPointPos _hadDrawList = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
		int __hadDrawListCustLen = _buf.getInt();
	int __hadDrawListCurPos = _buf.getCurPos();
	_hadDrawList.ReadUnzipBuf(_buf, __hadDrawListCurPos + __hadDrawListCustLen);
	_buf.setPosition(__hadDrawListCurPos + __hadDrawListCustLen);

		hadDrawList.Add(_hadDrawList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(lastRefreshTimeMs);
	_buf.putShort((short)hadDrawList.Count);
	for(int _i = 0; _i < hadDrawList.Count; _i++) { 
		_buf.putInt(hadDrawList[_i].GetBufSize());
	hadDrawList[_i].PutUnzipBuf(_buf);
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
	builder.Append("lastRefreshTimeMs").Append(":").Append(lastRefreshTimeMs.ToString()).Append(", ");
	builder.Append("hadDrawList").Append(":").Append(hadDrawList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

