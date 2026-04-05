package Common.CountdownEventObj;

import java.nio.ByteBuffer;
/*********
 * 倒计时事件_数据
 **/
public class CountdownEvent_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据ID */
private long dbId;
/** 事件ID */
private long eventId;
/** 触发时间(毫秒) */
private long triggerTimeMs;
/** 重置次数 */
private int resetCount;


public CountdownEvent_Info() {
	dbId = (long)0;
	eventId = (long)0;
	triggerTimeMs = (long)0;
	resetCount = 0;
}

public CountdownEvent_Info(
	 long _dbId
	, long _eventId
	, long _triggerTimeMs
	, int _resetCount
) {	dbId = _dbId;
	eventId = _eventId;
	triggerTimeMs = _triggerTimeMs;
	resetCount = _resetCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数据ID */
public long getDbId() { return dbId; }
/** 数据ID */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 事件ID */
public long getEventId() { return eventId; }
/** 事件ID */
public void setEventId(long _eventId) { eventId = _eventId; }
/** 触发时间(毫秒) */
public long getTriggerTimeMs() { return triggerTimeMs; }
/** 触发时间(毫秒) */
public void setTriggerTimeMs(long _triggerTimeMs) { triggerTimeMs = _triggerTimeMs; }
/** 重置次数 */
public int getResetCount() { return resetCount; }
/** 重置次数 */
public void setResetCount(int _resetCount) { resetCount = _resetCount; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) triggerTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resetCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(eventId);
	_buf.putLong(triggerTimeMs);
	_buf.putInt(resetCount);
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

