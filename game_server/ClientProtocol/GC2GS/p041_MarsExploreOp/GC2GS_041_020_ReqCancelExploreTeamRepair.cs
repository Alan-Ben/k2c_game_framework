using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p041_MarsExploreOp
{

/// <summary>
/// 火星探险-取消队伍维修
/// </summary>
public class GC2GS_041_020_ReqCancelExploreTeamRepair : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 队伍ID
/// </summary>
private long teamId;


public GC2GS_041_020_ReqCancelExploreTeamRepair() {
	teamId = (long)0;
}

public GC2GS_041_020_ReqCancelExploreTeamRepair(
	long _teamId
) {	teamId = _teamId;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)20; }

/// <summary>
/// 队伍ID
/// </summary>
public long getTeamId() { return teamId; }
/// <summary>
/// 队伍ID
/// </summary>
public void setTeamId(long _teamId) { teamId = _teamId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)20);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

