using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p002_MsgOp
{

/// <summary>
/// 获取对应用户的未读消息
/// </summary>
public class GC2GS_002_005_ReqUnreadPrivateMsgList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 回调序列号
/// </summary>
private long callBackSerial;
/// <summary>
/// 发送方用户标识
/// </summary>
private string senderChatUid;


public GC2GS_002_005_ReqUnreadPrivateMsgList() {
	callBackSerial = (long)0;
	senderChatUid = "";
}

public GC2GS_002_005_ReqUnreadPrivateMsgList(
	long _callBackSerial
	, string _senderChatUid
) {	callBackSerial = _callBackSerial;
	senderChatUid = _senderChatUid;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)5; }

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


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderChatUid);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderChatUid);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	callBackSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	senderChatUid = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(callBackSerial);
	_buf.putString(senderChatUid);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)5);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

