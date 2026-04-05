using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ActivityObj
{

/// <summary>
/// 活动阶段奖励事件任务信息
/// </summary>
public class Activity_StepRewardEventTaskInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 事件配置ID
/// </summary>
private long eventTaskId;
/// <summary>
/// 分数
/// </summary>
private long score;


public Activity_StepRewardEventTaskInfo() {
	eventTaskId = (long)0;
	score = (long)0;
}

public Activity_StepRewardEventTaskInfo(
	long _eventTaskId
	, long _score
) {	eventTaskId = _eventTaskId;
	score = _score;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 事件配置ID
/// </summary>
public long getEventTaskId() { return eventTaskId; }
/// <summary>
/// 事件配置ID
/// </summary>
public void setEventTaskId(long _eventTaskId) { eventTaskId = _eventTaskId; }
/// <summary>
/// 分数
/// </summary>
public long getScore() { return score; }
/// <summary>
/// 分数
/// </summary>
public void setScore(long _score) { score = _score; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventTaskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(eventTaskId);
	_buf.putLong(score);
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
	builder.Append("eventTaskId").Append(":").Append(eventTaskId.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

