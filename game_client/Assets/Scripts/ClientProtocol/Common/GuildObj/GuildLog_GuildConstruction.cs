using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟建设
/// </summary>
public class GuildLog_GuildConstruction : ALBasicProtocolPack._IALProtocolStructure {
private string playerName;
private long refId;
/// <summary>
/// 联盟经验
/// </summary>
private int guildExp;
/// <summary>
/// 联盟财富
/// </summary>
private int guildWealth;
/// <summary>
/// 个人联盟币
/// </summary>
private int guildCoin;


public GuildLog_GuildConstruction() {
	playerName = "";
	refId = (long)0;
	guildExp = 0;
	guildWealth = 0;
	guildCoin = 0;
}

public GuildLog_GuildConstruction(
	string _playerName
	, long _refId
	, int _guildExp
	, int _guildWealth
	, int _guildCoin
) {	playerName = _playerName;
	refId = _refId;
	guildExp = _guildExp;
	guildWealth = _guildWealth;
	guildCoin = _guildCoin;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public string getPlayerName() { return playerName; }
public void setPlayerName(string _playerName) { playerName = _playerName; }
public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 联盟经验
/// </summary>
public int getGuildExp() { return guildExp; }
/// <summary>
/// 联盟经验
/// </summary>
public void setGuildExp(int _guildExp) { guildExp = _guildExp; }
/// <summary>
/// 联盟财富
/// </summary>
public int getGuildWealth() { return guildWealth; }
/// <summary>
/// 联盟财富
/// </summary>
public void setGuildWealth(int _guildWealth) { guildWealth = _guildWealth; }
/// <summary>
/// 个人联盟币
/// </summary>
public int getGuildCoin() { return guildCoin; }
/// <summary>
/// 个人联盟币
/// </summary>
public void setGuildCoin(int _guildCoin) { guildCoin = _guildCoin; }


public int GetBufSize() {
	int _size = 20;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildExp = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildWealth = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildCoin = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(playerName);
	_buf.putLong(refId);
	_buf.putInt(guildExp);
	_buf.putInt(guildWealth);
	_buf.putInt(guildCoin);
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
	builder.Append("playerName").Append(":").Append(playerName.ToString()).Append(", ");
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("guildExp").Append(":").Append(guildExp.ToString()).Append(", ");
	builder.Append("guildWealth").Append(":").Append(guildWealth.ToString()).Append(", ");
	builder.Append("guildCoin").Append(":").Append(guildCoin.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

