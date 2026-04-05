using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p008_TravelOp
{

/// <summary>
/// 新增事件推送
/// </summary>
public class GS2GC_008_050_OnEventAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private Common.TravelObj.Travel_Event eventInfo;


public GS2GC_008_050_OnEventAdd() {
	eventInfo = new Common.TravelObj.Travel_Event();
}

public GS2GC_008_050_OnEventAdd(
	Common.TravelObj.Travel_Event _eventInfo
) {	eventInfo = _eventInfo;
}

public byte getMainOrder() { return (byte)8; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 空
/// </summary>
public Common.TravelObj.Travel_Event getEventInfo() { return eventInfo; }
/// <summary>
/// 空
/// </summary>
public void setEventInfo(Common.TravelObj.Travel_Event _eventInfo) { eventInfo = _eventInfo; }


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
	_buf.put((byte)8);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)50);
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

