using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p022_ChatOp
{

/// <summary>
/// 发送聊天房间消息-旧版协议，后续使用新版协议22-6
/// </summary>
public class GC2GS_022_004_ReqPlayerSendRoomMsg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 聊天房间ID
/// </summary>
private long roomId;
/// <summary>
/// 消息类型,解释消息内容
/// </summary>
private int msgType;
/// <summary>
/// 发送者信息
/// </summary>
private byte[] gameUser;
/// <summary>
/// 消息内容
/// </summary>
private byte[] gameContent;


public GC2GS_022_004_ReqPlayerSendRoomMsg() {
	roomId = (long)0;
	msgType = 0;
	gameUser = null;
	gameContent = null;
}

public GC2GS_022_004_ReqPlayerSendRoomMsg(
	long _roomId
	, int _msgType
	, byte[] _gameUser
	, byte[] _gameContent
) {	roomId = _roomId;
	msgType = _msgType;
	gameUser = _gameUser;
	gameContent = _gameContent;
}

public byte getMainOrder() { return (byte)22; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 聊天房间ID
/// </summary>
public long getRoomId() { return roomId; }
/// <summary>
/// 聊天房间ID
/// </summary>
public void setRoomId(long _roomId) { roomId = _roomId; }
/// <summary>
/// 消息类型,解释消息内容
/// </summary>
public int getMsgType() { return msgType; }
/// <summary>
/// 消息类型,解释消息内容
/// </summary>
public void setMsgType(int _msgType) { msgType = _msgType; }
/// <summary>
/// 发送者信息
/// </summary>
public byte[] getGameUser() { return gameUser; }

/// <summary>
/// 发送者信息
/// </summary>
public void setGameUser(byte[] _gameUser) { gameUser = _gameUser; }

/// <summary>
/// 消息内容
/// </summary>
public byte[] getGameContent() { return gameContent; }

/// <summary>
/// 消息内容
/// </summary>
public void setGameContent(byte[] _gameContent) { gameContent = _gameContent; }



public int GetBufSize() {
	int _size = 12;
	_size += 4 + (gameUser == null ? 0 : gameUser.Length);
	_size += 4 + (gameContent == null ? 0 : gameContent.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (gameUser == null ? 0 : gameUser.Length);
	_size += 4 + (gameContent == null ? 0 : gameContent.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msgType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gameUser = _buf.getByteBuffer();

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gameContent = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(roomId);
	_buf.putInt(msgType);
	_buf.putByteBuffer(gameUser);

	_buf.putByteBuffer(gameContent);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)22);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)22);
	_recBuf.put((byte)4);
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
	builder.Append("msgType").Append(":").Append(msgType.ToString()).Append(", ");
	builder.Append("gameUser").Append(":").Append(gameUser == null ? "null" : gameUser.ToString()).Append(", ");
	builder.Append("gameContent").Append(":").Append(gameContent == null ? "null" : gameContent.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

