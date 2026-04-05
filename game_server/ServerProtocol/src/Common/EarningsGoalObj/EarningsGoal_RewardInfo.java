package Common.EarningsGoalObj;

import java.nio.ByteBuffer;
/*********
 * 赚速奖励信息
 **/
public class EarningsGoal_RewardInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long refId;
/** 首达玩家cid */
private long firstReachCid;
/** 达成时间戳 */
private long timestamp;


public EarningsGoal_RewardInfo() {
	refId = (long)0;
	firstReachCid = (long)0;
	timestamp = (long)0;
}

public EarningsGoal_RewardInfo(
	 long _refId
	, long _firstReachCid
	, long _timestamp
) {	refId = _refId;
	firstReachCid = _firstReachCid;
	timestamp = _timestamp;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
/** 首达玩家cid */
public long getFirstReachCid() { return firstReachCid; }
/** 首达玩家cid */
public void setFirstReachCid(long _firstReachCid) { firstReachCid = _firstReachCid; }
/** 达成时间戳 */
public long getTimestamp() { return timestamp; }
/** 达成时间戳 */
public void setTimestamp(long _timestamp) { timestamp = _timestamp; }


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
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) firstReachCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timestamp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(refId);
	_buf.putLong(firstReachCid);
	_buf.putLong(timestamp);
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

