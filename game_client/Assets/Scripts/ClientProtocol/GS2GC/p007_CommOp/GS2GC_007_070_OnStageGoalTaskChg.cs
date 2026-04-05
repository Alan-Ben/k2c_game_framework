using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_070_OnStageGoalTaskChg : ALBasicProtocolPack._IALProtocolStructure {
private long step;
/// <summary>
/// 阶段任务数据
/// </summary>
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

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)70; }

public long getStep() { return step; }
public void setStep(long _step) { step = _step; }
/// <summary>
/// 阶段任务数据
/// </summary>
public Common.StageGoalObj.StageGoalTask_Info getTask() { return task; }
/// <summary>
/// 阶段任务数据
/// </summary>
public void setTask(Common.StageGoalObj.StageGoalTask_Info _task) { task = _task; }


public int GetBufSize() {
	int _size = 29;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 31;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	step = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _taskCustLen = _buf.getInt();
	int _taskCurPos = _buf.getCurPos();
	task.ReadUnzipBuf(_buf, _taskCurPos + _taskCustLen);
	_buf.setPosition(_taskCurPos + _taskCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(step);
	_buf.putInt(task.GetBufSize());
	task.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)70);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)70);
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
	builder.Append("task").Append(":").Append(task == null ? "null" : task.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

