using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class PlayerInfo_IconShow : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家id
/// </summary>
private long cid;
/// <summary>
/// 玩家名
/// </summary>
private string playerName;
/// <summary>
/// 头像id
/// </summary>
private long iconId;
/// <summary>
/// 头像框id
/// </summary>
private long iconBgkId;
/// <summary>
/// 气泡框id
/// </summary>
private long bubbleId;
/// <summary>
/// vip等级
/// </summary>
private long vipLvl;
/// <summary>
/// 玩家等级
/// </summary>
private long playerLvl;
/// <summary>
/// 联盟id
/// </summary>
private long guildId;
/// <summary>
/// 联盟名称
/// </summary>
private string guildName;
/// <summary>
/// true-在线 false-离线
/// </summary>
private bool isOnline;
/// <summary>
/// 最后一次离线时间戳（毫秒）
/// </summary>
private long lastOfflineMs;
/// <summary>
/// 最后一次在线时间戳（毫秒）
/// </summary>
private long lastOnlineMs;
/// <summary>
/// 联盟简称
/// </summary>
private string guildSimpleName;
/// <summary>
/// 总实力
/// </summary>
private long totalPower;
/// <summary>
/// 赚速
/// </summary>
private long earnings;
/// <summary>
/// 皮肤ID
/// </summary>
private long playerSkinId;
/// <summary>
/// 玩家当前穿戴称号数据
/// </summary>
private NPCommon.PlayerInfo_CurTitle curTitle;
/// <summary>
/// 是否展示称号
/// </summary>
private bool isShow;
/// <summary>
/// 玩家经验
/// </summary>
private long exp;


public PlayerInfo_IconShow() {
	cid = (long)0;
	playerName = "";
	iconId = (long)0;
	iconBgkId = (long)0;
	bubbleId = (long)0;
	vipLvl = (long)0;
	playerLvl = (long)0;
	guildId = (long)0;
	guildName = "";
	isOnline = false;
	lastOfflineMs = (long)0;
	lastOnlineMs = (long)0;
	guildSimpleName = "";
	totalPower = (long)0;
	earnings = (long)0;
	playerSkinId = (long)0;
	curTitle = new NPCommon.PlayerInfo_CurTitle();
	isShow = false;
	exp = (long)0;
}

