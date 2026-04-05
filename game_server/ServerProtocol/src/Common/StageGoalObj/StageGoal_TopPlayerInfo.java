package Common.StageGoalObj;

import java.nio.ByteBuffer;
/*********
 * 阶段目标Top玩家数据
 **/
public class StageGoal_TopPlayerInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 首达角色ID */
private long cid;
/** 小阶段ID */
private long stepId;


public StageGoal_TopPlayerInfo() {
	cid = (long)0;
	stepId = (long)0;
}

public StageGoal_TopPlayerInfo(
	 long _cid
	, long _stepId
) {	cid = _cid;
	stepId = _stepId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 首达角色ID */
public long getCid() { return cid; }
/** 首达角色ID */
public void setCid(long _cid) { cid = _cid; }
/** 小阶段ID */
public long getStepId() { return stepId; }
/** 小阶段ID */
public void setStepId(long _stepId) { stepId = _stepId; }


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
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stepId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(stepId);
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

