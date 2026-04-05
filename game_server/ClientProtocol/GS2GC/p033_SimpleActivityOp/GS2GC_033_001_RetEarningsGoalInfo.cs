using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p033_SimpleActivityOp
{

public class GS2GC_033_001_RetEarningsGoalInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奖励信息列表
/// </summary>
private List<Common.EarningsGoalObj.EarningsGoal_RewardInfo> rewardInfoList;
/// <summary>
/// 荣誉奖励信息列表
/// </summary>
private List<Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo> honorRewardInfoList;
/// <summary>
/// 已领取奖励列表
/// </summary>
private List<long> hadDrawRewardList;
/// <summary>
/// 已领取荣誉奖励列表
/// </summary>
private List<long> hadDrawHonorRewardList;


public GS2GC_033_001_RetEarningsGoalInfo() {
	rewardInfoList = new List<Common.EarningsGoalObj.EarningsGoal_RewardInfo>();
	honorRewardInfoList = new List<Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo>();
	hadDrawRewardList = new List<long>();
	hadDrawHonorRewardList = new List<long>();
}

public GS2GC_033_001_RetEarningsGoalInfo(
	List<Common.EarningsGoalObj.EarningsGoal_RewardInfo> _rewardInfoList
	, List<Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo> _honorRewardInfoList
	, List<long> _hadDrawRewardList
	, List<long> _hadDrawHonorRewardList
) {	rewardInfoList = _rewardInfoList;
	honorRewardInfoList = _honorRewardInfoList;
	hadDrawRewardList = _hadDrawRewardList;
	hadDrawHonorRewardList = _hadDrawHonorRewardList;
}

public byte getMainOrder() { return (byte)33; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 奖励信息列表
/// </summary>
public List<Common.EarningsGoalObj.EarningsGoal_RewardInfo> getRewardInfoList() { return rewardInfoList; }
/// <summary>
/// 奖励信息列表
/// </summary>
public void addRewardInfoList(Common.EarningsGoalObj.EarningsGoal_RewardInfo _rewardInfoList) { rewardInfoList.Add(_rewardInfoList); }
/// <summary>
/// 荣誉奖励信息列表
/// </summary>
public List<Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo> getHonorRewardInfoList() { return honorRewardInfoList; }
/// <summary>
/// 荣誉奖励信息列表
/// </summary>
public void addHonorRewardInfoList(Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo _honorRewardInfoList) { honorRewardInfoList.Add(_honorRewardInfoList); }
/// <summary>
/// 已领取奖励列表
/// </summary>
public List<long> getHadDrawRewardList() { return hadDrawRewardList; }
/// <summary>
/// 已领取奖励列表
/// </summary>
public void addHadDrawRewardList(long _hadDrawRewardList) { hadDrawRewardList.Add(_hadDrawRewardList); }
/// <summary>
/// 已领取荣誉奖励列表
/// </summary>
public List<long> getHadDrawHonorRewardList() { return hadDrawHonorRewardList; }
/// <summary>
/// 已领取荣誉奖励列表
/// </summary>
public void addHadDrawHonorRewardList(long _hadDrawHonorRewardList) { hadDrawHonorRewardList.Add(_hadDrawHonorRewardList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (rewardInfoList.Count * 28);
	_size += 2 + (honorRewardInfoList.Count * 28);
	_size += 2 + (hadDrawRewardList.Count * 8);
	_size += 2 + (hadDrawHonorRewardList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (rewardInfoList.Count * 28);
	_size += 2 + (honorRewardInfoList.Count * 28);
	_size += 2 + (hadDrawRewardList.Count * 8);
	_size += 2 + (hadDrawHonorRewardList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rewardInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardInfoListCount; _i++) { 
		Common.EarningsGoalObj.EarningsGoal_RewardInfo _rewardInfoList = new Common.EarningsGoalObj.EarningsGoal_RewardInfo();
		int __rewardInfoListCustLen = _buf.getInt();
	int __rewardInfoListCurPos = _buf.getCurPos();
	_rewardInfoList.ReadUnzipBuf(_buf, __rewardInfoListCurPos + __rewardInfoListCustLen);
	_buf.setPosition(__rewardInfoListCurPos + __rewardInfoListCustLen);

		rewardInfoList.Add(_rewardInfoList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _honorRewardInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _honorRewardInfoListCount; _i++) { 
		Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo _honorRewardInfoList = new Common.EarningsGoalObj.EarningsGoal_HonorRewardInfo();
		int __honorRewardInfoListCustLen = _buf.getInt();
	int __honorRewardInfoListCurPos = _buf.getCurPos();
	_honorRewardInfoList.ReadUnzipBuf(_buf, __honorRewardInfoListCurPos + __honorRewardInfoListCustLen);
	_buf.setPosition(__honorRewardInfoListCurPos + __honorRewardInfoListCustLen);

		honorRewardInfoList.Add(_honorRewardInfoList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawRewardListCount; _i++) { 
		long _hadDrawRewardList = (long)0;
		_hadDrawRewardList = _buf.getLong();
		hadDrawRewardList.Add(_hadDrawRewardList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawHonorRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawHonorRewardListCount; _i++) { 
		long _hadDrawHonorRewardList = (long)0;
		_hadDrawHonorRewardList = _buf.getLong();
		hadDrawHonorRewardList.Add(_hadDrawHonorRewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)rewardInfoList.Count);
	for(int _i = 0; _i < rewardInfoList.Count; _i++) { 
		_buf.putInt(rewardInfoList[_i].GetBufSize());
	rewardInfoList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)honorRewardInfoList.Count);
	for(int _i = 0; _i < honorRewardInfoList.Count; _i++) { 
		_buf.putInt(honorRewardInfoList[_i].GetBufSize());
	honorRewardInfoList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)hadDrawRewardList.Count);
	for(int _i = 0; _i < hadDrawRewardList.Count; _i++) { 
		_buf.putLong(hadDrawRewardList[_i]);
	}
	_buf.putShort((short)hadDrawHonorRewardList.Count);
	for(int _i = 0; _i < hadDrawHonorRewardList.Count; _i++) { 
		_buf.putLong(hadDrawHonorRewardList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)1);
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
	builder.Append("rewardInfoList").Append(":").Append(rewardInfoList.ToString()).Append(", ");
	builder.Append("honorRewardInfoList").Append(":").Append(honorRewardInfoList.ToString()).Append(", ");
	builder.Append("hadDrawRewardList").Append(":").Append(hadDrawRewardList.ToString()).Append(", ");
	builder.Append("hadDrawHonorRewardList").Append(":").Append(hadDrawHonorRewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

