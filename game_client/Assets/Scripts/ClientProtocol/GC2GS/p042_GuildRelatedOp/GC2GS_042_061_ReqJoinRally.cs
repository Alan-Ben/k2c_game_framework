using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p042_GuildRelatedOp
{

/// <summary>
/// 请求加入集结
/// </summary>
public class GC2GS_042_061_ReqJoinRally : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 集结ID
/// </summary>
private long rallyId;
/// <summary>
/// 队伍ID
/// </summary>
private long teamId;


public GC2GS_042_061_ReqJoinRally() {
	rallyId = (long)0;
	teamId = (long)0;
}

public GC2GS_042_061_ReqJoinRally(
	long _rallyId
	, long _teamId
) {	rallyId = _rallyId;
	teamId = _teamId;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 集结ID
/// </summary>
public long getRallyId() { return rallyId; }
/// <summary>
/// 集结ID
/// </summary>
public void setRallyId(long _rallyId) { rallyId = _rallyId; }
/// <summary>
/// 队伍ID
/// </summary>
public long getTeamId() { return teamId; }
/// <summary>
/// 队伍ID
/// </summary>
public void setTeamId(long _teamId) { teamId = _teamId; }


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
	rallyId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(rallyId);
	_buf.putLong(teamId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)61);
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
	builder.Append("teamId").Append(":").Append(teamId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

