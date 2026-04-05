package Common.NpChatObj;

import java.nio.ByteBuffer;
public class NPCommon_ChatPlayerContent implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家名 */
private String cName;
/** 玩家id */
private long cid;
/** 气泡框id */
private long bubbleId;
/** 头像id */
private long iconId;
/** 头像框id */
private long iconBgkId;
/** 皮肤ID */
private long playerSkinId;
/** 玩家当前穿戴称号数据 */
private NPCommon.PlayerInfo_CurTitle curTitle;
/** 是否展示称号 */
private boolean isShow;
/** VIP等级 */
private int vipLvl;


public NPCommon_ChatPlayerContent() {
	cName = "";
	cid = (long)0;
	bubbleId = (long)0;
	iconId = (long)0;
	iconBgkId = (long)0;
	playerSkinId = (long)0;
	curTitle = new NPCommon.PlayerInfo_CurTitle();
	isShow = false;
	vipLvl = 0;
}

public NPCommon_ChatPlayerContent(
	 String _cName
	, long _cid
	, long _bubbleId
	, long _iconId
	, long _iconBgkId
	, long _playerSkinId
	, NPCommon.PlayerInfo_CurTitle _curTitle
	, boolean _isShow
	, int _vipLvl
) {	cName = _cName;
	cid = _cid;
	bubbleId = _bubbleId;
	iconId = _iconId;
	iconBgkId = _iconBgkId;
	playerSkinId = _playerSkinId;
	curTitle = _curTitle;
	isShow = _isShow;
	vipLvl = _vipLvl;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家名 */
public String getCName() { return cName; }
/** 玩家名 */
public void setCName(String _cName) { cName = _cName; }
/** 玩家id */
public long getCid() { return cid; }
/** 玩家id */
public void setCid(long _cid) { cid = _cid; }
/** 气泡框id */
public long getBubbleId() { return bubbleId; }
/** 气泡框id */
public void setBubbleId(long _bubbleId) { bubbleId = _bubbleId; }
/** 头像id */
public long getIconId() { return iconId; }
/** 头像id */
public void setIconId(long _iconId) { iconId = _iconId; }
/** 头像框id */
public long getIconBgkId() { return iconBgkId; }
/** 头像框id */
public void setIconBgkId(long _iconBgkId) { iconBgkId = _iconBgkId; }
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
/** VIP等级 */
public int getVipLvl() { return vipLvl; }
/** VIP等级 */
public void setVipLvl(int _vipLvl) { vipLvl = _vipLvl; }


public final int GetBufSize() {
	int _size = 45;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cName);
	_size += 4 + curTitle.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 47;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cName);
	_size += 4 + curTitle.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bubbleId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconBgkId = _buf.getLong();
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
	if(_buf.remaining() > 0) vipLvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, cName);
	_buf.putLong(cid);
	_buf.putLong(bubbleId);
	_buf.putLong(iconId);
	_buf.putLong(iconBgkId);
	_buf.putLong(playerSkinId);
	_buf.putInt(curTitle.GetBufSize());
	curTitle.PutUnzipBuf(_buf);
	_buf.put(isShow?(byte)1:(byte)0);
	_buf.putInt(vipLvl);
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

