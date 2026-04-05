using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p012_ActivityTeamOp
{

/// <summary>
/// 踢出队伍玩家
/// </summary>
public class GC2GS_012_007_ReqKickActivityTeamPlayer : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 队伍实例ID
/// </summary>
private long teamId;
/// <summary>
/// 被踢玩家CID
/// </summary>
private long kickCid;


public GC2GS_012_007_ReqKickActivityTeamPlayer() {
	teamId = (long)0;
	kickCid = (long)0;
}

public GC2GS_012_007_ReqKickActivityTeamPlayer(
	long _teamId
	, long _kickCid
) {	teamId = _teamId;
	kickCid = _kickCid;
}

public byte getMainOrder() { return (byte)12; }

public byte getSubOrder() { return (byte)7; }

/// <summary>
/// 队伍实例ID
/// </summary>
public long getTeamId() { return teamId; }
/// <summary>
/// 队伍实例ID
/// </summary>
public void setTeamId(long _teamId) { teamId = _teamId; }
/// <summary>
/// 被踢玩家CID
/// </summary>
public long getKickCid() { return kickCid; }
/// <summary>
/// 被踢玩家CID
/// </summary>
public void setKickCid(long _kickCid) { kickCid = _kickCid; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	kickCid = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putLong(kickCid);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)7);
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
	builder.Append("kickCid").Append(":").Append(kickCid.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

