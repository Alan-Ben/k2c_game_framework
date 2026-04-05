package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_070_OnStageGoalTaskChg implements ALBasicProtocolPack._IALProtocolStructure {
private long step;
/** 阶段任务数据 */
private Common.StageGoalObj.StageGoalTask_Info task;


public GS2GC_007_070_OnStageGoalTaskChg() {
	step = (long)0;
	task = new Common.StageGoalObj.StageGoalTask_Info();
}

public GS2GC_007_070_OnStageGoalTaskChg(
	 long _step
	, Common.StageGoalObj.StageGoalTask_Info _task
) {	step = _step;
	task = _task;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)70; }

public long getStep() { return step; }
public void setStep(long _step) { step = _step; }
/** 阶段任务数据 */
public Common.StageGoalObj.StageGoalTask_Info getTask() { return task; }
/** 阶段任务数据 */
public void setTask(Common.StageGoalObj.StageGoalTask_Info _task) { task = _task; }


public final int GetBufSize() {
	int _size = 29;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 31;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) step = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _taskCustLen = _buf.getInt();
	int _taskCurPos = _buf.position();
	task.ReadUnzipBuf(_buf, _taskCurPos + _taskCustLen);
	_buf.position(_taskCurPos + _taskCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(step);
	_buf.putInt(task.GetBufSize());
	task.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)70);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)70);
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

