package GS2GC.p033_SimpleActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_033_001_RetEarningsGoalInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 奖励信息列表 */
private java.util.ArrayList<Common.EarningsGoalObj.EarningsGoal_RewardInfo> rewardInfoList;
/** 荣誉奖励信息列表 */
private java.util.ArrayList<Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo> honorRewardInfoList;
/** 已领取奖励列表 */
private java.util.ArrayList<Long> hadDrawRewardList;
/** 已领取荣誉奖励列表 */
private java.util.ArrayList<Long> hadDrawHonorRewardList;


public GS2GC_033_001_RetEarningsGoalInfo() {
	rewardInfoList = new java.util.ArrayList<Common.EarningsGoalObj.EarningsGoal_RewardInfo>();
	honorRewardInfoList = new java.util.ArrayList<Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo>();
	hadDrawRewardList = new java.util.ArrayList<Long>();
	hadDrawHonorRewardList = new java.util.ArrayList<Long>();
}

public GS2GC_033_001_RetEarningsGoalInfo(
	 java.util.ArrayList<Common.EarningsGoalObj.EarningsGoal_RewardInfo> _rewardInfoList
	, java.util.ArrayList<Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo> _honorRewardInfoList
	, java.util.ArrayList<Long> _hadDrawRewardList
	, java.util.ArrayList<Long> _hadDrawHonorRewardList
) {	rewardInfoList = _rewardInfoList;
	honorRewardInfoList = _honorRewardInfoList;
	hadDrawRewardList = _hadDrawRewardList;
	hadDrawHonorRewardList = _hadDrawHonorRewardList;
}

public final byte getMainOrder() { return (byte)33; }

public final byte getSubOrder() { return (byte)1; }

/** 奖励信息列表 */
public java.util.ArrayList<Common.EarningsGoalObj.EarningsGoal_RewardInfo> getRewardInfoList() { return rewardInfoList; }
/** 奖励信息列表 */
public void addRewardInfoList(Common.EarningsGoalObj.EarningsGoal_RewardInfo _rewardInfoList) { rewardInfoList.add(_rewardInfoList); }
/** 荣誉奖励信息列表 */
public java.util.ArrayList<Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo> getHonorRewardInfoList() { return honorRewardInfoList; }
/** 荣誉奖励信息列表 */
public void addHonorRewardInfoList(Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo _honorRewardInfoList) { honorRewardInfoList.add(_honorRewardInfoList); }
/** 已领取奖励列表 */
public java.util.ArrayList<Long> getHadDrawRewardList() { return hadDrawRewardList; }
/** 已领取奖励列表 */
public void addHadDrawRewardList(long _hadDrawRewardList) { hadDrawRewardList.add(_hadDrawRewardList); }
/** 已领取荣誉奖励列表 */
public java.util.ArrayList<Long> getHadDrawHonorRewardList() { return hadDrawHonorRewardList; }
/** 已领取荣誉奖励列表 */
public void addHadDrawHonorRewardList(long _hadDrawHonorRewardList) { hadDrawHonorRewardList.add(_hadDrawHonorRewardList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (rewardInfoList.size() * 28);
	_size += 2 + (honorRewardInfoList.size() * 28);
	_size += 2 + (hadDrawRewardList.size() * 8);
	_size += 2 + (hadDrawHonorRewardList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (rewardInfoList.size() * 28);
	_size += 2 + (honorRewardInfoList.size() * 28);
	_size += 2 + (hadDrawRewardList.size() * 8);
	_size += 2 + (hadDrawHonorRewardList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rewardInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardInfoListCount; _i++) { 
		Common.EarningsGoalObj.EarningsGoal_RewardInfo _rewardInfoList = new Common.EarningsGoalObj.EarningsGoal_RewardInfo();
		if(_buf.remaining() <= 0) return;
	int __rewardInfoListCustLen = _buf.getInt();
	int __rewardInfoListCurPos = _buf.position();
	_rewardInfoList.ReadUnzipBuf(_buf, __rewardInfoListCurPos + __rewardInfoListCustLen);
	_buf.position(__rewardInfoListCurPos + __rewardInfoListCustLen);

		rewardInfoList.add(_rewardInfoList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _honorRewardInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _honorRewardInfoListCount; _i++) { 
		Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo _honorRewardInfoList = new Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo();
		if(_buf.remaining() <= 0) return;
	int __honorRewardInfoListCustLen = _buf.getInt();
	int __honorRewardInfoListCurPos = _buf.position();
	_honorRewardInfoList.ReadUnzipBuf(_buf, __honorRewardInfoListCurPos + __honorRewardInfoListCustLen);
	_buf.position(__honorRewardInfoListCurPos + __honorRewardInfoListCustLen);

		honorRewardInfoList.add(_honorRewardInfoList);
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
	if(_buf.remaining() <= 0) return;
	short _hadDrawHonorRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawHonorRewardListCount; _i++) { 
		long _hadDrawHonorRewardList = (long)0;
		if(_buf.remaining() > 0) _hadDrawHonorRewardList = _buf.getLong();
		hadDrawHonorRewardList.add(_hadDrawHonorRewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)rewardInfoList.size());
	for(int _i = 0; _i < rewardInfoList.size(); _i++) { 
		_buf.putInt(rewardInfoList.get(_i).GetBufSize());
	rewardInfoList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)honorRewardInfoList.size());
	for(int _i = 0; _i < honorRewardInfoList.size(); _i++) { 
		_buf.putInt(honorRewardInfoList.get(_i).GetBufSize());
	honorRewardInfoList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)hadDrawRewardList.size());
	for(int _i = 0; _i < hadDrawRewardList.size(); _i++) { 
		_buf.putLong(hadDrawRewardList.get(_i));
	}
	_buf.putShort((short)hadDrawHonorRewardList.size());
	for(int _i = 0; _i < hadDrawHonorRewardList.size(); _i++) { 
		_buf.putLong(hadDrawHonorRewardList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)1);
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

