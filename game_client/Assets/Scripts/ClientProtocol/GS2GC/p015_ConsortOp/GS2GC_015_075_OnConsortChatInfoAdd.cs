using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人对话信息新增
/// </summary>
public class GS2GC_015_075_OnConsortChatInfoAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 对话信息
/// </summary>
private Common.ConsortObj.Consort_ChatInfo chatInfo;


public GS2GC_015_075_OnConsortChatInfoAdd() {
	chatInfo = new Common.ConsortObj.Consort_ChatInfo();
}

public GS2GC_015_075_OnConsortChatInfoAdd(
	Common.ConsortObj.Consort_ChatInfo _chatInfo
) {	chatInfo = _chatInfo;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)75; }

/// <summary>
/// 对话信息
/// </summary>
public Common.ConsortObj.Consort_ChatInfo getChatInfo() { return chatInfo; }
/// <summary>
/// 对话信息
/// </summary>
public void setChatInfo(Common.ConsortObj.Consort_ChatInfo _chatInfo) { chatInfo = _chatInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + chatInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + chatInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _chatInfoCustLen = _buf.getInt();
	int _chatInfoCurPos = _buf.getCurPos();
	chatInfo.ReadUnzipBuf(_buf, _chatInfoCurPos + _chatInfoCustLen);
	_buf.setPosition(_chatInfoCurPos + _chatInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(chatInfo.GetBufSize());
	chatInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)75);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)75);
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
	builder.Append("chatInfo").Append(":").Append(chatInfo == null ? "null" : chatInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

