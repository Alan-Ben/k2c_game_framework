package Common.CachedObj;

import java.nio.ByteBuffer;
public class CachedObj_CachedBubbleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 气泡框id */
private long bubbleId;
/** 超时时间戳，0一下表示永久 */
private int expiredTimeS;


public CachedObj_CachedBubbleInfo() {
	bubbleId = (long)0;
	expiredTimeS = 0;
}

public CachedObj_CachedBubbleInfo(
	 long _bubbleId
	, int _expiredTimeS
) {	bubbleId = _bubbleId;
	expiredTimeS = _expiredTimeS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 气泡框id */
public long getBubbleId() { return bubbleId; }
/** 气泡框id */
public void setBubbleId(long _bubbleId) { bubbleId = _bubbleId; }
/** 超时时间戳，0一下表示永久 */
public int getExpiredTimeS() { return expiredTimeS; }
/** 超时时间戳，0一下表示永久 */
public void setExpiredTimeS(int _expiredTimeS) { expiredTimeS = _expiredTimeS; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bubbleId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expiredTimeS = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(bubbleId);
	_buf.putInt(expiredTimeS);
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

