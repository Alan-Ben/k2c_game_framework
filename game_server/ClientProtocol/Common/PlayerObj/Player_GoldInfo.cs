using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.PlayerObj
{

/// <summary>
/// 玩家金币信息
/// </summary>
public class Player_GoldInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 上次结算时间
/// </summary>
private long lastSettleTimeMs;
private long count;
/// <summary>
/// 产出速度
/// </summary>
private long outputSpeed;
/// <summary>
/// 总消耗数量
/// </summary>
private long totalConsumeCount;


public Player_GoldInfo() {
	lastSettleTimeMs = (long)0;
	count = (long)0;
	outputSpeed = (long)0;
	totalConsumeCount = (long)0;
}

public Player_GoldInfo(
	long _lastSettleTimeMs
	, long _count
	, long _outputSpeed
	, long _totalConsumeCount
) {	lastSettleTimeMs = _lastSettleTimeMs;
	count = _count;
	outputSpeed = _outputSpeed;
	totalConsumeCount = _totalConsumeCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 上次结算时间
/// </summary>
public long getLastSettleTimeMs() { return lastSettleTimeMs; }
/// <summary>
/// 上次结算时间
/// </summary>
public void setLastSettleTimeMs(long _lastSettleTimeMs) { lastSettleTimeMs = _lastSettleTimeMs; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }
/// <summary>
/// 产出速度
/// </summary>
public long getOutputSpeed() { return outputSpeed; }
/// <summary>
/// 产出速度
/// </summary>
public void setOutputSpeed(long _outputSpeed) { outputSpeed = _outputSpeed; }
/// <summary>
/// 总消耗数量
/// </summary>
public long getTotalConsumeCount() { return totalConsumeCount; }
/// <summary>
/// 总消耗数量
/// </summary>
public void setTotalConsumeCount(long _totalConsumeCount) { totalConsumeCount = _totalConsumeCount; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastSettleTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	outputSpeed = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalConsumeCount = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(lastSettleTimeMs);
	_buf.putLong(count);
	_buf.putLong(outputSpeed);
	_buf.putLong(totalConsumeCount);
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
	builder.Append("lastSettleTimeMs").Append(":").Append(lastSettleTimeMs.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("outputSpeed").Append(":").Append(outputSpeed.ToString()).Append(", ");
	builder.Append("totalConsumeCount").Append(":").Append(totalConsumeCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

