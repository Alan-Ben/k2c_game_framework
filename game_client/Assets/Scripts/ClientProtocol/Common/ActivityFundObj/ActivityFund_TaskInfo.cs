using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ActivityFundObj
{

/// <summary>
/// 活动基金-任务信息
/// </summary>
public class ActivityFund_TaskInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务ID
/// </summary>
private long taskId;
/// <summary>
/// 当前计数
/// </summary>
private long currentCount;
/// <summary>
/// 已完成次数
/// </summary>
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 任务ID
/// </summary>
public long getTaskId() { return taskId; }
/// <summary>
/// 任务ID
/// </summary>
public void setTaskId(long _taskId) { taskId = _taskId; }
/// <summary>
/// 当前计数
/// </summary>
public long getCurrentCount() { return currentCount; }
/// <summary>
/// 当前计数
/// </summary>
public void setCurrentCount(long _currentCount) { currentCount = _currentCount; }
/// <summary>
/// 已完成次数
/// </summary>
public int getFinishedTimes() { return finishedTimes; }
/// <summary>
/// 已完成次数
/// </summary>
public void setFinishedTimes(int _finishedTimes) { finishedTimes = _finishedTimes; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	taskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	currentCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	finishedTimes = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(taskId);
	_buf.putLong(currentCount);
	_buf.putInt(finishedTimes);
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
	builder.Append("currentCount").Append(":").Append(currentCount.ToString()).Append(", ");
	builder.Append("finishedTimes").Append(":").Append(finishedTimes.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

