using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_MsgOp
{

/// <summary>
/// 推送私聊消息
/// </summary>
public class GS2GC_002_051_OnReceivePrivateMsg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 消息内容
/// </summary>
private Common.CommObj.PrivateChatMsg msg;


public GS2GC_002_051_OnReceivePrivateMsg() {
	msg = new Common.CommObj.PrivateChatMsg();
}

public GS2GC_002_051_OnReceivePrivateMsg(
	Common.CommObj.PrivateChatMsg _msg
) {	msg = _msg;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)51; }

/// <summary>
/// 消息内容
/// </summary>
public Common.CommObj.PrivateChatMsg getMsg() { return msg; }
/// <summary>
/// 消息内容
/// </summary>
public void setMsg(Common.CommObj.PrivateChatMsg _msg) { msg = _msg; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + msg.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + msg.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _msgCustLen = _buf.getInt();
	int _msgCurPos = _buf.getCurPos();
	msg.ReadUnzipBuf(_buf, _msgCurPos + _msgCustLen);
	_buf.setPosition(_msgCurPos + _msgCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(msg.GetBufSize());
	msg.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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
	builder.Append("msg").Append(":").Append(msg == null ? "null" : msg.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

