package Common.LevyObj;

import java.nio.ByteBuffer;
/*********
 * 征收粮食离线收益信息
 **/
public class Levy_FoodOfflineInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 离线时长 */
private long offlineMs;
/** 征收数量 */
private long count;


public Levy_FoodOfflineInfo() {
	offlineMs = (long)0;
	count = (long)0;
}

public Levy_FoodOfflineInfo(
	 long _offlineMs
	, long _count
) {	offlineMs = _offlineMs;
	count = _count;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 离线时长 */
public long getOfflineMs() { return offlineMs; }
/** 离线时长 */
public void setOfflineMs(long _offlineMs) { offlineMs = _offlineMs; }
/** 征收数量 */
public long getCount() { return count; }
/** 征收数量 */
public void setCount(long _count) { count = _count; }


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
	if(_buf.remaining() > 0) offlineMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(offlineMs);
	_buf.putLong(count);
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

