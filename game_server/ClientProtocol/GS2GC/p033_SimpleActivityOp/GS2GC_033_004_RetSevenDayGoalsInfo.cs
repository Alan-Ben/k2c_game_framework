using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p033_SimpleActivityOp
{

public class GS2GC_033_004_RetSevenDayGoalsInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务信息列表
/// </summary>
private List<Common.SimpleActivityObj.SevenDayGoals_TaskInfo> taskList;
/// <summary>
/// 已领取奖励列表
/// </summary>
private List<long> hadDrawRewardList;
/// <summary>
/// 当前积分
/// </summary>
private long score;
/// <summary>
/// 已领取的阶段奖励列表
/// </summary>
private List<long> hadDrawStepRewardList;


public GS2GC_033_004_RetSevenDayGoalsInfo() {
	taskList = new List<Common.SimpleActivityObj.SevenDayGoals_TaskInfo>();
	hadDrawRewardList = new List<long>();
	score = (long)0;
	hadDrawStepRewardList = new List<long>();
}

public GS2GC_033_004_RetSevenDayGoalsInfo(
	List<Common.SimpleActivityObj.SevenDayGoals_TaskInfo> _taskList
	, List<long> _hadDrawRewardList
	, long _score
	, List<long> _hadDrawStepRewardList
) {	taskList = _taskList;
	hadDrawRewardList = _hadDrawRewardList;
	score = _score;
	hadDrawStepRewardList = _hadDrawStepRewardList;
}

public byte getMainOrder() { return (byte)33; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 任务信息列表
/// </summary>
public List<Common.SimpleActivityObj.SevenDayGoals_TaskInfo> getTaskList() { return taskList; }
/// <summary>
/// 任务信息列表
/// </summary>
public void addTaskList(Common.SimpleActivityObj.SevenDayGoals_TaskInfo _taskList) { taskList.Add(_taskList); }
/// <summary>
/// 已领取奖励列表
/// </summary>
public List<long> getHadDrawRewardList() { return hadDrawRewardList; }
/// <summary>
/// 已领取奖励列表
/// </summary>
public void addHadDrawRewardList(long _hadDrawRewardList) { hadDrawRewardList.Add(_hadDrawRewardList); }
/// <summary>
/// 当前积分
/// </summary>
public long getScore() { return score; }
/// <summary>
/// 当前积分
/// </summary>
public void setScore(long _score) { score = _score; }
/// <summary>
/// 已领取的阶段奖励列表
/// </summary>
public List<long> getHadDrawStepRewardList() { return hadDrawStepRewardList; }
/// <summary>
/// 已领取的阶段奖励列表
/// </summary>
public void addHadDrawStepRewardList(long _hadDrawStepRewardList) { hadDrawStepRewardList.Add(_hadDrawStepRewardList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (taskList.Count * 20);
	_size += 2 + (hadDrawRewardList.Count * 8);
	_size += 2 + (hadDrawStepRewardList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (taskList.Count * 20);
	_size += 2 + (hadDrawRewardList.Count * 8);
	_size += 2 + (hadDrawStepRewardList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _taskListCount = _buf.getShort();
	for(int _i = 0; _i < _taskListCount; _i++) { 
		Common.SimpleActivityObj.SevenDayGoals_TaskInfo _taskList = new Common.SimpleActivityObj.SevenDayGoals_TaskInfo();
		int __taskListCustLen = _buf.getInt();
	int __taskListCurPos = _buf.getCurPos();
	_taskList.ReadUnzipBuf(_buf, __taskListCurPos + __taskListCustLen);
	_buf.setPosition(__taskListCurPos + __taskListCustLen);

		taskList.Add(_taskList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawRewardListCount; _i++) { 
		long _hadDrawRewardList = (long)0;
		_hadDrawRewardList = _buf.getLong();
		hadDrawRewardList.Add(_hadDrawRewardList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawStepRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawStepRewardListCount; _i++) { 
		long _hadDrawStepRewardList = (long)0;
		_hadDrawStepRewardList = _buf.getLong();
		hadDrawStepRewardList.Add(_hadDrawStepRewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)taskList.Count);
	for(int _i = 0; _i < taskList.Count; _i++) { 
		_buf.putInt(taskList[_i].GetBufSize());
	taskList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)hadDrawRewardList.Count);
	for(int _i = 0; _i < hadDrawRewardList.Count; _i++) { 
		_buf.putLong(hadDrawRewardList[_i]);
	}
	_buf.putLong(score);
	_buf.putShort((short)hadDrawStepRewardList.Count);
	for(int _i = 0; _i < hadDrawStepRewardList.Count; _i++) { 
		_buf.putLong(hadDrawStepRewardList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)4);
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
	builder.Append("hadDrawRewardList").Append(":").Append(hadDrawRewardList.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("hadDrawStepRewardList").Append(":").Append(hadDrawStepRewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

