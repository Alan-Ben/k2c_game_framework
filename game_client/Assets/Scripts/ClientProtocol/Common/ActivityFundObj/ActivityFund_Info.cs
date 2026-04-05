using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ActivityFundObj
{

/// <summary>
/// 活动基金-基金信息
/// </summary>
public class ActivityFund_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基金ID
/// </summary>
private long fundId;
/// <summary>
/// 活动实例ID（常驻基金为0）
/// </summary>
private long activityInstanceId;
/// <summary>
/// 公式分数
/// </summary>
private long formulaScore;
/// <summary>
/// 任务分数
/// </summary>
private long taskScore;
/// <summary>
/// 已领取免费档阶段
/// </summary>
private int hadDrawFreeStep;
/// <summary>
/// 已领取付费档阶段
/// </summary>
private int hadDrawPayStep;


public ActivityFund_Info() {
	fundId = (long)0;
	activityInstanceId = (long)0;
	formulaScore = (long)0;
	taskScore = (long)0;
	hadDrawFreeStep = 0;
	hadDrawPayStep = 0;
}

public ActivityFund_Info(
	long _fundId
	, long _activityInstanceId
	, long _formulaScore
	, long _taskScore
	, int _hadDrawFreeStep
	, int _hadDrawPayStep
) {	fundId = _fundId;
	activityInstanceId = _activityInstanceId;
	formulaScore = _formulaScore;
	taskScore = _taskScore;
	hadDrawFreeStep = _hadDrawFreeStep;
	hadDrawPayStep = _hadDrawPayStep;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 基金ID
/// </summary>
public long getFundId() { return fundId; }
/// <summary>
/// 基金ID
/// </summary>
public void setFundId(long _fundId) { fundId = _fundId; }
/// <summary>
/// 活动实例ID（常驻基金为0）
/// </summary>
public long getActivityInstanceId() { return activityInstanceId; }
/// <summary>
/// 活动实例ID（常驻基金为0）
/// </summary>
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/// <summary>
/// 公式分数
/// </summary>
public long getFormulaScore() { return formulaScore; }
/// <summary>
/// 公式分数
/// </summary>
public void setFormulaScore(long _formulaScore) { formulaScore = _formulaScore; }
/// <summary>
/// 任务分数
/// </summary>
public long getTaskScore() { return taskScore; }
/// <summary>
/// 任务分数
/// </summary>
public void setTaskScore(long _taskScore) { taskScore = _taskScore; }
/// <summary>
/// 已领取免费档阶段
/// </summary>
public int getHadDrawFreeStep() { return hadDrawFreeStep; }
/// <summary>
/// 已领取免费档阶段
/// </summary>
public void setHadDrawFreeStep(int _hadDrawFreeStep) { hadDrawFreeStep = _hadDrawFreeStep; }
/// <summary>
/// 已领取付费档阶段
/// </summary>
public int getHadDrawPayStep() { return hadDrawPayStep; }
/// <summary>
/// 已领取付费档阶段
/// </summary>
public void setHadDrawPayStep(int _hadDrawPayStep) { hadDrawPayStep = _hadDrawPayStep; }


public int GetBufSize() {
	int _size = 40;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fundId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	formulaScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	taskScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawFreeStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawPayStep = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(fundId);
	_buf.putLong(activityInstanceId);
	_buf.putLong(formulaScore);
	_buf.putLong(taskScore);
	_buf.putInt(hadDrawFreeStep);
	_buf.putInt(hadDrawPayStep);
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
	builder.Append("fundId").Append(":").Append(fundId.ToString()).Append(", ");
	builder.Append("activityInstanceId").Append(":").Append(activityInstanceId.ToString()).Append(", ");
	builder.Append("formulaScore").Append(":").Append(formulaScore.ToString()).Append(", ");
	builder.Append("taskScore").Append(":").Append(taskScore.ToString()).Append(", ");
	builder.Append("hadDrawFreeStep").Append(":").Append(hadDrawFreeStep.ToString()).Append(", ");
	builder.Append("hadDrawPayStep").Append(":").Append(hadDrawPayStep.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

