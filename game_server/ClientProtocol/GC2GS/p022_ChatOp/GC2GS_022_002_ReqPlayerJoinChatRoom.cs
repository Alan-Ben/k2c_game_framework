using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p022_ChatOp
{

/// <summary>
/// 请求加入聊天房间
/// </summary>
public class GC2GS_022_002_ReqPlayerJoinChatRoom : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 聊天房间类型
/// </summary>
private int roomType;
/// <summary>
/// 额外参数，活动组队聊天-活动实例ID，其他-0
/// </summary>
private long extId;


public GC2GS_022_002_ReqPlayerJoinChatRoom() {
	roomType = 0;
	extId = (long)0;
}

public GC2GS_022_002_ReqPlayerJoinChatRoom(
	int _roomType
	, long _extId
) {	roomType = _roomType;
	extId = _extId;
}

public byte getMainOrder() { return (byte)22; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 聊天房间类型
/// </summary>
public int getRoomType() { return roomType; }
/// <summary>
/// 聊天房间类型
/// </summary>
public void setRoomType(int _roomType) { roomType = _roomType; }
/// <summary>
/// 额外参数，活动组队聊天-活动实例ID，其他-0
/// </summary>
public long getExtId() { return extId; }
/// <summary>
/// 额外参数，活动组队聊天-活动实例ID，其他-0
/// </summary>
public void setExtId(long _extId) { extId = _extId; }


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
	roomType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	extId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(roomType);
	_buf.putLong(extId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)22);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)22);
	_recBuf.put((byte)2);
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
	builder.Append("extId").Append(":").Append(extId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

