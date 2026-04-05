package Common.SimpleActivityObj;

import java.nio.ByteBuffer;
/*********
 * 七日目标任务信息
 **/
public class SevenDayGoals_TaskInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务ID */
private long taskId;
/** 额外计数 */
private long extraCount;


public SevenDayGoals_TaskInfo() {
	taskId = (long)0;
	extraCount = (long)0;
}

public SevenDayGoals_TaskInfo(
	 long _taskId
	, long _extraCount
) {	taskId = _taskId;
	extraCount = _extraCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 任务ID */
public long getTaskId() { return taskId; }
/** 任务ID */
public void setTaskId(long _taskId) { taskId = _taskId; }
/** 额外计数 */
public long getExtraCount() { return extraCount; }
/** 额外计数 */
public void setExtraCount(long _extraCount) { extraCount = _extraCount; }


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
	if(_buf.remaining() > 0) taskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) extraCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(taskId);
	_buf.putLong(extraCount);
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

