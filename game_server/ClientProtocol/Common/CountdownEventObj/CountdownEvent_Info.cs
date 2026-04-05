using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CountdownEventObj
{

/// <summary>
/// 倒计时事件_数据
/// </summary>
public class CountdownEvent_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据ID
/// </summary>
private long dbId;
/// <summary>
/// 事件ID
/// </summary>
private long eventId;
/// <summary>
/// 触发时间(毫秒)
/// </summary>
private long triggerTimeMs;
/// <summary>
/// 重置次数
/// </summary>
private int resetCount;


public CountdownEvent_Info() {
	dbId = (long)0;
	eventId = (long)0;
	triggerTimeMs = (long)0;
	resetCount = 0;
}

public CountdownEvent_Info(
	long _dbId
	, long _eventId
	, long _triggerTimeMs
	, int _resetCount
) {	dbId = _dbId;
	eventId = _eventId;
	triggerTimeMs = _triggerTimeMs;
	resetCount = _resetCount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 数据ID
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据ID
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
/// <summary>
/// 事件ID
/// </summary>
public long getEventId() { return eventId; }
/// <summary>
/// 事件ID
/// </summary>
public void setEventId(long _eventId) { eventId = _eventId; }
/// <summary>
/// 触发时间(毫秒)
/// </summary>
public long getTriggerTimeMs() { return triggerTimeMs; }
/// <summary>
/// 触发时间(毫秒)
/// </summary>
public void setTriggerTimeMs(long _triggerTimeMs) { triggerTimeMs = _triggerTimeMs; }
/// <summary>
/// 重置次数
/// </summary>
public int getResetCount() { return resetCount; }
/// <summary>
/// 重置次数
/// </summary>
public void setResetCount(int _resetCount) { resetCount = _resetCount; }


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
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	triggerTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	resetCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(eventId);
	_buf.putLong(triggerTimeMs);
	_buf.putInt(resetCount);
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("eventId").Append(":").Append(eventId.ToString()).Append(", ");
	builder.Append("triggerTimeMs").Append(":").Append(triggerTimeMs.ToString()).Append(", ");
	builder.Append("resetCount").Append(":").Append(resetCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

