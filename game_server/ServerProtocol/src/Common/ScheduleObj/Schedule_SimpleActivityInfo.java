package Common.ScheduleObj;

import java.nio.ByteBuffer;
public class Schedule_SimpleActivityInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动ID */
private long activityId;
/** 发布ID/排期ID */
private long launchId;
/** 开始时间毫秒 */
private long startTimeMs;
/** 结束时间毫秒 */
private long endTimeMs;
/** 展示结束时间毫秒 */
private long closeTimeMs;
/** 状态：0预热 1进行中 2结算中 3已关闭 4排期中 */
private int state;


public Schedule_SimpleActivityInfo() {
	activityId = (long)0;
	launchId = (long)0;
	startTimeMs = (long)0;
	endTimeMs = (long)0;
	closeTimeMs = (long)0;
	state = 0;
}

public Schedule_SimpleActivityInfo(
	 long _activityId
	, long _launchId
	, long _startTimeMs
	, long _endTimeMs
	, long _closeTimeMs
	, int _state
) {	activityId = _activityId;
	launchId = _launchId;
	startTimeMs = _startTimeMs;
	endTimeMs = _endTimeMs;
	closeTimeMs = _closeTimeMs;
	state = _state;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 活动ID */
public long getActivityId() { return activityId; }
/** 活动ID */
public void setActivityId(long _activityId) { activityId = _activityId; }
/** 发布ID/排期ID */
public long getLaunchId() { return launchId; }
/** 发布ID/排期ID */
public void setLaunchId(long _launchId) { launchId = _launchId; }
/** 开始时间毫秒 */
public long getStartTimeMs() { return startTimeMs; }
/** 开始时间毫秒 */
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/** 结束时间毫秒 */
public long getEndTimeMs() { return endTimeMs; }
/** 结束时间毫秒 */
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }
/** 展示结束时间毫秒 */
public long getCloseTimeMs() { return closeTimeMs; }
/** 展示结束时间毫秒 */
public void setCloseTimeMs(long _closeTimeMs) { closeTimeMs = _closeTimeMs; }
/** 状态：0预热 1进行中 2结算中 3已关闭 4排期中 */
public int getState() { return state; }
/** 状态：0预热 1进行中 2结算中 3已关闭 4排期中 */
public void setState(int _state) { state = _state; }


public final int GetBufSize() {
	int _size = 44;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 46;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) launchId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) closeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) state = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityId);
	_buf.putLong(launchId);
	_buf.putLong(startTimeMs);
	_buf.putLong(endTimeMs);
	_buf.putLong(closeTimeMs);
	_buf.putInt(state);
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

