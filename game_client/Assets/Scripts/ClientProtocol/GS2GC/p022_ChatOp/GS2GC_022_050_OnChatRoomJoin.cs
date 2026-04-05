using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p022_ChatOp
{

/// <summary>
/// 加入新房间
/// </summary>
public class GS2GC_022_050_OnChatRoomJoin : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 聊天房间类型
/// </summary>
private int roomType;
/// <summary>
/// 聊天房间ID
/// </summary>
private long roomId;
/// <summary>
/// 额外参数
/// </summary>
private long extId;


public GS2GC_022_050_OnChatRoomJoin() {
	roomType = 0;
	roomId = (long)0;
	extId = (long)0;
}

public GS2GC_022_050_OnChatRoomJoin(
	int _roomType
	, long _roomId
	, long _extId
) {	roomType = _roomType;
	roomId = _roomId;
	extId = _extId;
}

public byte getMainOrder() { return (byte)22; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 聊天房间类型
/// </summary>
public int getRoomType() { return roomType; }
/// <summary>
/// 聊天房间类型
/// </summary>
public void setRoomType(int _roomType) { roomType = _roomType; }
/// <summary>
/// 聊天房间ID
/// </summary>
public long getRoomId() { return roomId; }
/// <summary>
/// 聊天房间ID
/// </summary>
public void setRoomId(long _roomId) { roomId = _roomId; }
/// <summary>
/// 额外参数
/// </summary>
public long getExtId() { return extId; }
/// <summary>
/// 额外参数
/// </summary>
public void setExtId(long _extId) { extId = _extId; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roomType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(roomType);
	_buf.putLong(roomId);
	_buf.putLong(extId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)22);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)22);
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
	builder.Append("roomType").Append(":").Append(roomType.ToString()).Append(", ");
	builder.Append("roomId").Append(":").Append(roomId.ToString()).Append(", ");
	builder.Append("extId").Append(":").Append(extId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

