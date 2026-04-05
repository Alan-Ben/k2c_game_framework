package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 玩家固定时间刷新CD数据
 **/
public class NPCommon_PlayerFixedCD implements ALBasicProtocolPack._IALProtocolStructure {
/** CD唯一ID */
private int cdId;
/** 当前数量 */
private int count;
/** 上次恢复时间 */
private long lastCalTimeMS;
/** 最大数量 */
private int maxCount;
/** 每次恢复点数 */
private int addCountPerTime;


public NPCommon_PlayerFixedCD() {
	cdId = 0;
	count = 0;
	lastCalTimeMS = (long)0;
	maxCount = 0;
	addCountPerTime = 0;
}

public NPCommon_PlayerFixedCD(
	 int _cdId
	, int _count
	, long _lastCalTimeMS
	, int _maxCount
	, int _addCountPerTime
) {	cdId = _cdId;
	count = _count;
	lastCalTimeMS = _lastCalTimeMS;
	maxCount = _maxCount;
	addCountPerTime = _addCountPerTime;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** CD唯一ID */
public int getCdId() { return cdId; }
/** CD唯一ID */
public void setCdId(int _cdId) { cdId = _cdId; }
/** 当前数量 */
public int getCount() { return count; }
/** 当前数量 */
public void setCount(int _count) { count = _count; }
/** 上次恢复时间 */
public long getLastCalTimeMS() { return lastCalTimeMS; }
/** 上次恢复时间 */
public void setLastCalTimeMS(long _lastCalTimeMS) { lastCalTimeMS = _lastCalTimeMS; }
/** 最大数量 */
public int getMaxCount() { return maxCount; }
/** 最大数量 */
public void setMaxCount(int _maxCount) { maxCount = _maxCount; }
/** 每次恢复点数 */
public int getAddCountPerTime() { return addCountPerTime; }
/** 每次恢复点数 */
public void setAddCountPerTime(int _addCountPerTime) { addCountPerTime = _addCountPerTime; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cdId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastCalTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addCountPerTime = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(cdId);
	_buf.putInt(count);
	_buf.putLong(lastCalTimeMS);
	_buf.putInt(maxCount);
	_buf.putInt(addCountPerTime);
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

