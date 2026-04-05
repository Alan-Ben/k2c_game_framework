package Common.ActivityObj;

import java.nio.ByteBuffer;
/*********
 * 活动阶段奖励事件任务信息
 **/
public class Activity_StepRewardEventTaskInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件配置ID */
private long eventTaskId;
/** 分数 */
private long score;


public Activity_StepRewardEventTaskInfo() {
	eventTaskId = (long)0;
	score = (long)0;
}

public Activity_StepRewardEventTaskInfo(
	 long _eventTaskId
	, long _score
) {	eventTaskId = _eventTaskId;
	score = _score;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 事件配置ID */
public long getEventTaskId() { return eventTaskId; }
/** 事件配置ID */
public void setEventTaskId(long _eventTaskId) { eventTaskId = _eventTaskId; }
/** 分数 */
public long getScore() { return score; }
/** 分数 */
public void setScore(long _score) { score = _score; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventTaskId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(eventTaskId);
	_buf.putLong(score);
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

