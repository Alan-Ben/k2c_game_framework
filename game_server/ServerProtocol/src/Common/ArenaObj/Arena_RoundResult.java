package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场回合结果
 **/
public class Arena_RoundResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 回合数 */
private int round;
/** 是否击败对手 */
private boolean isDefeat;
/** 对手扣除影响力 */
private int opponentDeductinfluence;
/** 获得影响力 */
private int gainInfluence;
/** 获得硬币 */
private int gainCoin;


public Arena_RoundResult() {
	round = 0;
	isDefeat = false;
	opponentDeductinfluence = 0;
	gainInfluence = 0;
	gainCoin = 0;
}

public Arena_RoundResult(
	 int _round
	, boolean _isDefeat
	, int _opponentDeductinfluence
	, int _gainInfluence
	, int _gainCoin
) {	round = _round;
	isDefeat = _isDefeat;
	opponentDeductinfluence = _opponentDeductinfluence;
	gainInfluence = _gainInfluence;
	gainCoin = _gainCoin;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 回合数 */
public int getRound() { return round; }
/** 回合数 */
public void setRound(int _round) { round = _round; }
/** 是否击败对手 */
public boolean getIsDefeat() { return isDefeat; }
/** 是否击败对手 */
public void setIsDefeat(boolean _isDefeat) { isDefeat = _isDefeat; }
/** 对手扣除影响力 */
public int getOpponentDeductinfluence() { return opponentDeductinfluence; }
/** 对手扣除影响力 */
public void setOpponentDeductinfluence(int _opponentDeductinfluence) { opponentDeductinfluence = _opponentDeductinfluence; }
/** 获得影响力 */
public int getGainInfluence() { return gainInfluence; }
/** 获得影响力 */
public void setGainInfluence(int _gainInfluence) { gainInfluence = _gainInfluence; }
/** 获得硬币 */
public int getGainCoin() { return gainCoin; }
/** 获得硬币 */
public void setGainCoin(int _gainCoin) { gainCoin = _gainCoin; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) round = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isDefeat = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) opponentDeductinfluence = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainInfluence = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainCoin = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(round);
	_buf.put(isDefeat?(byte)1:(byte)0);
	_buf.putInt(opponentDeductinfluence);
	_buf.putInt(gainInfluence);
	_buf.putInt(gainCoin);
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

