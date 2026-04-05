using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.LevyObj
{

/// <summary>
/// 银币征收信息
/// </summary>
public class Levy_Silver : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 上次结算时间（毫秒）
/// </summary>
private long lastSettleMs;
/// <summary>
/// 本次征收开始时间（毫秒）
/// </summary>
private long startMs;
/// <summary>
/// 每秒产出数量
/// </summary>
private long speed;
/// <summary>
/// 已经结算的数量总和
/// </summary>
private long settledSum;


public Levy_Silver() {
	lastSettleMs = (long)0;
	startMs = (long)0;
	speed = (long)0;
	settledSum = (long)0;
}

public Levy_Silver(
	long _lastSettleMs
	, long _startMs
	, long _speed
	, long _settledSum
) {	lastSettleMs = _lastSettleMs;
	startMs = _startMs;
	speed = _speed;
	settledSum = _settledSum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 上次结算时间（毫秒）
/// </summary>
public long getLastSettleMs() { return lastSettleMs; }
/// <summary>
/// 上次结算时间（毫秒）
/// </summary>
public void setLastSettleMs(long _lastSettleMs) { lastSettleMs = _lastSettleMs; }
/// <summary>
/// 本次征收开始时间（毫秒）
/// </summary>
public long getStartMs() { return startMs; }
/// <summary>
/// 本次征收开始时间（毫秒）
/// </summary>
public void setStartMs(long _startMs) { startMs = _startMs; }
/// <summary>
/// 每秒产出数量
/// </summary>
public long getSpeed() { return speed; }
/// <summary>
/// 每秒产出数量
/// </summary>
public void setSpeed(long _speed) { speed = _speed; }
/// <summary>
/// 已经结算的数量总和
/// </summary>
public long getSettledSum() { return settledSum; }
/// <summary>
/// 已经结算的数量总和
/// </summary>
public void setSettledSum(long _settledSum) { settledSum = _settledSum; }


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
	lastSettleMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	speed = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	settledSum = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(lastSettleMs);
	_buf.putLong(startMs);
	_buf.putLong(speed);
	_buf.putLong(settledSum);
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
	builder.Append("lastSettleMs").Append(":").Append(lastSettleMs.ToString()).Append(", ");
	builder.Append("startMs").Append(":").Append(startMs.ToString()).Append(", ");
	builder.Append("speed").Append(":").Append(speed.ToString()).Append(", ");
	builder.Append("settledSum").Append(":").Append(settledSum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

