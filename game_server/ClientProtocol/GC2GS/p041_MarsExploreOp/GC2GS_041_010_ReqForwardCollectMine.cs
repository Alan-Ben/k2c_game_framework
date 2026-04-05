using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p041_MarsExploreOp
{

/// <summary>
/// 火星探险-前往采集
/// </summary>
public class GC2GS_041_010_ReqForwardCollectMine : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 派遣队伍ID
/// </summary>
private long teamId;
/// <summary>
/// 矿产实例ID
/// </summary>
private long id;
/// <summary>
/// 是否Pvp行为
/// </summary>
private bool isPvpOp;
/// <summary>
/// 是否已知有其他队伍前往
/// </summary>
private bool isOtherTeamForward;


public GC2GS_041_010_ReqForwardCollectMine() {
	teamId = (long)0;
	id = (long)0;
	isPvpOp = false;
	isOtherTeamForward = false;
}

public GC2GS_041_010_ReqForwardCollectMine(
	long _teamId
	, long _id
	, bool _isPvpOp
	, bool _isOtherTeamForward
) {	teamId = _teamId;
	id = _id;
	isPvpOp = _isPvpOp;
	isOtherTeamForward = _isOtherTeamForward;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)10; }

/// <summary>
/// 派遣队伍ID
/// </summary>
public long getTeamId() { return teamId; }
/// <summary>
/// 派遣队伍ID
/// </summary>
public void setTeamId(long _teamId) { teamId = _teamId; }
/// <summary>
/// 矿产实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 矿产实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 是否Pvp行为
/// </summary>
public bool getIsPvpOp() { return isPvpOp; }
/// <summary>
/// 是否Pvp行为
/// </summary>
public void setIsPvpOp(bool _isPvpOp) { isPvpOp = _isPvpOp; }
/// <summary>
/// 是否已知有其他队伍前往
/// </summary>
public bool getIsOtherTeamForward() { return isOtherTeamForward; }
/// <summary>
/// 是否已知有其他队伍前往
/// </summary>
public void setIsOtherTeamForward(bool _isOtherTeamForward) { isOtherTeamForward = _isOtherTeamForward; }


public int GetBufSize() {
	int _size = 18;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 20;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isPvpOp = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isOtherTeamForward = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putLong(id);
	_buf.put(isPvpOp?(byte)1:(byte)0);
	_buf.put(isOtherTeamForward?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)10);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("isPvpOp").Append(":").Append(isPvpOp.ToString()).Append(", ");
	builder.Append("isOtherTeamForward").Append(":").Append(isOtherTeamForward.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

