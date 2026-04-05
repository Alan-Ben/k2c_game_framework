package GC2GS.p033_SimpleActivityOp;

import java.nio.ByteBuffer;
/*********
 * 领取赚速目标奖励
 **/
public class GC2GS_033_002_ReqEarningsGoalDrawReward implements ALBasicProtocolPack._IALProtocolStructure {
private long refId;
/** 活动实例ID */
private long activityInstanceId;


public GC2GS_033_002_ReqEarningsGoalDrawReward() {
	refId = (long)0;
	activityInstanceId = (long)0;
}

public GC2GS_033_002_ReqEarningsGoalDrawReward(
	 long _refId
	, long _activityInstanceId
) {	refId = _refId;
	activityInstanceId = _activityInstanceId;
}

public final byte getMainOrder() { return (byte)33; }

public final byte getSubOrder() { return (byte)2; }

public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
/** 活动实例ID */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }


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
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refId);
	_buf.putLong(activityInstanceId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)2);
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

