using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChildObj
{

/// <summary>
/// 子嗣训练位数据
/// </summary>
public class Child_SeatInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 训练位ID
/// </summary>
private long seatId;
/// <summary>
/// 最大脑力值
/// </summary>
private int maxEnergy;
/// <summary>
/// 脑力值
/// </summary>
private int energy;
/// <summary>
/// 上次计算时间（毫秒）
/// </summary>
private long lastCalMs;
/// <summary>
/// 脑力值已满时，获得下一点脑力值所需时间
/// </summary>
private long fullGetNextRemainMs;


public Child_SeatInfo() {
	seatId = (long)0;
	maxEnergy = 0;
	energy = 0;
	lastCalMs = (long)0;
	fullGetNextRemainMs = (long)0;
}

public Child_SeatInfo(
	long _seatId
	, int _maxEnergy
	, int _energy
	, long _lastCalMs
	, long _fullGetNextRemainMs
) {	seatId = _seatId;
	maxEnergy = _maxEnergy;
	energy = _energy;
	lastCalMs = _lastCalMs;
	fullGetNextRemainMs = _fullGetNextRemainMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 训练位ID
/// </summary>
public long getSeatId() { return seatId; }
/// <summary>
/// 训练位ID
/// </summary>
public void setSeatId(long _seatId) { seatId = _seatId; }
/// <summary>
/// 最大脑力值
/// </summary>
public int getMaxEnergy() { return maxEnergy; }
/// <summary>
/// 最大脑力值
/// </summary>
public void setMaxEnergy(int _maxEnergy) { maxEnergy = _maxEnergy; }
/// <summary>
/// 脑力值
/// </summary>
public int getEnergy() { return energy; }
/// <summary>
/// 脑力值
/// </summary>
public void setEnergy(int _energy) { energy = _energy; }
/// <summary>
/// 上次计算时间（毫秒）
/// </summary>
public long getLastCalMs() { return lastCalMs; }
/// <summary>
/// 上次计算时间（毫秒）
/// </summary>
public void setLastCalMs(long _lastCalMs) { lastCalMs = _lastCalMs; }
/// <summary>
/// 脑力值已满时，获得下一点脑力值所需时间
/// </summary>
public long getFullGetNextRemainMs() { return fullGetNextRemainMs; }
/// <summary>
/// 脑力值已满时，获得下一点脑力值所需时间
/// </summary>
public void setFullGetNextRemainMs(long _fullGetNextRemainMs) { fullGetNextRemainMs = _fullGetNextRemainMs; }


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
	seatId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxEnergy = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	energy = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastCalMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fullGetNextRemainMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(seatId);
	_buf.putInt(maxEnergy);
	_buf.putInt(energy);
	_buf.putLong(lastCalMs);
	_buf.putLong(fullGetNextRemainMs);
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
	builder.Append("seatId").Append(":").Append(seatId.ToString()).Append(", ");
	builder.Append("maxEnergy").Append(":").Append(maxEnergy.ToString()).Append(", ");
	builder.Append("energy").Append(":").Append(energy.ToString()).Append(", ");
	builder.Append("lastCalMs").Append(":").Append(lastCalMs.ToString()).Append(", ");
	builder.Append("fullGetNextRemainMs").Append(":").Append(fullGetNextRemainMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

