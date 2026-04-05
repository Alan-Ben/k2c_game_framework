package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 晚间副本_Boss信息
 **/
public class EveningDungeon_BossInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 波数 */
private int wave;
/** 扣除血量 */
private long deductedHp;
/** 总血量 */
private long totalHp;
/** 被击败时间戳 */
private long beDefeatTimeMs;
/** 击杀玩家ID */
private long defeatCid;


public EveningDungeon_BossInfo() {
	wave = 0;
	deductedHp = (long)0;
	totalHp = (long)0;
	beDefeatTimeMs = (long)0;
	defeatCid = (long)0;
}

public EveningDungeon_BossInfo(
	 int _wave
	, long _deductedHp
	, long _totalHp
	, long _beDefeatTimeMs
	, long _defeatCid
) {	wave = _wave;
	deductedHp = _deductedHp;
	totalHp = _totalHp;
	beDefeatTimeMs = _beDefeatTimeMs;
	defeatCid = _defeatCid;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 波数 */
public int getWave() { return wave; }
/** 波数 */
public void setWave(int _wave) { wave = _wave; }
/** 扣除血量 */
public long getDeductedHp() { return deductedHp; }
/** 扣除血量 */
public void setDeductedHp(long _deductedHp) { deductedHp = _deductedHp; }
/** 总血量 */
public long getTotalHp() { return totalHp; }
/** 总血量 */
public void setTotalHp(long _totalHp) { totalHp = _totalHp; }
/** 被击败时间戳 */
public long getBeDefeatTimeMs() { return beDefeatTimeMs; }
/** 被击败时间戳 */
public void setBeDefeatTimeMs(long _beDefeatTimeMs) { beDefeatTimeMs = _beDefeatTimeMs; }
/** 击杀玩家ID */
public long getDefeatCid() { return defeatCid; }
/** 击杀玩家ID */
public void setDefeatCid(long _defeatCid) { defeatCid = _defeatCid; }


public final int GetBufSize() {
	int _size = 36;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) wave = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) deductedHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) beDefeatTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) defeatCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(wave);
	_buf.putLong(deductedHp);
	_buf.putLong(totalHp);
	_buf.putLong(beDefeatTimeMs);
	_buf.putLong(defeatCid);
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

