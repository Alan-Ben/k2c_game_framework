package GC2GS.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 领取旅店客人图鉴奖励
 **/
public class GC2GS_034_010_ReqDrawGuestHandbookReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 特殊客人ID */
private long guestId;


public GC2GS_034_010_ReqDrawGuestHandbookReward() {
	guestId = (long)0;
}

public GC2GS_034_010_ReqDrawGuestHandbookReward(
	 long _guestId
) {	guestId = _guestId;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)10; }

/** 特殊客人ID */
public long getGuestId() { return guestId; }
/** 特殊客人ID */
public void setGuestId(long _guestId) { guestId = _guestId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guestId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guestId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)10);
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

