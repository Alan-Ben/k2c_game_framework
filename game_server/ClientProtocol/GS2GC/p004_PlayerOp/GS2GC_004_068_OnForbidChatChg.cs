using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

/// <summary>
/// 禁言数据变更推送
/// </summary>
public class GS2GC_004_068_OnForbidChatChg : ALBasicProtocolPack._IALProtocolStructure {
private NPCommon.NPCommon_ForbidChatInfo forbidChat;


public GS2GC_004_068_OnForbidChatChg() {
	forbidChat = new NPCommon.NPCommon_ForbidChatInfo();
}

public GS2GC_004_068_OnForbidChatChg(
	NPCommon.NPCommon_ForbidChatInfo _forbidChat
) {	forbidChat = _forbidChat;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)68; }

public NPCommon.NPCommon_ForbidChatInfo getForbidChat() { return forbidChat; }
public void setForbidChat(NPCommon.NPCommon_ForbidChatInfo _forbidChat) { forbidChat = _forbidChat; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _forbidChatCustLen = _buf.getInt();
	int _forbidChatCurPos = _buf.getCurPos();
	forbidChat.ReadUnzipBuf(_buf, _forbidChatCurPos + _forbidChatCustLen);
	_buf.setPosition(_forbidChatCurPos + _forbidChatCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(forbidChat.GetBufSize());
	forbidChat.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)68);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)68);
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
	builder.Append("forbidChat").Append(":").Append(forbidChat == null ? "null" : forbidChat.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

