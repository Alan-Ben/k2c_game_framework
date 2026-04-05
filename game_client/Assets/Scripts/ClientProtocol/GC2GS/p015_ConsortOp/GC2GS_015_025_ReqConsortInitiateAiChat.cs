using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p015_ConsortOp
{

/// <summary>
/// 家人-Ai对话
/// </summary>
public class GC2GS_015_025_ReqConsortInitiateAiChat : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人ID
/// </summary>
private long consortId;
/// <summary>
/// 消息列表
/// </summary>
private List<Common.Common_AiChatMessage> msgList;
/// <summary>
/// 客户端数据ID
/// </summary>
private long clientDataId;


public GC2GS_015_025_ReqConsortInitiateAiChat() {
	consortId = (long)0;
	msgList = new List<Common.Common_AiChatMessage>();
	clientDataId = (long)0;
}

public GC2GS_015_025_ReqConsortInitiateAiChat(
	long _consortId
	, List<Common.Common_AiChatMessage> _msgList
	, long _clientDataId
) {	consortId = _consortId;
	msgList = _msgList;
	clientDataId = _clientDataId;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)25; }

/// <summary>
/// 家人ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 家人ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 消息列表
/// </summary>
public List<Common.Common_AiChatMessage> getMsgList() { return msgList; }
/// <summary>
/// 消息列表
/// </summary>
public void addMsgList(Common.Common_AiChatMessage _msgList) { msgList.Add(_msgList); }
/// <summary>
/// 客户端数据ID
/// </summary>
public long getClientDataId() { return clientDataId; }
/// <summary>
/// 客户端数据ID
/// </summary>
public void setClientDataId(long _clientDataId) { clientDataId = _clientDataId; }


public int GetBufSize() {
	int _size = 16;
	_size += 2;
for(int _i = 0; _i < msgList.Count; _i++) {
	_size += 4 + msgList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
for(int _i = 0; _i < msgList.Count; _i++) {
	_size += 4 + msgList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _msgListCount = _buf.getShort();
	for(int _i = 0; _i < _msgListCount; _i++) { 
		Common.Common_AiChatMessage _msgList = new Common.Common_AiChatMessage();
		int __msgListCustLen = _buf.getInt();
	int __msgListCurPos = _buf.getCurPos();
	_msgList.ReadUnzipBuf(_buf, __msgListCurPos + __msgListCustLen);
	_buf.setPosition(__msgListCurPos + __msgListCustLen);

		msgList.Add(_msgList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	clientDataId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putShort((short)msgList.Count);
	for(int _i = 0; _i < msgList.Count; _i++) { 
		_buf.putInt(msgList[_i].GetBufSize());
	msgList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(clientDataId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)25);
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
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("msgList").Append(":").Append(msgList.ToString()).Append(", ");
	builder.Append("clientDataId").Append(":").Append(clientDataId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

