package GC2GS.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 领取活动阶段奖励
 **/
public class GC2GS_017_011_ReqDrawActivityStepReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例id */
private long instanceId;
/** 阶段奖励id */
private long stepRewardId;
/** 阶段id */
private int stepId;


public GC2GS_017_011_ReqDrawActivityStepReward() {
	instanceId = (long)0;
	stepRewardId = (long)0;
	stepId = 0;
}

public GC2GS_017_011_ReqDrawActivityStepReward(
	 long _instanceId
	, long _stepRewardId
	, int _stepId
) {	instanceId = _instanceId;
	stepRewardId = _stepRewardId;
	stepId = _stepId;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)11; }

/** 活动实例id */
public long getInstanceId() { return instanceId; }
/** 活动实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 阶段奖励id */
public long getStepRewardId() { return stepRewardId; }
/** 阶段奖励id */
public void setStepRewardId(long _stepRewardId) { stepRewardId = _stepRewardId; }
/** 阶段id */
public int getStepId() { return stepId; }
/** 阶段id */
public void setStepId(int _stepId) { stepId = _stepId; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stepRewardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stepId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(stepRewardId);
	_buf.putInt(stepId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)11);
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

