using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.StageGoalObj
{

/// <summary>
/// 阶段目标任务数据
/// </summary>
public class StageGoalTask_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 阶段任务ID
/// </summary>
private long taskId;
/// <summary>
/// 计数
/// </summary>
private long counter;
/// <summary>
/// 是否已领取奖励
/// </summary>
private bool hadDraw;


public StageGoalTask_Info() {
	taskId = (long)0;
	counter = (long)0;
	hadDraw = false;
}

public StageGoalTask_Info(
	long _taskId
	, long _counter
	, bool _hadDraw
) {	taskId = _taskId;
	counter = _counter;
	hadDraw = _hadDraw;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 阶段任务ID
/// </summary>
public long getTaskId() { return taskId; }
/// <summary>
/// 阶段任务ID
/// </summary>
public void setTaskId(long _taskId) { taskId = _taskId; }
/// <summary>
/// 计数
/// </summary>
public long getCounter() { return counter; }
/// <summary>
/// 计数
/// </summary>
public void setCounter(long _counter) { counter = _counter; }
/// <summary>
/// 是否已领取奖励
/// </summary>
public bool getHadDraw() { return hadDraw; }
/// <summary>
/// 是否已领取奖励
/// </summary>
public void setHadDraw(bool _hadDraw) { hadDraw = _hadDraw; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	taskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	counter = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDraw = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(taskId);
	_buf.putLong(counter);
	_buf.put(hadDraw?(byte)1:(byte)0);
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
	builder.Append("taskId").Append(":").Append(taskId.ToString()).Append(", ");
	builder.Append("counter").Append(":").Append(counter.ToString()).Append(", ");
	builder.Append("hadDraw").Append(":").Append(hadDraw.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

