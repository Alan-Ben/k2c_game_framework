using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.LevyObj
{

/// <summary>
/// 粮食征收信息
/// </summary>
public class Levy_Food : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 上次结算时间（毫秒）
/// </summary>
private long lastSettleMs;
/// <summary>
/// 每秒产出数量
/// </summary>
private long speed;


public Levy_Food() {
	lastSettleMs = (long)0;
	speed = (long)0;
}

public Levy_Food(
	long _lastSettleMs
	, long _speed
) {	lastSettleMs = _lastSettleMs;
	speed = _speed;
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
/// 每秒产出数量
/// </summary>
public long getSpeed() { return speed; }
/// <summary>
/// 每秒产出数量
/// </summary>
public void setSpeed(long _speed) { speed = _speed; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastSettleMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	speed = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(lastSettleMs);
	_buf.putLong(speed);
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
	builder.Append("speed").Append(":").Append(speed.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

