using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CommObj
{

public class PrivateChatMsg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 发送消息用户标识
/// </summary>
private string senderChatUid;
/// <summary>
/// 接收消息用户标识
/// </summary>
private string receiverChatUid;
/// <summary>
/// 私聊消息唯一id
/// </summary>
private long msgId;
/// <summary>
/// 消息内容
/// </summary>
private Common.CommObj.Common_Msg msg;
/// <summary>
/// 发送时间戳
/// </summary>
private long timeMs;


public PrivateChatMsg() {
	senderChatUid = "";
	receiverChatUid = "";
	msgId = (long)0;
	msg = new Common.CommObj.Common_Msg();
	timeMs = (long)0;
}

public PrivateChatMsg(
	string _senderChatUid
	, string _receiverChatUid
	, long _msgId
	, Common.CommObj.Common_Msg _msg
	, long _timeMs
) {	senderChatUid = _senderChatUid;
	receiverChatUid = _receiverChatUid;
	msgId = _msgId;
	msg = _msg;
	timeMs = _timeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 发送消息用户标识
/// </summary>
public string getSenderChatUid() { return senderChatUid; }
/// <summary>
/// 发送消息用户标识
/// </summary>
public void setSenderChatUid(string _senderChatUid) { senderChatUid = _senderChatUid; }
/// <summary>
/// 接收消息用户标识
/// </summary>
public string getReceiverChatUid() { return receiverChatUid; }
/// <summary>
/// 接收消息用户标识
/// </summary>
public void setReceiverChatUid(string _receiverChatUid) { receiverChatUid = _receiverChatUid; }
/// <summary>
/// 私聊消息唯一id
/// </summary>
public long getMsgId() { return msgId; }
/// <summary>
/// 私聊消息唯一id
/// </summary>
public void setMsgId(long _msgId) { msgId = _msgId; }
/// <summary>
/// 消息内容
/// </summary>
public Common.CommObj.Common_Msg getMsg() { return msg; }
/// <summary>
/// 消息内容
/// </summary>
public void setMsg(Common.CommObj.Common_Msg _msg) { msg = _msg; }
/// <summary>
/// 发送时间戳
/// </summary>
public long getTimeMs() { return timeMs; }
/// <summary>
/// 发送时间戳
/// </summary>
public void setTimeMs(long _timeMs) { timeMs = _timeMs; }


public int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderChatUid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(receiverChatUid);
	_size += 4 + msg.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderChatUid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(receiverChatUid);
	_size += 4 + msg.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderChatUid = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	receiverChatUid = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msgId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _msgCustLen = _buf.getInt();
	int _msgCurPos = _buf.getCurPos();
	msg.ReadUnzipBuf(_buf, _msgCurPos + _msgCustLen);
	_buf.setPosition(_msgCurPos + _msgCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(senderChatUid);
	_buf.putString(receiverChatUid);
	_buf.putLong(msgId);
	_buf.putInt(msg.GetBufSize());
	msg.PutUnzipBuf(_buf);
	_buf.putLong(timeMs);
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
	builder.Append("senderChatUid").Append(":").Append(senderChatUid.ToString()).Append(", ");
	builder.Append("receiverChatUid").Append(":").Append(receiverChatUid.ToString()).Append(", ");
	builder.Append("msgId").Append(":").Append(msgId.ToString()).Append(", ");
	builder.Append("msg").Append(":").Append(msg == null ? "null" : msg.ToString()).Append(", ");
	builder.Append("timeMs").Append(":").Append(timeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

