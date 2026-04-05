using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DungeonObj
{

/// <summary>
/// 晚间副本时间信息
/// </summary>
public class EveningDungeon_TimeInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 上轮关闭时间戳
/// </summary>
private long preCloseTimeMs;
/// <summary>
/// 预告时间戳
/// </summary>
private long previewTimeMs;
/// <summary>
/// 开始时间戳
/// </summary>
private long startTimeMs;
/// <summary>
/// 结束时间戳
/// </summary>
private long endTimeMs;


public EveningDungeon_TimeInfo() {
	preCloseTimeMs = (long)0;
	previewTimeMs = (long)0;
	startTimeMs = (long)0;
	endTimeMs = (long)0;
}

public EveningDungeon_TimeInfo(
	long _preCloseTimeMs
	, long _previewTimeMs
	, long _startTimeMs
	, long _endTimeMs
) {	preCloseTimeMs = _preCloseTimeMs;
	previewTimeMs = _previewTimeMs;
	startTimeMs = _startTimeMs;
	endTimeMs = _endTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 上轮关闭时间戳
/// </summary>
public long getPreCloseTimeMs() { return preCloseTimeMs; }
/// <summary>
/// 上轮关闭时间戳
/// </summary>
public void setPreCloseTimeMs(long _preCloseTimeMs) { preCloseTimeMs = _preCloseTimeMs; }
/// <summary>
/// 预告时间戳
/// </summary>
public long getPreviewTimeMs() { return previewTimeMs; }
/// <summary>
/// 预告时间戳
/// </summary>
public void setPreviewTimeMs(long _previewTimeMs) { previewTimeMs = _previewTimeMs; }
/// <summary>
/// 开始时间戳
/// </summary>
public long getStartTimeMs() { return startTimeMs; }
/// <summary>
/// 开始时间戳
/// </summary>
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/// <summary>
/// 结束时间戳
/// </summary>
public long getEndTimeMs() { return endTimeMs; }
/// <summary>
/// 结束时间戳
/// </summary>
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }


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
	preCloseTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	previewTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(preCloseTimeMs);
	_buf.putLong(previewTimeMs);
	_buf.putLong(startTimeMs);
	_buf.putLong(endTimeMs);
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
	builder.Append("preCloseTimeMs").Append(":").Append(preCloseTimeMs.ToString()).Append(", ");
	builder.Append("previewTimeMs").Append(":").Append(previewTimeMs.ToString()).Append(", ");
	builder.Append("startTimeMs").Append(":").Append(startTimeMs.ToString()).Append(", ");
	builder.Append("endTimeMs").Append(":").Append(endTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

