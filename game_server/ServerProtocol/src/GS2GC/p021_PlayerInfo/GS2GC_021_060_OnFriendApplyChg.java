package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 推送好友申请变更消息
 **/
public class GS2GC_021_060_OnFriendApplyChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 申请数据 */
private Common.FriendObj.Friend_ApplyInfo apply;


public GS2GC_021_060_OnFriendApplyChg() {
	apply = new Common.FriendObj.Friend_ApplyInfo();
}

public GS2GC_021_060_OnFriendApplyChg(
	 Common.FriendObj.Friend_ApplyInfo _apply
) {	apply = _apply;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)60; }

/** 申请数据 */
public Common.FriendObj.Friend_ApplyInfo getApply() { return apply; }
/** 申请数据 */
public void setApply(Common.FriendObj.Friend_ApplyInfo _apply) { apply = _apply; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _applyCustLen = _buf.getInt();
	int _applyCurPos = _buf.position();
	apply.ReadUnzipBuf(_buf, _applyCurPos + _applyCustLen);
	_buf.position(_applyCurPos + _applyCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(apply.GetBufSize());
	apply.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)60);
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

