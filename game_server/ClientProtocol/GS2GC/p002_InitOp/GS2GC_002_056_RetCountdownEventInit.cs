using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_056_RetCountdownEventInit : ALBasicProtocolPack._IALProtocolStructure {
private Common.CountdownEventObj.CountdownEvent_Info eventInfo;


public GS2GC_002_056_RetCountdownEventInit() {
	eventInfo = new Common.CountdownEventObj.CountdownEvent_Info();
}

public GS2GC_002_056_RetCountdownEventInit(
	Common.CountdownEventObj.CountdownEvent_Info _eventInfo
) {	eventInfo = _eventInfo;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)56; }

public Common.CountdownEventObj.CountdownEvent_Info getEventInfo() { return eventInfo; }
public void setEventInfo(Common.CountdownEventObj.CountdownEvent_Info _eventInfo) { eventInfo = _eventInfo; }


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
	_buf.put((byte)2);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)56);
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

