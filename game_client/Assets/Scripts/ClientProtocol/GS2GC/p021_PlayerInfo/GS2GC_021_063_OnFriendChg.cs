using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 推送好友变更消息
/// </summary>
public class GS2GC_021_063_OnFriendChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 好友数据
/// </summary>
private Common.FriendObj.Friend_Info friend;
/// <summary>
/// 是否同意方玩家
/// </summary>
private bool isAgree;


public GS2GC_021_063_OnFriendChg() {
	friend = new Common.FriendObj.Friend_Info();
	isAgree = false;
}

public GS2GC_021_063_OnFriendChg(
	Common.FriendObj.Friend_Info _friend
	, bool _isAgree
) {	friend = _friend;
	isAgree = _isAgree;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)63; }

/// <summary>
/// 好友数据
/// </summary>
public Common.FriendObj.Friend_Info getFriend() { return friend; }
/// <summary>
/// 好友数据
/// </summary>
public void setFriend(Common.FriendObj.Friend_Info _friend) { friend = _friend; }
/// <summary>
/// 是否同意方玩家
/// </summary>
public bool getIsAgree() { return isAgree; }
/// <summary>
/// 是否同意方玩家
/// </summary>
public void setIsAgree(bool _isAgree) { isAgree = _isAgree; }


public int GetBufSize() {
	int _size = 13;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _friendCustLen = _buf.getInt();
	int _friendCurPos = _buf.getCurPos();
	friend.ReadUnzipBuf(_buf, _friendCurPos + _friendCustLen);
	_buf.setPosition(_friendCurPos + _friendCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAgree = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(friend.GetBufSize());
	friend.PutUnzipBuf(_buf);
	_buf.put(isAgree?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)63);
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
	builder.Append("friend").Append(":").Append(friend == null ? "null" : friend.ToString()).Append(", ");
	builder.Append("isAgree").Append(":").Append(isAgree.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

