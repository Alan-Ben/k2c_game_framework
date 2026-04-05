using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-队伍数据
/// </summary>
public class Mars_Team : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 队伍ID
/// </summary>
private long teamId;
/// <summary>
/// 队伍名称
/// </summary>
private string name;
/// <summary>
/// 入驻大臣ID列表
/// </summary>
private List<long> heroIdList;
/// <summary>
/// 队伍状态
/// </summary>
private Common.MarsObj.Mars_TeamState state;
/// <summary>
/// 队伍损耗数量
/// </summary>
private long lossValue;


public Mars_Team() {
	teamId = (long)0;
	name = "";
	heroIdList = new List<long>();
	state = new Common.MarsObj.Mars_TeamState();
	lossValue = (long)0;
}

public Mars_Team(
	long _teamId
	, string _name
	, List<long> _heroIdList
	, Common.MarsObj.Mars_TeamState _state
	, long _lossValue
) {	teamId = _teamId;
	name = _name;
	heroIdList = _heroIdList;
	state = _state;
	lossValue = _lossValue;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 队伍ID
/// </summary>
public long getTeamId() { return teamId; }
/// <summary>
/// 队伍ID
/// </summary>
public void setTeamId(long _teamId) { teamId = _teamId; }
/// <summary>
/// 队伍名称
/// </summary>
public string getName() { return name; }
/// <summary>
/// 队伍名称
/// </summary>
public void setName(string _name) { name = _name; }
/// <summary>
/// 入驻大臣ID列表
/// </summary>
public List<long> getHeroIdList() { return heroIdList; }
/// <summary>
/// 入驻大臣ID列表
/// </summary>
public void addHeroIdList(long _heroIdList) { heroIdList.Add(_heroIdList); }
/// <summary>
/// 队伍状态
/// </summary>
public Common.MarsObj.Mars_TeamState getState() { return state; }
/// <summary>
/// 队伍状态
/// </summary>
public void setState(Common.MarsObj.Mars_TeamState _state) { state = _state; }
/// <summary>
/// 队伍损耗数量
/// </summary>
public long getLossValue() { return lossValue; }
/// <summary>
/// 队伍损耗数量
/// </summary>
public void setLossValue(long _lossValue) { lossValue = _lossValue; }


public int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 2 + (heroIdList.Count * 8);
	_size += 4 + state.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 2 + (heroIdList.Count * 8);
	_size += 4 + state.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _heroIdListCount = _buf.getShort();
	for(int _i = 0; _i < _heroIdListCount; _i++) { 
		long _heroIdList = (long)0;
		_heroIdList = _buf.getLong();
		heroIdList.Add(_heroIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _stateCustLen = _buf.getInt();
	int _stateCurPos = _buf.getCurPos();
	state.ReadUnzipBuf(_buf, _stateCurPos + _stateCustLen);
	_buf.setPosition(_stateCurPos + _stateCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lossValue = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putString(name);
	_buf.putShort((short)heroIdList.Count);
	for(int _i = 0; _i < heroIdList.Count; _i++) { 
		_buf.putLong(heroIdList[_i]);
	}
	_buf.putInt(state.GetBufSize());
	state.PutUnzipBuf(_buf);
	_buf.putLong(lossValue);
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
	builder.Append("teamId").Append(":").Append(teamId.ToString()).Append(", ");
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("heroIdList").Append(":").Append(heroIdList.ToString()).Append(", ");
	builder.Append("state").Append(":").Append(state == null ? "null" : state.ToString()).Append(", ");
	builder.Append("lossValue").Append(":").Append(lossValue.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

