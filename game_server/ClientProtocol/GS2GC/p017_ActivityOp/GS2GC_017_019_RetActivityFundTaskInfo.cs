using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

public class GS2GC_017_019_RetActivityFundTaskInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务列表
/// </summary>
private List<Common.ActivityFundObj.ActivityFund_TaskInfo> taskList;
/// <summary>
/// 任务下次刷新时间
/// </summary>
private long taskNextRefreshTime;


public GS2GC_017_019_RetActivityFundTaskInfo() {
	taskList = new List<Common.ActivityFundObj.ActivityFund_TaskInfo>();
	taskNextRefreshTime = (long)0;
}

public GS2GC_017_019_RetActivityFundTaskInfo(
	List<Common.ActivityFundObj.ActivityFund_TaskInfo> _taskList
	, long _taskNextRefreshTime
) {	taskList = _taskList;
	taskNextRefreshTime = _taskNextRefreshTime;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)19; }

/// <summary>
/// 任务列表
/// </summary>
public List<Common.ActivityFundObj.ActivityFund_TaskInfo> getTaskList() { return taskList; }
/// <summary>
/// 任务列表
/// </summary>
public void addTaskList(Common.ActivityFundObj.ActivityFund_TaskInfo _taskList) { taskList.Add(_taskList); }
/// <summary>
/// 任务下次刷新时间
/// </summary>
public long getTaskNextRefreshTime() { return taskNextRefreshTime; }
/// <summary>
/// 任务下次刷新时间
/// </summary>
public void setTaskNextRefreshTime(long _taskNextRefreshTime) { taskNextRefreshTime = _taskNextRefreshTime; }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (taskList.Count * 24);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (taskList.Count * 24);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _taskListCount = _buf.getShort();
	for(int _i = 0; _i < _taskListCount; _i++) { 
		Common.ActivityFundObj.ActivityFund_TaskInfo _taskList = new Common.ActivityFundObj.ActivityFund_TaskInfo();
		int __taskListCustLen = _buf.getInt();
	int __taskListCurPos = _buf.getCurPos();
	_taskList.ReadUnzipBuf(_buf, __taskListCurPos + __taskListCustLen);
	_buf.setPosition(__taskListCurPos + __taskListCustLen);

		taskList.Add(_taskList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	taskNextRefreshTime = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)taskList.Count);
	for(int _i = 0; _i < taskList.Count; _i++) { 
		_buf.putInt(taskList[_i].GetBufSize());
	taskList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(taskNextRefreshTime);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)19);
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
	builder.Append("taskList").Append(":").Append(taskList.ToString()).Append(", ");
	builder.Append("taskNextRefreshTime").Append(":").Append(taskNextRefreshTime.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

