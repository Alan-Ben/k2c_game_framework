package Common.StageGoalObj;

import java.nio.ByteBuffer;
/*********
 * 阶段目标任务数据
 **/
public class StageGoalTask_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 阶段任务ID */
private long taskId;
/** 计数 */
private long counter;
/** 是否已领取奖励 */
private boolean hadDraw;


public StageGoalTask_Info() {
	taskId = (long)0;
	counter = (long)0;
	hadDraw = false;
}

public StageGoalTask_Info(
	 long _taskId
	, long _counter
	, boolean _hadDraw
) {	taskId = _taskId;
	counter = _counter;
	hadDraw = _hadDraw;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 阶段任务ID */
public long getTaskId() { return taskId; }
/** 阶段任务ID */
public void setTaskId(long _taskId) { taskId = _taskId; }
/** 计数 */
public long getCounter() { return counter; }
/** 计数 */
public void setCounter(long _counter) { counter = _counter; }
/** 是否已领取奖励 */
public boolean getHadDraw() { return hadDraw; }
/** 是否已领取奖励 */
public void setHadDraw(boolean _hadDraw) { hadDraw = _hadDraw; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) taskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) counter = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDraw = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(taskId);
	_buf.putLong(counter);
	_buf.put(hadDraw?(byte)1:(byte)0);
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

