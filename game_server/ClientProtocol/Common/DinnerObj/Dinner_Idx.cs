using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DinnerObj
{

/// <summary>
/// 宴会索引信息
/// </summary>
public class Dinner_Idx : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宴会实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 宴会配置ID
/// </summary>
private long dinnerId;
/// <summary>
/// 开宴玩家CID
/// </summary>
private long ownerCid;
/// <summary>
/// 当前赴宴玩家数量
/// </summary>
private int joinerCount;
/// <summary>
/// 当前宴会人气
/// </summary>
private long score;
/// <summary>
/// 结束时间戳（秒）
/// </summary>
private int endTs;
/// <summary>
/// 是否加入宴会
/// </summary>
private bool isJoined;
/// <summary>
/// 凭证类型
/// </summary>
private Common.DinnerEnum.EDinnerPermitType permitType;
/// <summary>
/// 凭证类型ID
/// </summary>
private long permitTypeId;


public Dinner_Idx() {
	instanceId = (long)0;
	dinnerId = (long)0;
	ownerCid = (long)0;
	joinerCount = 0;
	score = (long)0;
	endTs = 0;
	isJoined = false;
	permitType = 0;
	permitTypeId = (long)0;
}

public Dinner_Idx(
	long _instanceId
	, long _dinnerId
	, long _ownerCid
	, int _joinerCount
	, long _score
	, int _endTs
	, bool _isJoined
	, Common.DinnerEnum.EDinnerPermitType _permitType
	, long _permitTypeId
) {	instanceId = _instanceId;
	dinnerId = _dinnerId;
	ownerCid = _ownerCid;
	joinerCount = _joinerCount;
	score = _score;
	endTs = _endTs;
	isJoined = _isJoined;
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
/// 开宴玩家CID
/// </summary>
public long getOwnerCid() { return ownerCid; }
/// <summary>
/// 开宴玩家CID
/// </summary>
public void setOwnerCid(long _ownerCid) { ownerCid = _ownerCid; }
/// <summary>
/// 当前赴宴玩家数量
/// </summary>
public int getJoinerCount() { return joinerCount; }
/// <summary>
/// 当前赴宴玩家数量
/// </summary>
public void setJoinerCount(int _joinerCount) { joinerCount = _joinerCount; }
/// <summary>
/// 当前宴会人气
/// </summary>
public long getScore() { return score; }
/// <summary>
/// 当前宴会人气
/// </summary>
public void setScore(long _score) { score = _score; }
/// <summary>
/// 结束时间戳（秒）
/// </summary>
public int getEndTs() { return endTs; }
/// <summary>
/// 结束时间戳（秒）
/// </summary>
public void setEndTs(int _endTs) { endTs = _endTs; }
/// <summary>
/// 是否加入宴会
/// </summary>
public bool getIsJoined() { return isJoined; }
/// <summary>
/// 是否加入宴会
/// </summary>
public void setIsJoined(bool _isJoined) { isJoined = _isJoined; }
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
	int _size = 53;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 55;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dinnerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	ownerCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinerCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isJoined = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	permitType = (Common.DinnerEnum.EDinnerPermitType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	permitTypeId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(dinnerId);
	_buf.putLong(ownerCid);
	_buf.putInt(joinerCount);
	_buf.putLong(score);
	_buf.putInt(endTs);
	_buf.put(isJoined?(byte)1:(byte)0);
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
	builder.Append("ownerCid").Append(":").Append(ownerCid.ToString()).Append(", ");
	builder.Append("joinerCount").Append(":").Append(joinerCount.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("endTs").Append(":").Append(endTs.ToString()).Append(", ");
	builder.Append("isJoined").Append(":").Append(isJoined.ToString()).Append(", ");
	builder.Append("permitType").Append(":").Append(permitType.ToString()).Append(", ");
	builder.Append("permitTypeId").Append(":").Append(permitTypeId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

