using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 批准弹劾事件
/// </summary>
public class GC2GS_032_026_ReqApproveLeaderImpeachEvent : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 事件数据ID
/// </summary>
private long eventDbId;


public GC2GS_032_026_ReqApproveLeaderImpeachEvent() {
	eventDbId = (long)0;
}

public GC2GS_032_026_ReqApproveLeaderImpeachEvent(
	long _eventDbId
) {	eventDbId = _eventDbId;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)26; }

/// <summary>
/// 事件数据ID
/// </summary>
public long getEventDbId() { return eventDbId; }
/// <summary>
/// 事件数据ID
/// </summary>
public void setEventDbId(long _eventDbId) { eventDbId = _eventDbId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	eventDbId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(eventDbId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)26);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)26);
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
	builder.Append("eventDbId").Append(":").Append(eventDbId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

