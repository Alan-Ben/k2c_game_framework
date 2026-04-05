using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DinnerObj
{

/// <summary>
/// 开宴记录-索引记录
/// </summary>
public class Dinner_StartLogIdx : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宴会实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 宴会配置ID
/// </summary>
private long dinnerId;
/// <summary>
/// 获得的宴会人气
/// </summary>
private long gainScore;
/// <summary>
/// 开始时间戳（秒）
/// </summary>
private int startTs;
/// <summary>
/// 赴宴玩家数量
/// </summary>
private int joinerCount;
/// <summary>
/// 凭证类型
/// </summary>
private Common.DinnerEnum.EDinnerPermitType permitType;
/// <summary>
/// 凭证类型ID
/// </summary>
private long permitTypeId;


public Dinner_StartLogIdx() {
	instanceId = (long)0;
	dinnerId = (long)0;
	gainScore = (long)0;
	startTs = 0;
	joinerCount = 0;
	permitType = 0;
	permitTypeId = (long)0;
}

public Dinner_StartLogIdx(
	long _instanceId
	, long _dinnerId
	, long _gainScore
	, int _startTs
	, int _joinerCount
	, Common.DinnerEnum.EDinnerPermitType _permitType
	, long _permitTypeId
) {	instanceId = _instanceId;
	dinnerId = _dinnerId;
	gainScore = _gainScore;
	startTs = _startTs;
	joinerCount = _joinerCount;
	permitType = _permitType;
	permitTypeId = _permitTypeId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 宴会实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 宴会实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 宴会配置ID
/// </summary>
public long getDinnerId() { return dinnerId; }
/// <summary>
/// 宴会配置ID
/// </summary>
public void setDinnerId(long _dinnerId) { dinnerId = _dinnerId; }
/// <summary>
/// 获得的宴会人气
/// </summary>
public long getGainScore() { return gainScore; }
/// <summary>
/// 获得的宴会人气
/// </summary>
public void setGainScore(long _gainScore) { gainScore = _gainScore; }
/// <summary>
/// 开始时间戳（秒）
/// </summary>
public int getStartTs() { return startTs; }
/// <summary>
/// 开始时间戳（秒）
/// </summary>
public void setStartTs(int _startTs) { startTs = _startTs; }
/// <summary>
/// 赴宴玩家数量
/// </summary>
public int getJoinerCount() { return joinerCount; }
/// <summary>
/// 赴宴玩家数量
/// </summary>
public void setJoinerCount(int _joinerCount) { joinerCount = _joinerCount; }
/// <summary>
/// 凭证类型
/// </summary>
public Common.DinnerEnum.EDinnerPermitType getPermitType() { return permitType; }
/// <summary>
/// 凭证类型
/// </summary>
public void setPermitType(Common.DinnerEnum.EDinnerPermitType _permitType) { permitType = _permitType; }
/// <summary>
/// 凭证类型ID
/// </summary>
public long getPermitTypeId() { return permitTypeId; }
/// <summary>
/// 凭证类型ID
/// </summary>
public void setPermitTypeId(long _permitTypeId) { permitTypeId = _permitTypeId; }


public int GetBufSize() {
	int _size = 44;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 46;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dinnerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinerCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	permitType = (Common.DinnerEnum.EDinnerPermitType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	permitTypeId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(dinnerId);
	_buf.putLong(gainScore);
	_buf.putInt(startTs);
	_buf.putInt(joinerCount);
	_buf.putInt((int)permitType);

	_buf.putLong(permitTypeId);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("dinnerId").Append(":").Append(dinnerId.ToString()).Append(", ");
	builder.Append("gainScore").Append(":").Append(gainScore.ToString()).Append(", ");
	builder.Append("startTs").Append(":").Append(startTs.ToString()).Append(", ");
	builder.Append("joinerCount").Append(":").Append(joinerCount.ToString()).Append(", ");
	builder.Append("permitType").Append(":").Append(permitType.ToString()).Append(", ");
	builder.Append("permitTypeId").Append(":").Append(permitTypeId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

