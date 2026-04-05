package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 领取阶段目标大阶段奖励
 **/
public class GC2GS_007_019_ReqTakeStageGoalBigStepReward implements ALBasicProtocolPack._IALProtocolStructure {
private long bigStepId;


public GC2GS_007_019_ReqTakeStageGoalBigStepReward() {
	bigStepId = (long)0;
}

public GC2GS_007_019_ReqTakeStageGoalBigStepReward(
	 long _bigStepId
) {	bigStepId = _bigStepId;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)19; }

public long getBigStepId() { return bigStepId; }
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
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)19);
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

