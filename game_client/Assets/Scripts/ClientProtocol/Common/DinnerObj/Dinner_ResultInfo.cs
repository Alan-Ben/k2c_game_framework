using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DinnerObj
{

/// <summary>
/// 宴会结算信息
/// </summary>
public class Dinner_ResultInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宴会实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 宴会配置ID
/// </summary>
private long dinnerId;
/// <summary>
/// 获取的宴会币
/// </summary>
private long gainCoin;
/// <summary>
/// 获得的宴会人气
/// </summary>
private long gainScore;
/// <summary>
/// 宴会人气加成
/// </summary>
private long scoreAddPer;
/// <summary>
/// 凭证类型
/// </summary>
private Common.DinnerEnum.EDinnerPermitType permitType;
/// <summary>
/// 凭证类型ID
/// </summary>
private long permitTypeId;
/// <summary>
/// 宾客信息列表
/// </summary>
private List<Common.DinnerObj.Dinner_ResultGuestInfo> guestLog;


public Dinner_ResultInfo() {
	instanceId = (long)0;
	dinnerId = (long)0;
	gainCoin = (long)0;
	gainScore = (long)0;
	scoreAddPer = (long)0;
	permitType = 0;
	permitTypeId = (long)0;
	guestLog = new List<Common.DinnerObj.Dinner_ResultGuestInfo>();
}

public Dinner_ResultInfo(
	long _instanceId
	, long _dinnerId
	, long _gainCoin
	, long _gainScore
	, long _scoreAddPer
	, Common.DinnerEnum.EDinnerPermitType _permitType
	, long _permitTypeId
	, List<Common.DinnerObj.Dinner_ResultGuestInfo> _guestLog
) {	instanceId = _instanceId;
	dinnerId = _dinnerId;
	gainCoin = _gainCoin;
	gainScore = _gainScore;
	scoreAddPer = _scoreAddPer;
	permitType = _permitType;
	permitTypeId = _permitTypeId;
	guestLog = _guestLog;
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
/// 获取的宴会币
/// </summary>
public long getGainCoin() { return gainCoin; }
/// <summary>
/// 获取的宴会币
/// </summary>
public void setGainCoin(long _gainCoin) { gainCoin = _gainCoin; }
/// <summary>
/// 获得的宴会人气
/// </summary>
public long getGainScore() { return gainScore; }
/// <summary>
/// 获得的宴会人气
/// </summary>
public void setGainScore(long _gainScore) { gainScore = _gainScore; }
/// <summary>
/// 宴会人气加成
/// </summary>
public long getScoreAddPer() { return scoreAddPer; }
/// <summary>
/// 宴会人气加成
/// </summary>
public void setScoreAddPer(long _scoreAddPer) { scoreAddPer = _scoreAddPer; }
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
/// <summary>
/// 宾客信息列表
/// </summary>
public List<Common.DinnerObj.Dinner_ResultGuestInfo> getGuestLog() { return guestLog; }
/// <summary>
/// 宾客信息列表
/// </summary>
public void addGuestLog(Common.DinnerObj.Dinner_ResultGuestInfo _guestLog) { guestLog.Add(_guestLog); }


public int GetBufSize() {
	int _size = 52;
	_size += 2 + (guestLog.Count * 40);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 54;
	_size += 2 + (guestLog.Count * 40);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dinnerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainCoin = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	scoreAddPer = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	permitType = (Common.DinnerEnum.EDinnerPermitType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	permitTypeId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _guestLogCount = _buf.getShort();
	for(int _i = 0; _i < _guestLogCount; _i++) { 
		Common.DinnerObj.Dinner_ResultGuestInfo _guestLog = new Common.DinnerObj.Dinner_ResultGuestInfo();
		int __guestLogCustLen = _buf.getInt();
	int __guestLogCurPos = _buf.getCurPos();
	_guestLog.ReadUnzipBuf(_buf, __guestLogCurPos + __guestLogCustLen);
	_buf.setPosition(__guestLogCurPos + __guestLogCustLen);

		guestLog.Add(_guestLog);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(dinnerId);
	_buf.putLong(gainCoin);
	_buf.putLong(gainScore);
	_buf.putLong(scoreAddPer);
	_buf.putInt((int)permitType);

	_buf.putLong(permitTypeId);
	_buf.putShort((short)guestLog.Count);
	for(int _i = 0; _i < guestLog.Count; _i++) { 
		_buf.putInt(guestLog[_i].GetBufSize());
	guestLog[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("gainCoin").Append(":").Append(gainCoin.ToString()).Append(", ");
	builder.Append("gainScore").Append(":").Append(gainScore.ToString()).Append(", ");
	builder.Append("scoreAddPer").Append(":").Append(scoreAddPer.ToString()).Append(", ");
	builder.Append("permitType").Append(":").Append(permitType.ToString()).Append(", ");
	builder.Append("permitTypeId").Append(":").Append(permitTypeId.ToString()).Append(", ");
	builder.Append("guestLog").Append(":").Append(guestLog.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

