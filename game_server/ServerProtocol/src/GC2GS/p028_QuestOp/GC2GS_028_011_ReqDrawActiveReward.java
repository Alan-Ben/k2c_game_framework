package GC2GS.p028_QuestOp;

import java.nio.ByteBuffer;
/*********
 * 领取活跃度奖励
 **/
public class GC2GS_028_011_ReqDrawActiveReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 任务刷新序列号 */
private long refreshSerial;
/** 活跃奖励id */
private long activeRewardId;


public GC2GS_028_011_ReqDrawActiveReward() {
	refreshSerial = (long)0;
	activeRewardId = (long)0;
}

public GC2GS_028_011_ReqDrawActiveReward(
	 long _refreshSerial
	, long _activeRewardId
) {	refreshSerial = _refreshSerial;
	activeRewardId = _activeRewardId;
}

public final byte getMainOrder() { return (byte)28; }

public final byte getSubOrder() { return (byte)11; }

/** 任务刷新序列号 */
public long getRefreshSerial() { return refreshSerial; }
/** 任务刷新序列号 */
public void setRefreshSerial(long _refreshSerial) { refreshSerial = _refreshSerial; }
/** 活跃奖励id */
public long getActiveRewardId() { return activeRewardId; }
/** 活跃奖励id */
public void setActiveRewardId(long _activeRewardId) { activeRewardId = _activeRewardId; }


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
	if(_buf.remaining() > 0) refreshSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activeRewardId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refreshSerial);
	_buf.putLong(activeRewardId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
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

