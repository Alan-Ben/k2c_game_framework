using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-挑战事件日志
/// </summary>
public class Mars_BattleEventLog : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 挑战是否成功
/// </summary>
private bool isSucc;
/// <summary>
/// 配置ID
/// </summary>
private long eventRefId;
/// <summary>
/// 进攻方信息
/// </summary>
private Common.MarsObj.Mars_ExploreBattlePlayerInfo attacker;
/// <summary>
/// NPC信息
/// </summary>
private Common.MarsObj.Mars_ExploreBattleNPCInfo npc;


public Mars_BattleEventLog() {
	isSucc = false;
	eventRefId = (long)0;
	attacker = new Common.MarsObj.Mars_ExploreBattlePlayerInfo();
	npc = new Common.MarsObj.Mars_ExploreBattleNPCInfo();
}

public Mars_BattleEventLog(
	bool _isSucc
	, long _eventRefId
	, Common.MarsObj.Mars_ExploreBattlePlayerInfo _attacker
	, Common.MarsObj.Mars_ExploreBattleNPCInfo _npc
) {	isSucc = _isSucc;
	eventRefId = _eventRefId;
	attacker = _attacker;
	npc = _npc;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 挑战是否成功
/// </summary>
public bool getIsSucc() { return isSucc; }
/// <summary>
/// 挑战是否成功
/// </summary>
public void setIsSucc(bool _isSucc) { isSucc = _isSucc; }
/// <summary>
/// 配置ID
/// </summary>
public long getEventRefId() { return eventRefId; }
/// <summary>
/// 配置ID
/// </summary>
public void setEventRefId(long _eventRefId) { eventRefId = _eventRefId; }
/// <summary>
/// 进攻方信息
/// </summary>
public Common.MarsObj.Mars_ExploreBattlePlayerInfo getAttacker() { return attacker; }
/// <summary>
/// 进攻方信息
/// </summary>
public void setAttacker(Common.MarsObj.Mars_ExploreBattlePlayerInfo _attacker) { attacker = _attacker; }
/// <summary>
/// NPC信息
/// </summary>
public Common.MarsObj.Mars_ExploreBattleNPCInfo getNpc() { return npc; }
/// <summary>
/// NPC信息
/// </summary>
public void setNpc(Common.MarsObj.Mars_ExploreBattleNPCInfo _npc) { npc = _npc; }


public int GetBufSize() {
	int _size = 73;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 75;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isSucc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _attackerCustLen = _buf.getInt();
	int _attackerCurPos = _buf.getCurPos();
	attacker.ReadUnzipBuf(_buf, _attackerCurPos + _attackerCustLen);
	_buf.setPosition(_attackerCurPos + _attackerCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _npcCustLen = _buf.getInt();
	int _npcCurPos = _buf.getCurPos();
	npc.ReadUnzipBuf(_buf, _npcCurPos + _npcCustLen);
	_buf.setPosition(_npcCurPos + _npcCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isSucc?(byte)1:(byte)0);
	_buf.putLong(eventRefId);
	_buf.putInt(attacker.GetBufSize());
	attacker.PutUnzipBuf(_buf);
	_buf.putInt(npc.GetBufSize());
	npc.PutUnzipBuf(_buf);
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
	builder.Append("eventRefId").Append(":").Append(eventRefId.ToString()).Append(", ");
	builder.Append("attacker").Append(":").Append(attacker == null ? "null" : attacker.ToString()).Append(", ");
	builder.Append("npc").Append(":").Append(npc == null ? "null" : npc.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

