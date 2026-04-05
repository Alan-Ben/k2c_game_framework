package Common.AnecdoteObj;

import java.nio.ByteBuffer;
/*********
 * 政务事件额外数据_赚速
 **/
public class Anecdote_EventExtraData_Earnings implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否已领取首次奖励 */
private boolean hadDrawFirstReward;
/** 是否已领取最终奖励 */
private boolean hadDrawFinalReward;


public Anecdote_EventExtraData_Earnings() {
	hadDrawFirstReward = false;
	hadDrawFinalReward = false;
}

public Anecdote_EventExtraData_Earnings(
	 boolean _hadDrawFirstReward
	, boolean _hadDrawFinalReward
) {	hadDrawFirstReward = _hadDrawFirstReward;
	hadDrawFinalReward = _hadDrawFinalReward;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 是否已领取首次奖励 */
public boolean getHadDrawFirstReward() { return hadDrawFirstReward; }
/** 是否已领取首次奖励 */
public void setHadDrawFirstReward(boolean _hadDrawFirstReward) { hadDrawFirstReward = _hadDrawFirstReward; }
/** 是否已领取最终奖励 */
public boolean getHadDrawFinalReward() { return hadDrawFinalReward; }
/** 是否已领取最终奖励 */
public void setHadDrawFinalReward(boolean _hadDrawFinalReward) { hadDrawFinalReward = _hadDrawFinalReward; }


public final int GetBufSize() {
	int _size = 2;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 4;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawFirstReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawFinalReward = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(hadDrawFirstReward?(byte)1:(byte)0);
	_buf.put(hadDrawFinalReward?(byte)1:(byte)0);
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

