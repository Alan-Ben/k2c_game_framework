using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p002_MsgOp
{

/// <summary>
/// 获得房间历史记录
/// </summary>
public class GC2GS_002_001_ReqRoomHistory : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 回调序列号
/// </summary>
private long callBackSerial;
/// <summary>
/// 房间id
/// </summary>
private long roomId;
/// <summary>
/// 最后消息id
/// </summary>
private long lastMsgId;
/// <summary>
/// 客户端展示数量，超过服务端上限，则以服务端上限数量为准
/// </summary>
private int showCount;


public GC2GS_002_001_ReqRoomHistory() {
	callBackSerial = (long)0;
	roomId = (long)0;
	lastMsgId = (long)0;
	showCount = 0;
}

public GC2GS_002_001_ReqRoomHistory(
	long _callBackSerial
	, long _roomId
	, long _lastMsgId
	, int _showCount
) {	callBackSerial = _callBackSerial;
	roomId = _roomId;
	lastMsgId = _lastMsgId;
	showCount = _showCount;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 回调序列号
/// </summary>
public long getCallBackSerial() { return callBackSerial; }
/// <summary>
/// 回调序列号
/// </summary>
public void setCallBackSerial(long _callBackSerial) { callBackSerial = _callBackSerial; }
/// <summary>
/// 房间id
/// </summary>
public long getRoomId() { return roomId; }
/// <summary>
/// 房间id
/// </summary>
public void setRoomId(long _roomId) { roomId = _roomId; }
/// <summary>
/// 最后消息id
/// </summary>
public long getLastMsgId() { return lastMsgId; }
/// <summary>
/// 最后消息id
/// </summary>
public void setLastMsgId(long _lastMsgId) { lastMsgId = _lastMsgId; }
/// <summary>
/// 客户端展示数量，超过服务端上限，则以服务端上限数量为准
/// </summary>
public int getShowCount() { return showCount; }
/// <summary>
/// 客户端展示数量，超过服务端上限，则以服务端上限数量为准
/// </summary>
public void setShowCount(int _showCount) { showCount = _showCount; }


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
	callBackSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastMsgId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	showCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(callBackSerial);
	_buf.putLong(roomId);
	_buf.putLong(lastMsgId);
	_buf.putInt(showCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)1);
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
	builder.Append("callBackSerial").Append(":").Append(callBackSerial.ToString()).Append(", ");
	builder.Append("roomId").Append(":").Append(roomId.ToString()).Append(", ");
	builder.Append("lastMsgId").Append(":").Append(lastMsgId.ToString()).Append(", ");
	builder.Append("showCount").Append(":").Append(showCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

