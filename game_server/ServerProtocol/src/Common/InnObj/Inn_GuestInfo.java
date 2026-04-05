package Common.InnObj;

import java.nio.ByteBuffer;
/*********
 * 旅店_客人信息
 **/
public class Inn_GuestInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 客人ID */
private long guestId;
/** 是否领取图鉴奖励 */
private boolean hadDrawHandbookReward;
/** 开始排队的ID */
private long startLineUpId;


public Inn_GuestInfo() {
	guestId = (long)0;
	hadDrawHandbookReward = false;
	startLineUpId = (long)0;
}

public Inn_GuestInfo(
	 long _guestId
	, boolean _hadDrawHandbookReward
	, long _startLineUpId
) {	guestId = _guestId;
	hadDrawHandbookReward = _hadDrawHandbookReward;
	startLineUpId = _startLineUpId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 客人ID */
public long getGuestId() { return guestId; }
/** 客人ID */
public void setGuestId(long _guestId) { guestId = _guestId; }
/** 是否领取图鉴奖励 */
public boolean getHadDrawHandbookReward() { return hadDrawHandbookReward; }
/** 是否领取图鉴奖励 */
public void setHadDrawHandbookReward(boolean _hadDrawHandbookReward) { hadDrawHandbookReward = _hadDrawHandbookReward; }
/** 开始排队的ID */
public long getStartLineUpId() { return startLineUpId; }
/** 开始排队的ID */
public void setStartLineUpId(long _startLineUpId) { startLineUpId = _startLineUpId; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guestId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawHandbookReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startLineUpId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guestId);
	_buf.put(hadDrawHandbookReward?(byte)1:(byte)0);
	_buf.putLong(startLineUpId);
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

