using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星队伍-加入集结行军状态数据
/// </summary>
public class MarsTeamState_JoinRally : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 联盟ID
/// </summary>
private long guildId;
/// <summary>
/// 集结ID
/// </summary>
private long rallyId;


public MarsTeamState_JoinRally() {
	guildId = (long)0;
	rallyId = (long)0;
}

public MarsTeamState_JoinRally(
	long _guildId
	, long _rallyId
) {	guildId = _guildId;
	rallyId = _rallyId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 联盟ID
/// </summary>
public long getGuildId() { return guildId; }
/// <summary>
/// 联盟ID
/// </summary>
public void setGuildId(long _guildId) { guildId = _guildId; }
/// <summary>
/// 集结ID
/// </summary>
public long getRallyId() { return rallyId; }
/// <summary>
/// 集结ID
/// </summary>
public void setRallyId(long _rallyId) { rallyId = _rallyId; }


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
	guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rallyId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(guildId);
	_buf.putLong(rallyId);
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
	builder.Append("guildId").Append(":").Append(guildId.ToString()).Append(", ");
	builder.Append("rallyId").Append(":").Append(rallyId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

