package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-boss挑战事件日志
 **/
public class Mars_BossBattleEventLog implements ALBasicProtocolPack._IALProtocolStructure {
/** 挑战是否成功 */
private boolean isSucc;
/** 配置ID */
private long eventRefId;
/** 进攻方信息 */
private Common.MarsObj.Mars_ExploreBattlePlayerInfo attacker;
/** NPC信息 */
private Common.MarsObj.Mars_ExploreBattleNPCInfo npc;


public Mars_BossBattleEventLog() {
	isSucc = false;
	eventRefId = (long)0;
	attacker = new Common.MarsObj.Mars_ExploreBattlePlayerInfo();
	npc = new Common.MarsObj.Mars_ExploreBattleNPCInfo();
}

public Mars_BossBattleEventLog(
	 boolean _isSucc
	, long _eventRefId
	, Common.MarsObj.Mars_ExploreBattlePlayerInfo _attacker
	, Common.MarsObj.Mars_ExploreBattleNPCInfo _npc
) {	isSucc = _isSucc;
	eventRefId = _eventRefId;
	attacker = _attacker;
	npc = _npc;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 挑战是否成功 */
public boolean getIsSucc() { return isSucc; }
/** 挑战是否成功 */
public void setIsSucc(boolean _isSucc) { isSucc = _isSucc; }
/** 配置ID */
public long getEventRefId() { return eventRefId; }
/** 配置ID */
public void setEventRefId(long _eventRefId) { eventRefId = _eventRefId; }
/** 进攻方信息 */
public Common.MarsObj.Mars_ExploreBattlePlayerInfo getAttacker() { return attacker; }
/** 进攻方信息 */
public void setAttacker(Common.MarsObj.Mars_ExploreBattlePlayerInfo _attacker) { attacker = _attacker; }
/** NPC信息 */
public Common.MarsObj.Mars_ExploreBattleNPCInfo getNpc() { return npc; }
/** NPC信息 */
public void setNpc(Common.MarsObj.Mars_ExploreBattleNPCInfo _npc) { npc = _npc; }


public final int GetBufSize() {
	int _size = 73;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 75;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSucc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) eventRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _attackerCustLen = _buf.getInt();
	int _attackerCurPos = _buf.position();
	attacker.ReadUnzipBuf(_buf, _attackerCurPos + _attackerCustLen);
	_buf.position(_attackerCurPos + _attackerCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _npcCustLen = _buf.getInt();
	int _npcCurPos = _buf.position();
	npc.ReadUnzipBuf(_buf, _npcCurPos + _npcCustLen);
	_buf.position(_npcCurPos + _npcCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isSucc?(byte)1:(byte)0);
	_buf.putLong(eventRefId);
	_buf.putInt(attacker.GetBufSize());
	attacker.PutUnzipBuf(_buf);
	_buf.putInt(npc.GetBufSize());
	npc.PutUnzipBuf(_buf);
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

