using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

public class NPCommon_ChatPlayerContent : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家名
/// </summary>
private string cName;
/// <summary>
/// 玩家id
/// </summary>
private long cid;
/// <summary>
/// 气泡框id
/// </summary>
private long bubbleId;
/// <summary>
/// 头像id
/// </summary>
private long iconId;
/// <summary>
/// 头像框id
/// </summary>
private long iconBgkId;
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
/// VIP等级
/// </summary>
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
	string _cName
	, long _cid
	, long _bubbleId
	, long _iconId
	, long _iconBgkId
	, long _playerSkinId
	, NPCommon.PlayerInfo_CurTitle _curTitle
	, bool _isShow
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 玩家名
/// </summary>
public string getCName() { return cName; }
/// <summary>
/// 玩家名
/// </summary>
public void setCName(string _cName) { cName = _cName; }
/// <summary>
/// 玩家id
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家id
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 气泡框id
/// </summary>
public long getBubbleId() { return bubbleId; }
/// <summary>
/// 气泡框id
/// </summary>
public void setBubbleId(long _bubbleId) { bubbleId = _bubbleId; }
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
/// VIP等级
/// </summary>
public int getVipLvl() { return vipLvl; }
/// <summary>
/// VIP等级
/// </summary>
public void setVipLvl(int _vipLvl) { vipLvl = _vipLvl; }


public int GetBufSize() {
	int _size = 45;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cName);
	_size += 4 + curTitle.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 47;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cName);
	_size += 4 + curTitle.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	bubbleId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconBgkId = _buf.getLong();
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
	vipLvl = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(cName);
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
	builder.Append("cName").Append(":").Append(cName.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("bubbleId").Append(":").Append(bubbleId.ToString()).Append(", ");
	builder.Append("iconId").Append(":").Append(iconId.ToString()).Append(", ");
	builder.Append("iconBgkId").Append(":").Append(iconBgkId.ToString()).Append(", ");
	builder.Append("playerSkinId").Append(":").Append(playerSkinId.ToString()).Append(", ");
	builder.Append("curTitle").Append(":").Append(curTitle == null ? "null" : curTitle.ToString()).Append(", ");
	builder.Append("isShow").Append(":").Append(isShow.ToString()).Append(", ");
	builder.Append("vipLvl").Append(":").Append(vipLvl.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

