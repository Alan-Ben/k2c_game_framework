package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场名人榜数据
 **/
public class Arena_CelebrityRankInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 攻击者CID */
private long attackerCid;
/** 攻击者名字 */
private String attackerName;
/** 防守者名字 */
private String defenderName;
/** 击败大臣数量 */
private int defeatHeroNum;
/** 是否指定攻击 */
private boolean isSelectAttack;
/** 发生时间戳 */
private long timeMs;


public Arena_CelebrityRankInfo() {
	dbId = (long)0;
	attackerCid = (long)0;
	attackerName = "";
	defenderName = "";
	defeatHeroNum = 0;
	isSelectAttack = false;
	timeMs = (long)0;
}

public Arena_CelebrityRankInfo(
	 long _dbId
	, long _attackerCid
	, String _attackerName
	, String _defenderName
	, int _defeatHeroNum
	, boolean _isSelectAttack
	, long _timeMs
) {	dbId = _dbId;
	attackerCid = _attackerCid;
	attackerName = _attackerName;
	defenderName = _defenderName;
	defeatHeroNum = _defeatHeroNum;
	isSelectAttack = _isSelectAttack;
	timeMs = _timeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 攻击者CID */
public long getAttackerCid() { return attackerCid; }
/** 攻击者CID */
public void setAttackerCid(long _attackerCid) { attackerCid = _attackerCid; }
/** 攻击者名字 */
public String getAttackerName() { return attackerName; }
/** 攻击者名字 */
public void setAttackerName(String _attackerName) { attackerName = _attackerName; }
/** 防守者名字 */
public String getDefenderName() { return defenderName; }
/** 防守者名字 */
public void setDefenderName(String _defenderName) { defenderName = _defenderName; }
/** 击败大臣数量 */
public int getDefeatHeroNum() { return defeatHeroNum; }
/** 击败大臣数量 */
public void setDefeatHeroNum(int _defeatHeroNum) { defeatHeroNum = _defeatHeroNum; }
/** 是否指定攻击 */
public boolean getIsSelectAttack() { return isSelectAttack; }
/** 是否指定攻击 */
public void setIsSelectAttack(boolean _isSelectAttack) { isSelectAttack = _isSelectAttack; }
/** 发生时间戳 */
public long getTimeMs() { return timeMs; }
/** 发生时间戳 */
public void setTimeMs(long _timeMs) { timeMs = _timeMs; }


public final int GetBufSize() {
	int _size = 29;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(defenderName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 31;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(defenderName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attackerCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attackerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) defenderName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) defeatHeroNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSelectAttack = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putLong(attackerCid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, attackerName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, defenderName);
	_buf.putInt(defeatHeroNum);
	_buf.put(isSelectAttack?(byte)1:(byte)0);
	_buf.putLong(timeMs);
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

