using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_065_OnJoinGuild : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否是创建联盟
/// </summary>
private bool isCreate;
private Common.GuildObj.Guild_DetailInfo guildInfo;


public GS2GC_032_065_OnJoinGuild() {
	isCreate = false;
	guildInfo = new Common.GuildObj.Guild_DetailInfo();
}

public GS2GC_032_065_OnJoinGuild(
	bool _isCreate
	, Common.GuildObj.Guild_DetailInfo _guildInfo
) {	isCreate = _isCreate;
	guildInfo = _guildInfo;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)65; }

/// <summary>
/// 是否是创建联盟
/// </summary>
public bool getIsCreate() { return isCreate; }
/// <summary>
/// 是否是创建联盟
/// </summary>
public void setIsCreate(bool _isCreate) { isCreate = _isCreate; }
public Common.GuildObj.Guild_DetailInfo getGuildInfo() { return guildInfo; }
public void setGuildInfo(Common.GuildObj.Guild_DetailInfo _guildInfo) { guildInfo = _guildInfo; }


public int GetBufSize() {
	int _size = 1;
	_size += 4 + guildInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;
	_size += 4 + guildInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isCreate = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _guildInfoCustLen = _buf.getInt();
	int _guildInfoCurPos = _buf.getCurPos();
	guildInfo.ReadUnzipBuf(_buf, _guildInfoCurPos + _guildInfoCustLen);
	_buf.setPosition(_guildInfoCurPos + _guildInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isCreate?(byte)1:(byte)0);
	_buf.putInt(guildInfo.GetBufSize());
	guildInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)65);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)65);
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
	builder.Append("isCreate").Append(":").Append(isCreate.ToString()).Append(", ");
	builder.Append("guildInfo").Append(":").Append(guildInfo == null ? "null" : guildInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

