using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p033_SimpleActivityOp
{

public class GS2GC_033_102_OnEarningsGoalHonorRewardReach : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 荣誉奖励信息
/// </summary>
private Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo honorRewardInfo;
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;


public GS2GC_033_102_OnEarningsGoalHonorRewardReach() {
	honorRewardInfo = new Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo();
	activityInstanceId = (long)0;
}

public GS2GC_033_102_OnEarningsGoalHonorRewardReach(
	Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo _honorRewardInfo
	, long _activityInstanceId
) {	honorRewardInfo = _honorRewardInfo;
	activityInstanceId = _activityInstanceId;
}

public byte getMainOrder() { return (byte)33; }

public byte getSubOrder() { return (byte)102; }

/// <summary>
/// 荣誉奖励信息
/// </summary>
public Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo getHonorRewardInfo() { return honorRewardInfo; }
/// <summary>
/// 荣誉奖励信息
/// </summary>
public void setHonorRewardInfo(Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo _honorRewardInfo) { honorRewardInfo = _honorRewardInfo; }
/// <summary>
/// 活动实例ID
/// </summary>
public long getActivityInstanceId() { return activityInstanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }


public int GetBufSize() {
	int _size = 36;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _honorRewardInfoCustLen = _buf.getInt();
	int _honorRewardInfoCurPos = _buf.getCurPos();
	honorRewardInfo.ReadUnzipBuf(_buf, _honorRewardInfoCurPos + _honorRewardInfoCustLen);
	_buf.setPosition(_honorRewardInfoCurPos + _honorRewardInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(honorRewardInfo.GetBufSize());
	honorRewardInfo.PutUnzipBuf(_buf);
	_buf.putLong(activityInstanceId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)102);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)102);
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
	builder.Append("honorRewardInfo").Append(":").Append(honorRewardInfo == null ? "null" : honorRewardInfo.ToString()).Append(", ");
	builder.Append("activityInstanceId").Append(":").Append(activityInstanceId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

