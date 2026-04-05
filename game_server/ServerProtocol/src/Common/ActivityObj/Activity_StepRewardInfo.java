package Common.ActivityObj;

import java.nio.ByteBuffer;
/*********
 * 活动阶段奖励信息
 **/
public class Activity_StepRewardInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long activityInstanceId;
/** 阶段奖励ID */
private long stepRewardId;
/** 分数 */
private long score;
/** 已领取阶段列表 */
private java.util.ArrayList<Integer> hadDrawStepList;
/** 事件任务数据列表 */
private java.util.ArrayList<Common.ActivityObj.Activity_StepRewardEventTaskInfo> eventTaskList;


public Activity_StepRewardInfo() {
	activityInstanceId = (long)0;
	stepRewardId = (long)0;
	score = (long)0;
	hadDrawStepList = new java.util.ArrayList<Integer>();
	eventTaskList = new java.util.ArrayList<Common.ActivityObj.Activity_StepRewardEventTaskInfo>();
}

public Activity_StepRewardInfo(
	 long _activityInstanceId
	, long _stepRewardId
	, long _score
	, java.util.ArrayList<Integer> _hadDrawStepList
	, java.util.ArrayList<Common.ActivityObj.Activity_StepRewardEventTaskInfo> _eventTaskList
) {	activityInstanceId = _activityInstanceId;
	stepRewardId = _stepRewardId;
	score = _score;
	hadDrawStepList = _hadDrawStepList;
	eventTaskList = _eventTaskList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 活动实例ID */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/** 阶段奖励ID */
public long getStepRewardId() { return stepRewardId; }
/** 阶段奖励ID */
public void setStepRewardId(long _stepRewardId) { stepRewardId = _stepRewardId; }
/** 分数 */
public long getScore() { return score; }
/** 分数 */
public void setScore(long _score) { score = _score; }
/** 已领取阶段列表 */
public java.util.ArrayList<Integer> getHadDrawStepList() { return hadDrawStepList; }
/** 已领取阶段列表 */
public void addHadDrawStepList(int _hadDrawStepList) { hadDrawStepList.add(_hadDrawStepList); }
/** 事件任务数据列表 */
public java.util.ArrayList<Common.ActivityObj.Activity_StepRewardEventTaskInfo> getEventTaskList() { return eventTaskList; }
/** 事件任务数据列表 */
public void addEventTaskList(Common.ActivityObj.Activity_StepRewardEventTaskInfo _eventTaskList) { eventTaskList.add(_eventTaskList); }


public final int GetBufSize() {
	int _size = 24;
	_size += 2 + (hadDrawStepList.size() * 4);
	_size += 2 + (eventTaskList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;
	_size += 2 + (hadDrawStepList.size() * 4);
	_size += 2 + (eventTaskList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stepRewardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawStepListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawStepListCount; _i++) { 
		int _hadDrawStepList = 0;
		if(_buf.remaining() > 0) _hadDrawStepList = _buf.getInt();
		hadDrawStepList.add(_hadDrawStepList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _eventTaskListCount = _buf.getShort();
	for(int _i = 0; _i < _eventTaskListCount; _i++) { 
		Common.ActivityObj.Activity_StepRewardEventTaskInfo _eventTaskList = new Common.ActivityObj.Activity_StepRewardEventTaskInfo();
		if(_buf.remaining() <= 0) return;
	int __eventTaskListCustLen = _buf.getInt();
	int __eventTaskListCurPos = _buf.position();
	_eventTaskList.ReadUnzipBuf(_buf, __eventTaskListCurPos + __eventTaskListCustLen);
	_buf.position(__eventTaskListCurPos + __eventTaskListCustLen);

		eventTaskList.add(_eventTaskList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(stepRewardId);
	_buf.putLong(score);
	_buf.putShort((short)hadDrawStepList.size());
	for(int _i = 0; _i < hadDrawStepList.size(); _i++) { 
		_buf.putInt(hadDrawStepList.get(_i));
	}
	_buf.putShort((short)eventTaskList.size());
	for(int _i = 0; _i < eventTaskList.size(); _i++) { 
		_buf.putInt(eventTaskList.get(_i).GetBufSize());
	eventTaskList.get(_i).PutUnzipBuf(_buf);
	}
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

