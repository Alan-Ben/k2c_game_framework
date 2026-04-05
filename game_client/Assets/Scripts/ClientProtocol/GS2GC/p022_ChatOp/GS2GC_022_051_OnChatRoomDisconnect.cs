using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p022_ChatOp
{

/// <summary>
/// 聊天房间断开
/// </summary>
public class GS2GC_022_051_OnChatRoomDisconnect : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 聊天房间ID
/// </summary>
private long roomId;


public GS2GC_022_051_OnChatRoomDisconnect() {
	roomId = (long)0;
}

public GS2GC_022_051_OnChatRoomDisconnect(
	long _roomId
) {	roomId = _roomId;
}

public byte getMainOrder() { return (byte)22; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 聊天房间ID
/// </summary>
public long getRoomId() { return roomId; }
/// <summary>
/// 聊天房间ID
/// </summary>
public void setRoomId(long _roomId) { roomId = _roomId; }


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
	roomId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(roomId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)22);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)22);
	_recBuf.put((byte)51);
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
	builder.Append("roomId").Append(":").Append(roomId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

