package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 领取阶段目标首达奖励
 **/
public class GC2GS_007_015_ReqDrawStageGoalFirstReachReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 大阶段ID */
private long bigStepId;


public GC2GS_007_015_ReqDrawStageGoalFirstReachReward() {
	bigStepId = (long)0;
}

public GC2GS_007_015_ReqDrawStageGoalFirstReachReward(
	 long _bigStepId
) {	bigStepId = _bigStepId;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)15; }

/** 大阶段ID */
public long getBigStepId() { return bigStepId; }
/** 大阶段ID */
public void setBigStepId(long _bigStepId) { bigStepId = _bigStepId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bigStepId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(bigStepId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)15);
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

