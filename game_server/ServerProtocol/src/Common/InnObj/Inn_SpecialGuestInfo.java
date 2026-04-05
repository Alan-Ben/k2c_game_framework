package Common.InnObj;

import java.nio.ByteBuffer;
/*********
 * 旅店_特殊客人信息
 **/
public class Inn_SpecialGuestInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 特殊客人ID */
private long specialGuestId;
/** 是否接待过 */
private boolean hadBeenServe;
/** 是否领取图鉴奖励 */
private boolean hadDrawHandbookReward;


public Inn_SpecialGuestInfo() {
	specialGuestId = (long)0;
	hadBeenServe = false;
	hadDrawHandbookReward = false;
}

public Inn_SpecialGuestInfo(
	 long _specialGuestId
	, boolean _hadBeenServe
	, boolean _hadDrawHandbookReward
) {	specialGuestId = _specialGuestId;
	hadBeenServe = _hadBeenServe;
	hadDrawHandbookReward = _hadDrawHandbookReward;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 特殊客人ID */
public long getSpecialGuestId() { return specialGuestId; }
/** 特殊客人ID */
public void setSpecialGuestId(long _specialGuestId) { specialGuestId = _specialGuestId; }
/** 是否接待过 */
public boolean getHadBeenServe() { return hadBeenServe; }
/** 是否接待过 */
public void setHadBeenServe(boolean _hadBeenServe) { hadBeenServe = _hadBeenServe; }
/** 是否领取图鉴奖励 */
public boolean getHadDrawHandbookReward() { return hadDrawHandbookReward; }
/** 是否领取图鉴奖励 */
public void setHadDrawHandbookReward(boolean _hadDrawHandbookReward) { hadDrawHandbookReward = _hadDrawHandbookReward; }


public final int GetBufSize() {
	int _size = 10;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 12;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) specialGuestId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadBeenServe = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawHandbookReward = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(specialGuestId);
	_buf.put(hadBeenServe?(byte)1:(byte)0);
	_buf.put(hadDrawHandbookReward?(byte)1:(byte)0);
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

