package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 领取阶段目标子任务奖励
 **/
public class GC2GS_007_018_ReqTakeStageGoalSubTaskReward implements ALBasicProtocolPack._IALProtocolStructure {
private long taskId;


public GC2GS_007_018_ReqTakeStageGoalSubTaskReward() {
	taskId = (long)0;
}

public GC2GS_007_018_ReqTakeStageGoalSubTaskReward(
	 long _taskId
) {	taskId = _taskId;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)18; }

public long getTaskId() { return taskId; }
public void setTaskId(long _taskId) { taskId = _taskId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) taskId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(taskId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)18);
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

