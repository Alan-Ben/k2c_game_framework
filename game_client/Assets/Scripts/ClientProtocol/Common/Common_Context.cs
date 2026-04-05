using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

/// <summary>
/// 通用上下文
/// </summary>
public class Common_Context : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 事件id
/// </summary>
private int eventId;
/// <summary>
/// 上下文唯一id
/// </summary>
private long guid;


public Common_Context() {
	eventId = 0;
	guid = (long)0;
}

public Common_Context(
	int _eventId
	, long _guid
) {	eventId = _eventId;
	guid = _guid;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 事件id
/// </summary>
public int getEventId() { return eventId; }
/// <summary>
/// 事件id
/// </summary>
public void setEventId(int _eventId) { eventId = _eventId; }
/// <summary>
/// 上下文唯一id
/// </summary>
public long getGuid() { return guid; }
/// <summary>
/// 上下文唯一id
/// </summary>
public void setGuid(long _guid) { guid = _guid; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guid = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(eventId);
	_buf.putLong(guid);
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
	builder.Append("eventId").Append(":").Append(eventId.ToString()).Append(", ");
	builder.Append("guid").Append(":").Append(guid.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

