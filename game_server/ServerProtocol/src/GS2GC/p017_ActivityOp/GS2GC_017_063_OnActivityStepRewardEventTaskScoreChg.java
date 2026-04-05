package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动阶段事件任务奖励分数变更
 **/
public class GS2GC_017_063_OnActivityStepRewardEventTaskScoreChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long activityInstanceId;
/** 阶段奖励ID */
private long stepRewardId;
/** 事件配置ID */
private long eventTaskId;
private long score;


public GS2GC_017_063_OnActivityStepRewardEventTaskScoreChg() {
	activityInstanceId = (long)0;
	stepRewardId = (long)0;
	eventTaskId = (long)0;
	score = (long)0;
}

public GS2GC_017_063_OnActivityStepRewardEventTaskScoreChg(
	 long _activityInstanceId
	, long _stepRewardId
	, long _eventTaskId
	, long _score
) {	activityInstanceId = _activityInstanceId;
	stepRewardId = _stepRewardId;
	eventTaskId = _eventTaskId;
	score = _score;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)63; }

/** 活动实例ID */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/** 阶段奖励ID */
public long getStepRewardId() { return stepRewardId; }
/** 阶段奖励ID */
public void setStepRewardId(long _stepRewardId) { stepRewardId = _stepRewardId; }
/** 事件配置ID */
public long getEventTaskId() { return eventTaskId; }
/** 事件配置ID */
public void setEventTaskId(long _eventTaskId) { eventTaskId = _eventTaskId; }
public long getScore() { return score; }
public void setScore(long _score) { score = _score; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stepRewardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventTaskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(stepRewardId);
	_buf.putLong(eventTaskId);
	_buf.putLong(score);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)63);
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

