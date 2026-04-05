using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p040_MarsPeopleOp
{

/// <summary>
/// 事件触发
/// </summary>
public class GS2GC_040_057_OnMarsEventTrigger : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 火星居民事件
/// </summary>
private Common.MarsObj.Mars_Event marsEvent;


public GS2GC_040_057_OnMarsEventTrigger() {
	marsEvent = new Common.MarsObj.Mars_Event();
}

public GS2GC_040_057_OnMarsEventTrigger(
	Common.MarsObj.Mars_Event _marsEvent
) {	marsEvent = _marsEvent;
}

public byte getMainOrder() { return (byte)40; }

public byte getSubOrder() { return (byte)57; }

/// <summary>
/// 火星居民事件
/// </summary>
public Common.MarsObj.Mars_Event getMarsEvent() { return marsEvent; }
/// <summary>
/// 火星居民事件
/// </summary>
public void setMarsEvent(Common.MarsObj.Mars_Event _marsEvent) { marsEvent = _marsEvent; }


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
	int _marsEventCustLen = _buf.getInt();
	int _marsEventCurPos = _buf.getCurPos();
	marsEvent.ReadUnzipBuf(_buf, _marsEventCurPos + _marsEventCustLen);
	_buf.setPosition(_marsEventCurPos + _marsEventCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(marsEvent.GetBufSize());
	marsEvent.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)57);
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
	builder.Append("marsEvent").Append(":").Append(marsEvent == null ? "null" : marsEvent.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

