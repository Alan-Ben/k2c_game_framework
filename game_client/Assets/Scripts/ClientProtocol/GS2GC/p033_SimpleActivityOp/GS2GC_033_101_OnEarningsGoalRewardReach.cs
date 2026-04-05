using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p033_SimpleActivityOp
{

public class GS2GC_033_101_OnEarningsGoalRewardReach : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奖励信息
/// </summary>
private Common.EarningsGoalObj.EarningsGoal_RewardInfo rewardInfo;
/// <summary>
/// 活动实例ID
/// </summary>
private long activityInstanceId;


public GS2GC_033_101_OnEarningsGoalRewardReach() {
	rewardInfo = new Common.EarningsGoalObj.EarningsGoal_RewardInfo();
	activityInstanceId = (long)0;
}

public GS2GC_033_101_OnEarningsGoalRewardReach(
	Common.EarningsGoalObj.EarningsGoal_RewardInfo _rewardInfo
	, long _activityInstanceId
) {	rewardInfo = _rewardInfo;
	activityInstanceId = _activityInstanceId;
}

public byte getMainOrder() { return (byte)33; }

public byte getSubOrder() { return (byte)101; }

/// <summary>
/// 奖励信息
/// </summary>
public Common.EarningsGoalObj.EarningsGoal_RewardInfo getRewardInfo() { return rewardInfo; }
/// <summary>
/// 奖励信息
/// </summary>
public void setRewardInfo(Common.EarningsGoalObj.EarningsGoal_RewardInfo _rewardInfo) { rewardInfo = _rewardInfo; }
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
	int _rewardInfoCustLen = _buf.getInt();
	int _rewardInfoCurPos = _buf.getCurPos();
	rewardInfo.ReadUnzipBuf(_buf, _rewardInfoCurPos + _rewardInfoCustLen);
	_buf.setPosition(_rewardInfoCurPos + _rewardInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityInstanceId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(rewardInfo.GetBufSize());
	rewardInfo.PutUnzipBuf(_buf);
	_buf.putLong(activityInstanceId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)101);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)101);
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
	builder.Append("rewardInfo").Append(":").Append(rewardInfo == null ? "null" : rewardInfo.ToString()).Append(", ");
	builder.Append("activityInstanceId").Append(":").Append(activityInstanceId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

