package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 晚间副本_攻击日志
 **/
public class EveningDungeon_AttackLog implements ALBasicProtocolPack._IALProtocolStructure {
/** 序列号 */
private long serial;
/** 玩家ID */
private long cid;
/** 波数 */
private int wave;
/** 造成伤害 */
private long harmHp;
/** 玩家名 */
private String playerName;
/** 头像id */
private long iconId;
/** 时间戳 */
private long timestamp;
/** 攻击英雄ID */
private long attackHeroId;


public EveningDungeon_AttackLog() {
	serial = (long)0;
	cid = (long)0;
	wave = 0;
	harmHp = (long)0;
	playerName = "";
	iconId = (long)0;
	timestamp = (long)0;
	attackHeroId = (long)0;
}

public EveningDungeon_AttackLog(
	 long _serial
	, long _cid
	, int _wave
	, long _harmHp
	, String _playerName
	, long _iconId
	, long _timestamp
	, long _attackHeroId
) {	serial = _serial;
	cid = _cid;
	wave = _wave;
	harmHp = _harmHp;
	playerName = _playerName;
	iconId = _iconId;
	timestamp = _timestamp;
	attackHeroId = _attackHeroId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 序列号 */
public long getSerial() { return serial; }
/** 序列号 */
public void setSerial(long _serial) { serial = _serial; }
/** 玩家ID */
public long getCid() { return cid; }
/** 玩家ID */
public void setCid(long _cid) { cid = _cid; }
/** 波数 */
public int getWave() { return wave; }
/** 波数 */
public void setWave(int _wave) { wave = _wave; }
/** 造成伤害 */
public long getHarmHp() { return harmHp; }
/** 造成伤害 */
public void setHarmHp(long _harmHp) { harmHp = _harmHp; }
/** 玩家名 */
public String getPlayerName() { return playerName; }
/** 玩家名 */
public void setPlayerName(String _playerName) { playerName = _playerName; }
/** 头像id */
public long getIconId() { return iconId; }
/** 头像id */
public void setIconId(long _iconId) { iconId = _iconId; }
/** 时间戳 */
public long getTimestamp() { return timestamp; }
/** 时间戳 */
public void setTimestamp(long _timestamp) { timestamp = _timestamp; }
/** 攻击英雄ID */
public long getAttackHeroId() { return attackHeroId; }
/** 攻击英雄ID */
public void setAttackHeroId(long _attackHeroId) { attackHeroId = _attackHeroId; }


public final int GetBufSize() {
	int _size = 52;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 54;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) wave = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) harmHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) iconId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timestamp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attackHeroId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(serial);
	_buf.putLong(cid);
	_buf.putInt(wave);
	_buf.putLong(harmHp);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playerName);
	_buf.putLong(iconId);
	_buf.putLong(timestamp);
	_buf.putLong(attackHeroId);
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

