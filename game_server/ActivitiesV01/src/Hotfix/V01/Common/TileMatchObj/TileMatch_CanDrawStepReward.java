package Hotfix.V01.Common.TileMatchObj;

import java.nio.ByteBuffer;
/*********
 * 三消-可领取阶段奖励
 **/
public class TileMatch_CanDrawStepReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 阶段 */
private int step;
/** 奖励数量 */
private int num;


public TileMatch_CanDrawStepReward() {
	step = 0;
	num = 0;
}

public TileMatch_CanDrawStepReward(
	 int _step
	, int _num
) {	step = _step;
	num = _num;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 阶段 */
public int getStep() { return step; }
/** 阶段 */
public void setStep(int _step) { step = _step; }
/** 奖励数量 */
public int getNum() { return num; }
/** 奖励数量 */
public void setNum(int _num) { num = _num; }


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
	if(_buf.remaining() > 0) step = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(step);
	_buf.putInt(num);
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

