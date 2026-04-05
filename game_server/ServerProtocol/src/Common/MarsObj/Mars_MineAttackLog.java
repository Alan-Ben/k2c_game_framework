package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-矿挑战日志
 **/
public class Mars_MineAttackLog implements ALBasicProtocolPack._IALProtocolStructure {
/** 挑战是否成功 */
private boolean isSucc;
/** 矿配置ID */
private long mineRefId;
/** 进攻方信息 */
private Common.MarsObj.Mars_ExploreBattlePlayerInfo attacker;
/** 防守方信息 */
private Common.MarsObj.Mars_ExploreBattlePlayerInfo defender;


public Mars_MineAttackLog() {
	isSucc = false;
	mineRefId = (long)0;
	attacker = new Common.MarsObj.Mars_ExploreBattlePlayerInfo();
	defender = new Common.MarsObj.Mars_ExploreBattlePlayerInfo();
}

public Mars_MineAttackLog(
	 boolean _isSucc
	, long _mineRefId
	, Common.MarsObj.Mars_ExploreBattlePlayerInfo _attacker
	, Common.MarsObj.Mars_ExploreBattlePlayerInfo _defender
) {	isSucc = _isSucc;
	mineRefId = _mineRefId;
	attacker = _attacker;
	defender = _defender;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 挑战是否成功 */
public boolean getIsSucc() { return isSucc; }
/** 挑战是否成功 */
public void setIsSucc(boolean _isSucc) { isSucc = _isSucc; }
/** 矿配置ID */
public long getMineRefId() { return mineRefId; }
/** 矿配置ID */
public void setMineRefId(long _mineRefId) { mineRefId = _mineRefId; }
/** 进攻方信息 */
public Common.MarsObj.Mars_ExploreBattlePlayerInfo getAttacker() { return attacker; }
/** 进攻方信息 */
public void setAttacker(Common.MarsObj.Mars_ExploreBattlePlayerInfo _attacker) { attacker = _attacker; }
/** 防守方信息 */
public Common.MarsObj.Mars_ExploreBattlePlayerInfo getDefender() { return defender; }
/** 防守方信息 */
public void setDefender(Common.MarsObj.Mars_ExploreBattlePlayerInfo _defender) { defender = _defender; }


public final int GetBufSize() {
	int _size = 81;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 83;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSucc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _attackerCustLen = _buf.getInt();
	int _attackerCurPos = _buf.position();
	attacker.ReadUnzipBuf(_buf, _attackerCurPos + _attackerCustLen);
	_buf.position(_attackerCurPos + _attackerCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _defenderCustLen = _buf.getInt();
	int _defenderCurPos = _buf.position();
	defender.ReadUnzipBuf(_buf, _defenderCurPos + _defenderCustLen);
	_buf.position(_defenderCurPos + _defenderCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isSucc?(byte)1:(byte)0);
	_buf.putLong(mineRefId);
	_buf.putInt(attacker.GetBufSize());
	attacker.PutUnzipBuf(_buf);
	_buf.putInt(defender.GetBufSize());
	defender.PutUnzipBuf(_buf);
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

