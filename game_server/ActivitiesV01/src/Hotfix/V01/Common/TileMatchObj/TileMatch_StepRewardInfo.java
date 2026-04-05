package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-阶段奖励信息
 **/
public class TileMatch_StepRewardInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 阶段 */
private int step;
/** 分数 */
private long curScore;
/** 序列号 */
private int serialId;


public TileMatch_StepRewardInfo() {
	step = 0;
	curScore = (long)0;
	serialId = 0;
}

public TileMatch_StepRewardInfo(
	 int _step
	, long _curScore
	, int _serialId
) {	step = _step;
	curScore = _curScore;
	serialId = _serialId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 阶段 */
public int getStep() { return step; }
/** 阶段 */
public void setStep(int _step) { step = _step; }
/** 分数 */
public long getCurScore() { return curScore; }
/** 分数 */
public void setCurScore(long _curScore) { curScore = _curScore; }
/** 序列号 */
public int getSerialId() { return serialId; }
/** 序列号 */
public void setSerialId(int _serialId) { serialId = _serialId; }


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
	if(_buf.remaining() > 0) step = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) curScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serialId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(step);
	_buf.putLong(curScore);
	_buf.putInt(serialId);
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

