using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.LevyObj
{

/// <summary>
/// 士兵征收信息
/// </summary>
public class Levy_Soldier : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 下次结算时间（毫秒）
/// </summary>
private long nextSettleMs;
/// <summary>
/// 当前周期用时（毫秒）
/// </summary>
private long curDurationMs;
/// <summary>
/// 使用次数
/// </summary>
private int count;


public Levy_Soldier() {
	nextSettleMs = (long)0;
	curDurationMs = (long)0;
	count = 0;
}

public Levy_Soldier(
	long _nextSettleMs
	, long _curDurationMs
	, int _count
) {	nextSettleMs = _nextSettleMs;
	curDurationMs = _curDurationMs;
	count = _count;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 下次结算时间（毫秒）
/// </summary>
public long getNextSettleMs() { return nextSettleMs; }
/// <summary>
/// 下次结算时间（毫秒）
/// </summary>
public void setNextSettleMs(long _nextSettleMs) { nextSettleMs = _nextSettleMs; }
/// <summary>
/// 当前周期用时（毫秒）
/// </summary>
public long getCurDurationMs() { return curDurationMs; }
/// <summary>
/// 当前周期用时（毫秒）
/// </summary>
public void setCurDurationMs(long _curDurationMs) { curDurationMs = _curDurationMs; }
/// <summary>
/// 使用次数
/// </summary>
public int getCount() { return count; }
/// <summary>
/// 使用次数
/// </summary>
public void setCount(int _count) { count = _count; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextSettleMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curDurationMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(nextSettleMs);
	_buf.putLong(curDurationMs);
	_buf.putInt(count);
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
	builder.Append("nextSettleMs").Append(":").Append(nextSettleMs.ToString()).Append(", ");
	builder.Append("curDurationMs").Append(":").Append(curDurationMs.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

