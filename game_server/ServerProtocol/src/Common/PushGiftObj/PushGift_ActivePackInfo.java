package Common.PushGiftObj;

import java.nio.ByteBuffer;
/*********
 * 推送礼包-激活礼包信息
 **/
public class PushGift_ActivePackInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 推送礼包id */
private long pushGiftId;
/** 激活时间毫秒 */
private long activateTimeMs;
/** 是否已读 */
private boolean hasRead;
/** 是否自动激活 */
private boolean isAutoActive;


public PushGift_ActivePackInfo() {
	pushGiftId = (long)0;
	activateTimeMs = (long)0;
	hasRead = false;
	isAutoActive = false;
}

public PushGift_ActivePackInfo(
	 long _pushGiftId
	, long _activateTimeMs
	, boolean _hasRead
	, boolean _isAutoActive
) {	pushGiftId = _pushGiftId;
	activateTimeMs = _activateTimeMs;
	hasRead = _hasRead;
	isAutoActive = _isAutoActive;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 推送礼包id */
public long getPushGiftId() { return pushGiftId; }
/** 推送礼包id */
public void setPushGiftId(long _pushGiftId) { pushGiftId = _pushGiftId; }
/** 激活时间毫秒 */
public long getActivateTimeMs() { return activateTimeMs; }
/** 激活时间毫秒 */
public void setActivateTimeMs(long _activateTimeMs) { activateTimeMs = _activateTimeMs; }
/** 是否已读 */
public boolean getHasRead() { return hasRead; }
/** 是否已读 */
public void setHasRead(boolean _hasRead) { hasRead = _hasRead; }
/** 是否自动激活 */
public boolean getIsAutoActive() { return isAutoActive; }
/** 是否自动激活 */
public void setIsAutoActive(boolean _isAutoActive) { isAutoActive = _isAutoActive; }


public final int GetBufSize() {
	int _size = 18;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 20;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pushGiftId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activateTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hasRead = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAutoActive = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(pushGiftId);
	_buf.putLong(activateTimeMs);
	_buf.put(hasRead?(byte)1:(byte)0);
	_buf.put(isAutoActive?(byte)1:(byte)0);
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

