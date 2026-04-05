using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ActivityObj
{

/// <summary>
/// 活动信息
/// </summary>
public class Activity_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long instanceId;
private long activityId;
/// <summary>
/// 活动开始时间 仅用于展示
/// </summary>
private long startTimeMs;
/// <summary>
/// 活动结束时间 仅用于展示
/// </summary>
private long endTimeMs;
/// <summary>
/// 活动结算时间 仅用于展示
/// </summary>
private long settleTimeMs;
/// <summary>
/// 活动关闭时间 仅用于展示
/// </summary>
private long closeTimeMs;
/// <summary>
/// 当前状态
/// </summary>
private Common.ActivityEnum.EActivityState state;
/// <summary>
/// 参与跨服的usId列表 空列表则为单服活动
/// </summary>
private List<int> usIdList;
/// <summary>
/// 来自SS服务器排期的自增ID，可以用于上传跨服组队服务器，用于表示组队的分组ID
/// </summary>
private long usGroupId;


public Activity_Info() {
	instanceId = (long)0;
	activityId = (long)0;
	startTimeMs = (long)0;
	endTimeMs = (long)0;
	settleTimeMs = (long)0;
	closeTimeMs = (long)0;
	state = 0;
	usIdList = new List<int>();
	usGroupId = (long)0;
}

public Activity_Info(
	long _instanceId
	, long _activityId
	, long _startTimeMs
	, long _endTimeMs
	, long _settleTimeMs
	, long _closeTimeMs
	, Common.ActivityEnum.EActivityState _state
	, List<int> _usIdList
	, long _usGroupId
) {	instanceId = _instanceId;
	activityId = _activityId;
	startTimeMs = _startTimeMs;
	endTimeMs = _endTimeMs;
	settleTimeMs = _settleTimeMs;
	closeTimeMs = _closeTimeMs;
	state = _state;
	usIdList = _usIdList;
	usGroupId = _usGroupId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
public long getActivityId() { return activityId; }
public void setActivityId(long _activityId) { activityId = _activityId; }
/// <summary>
/// 活动开始时间 仅用于展示
/// </summary>
public long getStartTimeMs() { return startTimeMs; }
/// <summary>
/// 活动开始时间 仅用于展示
/// </summary>
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/// <summary>
/// 活动结束时间 仅用于展示
/// </summary>
public long getEndTimeMs() { return endTimeMs; }
/// <summary>
/// 活动结束时间 仅用于展示
/// </summary>
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }
/// <summary>
/// 活动结算时间 仅用于展示
/// </summary>
public long getSettleTimeMs() { return settleTimeMs; }
/// <summary>
/// 活动结算时间 仅用于展示
/// </summary>
public void setSettleTimeMs(long _settleTimeMs) { settleTimeMs = _settleTimeMs; }
/// <summary>
/// 活动关闭时间 仅用于展示
/// </summary>
public long getCloseTimeMs() { return closeTimeMs; }
/// <summary>
/// 活动关闭时间 仅用于展示
/// </summary>
public void setCloseTimeMs(long _closeTimeMs) { closeTimeMs = _closeTimeMs; }
/// <summary>
/// 当前状态
/// </summary>
public Common.ActivityEnum.EActivityState getState() { return state; }
/// <summary>
/// 当前状态
/// </summary>
public void setState(Common.ActivityEnum.EActivityState _state) { state = _state; }
/// <summary>
/// 参与跨服的usId列表 空列表则为单服活动
/// </summary>
public List<int> getUsIdList() { return usIdList; }
/// <summary>
/// 参与跨服的usId列表 空列表则为单服活动
/// </summary>
public void addUsIdList(int _usIdList) { usIdList.Add(_usIdList); }
/// <summary>
/// 来自SS服务器排期的自增ID，可以用于上传跨服组队服务器，用于表示组队的分组ID
/// </summary>
public long getUsGroupId() { return usGroupId; }
/// <summary>
/// 来自SS服务器排期的自增ID，可以用于上传跨服组队服务器，用于表示组队的分组ID
/// </summary>
public void setUsGroupId(long _usGroupId) { usGroupId = _usGroupId; }


public int GetBufSize() {
	int _size = 60;
	_size += 2 + (usIdList.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 62;
	_size += 2 + (usIdList.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	settleTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	closeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	state = (Common.ActivityEnum.EActivityState)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _usIdListCount = _buf.getShort();
	for(int _i = 0; _i < _usIdListCount; _i++) { 
		int _usIdList = 0;
		_usIdList = _buf.getInt();
		usIdList.Add(_usIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	usGroupId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(activityId);
	_buf.putLong(startTimeMs);
	_buf.putLong(endTimeMs);
	_buf.putLong(settleTimeMs);
	_buf.putLong(closeTimeMs);
	_buf.putInt((int)state);

	_buf.putShort((short)usIdList.Count);
	for(int _i = 0; _i < usIdList.Count; _i++) { 
		_buf.putInt(usIdList[_i]);
	}
	_buf.putLong(usGroupId);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("activityId").Append(":").Append(activityId.ToString()).Append(", ");
	builder.Append("startTimeMs").Append(":").Append(startTimeMs.ToString()).Append(", ");
	builder.Append("endTimeMs").Append(":").Append(endTimeMs.ToString()).Append(", ");
	builder.Append("settleTimeMs").Append(":").Append(settleTimeMs.ToString()).Append(", ");
	builder.Append("closeTimeMs").Append(":").Append(closeTimeMs.ToString()).Append(", ");
	builder.Append("state").Append(":").Append(state.ToString()).Append(", ");
	builder.Append("usIdList").Append(":").Append(usIdList.ToString()).Append(", ");
	builder.Append("usGroupId").Append(":").Append(usGroupId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

