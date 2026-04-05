package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
/*********
 * 玩家登录过的服务及服务器上的角色展示信息
 **/
public class NP_SYS_PlayerJoinedUSInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 服务器信息 */
private NPCommon.NP_SYS_ServerItem serverItem;
/** 玩家cid */
private long cid;
/** 玩家名 */
private String playerName;
/** 头像展示信息 */
private NPCommon.PlayerInfo_IconShow iconShowInfo;
/** 最后登录时间 */
private long lastLoginTimeMs;
/** 是否被封禁 */
private boolean isFreeze;
/** 封禁结束时间 */
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
	, String _playerName
	, NPCommon.PlayerInfo_IconShow _iconShowInfo
	, long _lastLoginTimeMs
	, boolean _isFreeze
	, long _freezeTimeMs
) {	serverItem = _serverItem;
	cid = _cid;
	playerName = _playerName;
	iconShowInfo = _iconShowInfo;
	lastLoginTimeMs = _lastLoginTimeMs;
	isFreeze = _isFreeze;
	freezeTimeMs = _freezeTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 服务器信息 */
public NPCommon.NP_SYS_ServerItem getServerItem() { return serverItem; }
/** 服务器信息 */
public void setServerItem(NPCommon.NP_SYS_ServerItem _serverItem) { serverItem = _serverItem; }
/** 玩家cid */
public long getCid() { return cid; }
/** 玩家cid */
public void setCid(long _cid) { cid = _cid; }
/** 玩家名 */
public String getPlayerName() { return playerName; }
/** 玩家名 */
public void setPlayerName(String _playerName) { playerName = _playerName; }
/** 头像展示信息 */
public NPCommon.PlayerInfo_IconShow getIconShowInfo() { return iconShowInfo; }
/** 头像展示信息 */
public void setIconShowInfo(NPCommon.PlayerInfo_IconShow _iconShowInfo) { iconShowInfo = _iconShowInfo; }
/** 最后登录时间 */
public long getLastLoginTimeMs() { return lastLoginTimeMs; }
/** 最后登录时间 */
public void setLastLoginTimeMs(long _lastLoginTimeMs) { lastLoginTimeMs = _lastLoginTimeMs; }
/** 是否被封禁 */
public boolean getIsFreeze() { return isFreeze; }
/** 是否被封禁 */
public void setIsFreeze(boolean _isFreeze) { isFreeze = _isFreeze; }
/** 封禁结束时间 */
public long getFreezeTimeMs() { return freezeTimeMs; }
/** 封禁结束时间 */
public void setFreezeTimeMs(long _freezeTimeMs) { freezeTimeMs = _freezeTimeMs; }


public final int GetBufSize() {
	int _size = 25;
	_size += 4 + serverItem.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + iconShowInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;
	_size += 4 + serverItem.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += 4 + iconShowInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _serverItemCustLen = _buf.getInt();
	int _serverItemCurPos = _buf.position();
	serverItem.ReadUnzipBuf(_buf, _serverItemCurPos + _serverItemCustLen);
	_buf.position(_serverItemCurPos + _serverItemCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _iconShowInfoCustLen = _buf.getInt();
	int _iconShowInfoCurPos = _buf.position();
	iconShowInfo.ReadUnzipBuf(_buf, _iconShowInfoCurPos + _iconShowInfoCustLen);
	_buf.position(_iconShowInfoCurPos + _iconShowInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastLoginTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isFreeze = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) freezeTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverItem.GetBufSize());
	serverItem.PutUnzipBuf(_buf);
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playerName);
	_buf.putInt(iconShowInfo.GetBufSize());
	iconShowInfo.PutUnzipBuf(_buf);
	_buf.putLong(lastLoginTimeMs);
	_buf.put(isFreeze?(byte)1:(byte)0);
	_buf.putLong(freezeTimeMs);
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

