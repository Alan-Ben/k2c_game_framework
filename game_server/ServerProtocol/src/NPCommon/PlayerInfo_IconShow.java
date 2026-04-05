package NPCommon;

import java.nio.ByteBuffer;
public class PlayerInfo_IconShow implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家id */
private long cid;
/** 玩家名 */
private String playerName;
/** 头像id */
private long iconId;
/** 头像框id */
private long iconBgkId;
/** 气泡框id */
private long bubbleId;
/** vip等级 */
private long vipLvl;
/** 玩家等级 */
private long playerLvl;
/** 联盟id */
private long guildId;
/** 联盟名称 */
private String guildName;
/** true-在线 false-离线 */
private boolean isOnline;
/** 最后一次离线时间戳（毫秒） */
private long lastOfflineMs;
/** 最后一次在线时间戳（毫秒） */
private long lastOnlineMs;
/** 联盟简称 */
private String guildSimpleName;
/** 总实力 */
private long totalPower;
/** 赚速 */
private long earnings;
/** 皮肤ID */
private long playerSkinId;
/** 玩家当前穿戴称号数据 */
private NPCommon.PlayerInfo_CurTitle curTitle;
/** 是否展示称号 */
private boolean isShow;
/** 玩家经验 */
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
	, String _playerName
	, long _iconId
	, long _iconBgkId
	, long _bubbleId
	, long _vipLvl
	, long _playerLvl
	, long _guildId
	, String _guildName
	, boolean _isOnline
	, long _lastOfflineMs
	, long _lastOnlineMs
	, String _guildSimpleName
	, long _totalPower
	, long _earnings
	, long _playerSkinId
	, NPCommon.PlayerInfo_CurTitle _curTitle
	, boolean _isShow
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
/** 头像id */
public long getIconId() { return iconId; }
/** 头像id */
public void setIconId(long _iconId) { iconId = _iconId; }
/** 头像框id */
public long getIconBgkId() { return iconBgkId; }
/** 头像框id */
public void setIconBgkId(long _iconBgkId) { iconBgkId = _iconBgkId; }
/** 气泡框id */
public long getBubbleId() { return bubbleId; }
/** 气泡框id */
public void setBubbleId(long _bubbleId) { bubbleId = _bubbleId; }
/** vip等级 */
public long getVipLvl() { return vipLvl; }
/** vip等级 */
public void setVipLvl(long _vipLvl) { vipLvl = _vipLvl; }
/** 玩家等级 */
public long getPlayerLvl() { return playerLvl; }
/** 玩家等级 */
public void setPlayerLvl(long _playerLvl) { playerLvl = _playerLvl; }
/** 联盟id */
public long getGuildId() { return guildId; }
/** 联盟id */
public void setGuildId(long _guildId) { guildId = _guildId; }
/** 联盟名称 */
public String getGuildName() { return guildName; }
/** 联盟名称 */
public void setGuildName(String _guildName) { guildName = _guildName; }
/** true-在线 false-离线 */
public boolean getIsOnline() { return isOnline; }
/** true-在线 false-离线 */
public void setIsOnline(boolean _isOnline) { isOnline = _isOnline; }
/** 最后一次离线时间戳（毫秒） */
public long getLastOfflineMs() { return lastOfflineMs; }
/** 最后一次离线时间戳（毫秒） */
public void setLastOfflineMs(long _lastOfflineMs) { lastOfflineMs = _lastOfflineMs; }
/** 最后一次在线时间戳（毫秒） */
public long getLastOnlineMs() { return lastOnlineMs; }
/** 最后一次在线时间戳（毫秒） */
public void setLastOnlineMs(long _lastOnlineMs) { lastOnlineMs = _lastOnlineMs; }
/** 联盟简称 */
public String getGuildSimpleName() { return guildSimpleName; }
/** 联盟简称 */
public void setGuildSimpleName(String _guildSimpleName) { guildSimpleName = _guildSimpleName; }
/** 总实力 */
public long getTotalPower() { return totalPower; }
/** 总实力 */
public void setTotalPower(long _totalPower) { totalPower = _totalPower; }
/** 赚速 */
public long getEarnings() { return earnings; }
/** 赚速 */
public void setEarnings(long _earnings) { earnings = _earnings; }
/** 皮肤ID */
public long getPlayerSkinId() { return playerSkinId; }
/** 皮肤ID */
public void setPlayerSkinId(long _playerSkinId) { playerSkinId = _playerSkinId; }
/** 玩家当前穿戴称号数据 */
public NPCommon.PlayerInfo_CurTitle getCurTitle() { return curTitle; }
/** 玩家当前穿戴称号数据 */
public void setCurTitle(NPCommon.PlayerInfo_CurTitle _curTitle) { curTitle = _curTitle; }
/** 是否展示称号 */
public boolean getIsShow() { return isShow; }
/** 是否展示称号 */
public void setIsShow(boolean _isShow) { isShow = _isShow; }
/** 玩家经验 */
public long getExp() { return exp; }
/** 玩家经验 */
public void setExp(long _exp) { exp = _exp; }


public final int GetBufSize() {
	int _size = 106;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildSimpleName);
	_size += 4 + curTitle.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 108;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildSimpleName);
	_size += 4 + curTitle.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconBgkId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bubbleId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) vipLvl = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerLvl = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isOnline = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastOfflineMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastOnlineMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildSimpleName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalPower = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) earnings = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerSkinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _curTitleCustLen = _buf.getInt();
	int _curTitleCurPos = _buf.position();
	curTitle.ReadUnzipBuf(_buf, _curTitleCurPos + _curTitleCustLen);
	_buf.position(_curTitleCurPos + _curTitleCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isShow = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) exp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playerName);
	_buf.putLong(iconId);
	_buf.putLong(iconBgkId);
	_buf.putLong(bubbleId);
	_buf.putLong(vipLvl);
	_buf.putLong(playerLvl);
	_buf.putLong(guildId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildName);
	_buf.put(isOnline?(byte)1:(byte)0);
	_buf.putLong(lastOfflineMs);
	_buf.putLong(lastOnlineMs);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildSimpleName);
	_buf.putLong(totalPower);
	_buf.putLong(earnings);
	_buf.putLong(playerSkinId);
	_buf.putInt(curTitle.GetBufSize());
	curTitle.PutUnzipBuf(_buf);
	_buf.put(isShow?(byte)1:(byte)0);
	_buf.putLong(exp);
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

