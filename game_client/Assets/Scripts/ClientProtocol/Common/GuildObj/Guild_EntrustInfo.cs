using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟杂物委托信息
/// </summary>
public class Guild_EntrustInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 序列号
/// </summary>
private long serial;
/// <summary>
/// 配表id
/// </summary>
private long refId;
/// <summary>
/// 事件id
/// </summary>
private long eventId;
/// <summary>
/// 当前进度
/// </summary>
private int point;


public Guild_EntrustInfo() {
	serial = (long)0;
	refId = (long)0;
	eventId = (long)0;
	point = 0;
}

public Guild_EntrustInfo(
	long _serial
	, long _refId
	, long _eventId
	, int _point
) {	serial = _serial;
	refId = _refId;
	eventId = _eventId;
	point = _point;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 序列号
/// </summary>
public long getSerial() { return serial; }
/// <summary>
/// 序列号
/// </summary>
public void setSerial(long _serial) { serial = _serial; }
/// <summary>
/// 配表id
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// 配表id
/// </summary>
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 事件id
/// </summary>
public long getEventId() { return eventId; }
/// <summary>
/// 事件id
/// </summary>
public void setEventId(long _eventId) { eventId = _eventId; }
/// <summary>
/// 当前进度
/// </summary>
public int getPoint() { return point; }
/// <summary>
/// 当前进度
/// </summary>
public void setPoint(int _point) { point = _point; }


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
	serial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	point = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(serial);
	_buf.putLong(refId);
	_buf.putLong(eventId);
	_buf.putInt(point);
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
	builder.Append("serial").Append(":").Append(serial.ToString()).Append(", ");
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("eventId").Append(":").Append(eventId.ToString()).Append(", ");
	builder.Append("point").Append(":").Append(point.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

