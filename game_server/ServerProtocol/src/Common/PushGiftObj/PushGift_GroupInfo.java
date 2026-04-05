package Common.PushGiftObj;

import java.nio.ByteBuffer;
/*********
 * 推送礼包-组信息
 **/
public class PushGift_GroupInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包组id */
private long groupId;
/** 上次触发时间毫秒 */
private long lastTriggerTimeMs;
/** 当前激活的礼包 */
private Common.PushGiftObj.PushGift_ActivePackInfo activePack;


public PushGift_GroupInfo() {
	groupId = (long)0;
	lastTriggerTimeMs = (long)0;
	activePack = new Common.PushGiftObj.PushGift_ActivePackInfo();
}

public PushGift_GroupInfo(
	 long _groupId
	, long _lastTriggerTimeMs
	, Common.PushGiftObj.PushGift_ActivePackInfo _activePack
) {	groupId = _groupId;
	lastTriggerTimeMs = _lastTriggerTimeMs;
	activePack = _activePack;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 礼包组id */
public long getGroupId() { return groupId; }
/** 礼包组id */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 上次触发时间毫秒 */
public long getLastTriggerTimeMs() { return lastTriggerTimeMs; }
/** 上次触发时间毫秒 */
public void setLastTriggerTimeMs(long _lastTriggerTimeMs) { lastTriggerTimeMs = _lastTriggerTimeMs; }
/** 当前激活的礼包 */
public Common.PushGiftObj.PushGift_ActivePackInfo getActivePack() { return activePack; }
/** 当前激活的礼包 */
public void setActivePack(Common.PushGiftObj.PushGift_ActivePackInfo _activePack) { activePack = _activePack; }


public final int GetBufSize() {
	int _size = 38;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 40;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastTriggerTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _activePackCustLen = _buf.getInt();
	int _activePackCurPos = _buf.position();
	activePack.ReadUnzipBuf(_buf, _activePackCurPos + _activePackCustLen);
	_buf.position(_activePackCurPos + _activePackCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(lastTriggerTimeMs);
	_buf.putInt(activePack.GetBufSize());
	activePack.PutUnzipBuf(_buf);
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

