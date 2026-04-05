using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.PlayerObj
{

/// <summary>
/// 玩家火星能量信息
/// </summary>
public class Player_MarsEnergyInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 上次结算时间
/// </summary>
private long lastSettleTimeMs;
private long count;
/// <summary>
/// 消耗速度（单位：分钟）
/// </summary>
private long costSpeed;


public Player_MarsEnergyInfo() {
	lastSettleTimeMs = (long)0;
	count = (long)0;
	costSpeed = (long)0;
}

public Player_MarsEnergyInfo(
	long _lastSettleTimeMs
	, long _count
	, long _costSpeed
) {	lastSettleTimeMs = _lastSettleTimeMs;
	count = _count;
	costSpeed = _costSpeed;
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
/// 消耗速度（单位：分钟）
/// </summary>
public long getCostSpeed() { return costSpeed; }
/// <summary>
/// 消耗速度（单位：分钟）
/// </summary>
public void setCostSpeed(long _costSpeed) { costSpeed = _costSpeed; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastSettleTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	costSpeed = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(lastSettleTimeMs);
	_buf.putLong(count);
	_buf.putLong(costSpeed);
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
	builder.Append("costSpeed").Append(":").Append(costSpeed.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

