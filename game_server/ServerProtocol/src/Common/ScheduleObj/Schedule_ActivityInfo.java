package Common.ScheduleObj;

import java.nio.ByteBuffer;
/*********
 * 排期活动数据
 **/
public class Schedule_ActivityInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动ID */
private long activityId;
/** 开启时间 */
private long startTimeMs;
/** 结算时间 */
private long endTimeMs;
/** 关闭时间 */
private long closeTimeMs;


public Schedule_ActivityInfo() {
	activityId = (long)0;
	startTimeMs = (long)0;
	endTimeMs = (long)0;
	closeTimeMs = (long)0;
}

public Schedule_ActivityInfo(
	 long _activityId
	, long _startTimeMs
	, long _endTimeMs
	, long _closeTimeMs
) {	activityId = _activityId;
	startTimeMs = _startTimeMs;
	endTimeMs = _endTimeMs;
	closeTimeMs = _closeTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 活动ID */
public long getActivityId() { return activityId; }
/** 活动ID */
public void setActivityId(long _activityId) { activityId = _activityId; }
/** 开启时间 */
public long getStartTimeMs() { return startTimeMs; }
/** 开启时间 */
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/** 结算时间 */
public long getEndTimeMs() { return endTimeMs; }
/** 结算时间 */
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }
/** 关闭时间 */
public long getCloseTimeMs() { return closeTimeMs; }
/** 关闭时间 */
public void setCloseTimeMs(long _closeTimeMs) { closeTimeMs = _closeTimeMs; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) closeTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityId);
	_buf.putLong(startTimeMs);
	_buf.putLong(endTimeMs);
	_buf.putLong(closeTimeMs);
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

