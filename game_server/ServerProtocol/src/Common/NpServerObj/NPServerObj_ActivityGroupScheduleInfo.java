package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 活动组排期信息
 **/
public class NPServerObj_ActivityGroupScheduleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动组id */
private long activityGroupId;
/** 开启时间 */
private long startTimeMs;
/** 结算时间 */
private long endTimeMs;
/** 关闭时间 */
private long closeTimeMs;


public NPServerObj_ActivityGroupScheduleInfo() {
	activityGroupId = (long)0;
	startTimeMs = (long)0;
	endTimeMs = (long)0;
	closeTimeMs = (long)0;
}

public NPServerObj_ActivityGroupScheduleInfo(
	 long _activityGroupId
	, long _startTimeMs
	, long _endTimeMs
	, long _closeTimeMs
) {	activityGroupId = _activityGroupId;
	startTimeMs = _startTimeMs;
	endTimeMs = _endTimeMs;
	closeTimeMs = _closeTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 活动组id */
public long getActivityGroupId() { return activityGroupId; }
/** 活动组id */
public void setActivityGroupId(long _activityGroupId) { activityGroupId = _activityGroupId; }
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
	if(_buf.remaining() > 0) activityGroupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) closeTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityGroupId);
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

