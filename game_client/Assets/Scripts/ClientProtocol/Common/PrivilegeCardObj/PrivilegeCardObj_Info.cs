using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.PrivilegeCardObj
{

/// <summary>
/// 权益卡数据
/// </summary>
public class PrivilegeCardObj_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 权益卡类型
/// </summary>
private Common.PrivilegeCardEnum.EPrivilegeCardType cardType;
/// <summary>
/// 生效开始时间戳（秒）
/// </summary>
private long startS;
/// <summary>
/// 生效结束时间戳（秒）
/// </summary>
private long endS;
/// <summary>
/// 最后一次领取每日奖励的时间戳（秒）
/// </summary>
private long lastGainDailyRewardS;


public PrivilegeCardObj_Info() {
	cardType = 0;
	startS = (long)0;
	endS = (long)0;
	lastGainDailyRewardS = (long)0;
}

public PrivilegeCardObj_Info(
	Common.PrivilegeCardEnum.EPrivilegeCardType _cardType
	, long _startS
	, long _endS
	, long _lastGainDailyRewardS
) {	cardType = _cardType;
	startS = _startS;
	endS = _endS;
	lastGainDailyRewardS = _lastGainDailyRewardS;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 权益卡类型
/// </summary>
public Common.PrivilegeCardEnum.EPrivilegeCardType getCardType() { return cardType; }
/// <summary>
/// 权益卡类型
/// </summary>
public void setCardType(Common.PrivilegeCardEnum.EPrivilegeCardType _cardType) { cardType = _cardType; }
/// <summary>
/// 生效开始时间戳（秒）
/// </summary>
public long getStartS() { return startS; }
/// <summary>
/// 生效开始时间戳（秒）
/// </summary>
public void setStartS(long _startS) { startS = _startS; }
/// <summary>
/// 生效结束时间戳（秒）
/// </summary>
public long getEndS() { return endS; }
/// <summary>
/// 生效结束时间戳（秒）
/// </summary>
public void setEndS(long _endS) { endS = _endS; }
/// <summary>
/// 最后一次领取每日奖励的时间戳（秒）
/// </summary>
public long getLastGainDailyRewardS() { return lastGainDailyRewardS; }
/// <summary>
/// 最后一次领取每日奖励的时间戳（秒）
/// </summary>
public void setLastGainDailyRewardS(long _lastGainDailyRewardS) { lastGainDailyRewardS = _lastGainDailyRewardS; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cardType = (Common.PrivilegeCardEnum.EPrivilegeCardType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startS = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endS = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastGainDailyRewardS = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)cardType);

	_buf.putLong(startS);
	_buf.putLong(endS);
	_buf.putLong(lastGainDailyRewardS);
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
	builder.Append("cardType").Append(":").Append(cardType.ToString()).Append(", ");
	builder.Append("startS").Append(":").Append(startS.ToString()).Append(", ");
	builder.Append("endS").Append(":").Append(endS.ToString()).Append(", ");
	builder.Append("lastGainDailyRewardS").Append(":").Append(lastGainDailyRewardS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

