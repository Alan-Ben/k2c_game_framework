using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p015_ConsortOp
{

/// <summary>
/// 家人-朋友圈AI对话
/// </summary>
public class GC2GS_015_024_ReqConsortAiChatMoment : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例id
/// </summary>
private long instanceId;
/// <summary>
/// 消息列表
/// </summary>
private List<Common.Common_AiChatMessage> msgList;
/// <summary>
/// 家人ID
/// </summary>
private long consortId;
/// <summary>
/// 是否是朋友圈内容
/// </summary>
private bool isMomentContent;


public GC2GS_015_024_ReqConsortAiChatMoment() {
	instanceId = (long)0;
	msgList = new List<Common.Common_AiChatMessage>();
	consortId = (long)0;
	isMomentContent = false;
}

public GC2GS_015_024_ReqConsortAiChatMoment(
	long _instanceId
	, List<Common.Common_AiChatMessage> _msgList
	, long _consortId
	, bool _isMomentContent
) {	instanceId = _instanceId;
	msgList = _msgList;
	consortId = _consortId;
	isMomentContent = _isMomentContent;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)24; }

/// <summary>
/// 实例id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 实例id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 消息列表
/// </summary>
public List<Common.Common_AiChatMessage> getMsgList() { return msgList; }
/// <summary>
/// 消息列表
/// </summary>
public void addMsgList(Common.Common_AiChatMessage _msgList) { msgList.Add(_msgList); }
/// <summary>
/// 家人ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 家人ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 是否是朋友圈内容
/// </summary>
public bool getIsMomentContent() { return isMomentContent; }
/// <summary>
/// 是否是朋友圈内容
/// </summary>
public void setIsMomentContent(bool _isMomentContent) { isMomentContent = _isMomentContent; }


public int GetBufSize() {
	int _size = 17;
	_size += 2;
for(int _i = 0; _i < msgList.Count; _i++) {
	_size += 4 + msgList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;
	_size += 2;
for(int _i = 0; _i < msgList.Count; _i++) {
	_size += 4 + msgList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
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
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isMomentContent = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putShort((short)msgList.Count);
	for(int _i = 0; _i < msgList.Count; _i++) { 
		_buf.putInt(msgList[_i].GetBufSize());
	msgList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(consortId);
	_buf.put(isMomentContent?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)24);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)24);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("msgList").Append(":").Append(msgList.ToString()).Append(", ");
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("isMomentContent").Append(":").Append(isMomentContent.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

