using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CrossTeamObj
{

/// <summary>
/// 组队成员数据
/// </summary>
public class CrossTeamMember_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 成员ID
/// </summary>
private long cid;
/// <summary>
/// 成员职位
/// </summary>
private Common.CrossTeamEnum.ENPCrossTeamMemberPos pos;
/// <summary>
/// 加入时间
/// </summary>
private long joinMs;


public CrossTeamMember_Info() {
	cid = (long)0;
	pos = 0;
	joinMs = (long)0;
}

public CrossTeamMember_Info(
	long _cid
	, Common.CrossTeamEnum.ENPCrossTeamMemberPos _pos
	, long _joinMs
) {	cid = _cid;
	pos = _pos;
	joinMs = _joinMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 成员ID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 成员ID
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 成员职位
/// </summary>
public Common.CrossTeamEnum.ENPCrossTeamMemberPos getPos() { return pos; }
/// <summary>
/// 成员职位
/// </summary>
public void setPos(Common.CrossTeamEnum.ENPCrossTeamMemberPos _pos) { pos = _pos; }
/// <summary>
/// 加入时间
/// </summary>
public long getJoinMs() { return joinMs; }
/// <summary>
/// 加入时间
/// </summary>
public void setJoinMs(long _joinMs) { joinMs = _joinMs; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pos = (Common.CrossTeamEnum.ENPCrossTeamMemberPos)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putInt((int)pos);

	_buf.putLong(joinMs);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("pos").Append(":").Append(pos.ToString()).Append(", ");
	builder.Append("joinMs").Append(":").Append(joinMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

