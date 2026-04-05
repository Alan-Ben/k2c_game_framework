using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-矿防守日志
/// </summary>
public class Mars_MineDefenceLog : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 防守是否成功
/// </summary>
private bool isSucc;
/// <summary>
/// 矿配置ID
/// </summary>
private long mineRefId;
/// <summary>
/// 防守方信息
/// </summary>
private Common.MarsObj.Mars_ExploreBattlePlayerInfo defender;
/// <summary>
/// 进攻方信息
/// </summary>
private Common.MarsObj.Mars_ExploreBattlePlayerInfo attacker;


public Mars_MineDefenceLog() {
	isSucc = false;
	mineRefId = (long)0;
	defender = new Common.MarsObj.Mars_ExploreBattlePlayerInfo();
	attacker = new Common.MarsObj.Mars_ExploreBattlePlayerInfo();
}

public Mars_MineDefenceLog(
	bool _isSucc
	, long _mineRefId
	, Common.MarsObj.Mars_ExploreBattlePlayerInfo _defender
	, Common.MarsObj.Mars_ExploreBattlePlayerInfo _attacker
) {	isSucc = _isSucc;
	mineRefId = _mineRefId;
	defender = _defender;
	attacker = _attacker;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 防守是否成功
/// </summary>
public bool getIsSucc() { return isSucc; }
/// <summary>
/// 防守是否成功
/// </summary>
public void setIsSucc(bool _isSucc) { isSucc = _isSucc; }
/// <summary>
/// 矿配置ID
/// </summary>
public long getMineRefId() { return mineRefId; }
/// <summary>
/// 矿配置ID
/// </summary>
public void setMineRefId(long _mineRefId) { mineRefId = _mineRefId; }
/// <summary>
/// 防守方信息
/// </summary>
public Common.MarsObj.Mars_ExploreBattlePlayerInfo getDefender() { return defender; }
/// <summary>
/// 防守方信息
/// </summary>
public void setDefender(Common.MarsObj.Mars_ExploreBattlePlayerInfo _defender) { defender = _defender; }
/// <summary>
/// 进攻方信息
/// </summary>
public Common.MarsObj.Mars_ExploreBattlePlayerInfo getAttacker() { return attacker; }
/// <summary>
/// 进攻方信息
/// </summary>
public void setAttacker(Common.MarsObj.Mars_ExploreBattlePlayerInfo _attacker) { attacker = _attacker; }


public int GetBufSize() {
	int _size = 81;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 83;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isSucc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mineRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _defenderCustLen = _buf.getInt();
	int _defenderCurPos = _buf.getCurPos();
	defender.ReadUnzipBuf(_buf, _defenderCurPos + _defenderCustLen);
	_buf.setPosition(_defenderCurPos + _defenderCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _attackerCustLen = _buf.getInt();
	int _attackerCurPos = _buf.getCurPos();
	attacker.ReadUnzipBuf(_buf, _attackerCurPos + _attackerCustLen);
	_buf.setPosition(_attackerCurPos + _attackerCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isSucc?(byte)1:(byte)0);
	_buf.putLong(mineRefId);
	_buf.putInt(defender.GetBufSize());
	defender.PutUnzipBuf(_buf);
	_buf.putInt(attacker.GetBufSize());
	attacker.PutUnzipBuf(_buf);
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
	builder.Append("isSucc").Append(":").Append(isSucc.ToString()).Append(", ");
	builder.Append("mineRefId").Append(":").Append(mineRefId.ToString()).Append(", ");
	builder.Append("defender").Append(":").Append(defender == null ? "null" : defender.ToString()).Append(", ");
	builder.Append("attacker").Append(":").Append(attacker == null ? "null" : attacker.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

