using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动阶段事件任务奖励分数变更
/// </summary>
public class GS2GC_017_063_OnActivityStepRewardEventTaskScoreChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;
/// <summary>
/// 阶段奖励ID
/// </summary>
private long stepRewardId;
/// <summary>
/// 事件配置ID
/// </summary>
private long eventTaskId;
private long score;


public GS2GC_017_063_OnActivityStepRewardEventTaskScoreChg() {
	activityInstanceId = (long)0;
	stepRewardId = (long)0;
	eventTaskId = (long)0;
	score = (long)0;
}

public GS2GC_017_063_OnActivityStepRewardEventTaskScoreChg(
	long _activityInstanceId
	, long _stepRewardId
	, long _eventTaskId
	, long _score
) {	activityInstanceId = _activityInstanceId;
	stepRewardId = _stepRewardId;
	eventTaskId = _eventTaskId;
	score = _score;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)63; }

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
/// 事件配置ID
/// </summary>
public long getEventTaskId() { return eventTaskId; }
/// <summary>
/// 事件配置ID
/// </summary>
public void setEventTaskId(long _eventTaskId) { eventTaskId = _eventTaskId; }
public long getScore() { return score; }
public void setScore(long _score) { score = _score; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stepRewardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventTaskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(stepRewardId);
	_buf.putLong(eventTaskId);
	_buf.putLong(score);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)63);
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
	builder.Append("eventTaskId").Append(":").Append(eventTaskId.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

