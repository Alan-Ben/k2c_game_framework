using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChapterObj
{

/// <summary>
/// 关卡事件信息
/// </summary>
public class Chapter_EventInfo : ALBasicProtocolPack._IALProtocolStructure {
private long eventId;
private byte[] extraData;


public Chapter_EventInfo() {
	eventId = (long)0;
	extraData = null;
}

public Chapter_EventInfo(
	long _eventId
	, byte[] _extraData
) {	eventId = _eventId;
	extraData = _extraData;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getEventId() { return eventId; }
public void setEventId(long _eventId) { eventId = _eventId; }
public byte[] getExtraData() { return extraData; }

public void setExtraData(byte[] _extraData) { extraData = _extraData; }



public int GetBufSize() {
	int _size = 8;
	_size += 4 + (extraData == null ? 0 : extraData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (extraData == null ? 0 : extraData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extraData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
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
	builder.Append("eventId").Append(":").Append(eventId.ToString()).Append(", ");
	builder.Append("extraData").Append(":").Append(extraData == null ? "null" : extraData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

