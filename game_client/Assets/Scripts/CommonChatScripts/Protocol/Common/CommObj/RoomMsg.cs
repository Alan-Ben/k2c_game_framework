using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CommObj
{

public class RoomMsg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 用户唯一标识
/// </summary>
private string chatUid;
/// <summary>
/// 聊天房间唯一id
/// </summary>
private long roomId;
/// <summary>
/// 房间消息唯一id
/// </summary>
private long msgId;
/// <summary>
/// 发送消息时的时间戳
/// </summary>
private long timeMs;
/// <summary>
/// 消息内容
/// </summary>
private Common.CommObj.Common_Msg msg;


public RoomMsg() {
	chatUid = "";
	roomId = (long)0;
	msgId = (long)0;
	timeMs = (long)0;
	msg = new Common.CommObj.Common_Msg();
}

public RoomMsg(
	string _chatUid
	, long _roomId
	, long _msgId
	, long _timeMs
	, Common.CommObj.Common_Msg _msg
) {	chatUid = _chatUid;
	roomId = _roomId;
	msgId = _msgId;
	timeMs = _timeMs;
	msg = _msg;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 用户唯一标识
/// </summary>
public string getChatUid() { return chatUid; }
/// <summary>
/// 用户唯一标识
/// </summary>
public void setChatUid(string _chatUid) { chatUid = _chatUid; }
/// <summary>
/// 聊天房间唯一id
/// </summary>
public long getRoomId() { return roomId; }
/// <summary>
/// 聊天房间唯一id
/// </summary>
public void setRoomId(long _roomId) { roomId = _roomId; }
/// <summary>
/// 房间消息唯一id
/// </summary>
public long getMsgId() { return msgId; }
/// <summary>
/// 房间消息唯一id
/// </summary>
public void setMsgId(long _msgId) { msgId = _msgId; }
/// <summary>
/// 发送消息时的时间戳
/// </summary>
public long getTimeMs() { return timeMs; }
/// <summary>
/// 发送消息时的时间戳
/// </summary>
public void setTimeMs(long _timeMs) { timeMs = _timeMs; }
/// <summary>
/// 消息内容
/// </summary>
public Common.CommObj.Common_Msg getMsg() { return msg; }
/// <summary>
/// 消息内容
/// </summary>
public void setMsg(Common.CommObj.Common_Msg _msg) { msg = _msg; }


public int GetBufSize() {
	int _size = 24;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chatUid);
	_size += 4 + msg.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chatUid);
	_size += 4 + msg.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chatUid = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msgId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _msgCustLen = _buf.getInt();
	int _msgCurPos = _buf.getCurPos();
	msg.ReadUnzipBuf(_buf, _msgCurPos + _msgCustLen);
	_buf.setPosition(_msgCurPos + _msgCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(chatUid);
	_buf.putLong(roomId);
	_buf.putLong(msgId);
	_buf.putLong(timeMs);
	_buf.putInt(msg.GetBufSize());
	msg.PutUnzipBuf(_buf);
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
	builder.Append("chatUid").Append(":").Append(chatUid.ToString()).Append(", ");
	builder.Append("roomId").Append(":").Append(roomId.ToString()).Append(", ");
	builder.Append("msgId").Append(":").Append(msgId.ToString()).Append(", ");
	builder.Append("timeMs").Append(":").Append(timeMs.ToString()).Append(", ");
	builder.Append("msg").Append(":").Append(msg == null ? "null" : msg.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

