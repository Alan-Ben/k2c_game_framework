using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 请求加入联盟
/// </summary>
public class GC2GS_032_002_ReqJoinGuild : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 联盟id
/// </summary>
private long guildId;


public GC2GS_032_002_ReqJoinGuild() {
	guildId = (long)0;
}

public GC2GS_032_002_ReqJoinGuild(
	long _guildId
) {	guildId = _guildId;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 联盟id
/// </summary>
public long getGuildId() { return guildId; }
/// <summary>
/// 联盟id
/// </summary>
public void setGuildId(long _guildId) { guildId = _guildId; }


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
	guildId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(guildId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)2);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

