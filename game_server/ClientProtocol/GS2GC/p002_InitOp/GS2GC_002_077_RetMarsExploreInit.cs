using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 火星探险数据初始化
/// </summary>
public class GS2GC_002_077_RetMarsExploreInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 探索数据
/// </summary>
private Common.MarsObj.Mars_Explore explore;
/// <summary>
/// 事件数据
/// </summary>
private List<Common.MarsObj.Mars_ExploreEvent> eventList;
/// <summary>
/// 队伍列表
/// </summary>
private List<Common.MarsObj.Mars_Team> teamList;
/// <summary>
/// 火星矿产数据索引列表
/// </summary>
private List<Common.MarsObj.Mars_MineIdx> mineIdx;


public GS2GC_002_077_RetMarsExploreInit() {
	explore = new Common.MarsObj.Mars_Explore();
	eventList = new List<Common.MarsObj.Mars_ExploreEvent>();
	teamList = new List<Common.MarsObj.Mars_Team>();
	mineIdx = new List<Common.MarsObj.Mars_MineIdx>();
}

public GS2GC_002_077_RetMarsExploreInit(
	Common.MarsObj.Mars_Explore _explore
	, List<Common.MarsObj.Mars_ExploreEvent> _eventList
	, List<Common.MarsObj.Mars_Team> _teamList
	, List<Common.MarsObj.Mars_MineIdx> _mineIdx
) {	explore = _explore;
	eventList = _eventList;
	teamList = _teamList;
	mineIdx = _mineIdx;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)77; }

/// <summary>
/// 探索数据
/// </summary>
public Common.MarsObj.Mars_Explore getExplore() { return explore; }
/// <summary>
/// 探索数据
/// </summary>
public void setExplore(Common.MarsObj.Mars_Explore _explore) { explore = _explore; }
/// <summary>
/// 事件数据
/// </summary>
public List<Common.MarsObj.Mars_ExploreEvent> getEventList() { return eventList; }
/// <summary>
/// 事件数据
/// </summary>
public void addEventList(Common.MarsObj.Mars_ExploreEvent _eventList) { eventList.Add(_eventList); }
/// <summary>
/// 队伍列表
/// </summary>
public List<Common.MarsObj.Mars_Team> getTeamList() { return teamList; }
/// <summary>
/// 队伍列表
/// </summary>
public void addTeamList(Common.MarsObj.Mars_Team _teamList) { teamList.Add(_teamList); }
/// <summary>
/// 火星矿产数据索引列表
/// </summary>
public List<Common.MarsObj.Mars_MineIdx> getMineIdx() { return mineIdx; }
/// <summary>
/// 火星矿产数据索引列表
/// </summary>
public void addMineIdx(Common.MarsObj.Mars_MineIdx _mineIdx) { mineIdx.Add(_mineIdx); }


public int GetBufSize() {
	int _size = 12;
	_size += 2 + (eventList.Count * 45);
	_size += 2;
for(int _i = 0; _i < teamList.Count; _i++) {
	_size += 4 + teamList[_i].GetBufSize();
	}

	_size += 2 + (mineIdx.Count * 44);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (eventList.Count * 45);
	_size += 2;
for(int _i = 0; _i < teamList.Count; _i++) {
	_size += 4 + teamList[_i].GetBufSize();
	}

	_size += 2 + (mineIdx.Count * 44);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _exploreCustLen = _buf.getInt();
	int _exploreCurPos = _buf.getCurPos();
	explore.ReadUnzipBuf(_buf, _exploreCurPos + _exploreCustLen);
	_buf.setPosition(_exploreCurPos + _exploreCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _eventListCount = _buf.getShort();
	for(int _i = 0; _i < _eventListCount; _i++) { 
		Common.MarsObj.Mars_ExploreEvent _eventList = new Common.MarsObj.Mars_ExploreEvent();
		int __eventListCustLen = _buf.getInt();
	int __eventListCurPos = _buf.getCurPos();
	_eventList.ReadUnzipBuf(_buf, __eventListCurPos + __eventListCustLen);
	_buf.setPosition(__eventListCurPos + __eventListCustLen);

		eventList.Add(_eventList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _teamListCount = _buf.getShort();
	for(int _i = 0; _i < _teamListCount; _i++) { 
		Common.MarsObj.Mars_Team _teamList = new Common.MarsObj.Mars_Team();
		int __teamListCustLen = _buf.getInt();
	int __teamListCurPos = _buf.getCurPos();
	_teamList.ReadUnzipBuf(_buf, __teamListCurPos + __teamListCustLen);
	_buf.setPosition(__teamListCurPos + __teamListCustLen);

		teamList.Add(_teamList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _mineIdxCount = _buf.getShort();
	for(int _i = 0; _i < _mineIdxCount; _i++) { 
		Common.MarsObj.Mars_MineIdx _mineIdx = new Common.MarsObj.Mars_MineIdx();
		int __mineIdxCustLen = _buf.getInt();
	int __mineIdxCurPos = _buf.getCurPos();
	_mineIdx.ReadUnzipBuf(_buf, __mineIdxCurPos + __mineIdxCustLen);
	_buf.setPosition(__mineIdxCurPos + __mineIdxCustLen);

		mineIdx.Add(_mineIdx);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(explore.GetBufSize());
	explore.PutUnzipBuf(_buf);
	_buf.putShort((short)eventList.Count);
	for(int _i = 0; _i < eventList.Count; _i++) { 
		_buf.putInt(eventList[_i].GetBufSize());
	eventList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)teamList.Count);
	for(int _i = 0; _i < teamList.Count; _i++) { 
		_buf.putInt(teamList[_i].GetBufSize());
	teamList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)mineIdx.Count);
	for(int _i = 0; _i < mineIdx.Count; _i++) { 
		_buf.putInt(mineIdx[_i].GetBufSize());
	mineIdx[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)77);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)77);
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
	builder.Append("explore").Append(":").Append(explore == null ? "null" : explore.ToString()).Append(", ");
	builder.Append("eventList").Append(":").Append(eventList.ToString()).Append(", ");
	builder.Append("teamList").Append(":").Append(teamList.ToString()).Append(", ");
	builder.Append("mineIdx").Append(":").Append(mineIdx.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

