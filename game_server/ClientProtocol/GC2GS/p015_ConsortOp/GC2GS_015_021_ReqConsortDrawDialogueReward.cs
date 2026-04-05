using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p015_ConsortOp
{

/// <summary>
/// 家人-领取对话奖励
/// </summary>
public class GC2GS_015_021_ReqConsortDrawDialogueReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人ID
/// </summary>
private long consortId;
/// <summary>
/// 对话ID
/// </summary>
private long dialogueId;


public GC2GS_015_021_ReqConsortDrawDialogueReward() {
	consortId = (long)0;
	dialogueId = (long)0;
}

public GC2GS_015_021_ReqConsortDrawDialogueReward(
	long _consortId
	, long _dialogueId
) {	consortId = _consortId;
	dialogueId = _dialogueId;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)21; }

/// <summary>
/// 家人ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 家人ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 对话ID
/// </summary>
public long getDialogueId() { return dialogueId; }
/// <summary>
/// 对话ID
/// </summary>
public void setDialogueId(long _dialogueId) { dialogueId = _dialogueId; }


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
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dialogueId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(dialogueId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)21);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

