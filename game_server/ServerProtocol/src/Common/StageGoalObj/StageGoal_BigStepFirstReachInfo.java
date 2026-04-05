package Common.StageGoalObj;

import java.nio.ByteBuffer;
/*********
 * 阶段目标大阶段首达数据
 **/
public class StageGoal_BigStepFirstReachInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 大阶段ID */
private long bigStepId;
/** 首达角色ID */
private long cid;
/** 首达时间，时间戳 */
private long reachTimeMs;


public StageGoal_BigStepFirstReachInfo() {
	bigStepId = (long)0;
	cid = (long)0;
	reachTimeMs = (long)0;
}

public StageGoal_BigStepFirstReachInfo(
	 long _bigStepId
	, long _cid
	, long _reachTimeMs
) {	bigStepId = _bigStepId;
	cid = _cid;
	reachTimeMs = _reachTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 大阶段ID */
public long getBigStepId() { return bigStepId; }
/** 大阶段ID */
public void setBigStepId(long _bigStepId) { bigStepId = _bigStepId; }
/** 首达角色ID */
public long getCid() { return cid; }
/** 首达角色ID */
public void setCid(long _cid) { cid = _cid; }
/** 首达时间，时间戳 */
public long getReachTimeMs() { return reachTimeMs; }
/** 首达时间，时间戳 */
public void setReachTimeMs(long _reachTimeMs) { reachTimeMs = _reachTimeMs; }


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
	if(_buf.remaining() > 0) bigStepId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) reachTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(bigStepId);
	_buf.putLong(cid);
	_buf.putLong(reachTimeMs);
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

