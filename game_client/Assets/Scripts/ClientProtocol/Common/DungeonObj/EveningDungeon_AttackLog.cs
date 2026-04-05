using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DungeonObj
{

/// <summary>
/// 晚间副本_攻击日志
/// </summary>
public class EveningDungeon_AttackLog : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 序列号
/// </summary>
private long serial;
/// <summary>
/// 玩家ID
/// </summary>
private long cid;
/// <summary>
/// 波数
/// </summary>
private int wave;
/// <summary>
/// 造成伤害
/// </summary>
private long harmHp;
/// <summary>
/// 玩家名
/// </summary>
private string playerName;
/// <summary>
/// 头像id
/// </summary>
private long iconId;
/// <summary>
/// 时间戳
/// </summary>
private long timestamp;
/// <summary>
/// 攻击英雄ID
/// </summary>
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
	, string _playerName
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 序列号
/// </summary>
public long getSerial() { return serial; }
/// <summary>
/// 序列号
/// </summary>
public void setSerial(long _serial) { serial = _serial; }
/// <summary>
/// 玩家ID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家ID
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 波数
/// </summary>
public int getWave() { return wave; }
/// <summary>
/// 波数
/// </summary>
public void setWave(int _wave) { wave = _wave; }
/// <summary>
/// 造成伤害
/// </summary>
public long getHarmHp() { return harmHp; }
/// <summary>
/// 造成伤害
/// </summary>
public void setHarmHp(long _harmHp) { harmHp = _harmHp; }
/// <summary>
/// 玩家名
/// </summary>
public string getPlayerName() { return playerName; }
/// <summary>
/// 玩家名
/// </summary>
public void setPlayerName(string _playerName) { playerName = _playerName; }
/// <summary>
/// 头像id
/// </summary>
public long getIconId() { return iconId; }
/// <summary>
/// 头像id
/// </summary>
public void setIconId(long _iconId) { iconId = _iconId; }
/// <summary>
/// 时间戳
/// </summary>
public long getTimestamp() { return timestamp; }
/// <summary>
/// 时间戳
/// </summary>
public void setTimestamp(long _timestamp) { timestamp = _timestamp; }
/// <summary>
/// 攻击英雄ID
/// </summary>
public long getAttackHeroId() { return attackHeroId; }
/// <summary>
/// 攻击英雄ID
/// </summary>
public void setAttackHeroId(long _attackHeroId) { attackHeroId = _attackHeroId; }


public int GetBufSize() {
	int _size = 52;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 54;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	wave = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	harmHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	playerName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	iconId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timestamp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attackHeroId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(serial);
	_buf.putLong(cid);
	_buf.putInt(wave);
	_buf.putLong(harmHp);
	_buf.putString(playerName);
	_buf.putLong(iconId);
	_buf.putLong(timestamp);
	_buf.putLong(attackHeroId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("serial").Append(":").Append(serial.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("wave").Append(":").Append(wave.ToString()).Append(", ");
	builder.Append("harmHp").Append(":").Append(harmHp.ToString()).Append(", ");
	builder.Append("playerName").Append(":").Append(playerName.ToString()).Append(", ");
	builder.Append("iconId").Append(":").Append(iconId.ToString()).Append(", ");
	builder.Append("timestamp").Append(":").Append(timestamp.ToString()).Append(", ");
	builder.Append("attackHeroId").Append(":").Append(attackHeroId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

