using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p002_MsgOp
{

/// <summary>
/// 确认私聊消息，确认后服务端移除数据
/// </summary>
public class GC2GS_002_006_ReqConfirmPrivateMsg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 回调序列号
/// </summary>
private long callBackSerial;
/// <summary>
/// 发送方用户标识
/// </summary>
private string senderChatUid;
/// <summary>
/// 已确认的最大消息记录
/// </summary>
private long confirmMsgId;


public GC2GS_002_006_ReqConfirmPrivateMsg() {
	callBackSerial = (long)0;
	senderChatUid = "";
	confirmMsgId = (long)0;
}

public GC2GS_002_006_ReqConfirmPrivateMsg(
	long _callBackSerial
	, string _senderChatUid
	, long _confirmMsgId
) {	callBackSerial = _callBackSerial;
	senderChatUid = _senderChatUid;
	confirmMsgId = _confirmMsgId;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 回调序列号
/// </summary>
public long getCallBackSerial() { return callBackSerial; }
/// <summary>
/// 回调序列号
/// </summary>
public void setCallBackSerial(long _callBackSerial) { callBackSerial = _callBackSerial; }
/// <summary>
/// 发送方用户标识
/// </summary>
public string getSenderChatUid() { return senderChatUid; }
/// <summary>
/// 发送方用户标识
/// </summary>
public void setSenderChatUid(string _senderChatUid) { senderChatUid = _senderChatUid; }
/// <summary>
/// 已确认的最大消息记录
/// </summary>
public long getConfirmMsgId() { return confirmMsgId; }
/// <summary>
/// 已确认的最大消息记录
/// </summary>
public void setConfirmMsgId(long _confirmMsgId) { confirmMsgId = _confirmMsgId; }


public int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderChatUid);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderChatUid);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	callBackSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderChatUid = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	confirmMsgId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(callBackSerial);
	_buf.putString(senderChatUid);
	_buf.putLong(confirmMsgId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)6);
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
	builder.Append("senderChatUid").Append(":").Append(senderChatUid.ToString()).Append(", ");
	builder.Append("confirmMsgId").Append(":").Append(confirmMsgId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

