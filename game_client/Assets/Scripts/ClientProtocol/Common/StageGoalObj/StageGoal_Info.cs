using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.StageGoalObj
{

/// <summary>
/// 阶段目标数据
/// </summary>
public class StageGoal_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 阶段ID
/// </summary>
private long step;
/// <summary>
/// 任务数据列表
/// </summary>
private List<Common.StageGoalObj.StageGoalTask_Info> taskList;
/// <summary>
/// 是否完成，表示所有阶段任务都已完成并领取奖励
/// </summary>
private bool isDone;


public StageGoal_Info() {
	step = (long)0;
	taskList = new List<Common.StageGoalObj.StageGoalTask_Info>();
	isDone = false;
}

public StageGoal_Info(
	long _step
	, List<Common.StageGoalObj.StageGoalTask_Info> _taskList
	, bool _isDone
) {	step = _step;
	taskList = _taskList;
	isDone = _isDone;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 阶段ID
/// </summary>
public long getStep() { return step; }
/// <summary>
/// 阶段ID
/// </summary>
public void setStep(long _step) { step = _step; }
/// <summary>
/// 任务数据列表
/// </summary>
public List<Common.StageGoalObj.StageGoalTask_Info> getTaskList() { return taskList; }
/// <summary>
/// 任务数据列表
/// </summary>
public void addTaskList(Common.StageGoalObj.StageGoalTask_Info _taskList) { taskList.Add(_taskList); }
/// <summary>
/// 是否完成，表示所有阶段任务都已完成并领取奖励
/// </summary>
public bool getIsDone() { return isDone; }
/// <summary>
/// 是否完成，表示所有阶段任务都已完成并领取奖励
/// </summary>
public void setIsDone(bool _isDone) { isDone = _isDone; }


public int GetBufSize() {
	int _size = 9;
	_size += 2 + (taskList.Count * 21);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;
	_size += 2 + (taskList.Count * 21);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	step = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _taskListCount = _buf.getShort();
	for(int _i = 0; _i < _taskListCount; _i++) { 
		Common.StageGoalObj.StageGoalTask_Info _taskList = new Common.StageGoalObj.StageGoalTask_Info();
		int __taskListCustLen = _buf.getInt();
	int __taskListCurPos = _buf.getCurPos();
	_taskList.ReadUnzipBuf(_buf, __taskListCurPos + __taskListCustLen);
	_buf.setPosition(__taskListCurPos + __taskListCustLen);

		taskList.Add(_taskList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isDone = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(step);
	_buf.putShort((short)taskList.Count);
	for(int _i = 0; _i < taskList.Count; _i++) { 
		_buf.putInt(taskList[_i].GetBufSize());
	taskList[_i].PutUnzipBuf(_buf);
	}
	_buf.put(isDone?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("step").Append(":").Append(step.ToString()).Append(", ");
	builder.Append("taskList").Append(":").Append(taskList.ToString()).Append(", ");
	builder.Append("isDone").Append(":").Append(isDone.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

