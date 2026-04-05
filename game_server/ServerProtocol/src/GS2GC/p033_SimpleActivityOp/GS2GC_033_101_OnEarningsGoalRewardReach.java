package GS2GC.p033_SimpleActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_033_101_OnEarningsGoalRewardReach implements ALBasicProtocolPack._IALProtocolStructure {
/** 奖励信息 */
private Common.EarningsGoalObj.EarningsGoal_RewardInfo rewardInfo;
/** 活动实例ID */
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

public final byte getMainOrder() { return (byte)33; }

public final byte getSubOrder() { return (byte)101; }

/** 奖励信息 */
public Common.EarningsGoalObj.EarningsGoal_RewardInfo getRewardInfo() { return rewardInfo; }
/** 奖励信息 */
public void setRewardInfo(Common.EarningsGoalObj.EarningsGoal_RewardInfo _rewardInfo) { rewardInfo = _rewardInfo; }
/** 活动实例ID */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }


public final int GetBufSize() {
	int _size = 36;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _rewardInfoCustLen = _buf.getInt();
	int _rewardInfoCurPos = _buf.position();
	rewardInfo.ReadUnzipBuf(_buf, _rewardInfoCurPos + _rewardInfoCustLen);
	_buf.position(_rewardInfoCurPos + _rewardInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(rewardInfo.GetBufSize());
	rewardInfo.PutUnzipBuf(_buf);
	_buf.putLong(activityInstanceId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)101);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)101);
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

