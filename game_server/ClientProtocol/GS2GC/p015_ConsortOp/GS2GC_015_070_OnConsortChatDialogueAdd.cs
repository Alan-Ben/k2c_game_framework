using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 新增家人对话推送
/// </summary>
public class GS2GC_015_070_OnConsortChatDialogueAdd : ALBasicProtocolPack._IALProtocolStructure {
private long consortId;
/// <summary>
/// 对话ID
/// </summary>
private long dialogueId;
/// <summary>
/// 触发时间戳
/// </summary>
private long triggerTimeMs;


public GS2GC_015_070_OnConsortChatDialogueAdd() {
	consortId = (long)0;
	dialogueId = (long)0;
	triggerTimeMs = (long)0;
}

public GS2GC_015_070_OnConsortChatDialogueAdd(
	long _consortId
	, long _dialogueId
	, long _triggerTimeMs
) {	consortId = _consortId;
	dialogueId = _dialogueId;
	triggerTimeMs = _triggerTimeMs;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)70; }

public long getConsortId() { return consortId; }
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 对话ID
/// </summary>
public long getDialogueId() { return dialogueId; }
/// <summary>
/// 对话ID
/// </summary>
public void setDialogueId(long _dialogueId) { dialogueId = _dialogueId; }
/// <summary>
/// 触发时间戳
/// </summary>
public long getTriggerTimeMs() { return triggerTimeMs; }
/// <summary>
/// 触发时间戳
/// </summary>
public void setTriggerTimeMs(long _triggerTimeMs) { triggerTimeMs = _triggerTimeMs; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dialogueId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	triggerTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(dialogueId);
	_buf.putLong(triggerTimeMs);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)70);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)70);
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
	builder.Append("dialogueId").Append(":").Append(dialogueId.ToString()).Append(", ");
	builder.Append("triggerTimeMs").Append(":").Append(triggerTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

