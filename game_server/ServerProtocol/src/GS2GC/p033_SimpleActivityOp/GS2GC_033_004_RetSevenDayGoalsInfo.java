package GS2GC.p033_SimpleActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_033_004_RetSevenDayGoalsInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务信息列表 */
private java.util.ArrayList<Common.SimpleActivityObj.SevenDayGoals_TaskInfo> taskList;
/** 已领取奖励列表 */
private java.util.ArrayList<Long> hadDrawRewardList;
/** 当前积分 */
private long score;
/** 已领取的阶段奖励列表 */
private java.util.ArrayList<Long> hadDrawStepRewardList;


public GS2GC_033_004_RetSevenDayGoalsInfo() {
	taskList = new java.util.ArrayList<Common.SimpleActivityObj.SevenDayGoals_TaskInfo>();
	hadDrawRewardList = new java.util.ArrayList<Long>();
	score = (long)0;
	hadDrawStepRewardList = new java.util.ArrayList<Long>();
}

public GS2GC_033_004_RetSevenDayGoalsInfo(
	 java.util.ArrayList<Common.SimpleActivityObj.SevenDayGoals_TaskInfo> _taskList
	, java.util.ArrayList<Long> _hadDrawRewardList
	, long _score
	, java.util.ArrayList<Long> _hadDrawStepRewardList
) {	taskList = _taskList;
	hadDrawRewardList = _hadDrawRewardList;
	score = _score;
	hadDrawStepRewardList = _hadDrawStepRewardList;
}

public final byte getMainOrder() { return (byte)33; }

public final byte getSubOrder() { return (byte)4; }

/** 任务信息列表 */
public java.util.ArrayList<Common.SimpleActivityObj.SevenDayGoals_TaskInfo> getTaskList() { return taskList; }
/** 任务信息列表 */
public void addTaskList(Common.SimpleActivityObj.SevenDayGoals_TaskInfo _taskList) { taskList.add(_taskList); }
/** 已领取奖励列表 */
public java.util.ArrayList<Long> getHadDrawRewardList() { return hadDrawRewardList; }
/** 已领取奖励列表 */
public void addHadDrawRewardList(long _hadDrawRewardList) { hadDrawRewardList.add(_hadDrawRewardList); }
/** 当前积分 */
public long getScore() { return score; }
/** 当前积分 */
public void setScore(long _score) { score = _score; }
/** 已领取的阶段奖励列表 */
public java.util.ArrayList<Long> getHadDrawStepRewardList() { return hadDrawStepRewardList; }
/** 已领取的阶段奖励列表 */
public void addHadDrawStepRewardList(long _hadDrawStepRewardList) { hadDrawStepRewardList.add(_hadDrawStepRewardList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (taskList.size() * 20);
	_size += 2 + (hadDrawRewardList.size() * 8);
	_size += 2 + (hadDrawStepRewardList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (taskList.size() * 20);
	_size += 2 + (hadDrawRewardList.size() * 8);
	_size += 2 + (hadDrawStepRewardList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _taskListCount = _buf.getShort();
	for(int _i = 0; _i < _taskListCount; _i++) { 
		Common.SimpleActivityObj.SevenDayGoals_TaskInfo _taskList = new Common.SimpleActivityObj.SevenDayGoals_TaskInfo();
		if(_buf.remaining() <= 0) return;
	int __taskListCustLen = _buf.getInt();
	int __taskListCurPos = _buf.position();
	_taskList.ReadUnzipBuf(_buf, __taskListCurPos + __taskListCustLen);
	_buf.position(__taskListCurPos + __taskListCustLen);

		taskList.add(_taskList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawRewardListCount; _i++) { 
		long _hadDrawRewardList = (long)0;
		if(_buf.remaining() > 0) _hadDrawRewardList = _buf.getLong();
		hadDrawRewardList.add(_hadDrawRewardList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawStepRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawStepRewardListCount; _i++) { 
		long _hadDrawStepRewardList = (long)0;
		if(_buf.remaining() > 0) _hadDrawStepRewardList = _buf.getLong();
		hadDrawStepRewardList.add(_hadDrawStepRewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)taskList.size());
	for(int _i = 0; _i < taskList.size(); _i++) { 
		_buf.putInt(taskList.get(_i).GetBufSize());
	taskList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)hadDrawRewardList.size());
	for(int _i = 0; _i < hadDrawRewardList.size(); _i++) { 
		_buf.putLong(hadDrawRewardList.get(_i));
	}
	_buf.putLong(score);
	_buf.putShort((short)hadDrawStepRewardList.size());
	for(int _i = 0; _i < hadDrawStepRewardList.size(); _i++) { 
		_buf.putLong(hadDrawStepRewardList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)4);
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

