package Common.CachedObj;

import java.nio.ByteBuffer;
public class CachedObj_CachedPlayerInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家id */
private long cid;
/** 玩家名 */
private String playerName;
/** vip等级 */
private long vipLvl;
/** 玩家等级 */
private long playerLvl;
/** 当前佩戴头像 */
private Common.CachedObj.CachedObj_CachedIconInfo iconInfo;
/** 当前佩戴头像框 */
private Common.CachedObj.CachedObj_CachedIconBgkInfo iconBgkInfo;
/** 当前佩戴气泡框 */
private Common.CachedObj.CachedObj_CachedBubbleInfo bubbleInfo;
/** 联盟信息 */
private Common.CachedObj.CachedObj_CachedGuildInfo guildInfo;
/** 最近离线时间 */
private long lastOfflineTimeMs;
/** 最近登录时间 */
private long lastOnlineTimeMs;
/** 冻结结束时间 */
private long freezeTimeMs;
/** 玩家语言 */
private String language;
/** 当前佩戴Q版形象 */
private Common.CachedObj.CachedObj_CachedCuteActorInfo cuteAcotrInfo;
/** 总实力 */
private long totalPower;
/** 历史最高实力 */
private long maxPower;
/** 赚速 */
private long earnings;
/** 历史最大赚速 */
private long maxEarnings;
/** 经验 */
private long exp;
/** 称号相关 */
private Common.CachedObj.CachedObj_CachedTitleObj titleObjV2;
/** 玩家皮肤 */
private long playerSkin;
/** VIP经验 */
private long vipExp;


public CachedObj_CachedPlayerInfo() {
	cid = (long)0;
	playerName = "";
	vipLvl = (long)0;
	playerLvl = (long)0;
	iconInfo = new Common.CachedObj.CachedObj_CachedIconInfo();
	iconBgkInfo = new Common.CachedObj.CachedObj_CachedIconBgkInfo();
	bubbleInfo = new Common.CachedObj.CachedObj_CachedBubbleInfo();
	guildInfo = new Common.CachedObj.CachedObj_CachedGuildInfo();
	lastOfflineTimeMs = (long)0;
	lastOnlineTimeMs = (long)0;
	freezeTimeMs = (long)0;
	language = "";
	cuteAcotrInfo = new Common.CachedObj.CachedObj_CachedCuteActorInfo();
	totalPower = (long)0;
	maxPower = (long)0;
	earnings = (long)0;
	maxEarnings = (long)0;
	exp = (long)0;
	titleObjV2 = new Common.CachedObj.CachedObj_CachedTitleObj();
	playerSkin = (long)0;
	vipExp = (long)0;
}

