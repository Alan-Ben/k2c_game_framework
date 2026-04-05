using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TravelObj
{

/// <summary>
/// 游历事件
/// </summary>
public class Travel_Event : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 事件ID
/// </summary>
private long eventId;
/// <summary>
/// 位置
/// </summary>
private long pos;


public Travel_Event() {
	instanceId = (long)0;
	eventId = (long)0;
	pos = (long)0;
}

public Travel_Event(
	long _instanceId
	, long _eventId
	, long _pos
) {	instanceId = _instanceId;
	eventId = _eventId;
	pos = _pos;
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
/// 事件ID
/// </summary>
public long getEventId() { return eventId; }
/// <summary>
/// 事件ID
/// </summary>
public void setEventId(long _eventId) { eventId = _eventId; }
/// <summary>
/// 位置
/// </summary>
public long getPos() { return pos; }
/// <summary>
/// 位置
/// </summary>
public void setPos(long _pos) { pos = _pos; }


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
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pos = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(eventId);
	_buf.putLong(pos);
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
	builder.Append("eventId").Append(":").Append(eventId.ToString()).Append(", ");
	builder.Append("pos").Append(":").Append(pos.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

