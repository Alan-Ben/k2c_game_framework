using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.AnecdoteObj
{

/// <summary>
/// 政务事件数据
/// </summary>
public class Anecdote_EventInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 位置id
/// </summary>
private long posId;
/// <summary>
/// 事件id
/// </summary>
private long eventId;
/// <summary>
/// 事件额外信息
/// </summary>
private byte[] extraData;


public Anecdote_EventInfo() {
	instanceId = (long)0;
	posId = (long)0;
	eventId = (long)0;
	extraData = null;
}

public Anecdote_EventInfo(
	long _instanceId
	, long _posId
	, long _eventId
	, byte[] _extraData
) {	instanceId = _instanceId;
	posId = _posId;
	eventId = _eventId;
	extraData = _extraData;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 位置id
/// </summary>
public long getPosId() { return posId; }
/// <summary>
/// 位置id
/// </summary>
public void setPosId(long _posId) { posId = _posId; }
/// <summary>
/// 事件id
/// </summary>
public long getEventId() { return eventId; }
/// <summary>
/// 事件id
/// </summary>
public void setEventId(long _eventId) { eventId = _eventId; }
/// <summary>
/// 事件额外信息
/// </summary>
public byte[] getExtraData() { return extraData; }

/// <summary>
/// 事件额外信息
/// </summary>
public void setExtraData(byte[] _extraData) { extraData = _extraData; }



public int GetBufSize() {
	int _size = 24;
	_size += 4 + (extraData == null ? 0 : extraData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;
	_size += 4 + (extraData == null ? 0 : extraData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	posId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extraData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(posId);
	_buf.putLong(eventId);
	_buf.putByteBuffer(extraData);

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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("posId").Append(":").Append(posId.ToString()).Append(", ");
	builder.Append("eventId").Append(":").Append(eventId.ToString()).Append(", ");
	builder.Append("extraData").Append(":").Append(extraData == null ? "null" : extraData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

