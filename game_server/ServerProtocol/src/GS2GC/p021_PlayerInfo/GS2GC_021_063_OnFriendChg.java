package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 推送好友变更消息
 **/
public class GS2GC_021_063_OnFriendChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 好友数据 */
private Common.FriendObj.Friend_Info friend;
/** 是否同意方玩家 */
private boolean isAgree;


public GS2GC_021_063_OnFriendChg() {
	friend = new Common.FriendObj.Friend_Info();
	isAgree = false;
}

public GS2GC_021_063_OnFriendChg(
	 Common.FriendObj.Friend_Info _friend
	, boolean _isAgree
) {	friend = _friend;
	isAgree = _isAgree;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)63; }

/** 好友数据 */
public Common.FriendObj.Friend_Info getFriend() { return friend; }
/** 好友数据 */
public void setFriend(Common.FriendObj.Friend_Info _friend) { friend = _friend; }
/** 是否同意方玩家 */
public boolean getIsAgree() { return isAgree; }
/** 是否同意方玩家 */
public void setIsAgree(boolean _isAgree) { isAgree = _isAgree; }


public final int GetBufSize() {
	int _size = 13;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _friendCustLen = _buf.getInt();
	int _friendCurPos = _buf.position();
	friend.ReadUnzipBuf(_buf, _friendCurPos + _friendCustLen);
	_buf.position(_friendCurPos + _friendCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAgree = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(friend.GetBufSize());
	friend.PutUnzipBuf(_buf);
	_buf.put(isAgree?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)63);
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

