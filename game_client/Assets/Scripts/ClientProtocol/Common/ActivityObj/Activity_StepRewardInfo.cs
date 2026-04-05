using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ActivityObj
{

/// <summary>
/// 活动阶段奖励信息
/// </summary>
public class Activity_StepRewardInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;
/// <summary>
/// 阶段奖励ID
/// </summary>
private long stepRewardId;
/// <summary>
/// 分数
/// </summary>
private long score;
/// <summary>
/// 已领取阶段列表
/// </summary>
private List<int> hadDrawStepList;
/// <summary>
/// 事件任务数据列表
/// </summary>
private List<Common.ActivityObj.Activity_StepRewardEventTaskInfo> eventTaskList;


public Activity_StepRewardInfo() {
	activityInstanceId = (long)0;
	stepRewardId = (long)0;
	score = (long)0;
	hadDrawStepList = new List<int>();
	eventTaskList = new List<Common.ActivityObj.Activity_StepRewardEventTaskInfo>();
}

public Activity_StepRewardInfo(
	long _activityInstanceId
	, long _stepRewardId
	, long _score
	, List<int> _hadDrawStepList
	, List<Common.ActivityObj.Activity_StepRewardEventTaskInfo> _eventTaskList
) {	activityInstanceId = _activityInstanceId;
	stepRewardId = _stepRewardId;
	score = _score;
	hadDrawStepList = _hadDrawStepList;
	eventTaskList = _eventTaskList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getActivityInstanceId() { return activityInstanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/// <summary>
/// 阶段奖励ID
/// </summary>
public long getStepRewardId() { return stepRewardId; }
/// <summary>
/// 阶段奖励ID
/// </summary>
public void setStepRewardId(long _stepRewardId) { stepRewardId = _stepRewardId; }
/// <summary>
/// 分数
/// </summary>
public long getScore() { return score; }
/// <summary>
/// 分数
/// </summary>
public void setScore(long _score) { score = _score; }
/// <summary>
/// 已领取阶段列表
/// </summary>
public List<int> getHadDrawStepList() { return hadDrawStepList; }
/// <summary>
/// 已领取阶段列表
/// </summary>
public void addHadDrawStepList(int _hadDrawStepList) { hadDrawStepList.Add(_hadDrawStepList); }
/// <summary>
/// 事件任务数据列表
/// </summary>
public List<Common.ActivityObj.Activity_StepRewardEventTaskInfo> getEventTaskList() { return eventTaskList; }
/// <summary>
/// 事件任务数据列表
/// </summary>
public void addEventTaskList(Common.ActivityObj.Activity_StepRewardEventTaskInfo _eventTaskList) { eventTaskList.Add(_eventTaskList); }


public int GetBufSize() {
	int _size = 24;
	_size += 2 + (hadDrawStepList.Count * 4);
	_size += 2 + (eventTaskList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;
	_size += 2 + (hadDrawStepList.Count * 4);
	_size += 2 + (eventTaskList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stepRewardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawStepListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawStepListCount; _i++) { 
		int _hadDrawStepList = 0;
		_hadDrawStepList = _buf.getInt();
		hadDrawStepList.Add(_hadDrawStepList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _eventTaskListCount = _buf.getShort();
	for(int _i = 0; _i < _eventTaskListCount; _i++) { 
		Common.ActivityObj.Activity_StepRewardEventTaskInfo _eventTaskList = new Common.ActivityObj.Activity_StepRewardEventTaskInfo();
		int __eventTaskListCustLen = _buf.getInt();
	int __eventTaskListCurPos = _buf.getCurPos();
	_eventTaskList.ReadUnzipBuf(_buf, __eventTaskListCurPos + __eventTaskListCustLen);
	_buf.setPosition(__eventTaskListCurPos + __eventTaskListCustLen);

		eventTaskList.Add(_eventTaskList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(stepRewardId);
	_buf.putLong(score);
	_buf.putShort((short)hadDrawStepList.Count);
	for(int _i = 0; _i < hadDrawStepList.Count; _i++) { 
		_buf.putInt(hadDrawStepList[_i]);
	}
	_buf.putShort((short)eventTaskList.Count);
	for(int _i = 0; _i < eventTaskList.Count; _i++) { 
		_buf.putInt(eventTaskList[_i].GetBufSize());
	eventTaskList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("activityInstanceId").Append(":").Append(activityInstanceId.ToString()).Append(", ");
	builder.Append("stepRewardId").Append(":").Append(stepRewardId.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("hadDrawStepList").Append(":").Append(hadDrawStepList.ToString()).Append(", ");
	builder.Append("eventTaskList").Append(":").Append(eventTaskList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

