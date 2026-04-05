using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpPlayerInfoObj
{

/// <summary>
/// 玩家登录过的服务及服务器上的角色展示信息
/// </summary>
public class NP_SYS_PlayerJoinedUSInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 服务器信息
/// </summary>
private NPCommon.NP_SYS_ServerItem serverItem;
/// <summary>
/// 玩家cid
/// </summary>
private long cid;
/// <summary>
/// 玩家名
/// </summary>
private string playerName;
/// <summary>
/// 头像展示信息
/// </summary>
private NPCommon.PlayerInfo_IconShow iconShowInfo;
/// <summary>
/// 最后登录时间
/// </summary>
private long lastLoginTimeMs;
/// <summary>
/// 是否被封禁
/// </summary>
private bool isFreeze;
/// <summary>
/// 封禁结束时间
/// </summary>
private long freezeTimeMs;


public NP_SYS_PlayerJoinedUSInfo() {
	serverItem = new NPCommon.NP_SYS_ServerItem();
	cid = (long)0;
	playerName = "";
	iconShowInfo = new NPCommon.PlayerInfo_IconShow();
	lastLoginTimeMs = (long)0;
	isFreeze = false;
	freezeTimeMs = (long)0;
}

public NP_SYS_PlayerJoinedUSInfo(
	NPCommon.NP_SYS_ServerItem _serverItem
	, long _cid
	, string _playerName
	, NPCommon.PlayerInfo_IconShow _iconShowInfo
	, long _lastLoginTimeMs
	, bool _isFreeze
	, long _freezeTimeMs
) {	serverItem = _serverItem;
	cid = _cid;
	playerName = _playerName;
	iconShowInfo = _iconShowInfo;
	lastLoginTimeMs = _lastLoginTimeMs;
	isFreeze = _isFreeze;
	freezeTimeMs = _freezeTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 服务器信息
/// </summary>
public NPCommon.NP_SYS_ServerItem getServerItem() { return serverItem; }
/// <summary>
/// 服务器信息
/// </summary>
public void setServerItem(NPCommon.NP_SYS_ServerItem _serverItem) { serverItem = _serverItem; }
/// <summary>
/// 玩家cid
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家cid
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 玩家名
/// </summary>
public string getPlayerName() { return playerName; }
/// <summary>
/// 玩家名
/// </summary>
public void setPlayerName(string _playerName) { playerName = _playerName; }
/// <summary>
/// 头像展示信息
/// </summary>
public NPCommon.PlayerInfo_IconShow getIconShowInfo() { return iconShowInfo; }
/// <summary>
/// 头像展示信息
/// </summary>
public void setIconShowInfo(NPCommon.PlayerInfo_IconShow _iconShowInfo) { iconShowInfo = _iconShowInfo; }
/// <summary>
/// 最后登录时间
/// </summary>
public long getLastLoginTimeMs() { return lastLoginTimeMs; }
/// <summary>
/// 最后登录时间
/// </summary>
public void setLastLoginTimeMs(long _lastLoginTimeMs) { lastLoginTimeMs = _lastLoginTimeMs; }
/// <summary>
/// 是否被封禁
/// </summary>
public bool getIsFreeze() { return isFreeze; }
/// <summary>
/// 是否被封禁
/// </summary>
public void setIsFreeze(bool _isFreeze) { isFreeze = _isFreeze; }
/// <summary>
/// 封禁结束时间
/// </summary>
public long getFreezeTimeMs() { return freezeTimeMs; }
/// <summary>
/// 封禁结束时间
/// </summary>
public void setFreezeTimeMs(long _freezeTimeMs) { freezeTimeMs = _freezeTimeMs; }


public int GetBufSize() {
	int _size = 25;
	_size += 4 + serverItem.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + iconShowInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 27;
	_size += 4 + serverItem.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + iconShowInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _serverItemCustLen = _buf.getInt();
	int _serverItemCurPos = _buf.getCurPos();
	serverItem.ReadUnzipBuf(_buf, _serverItemCurPos + _serverItemCustLen);
	_buf.setPosition(_serverItemCurPos + _serverItemCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _iconShowInfoCustLen = _buf.getInt();
	int _iconShowInfoCurPos = _buf.getCurPos();
	iconShowInfo.ReadUnzipBuf(_buf, _iconShowInfoCurPos + _iconShowInfoCustLen);
	_buf.setPosition(_iconShowInfoCurPos + _iconShowInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastLoginTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isFreeze = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	freezeTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(serverItem.GetBufSize());
	serverItem.PutUnzipBuf(_buf);
	_buf.putLong(cid);
	_buf.putString(playerName);
	_buf.putInt(iconShowInfo.GetBufSize());
	iconShowInfo.PutUnzipBuf(_buf);
	_buf.putLong(lastLoginTimeMs);
	_buf.put(isFreeze?(byte)1:(byte)0);
	_buf.putLong(freezeTimeMs);
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
	builder.Append("serverItem").Append(":").Append(serverItem == null ? "null" : serverItem.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("playerName").Append(":").Append(playerName.ToString()).Append(", ");
	builder.Append("iconShowInfo").Append(":").Append(iconShowInfo == null ? "null" : iconShowInfo.ToString()).Append(", ");
	builder.Append("lastLoginTimeMs").Append(":").Append(lastLoginTimeMs.ToString()).Append(", ");
	builder.Append("isFreeze").Append(":").Append(isFreeze.ToString()).Append(", ");
	builder.Append("freezeTimeMs").Append(":").Append(freezeTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

