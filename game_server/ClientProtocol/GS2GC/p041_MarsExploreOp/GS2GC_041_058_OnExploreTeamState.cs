using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p041_MarsExploreOp
{

/// <summary>
/// 火星探索队伍-队伍状态变更
/// </summary>
public class GS2GC_041_058_OnExploreTeamState : ALBasicProtocolPack._IALProtocolStructure {
private long teamId;
private Common.MarsObj.Mars_TeamState state;


public GS2GC_041_058_OnExploreTeamState() {
	teamId = (long)0;
	state = new Common.MarsObj.Mars_TeamState();
}

public GS2GC_041_058_OnExploreTeamState(
	long _teamId
	, Common.MarsObj.Mars_TeamState _state
) {	teamId = _teamId;
	state = _state;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)58; }

public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }
public Common.MarsObj.Mars_TeamState getState() { return state; }
public void setState(Common.MarsObj.Mars_TeamState _state) { state = _state; }


public int GetBufSize() {
	int _size = 8;
	_size += 4 + state.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + state.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _stateCustLen = _buf.getInt();
	int _stateCurPos = _buf.getCurPos();
	state.ReadUnzipBuf(_buf, _stateCurPos + _stateCustLen);
	_buf.setPosition(_stateCurPos + _stateCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putInt(state.GetBufSize());
	state.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)58);
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
	builder.Append("state").Append(":").Append(state == null ? "null" : state.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

