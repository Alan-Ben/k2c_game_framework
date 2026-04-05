using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.RushExchangeObj
{

/// <summary>
/// 急速兑换信息
/// </summary>
public class RushExchange_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 礼包组ID
/// </summary>
private long groupId;
/// <summary>
/// 当前礼包配置ID
/// </summary>
private long refId;
/// <summary>
/// 当前兑换开始时间 如果没兑换影响刷新礼包时间
/// </summary>
private long activeTimeMs;
/// <summary>
/// 兑换时间 影响什么时候可以领奖
/// </summary>
private long exchangeTimeMs;
/// <summary>
/// 是否已领奖
/// </summary>
private bool isRewarded;
/// <summary>
/// 当天兑换次数
/// </summary>
private int todayExchangeCount;
/// <summary>
/// 下次刷新兑换次数时间
/// </summary>
private long nextResetCountTimeMs;


public RushExchange_Info() {
	groupId = (long)0;
	refId = (long)0;
	activeTimeMs = (long)0;
	exchangeTimeMs = (long)0;
	isRewarded = false;
	todayExchangeCount = 0;
	nextResetCountTimeMs = (long)0;
}

public RushExchange_Info(
	long _groupId
	, long _refId
	, long _activeTimeMs
	, long _exchangeTimeMs
	, bool _isRewarded
	, int _todayExchangeCount
	, long _nextResetCountTimeMs
) {	groupId = _groupId;
	refId = _refId;
	activeTimeMs = _activeTimeMs;
	exchangeTimeMs = _exchangeTimeMs;
	isRewarded = _isRewarded;
	todayExchangeCount = _todayExchangeCount;
	nextResetCountTimeMs = _nextResetCountTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 礼包组ID
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 礼包组ID
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }
/// <summary>
/// 当前礼包配置ID
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// 当前礼包配置ID
/// </summary>
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 当前兑换开始时间 如果没兑换影响刷新礼包时间
/// </summary>
public long getActiveTimeMs() { return activeTimeMs; }
/// <summary>
/// 当前兑换开始时间 如果没兑换影响刷新礼包时间
/// </summary>
public void setActiveTimeMs(long _activeTimeMs) { activeTimeMs = _activeTimeMs; }
/// <summary>
/// 兑换时间 影响什么时候可以领奖
/// </summary>
public long getExchangeTimeMs() { return exchangeTimeMs; }
/// <summary>
/// 兑换时间 影响什么时候可以领奖
/// </summary>
public void setExchangeTimeMs(long _exchangeTimeMs) { exchangeTimeMs = _exchangeTimeMs; }
/// <summary>
/// 是否已领奖
/// </summary>
public bool getIsRewarded() { return isRewarded; }
/// <summary>
/// 是否已领奖
/// </summary>
public void setIsRewarded(bool _isRewarded) { isRewarded = _isRewarded; }
/// <summary>
/// 当天兑换次数
/// </summary>
public int getTodayExchangeCount() { return todayExchangeCount; }
/// <summary>
/// 当天兑换次数
/// </summary>
public void setTodayExchangeCount(int _todayExchangeCount) { todayExchangeCount = _todayExchangeCount; }
/// <summary>
/// 下次刷新兑换次数时间
/// </summary>
public long getNextResetCountTimeMs() { return nextResetCountTimeMs; }
/// <summary>
/// 下次刷新兑换次数时间
/// </summary>
public void setNextResetCountTimeMs(long _nextResetCountTimeMs) { nextResetCountTimeMs = _nextResetCountTimeMs; }


public int GetBufSize() {
	int _size = 45;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 47;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exchangeTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isRewarded = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	todayExchangeCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextResetCountTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(groupId);
	_buf.putLong(refId);
	_buf.putLong(activeTimeMs);
	_buf.putLong(exchangeTimeMs);
	_buf.put(isRewarded?(byte)1:(byte)0);
	_buf.putInt(todayExchangeCount);
	_buf.putLong(nextResetCountTimeMs);
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
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("activeTimeMs").Append(":").Append(activeTimeMs.ToString()).Append(", ");
	builder.Append("exchangeTimeMs").Append(":").Append(exchangeTimeMs.ToString()).Append(", ");
	builder.Append("isRewarded").Append(":").Append(isRewarded.ToString()).Append(", ");
	builder.Append("todayExchangeCount").Append(":").Append(todayExchangeCount.ToString()).Append(", ");
	builder.Append("nextResetCountTimeMs").Append(":").Append(nextResetCountTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

