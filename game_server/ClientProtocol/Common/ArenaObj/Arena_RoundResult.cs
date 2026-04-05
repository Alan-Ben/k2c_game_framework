using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ArenaObj
{

/// <summary>
/// 竞技场回合结果
/// </summary>
public class Arena_RoundResult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 回合数
/// </summary>
private int round;
/// <summary>
/// 是否击败对手
/// </summary>
private bool isDefeat;
/// <summary>
/// 对手扣除影响力
/// </summary>
private int opponentDeductinfluence;
/// <summary>
/// 获得影响力
/// </summary>
private int gainInfluence;
/// <summary>
/// 获得硬币
/// </summary>
private int gainCoin;


public Arena_RoundResult() {
	round = 0;
	isDefeat = false;
	opponentDeductinfluence = 0;
	gainInfluence = 0;
	gainCoin = 0;
}

public Arena_RoundResult(
	int _round
	, bool _isDefeat
	, int _opponentDeductinfluence
	, int _gainInfluence
	, int _gainCoin
) {	round = _round;
	isDefeat = _isDefeat;
	opponentDeductinfluence = _opponentDeductinfluence;
	gainInfluence = _gainInfluence;
	gainCoin = _gainCoin;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 回合数
/// </summary>
public int getRound() { return round; }
/// <summary>
/// 回合数
/// </summary>
public void setRound(int _round) { round = _round; }
/// <summary>
/// 是否击败对手
/// </summary>
public bool getIsDefeat() { return isDefeat; }
/// <summary>
/// 是否击败对手
/// </summary>
public void setIsDefeat(bool _isDefeat) { isDefeat = _isDefeat; }
/// <summary>
/// 对手扣除影响力
/// </summary>
public int getOpponentDeductinfluence() { return opponentDeductinfluence; }
/// <summary>
/// 对手扣除影响力
/// </summary>
public void setOpponentDeductinfluence(int _opponentDeductinfluence) { opponentDeductinfluence = _opponentDeductinfluence; }
/// <summary>
/// 获得影响力
/// </summary>
public int getGainInfluence() { return gainInfluence; }
/// <summary>
/// 获得影响力
/// </summary>
public void setGainInfluence(int _gainInfluence) { gainInfluence = _gainInfluence; }
/// <summary>
/// 获得硬币
/// </summary>
public int getGainCoin() { return gainCoin; }
/// <summary>
/// 获得硬币
/// </summary>
public void setGainCoin(int _gainCoin) { gainCoin = _gainCoin; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	round = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isDefeat = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	opponentDeductinfluence = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainInfluence = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainCoin = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(round);
	_buf.put(isDefeat?(byte)1:(byte)0);
	_buf.putInt(opponentDeductinfluence);
	_buf.putInt(gainInfluence);
	_buf.putInt(gainCoin);
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
	builder.Append("round").Append(":").Append(round.ToString()).Append(", ");
	builder.Append("isDefeat").Append(":").Append(isDefeat.ToString()).Append(", ");
	builder.Append("opponentDeductinfluence").Append(":").Append(opponentDeductinfluence.ToString()).Append(", ");
	builder.Append("gainInfluence").Append(":").Append(gainInfluence.ToString()).Append(", ");
	builder.Append("gainCoin").Append(":").Append(gainCoin.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

