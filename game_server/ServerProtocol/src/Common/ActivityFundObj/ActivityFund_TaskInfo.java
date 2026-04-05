package Common.ActivityFundObj;

import java.nio.ByteBuffer;
/*********
 * 活动基金-任务信息
 **/
public class ActivityFund_TaskInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务ID */
private long taskId;
/** 当前计数 */
private long currentCount;
/** 已完成次数 */
private int finishedTimes;


public ActivityFund_TaskInfo() {
	taskId = (long)0;
	currentCount = (long)0;
	finishedTimes = 0;
}

public ActivityFund_TaskInfo(
	 long _taskId
	, long _currentCount
	, int _finishedTimes
) {	taskId = _taskId;
	currentCount = _currentCount;
	finishedTimes = _finishedTimes;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 任务ID */
public long getTaskId() { return taskId; }
/** 任务ID */
public void setTaskId(long _taskId) { taskId = _taskId; }
/** 当前计数 */
public long getCurrentCount() { return currentCount; }
/** 当前计数 */
public void setCurrentCount(long _currentCount) { currentCount = _currentCount; }
/** 已完成次数 */
public int getFinishedTimes() { return finishedTimes; }
/** 已完成次数 */
public void setFinishedTimes(int _finishedTimes) { finishedTimes = _finishedTimes; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) taskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) currentCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) finishedTimes = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(taskId);
	_buf.putLong(currentCount);
	_buf.putInt(finishedTimes);
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