public CachedObj_CachedPlayerInfo(
	 long _cid
	, String _playerName
	, long _vipLvl
	, long _playerLvl
	, Common.CachedObj.CachedObj_CachedIconInfo _iconInfo
	, Common.CachedObj.CachedObj_CachedIconBgkInfo _iconBgkInfo
	, Common.CachedObj.CachedObj_CachedBubbleInfo _bubbleInfo
	, Common.CachedObj.CachedObj_CachedGuildInfo _guildInfo
	, long _lastOfflineTimeMs
	, long _lastOnlineTimeMs
	, long _freezeTimeMs
	, String _language
	, Common.CachedObj.CachedObj_CachedCuteActorInfo _cuteAcotrInfo
	, long _totalPower
	, long _maxPower
	, long _earnings
	, long _maxEarnings
	, long _exp
	, Common.CachedObj.CachedObj_CachedTitleObj _titleObjV2
	, long _playerSkin
	, long _vipExp
) {	cid = _cid;
	playerName = _playerName;
	vipLvl = _vipLvl;
	playerLvl = _playerLvl;
	iconInfo = _iconInfo;
	iconBgkInfo = _iconBgkInfo;
	bubbleInfo = _bubbleInfo;
	guildInfo = _guildInfo;
	lastOfflineTimeMs = _lastOfflineTimeMs;
	lastOnlineTimeMs = _lastOnlineTimeMs;
	freezeTimeMs = _freezeTimeMs;
	language = _language;
	cuteAcotrInfo = _cuteAcotrInfo;
	totalPower = _totalPower;
	maxPower = _maxPower;
	earnings = _earnings;
	maxEarnings = _maxEarnings;
	exp = _exp;
	titleObjV2 = _titleObjV2;
	playerSkin = _playerSkin;
	vipExp = _vipExp;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家id */
public long getCid() { return cid; }
/** 玩家id */
public void setCid(long _cid) { cid = _cid; }
/** 玩家名 */
public String getPlayerName() { return playerName; }
/** 玩家名 */
public void setPlayerName(String _playerName) { playerName = _playerName; }
/** vip等级 */
public long getVipLvl() { return vipLvl; }
/** vip等级 */
public void setVipLvl(long _vipLvl) { vipLvl = _vipLvl; }
/** 玩家等级 */
public long getPlayerLvl() { return playerLvl; }
/** 玩家等级 */
public void setPlayerLvl(long _playerLvl) { playerLvl = _playerLvl; }
/** 当前佩戴头像 */
public Common.CachedObj.CachedObj_CachedIconInfo getIconInfo() { return iconInfo; }
/** 当前佩戴头像 */
public void setIconInfo(Common.CachedObj.CachedObj_CachedIconInfo _iconInfo) { iconInfo = _iconInfo; }
/** 当前佩戴头像框 */
public Common.CachedObj.CachedObj_CachedIconBgkInfo getIconBgkInfo() { return iconBgkInfo; }
/** 当前佩戴头像框 */
public void setIconBgkInfo(Common.CachedObj.CachedObj_CachedIconBgkInfo _iconBgkInfo) { iconBgkInfo = _iconBgkInfo; }
/** 当前佩戴气泡框 */
public Common.CachedObj.CachedObj_CachedBubbleInfo getBubbleInfo() { return bubbleInfo; }
/** 当前佩戴气泡框 */
public void setBubbleInfo(Common.CachedObj.CachedObj_CachedBubbleInfo _bubbleInfo) { bubbleInfo = _bubbleInfo; }
/** 联盟信息 */
public Common.CachedObj.CachedObj_CachedGuildInfo getGuildInfo() { return guildInfo; }
/** 联盟信息 */
public void setGuildInfo(Common.CachedObj.CachedObj_CachedGuildInfo _guildInfo) { guildInfo = _guildInfo; }
/** 最近离线时间 */
public long getLastOfflineTimeMs() { return lastOfflineTimeMs; }
/** 最近离线时间 */
public void setLastOfflineTimeMs(long _lastOfflineTimeMs) { lastOfflineTimeMs = _lastOfflineTimeMs; }
/** 最近登录时间 */
public long getLastOnlineTimeMs() { return lastOnlineTimeMs; }
/** 最近登录时间 */
public void setLastOnlineTimeMs(long _lastOnlineTimeMs) { lastOnlineTimeMs = _lastOnlineTimeMs; }
/** 冻结结束时间 */
public long getFreezeTimeMs() { return freezeTimeMs; }
/** 冻结结束时间 */
public void setFreezeTimeMs(long _freezeTimeMs) { freezeTimeMs = _freezeTimeMs; }
/** 玩家语言 */
public String getLanguage() { return language; }
/** 玩家语言 */
public void setLanguage(String _language) { language = _language; }
/** 当前佩戴Q版形象 */
public Common.CachedObj.CachedObj_CachedCuteActorInfo getCuteAcotrInfo() { return cuteAcotrInfo; }
/** 当前佩戴Q版形象 */
public void setCuteAcotrInfo(Common.CachedObj.CachedObj_CachedCuteActorInfo _cuteAcotrInfo) { cuteAcotrInfo = _cuteAcotrInfo; }
/** 总实力 */
public long getTotalPower() { return totalPower; }
/** 总实力 */
public void setTotalPower(long _totalPower) { totalPower = _totalPower; }
/** 历史最高实力 */
public long getMaxPower() { return maxPower; }
/** 历史最高实力 */
public void setMaxPower(long _maxPower) { maxPower = _maxPower; }
/** 赚速 */
public long getEarnings() { return earnings; }
/** 赚速 */
public void setEarnings(long _earnings) { earnings = _earnings; }
/** 历史最大赚速 */
public long getMaxEarnings() { return maxEarnings; }
/** 历史最大赚速 */
public void setMaxEarnings(long _maxEarnings) { maxEarnings = _maxEarnings; }
/** 经验 */
public long getExp() { return exp; }
/** 经验 */
public void setExp(long _exp) { exp = _exp; }
/** 称号相关 */
public Common.CachedObj.CachedObj_CachedTitleObj getTitleObjV2() { return titleObjV2; }
/** 称号相关 */
public void setTitleObjV2(Common.CachedObj.CachedObj_CachedTitleObj _titleObjV2) { titleObjV2 = _titleObjV2; }
/** 玩家皮肤 */
public long getPlayerSkin() { return playerSkin; }
/** 玩家皮肤 */
public void setPlayerSkin(long _playerSkin) { playerSkin = _playerSkin; }
/** VIP经验 */
public long getVipExp() { return vipExp; }
/** VIP经验 */
public void setVipExp(long _vipExp) { vipExp = _vipExp; }


public final int GetBufSize() {
	int _size = 168;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + guildInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(language);
	_size += 4 + titleObjV2.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 170;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + guildInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(language);
	_size += 4 + titleObjV2.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) vipLvl = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerLvl = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _iconInfoCustLen = _buf.getInt();
	int _iconInfoCurPos = _buf.position();
	iconInfo.ReadUnzipBuf(_buf, _iconInfoCurPos + _iconInfoCustLen);
	_buf.position(_iconInfoCurPos + _iconInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _iconBgkInfoCustLen = _buf.getInt();
	int _iconBgkInfoCurPos = _buf.position();
	iconBgkInfo.ReadUnzipBuf(_buf, _iconBgkInfoCurPos + _iconBgkInfoCustLen);
	_buf.position(_iconBgkInfoCurPos + _iconBgkInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _bubbleInfoCustLen = _buf.getInt();
	int _bubbleInfoCurPos = _buf.position();
	bubbleInfo.ReadUnzipBuf(_buf, _bubbleInfoCurPos + _bubbleInfoCustLen);
	_buf.position(_bubbleInfoCurPos + _bubbleInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _guildInfoCustLen = _buf.getInt();
	int _guildInfoCurPos = _buf.position();
	guildInfo.ReadUnzipBuf(_buf, _guildInfoCurPos + _guildInfoCustLen);
	_buf.position(_guildInfoCurPos + _guildInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastOfflineTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastOnlineTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) freezeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) language = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _cuteAcotrInfoCustLen = _buf.getInt();
	int _cuteAcotrInfoCurPos = _buf.position();
	cuteAcotrInfo.ReadUnzipBuf(_buf, _cuteAcotrInfoCurPos + _cuteAcotrInfoCustLen);
	_buf.position(_cuteAcotrInfoCurPos + _cuteAcotrInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) earnings = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxEarnings = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) exp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _titleObjV2CustLen = _buf.getInt();
	int _titleObjV2CurPos = _buf.position();
	titleObjV2.ReadUnzipBuf(_buf, _titleObjV2CurPos + _titleObjV2CustLen);
	_buf.position(_titleObjV2CurPos + _titleObjV2CustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerSkin = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) vipExp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playerName);
	_buf.putLong(vipLvl);
	_buf.putLong(playerLvl);
	_buf.putInt(iconInfo.GetBufSize());
	iconInfo.PutUnzipBuf(_buf);
	_buf.putInt(iconBgkInfo.GetBufSize());
	iconBgkInfo.PutUnzipBuf(_buf);
	_buf.putInt(bubbleInfo.GetBufSize());
	bubbleInfo.PutUnzipBuf(_buf);
	_buf.putInt(guildInfo.GetBufSize());
	guildInfo.PutUnzipBuf(_buf);
	_buf.putLong(lastOfflineTimeMs);
	_buf.putLong(lastOnlineTimeMs);
	_buf.putLong(freezeTimeMs);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, language);
	_buf.putInt(cuteAcotrInfo.GetBufSize());
	cuteAcotrInfo.PutUnzipBuf(_buf);
	_buf.putLong(totalPower);
	_buf.putLong(maxPower);
	_buf.putLong(earnings);
	_buf.putLong(maxEarnings);
	_buf.putLong(exp);
	_buf.putInt(titleObjV2.GetBufSize());
	titleObjV2.PutUnzipBuf(_buf);
	_buf.putLong(playerSkin);
	_buf.putLong(vipExp);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

