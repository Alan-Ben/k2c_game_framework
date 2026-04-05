using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_018_RetActivityList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动信息列表
/// </summary>
private List<Common.ActivityObj.Activity_Info> activityList;
/// <summary>
/// 活动玩家相关信息列表
/// </summary>
private List<Common.ActivityObj.Activity_PlayerData> playerDataList;
/// <summary>
/// 活动阶段奖励信息列表
/// </summary>
private List<Common.ActivityObj.Activity_StepRewardInfo> stepRewardList;


public GS2GC_002_018_RetActivityList() {
	activityList = new List<Common.ActivityObj.Activity_Info>();
	playerDataList = new List<Common.ActivityObj.Activity_PlayerData>();
	stepRewardList = new List<Common.ActivityObj.Activity_StepRewardInfo>();
}

public GS2GC_002_018_RetActivityList(
	List<Common.ActivityObj.Activity_Info> _activityList
	, List<Common.ActivityObj.Activity_PlayerData> _playerDataList
	, List<Common.ActivityObj.Activity_StepRewardInfo> _stepRewardList
) {	activityList = _activityList;
	playerDataList = _playerDataList;
	stepRewardList = _stepRewardList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)18; }

/// <summary>
/// 活动信息列表
/// </summary>
public List<Common.ActivityObj.Activity_Info> getActivityList() { return activityList; }
/// <summary>
/// 活动信息列表
/// </summary>
public void addActivityList(Common.ActivityObj.Activity_Info _activityList) { activityList.Add(_activityList); }
/// <summary>
/// 活动玩家相关信息列表
/// </summary>
public List<Common.ActivityObj.Activity_PlayerData> getPlayerDataList() { return playerDataList; }
/// <summary>
/// 活动玩家相关信息列表
/// </summary>
public void addPlayerDataList(Common.ActivityObj.Activity_PlayerData _playerDataList) { playerDataList.Add(_playerDataList); }
/// <summary>
/// 活动阶段奖励信息列表
/// </summary>
public List<Common.ActivityObj.Activity_StepRewardInfo> getStepRewardList() { return stepRewardList; }
/// <summary>
/// 活动阶段奖励信息列表
/// </summary>
public void addStepRewardList(Common.ActivityObj.Activity_StepRewardInfo _stepRewardList) { stepRewardList.Add(_stepRewardList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < activityList.Count; _i++) {
	_size += 4 + activityList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < playerDataList.Count; _i++) {
	_size += 4 + playerDataList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < stepRewardList.Count; _i++) {
	_size += 4 + stepRewardList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < activityList.Count; _i++) {
	_size += 4 + activityList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < playerDataList.Count; _i++) {
	_size += 4 + playerDataList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < stepRewardList.Count; _i++) {
	_size += 4 + stepRewardList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _activityListCount = _buf.getShort();
	for(int _i = 0; _i < _activityListCount; _i++) { 
		Common.ActivityObj.Activity_Info _activityList = new Common.ActivityObj.Activity_Info();
		int __activityListCustLen = _buf.getInt();
	int __activityListCurPos = _buf.getCurPos();
	_activityList.ReadUnzipBuf(_buf, __activityListCurPos + __activityListCustLen);
	_buf.setPosition(__activityListCurPos + __activityListCustLen);

		activityList.Add(_activityList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _playerDataListCount = _buf.getShort();
	for(int _i = 0; _i < _playerDataListCount; _i++) { 
		Common.ActivityObj.Activity_PlayerData _playerDataList = new Common.ActivityObj.Activity_PlayerData();
		int __playerDataListCustLen = _buf.getInt();
	int __playerDataListCurPos = _buf.getCurPos();
	_playerDataList.ReadUnzipBuf(_buf, __playerDataListCurPos + __playerDataListCustLen);
	_buf.setPosition(__playerDataListCurPos + __playerDataListCustLen);

		playerDataList.Add(_playerDataList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _stepRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _stepRewardListCount; _i++) { 
		Common.ActivityObj.Activity_StepRewardInfo _stepRewardList = new Common.ActivityObj.Activity_StepRewardInfo();
		int __stepRewardListCustLen = _buf.getInt();
	int __stepRewardListCurPos = _buf.getCurPos();
	_stepRewardList.ReadUnzipBuf(_buf, __stepRewardListCurPos + __stepRewardListCustLen);
	_buf.setPosition(__stepRewardListCurPos + __stepRewardListCustLen);

		stepRewardList.Add(_stepRewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)activityList.Count);
	for(int _i = 0; _i < activityList.Count; _i++) { 
		_buf.putInt(activityList[_i].GetBufSize());
	activityList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)playerDataList.Count);
	for(int _i = 0; _i < playerDataList.Count; _i++) { 
		_buf.putInt(playerDataList[_i].GetBufSize());
	playerDataList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)stepRewardList.Count);
	for(int _i = 0; _i < stepRewardList.Count; _i++) { 
		_buf.putInt(stepRewardList[_i].GetBufSize());
	stepRewardList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)18);
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
	builder.Append("activityList").Append(":").Append(activityList.ToString()).Append(", ");
	builder.Append("playerDataList").Append(":").Append(playerDataList.ToString()).Append(", ");
	builder.Append("stepRewardList").Append(":").Append(stepRewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

