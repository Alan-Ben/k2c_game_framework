using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.PlayerObj
{

/// <summary>
/// 玩家贸易站数据
/// </summary>
public class Player_StationInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 等级
/// </summary>
private int level;
/// <summary>
/// 已产出数量
/// </summary>
private long hadOutputNum;
/// <summary>
/// 已产出时间
/// </summary>
private long hadOutputSec;
/// <summary>
/// 上次结算时间戳
/// </summary>
private long lastSettleTimeMs;


public Player_StationInfo() {
	level = 0;
	hadOutputNum = (long)0;
	hadOutputSec = (long)0;
	lastSettleTimeMs = (long)0;
}

public Player_StationInfo(
	int _level
	, long _hadOutputNum
	, long _hadOutputSec
	, long _lastSettleTimeMs
) {	level = _level;
	hadOutputNum = _hadOutputNum;
	hadOutputSec = _hadOutputSec;
	lastSettleTimeMs = _lastSettleTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 等级
/// </summary>
public void setLevel(int _level) { level = _level; }
/// <summary>
/// 已产出数量
/// </summary>
public long getHadOutputNum() { return hadOutputNum; }
/// <summary>
/// 已产出数量
/// </summary>
public void setHadOutputNum(long _hadOutputNum) { hadOutputNum = _hadOutputNum; }
/// <summary>
/// 已产出时间
/// </summary>
public long getHadOutputSec() { return hadOutputSec; }
/// <summary>
/// 已产出时间
/// </summary>
public void setHadOutputSec(long _hadOutputSec) { hadOutputSec = _hadOutputSec; }
/// <summary>
/// 上次结算时间戳
/// </summary>
public long getLastSettleTimeMs() { return lastSettleTimeMs; }
/// <summary>
/// 上次结算时间戳
/// </summary>
public void setLastSettleTimeMs(long _lastSettleTimeMs) { lastSettleTimeMs = _lastSettleTimeMs; }


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
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadOutputNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadOutputSec = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastSettleTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(level);
	_buf.putLong(hadOutputNum);
	_buf.putLong(hadOutputSec);
	_buf.putLong(lastSettleTimeMs);
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
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("hadOutputNum").Append(":").Append(hadOutputNum.ToString()).Append(", ");
	builder.Append("hadOutputSec").Append(":").Append(hadOutputSec.ToString()).Append(", ");
	builder.Append("lastSettleTimeMs").Append(":").Append(lastSettleTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

