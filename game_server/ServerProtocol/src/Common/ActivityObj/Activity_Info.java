package Common.ActivityObj;

import java.nio.ByteBuffer;
/*********
 * 活动信息
 **/
public class Activity_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long instanceId;
private long activityId;
/** 活动开始时间 仅用于展示 */
private long startTimeMs;
/** 活动结束时间 仅用于展示 */
private long endTimeMs;
/** 活动结算时间 仅用于展示 */
private long settleTimeMs;
/** 活动关闭时间 仅用于展示 */
private long closeTimeMs;
/** 当前状态 */
private Common.ActivityEnum.EActivityState state;
/** 参与跨服的usId列表 空列表则为单服活动 */
private java.util.ArrayList<Integer> usIdList;
/** 来自SS服务器排期的自增ID，可以用于上传跨服组队服务器，用于表示组队的分组ID */
private long usGroupId;


public Activity_Info() {
	instanceId = (long)0;
	activityId = (long)0;
	startTimeMs = (long)0;
	endTimeMs = (long)0;
	settleTimeMs = (long)0;
	closeTimeMs = (long)0;
	state = Common.ActivityEnum.EActivityState.values()[0];
	usIdList = new java.util.ArrayList<Integer>();
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
	, java.util.ArrayList<Integer> _usIdList
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 活动实例ID */
public long getInstanceId() { return instanceId; }
/** 活动实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
public long getActivityId() { return activityId; }
public void setActivityId(long _activityId) { activityId = _activityId; }
/** 活动开始时间 仅用于展示 */
public long getStartTimeMs() { return startTimeMs; }
/** 活动开始时间 仅用于展示 */
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/** 活动结束时间 仅用于展示 */
public long getEndTimeMs() { return endTimeMs; }
/** 活动结束时间 仅用于展示 */
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }
/** 活动结算时间 仅用于展示 */
public long getSettleTimeMs() { return settleTimeMs; }
/** 活动结算时间 仅用于展示 */
public void setSettleTimeMs(long _settleTimeMs) { settleTimeMs = _settleTimeMs; }
/** 活动关闭时间 仅用于展示 */
public long getCloseTimeMs() { return closeTimeMs; }
/** 活动关闭时间 仅用于展示 */
public void setCloseTimeMs(long _closeTimeMs) { closeTimeMs = _closeTimeMs; }
/** 当前状态 */
public Common.ActivityEnum.EActivityState getState() { return state; }
/** 当前状态 */
public void setState(Common.ActivityEnum.EActivityState _state) { state = _state; }
/** 参与跨服的usId列表 空列表则为单服活动 */
public java.util.ArrayList<Integer> getUsIdList() { return usIdList; }
/** 参与跨服的usId列表 空列表则为单服活动 */
public void addUsIdList(int _usIdList) { usIdList.add(_usIdList); }
/** 来自SS服务器排期的自增ID，可以用于上传跨服组队服务器，用于表示组队的分组ID */
public long getUsGroupId() { return usGroupId; }
/** 来自SS服务器排期的自增ID，可以用于上传跨服组队服务器，用于表示组队的分组ID */
public void setUsGroupId(long _usGroupId) { usGroupId = _usGroupId; }


public final int GetBufSize() {
	int _size = 60;
	_size += 2 + (usIdList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 62;
	_size += 2 + (usIdList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) settleTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) closeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) state = Common.ActivityEnum.EActivityState.EActivityState_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _usIdListCount = _buf.getShort();
	for(int _i = 0; _i < _usIdListCount; _i++) { 
		int _usIdList = 0;
		if(_buf.remaining() > 0) _usIdList = _buf.getInt();
		usIdList.add(_usIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usGroupId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(activityId);
	_buf.putLong(startTimeMs);
	_buf.putLong(endTimeMs);
	_buf.putLong(settleTimeMs);
	_buf.putLong(closeTimeMs);
	_buf.putInt(state.ordinal());

	_buf.putShort((short)usIdList.size());
	for(int _i = 0; _i < usIdList.size(); _i++) { 
		_buf.putInt(usIdList.get(_i));
	}
	_buf.putLong(usGroupId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

