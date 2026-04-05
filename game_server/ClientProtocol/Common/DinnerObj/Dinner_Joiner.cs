using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DinnerObj
{

/// <summary>
/// 赴宴玩家数据
/// </summary>
public class Dinner_Joiner : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 赴宴对象类型
/// </summary>
private Common.DinnerEnum.EDinnerJoinerType joinerType;
/// <summary>
/// 赴宴对象ID
/// </summary>
private long joinerId;
/// <summary>
/// 赴宴花费配置ID
/// </summary>
private long costId;
/// <summary>
/// 玩家宴会人气
/// </summary>
private long score;
/// <summary>
/// 赴宴时间戳（毫秒）
/// </summary>
private long joinTimeMs;


public Dinner_Joiner() {
	joinerType = 0;
	joinerId = (long)0;
	costId = (long)0;
	score = (long)0;
	joinTimeMs = (long)0;
}

public Dinner_Joiner(
	Common.DinnerEnum.EDinnerJoinerType _joinerType
	, long _joinerId
	, long _costId
	, long _score
	, long _joinTimeMs
) {	joinerType = _joinerType;
	joinerId = _joinerId;
	costId = _costId;
	score = _score;
	joinTimeMs = _joinTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 赴宴对象类型
/// </summary>
public Common.DinnerEnum.EDinnerJoinerType getJoinerType() { return joinerType; }
/// <summary>
/// 赴宴对象类型
/// </summary>
public void setJoinerType(Common.DinnerEnum.EDinnerJoinerType _joinerType) { joinerType = _joinerType; }
/// <summary>
/// 赴宴对象ID
/// </summary>
public long getJoinerId() { return joinerId; }
/// <summary>
/// 赴宴对象ID
/// </summary>
public void setJoinerId(long _joinerId) { joinerId = _joinerId; }
/// <summary>
/// 赴宴花费配置ID
/// </summary>
public long getCostId() { return costId; }
/// <summary>
/// 赴宴花费配置ID
/// </summary>
public void setCostId(long _costId) { costId = _costId; }
/// <summary>
/// 玩家宴会人气
/// </summary>
public long getScore() { return score; }
/// <summary>
/// 玩家宴会人气
/// </summary>
public void setScore(long _score) { score = _score; }
/// <summary>
/// 赴宴时间戳（毫秒）
/// </summary>
public long getJoinTimeMs() { return joinTimeMs; }
/// <summary>
/// 赴宴时间戳（毫秒）
/// </summary>
public void setJoinTimeMs(long _joinTimeMs) { joinTimeMs = _joinTimeMs; }


public int GetBufSize() {
	int _size = 36;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinerType = (Common.DinnerEnum.EDinnerJoinerType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	costId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)joinerType);

	_buf.putLong(joinerId);
	_buf.putLong(costId);
	_buf.putLong(score);
	_buf.putLong(joinTimeMs);
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
	builder.Append("joinerType").Append(":").Append(joinerType.ToString()).Append(", ");
	builder.Append("joinerId").Append(":").Append(joinerId.ToString()).Append(", ");
	builder.Append("costId").Append(":").Append(costId.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("joinTimeMs").Append(":").Append(joinTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

