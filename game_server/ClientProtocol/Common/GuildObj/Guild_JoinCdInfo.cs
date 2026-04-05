using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟加入cd信息
/// </summary>
public class Guild_JoinCdInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 联盟免cd加入次数
/// </summary>
private int guildFreeJoinCdNum;
/// <summary>
/// 加入联盟cd结束时间
/// </summary>
private long joinGuildCdEndTimeMs;


public Guild_JoinCdInfo() {
	guildFreeJoinCdNum = 0;
	joinGuildCdEndTimeMs = (long)0;
}

public Guild_JoinCdInfo(
	int _guildFreeJoinCdNum
	, long _joinGuildCdEndTimeMs
) {	guildFreeJoinCdNum = _guildFreeJoinCdNum;
	joinGuildCdEndTimeMs = _joinGuildCdEndTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 联盟免cd加入次数
/// </summary>
public int getGuildFreeJoinCdNum() { return guildFreeJoinCdNum; }
/// <summary>
/// 联盟免cd加入次数
/// </summary>
public void setGuildFreeJoinCdNum(int _guildFreeJoinCdNum) { guildFreeJoinCdNum = _guildFreeJoinCdNum; }
/// <summary>
/// 加入联盟cd结束时间
/// </summary>
public long getJoinGuildCdEndTimeMs() { return joinGuildCdEndTimeMs; }
/// <summary>
/// 加入联盟cd结束时间
/// </summary>
public void setJoinGuildCdEndTimeMs(long _joinGuildCdEndTimeMs) { joinGuildCdEndTimeMs = _joinGuildCdEndTimeMs; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildFreeJoinCdNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinGuildCdEndTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(guildFreeJoinCdNum);
	_buf.putLong(joinGuildCdEndTimeMs);
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
	builder.Append("guildFreeJoinCdNum").Append(":").Append(guildFreeJoinCdNum.ToString()).Append(", ");
	builder.Append("joinGuildCdEndTimeMs").Append(":").Append(joinGuildCdEndTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

