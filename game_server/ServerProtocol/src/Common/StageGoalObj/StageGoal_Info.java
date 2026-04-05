package Common.StageGoalObj;

import java.nio.ByteBuffer;
/*********
 * 阶段目标数据
 **/
public class StageGoal_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 阶段ID */
private long step;
/** 任务数据列表 */
private java.util.ArrayList<Common.StageGoalObj.StageGoalTask_Info> taskList;
/** 是否完成，表示所有阶段任务都已完成并领取奖励 */
private boolean isDone;


public StageGoal_Info() {
	step = (long)0;
	taskList = new java.util.ArrayList<Common.StageGoalObj.StageGoalTask_Info>();
	isDone = false;
}

public StageGoal_Info(
	 long _step
	, java.util.ArrayList<Common.StageGoalObj.StageGoalTask_Info> _taskList
	, boolean _isDone
) {	step = _step;
	taskList = _taskList;
	isDone = _isDone;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 阶段ID */
public long getStep() { return step; }
/** 阶段ID */
public void setStep(long _step) { step = _step; }
/** 任务数据列表 */
public java.util.ArrayList<Common.StageGoalObj.StageGoalTask_Info> getTaskList() { return taskList; }
/** 任务数据列表 */
public void addTaskList(Common.StageGoalObj.StageGoalTask_Info _taskList) { taskList.add(_taskList); }
/** 是否完成，表示所有阶段任务都已完成并领取奖励 */
public boolean getIsDone() { return isDone; }
/** 是否完成，表示所有阶段任务都已完成并领取奖励 */
public void setIsDone(boolean _isDone) { isDone = _isDone; }


public final int GetBufSize() {
	int _size = 9;
	_size += 2 + (taskList.size() * 21);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;
	_size += 2 + (taskList.size() * 21);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) step = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _taskListCount = _buf.getShort();
	for(int _i = 0; _i < _taskListCount; _i++) { 
		Common.StageGoalObj.StageGoalTask_Info _taskList = new Common.StageGoalObj.StageGoalTask_Info();
		if(_buf.remaining() <= 0) return;
	int __taskListCustLen = _buf.getInt();
	int __taskListCurPos = _buf.position();
	_taskList.ReadUnzipBuf(_buf, __taskListCurPos + __taskListCustLen);
	_buf.position(__taskListCurPos + __taskListCustLen);

		taskList.add(_taskList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isDone = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(step);
	_buf.putShort((short)taskList.size());
	for(int _i = 0; _i < taskList.size(); _i++) { 
		_buf.putInt(taskList.get(_i).GetBufSize());
	taskList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.put(isDone?(byte)1:(byte)0);
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

