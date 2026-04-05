package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动阶段奖励分数变更
 **/
public class GS2GC_017_057_OnActivityStepRewardScoreChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long instanceId;
/** 阶段奖励ID */
private long stepRewardId;
private long score;


public GS2GC_017_057_OnActivityStepRewardScoreChg() {
	instanceId = (long)0;
	stepRewardId = (long)0;
	score = (long)0;
}

public GS2GC_017_057_OnActivityStepRewardScoreChg(
	 long _instanceId
	, long _stepRewardId
	, long _score
) {	instanceId = _instanceId;
	stepRewardId = _stepRewardId;
	score = _score;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)57; }

/** 活动实例ID */
public long getInstanceId() { return instanceId; }
/** 活动实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 阶段奖励ID */
public long getStepRewardId() { return stepRewardId; }
/** 阶段奖励ID */
public void setStepRewardId(long _stepRewardId) { stepRewardId = _stepRewardId; }
public long getScore() { return score; }
public void setScore(long _score) { score = _score; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stepRewardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(stepRewardId);
	_buf.putLong(score);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)57);
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

