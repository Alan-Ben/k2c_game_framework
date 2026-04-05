package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场战报展示
 **/
public class Arena_BattleReportShow implements ALBasicProtocolPack._IALProtocolStructure {
/** 对手CID */
private long opponentCid;
/** 击败我方大臣数量 */
private int defeatHeroNum;
/** 扣除影响力 */
private int deductinfluence;
/** 时间戳 */
private long timestamp;


public Arena_BattleReportShow() {
	opponentCid = (long)0;
	defeatHeroNum = 0;
	deductinfluence = 0;
	timestamp = (long)0;
}

public Arena_BattleReportShow(
	 long _opponentCid
	, int _defeatHeroNum
	, int _deductinfluence
	, long _timestamp
) {	opponentCid = _opponentCid;
	defeatHeroNum = _defeatHeroNum;
	deductinfluence = _deductinfluence;
	timestamp = _timestamp;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 对手CID */
public long getOpponentCid() { return opponentCid; }
/** 对手CID */
public void setOpponentCid(long _opponentCid) { opponentCid = _opponentCid; }
/** 击败我方大臣数量 */
public int getDefeatHeroNum() { return defeatHeroNum; }
/** 击败我方大臣数量 */
public void setDefeatHeroNum(int _defeatHeroNum) { defeatHeroNum = _defeatHeroNum; }
/** 扣除影响力 */
public int getDeductinfluence() { return deductinfluence; }
/** 扣除影响力 */
public void setDeductinfluence(int _deductinfluence) { deductinfluence = _deductinfluence; }
/** 时间戳 */
public long getTimestamp() { return timestamp; }
/** 时间戳 */
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
	if(_buf.remaining() > 0) opponentCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) defeatHeroNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) deductinfluence = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timestamp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(opponentCid);
	_buf.putInt(defeatHeroNum);
	_buf.putInt(deductinfluence);
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

