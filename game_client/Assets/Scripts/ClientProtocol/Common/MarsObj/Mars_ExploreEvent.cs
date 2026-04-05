using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星探索-事件数据
/// </summary>
public class Mars_ExploreEvent : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long id;
/// <summary>
/// 事件状态
/// </summary>
private Common.MarsEnum.EMarsExploreEventType eventType;
/// <summary>
/// 事件ID
/// </summary>
private long eventId;
/// <summary>
/// 事件位置
/// </summary>
private long pos;
/// <summary>
/// 是否完成
/// </summary>
private bool isDone;
/// <summary>
/// 探索等级
/// </summary>
private int exploreLvl;
/// <summary>
/// 创建时间（毫秒）
/// </summary>
private long createdMs;


public Mars_ExploreEvent() {
	id = (long)0;
	eventType = 0;
	eventId = (long)0;
	pos = (long)0;
	isDone = false;
	exploreLvl = 0;
	createdMs = (long)0;
}

public Mars_ExploreEvent(
	long _id
	, Common.MarsEnum.EMarsExploreEventType _eventType
	, long _eventId
	, long _pos
	, bool _isDone
	, int _exploreLvl
	, long _createdMs
) {	id = _id;
	eventType = _eventType;
	eventId = _eventId;
	pos = _pos;
	isDone = _isDone;
	exploreLvl = _exploreLvl;
	createdMs = _createdMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 事件状态
/// </summary>
public Common.MarsEnum.EMarsExploreEventType getEventType() { return eventType; }
/// <summary>
/// 事件状态
/// </summary>
public void setEventType(Common.MarsEnum.EMarsExploreEventType _eventType) { eventType = _eventType; }
/// <summary>
/// 事件ID
/// </summary>
public long getEventId() { return eventId; }
/// <summary>
/// 事件ID
/// </summary>
public void setEventId(long _eventId) { eventId = _eventId; }
/// <summary>
/// 事件位置
/// </summary>
public long getPos() { return pos; }
/// <summary>
/// 事件位置
/// </summary>
public void setPos(long _pos) { pos = _pos; }
/// <summary>
/// 是否完成
/// </summary>
public bool getIsDone() { return isDone; }
/// <summary>
/// 是否完成
/// </summary>
public void setIsDone(bool _isDone) { isDone = _isDone; }
/// <summary>
/// 探索等级
/// </summary>
public int getExploreLvl() { return exploreLvl; }
/// <summary>
/// 探索等级
/// </summary>
public void setExploreLvl(int _exploreLvl) { exploreLvl = _exploreLvl; }
/// <summary>
/// 创建时间（毫秒）
/// </summary>
public long getCreatedMs() { return createdMs; }
/// <summary>
/// 创建时间（毫秒）
/// </summary>
public void setCreatedMs(long _createdMs) { createdMs = _createdMs; }


public int GetBufSize() {
	int _size = 41;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 43;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventType = (Common.MarsEnum.EMarsExploreEventType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pos = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isDone = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exploreLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createdMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt((int)eventType);

	_buf.putLong(eventId);
	_buf.putLong(pos);
	_buf.put(isDone?(byte)1:(byte)0);
	_buf.putInt(exploreLvl);
	_buf.putLong(createdMs);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("eventType").Append(":").Append(eventType.ToString()).Append(", ");
	builder.Append("eventId").Append(":").Append(eventId.ToString()).Append(", ");
	builder.Append("pos").Append(":").Append(pos.ToString()).Append(", ");
	builder.Append("isDone").Append(":").Append(isDone.ToString()).Append(", ");
	builder.Append("exploreLvl").Append(":").Append(exploreLvl.ToString()).Append(", ");
	builder.Append("createdMs").Append(":").Append(createdMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

