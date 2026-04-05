package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_005_RetRoundAttack implements ALBasicProtocolPack._IALProtocolStructure {
/** 回合结果 */
private Common.ArenaObj.Arena_RoundResult roundResult;
/** 回合奖励 */
private Common.ArenaObj.Arena_RoundReward roundReward;
/** 战斗结果 */
private Common.ArenaObj.Arena_BattleResult battleResult;


public GS2GC_023_005_RetRoundAttack() {
	roundResult = new Common.ArenaObj.Arena_RoundResult();
	roundReward = new Common.ArenaObj.Arena_RoundReward();
	battleResult = new Common.ArenaObj.Arena_BattleResult();
}

public GS2GC_023_005_RetRoundAttack(
	 Common.ArenaObj.Arena_RoundResult _roundResult
	, Common.ArenaObj.Arena_RoundReward _roundReward
	, Common.ArenaObj.Arena_BattleResult _battleResult
) {	roundResult = _roundResult;
	roundReward = _roundReward;
	battleResult = _battleResult;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)5; }

/** 回合结果 */
public Common.ArenaObj.Arena_RoundResult getRoundResult() { return roundResult; }
/** 回合结果 */
public void setRoundResult(Common.ArenaObj.Arena_RoundResult _roundResult) { roundResult = _roundResult; }
/** 回合奖励 */
public Common.ArenaObj.Arena_RoundReward getRoundReward() { return roundReward; }
/** 回合奖励 */
public void setRoundReward(Common.ArenaObj.Arena_RoundReward _roundReward) { roundReward = _roundReward; }
/** 战斗结果 */
public Common.ArenaObj.Arena_BattleResult getBattleResult() { return battleResult; }
/** 战斗结果 */
public void setBattleResult(Common.ArenaObj.Arena_BattleResult _battleResult) { battleResult = _battleResult; }


public final int GetBufSize() {
	int _size = 21;
	_size += 4 + roundReward.GetBufSize();
	_size += 4 + battleResult.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;
	_size += 4 + roundReward.GetBufSize();
	_size += 4 + battleResult.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _roundResultCustLen = _buf.getInt();
	int _roundResultCurPos = _buf.position();
	roundResult.ReadUnzipBuf(_buf, _roundResultCurPos + _roundResultCustLen);
	_buf.position(_roundResultCurPos + _roundResultCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _roundRewardCustLen = _buf.getInt();
	int _roundRewardCurPos = _buf.position();
	roundReward.ReadUnzipBuf(_buf, _roundRewardCurPos + _roundRewardCustLen);
	_buf.position(_roundRewardCurPos + _roundRewardCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _battleResultCustLen = _buf.getInt();
	int _battleResultCurPos = _buf.position();
	battleResult.ReadUnzipBuf(_buf, _battleResultCurPos + _battleResultCustLen);
	_buf.position(_battleResultCurPos + _battleResultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(roundResult.GetBufSize());
	roundResult.PutUnzipBuf(_buf);
	_buf.putInt(roundReward.GetBufSize());
	roundReward.PutUnzipBuf(_buf);
	_buf.putInt(battleResult.GetBufSize());
	battleResult.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)5);
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

