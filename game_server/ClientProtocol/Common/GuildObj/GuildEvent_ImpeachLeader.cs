using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟弹劾盟主事件
/// </summary>
public class GuildEvent_ImpeachLeader : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 事件状态
/// </summary>
private Common.GuildEnum.EGuildImpeachLeaderEventState state;
/// <summary>
/// 盟主cid
/// </summary>
private long leaderCid;
/// <summary>
/// 发起时间戳
/// </summary>
private long requestTimeMs;
/// <summary>
/// 同意列表
/// </summary>
private List<long> agreeMemberCidList;


public GuildEvent_ImpeachLeader() {
	state = 0;
	leaderCid = (long)0;
	requestTimeMs = (long)0;
	agreeMemberCidList = new List<long>();
}

public GuildEvent_ImpeachLeader(
	Common.GuildEnum.EGuildImpeachLeaderEventState _state
	, long _leaderCid
	, long _requestTimeMs
	, List<long> _agreeMemberCidList
) {	state = _state;
	leaderCid = _leaderCid;
	requestTimeMs = _requestTimeMs;
	agreeMemberCidList = _agreeMemberCidList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 事件状态
/// </summary>
public Common.GuildEnum.EGuildImpeachLeaderEventState getState() { return state; }
/// <summary>
/// 事件状态
/// </summary>
public void setState(Common.GuildEnum.EGuildImpeachLeaderEventState _state) { state = _state; }
/// <summary>
/// 盟主cid
/// </summary>
public long getLeaderCid() { return leaderCid; }
/// <summary>
/// 盟主cid
/// </summary>
public void setLeaderCid(long _leaderCid) { leaderCid = _leaderCid; }
/// <summary>
/// 发起时间戳
/// </summary>
public long getRequestTimeMs() { return requestTimeMs; }
/// <summary>
/// 发起时间戳
/// </summary>
public void setRequestTimeMs(long _requestTimeMs) { requestTimeMs = _requestTimeMs; }
/// <summary>
/// 同意列表
/// </summary>
public List<long> getAgreeMemberCidList() { return agreeMemberCidList; }
/// <summary>
/// 同意列表
/// </summary>
public void addAgreeMemberCidList(long _agreeMemberCidList) { agreeMemberCidList.Add(_agreeMemberCidList); }


public int GetBufSize() {
	int _size = 20;
	_size += 2 + (agreeMemberCidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += 2 + (agreeMemberCidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	state = (Common.GuildEnum.EGuildImpeachLeaderEventState)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	leaderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	requestTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _agreeMemberCidListCount = _buf.getShort();
	for(int _i = 0; _i < _agreeMemberCidListCount; _i++) { 
		long _agreeMemberCidList = (long)0;
		_agreeMemberCidList = _buf.getLong();
		agreeMemberCidList.Add(_agreeMemberCidList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)state);

	_buf.putLong(leaderCid);
	_buf.putLong(requestTimeMs);
	_buf.putShort((short)agreeMemberCidList.Count);
	for(int _i = 0; _i < agreeMemberCidList.Count; _i++) { 
		_buf.putLong(agreeMemberCidList[_i]);
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
	builder.Append("state").Append(":").Append(state.ToString()).Append(", ");
	builder.Append("leaderCid").Append(":").Append(leaderCid.ToString()).Append(", ");
	builder.Append("requestTimeMs").Append(":").Append(requestTimeMs.ToString()).Append(", ");
	builder.Append("agreeMemberCidList").Append(":").Append(agreeMemberCidList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

