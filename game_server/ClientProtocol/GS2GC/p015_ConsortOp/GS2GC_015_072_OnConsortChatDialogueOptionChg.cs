using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人对话选择变更推送
/// </summary>
public class GS2GC_015_072_OnConsortChatDialogueOptionChg : ALBasicProtocolPack._IALProtocolStructure {
private long consortId;
/// <summary>
/// 对话ID
/// </summary>
private long dialogueId;
/// <summary>
/// 对话选项信息
/// </summary>
private Common.ConsortObj.Consort_DialogueOption optionInfo;


public GS2GC_015_072_OnConsortChatDialogueOptionChg() {
	consortId = (long)0;
	dialogueId = (long)0;
	optionInfo = new Common.ConsortObj.Consort_DialogueOption();
}

public GS2GC_015_072_OnConsortChatDialogueOptionChg(
	long _consortId
	, long _dialogueId
	, Common.ConsortObj.Consort_DialogueOption _optionInfo
) {	consortId = _consortId;
	dialogueId = _dialogueId;
	optionInfo = _optionInfo;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)72; }

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
/// 对话选项信息
/// </summary>
public Common.ConsortObj.Consort_DialogueOption getOptionInfo() { return optionInfo; }
/// <summary>
/// 对话选项信息
/// </summary>
public void setOptionInfo(Common.ConsortObj.Consort_DialogueOption _optionInfo) { optionInfo = _optionInfo; }


public int GetBufSize() {
	int _size = 36;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dialogueId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _optionInfoCustLen = _buf.getInt();
	int _optionInfoCurPos = _buf.getCurPos();
	optionInfo.ReadUnzipBuf(_buf, _optionInfoCurPos + _optionInfoCustLen);
	_buf.setPosition(_optionInfoCurPos + _optionInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(dialogueId);
	_buf.putInt(optionInfo.GetBufSize());
	optionInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)72);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)72);
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
	builder.Append("optionInfo").Append(":").Append(optionInfo == null ? "null" : optionInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

