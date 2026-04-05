package GS2GC.p033_SimpleActivityOp;

import java.nio.ByteBuffer;
public class GS2GC_033_109_OnRechargeRebateCountChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long activityInstanceId;
/** 返利组ID */
private long groupId;
/** 当前计数 VIP经验或充值天数 */
private long count;


public GS2GC_033_109_OnRechargeRebateCountChg() {
	activityInstanceId = (long)0;
	groupId = (long)0;
	count = (long)0;
}

public GS2GC_033_109_OnRechargeRebateCountChg(
	 long _activityInstanceId
	, long _groupId
	, long _count
) {	activityInstanceId = _activityInstanceId;
	groupId = _groupId;
	count = _count;
}

public final byte getMainOrder() { return (byte)33; }

public final byte getSubOrder() { return (byte)109; }

/** 活动实例ID */
public long getActivityInstanceId() { return activityInstanceId; }
/** 活动实例ID */
public void setActivityInstanceId(long _activityInstanceId) { activityInstanceId = _activityInstanceId; }
/** 返利组ID */
public long getGroupId() { return groupId; }
/** 返利组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 当前计数 VIP经验或充值天数 */
public long getCount() { return count; }
/** 当前计数 VIP经验或充值天数 */
public void setCount(long _count) { count = _count; }


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
	if(_buf.remaining() > 0) activityInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activityInstanceId);
	_buf.putLong(groupId);
	_buf.putLong(count);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)33);
	_buf.put((byte)109);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)33);
	_recBuf.put((byte)109);
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

