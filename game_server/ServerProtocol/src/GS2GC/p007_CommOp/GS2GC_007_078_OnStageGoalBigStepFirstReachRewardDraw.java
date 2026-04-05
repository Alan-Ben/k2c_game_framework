package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 阶段目标大阶段首达奖励领取推送
 **/
public class GS2GC_007_078_OnStageGoalBigStepFirstReachRewardDraw implements ALBasicProtocolPack._IALProtocolStructure {
private long bigStep;


public GS2GC_007_078_OnStageGoalBigStepFirstReachRewardDraw() {
	bigStep = (long)0;
}

public GS2GC_007_078_OnStageGoalBigStepFirstReachRewardDraw(
	 long _bigStep
) {	bigStep = _bigStep;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)78; }

public long getBigStep() { return bigStep; }
public void setBigStep(long _bigStep) { bigStep = _bigStep; }


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
	if(_buf.remaining() > 0) bigStep = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(bigStep);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)78);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)78);
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

