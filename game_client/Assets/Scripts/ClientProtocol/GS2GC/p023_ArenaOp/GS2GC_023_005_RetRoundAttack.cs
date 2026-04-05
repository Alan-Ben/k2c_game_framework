using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p023_ArenaOp
{

public class GS2GC_023_005_RetRoundAttack : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 回合结果
/// </summary>
private Common.ArenaObj.Arena_RoundResult roundResult;
/// <summary>
/// 回合奖励
/// </summary>
private Common.ArenaObj.Arena_RoundReward roundReward;
/// <summary>
/// 战斗结果
/// </summary>
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

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 回合结果
/// </summary>
public Common.ArenaObj.Arena_RoundResult getRoundResult() { return roundResult; }
/// <summary>
/// 回合结果
/// </summary>
public void setRoundResult(Common.ArenaObj.Arena_RoundResult _roundResult) { roundResult = _roundResult; }
/// <summary>
/// 回合奖励
/// </summary>
public Common.ArenaObj.Arena_RoundReward getRoundReward() { return roundReward; }
/// <summary>
/// 回合奖励
/// </summary>
public void setRoundReward(Common.ArenaObj.Arena_RoundReward _roundReward) { roundReward = _roundReward; }
/// <summary>
/// 战斗结果
/// </summary>
public Common.ArenaObj.Arena_BattleResult getBattleResult() { return battleResult; }
/// <summary>
/// 战斗结果
/// </summary>
public void setBattleResult(Common.ArenaObj.Arena_BattleResult _battleResult) { battleResult = _battleResult; }


public int GetBufSize() {
	int _size = 21;
	_size += 4 + roundReward.GetBufSize();
	_size += 4 + battleResult.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 23;
	_size += 4 + roundReward.GetBufSize();
	_size += 4 + battleResult.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _roundResultCustLen = _buf.getInt();
	int _roundResultCurPos = _buf.getCurPos();
	roundResult.ReadUnzipBuf(_buf, _roundResultCurPos + _roundResultCustLen);
	_buf.setPosition(_roundResultCurPos + _roundResultCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _roundRewardCustLen = _buf.getInt();
	int _roundRewardCurPos = _buf.getCurPos();
	roundReward.ReadUnzipBuf(_buf, _roundRewardCurPos + _roundRewardCustLen);
	_buf.setPosition(_roundRewardCurPos + _roundRewardCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _battleResultCustLen = _buf.getInt();
	int _battleResultCurPos = _buf.getCurPos();
	battleResult.ReadUnzipBuf(_buf, _battleResultCurPos + _battleResultCustLen);
	_buf.setPosition(_battleResultCurPos + _battleResultCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(roundResult.GetBufSize());
	roundResult.PutUnzipBuf(_buf);
	_buf.putInt(roundReward.GetBufSize());
	roundReward.PutUnzipBuf(_buf);
	_buf.putInt(battleResult.GetBufSize());
	battleResult.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)5);
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
	builder.Append("roundResult").Append(":").Append(roundResult == null ? "null" : roundResult.ToString()).Append(", ");
	builder.Append("roundReward").Append(":").Append(roundReward == null ? "null" : roundReward.ToString()).Append(", ");
	builder.Append("battleResult").Append(":").Append(battleResult == null ? "null" : battleResult.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

