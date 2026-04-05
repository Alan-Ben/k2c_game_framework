package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_017_019_RetActivityFundTaskInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务列表 */
private java.util.ArrayList<Common.ActivityFundObj.ActivityFund_TaskInfo> taskList;
/** 任务下次刷新时间 */
private long taskNextRefreshTime;


public GS2GC_017_019_RetActivityFundTaskInfo() {
	taskList = new java.util.ArrayList<Common.ActivityFundObj.ActivityFund_TaskInfo>();
	taskNextRefreshTime = (long)0;
}

public GS2GC_017_019_RetActivityFundTaskInfo(
	 java.util.ArrayList<Common.ActivityFundObj.ActivityFund_TaskInfo> _taskList
	, long _taskNextRefreshTime
) {	taskList = _taskList;
	taskNextRefreshTime = _taskNextRefreshTime;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)19; }

/** 任务列表 */
public java.util.ArrayList<Common.ActivityFundObj.ActivityFund_TaskInfo> getTaskList() { return taskList; }
/** 任务列表 */
public void addTaskList(Common.ActivityFundObj.ActivityFund_TaskInfo _taskList) { taskList.add(_taskList); }
/** 任务下次刷新时间 */
public long getTaskNextRefreshTime() { return taskNextRefreshTime; }
/** 任务下次刷新时间 */
public void setTaskNextRefreshTime(long _taskNextRefreshTime) { taskNextRefreshTime = _taskNextRefreshTime; }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (taskList.size() * 24);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (taskList.size() * 24);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _taskListCount = _buf.getShort();
	for(int _i = 0; _i < _taskListCount; _i++) { 
		Common.ActivityFundObj.ActivityFund_TaskInfo _taskList = new Common.ActivityFundObj.ActivityFund_TaskInfo();
		if(_buf.remaining() <= 0) return;
	int __taskListCustLen = _buf.getInt();
	int __taskListCurPos = _buf.position();
	_taskList.ReadUnzipBuf(_buf, __taskListCurPos + __taskListCustLen);
	_buf.position(__taskListCurPos + __taskListCustLen);

		taskList.add(_taskList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) taskNextRefreshTime = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)taskList.size());
	for(int _i = 0; _i < taskList.size(); _i++) { 
		_buf.putInt(taskList.get(_i).GetBufSize());
	taskList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(taskNextRefreshTime);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)19);
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

