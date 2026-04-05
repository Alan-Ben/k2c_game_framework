package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_018_RetActivityList implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动信息列表 */
private java.util.ArrayList<Common.ActivityObj.Activity_Info> activityList;
/** 活动玩家相关信息列表 */
private java.util.ArrayList<Common.ActivityObj.Activity_PlayerData> playerDataList;
/** 活动阶段奖励信息列表 */
private java.util.ArrayList<Common.ActivityObj.Activity_StepRewardInfo> stepRewardList;


public GS2GC_002_018_RetActivityList() {
	activityList = new java.util.ArrayList<Common.ActivityObj.Activity_Info>();
	playerDataList = new java.util.ArrayList<Common.ActivityObj.Activity_PlayerData>();
	stepRewardList = new java.util.ArrayList<Common.ActivityObj.Activity_StepRewardInfo>();
}

public GS2GC_002_018_RetActivityList(
	 java.util.ArrayList<Common.ActivityObj.Activity_Info> _activityList
	, java.util.ArrayList<Common.ActivityObj.Activity_PlayerData> _playerDataList
	, java.util.ArrayList<Common.ActivityObj.Activity_StepRewardInfo> _stepRewardList
) {	activityList = _activityList;
	playerDataList = _playerDataList;
	stepRewardList = _stepRewardList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)18; }

/** 活动信息列表 */
public java.util.ArrayList<Common.ActivityObj.Activity_Info> getActivityList() { return activityList; }
/** 活动信息列表 */
public void addActivityList(Common.ActivityObj.Activity_Info _activityList) { activityList.add(_activityList); }
/** 活动玩家相关信息列表 */
public java.util.ArrayList<Common.ActivityObj.Activity_PlayerData> getPlayerDataList() { return playerDataList; }
/** 活动玩家相关信息列表 */
public void addPlayerDataList(Common.ActivityObj.Activity_PlayerData _playerDataList) { playerDataList.add(_playerDataList); }
/** 活动阶段奖励信息列表 */
public java.util.ArrayList<Common.ActivityObj.Activity_StepRewardInfo> getStepRewardList() { return stepRewardList; }
/** 活动阶段奖励信息列表 */
public void addStepRewardList(Common.ActivityObj.Activity_StepRewardInfo _stepRewardList) { stepRewardList.add(_stepRewardList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < activityList.size(); _i++) {
	_size += 4 + activityList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < playerDataList.size(); _i++) {
	_size += 4 + playerDataList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < stepRewardList.size(); _i++) {
	_size += 4 + stepRewardList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < activityList.size(); _i++) {
	_size += 4 + activityList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < playerDataList.size(); _i++) {
	_size += 4 + playerDataList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < stepRewardList.size(); _i++) {
	_size += 4 + stepRewardList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _activityListCount = _buf.getShort();
	for(int _i = 0; _i < _activityListCount; _i++) { 
		Common.ActivityObj.Activity_Info _activityList = new Common.ActivityObj.Activity_Info();
		if(_buf.remaining() <= 0) return;
	int __activityListCustLen = _buf.getInt();
	int __activityListCurPos = _buf.position();
	_activityList.ReadUnzipBuf(_buf, __activityListCurPos + __activityListCustLen);
	_buf.position(__activityListCurPos + __activityListCustLen);

		activityList.add(_activityList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _playerDataListCount = _buf.getShort();
	for(int _i = 0; _i < _playerDataListCount; _i++) { 
		Common.ActivityObj.Activity_PlayerData _playerDataList = new Common.ActivityObj.Activity_PlayerData();
		if(_buf.remaining() <= 0) return;
	int __playerDataListCustLen = _buf.getInt();
	int __playerDataListCurPos = _buf.position();
	_playerDataList.ReadUnzipBuf(_buf, __playerDataListCurPos + __playerDataListCustLen);
	_buf.position(__playerDataListCurPos + __playerDataListCustLen);

		playerDataList.add(_playerDataList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _stepRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _stepRewardListCount; _i++) { 
		Common.ActivityObj.Activity_StepRewardInfo _stepRewardList = new Common.ActivityObj.Activity_StepRewardInfo();
		if(_buf.remaining() <= 0) return;
	int __stepRewardListCustLen = _buf.getInt();
	int __stepRewardListCurPos = _buf.position();
	_stepRewardList.ReadUnzipBuf(_buf, __stepRewardListCurPos + __stepRewardListCustLen);
	_buf.position(__stepRewardListCurPos + __stepRewardListCustLen);

		stepRewardList.add(_stepRewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)activityList.size());
	for(int _i = 0; _i < activityList.size(); _i++) { 
		_buf.putInt(activityList.get(_i).GetBufSize());
	activityList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)playerDataList.size());
	for(int _i = 0; _i < playerDataList.size(); _i++) { 
		_buf.putInt(playerDataList.get(_i).GetBufSize());
	playerDataList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)stepRewardList.size());
	for(int _i = 0; _i < stepRewardList.size(); _i++) { 
		_buf.putInt(stepRewardList.get(_i).GetBufSize());
	stepRewardList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)18);
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