public PlayerInfo_IconShow(
	long _cid
	, string _playerName
	, long _iconId
	, long _iconBgkId
	, long _bubbleId
	, long _vipLvl
	, long _playerLvl
	, long _guildId
	, string _guildName
	, bool _isOnline
	, long _lastOfflineMs
	, long _lastOnlineMs
	, string _guildSimpleName
	, long _totalPower
	, long _earnings
	, long _playerSkinId
	, NPCommon.PlayerInfo_CurTitle _curTitle
	, bool _isShow
	, long _exp
) {	cid = _cid;
	playerName = _playerName;
	iconId = _iconId;
	iconBgkId = _iconBgkId;
	bubbleId = _bubbleId;
	vipLvl = _vipLvl;
	playerLvl = _playerLvl;
	guildId = _guildId;
	guildName = _guildName;
	isOnline = _isOnline;
	lastOfflineMs = _lastOfflineMs;
	lastOnlineMs = _lastOnlineMs;
	guildSimpleName = _guildSimpleName;
	totalPower = _totalPower;
	earnings = _earnings;
	playerSkinId = _playerSkinId;
	curTitle = _curTitle;
	isShow = _isShow;
	exp = _exp;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 玩家id
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家id
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
/// 头像id
/// </summary>
public long getIconId() { return iconId; }
/// <summary>
/// 头像id
/// </summary>
public void setIconId(long _iconId) { iconId = _iconId; }
/// <summary>
/// 头像框id
/// </summary>
public long getIconBgkId() { return iconBgkId; }
/// <summary>
/// 头像框id
/// </summary>
public void setIconBgkId(long _iconBgkId) { iconBgkId = _iconBgkId; }
/// <summary>
/// 气泡框id
/// </summary>
public long getBubbleId() { return bubbleId; }
/// <summary>
/// 气泡框id
/// </summary>
public void setBubbleId(long _bubbleId) { bubbleId = _bubbleId; }
/// <summary>
/// vip等级
/// </summary>
public long getVipLvl() { return vipLvl; }
/// <summary>
/// vip等级
/// </summary>
public void setVipLvl(long _vipLvl) { vipLvl = _vipLvl; }
/// <summary>
/// 玩家等级
/// </summary>
public long getPlayerLvl() { return playerLvl; }
/// <summary>
/// 玩家等级
/// </summary>
public void setPlayerLvl(long _playerLvl) { playerLvl = _playerLvl; }
/// <summary>
/// 联盟id
/// </summary>
public long getGuildId() { return guildId; }
/// <summary>
/// 联盟id
/// </summary>
public void setGuildId(long _guildId) { guildId = _guildId; }
/// <summary>
/// 联盟名称
/// </summary>
public string getGuildName() { return guildName; }
/// <summary>
/// 联盟名称
/// </summary>
public void setGuildName(string _guildName) { guildName = _guildName; }
/// <summary>
/// true-在线 false-离线
/// </summary>
public bool getIsOnline() { return isOnline; }
/// <summary>
/// true-在线 false-离线
/// </summary>
public void setIsOnline(bool _isOnline) { isOnline = _isOnline; }
/// <summary>
/// 最后一次离线时间戳（毫秒）
/// </summary>
public long getLastOfflineMs() { return lastOfflineMs; }
/// <summary>
/// 最后一次离线时间戳（毫秒）
/// </summary>
public void setLastOfflineMs(long _lastOfflineMs) { lastOfflineMs = _lastOfflineMs; }
/// <summary>
/// 最后一次在线时间戳（毫秒）
/// </summary>
public long getLastOnlineMs() { return lastOnlineMs; }
/// <summary>
/// 最后一次在线时间戳（毫秒）
/// </summary>
public void setLastOnlineMs(long _lastOnlineMs) { lastOnlineMs = _lastOnlineMs; }
/// <summary>
/// 联盟简称
/// </summary>
public string getGuildSimpleName() { return guildSimpleName; }
/// <summary>
/// 联盟简称
/// </summary>
public void setGuildSimpleName(string _guildSimpleName) { guildSimpleName = _guildSimpleName; }
/// <summary>
/// 总实力
/// </summary>
public long getTotalPower() { return totalPower; }
/// <summary>
/// 总实力
/// </summary>
public void setTotalPower(long _totalPower) { totalPower = _totalPower; }
/// <summary>
/// 赚速
/// </summary>
public long getEarnings() { return earnings; }
/// <summary>
/// 赚速
/// </summary>
public void setEarnings(long _earnings) { earnings = _earnings; }
/// <summary>
/// 皮肤ID
/// </summary>
public long getPlayerSkinId() { return playerSkinId; }
/// <summary>
/// 皮肤ID
/// </summary>
public void setPlayerSkinId(long _playerSkinId) { playerSkinId = _playerSkinId; }
/// <summary>
/// 玩家当前穿戴称号数据
/// </summary>
public NPCommon.PlayerInfo_CurTitle getCurTitle() { return curTitle; }
/// <summary>
/// 玩家当前穿戴称号数据
/// </summary>
public void setCurTitle(NPCommon.PlayerInfo_CurTitle _curTitle) { curTitle = _curTitle; }
/// <summary>
/// 是否展示称号
/// </summary>
public bool getIsShow() { return isShow; }
/// <summary>
/// 是否展示称号
/// </summary>
public void setIsShow(bool _isShow) { isShow = _isShow; }
/// <summary>
/// 玩家经验
/// </summary>
public long getExp() { return exp; }
/// <summary>
/// 玩家经验
/// </summary>
public void setExp(long _exp) { exp = _exp; }


public int GetBufSize() {
	int _size = 106;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildSimpleName);
	_size += 4 + curTitle.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 108;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildSimpleName);
	_size += 4 + curTitle.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconBgkId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	bubbleId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	vipLvl = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerLvl = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isOnline = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastOfflineMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastOnlineMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildSimpleName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	earnings = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerSkinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _curTitleCustLen = _buf.getInt();
	int _curTitleCurPos = _buf.getCurPos();
	curTitle.ReadUnzipBuf(_buf, _curTitleCurPos + _curTitleCustLen);
	_buf.setPosition(_curTitleCurPos + _curTitleCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isShow = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putString(playerName);
	_buf.putLong(iconId);
	_buf.putLong(iconBgkId);
	_buf.putLong(bubbleId);
	_buf.putLong(vipLvl);
	_buf.putLong(playerLvl);
	_buf.putLong(guildId);
	_buf.putString(guildName);
	_buf.put(isOnline?(byte)1:(byte)0);
	_buf.putLong(lastOfflineMs);
	_buf.putLong(lastOnlineMs);
	_buf.putString(guildSimpleName);
	_buf.putLong(totalPower);
	_buf.putLong(earnings);
	_buf.putLong(playerSkinId);
	_buf.putInt(curTitle.GetBufSize());
	curTitle.PutUnzipBuf(_buf);
	_buf.put(isShow?(byte)1:(byte)0);
	_buf.putLong(exp);
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
	builder.Append("playerName").Append(":").Append(playerName.ToString()).Append(", ");
	builder.Append("iconId").Append(":").Append(iconId.ToString()).Append(", ");
	builder.Append("iconBgkId").Append(":").Append(iconBgkId.ToString()).Append(", ");
	builder.Append("bubbleId").Append(":").Append(bubbleId.ToString()).Append(", ");
	builder.Append("vipLvl").Append(":").Append(vipLvl.ToString()).Append(", ");
	builder.Append("playerLvl").Append(":").Append(playerLvl.ToString()).Append(", ");
	builder.Append("guildId").Append(":").Append(guildId.ToString()).Append(", ");
	builder.Append("guildName").Append(":").Append(guildName.ToString()).Append(", ");
	builder.Append("isOnline").Append(":").Append(isOnline.ToString()).Append(", ");
	builder.Append("lastOfflineMs").Append(":").Append(lastOfflineMs.ToString()).Append(", ");
	builder.Append("lastOnlineMs").Append(":").Append(lastOnlineMs.ToString()).Append(", ");
	builder.Append("guildSimpleName").Append(":").Append(guildSimpleName.ToString()).Append(", ");
	builder.Append("totalPower").Append(":").Append(totalPower.ToString()).Append(", ");
	builder.Append("earnings").Append(":").Append(earnings.ToString()).Append(", ");
	builder.Append("playerSkinId").Append(":").Append(playerSkinId.ToString()).Append(", ");
	builder.Append("curTitle").Append(":").Append(curTitle == null ? "null" : curTitle.ToString()).Append(", ");
	builder.Append("isShow").Append(":").Append(isShow.ToString()).Append(", ");
	builder.Append("exp").Append(":").Append(exp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

