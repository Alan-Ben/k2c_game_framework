using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_059_OnGuildEventAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 事件信息
/// </summary>
private Common.GuildObj.Guild_EventInfo eventInfo;


public GS2GC_032_059_OnGuildEventAdd() {
	eventInfo = new Common.GuildObj.Guild_EventInfo();
}

public GS2GC_032_059_OnGuildEventAdd(
	Common.GuildObj.Guild_EventInfo _eventInfo
) {	eventInfo = _eventInfo;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)59; }

/// <summary>
/// 事件信息
/// </summary>
public Common.GuildObj.Guild_EventInfo getEventInfo() { return eventInfo; }
/// <summary>
/// 事件信息
/// </summary>
public void setEventInfo(Common.GuildObj.Guild_EventInfo _eventInfo) { eventInfo = _eventInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + eventInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + eventInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _eventInfoCustLen = _buf.getInt();
	int _eventInfoCurPos = _buf.getCurPos();
	eventInfo.ReadUnzipBuf(_buf, _eventInfoCurPos + _eventInfoCustLen);
	_buf.setPosition(_eventInfoCurPos + _eventInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(eventInfo.GetBufSize());
	eventInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)59);
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
	builder.Append("eventInfo").Append(":").Append(eventInfo == null ? "null" : eventInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

